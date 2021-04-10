namespace FSharp.Literals.DefaultValueProviders

open System

type DBNullDefaultValueProvider() =
    static member Singleton = DBNullDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty = typeof<DBNull>
        member this.defaultValue(loop,ty) = box DBNull.Value
