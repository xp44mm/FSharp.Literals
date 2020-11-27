module FSharp.Literals.DiscriminatedUnion

open Microsoft.FSharp.Reflection
open System.Collections.Concurrent
open System

///获取可区分联合(discriminated union)实例的标签名
/// == Regex.Match(instance.ToString(),@"^\w+")
let getTag<'DU> =
    let caseNames =
        FSharpType.GetUnionCases typeof<'DU>
        |> Array.map(fun info -> info.Name)
    let reader = FSharpValue.PreComputeUnionTagReader typeof<'DU>
    fun (instance:'DU) -> caseNames.[reader instance]

///可区分联合分解一次
let unionReader =
    let dic = ConcurrentDictionary<Type, obj -> string * (Type*obj)[]>(HashIdentity.Structural)
    fun (ty:Type) ->
        if dic.ContainsKey ty |> not then
            let unionCases = FSharpType.GetUnionCases ty
            let tagReader = FSharpValue.PreComputeUnionTagReader ty

            //tag的前缀
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
