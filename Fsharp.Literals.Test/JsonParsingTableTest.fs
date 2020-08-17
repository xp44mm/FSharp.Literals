namespace FSharp.Literals.Jsons.Tests

open Xunit
open Xunit.Abstractions
open System.IO

open Compiler
open FSharp.Literals
open FSharp.Literals.Jsons

type JsonParsingTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName

    [<Fact>]
    member this.``productions``() =
        let yacc = Path.Combine(solutionPath, @"FSharp.Literals\json.yacc")

        let text = File.ReadAllText(yacc)
        let driver = YaccFileDriver.parse text

        let result =
            [
                "let productions = "
                "    " + Render.stringify driver.productions
            ] |> String.concat "\r\n"

        output.WriteLine(result)

        //测试数据是否为最新，确保没有过时
        Should.equal driver.productions JsonParsingTable.productions

    [<Fact>]
    member this.``shiftReduceConflicts``() =
        let tbl = ParsingTable.ambiguousTable(JsonParsingTable.productions)
        show tbl.shiftReduceConflicts

        //根据这个表输入优先级
        let conflicts = []

        //测试数据是否为最新，确保没有过时
        Should.equal conflicts tbl.shiftReduceConflicts

    [<Fact>]
    member this.``Parsing Table Generator``() =

        let gen = ParsingTableGenerator(JsonParsingTable.productions,[])

        let parsingTable = 
            gen.encodeTable
            |> List.toArray

        let result =
            [
                "let parsingTable = "
                "    " + Render.stringify parsingTable
            ] |> String.concat "\r\n"

        output.WriteLine(result)

        //测试数据是否为最新，确保没有过时
        Should.equal parsingTable JsonParsingTable.parsingTable

    //[<Fact>]
    //member this.``regex first or last token test``() =
    //    let yacc = Path.Combine(solutionPath, @"Compiler\lex.yacc")

    //    let text = File.ReadAllText(yacc)
    //    let driver = YaccFileDriver.parse text

    //    let grammar = Grammar.create driver.productions
    //    let nullableTable = NullableCollection grammar

    //    let first =
    //        FirstCollection(grammar,nullableTable).firstMap.["Expr"]

    //    let last =
    //        LastCollection(grammar,nullableTable).lastMap.["Expr"]

    //    output.WriteLine("let first = ")
    //    show first
    //    output.WriteLine("let last = ")
    //    show last

    //    Should.equal first LexFileTokenizer.first
    //    Should.equal last LexFileTokenizer.last
