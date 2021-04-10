namespace FSharp.Literals.DefaultValueProviders

open System
open System.Reflection

type UriDefaultValueProvider() =
    static member Singleton = UriDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty = typeof<Uri>
        member this.defaultValue(loop,ty) = new Uri("http://www.contoso.com/") |> box

