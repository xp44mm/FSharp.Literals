[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Literals.Jsons.Json

let parse(text:string) = JsonDriver.parse text

let stringify tree = JsonRender.stringify tree


