value:"{" fields "}"
	| "[" values "]"
	| STRING
	| NULL
	| FALSE
	| TRUE
	| CHAR
	| SBYTE
	| BYTE
	| INT16
	| INT32
	| INT64
	| INTPTR
	| UINT16
	| UINT32
	| UINT64
	| UINTPTR
	| BIGINTEGER
	| SINGLE
	| DOUBLE
	| DECIMAL
	;

fields: fields "," field
	| field
	|
	;

field: STRING ":" value
	;

values: values "," value
	| value
	|
	;


