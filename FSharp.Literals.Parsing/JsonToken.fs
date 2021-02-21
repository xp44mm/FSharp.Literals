namespace FSharp.Literals.Parsing

open System
open System.Numerics

type JsonToken = 
| COMMA
| COLON
| RIGHT_BRACK
| LEFT_BRACK
| RIGHT_BRACE
| LEFT_BRACE
| NULL
| FALSE
| TRUE
| STRING     of string
| CHAR       of char
| SBYTE      of SByte
| BYTE       of Byte
| INT16      of Int16
| INT32      of Int32
| INT64      of Int64
| INTPTR     of IntPtr
| UINT16     of UInt16
| UINT32     of UInt32
| UINT64     of UInt64
| UINTPTR    of UIntPtr
| BIGINTEGER of BigInteger
| SINGLE     of Single
| DOUBLE     of Double
| DECIMAL    of Decimal

    member this.tag = 
        match this with
        | COMMA       -> ","
        | COLON       -> ":"
        | LEFT_BRACK  -> "["
        | RIGHT_BRACK -> "]"
        | LEFT_BRACE  -> "{"
        | RIGHT_BRACE -> "}"
        | NULL        -> "null"
        | FALSE       -> "false"
        | TRUE        -> "true"
        | STRING      _ -> "STRING"
        | CHAR        _ -> "CHAR"
        | SBYTE       _ -> "SBYTE"
        | BYTE        _ -> "BYTE"
        | INT16       _ -> "INT16"
        | INT32       _ -> "INT32"
        | INT64       _ -> "INT64"
        | INTPTR      _ -> "INTPTR"
        | UINT16      _ -> "UINT16"
        | UINT32      _ -> "UINT32"
        | UINT64      _ -> "UINT64"
        | UINTPTR     _ -> "UINTPTR"
        | BIGINTEGER  _ -> "BIGINTEGER"
        | SINGLE      _ -> "SINGLE"
        | DOUBLE      _ -> "DOUBLE"
        | DECIMAL     _ -> "DECIMAL"
    