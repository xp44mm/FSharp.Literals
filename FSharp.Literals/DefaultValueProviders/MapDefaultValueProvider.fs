namespace FSharp.Literals.DefaultValueProviders

open System
open FSharp.Idioms

type MapDefaultValueProvider() =
    static member Singleton = MapDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>>
        member this.defaultValue(loop,ty) =
            let emptyGeneric = MapType.mapModuleType.GetMethod "Empty"
            let empty = emptyGeneric.MakeGenericMethod(ty.GenericTypeArguments)
            empty.Invoke(null, Array.empty) |> box

