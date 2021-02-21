namespace FSharp.Literals

open System
open System.Numerics

type PrimitiveType = 
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

