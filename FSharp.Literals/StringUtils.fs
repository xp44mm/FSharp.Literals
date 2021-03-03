module FSharp.Literals.StringUtils

open System.Text.RegularExpressions
open System.Globalization
open System

///是否為F#標識符
let isIdentifier (tok:string) =
    Regex.IsMatch(tok,@"^[\w-[\d]][\w']*$")
    
/// xyz -> "xyz"
let toStringLiteral (value:string) =
    value.ToCharArray()
    |> Array.mapi(fun i c ->
        match c with
        | '\a' -> @"\a"
        | '\b' -> @"\b"
        | '\f' -> @"\f"
        | '\n' -> @"\n"
        | '\r' -> @"\r"
        | '\t' -> @"\t"
        | '\v' -> @"\v"
        | '\\' -> @"\\"
        | '\"' -> @"\"""
        //| '\'' -> @"\'"
        | c -> c.ToString(CultureInfo.InvariantCulture)
    )
    |> String.concat ""
    |> sprintf "\"%s\""

/// c -> 'c'
let toCharLiteral c = 
    match c with
    | '\a' -> @"\a"
    | '\b' -> @"\b"
    | '\f' -> @"\f"
    | '\n' -> @"\n"
    | '\r' -> @"\r"
    | '\t' -> @"\t"
    | '\v' -> @"\v"
    | '\\' -> @"\\"
    //| '\"' -> @"\"""
    | '\'' -> @"\'"
    | c -> c.ToString(CultureInfo.InvariantCulture)
    |> sprintf "'%s'"

//表达式不加括號環境優先級設爲0，必加括號環境優先級設爲一个肯定是最大的数字
let putparen (precContext:int) (precExpr:int) (expr:string) =
    if precExpr > precContext then expr else "(" + expr + ")"
