namespace FSharp.Literals.DefaultValueProviders

open System
open System.Reflection

type GuidDefaultValueProvider() =
    static member Singleton = GuidDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty = typeof<Guid>
        member this.defaultValue(loop,ty) = new Guid() |> box

