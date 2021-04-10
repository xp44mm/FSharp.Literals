namespace FSharp.Literals.DefaultValueProviders

open System
open System.Reflection

type TimeSpanDefaultValueProvider() =
    static member Singleton = TimeSpanDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty = typeof<TimeSpan>
        member this.defaultValue(loop,ty) = box <| TimeSpan.Zero

