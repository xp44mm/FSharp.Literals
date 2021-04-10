namespace FSharp.Literals

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open FSharp.Literals

type StringLiteralTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``escape sequence in string``() =
        //https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/strings
        let x = "\a\b\f\n\r\t\v\\\"\'"
        let y = x.ToCharArray()
        should.equal y [|'\a';'\b';'\f';'\n';'\r';'\t';'\v';'\\';'\"';'\'';|]

    [<Fact>]
    member this.``Quotation mark``() =
        should.equal "\"" <| '\"'.ToString()
        should.equal "\"" <| '"'.ToString()
    
    [<Fact>]
    member this.``Apostrophe``() =
        should.equal '\'' <| "\'".Chars 0
        should.equal '\'' <| "'".Chars 0

    [<Fact>]
    member this.``toStringLiteral empty``() =
        let x = ""
        let y = StringUtils.toStringLiteral x
        should.equal y "\"\""

    [<Fact>]
    member this.``toStringLiteral null char``() =
        let x = "\u0000"
        let y = StringUtils.toStringLiteral x
        should.equal y @"""\u0000"""

    [<Fact>]
    member this.``toCharLiteral null char``() =
        let x = '\u0000'
        let y = StringUtils.toCharLiteral x
        should.equal y @"'\u0000'"

    [<Fact>]
    member this.``toStringLiteral Unit Separator``() =
        let x = "\u001f"
        let y = StringUtils.toStringLiteral x
        should.equal y @"""\u001f"""

    [<Fact>]
    member this.``toCharLiteral Unit Separator``() =
        let x = '\u001f'
        let y = StringUtils.toCharLiteral x
        should.equal y @"'\u001f'"

    [<Fact>]
    member this.``toStringLiteral quote``() =
        let x = "\""
        let y = StringUtils.toStringLiteral x
        should.equal y "\"\\\"\""


    [<Fact>]
    member this.``toStringLiteral Escape Characters``() =
        let x = String [|'"';'\\';'\b';'\f';'\n';'\r';'\t';'\\';'w';'\\'|]
        let y = StringUtils.toStringLiteral x
        should.equal y <| """ "\"\\\b\f\n\r\t\\w\\" """.Trim()

    [<Fact>]
    member this.``toStringLiteral Trigraph``() =
        let x = "\032" 
        let y = StringUtils.toStringLiteral x
        should.equal y "\" \""

    [<Fact>]
    member this.``toStringLiteral Unicode character``() =
        let x = "\u00a9"
        let y = StringUtils.toStringLiteral x
        should.equal y "\"©\""

    [<Fact>]
    member this.``toStringLiteral Long Unicode character``() =
        let x = "\U00002260"
        let y = StringUtils.toStringLiteral x
        should.equal y "\"≠\""

