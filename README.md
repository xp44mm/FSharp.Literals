# FSharp.Literals

Generic Pretty-Printing

Some useful generic functions are available for generic formatting of values. The simplest way to access this functionality is to use the `%A` specifiers in printf format strings. Here is an example:

```F#
> sprintf "result = %A" ([1], [true]);;
val it : string = "result = ([1], [true])"
```

This code uses .NET and F# reflection to walk the structure of values so as to build a formatted representation of the value. You format structural types such as lists and tuples using the syntax of F# source code. Unrecognized values are formatted by calling the .NET `ToString()` method for these values.

### Get Started

```F#
open FSharp.Literals
Render.stringify ([1], [true])
```

result: 

```F#
[1], [true]
```