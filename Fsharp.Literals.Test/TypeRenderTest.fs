﻿namespace FSharp.Literals

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open Microsoft.FSharp.Reflection

type TypeRenderTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``simple type test``() =
        let ty = typeof<bool>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "bool"

    [<Fact>]
    member this.``enum type test``() =
        let ty = typeof<DateTimeKind>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "DateTimeKind"

    [<Fact>]
    member this.``else type test``() =
        let ty = typeof<Guid>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Guid"

    [<Fact>]
    member this.``array type test``() =
        let ty = typeof<int[]>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "int[]"

    [<Fact>]
    member this.``tuple array type test``() =
        let ty = typeof<(string*int)[]>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "(string*int)[]"


    [<Fact>]
    member this.``tuple type test``() =
        let ty = typeof<int*string>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "int*string"

    [<Fact>]
    member this.``nested tuple type test``() =
        let ty = typeof<(string*int)*(float*bool)>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "(string*int)*(float*bool)"


    [<Fact>]
    member this.``anonymuous type test``() =
        let ty = typeof<{|x:string|}>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "{|x:string|}"

    [<Fact>]
    member this.``generic type test``() =
        let ty = typeof<Nullable<int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Nullable<int>"

    [<Fact>]
    member this.``generic type definition test``() =
        let ty = typedefof<Nullable<_>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Nullable<'T>"

    [<Fact>]
    member this.``list type test``() =
        let ty = typeof<list<int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "list<int>"

    [<Fact>]
    member this.``Set type test``() =
        let ty = typeof<Set<int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Set<int>"

    [<Fact>]
    member this.``Map type test``() =
        let ty = typeof<Map<int,int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Map<int,int>"

    [<Fact>]
    member this.``Seq type test``() =
        let ty = typeof<seq<int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "seq<int>"

    [<Fact>]
    member this.``ResizeArray type test``() =
        let ty = typeof<ResizeArray<int>>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "ResizeArray<int>"

    [<Fact>]
    member this.``function type test``() =
        let ty = typeof<Type -> string>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "Type->string"

    [<Fact>]
    member this.``unit function type test``() =
        let ty = typeof<unit -> unit>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "unit->unit"

    [<Fact>]
    member this.``nested function type test``() =
        let ty = typeof<(Type->string)->(bool->float)>
        let y = Render.stringifyTypeDynamic ty
        should.equal y "(Type->string)->bool->float"
