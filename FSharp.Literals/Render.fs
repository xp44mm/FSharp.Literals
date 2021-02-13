module FSharp.Literals.Render

/// similar to `sprintf "%A" value`
let stringify<'t> (value:'t) = 
    let tp = typeof<'t>
    ParenRender.instanceToString 0 tp value