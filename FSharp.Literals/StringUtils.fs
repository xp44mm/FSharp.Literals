module FSharp.Literals.StringUtils

open System
open System.Text.RegularExpressions
open System.Globalization

let private nonprintablePairs =
    [
        '\\'    , "\\\\"
        '\t'    , "\\t"
        '\n'    , "\\n"
        '\r'    , "\\r"
        '\f'    , "\\f"
        '\b'    , "\\b"
        '\u0000', "\\u0000" // Null char
        '\u0085', "\\u0085" // Next Line
        '\u2028', "\\u2028" // Line Separator
        '\u2029', "\\u2029" // Paragraph Separator
    ]

let private charEscapings =
    ('\'' , @"\'") :: nonprintablePairs
    |> Map.ofList

/// ['s'] -> "s"
let private stringEscapings =
    ('"' , "\\\"") :: nonprintablePairs
    |> Map.ofList

let private charUnescapings =
    charEscapings
    |> Map.toSeq
    |> Seq.map(fun(x,y)->y,x)
    |> Map.ofSeq

/// ["s"] -> 's'
let private stringUnescapings =
    stringEscapings
    |> Map.toSeq
    |> Seq.map(fun(x,y)->y,x)
    |> Map.ofSeq

let private tryPrefix (pattern:string) inp =
    let re = Regex (String.Format("^(?:{0})", pattern))
    let m = re.Match(inp)
    if m.Success then
        Some(m.Value,inp.[m.Value.Length..])
    else
        None

/// c -> 'c'
let toCharLiteral (c:char) =
    if charEscapings.ContainsKey c then
        charEscapings.[c]
    else
        c.ToString(CultureInfo.InvariantCulture)
    |> sprintf "'%s'"

/// xyz -> "xyz"
let toStringLiteral (value:string) =
    value.ToCharArray()
    |> Array.map(fun c ->
        if stringEscapings.ContainsKey c then
            stringEscapings.[c]
        else
            c.ToString(CultureInfo.InvariantCulture)
    )
    |> String.concat ""
    |> fun s -> "\"" + s + "\""


///输入字符串的前缀子字符串符合给定的模式
let (|Prefix|_|) = tryPrefix

///匹配输入字符串的第一个字符，返回剩余字符串
let (|PrefixChar|_|) (c:char) (inp:string) =
    if inp.[0] = c then
        Some inp.[1..]
    else
        None

/// \uffff -> char
let parseCode (literal:string) =
    let hex = Convert.ToUInt32(literal.[2..], 16)
    Convert.ToChar hex

/// 'c' -> c
let parseCharLiteral (quotedString:string) =
    let content = quotedString.[1..quotedString.Length-2]//去掉首尾包围的引号
    if charUnescapings.ContainsKey content then
        charUnescapings.[content]
    elif content.StartsWith @"\u" then
        parseCode content
    else
        content.[0]

/// "xyz" -> xyz
let parseStringLiteral (quotedString:string) =
    let rec loop inp =
        seq {
            match inp with
            | "" -> ()

            | Prefix """\\["bfnrt\\]""" (x,rest) ->
                yield stringUnescapings.[x]
                yield! loop rest

            | Prefix """\\u[0-9a-fA-F]{4}""" (x,rest) ->
                yield parseCode x
                yield! loop rest

            | _ ->
                yield inp.[0]
                yield! loop inp.[1..]
        }
    //去掉首尾包围的引号
    let content = quotedString.[1..quotedString.Length-2]
    String(loop content |> Array.ofSeq)

///是否為標識符
let isIdentifier (tok:string) =
    Regex.IsMatch(tok,@"^[\w-[\d]]\w*$")