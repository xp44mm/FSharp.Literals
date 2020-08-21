# FSharp.Literals

This code uses .NET and F# reflection to walk the structure of values so as to build a formatted representation of the value. You format structural types such as lists and tuples using the syntax of F# source code. Unrecognized values are formatted by calling the F# `sprintf "%A"` method for these values.


## Getting FSharp.Literals over NuGet

The simplest way of integrating FSharp.Literals to your project is by using NuGet. You can install FSharp.Literals by opening the package manager console (PM) and typing in the following statement:

```
Install-Package FSharp.Literals
```

You can also use the graphical library package manager ("Manage NuGet Packages for Solution"). Searching for "FSharp.Literals" in the official NuGet online feed will find this library.

## Get Started

The basic usage:

```F#
open FSharp.Literals
Render.stringify ([1], [true])
```

result: 

```F#
[1], [true]
```

When instance is about `Nullable<'t>`, directly or indirectly, you should use more special method:

```F#
let obj = box (Nullable 20)
Render.stringifyNullableType (typeof<Nullable<int>>) obj
```

