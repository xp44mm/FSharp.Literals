module FSharp.Literals.Jsons.JsonCreation

open Compiler

let rec createValue = function
| Nonterminal("value",[Terminal(LEFT_BRACE);fields;Terminal(RIGHT_BRACE)]) ->
    fields
    |> createFields
    |> Map.ofList
    |> Json.Map
| Nonterminal("value",[Terminal(LEFT_BRACK);values;Terminal(RIGHT_BRACK)]) ->
    values
    |> createValues
    |> Array.ofList
    |> Array.rev
    |> Json.List
| Nonterminal("value",[Terminal(NULL)]) ->
    Json.Null
| Nonterminal("value",[Terminal(FALSE)]) ->
    Json.False
| Nonterminal("value",[Terminal(TRUE)]) ->
    Json.True
| Nonterminal("value",[Terminal(STRING s)]) ->
    Json.String s
| Nonterminal("value",[Terminal(CHAR s)]) ->
    Json.Char s
| Nonterminal("value",[Terminal(SBYTE x)]) ->
    Json.SByte x
| Nonterminal("value",[Terminal(BYTE x)]) ->
    Json.Byte x
| Nonterminal("value",[Terminal(INT16 x)]) ->
    Json.Int16 x
| Nonterminal("value",[Terminal(UINT16 x)]) ->
    Json.UInt16 x
| Nonterminal("value",[Terminal(INT32 x)]) ->
    Json.Int32 x
| Nonterminal("value",[Terminal(UINT32 x)]) ->
    Json.UInt32 x
| Nonterminal("value",[Terminal(INT64 x)]) ->
    Json.Int64 x
| Nonterminal("value",[Terminal(UINT64 x)]) ->
    Json.UInt64 x
| Nonterminal("value",[Terminal(INTPTR x)]) ->
    Json.IntPtr x
| Nonterminal("value",[Terminal(UINTPTR x)]) ->
    x
    |> Json.UIntPtr
| Nonterminal("value",[Terminal(BIGINTEGER x)]) ->
    x
    |> Json.BigInteger
| Nonterminal("value",[Terminal(SINGLE x)]) ->
    x
    |> Json.Single
| Nonterminal("value",[Terminal(DOUBLE x)]) ->
    x
    |> Json.Double
| Nonterminal("value",[Terminal(DECIMAL x)]) ->
    x
    |> Json.Decimal
| never -> failwithf "%A"  <| never.get_firstLevel()

and createFields = function
| Nonterminal("fields",[Nonterminal("fields",_) as ls; comma; field]) ->
    createField field :: createFields ls
| Nonterminal("fields",[field]) ->
    [createField field]
| Nonterminal("fields",[]) ->
    []
| never -> failwithf "%A"  <| never.get_firstLevel()

and createField = function
| Nonterminal("field",[Terminal(STRING s); colon; value]) ->
    (s, createValue value)
| never -> failwithf "%A"  <| never.get_firstLevel()

and createValues = function
| Nonterminal("values",[Nonterminal("values",_) as ls; comma; value]) ->
    createValue value :: createValues ls
| Nonterminal("values",[value]) ->
    [createValue value]
| Nonterminal("values",[]) ->
    []
| never -> failwithf "%A" <| never.get_firstLevel()
