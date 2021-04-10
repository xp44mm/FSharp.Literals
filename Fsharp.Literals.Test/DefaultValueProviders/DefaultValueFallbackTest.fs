namespace FSharp.Literals.DefaultValueProviders

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open System.Reflection
open System.Text.RegularExpressions

type DefaultValueFallbackTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``char test``() =
        let x = typeof<char>
        let y = DefaultValueDriver.defaultValue [] x :?> char
        should.equal y '\u0000'

    [<Fact>]
    member this.``sbyte test``() =
        let x = 0y
        let y = DefaultValueDriver.defaultValue [] typeof<sbyte> :?> sbyte
        should.equal x y

    [<Fact>]
    member this.``byte test``() =
        let x = 0uy
        let y = DefaultValueDriver.defaultValue [] typeof<byte> :?> byte
        should.equal x y


    [<Fact>]
    member this.``int16 test``() =
        let x = 0s
        let y = DefaultValueDriver.defaultValue [] typeof<int16> :?> int16
        should.equal x y

    [<Fact>]
    member this.``uint16 test``() =
        let x = 0us
        let y = DefaultValueDriver.defaultValue [] typeof<uint16> :?> uint16
        should.equal x y

    [<Fact>]
    member this.``int32 test``() =
        let x = 0
        let y = DefaultValueDriver.defaultValue [] typeof<int32> :?> int32
        should.equal x y

    [<Fact>]
    member this.``uint32 test``() =
        let x = 0u
        let y = DefaultValueDriver.defaultValue [] typeof<uint32> :?> uint32
        should.equal x y

    [<Fact>]
    member this.``int64 test``() =
        let x = 0L
        let y = DefaultValueDriver.defaultValue [] typeof<int64> :?> int64
        should.equal x y

    [<Fact>]
    member this.``uint64 test``() =
        let x = 0UL
        let y = DefaultValueDriver.defaultValue [] typeof<uint64> :?> uint64
        should.equal x y

    [<Fact>]
    member this.``nativeint test``() =
        let x = 0n
        let y = DefaultValueDriver.defaultValue [] typeof<nativeint> :?> nativeint
        should.equal x y

    [<Fact>]
    member this.``unativeint test``() =
        let x = 0un
        let y = DefaultValueDriver.defaultValue [] typeof<unativeint> :?> unativeint
        should.equal x y

    [<Fact>]
    member this.``single test``() =
        let x = 0.0f
        let y = DefaultValueDriver.defaultValue [] typeof<single> :?> single
        should.equal x y

    [<Fact>]
    member this.``float test``() =
        let x = 0.0
        let y = DefaultValueDriver.defaultValue [] typeof<float> :?> float
        should.equal x y

    [<Fact>]
    member this.``decimal test``() =
        let x = 0M
        let y = DefaultValueDriver.defaultValue [] typeof<decimal> :?> decimal
        should.equal x y

    [<Fact>]
    member this.``bigint test``() =
        let x = 0I
        let y = DefaultValueDriver.defaultValue [] typeof<bigint> :?> bigint
        should.equal x y

    [<Fact>]
    member this.``string test``() =
        let x = ""
        let y = DefaultValueDriver.defaultValue [] typeof<string> :?> string
        should.equal x y

    [<Fact>]
    member this.``array test``() =
        let x:int[] = [||]
        let y = DefaultValueDriver.defaultValue [ArrayDefaultValueProvider.Singleton] typeof<int[]> :?> int[]
        should.equal x y

    [<Fact>]
    member this.``tuple test``() =
        let x = 0,0.0,""
        let y = DefaultValueDriver.defaultValue [TupleDefaultValueProvider.Singleton] typeof<int*float*string> :?> int*float*string
        should.equal x y

    [<Fact>]
    member this.``DBNull test``() =
        let x = DBNull.Value
        let y = DefaultValueDriver.defaultValue [DBNullDefaultValueProvider.Singleton] typeof<DBNull> :?> DBNull
        should.equal x y

    [<Fact>]
    member this.``Nullable test``() =
        let x = Nullable()
        let y = DefaultValueDriver.defaultValue [NullableDefaultValueProvider.Singleton] typeof<Nullable<int>> :?> Nullable<int>
        should.equal x y

    [<Fact>]
    member this.``flags test``() =
        let x = BindingFlags.Default
        let y = DefaultValueDriver.defaultValue [EnumDefaultValueProvider.Singleton] typeof<BindingFlags> :?> BindingFlags
        should.equal x y

    [<Fact>]
    member this.``enum test``() =
        let x = RegexOptions.None
        let y = DefaultValueDriver.defaultValue [EnumDefaultValueProvider.Singleton] typeof<RegexOptions> :?> RegexOptions
        should.equal x y

    [<Fact>]
    member this.``guid test``() =
        let y = DefaultValueDriver.defaultValue [GuidDefaultValueProvider.Singleton] typeof<Guid> :?> Guid
        Assert.IsType<Guid>(y)

    [<Fact>]
    member this.``uri test``() =
        let y = DefaultValueDriver.defaultValue [UriDefaultValueProvider.Singleton] typeof<Uri> :?> Uri
        Assert.IsType<Uri>(y)

    [<Fact>]
    member this.``datetimeoffset test``() =
        let y = DefaultValueDriver.defaultValue [DateTimeOffsetDefaultValueProvider.Singleton] typeof<DateTimeOffset> :?> DateTimeOffset
        Assert.IsType<DateTimeOffset>(y)

    [<Fact>]
    member this.``timespan test``() =
        let x = TimeSpan.Zero
        let y = DefaultValueDriver.defaultValue [TimeSpanDefaultValueProvider.Singleton] typeof<TimeSpan> :?> TimeSpan
        should.equal x y

    [<Fact>]
    member this.``option test``() =
        let x = None
        let y = DefaultValueDriver.defaultValue [OptionDefaultValueProvider.Singleton] typeof<int option> :?> int option
        should.equal x y

    [<Fact>]
    member this.``list test``() =
        let x = []
        let y = DefaultValueDriver.defaultValue [ListDefaultValueProvider.Singleton] typeof<int list> :?> int list
        should.equal x y

    [<Fact>]
    member this.``set test``() =
        let x = Set.empty
        let y = DefaultValueDriver.defaultValue [SetDefaultValueProvider.Singleton] typeof<int Set> :?> int Set
        should.equal x y

    [<Fact>]
    member this.``map test``() =
        let x = Map.empty
        let y = DefaultValueDriver.defaultValue [MapDefaultValueProvider.Singleton] typeof<Map<int,int>> :?> Map<int,int>
        should.equal x y


