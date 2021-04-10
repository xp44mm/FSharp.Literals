module FSharp.Literals.TypeRender

open System
open Microsoft.FSharp.Reflection
open System.Text.RegularExpressions

//
let precedences =
    [
        []
        [":"]
        [","]
        ["->"]
        ["*"]
        ["<>";"[]"]
        ["max"]
    ]
    |> List.mapi(fun i ls ->
        ls
        |> List.map(fun sym -> sym,i*10)
    )
    |> List.concat
    |> Map.ofList

///打印简单的类型
let printSimpleType (ty:Type) =
    if ty.IsEnum then ty.Name
    elif ty = typeof<unit>       then "unit"
    elif ty = typeof<bool>       then "bool"
    elif ty = typeof<string>     then "string"
    elif ty = typeof<char>       then "char"
    elif ty = typeof<sbyte>      then "sbyte"
    elif ty = typeof<byte>       then "byte"
    elif ty = typeof<int16>      then "int16"
    elif ty = typeof<uint16>     then "uint16"
    elif ty = typeof<int>        then "int"
    elif ty = typeof<uint32>     then "uint32"
    elif ty = typeof<int64>      then "int64"
    elif ty = typeof<uint64>     then "uint64"
    elif ty = typeof<single>     then "single"
    elif ty = typeof<float>      then "float"
    elif ty = typeof<decimal>    then "decimal"
    elif ty = typeof<nativeint>  then "nativeint"
    elif ty = typeof<unativeint> then "unativeint"
    elif ty = typeof<bigint>     then "bigint"
    else ty.Name


let ArrayTypePrinter = {
    new TypePrinter with
        member _.filter(ty) = ty.IsArray && ty.GetArrayRank() = 1
        member _.print(loop, prec, ty) = 
            let arrayPrec = precedences.["[]"]
            //Console.WriteLine(sprintf"arrayPrec:%d" arrayPrec)
            let elementType = ty.GetElementType()
            loop arrayPrec elementType + "[]"
            |> StringUtils.putparen prec arrayPrec
}

let TupleTypePrinter = {
    new TypePrinter with
        member _.filter(ty) = FSharpType.IsTuple ty
        member _.print(loop, prec, ty) = 
            let tuplePrec = precedences.["*"]
            //Console.WriteLine(sprintf"tuplePrec:%d" tuplePrec)

            FSharpType.GetTupleElements ty
            |> Array.map(loop (tuplePrec+1))
            |> String.concat "*"
            |> StringUtils.putparen prec tuplePrec
}

let getGenericTypeName (type_name:string) =
    match type_name.Split('`').[0] with
    | "FSharpList" -> "list"
    | "FSharpSet" -> "Set"
    | "FSharpMap" -> "Map"
    | "IEnumerable" -> "seq"
    | "List" -> "ResizeArray"
    | "Void" -> "unit"
    | name -> name

let GenericTypeDefinitionPrinter = {
    new TypePrinter with
        member _.filter(ty) = ty.IsGenericType && ty.IsGenericTypeDefinition
        member _.print(loop, prec, ty) = 
            let name = getGenericTypeName ty.Name

            ty.GetGenericArguments()
            |> Array.filter(fun p -> p.IsGenericParameter)
            |> Array.sortBy(fun p -> p.GenericParameterPosition)
            |> Array.map(fun t -> "'" + t.Name)
            |> String.concat ","
            |> sprintf "%s<%s>" name
            |> StringUtils.putparen prec precedences.["<>"]
}

let GenericTypePrinter = {
    new TypePrinter with
        member _.filter(ty) = ty.IsGenericType
        member _.print(loop, prec, ty) = 
            let name = getGenericTypeName ty.Name
            ty.GenericTypeArguments
            |> Array.map(loop precedences.[","])
            |> String.concat ","
            |> sprintf "%s<%s>" name
            |> StringUtils.putparen prec precedences.["<>"]
}

let AnonymousRecordTypePrinter = {
    new TypePrinter with
        member _.filter(ty) = FSharpType.IsRecord ty && Regex.IsMatch(ty.Name,"^<>f__AnonymousType\d+`\d+$")
        member _.print(loop, prec, ty) = 
            FSharpType.GetRecordFields ty
            |> Array.map(fun pi -> sprintf "%s:%s" pi.Name <| loop precedences.[":"] pi.PropertyType)
            |> String.concat ";"
            |> sprintf "{|%s|}"
}

let FunctionTypePrinter = {
    new TypePrinter with
        member _.filter(ty) = FSharpType.IsFunction ty
        member _.print(loop, prec, ty) = 
            let domainType, rangeType = FSharpType.GetFunctionElements ty

            let funPrec = precedences.["->"]

            let domainType = loop (funPrec+1) domainType
            let rangeType  = loop (funPrec-1) rangeType

            sprintf "%s->%s" domainType rangeType
            |> StringUtils.putparen prec funPrec
}

let printers = [
    ArrayTypePrinter
    TupleTypePrinter
    AnonymousRecordTypePrinter
    FunctionTypePrinter
    GenericTypeDefinitionPrinter
    GenericTypePrinter
]

/// 根据优先级确定表达式是否带括号
let rec printParen (printers:#seq<TypePrinter>) (prec:int) (ty:Type) =
    let print =
        printers
        |> Seq.tryFind(fun reader -> reader.filter ty)
        |> Option.map(fun x -> x.print)
        |> Option.defaultValue(fun(loop,prec,ty) -> printSimpleType ty)

    print(printParen printers, prec, ty)
