# FSharp.Literals

This code uses .NET and F# reflection to walk the structure of values so as to build a formatted representation of the value. You format structural types such as lists and tuples using the syntax of F# source code. Unrecognized values are formatted by calling the F# `sprintf "%A"` method for these values.

### Installation Instructions

The recommended way to get `FSharp.Literals` is to use NuGet. The following packages are provided and maintained in the public NuGet Gallery.

### Get Started

the basic usage:

```F#
open FSharp.Literals
Render.stringify ([1], [true])
```

result: 

```F#
[1], [true]
```

