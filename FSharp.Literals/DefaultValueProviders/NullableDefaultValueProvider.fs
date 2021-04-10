namespace FSharp.Literals.DefaultValueProviders

open System

type NullableDefaultValueProvider() =
    static member Singleton = NullableDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>
        member this.defaultValue(loop,ty) = null
