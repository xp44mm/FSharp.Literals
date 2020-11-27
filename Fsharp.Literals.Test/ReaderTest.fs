namespace FSharp.Literals.Test

open Xunit
open Xunit.Abstractions

open System.Reflection
open Microsoft.FSharp.Reflection
open FSharp.Literals

type ReaderTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``get fsharp modules``() =
        let fsharpAssem = Assembly.Load("FSharp.Core")
        output.WriteLine(fsharpAssem.FullName)

        fsharpAssem.GetExportedTypes()
        |> Array.filter(fun ty -> FSharpType.IsModule ty)
        |> Array.map(fun ty -> ty.FullName)
        |> String.concat "\n"
        |> output.WriteLine


    [<Fact>]
    member this.``get ToArray``() =
        let fsharpAssembly = Assembly.Load("FSharp.Core")

        let setModuleType = fsharpAssembly.GetType("Microsoft.FSharp.Collections.SetModule")

        let ty = typeof<Set<int>>
        let elementType = ty.GenericTypeArguments.[0]
        let mToArray = (setModuleType.GetMethod "ToArray").MakeGenericMethod(elementType)

        let a = Set.ofList [1;2;3] |> box

        let b = mToArray.Invoke(null,[|a|])

        ParenRender.stringify b
        |> output.WriteLine

    [<Fact>]
    member this.``get map reader``() =
        let mToArrayDef = Readers.mapModuleType.GetMethod "ToArray"

        let ty = typeof<Map<string,int>>

        let typeArguments = ty.GenericTypeArguments

        let mToArray = mToArrayDef.MakeGenericMethod(typeArguments)

        let a = Map.ofList ["1",1;"2",2;"3", 3]

        let b = mToArray.Invoke(null,[|a|])

        ParenRender.stringify b
        |> output.WriteLine

    [<Fact>]
    member this.``get seq reader``() =
        let seqTypeDef = typeof<seq<_>>.GetGenericTypeDefinition()
        let giterorDef = typeof<System.Collections.Generic.IEnumerator<_>>.GetGenericTypeDefinition()
        let biteror = typeof<System.Collections.IEnumerator>
        let mMoveNext = biteror.GetMethod("MoveNext")

        //以上是反射方法的准备，下面是读取元素的过程
        let ls = seq [1;2;3]
        let ty = ls.GetType()
        let arguments = ty.GetGenericArguments()
        
        let seqType = seqTypeDef.MakeGenericType arguments
        let mGetEnumerator = seqType.GetMethod("GetEnumerator")

        let enumerator = mGetEnumerator.Invoke(ls,[||])
        let giteror = giterorDef.MakeGenericType arguments
        let pCurrent = giteror.GetProperty("Current")

        let arr =
            [|
                while(mMoveNext.Invoke(enumerator,[||])|>unbox<bool>) do
                    yield pCurrent.GetValue(enumerator)
            |]
        Render.stringify arr
        |> output.WriteLine

