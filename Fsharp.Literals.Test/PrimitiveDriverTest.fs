namespace FSharp.Literals.Jsons.Tests

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals.Jsons

type PrimitiveDriverTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``char test``() =
        let json = (Json.Char '0')
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``sbyte test``() =
        let json = (Json.SByte 0y)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``byte test``() =
        let json = (Json.Byte 0uy)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``int16 test``() =
        let json = (Json.Int16 0s)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``uint16 test``() =
        let json = (Json.UInt16 0us)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``int32 test``() =
        let json = (Json.Int32 230)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``uint32 test``() =
        let json = (Json.UInt32 0u)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``int64 test``() =
        let json = (Json.Int64 0L)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``uint64 test``() =
        let json = (Json.UInt64 0UL)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``nativeint test``() =
        let json = (Json.IntPtr 0n)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``unativeint test``() =
        let json = (Json.UIntPtr 0un)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``single test``() =
        let json = (Json.Single 0.033333335f)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json


    [<Fact>]
    member this.``float test``() =
        let json = (Json.Double 0.03333333333333333)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``render decimal test``() =
        let json = (Json.Decimal 0M)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json

    [<Fact>]
    member this.``bigint test``() =
        let json = (Json.BigInteger 0I)
        let x = JsonRender.stringify json
        let y = JsonDriver.parse x
        Should.equal y json






