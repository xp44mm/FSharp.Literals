module FSharp.Literals.ParenRender

open System
open System.Globalization
open FSharp.Reflection

let decimalpoint (s:string) =
    if s.Contains "." || s.Contains "E" || s.Contains "e" then
        s
    else
        s + ".0"

//注意不加括號環境優先級設爲0，必加括號環境優先級設爲最高
let highest = 999

//todo:智能加括號，操作符结合性不影响加括号。
let precedences =
    [
        ["."]
        [" "]
        ["|||"]
        [","]
        [";"]
    ]
    |> List.mapi(fun i ls ->
        ls
        |> List.map(fun sym -> sym,(highest - i * 10))
    )
    |> List.concat
    |> Map.ofList

let putparen (precContext:int) (precCurrent:int) (expr:string) =
    if precContext < precCurrent then expr else String.Format("({0})",expr)

///可能会有Nullable<>時，顯式使用明確的typeof<>提供类型
let rec instanceToString (precContext:int) (ty:Type) (obj:obj) =
    if ty = typeof<bool> then
        let b = unbox<bool> obj
        if b then "true" else "false"

    elif ty = typeof<sbyte> then
        let value = unbox<sbyte> obj
        Convert.ToString value + "y"

    elif ty = typeof<byte> then
        let value = unbox<byte> obj
        Convert.ToString value + "uy"

    elif ty = typeof<int16> then
        let value = unbox<int16> obj
        Convert.ToString value  + "s"

    elif ty = typeof<uint16> then
        let value = unbox<uint16> obj
        Convert.ToString value  + "us"

    elif ty = typeof<int> then
        let value = unbox<int> obj
        Convert.ToString value

    elif ty = typeof<uint32> then
        let value = unbox<uint32> obj
        Convert.ToString value  + "u"

    elif ty = typeof<int64> then
        let value = unbox<int64> obj
        Convert.ToString value  + "L"

    elif ty = typeof<uint64> then
        let value = unbox<uint64> obj
        Convert.ToString value + "UL"

    elif ty = typeof<nativeint> then
        let value = unbox<nativeint> obj
        Convert.ToString value + "n"

    elif ty = typeof<unativeint> then
        let value = unbox<unativeint> obj
        Convert.ToString value + "un"

    elif ty = typeof<single> then
        let value = unbox<single> obj
        let s = value.ToString("R", CultureInfo.InvariantCulture) // "G9"
        decimalpoint s + "f"

    elif ty = typeof<float> then
        let value = unbox<float> obj
        let s = value.ToString("R", CultureInfo.InvariantCulture) // "G17"
        decimalpoint s

    elif ty = typeof<decimal> then
        let value = unbox<decimal> obj
        Convert.ToString value + "M"

    elif ty = typeof<bigint> then
        let value = unbox<bigint> obj
        Convert.ToString value + "I"

    elif ty = typeof<char> then
        unbox<char> obj
        |> StringUtils.toCharLiteral

    elif ty = typeof<string> then
        unbox<string> obj
        |> StringUtils.toStringLiteral

    elif ty = typeof<TimeSpan> then //空格運算符優先級
        let tspan = unbox<TimeSpan> obj

        [
            tspan.Days
            tspan.Hours
            tspan.Minutes
            tspan.Seconds
            tspan.Milliseconds
        ]
        |> List.map(fun i -> i.ToString())
        |> String.concat ","
        |> sprintf "TimeSpan(%s)"
        |> putparen precContext precedences.[" "]

    elif ty = typeof<DateTimeOffset> then //空格運算符優先級
        let thisDate = unbox<DateTimeOffset> obj
        [
            thisDate.Year       .ToString()
            thisDate.Month      .ToString()
            thisDate.Day        .ToString()
            thisDate.Hour       .ToString()
            thisDate.Minute     .ToString()
            thisDate.Second     .ToString()
            thisDate.Millisecond.ToString()
            thisDate.Offset |> instanceToString precedences.[","] typeof<TimeSpan>
        ]
        |> String.concat ","
        |> sprintf "DateTimeOffset(%s)"
        |> putparen precContext precedences.[" "]

    elif ty = typeof<DateTime> then
        let dt = unbox<DateTime> obj
        instanceToString precContext typeof<DateTimeOffset> (DateTimeOffset dt)

    elif ty = typeof<Guid> then //空格運算符優先級
        let id = unbox<Guid> obj
        sprintf "Guid(\"%s\")" <| id.ToString()
        |> putparen precContext precedences.[" "]

    elif ty.IsEnum then
        if ty.IsDefined(typeof<FlagsAttribute>,false) then
            let reader = Readers.flagsReader ty
            reader obj
            |> String.concat "|||"
            |> putparen precContext precedences.["|||"]
        else
            Enum.GetName(ty,obj)
            |> sprintf "%s.%s" ty.Name
            |> putparen precContext precedences.["."]
    elif ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>> then //.GetGenericTypeDefinition()
        if obj = null then
            "Nullable()"
        else
            let underlyingType = ty.GenericTypeArguments.[0]

            instanceToString precedences.[" "] underlyingType obj
            |> sprintf "Nullable %s"
        |> putparen precContext precedences.[" "]
    elif ty.IsArray && ty.GetArrayRank() = 1 then //一定無需加括號
        let reader = Readers.arrayReader ty
        let elements = reader obj
        let elemType = ty.GetElementType()

        arrayToString elemType elements
        |> sprintf "[|%s|]"

    elif ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>> then // .GetGenericTypeDefinition()
        //一定無需加括號
        let reader = Readers.listReader ty
        let elements = reader obj
        let elemType = ty.GenericTypeArguments.[0]

        arrayToString elemType elements
        |> sprintf "[%s]"

    elif ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>> then //.GetGenericTypeDefinition()
        let reader = Readers.setReader ty
        let elements = reader obj
        if elements.Length = 0 then
            "Set.empty"
        else
            let elementType = ty.GenericTypeArguments.[0]

            arrayToString elementType elements
            |> fun content ->
                String.Format("set [{0}]",content)
            |> putparen precContext precedences.[" "]

    elif ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>> then //.GetGenericTypeDefinition() 
        let reader = Readers.mapReader ty
        let elements = reader obj
        if elements.Length = 0 then
            "Map.empty"
        else
            let tupleType = FSharpType.MakeTupleType(ty.GenericTypeArguments)

            arrayToString tupleType elements
            |> fun content ->
                String.Format("Map.ofList [{0}]",content)
            |> putparen precContext precedences.[" "]

    elif FSharpType.IsTuple ty then
        let reader = Readers.tupleReader ty
        let elements = reader obj
        let elementTypes = FSharpType.GetTupleElements(ty)

        Array.zip elementTypes elements
        |> tupleToString
        |> putparen precContext precedences.[","]

    elif FSharpType.IsUnion ty then
        let reader = DiscriminatedUnion.unionReader ty
        let name,fields = reader obj

        match fields with
        | [||] -> name
        | [|ftype,field|] ->
            let payload = instanceToString precedences.[" "] ftype field
            if payload.StartsWith("(") then name + payload else name + " " + payload
            |> putparen precContext precedences.[" "]
        | _ ->
            fields
            |> tupleToString
            |> sprintf "%s(%s)" name
            |> putparen precContext precedences.[" "]

    elif FSharpType.IsRecord ty then
        let reader = Readers.recordReader ty
        let fields = reader obj

        fields
        |> Array.map(fun(nm,tp,value)->
            let nm = if StringUtils.isIdentifier nm then nm else String.Format("``{0}``",nm)
            let value = instanceToString 0 tp value
            String.Format("{0}={1}",nm,value)
        )
        |> String.concat ";"
        |> sprintf "{%s}"
    elif obj = null then
        "null" //没有类型信息，null,nullable,None都打印成null
    elif ty = typeof<obj> && obj.GetType() <> typeof<obj> then
        instanceToString precContext (obj.GetType()) obj
    else
        sprintf "%A" obj

and arrayToString elemType (elements:obj[]) =
    elements
    |> Array.map(instanceToString precedences.[";"] elemType)
    |> String.concat ";"

and tupleToString fields =
    fields
    |> Array.map(fun(ftype,field)-> instanceToString precedences.[","] ftype field)
    |> String.concat ","

// null, nullable, None需要显示提供类型
let stringifyNullableType tp obj = instanceToString 0 tp obj

///确定可以使用obj.GetType()
let stringify obj = 
    stringifyNullableType (obj.GetType()) obj

