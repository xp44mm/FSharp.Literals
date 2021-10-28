module FSharp.Literals.Literal

open System

//let printTypeDynamic (ty:Type) = TypeRender.printParen TypeRender.printers 0 ty

//let printType<'t> = printTypeDynamic typeof<'t>

let stringifyTypeDynamic (ty:Type) = TypeRender.stringifyParen TypeRender.stringifies 0 ty

let stringifyType<'t> = stringifyTypeDynamic typeof<'t>

/// print dynamic value
let stringifyDynamic (ty:Type) (value:obj) = ParenRender.instanceToString 0 ty value

/// print generic value
let stringify<'t> (value:'t) = stringifyDynamic typeof<'t> value

open FSharp.Literals.DefaultValues

let defaultValueDynamic (ty:Type) = DefaultValueDriver.defaultValue DefaultValues.getDefaults ty

let defaultValue<'t> = defaultValueDynamic typeof<'t> :?> 't