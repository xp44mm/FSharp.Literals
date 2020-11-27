namespace FSharp.Literals.Test

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type DiscriminatedUnionTest(output : ITestOutputHelper) =
    let show res = 
        res 
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``test getTag``() =
        let du = Some 0
        let y = DiscriminatedUnion.getTag du
        let res = "Some"
        Should.equal y res

    [<Fact>]
    member this.``test unionReader``() =
        let du = Some 0
        let y = DiscriminatedUnion.unionReader (du.GetType()) du
        let res = "Some",[|typeof<int>,box 0|]
        Should.equal y res
