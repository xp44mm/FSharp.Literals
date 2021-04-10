namespace FSharp.Literals.DefaultValueProviders

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UionExample0 =
| Zero

type UionExample1 =
| OnlyOne of int

type UionExample2 =
| Pair of int * string

type UnionTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``UionExample0``() =
        let x = Zero
        let y = DefaultValueDriver.defaultValue [UnionDefaultValueProvider.Singleton] typeof<UionExample0> :?> UionExample0
        should.equal x y 

    [<Fact>]
    member this.``UionExample1``() =
        let x = OnlyOne 0
        let y = DefaultValueDriver.defaultValue [UnionDefaultValueProvider.Singleton] typeof<UionExample1> :?> UionExample1
        should.equal x y 

    [<Fact>]
    member this.``UionExample2``() =
        let x = Pair (0,"")
        let y = DefaultValueDriver.defaultValue [UnionDefaultValueProvider.Singleton] typeof<UionExample2> :?> UionExample2
        should.equal x y 

