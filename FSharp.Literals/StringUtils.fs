module FSharp.Literals.StringUtils

open System
open System.Text.RegularExpressions
open System.Globalization
open FSharp.Idioms.StringOps

///是否為F#標識符
let isIdentifier (tok:string) =
    Regex.IsMatch(tok,@"^[\w-[\d]][\w']*$")
    
/// xyz -> "xyz"
let toStringLiteral (value:string) =
    let needDouble(follower:string) = 
        match follower with
        | ""
        | PrefixChar '"' _ | PrefixChar '\\' _ 
        | PrefixChar '\b' _ | PrefixChar '\f' _ | PrefixChar '\n' _ | PrefixChar '\r' _ | PrefixChar '\t' _
        | PrefixChar 'b' _ | PrefixChar 'f' _ | PrefixChar 'n' _ | PrefixChar 'r' _ | PrefixChar 't' _
        | Prefix @"[0-9A-Za-z]{3,}" _
        | Prefix @"u[0-9A-Za-z]{4,}" _
        | Prefix @"U[0-9A-Za-z]{8,}" _
            -> true
        | c -> false

    let chars = value.ToCharArray()
    chars
    |> Array.mapi(fun i c ->
        match c with
        | '\\' -> 
            if needDouble value.[i+1..] then
                @"\\"
            else
                @"\"
        | '"' -> "\\\""
        | '\b' -> @"\b"
        | '\f' -> @"\f"
        | '\n' -> @"\n"
        | '\r' -> @"\r"
        | '\t' -> @"\t"
        | c -> c.ToString(CultureInfo.InvariantCulture)
    )
    |> String.concat ""
    |> fun s -> "\"" + s + "\""

/// c -> 'c'
let toCharLiteral = function
    | '\\' -> @"'\\'"
    | '\'' -> @"'\''"
    | '\b' -> @"'\b'"
    | '\f' -> @"'\f'"
    | '\n' -> @"'\n'"
    | '\r' -> @"'\r'"
    | '\t' -> @"'\t'"
    | c -> String [|'\'';c;'\''|]
