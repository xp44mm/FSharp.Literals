namespace FSharp.Literals.DefaultValueProviders

open System
open System.Reflection

type EnumDefaultValueProvider() =
    static member Singleton = EnumDefaultValueProvider() :> DefaultValueProvider
    interface DefaultValueProvider with
        member this.filter ty = ty.IsEnum
        member this.defaultValue(loop,ty) = 
            //let enumUnderlyingType = ty.GetEnumUnderlyingType()
            Enum.GetNames(ty)
            |> Array.map(fun name ->
                ty.GetField(
                    name, BindingFlags.Public ||| BindingFlags.Static
                    ).GetValue(null)
            )
            |> Array.minBy(hash)

