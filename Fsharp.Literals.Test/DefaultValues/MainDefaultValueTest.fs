namespace FSharp.Literals.DefaultValues

open Xunit
open Xunit.Abstractions
open FSharp.Literals

type MainDefaultValueTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``char dynamic test``() =
        let x = typeof<char>
        let y = Literal.defaultValueDynamic x :?> char
        should.equal y '\u0000'

    [<Fact>]
    member this.``char test``() =
        let x = typeof<char>
        let y = Literal.defaultValue<char>
        should.equal y '\u0000'

