namespace FSharp.Literals.DefaultValueProviders

open System
open FSharp.Idioms

type ListDefaultValueProvider() =
    static member Singleton = ListDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>>
        member this.defaultValue(loop,ty) =
            let listModuleType = ListType.listModuleType
            let emptyGeneric = listModuleType.GetMethod "Empty"
            let listempty = emptyGeneric.MakeGenericMethod(ty.GenericTypeArguments)
            listempty.Invoke(null, Array.empty) |> box

