module FSharp.Literals.Render

let stringify = ParenRender.stringify

let stringifyNullableType = ParenRender.stringifyNullableType

let serialize<'t> (value:'t) = 
    let tp = typeof<'t>
    ParenRender.instanceToString 0 tp value