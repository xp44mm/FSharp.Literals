namespace FSharp.Literals.Test

open Xunit
open Xunit.Abstractions
open System
open System.Reflection
open System.IO
open System.Text.RegularExpressions
open FSharp.Literals

type RenderTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``render sbyte test``() =
        let x = 0y
        let y = ParenRender.stringify x
        Assert.Equal("0y",y)

    [<Fact>]
    member this.``render byte test``() =
        let x = 0uy
        let y = ParenRender.stringify x
        Assert.Equal("0uy",y)

    [<Fact>]
    member this.``render int16 test``() =
        let x = 0s
        let y = ParenRender.stringify x
        Assert.Equal("0s",y)

    [<Fact>]
    member this.``render uint16 test``() =
        let x = 0us
        let y = ParenRender.stringify x
        Assert.Equal("0us",y)

    [<Fact>]
    member this.``render int test``() =
        let x = 0
        let y = ParenRender.stringify x
        Assert.Equal("0",y)

    [<Fact>]
    member this.``render uint32 test``() =
        let x = 0u
        let y = ParenRender.stringify x
        Assert.Equal("0u",y)

    [<Fact>]
    member this.``render int64 test``() =
        let x = 0L
        let y = ParenRender.stringify x
        Assert.Equal("0L",y)

    [<Fact>]
    member this.``render uint64 test``() =
        let x = 0UL
        let y = ParenRender.stringify x
        Assert.Equal("0UL",y)

    [<Fact>]
    member this.``render nativeint test``() =
        let x = 0n
        let y = ParenRender.stringify x
        Assert.Equal("0n",y)

    [<Fact>]
    member this.``render unativeint test``() =
        let x = 0un
        let y = ParenRender.stringify x
        Assert.Equal("0un",y)

    [<Fact>]
    member this.``render single test``() =
        let x = (0.1f/3.0f)
        let y = ParenRender.stringify x
        Assert.Equal("0.033333335f",y)

        let x1 = 1.2f
        let y1 = ParenRender.stringify x1
        Assert.Equal("1.2f",y1)

        let x0 = 1.0f
        let y0 = ParenRender.stringify x0
        Assert.Equal("1.0f",y0)

    [<Fact>]
    member this.``render float test``() =
        let x = (0.1/3.)
        let y = ParenRender.stringify x
        Assert.Equal("0.03333333333333333",y)

        let x1 = 1.2
        let y1 = ParenRender.stringify x1
        Assert.Equal("1.2",y1)

        let x0 = 1.0
        let y0 = ParenRender.stringify x0
        Assert.Equal("1.0",y0)


    [<Fact>]
    member this.``render decimal test``() =
        let x = 0M
        let y = ParenRender.stringify x
        Assert.Equal("0M",y)

    [<Fact>]
    member this.``render bigint test``() =
        let x = 0I
        let y = ParenRender.stringify x
        Assert.Equal("0I",y)

    [<Fact>]
    member this.``render char test``() =
        let ls = '\t'
        let res = ParenRender.instanceToString 0 typeof<char> ls
        Assert.Equal(@"'\t'",res)

    [<Fact>]
    member this.``render DateTimeOffset test``() =
        let dto = DateTimeOffset(2019,9,19,15,18,16,757,TimeSpan(0,8,0,0,0)) //DateTimeOffset.Now
        let res = ParenRender.instanceToString 0 typeof<DateTimeOffset> dto
        Assert.Equal("DateTimeOffset(2019,9,19,15,18,16,757,TimeSpan(0,8,0,0,0))",res)

    [<Fact>]
    member this.``render nullable test``() =
        let x = Nullable(3)
        let ni = Nullable()

        let ls = [x;ni]

        let resx = Render.stringifyNullableType typeof<Nullable<int>> x
        let resn = Render.stringifyNullableType typeof<Nullable<_>> ni
        let resl = Render.stringifyNullableType typeof<Nullable<int> list> ls

        Assert.Equal("Nullable 3",resx)
        Assert.Equal("Nullable()",resn)
        Assert.Equal("[Nullable 3;Nullable()]",resl)

    [<Fact>]
    member this.``render array test``() =
        let ls = [|1;2;3|]
        let res = ParenRender.stringify ls
        Assert.Equal("[|1;2;3|]",res)

    [<Fact>]
    member this.``render list test``() =
        let ls = [1;2;3]
        let res = ParenRender.instanceToString 0 typeof<List<int>> ls
        Assert.Equal("[1;2;3]",res)

    [<Fact>]
    member this.``render set test``() =
        let ls = Set.ofList [1;2;3]
        let res = ParenRender.stringify ls
        Assert.Equal("set [1;2;3]",res)

    [<Fact>]
    member this.``render map test``() =
        let ls = Map.ofList ["1",1;"2",2;"3", 3]
        let res = ParenRender.stringify ls

        Assert.Equal("Map.ofList [\"1\",1;\"2\",2;\"3\",3]",res)

    [<Fact>]
    member this.``render tuple test``() =
        let ls = ([1;2;3],"x")
        let res = ParenRender.instanceToString 0 typeof<int list * string> ls
        output.WriteLine(res)

    [<Fact>]
    member this.``render null test``() =
        let ls = null
        let res = ParenRender.instanceToString 0 typeof<_> ls
        Assert.Equal("null",res)

    [<Fact>]
    member this.``render enum test``() =
        let e = FileMode.Open
        let res = ParenRender.instanceToString 0 typeof<FileMode> e
        output.WriteLine(res)

    [<Fact>]
    member this.``render flags enum test``() =
        let flags = BindingFlags.Public ||| BindingFlags.NonPublic

        let res = ParenRender.instanceToString 0 typeof<BindingFlags> flags
        output.WriteLine(res)

    [<Fact>]
    member this.``render flags none enum test``() =
        let none = RegexOptions.None
        let res = ParenRender.instanceToString 0 typeof<RegexOptions> none
        output.WriteLine(res)

    [<Fact>]
    member this.``render some test``() =
        let flags = Some 123
        let res = ParenRender.instanceToString 0 typeof<int option> flags
        Assert.Equal("Some 123",res)

    [<Fact>]
    member this.``render record test``() =
        let record = {| name = "xyz"; ``your age`` = 18 |}
        let res = ParenRender.stringify record
        output.WriteLine(res)
        Assert.Equal("""{name="xyz";``your age``=18}""",res)


    [<Fact>]
    member this.``render elasticAll test``() =
        let arr = [| ("10",([-50.0;20.0;100.0;200.0;250.0;260.0;280.0;300.0;320.0;340.0;350.0;360.0;380.0;400.0;410.0;420.0;430.0;440.0;450.0],[198.0;198.0;191.0;181.0;176.0;175.0;173.0;171.0;168.0;166.0;164.0;163.0;160.0;157.0;156.0;155.0;155.0;154.0;153.0])); ("20",([-50.0;20.0;100.0;200.0;250.0;260.0;280.0;300.0;320.0;340.0;350.0;360.0;380.0;400.0;410.0;420.0;430.0;440.0;450.0;460.0;470.0;480.0],[198.0;198.0;183.0;175.0;171.0;170.0;168.0;166.0;165.0;163.0;162.0;161.0;159.0;158.0;155.0;153.0;151.0;148.0;146.0;144.0;141.0;129.0])); ("Q235",([-50.0;20.0;100.0;200.0;250.0;260.0;280.0;300.0;350.0;400.0],[206.0;206.0;200.0;192.0;188.0;187.0;186.0;184.0;170.0;160.0])); ("Q345",([-50.0;20.0;100.0;200.0;250.0;260.0;280.0;300.0;320.0;340.0;350.0;360.0;380.0;400.0;450.0],[206.0;206.0;200.0;189.0;185.0;184.0;183.0;181.0;179.0;177.0;176.0;175.0;173.0;171.0;160.0])) |];

        let e = snd arr.[0]
        let ls = fst e
        let elasticAll = Map.ofArray arr
        
        let res = Render.stringify ls
        res |> output.WriteLine

        ////Assert.Equal("Some 123",res)
