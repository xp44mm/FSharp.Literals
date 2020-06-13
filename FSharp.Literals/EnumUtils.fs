module FSharp.Literals.EnumUtils

open System
open Microsoft.FSharp.Core.Operators

let isGenericZero (ty:Type) (flag:obj) =
    if ty = typeof<sbyte> then
        let flag = unbox<sbyte> flag
        flag = 0y
    elif ty = typeof<byte> then
        let flag = unbox<byte> flag
        flag = 0uy
    elif ty = typeof<int16> then
        let flag = unbox<int16> flag
        flag = 0s
    elif ty = typeof<uint16> then
        let flag = unbox<uint16> flag
        flag = 0us
    elif ty = typeof<int32> then
        let flag = unbox<int32> flag
        flag = 0
    elif ty = typeof<uint32> then
        let flag = unbox<uint32> flag
        flag = 0u
    elif ty = typeof<int64> then
        let flag = unbox<int64> flag
        flag = 0L
    elif ty = typeof<uint64> then
        let flag = unbox<uint64> flag
        flag = 0UL
    elif ty = typeof<bool> then // never
        let flag = unbox<bool> flag
        flag = false

    elif ty = typeof<char> then // never
        let flag = unbox<char> flag
        flag = char 0

    else
        failwithf "%A" ty

///
let genericMask (ty:Type) (inp:obj) (flag:obj) =
    if ty = typeof<sbyte> then
        let inp = unbox<sbyte> inp
        let flag = unbox<sbyte> flag
        inp &&& flag = flag
    elif ty = typeof<byte> then
        let inp = unbox<byte> inp
        let flag = unbox<byte> flag
        inp &&& flag = flag
    elif ty = typeof<int16> then
        let inp = unbox<int16> inp
        let flag = unbox<int16> flag
        inp &&& flag = flag
    elif ty = typeof<uint16> then
        let inp = unbox<uint16> inp
        let flag = unbox<uint16> flag
        inp &&& flag = flag

    elif ty = typeof<int32> then
        let inp = unbox<int32> inp
        let flag = unbox<int32> flag
        inp &&& flag = flag

    elif ty = typeof<uint32> then
        let inp = unbox<uint32> inp
        let flag = unbox<uint32> flag
        inp &&& flag = flag

    elif ty = typeof<int64> then
        let inp = unbox<int64> inp
        let flag = unbox<int64> flag
        inp &&& flag = flag

    elif ty = typeof<uint64> then
        let inp = unbox<uint64> inp
        let flag = unbox<uint64> flag
        inp &&& flag = flag

    elif ty = typeof<char> then // never
        let inp = unbox<char> inp |> byte
        let flag = unbox<char> flag |> byte
        inp &&& flag = flag

    elif ty = typeof<bool> then // never
        let inp = unbox<bool> inp |> Convert.ToByte
        let flag = unbox<bool> flag |> Convert.ToByte
        inp &&& flag = flag

    else
        failwith "Unknown Flags Enum Underlying Type."

/// from newtonsoft
let private toUInt64 (ty:Type) =
    if ty = typeof<sbyte> then
        unbox<sbyte> >> uint64
    elif ty = typeof<byte> then
        unbox<byte> >> uint64
    elif ty = typeof<int16> then
        unbox<int16> >> uint64
    elif ty = typeof<uint16> then
        unbox<uint16> >> uint64
    elif ty = typeof<char> then
        unbox<char> >> uint64
    elif ty = typeof<int32> then
        unbox<int32> >> uint64
    elif ty = typeof<uint32> then
        unbox<uint16> >> uint64
    elif ty = typeof<int64> then
        unbox<int64> >> uint64
    elif ty = typeof<bool> then
        unbox<bool> >> Convert.ToByte >> uint64
    elif ty = typeof<uint64> then
        unbox<uint64>
    else
        failwith "Unknown Enum Underlying Type."