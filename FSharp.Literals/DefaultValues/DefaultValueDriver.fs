module FSharp.Literals.DefaultValues.DefaultValueDriver

open FSharp.Literals.DefaultValues
open System


/// 主函数
let rec defaultValue (providers: seq<Type -> ((Type -> obj) -> obj) option>) (ty: Type) =
    let action =
        providers
        |> Seq.tryPick (fun g -> g ty)
        |> Option.defaultValue (fun (loop: Type -> obj) -> DefaultValues.fallback ty)
    action (defaultValue providers)
