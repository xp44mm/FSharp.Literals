[<AutoOpen>]
module AssertEquality

open FSharp.xUnit
 
//type JPropertyEqualityComparerAdapter() =
//    static member Singleton = JPropertyEqualityComparerAdapter() :> EqualityComparerAdapter
//    interface EqualityComparerAdapter with
//        member this.filter ty = ty = typeof<JProperty>
//        member this.getEqualityComparer(loop,ty) =
//            let stringComparer = loop typeof<string>
//            let jTokenEqualityComparer = JTokenEqualityComparer()
//            {
//                new IEqualityComparer with
//                    member this.Equals(p1,p2) =
//                        let p1 = unbox<JProperty> p1
//                        let p2 = unbox<JProperty> p2
//                        stringComparer.Equals(p1.Name, p2.Name) && jTokenEqualityComparer.Equals(p1.Value, p2.Value)
//                    member this.GetHashCode(p) = 
//                        let p = unbox<JProperty> p
//                        hash [|stringComparer.GetHashCode p.Name; jTokenEqualityComparer.GetHashCode p.Value|]
//            }
    

//type JTokenEqualityComparerAdapter() =
//    static member Singleton = JTokenEqualityComparerAdapter() :> EqualityComparerAdapter
//    interface EqualityComparerAdapter with
//        member this.filter ty = typeof<JToken>.IsAssignableFrom ty
//        member this.getEqualityComparer(loop,ty) =
//            let genericComp = JTokenEqualityComparer()
//            {
//                new IEqualityComparer with
//                    member this.Equals(p1,p2) = genericComp.Equals(unbox<JToken> p1,unbox<JToken> p2)
//                    member this.GetHashCode(p) = 
//                        genericComp.GetHashCode(unbox<JToken> p)
//            }

let should = EqualConfig.``override``[ 
    //JPropertyEqualityComparerAdapter.Singleton;
    //JTokenEqualityComparerAdapter.Singleton; 
    ]


