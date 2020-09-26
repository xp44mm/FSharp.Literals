module FSharp.Literals.Readers

open System
open System.Collections.Concurrent
open System.Reflection
open FSharp.Reflection

let fsharpAssembly = Assembly.Load("FSharp.Core")
let setModuleType = fsharpAssembly.GetType("Microsoft.FSharp.Collections.SetModule")
let mapModuleType = fsharpAssembly.GetType("Microsoft.FSharp.Collections.MapModule")
let seqTypeDef = typeof<seq<_>>.GetGenericTypeDefinition()
let giterorDef = typeof<System.Collections.Generic.IEnumerator<_>>.GetGenericTypeDefinition()
let biteror = typeof<System.Collections.IEnumerator>
let mMoveNext = biteror.GetMethod("MoveNext")

///列表分解一次为元素
let listReader =
    let dic = ConcurrentDictionary<Type, obj -> obj[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        //let elemType = ty.GenericTypeArguments.[0]
        if dic.ContainsKey(ty) |> not then
            let pIsEmpty = ty.GetProperty("IsEmpty")
            let pHead = ty.GetProperty("Head")
            let pTail = ty.GetProperty("Tail")
            let rec objToRevList acc ls =
                if pIsEmpty.GetValue(ls) |> unbox<bool> then
                    acc
                else
                    let elem = pHead.GetValue(ls)
                    let tail = pTail.GetValue(ls)
                    let acc = elem :: acc
                    objToRevList acc tail
            let reader obj =
                obj
                |> objToRevList []
                |> List.rev
                |> List.toArray
            dic.TryAdd(ty, reader) |> ignore

        dic.[ty]

///元组分解一次为元素
let tupleReader =
    let dic = ConcurrentDictionary<Type, obj -> obj[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey(ty)|> not then
            let reader = FSharpValue.PreComputeTupleReader ty
            dic.TryAdd(ty,reader) |> ignore
        dic.[ty]

///可区分联合分解一次
let unionReader =
    let dic = ConcurrentDictionary<Type, obj -> string * (Type*obj)[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey ty |> not then
            let unionCases = FSharpType.GetUnionCases ty
            let tagReader = FSharpValue.PreComputeUnionTagReader ty

            let qualifiedAccess = 
                let ty = unionCases.[0].DeclaringType
                if ty.IsDefined(typeof<RequireQualifiedAccessAttribute>,false) then
                    ty.Name + "."
                else
                    ""
            let unionCases =
                unionCases
                |> Array.map(fun uc ->
                    {|
                        name = qualifiedAccess + uc.Name
                        fieldTypes = uc.GetFields() |> Array.map(fun pi -> pi.PropertyType)
                        reader = FSharpValue.PreComputeUnionReader uc
                    |})
            let reader (obj:obj) =
                let tag = tagReader obj
                let unionCase = unionCases.[tag]

                let types = unionCase.fieldTypes
                let values = unionCase.reader obj
                let fields = Array.zip types values
                unionCase.name, fields
            dic.TryAdd(ty,reader) |> ignore
        dic.[ty]

let recordReader =
    let dic = ConcurrentDictionary<Type, obj -> (string*Type*obj)[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey(ty)|> not then
            let fields = FSharpType.GetRecordFields ty
            let getValues = FSharpValue.PreComputeRecordReader ty
            let reader (obj:obj) =
                getValues obj
                |> Array.zip fields
                |> Array.map(fun(pi,value)->
                    pi.Name, pi.PropertyType, value
                )
            dic.TryAdd(ty,reader) |> ignore
        dic.[ty]

///[<Flags>] Enum 的值
let flagsReader =
    let dic = ConcurrentDictionary<Type, obj -> string[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey ty |> not then
            let enumUnderlyingType = ty.GetEnumUnderlyingType()
            let zeroPairs,positivePairs =
                Enum.GetNames(ty)
                |> Array.map(fun name ->
                    let f = ty.GetField(name, BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static)
                    f.GetValue(null),sprintf "%s.%s" ty.Name name
                )
                |> Array.partition(fun(v,nm)-> EnumUtils.isGenericZero enumUnderlyingType v)

            let zeroNames = zeroPairs |> Array.map snd

            let reader (obj:obj) =
                let flags =
                    positivePairs
                    |> Array.filter(fun(value,name)->EnumUtils.genericMask enumUnderlyingType obj value)
                    |> Array.map snd

                if Array.isEmpty flags then zeroNames else flags
            dic.TryAdd(ty,reader) |> ignore
        dic.[ty]

///数组分解一次为元素
let arrayReader =
    let dic = ConcurrentDictionary<Type, obj -> obj[]>(HashIdentity.Structural)
    fun (arrayType:Type) ->
        if dic.ContainsKey(arrayType) |> not then
            let pLength = arrayType.GetProperty("Length")
            let mGetValue = arrayType.GetMethod("GetValue",[|typeof<int>|])

            let reader obj =
                let len = pLength.GetValue(obj) |> unbox<int>

                [|1..len|]
                |> Array.mapi(fun i _ ->
                    mGetValue.Invoke(obj,[|box i|])
                )
            dic.TryAdd(arrayType, reader) |> ignore

        dic.[arrayType]

///Set可以讀取到集合中的元素
let setReader =
    let dic = ConcurrentDictionary<Type, obj -> obj[]>(HashIdentity.Structural)
    let mToArrayDef = setModuleType.GetMethod "ToArray"
    fun (ty:Type) ->
        if dic.ContainsKey(ty) |> not then
            let elementType = ty.GenericTypeArguments.[0]
            let arrayType = ty.GenericTypeArguments.[0].MakeArrayType()
            let arrReader = arrayReader arrayType
            let mToArray = mToArrayDef.MakeGenericMethod(elementType)
            
            let reader obj =
                mToArray.Invoke(null,[|obj|])
                |> arrReader
            dic.TryAdd(ty, reader) |> ignore
        dic.[ty]

///Map转化为数组
let mapReader =
    let dic = ConcurrentDictionary<Type, obj -> (obj)[]>(HashIdentity.Structural)
    let mToArrayDef = mapModuleType.GetMethod "ToArray"
    fun (ty:Type) ->
        if dic.ContainsKey(ty) |> not then
            let typeArguments = ty.GenericTypeArguments
            let tupleType = FSharpType.MakeTupleType(ty.GenericTypeArguments)
            let arrayType = tupleType.MakeArrayType()
            let arrReader = arrayReader arrayType
            let mToArray = mToArrayDef.MakeGenericMethod(typeArguments)

            let reader obj =
                mToArray.Invoke(null,[|obj|])
                |> arrReader
            dic.TryAdd(ty, reader) |> ignore
        dic.[ty]

/// 读取序列中的元素
let seqReader =
    let dic = ConcurrentDictionary<Type, obj -> obj[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey(ty) |> not then
            let elemType = ty.GenericTypeArguments.[0]
            let seqType = seqTypeDef.MakeGenericType(elemType)
            let mGetEnumerator = seqType.GetMethod("GetEnumerator")
            let giteror = giterorDef.MakeGenericType(elemType)
            let pCurrent = giteror.GetProperty("Current")

            let reader ls =
                let enumerator = mGetEnumerator.Invoke(ls,[||])
                [|
                    while(mMoveNext.Invoke(enumerator,[||])|>unbox<bool>) do
                        yield pCurrent.GetValue(enumerator)
                |]
            dic.TryAdd(ty, reader) |> ignore
        dic.[ty]


