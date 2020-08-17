namespace FSharp.Literals.Test

open Xunit
open Xunit.Abstractions

open System

type IEqualityComparerTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``float equals``() =
        let x = 4./3.
        let y = 1.3333333333333333 // 16个3
        Assert.Equal(x,y) //小数点后16位相等

        //output.WriteLine(x100.ToString())
        //let x100len = x100.ToString().Length - 1
        //Assert.Equal(x100len,15) //双精度浮点数的打印结果保留15位有效数字

        let p = x.ToString()

        //output.WriteLine(p)

        let y = System.Double.Parse(p)

        let dx = (x - y)
        let logdx = log10 dx
        let move = floor logdx

        output.WriteLine(sprintf "log10 dx = %f" logdx)
        output.WriteLine(sprintf "floor logdx = %f" move)

        let s = dx.ToString() //第16，17位有效数字
        output.WriteLine(s)

        //let dx = decimal x
        //let dxs = dx.ToString()
        //let dxsl = dxs.Length - 2
        //output.WriteLine(dxsl.ToString())

        //Assert.Equal(x.ToString(),x.ToString("0.0#######################################"))
        //let y14 = System.Math.Round(x,14)
        //let y15 = System.Math.Round(x,15)
        //let len = x.ToString().Length - 2
        //Assert.Equal(len,14)

        //Assert.Equal(x,y14)
        //Assert.Equal(x,y15)

        //Assert.Equal(x,y14, Should.floatEqualityComparer)

    [<Fact>]
    member this.``float 打印值圆整``() =
        let x = -1.3333333333333333 // 16个3
        let y = -3.99999999999999 //14*9
        let z = -3.999999999999999 //15*9

        output.WriteLine(x.ToString())
        output.WriteLine(y.ToString())
        output.WriteLine(z.ToString()) //打印值会圆整，误差将有负数

        //原值>0,误差>0,末尾追加数字
        // 1.33 - 1.3 = 0.03

        //原值<0,误差<0, 末尾追加数字
        // -1.33 - -1.3 = -0.03

    [<Fact>]
    member this.``float round-trip``() =
        let x = 4./3.

        let s = x.ToString("R",Globalization.CultureInfo.InvariantCulture)
        let l = s.Length-1
        Assert.Equal(l,17)
        let y = Double.Parse s
        Assert.Equal(x,y)
        output.WriteLine(s)
