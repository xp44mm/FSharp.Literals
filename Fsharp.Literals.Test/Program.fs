module Program

open System
open Microsoft.FSharp.Reflection

open FSharp.Literals.DefaultValueProviders

let [<EntryPoint>] main _ = 
    let ty = typeof<string>
    Console.WriteLine( ty.IsValueType)
    Console.WriteLine( ty.IsPrimitive)
    Console.WriteLine( ty.IsClass)

    0
