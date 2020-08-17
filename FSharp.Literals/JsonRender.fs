module FSharp.Literals.Jsons.JsonRender

open FSharp.Literals.StringUtils
open System.Globalization

let decimalpoint (s:string) =
    if s.Contains "." || s.Contains "E" || s.Contains "e" then
        s
    else
        s + ".0"

let rec stringify = function
| Json.Map mp ->
    mp
    |> Map.toArray
    |> Array.map(fun(k,v)->
        toStringLiteral k + ":" + stringify v
    )
    |> String.concat ","
    |> sprintf "{%s}"
| Json.List ls ->
    ls
    |> Array.map(fun v -> stringify v )
    |> String.concat ","
    |> sprintf "[%s]"
| Json.Null -> "null"
| Json.False -> "false"
| Json.True -> "true"
| Json.String x -> toStringLiteral x
| Json.Char c -> toCharLiteral c
| Json.SByte value -> 
    value.ToString(CultureInfo.InvariantCulture) + "y"
| Json.Byte value -> 
    value.ToString(CultureInfo.InvariantCulture) + "uy"
| Json.Int16 value -> 
    value.ToString(CultureInfo.InvariantCulture)  + "s"
| Json.UInt16 value -> 
    value.ToString(CultureInfo.InvariantCulture)  + "us"
| Json.Int32 value -> 
    value.ToString(CultureInfo.InvariantCulture)
| Json.UInt32 value -> 
    value.ToString(CultureInfo.InvariantCulture)  + "u"
| Json.Int64 value -> 
    value.ToString(CultureInfo.InvariantCulture)  + "L"
| Json.UInt64 value -> 
    value.ToString(CultureInfo.InvariantCulture) + "UL"
| Json.IntPtr value -> 
    value.ToString() + "n"
| Json.UIntPtr value -> 
    value.ToString() + "un"
| Json.BigInteger value -> 
    value.ToString(CultureInfo.InvariantCulture) + "I"
| Json.Single value -> 
    // R:與G相同，但無損打印出所有精度。
    let s = value.ToString("R", CultureInfo.InvariantCulture) // "G9"
    decimalpoint s + "f"
| Json.Double value -> 
    let s = value.ToString("R", CultureInfo.InvariantCulture) // "G17"
    decimalpoint s
| Json.Decimal value -> 
    value.ToString(CultureInfo.InvariantCulture) + "M"


