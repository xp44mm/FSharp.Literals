module FSharp.Literals.StringUtils

open System.Text.RegularExpressions
open System.Globalization
open System

///是否為F#標識符
let isIdentifier (tok:string) =
    Regex.IsMatch(tok,@"^[\w-[\d]][\w']*$")
    
/// unprintable control codes -> utf-16 | else -> directly map
let ordinaryCharToLiteral (c:Char) =
    match int c with 
    | charCode when charCode < 16 ->
        "\\u000" + Convert.ToString(charCode,16)
    | charCode when charCode < 32 ->
        "\\u00" + Convert.ToString(charCode,16)
    | _ -> c.ToString(CultureInfo.InvariantCulture)

/// xyz -> "xyz"
let toStringLiteral (value:string) =
    value.ToCharArray()
    |> Array.mapi(fun i c ->
        match c with
        | '\"' -> @"\"""
        | '\\' -> @"\\"
        | '\a' -> @"\a"
        | '\b' -> @"\b"
        | '\t' -> @"\t"
        | '\n' -> @"\n"
        | '\v' -> @"\v"
        | '\f' -> @"\f"
        | '\r' -> @"\r"
        | c -> ordinaryCharToLiteral c
    )
    |> String.concat ""
    |> sprintf "\"%s\""

/// c -> 'c'
let toCharLiteral c = 
    match c with
    | '\'' -> @"\'"
    | '\\' -> @"\\"
    | '\a' -> @"\a"
    | '\b' -> @"\b"
    | '\t' -> @"\t"
    | '\n' -> @"\n"
    | '\v' -> @"\v"
    | '\f' -> @"\f"
    | '\r' -> @"\r"
    | c -> ordinaryCharToLiteral c
    |> sprintf "'%s'"

//表达式不加括號環境優先級設爲0，必加括號環境優先級設爲一个肯定是最大的数字
let putparen (precContext:int) (precExpr:int) (expr:string) =
    if precExpr > precContext then expr else "(" + expr + ")"
