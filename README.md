# FSharp.Literals

This code uses .NET and F# reflection to walk the structure of values so as to build a formatted representation of the value. You format structural types such as lists and tuples using the syntax of F# source code. Unrecognized values are formatted by calling the F# `sprintf "%A"` method for these values.


## Getting FSharp.Literals over NuGet

The simplest way of integrating FSharp.Literals to your project is by using NuGet. You can install FSharp.Literals by opening the package manager console (PM) and typing in the following statement:

```
Install-Package FSharp.Literals
```

You can also use the graphical library package manager ("Manage NuGet Packages for Solution"). Searching for "FSharp.Literals" in the official NuGet online feed will find this library.

## Get Started

The basic usage：

```F#
let code = ReadOnlyRecord.databaseDefinition connstr db_name
```

以上代码将会生成表字段组成的记录，并且有一个静态数组，装有数据库表内的所有数据。

有两个方法用于验证数据库文件是否过期：




