# FSharp.Literals

To print F# types and values using the syntax of F# source code.


## Getting FSharp.Literals over NuGet

The simplest way of integrating `FSharp.Literals` to your project is by using NuGet. You can install library by opening the package manager console (PM) and typing in the following statement:

```
Install-Package FSharp.Literals
```

You can also use the graphical library package manager ("Manage NuGet Packages for Solution"). Searching for "FSharp.Literals" in the official NuGet online feed will find this library.

## Get Started

### print value

The basic usage is `Render.stringify<'t> (obj:'t)`:

```F#
open FSharp.Literals
Render.stringify ([1], [true])
```

result is F# source code, it can be pasted directly into F# files.

```F#
[1], [true]
```

This code uses .NET and F# reflection to walk the structure of values so as to build a formatted representation of the value. You format structural types such as lists and tuples using the syntax of F# source code. Unrecognized values are formatted by calling the F# `sprintf "%A"` method for these values.

### print type

The basic usage is `Render.printTypeObj (ty:Type)`:

```F#
open FSharp.Literals

let ty = typeof<(string*int)*(float*bool)>
let y = Render.printTypeObj ty
Should.equal y "(string*int)*(float*bool)"

```

You can also use shortcut methods same as this method:

```F#
let y = Render.printType<(string*int)*(float*bool)>
Should.equal y "(string*int)*(float*bool)"
```

to print F# types using the syntax of F# source code. In order to please the C#-programmers, it's all this `list<int>` style instead of `int list`. You can also re-implement `TypePrinter` interfaces to custom printed result in your styles.


> **Note**: `Should.equal` in library `FSharp.xUnit`.

> The JSON format is a common format for exchanging data. You can use the NuGet package `FSharpCompiler.Json` to serialize the F# data.