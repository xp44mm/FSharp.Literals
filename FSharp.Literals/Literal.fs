module FSharp.Literals.Literal

open System

let printTypeDynamic (ty:Type) = TypeRender.printParen TypeRender.printers 0 ty

let printType<'t> = printTypeDynamic typeof<'t>

/// print dynamic value
let stringifyDynamic (ty:Type) (value:obj) = ParenRender.instanceToString 0 ty value

/// print generic value
let stringify<'t> (value:'t) = stringifyDynamic typeof<'t> value

open FSharp.Literals.DefaultValueProviders

let defaultValueDynamic (ty:Type) = DefaultValueDriver.defaultValue DefaultValueProviders.providers ty

let defaultValue<'t> = defaultValueDynamic typeof<'t> :?> 't