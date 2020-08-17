namespace FSharp.Literals.Jsons.Tests

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.Literals.Jsons


type TokenizeTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``char test``() =
        let x = "'0'"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (CHAR '0')

    [<Fact>]
    member this.``sbyte test``() =
        let x = "0y"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (SBYTE 0y)

    [<Fact>]
    member this.``byte test``() =
        let x = "0uy"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (BYTE 0uy)

    [<Fact>]
    member this.``int16 test``() =
        let x = "0s"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INT16 0s)


    [<Fact>]
    member this.``uint16 test``() =
        let x = "0us"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (UINT16 0us)

    [<Fact>]
    member this.``int32 test``() =
        let x = "230"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INT32 230)

    [<Fact>]
    member this.``uint32 test``() =
        let x = "0u"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (UINT32 0u)


    [<Fact>]
    member this.``int64 test``() =
        let x = "0L"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INT64 0L)

    [<Fact>]
    member this.``uint64 test``() =
        let x = "0UL"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (UINT64 0UL)

    [<Fact>]
    member this.``nativeint test``() =
        let x = "0n"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INTPTR 0n)

    [<Fact>]
    member this.``unativeint test``() =
        let x = "0un"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (UINTPTR 0un)

    [<Fact>]
    member this.``single test``() =
        let x = "0.033333335f"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (SINGLE 0.033333335f)

    [<Fact>]
    member this.``float test``() =
        let x = "0.03333333333333333"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (DOUBLE 0.03333333333333333)

    [<Fact>]
    member this.``render decimal test``() =
        let x = "0M"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (DECIMAL 0M)

    [<Fact>]
    member this.``bigint test``() =
        let x = "0I"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (BIGINTEGER 0I)

    [<Fact>]
    member this.``0xFFuy test``() =
        let x = "0xFFuy"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (BYTE 0xFFuy)

    [<Fact>]
    member this.``0x0800s test``() =
        let x = "0x0800s"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INT16 0x0800s)

    [<Fact>]
    member this.``0b0001 test``() =
        let x = "0b0001"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INT32 0b0001)

    [<Fact>]
    member this.``-19n test``() =
        let x = "-19n"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (INTPTR -19n)

    [<Fact>]
    member this.``null test``() =
        let x = "null"
        let y = JsonTokenizer.tokenize x |> Seq.exactlyOne
        Should.equal y (NULL)

    [<Fact>]
    member this.``ToStringEnsureEscapedArrayLength test``() =
        let nonAsciiChar = char 257
        let escapableNonQuoteAsciiChar = '\u0000'

        let value = sprintf "%c%s%c" nonAsciiChar @"\" escapableNonQuoteAsciiChar

        let convertedValue = StringUtils.toStringLiteral(value)
        output.WriteLine(convertedValue)
        let expect = "\"" + String[|nonAsciiChar|] + @"\\\u0000"""

        Should.equal expect convertedValue

    [<Fact>]
    member this.``StringEscaping test``() =
        let v = "It's a good day\r\n\"sunshine\""
        let json = StringUtils.toStringLiteral(v)
        Should.equal @"""It's a good day\r\n\""sunshine\""""" json
