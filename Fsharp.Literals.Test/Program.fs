module FSharp.Literals.Program

open System
open Microsoft.FSharp.Reflection


let [<EntryPoint>] main _ = 
    let ty = typeof<(string*int)[]>
    let y = Render.printTypeObj ty

    0
