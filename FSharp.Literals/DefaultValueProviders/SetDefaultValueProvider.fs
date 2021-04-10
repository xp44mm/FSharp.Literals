namespace FSharp.Literals.DefaultValueProviders

open System
open FSharp.Idioms

type SetDefaultValueProvider() =
    static member Singleton = SetDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>>
        member this.defaultValue(loop,ty) =
            let emptyGeneric = SetType.setModuleType.GetMethod "Empty"
            let empty = emptyGeneric.MakeGenericMethod(ty.GenericTypeArguments)
            empty.Invoke(null, Array.empty) |> box

