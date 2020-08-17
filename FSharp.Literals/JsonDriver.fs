module FSharp.Literals.Jsons.JsonDriver

open Compiler

let parser =
    ParsingProgram(
        JsonParsingTable.productions,
        JsonParsingTable.parsingTable)

let parseTokens tokens =
    let parsingTree = parser.parse(JsonTokenizer.getTag, tokens)
    let expr = JsonCreation.createValue parsingTree
    expr

let parse(text:string) =
    let tokens =
        text
        |> JsonTokenizer.tokenize
    let tree = parseTokens tokens
    tree
