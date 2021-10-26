﻿module FSharp.Literals.DefaultValueProviders.DefaultValueDriver

open System
open Microsoft.FSharp.Core
open System.Reflection

let defaultValueFallback (aty:Type) = 
    if   aty.Equals(typeof<bool>      ) then box false
    elif aty.Equals(typeof<sbyte>     ) then box 0y
    elif aty.Equals(typeof<int16>     ) then box 0s
    elif aty.Equals(typeof<int32>     ) then box 0
    elif aty.Equals(typeof<int64>     ) then box 0L
    elif aty.Equals(typeof<nativeint> ) then box 0n
    elif aty.Equals(typeof<byte>      ) then box 0uy
    elif aty.Equals(typeof<uint16>    ) then box 0us
    elif aty.Equals(typeof<char>      ) then box '\u0000'
    elif aty.Equals(typeof<uint32>    ) then box 0u
    elif aty.Equals(typeof<uint64>    ) then box 0UL
    elif aty.Equals(typeof<unativeint>) then box 0un
    elif aty.Equals(typeof<decimal>   ) then box 0M
    elif aty.Equals(typeof<float>     ) then box 0.0
    elif aty.Equals(typeof<float32>   ) then box 0.0f
    elif aty.Equals(typeof<bigint>    ) then box 0I
    elif aty.Equals(typeof<string>    ) then box ""
    else 
       try
           let pinfo = aty.GetProperty("Zero",BindingFlags.Static ||| BindingFlags.Public)
           pinfo.GetValue(null,null)
       with _ -> null

/// 主函数
let rec defaultValue (providers:#seq<DefaultValueProvider>) (ty:Type) =
    let action =
        providers
        |> Seq.tryFind(fun provider -> provider.filter ty)
        |> Option.map(fun x -> x.defaultValue)
        |> Option.defaultValue(fun(loop,ty) -> 
            defaultValueFallback ty
            )
    action(defaultValue providers, ty)
