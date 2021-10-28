﻿namespace FSharp.Literals.DefaultValues

//namespace FSharpCompiler.Json

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type Person = { name : string; age : int }

type RecordTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``record test``() =
        let x = { name = ""; age = 0 }
        let y = DefaultValueDriver.defaultValue [RecordDefaultValue.getDefault] typeof<Person> :?> Person
        should.equal x y 

    [<Fact>]
    member this.``anonymous record test``() =
        let x = {| name = ""; age = 0 |}
        let y = DefaultValueDriver.defaultValue [RecordDefaultValue.getDefault] typeof<{| name : string; age : int |}> :?> {| name : string; age : int |}
        should.equal x y 
