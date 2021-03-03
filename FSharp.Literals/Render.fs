module FSharp.Literals.Render

open System

/// print dynamic value
let stringifyObj (tp:Type) (value:obj) = ParenRender.instanceToString 0 tp value

/// print generic value
let stringify<'t> (value:'t) = 
    let tp = typeof<'t>
    stringifyObj tp value

let printTypeObj (ty:Type) = TypeRender.printParen TypeRender.printers 0 ty

let printType<'t> = printTypeObj typeof<'t>
