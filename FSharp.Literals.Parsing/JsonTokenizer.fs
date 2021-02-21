module FSharp.Literals.Parsing.JsonTokenizer

open FSharp.Idioms

open System.Globalization

type NumberInfo =
    {
        known: string list
        flags: Set<char>
    }

let realNumberFlags = set [ '.'; 'e' ]

let isDouble flags =
    (realNumberFlags - flags).IsProperSubsetOf realNumberFlags

let realStyle (flags:Set<_>) =
    let p =
        if flags.Contains '.' then
            NumberStyles.AllowDecimalPoint
        else
            NumberStyles.None
    let e =
        if flags.Contains 'e' then
            NumberStyles.AllowExponent
        else
            NumberStyles.None

    p ||| e

let getBase (flags:Set<_>) =
    if flags.Contains 'x' then
        16
    elif flags.Contains 'o' then
        8
    elif flags.Contains 'b' then
        2
    else
        10

let tokenize inp =
    let rec loop (inp:string) =
        seq {
            match inp with
            | "" -> ()

            | Prefix @"\s+" (_,rest) -> yield! loop rest

            | PrefixChar '{' rest ->
                yield LEFT_BRACE
                yield! loop rest

            | PrefixChar '}' rest ->
                yield RIGHT_BRACE
                yield! loop rest

            | PrefixChar '[' rest ->
                yield LEFT_BRACK
                yield! loop rest

            | PrefixChar ']' rest ->
                yield RIGHT_BRACK
                yield! loop rest

            | PrefixChar ':' rest ->
                yield COLON
                yield! loop rest

            | PrefixChar ',' rest ->
                yield COMMA
                yield! loop rest

            | Prefix """(?:"(\\[/'"bfnrt\\]|\\u[0-9a-fA-F]{4}|[^\\"])*")""" (lexeme,rest) ->
                yield  STRING(StringLiteral. parseStringLiteral lexeme)
                yield! loop rest

            | Prefix @"'(\\[\\'bfnrt]|\\u[0-9a-fA-F]{4}|[^\\'])'" (lexeme,rest) ->
                yield  CHAR(StringLiteral.parseCharLiteral lexeme)
                yield! loop rest

            | Prefix @"null\b" (_,rest) ->
                yield NULL
                yield! loop rest

            | Prefix @"true\b" (_,rest) ->
                yield TRUE
                yield! loop rest

            | Prefix @"false\b" (_,rest) ->
                yield FALSE
                yield! loop rest

            | Prefix @"[-+]?\d+" (a,rest) ->
                let known, flags =
                    match a.[0] with
                    | '-' -> a.[1..], set ['-']
                    | '+' -> a.[1..], set []
                    | _ -> a, set[]
                yield! numberLoop {known=[known];flags=flags} rest
            | never -> failwith never
        }
    and numberLoop (info:NumberInfo) inp =
        match inp with
        | Prefix @"\.\d+" (x,rest) ->
            let info = {
                known = [info.known.Head + x]
                flags = info.flags.Add '.'
            }
            numberLoop info rest
        | Prefix @"[eE][-+]?\d+" (x,rest) ->
            let info = {
                known = [info.known.Head + x]
                flags = info.flags.Add 'e'
            }
            numberLoop info rest
        | Prefix @"[xX][0-9a-fA-F]+" (x,rest) ->
            let info = {
                known = [x.[1..]]
                flags = info.flags.Add 'x'
            }
            numberLoop info rest
        | Prefix @"[oO][0-7]+" (x,rest) ->
            let info = {
                known = [x.[1..]]
                flags = info.flags.Add 'o'
            }
            numberLoop info rest
        | Prefix @"[bB][01]+" (x,rest) ->
            let info = {
                known = [x.[1..]]
                flags = info.flags.Add 'b'
            }
            numberLoop info rest
        | Prefix @"([fFmMIu]|u?[ysln]|U?L)\b" (x,rest) ->
            let info = {
                known = x :: info.known
                flags = info.flags.Add '$'
            }
            numberLoop info rest
        | _  ->
            let inline sign x =
                if info.flags.Contains '-' then -x else x
            seq {
                if info.flags.Contains '$' then
                    match info.known.Head with
                    |  "f" | "F" ->
                        let ii =
                            System.Single.Parse(info.known.Tail.Head,realStyle info.flags)
                            |> sign
                        yield SINGLE ii
                    |  "m" | "M" ->
                        let ii =
                            System.Decimal.Parse(info.known.Tail.Head,realStyle info.flags)
                            |> sign
                        yield DECIMAL ii
                    |  "y" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parseInt32
                            |> System.Convert.ToSByte
                            |> sign
                        yield SBYTE ii
                    | "uy" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parseInt32
                            |> System.Convert.ToByte
                        yield BYTE ii
                    |  "s" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parseInt32
                            |> System.Convert.ToInt16
                            |> sign
                        yield INT16 ii
                    | "us" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parseInt32
                            |> System.Convert.ToUInt16
                        yield UINT16 ii
                    |  "l" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parseInt32
                            |> sign
                        yield INT32 ii
                    | "u"|"ul" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse System.Convert.ToUInt32
                        yield UINT32 ii
                    |  "L" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse System.Convert.ToInt64
                            |> sign
                        yield INT64 ii
                    | "UL" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse System.Convert.ToUInt64
                        yield UINT64 ii
                    |  "n" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse System.Convert.ToInt64
                            |> System.IntPtr.op_Explicit
                            |> sign
                        yield INTPTR ii
                    | "un" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse System.Convert.ToUInt64
                            |> System.UIntPtr.op_Explicit

                        yield UINTPTR ii
                    | "I" ->
                        let ii =
                            (getBase info.flags,info.known.Tail.Head)
                            ||> ParseInteger.parse (System.Numerics.BigInteger.op_Implicit)
                            |> sign
                        yield BIGINTEGER ii
                    | never -> failwith never
                elif isDouble info.flags then
                    let ii =
                        System.Double.Parse (info.known.Head, realStyle info.flags)
                        |> sign
                    yield DOUBLE ii
                else
                    let ii =
                        (getBase info.flags, info.known.Head)
                        ||> ParseInteger.parseInt32
                        |> sign
                    yield INT32 ii

                yield! loop inp
            }

    loop inp

