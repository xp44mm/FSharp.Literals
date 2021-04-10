namespace FSharp.Literals.DefaultValueProviders

open System

/// read from fsharp value to json
type DefaultValueProvider = 
    abstract filter: Type -> bool
    abstract defaultValue: loop:(Type -> obj) * Type -> obj

