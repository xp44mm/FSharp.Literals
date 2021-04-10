namespace FSharp.Literals.DefaultValueProviders

open System
open Microsoft.FSharp.Reflection

type RecordDefaultValueProvider() =
    static member Singleton = RecordDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = FSharpType.IsRecord ty
        member this.defaultValue(loop,ty) =
            let fields = FSharpType.GetRecordFields(ty)
            let values =
                fields
                |> Array.map(fun pi -> loop pi.PropertyType)
            FSharpValue.MakeRecord(ty,values)

