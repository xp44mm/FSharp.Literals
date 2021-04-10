namespace FSharp.Literals.DefaultValueProviders

open System
open Microsoft.FSharp.Reflection

type OptionDefaultValueProvider() =
    static member Singleton = OptionDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = FSharpType.IsUnion ty && ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Option<_>>
        member this.defaultValue(loop,ty) = null

