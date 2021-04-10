module FSharp.Literals.DefaultValueProviders.DefaultValueProviders

let providers = [
    DateTimeOffsetDefaultValueProvider.Singleton
    TimeSpanDefaultValueProvider.Singleton
    GuidDefaultValueProvider.Singleton
    UriDefaultValueProvider.Singleton
    EnumDefaultValueProvider.Singleton
    DBNullDefaultValueProvider.Singleton
    NullableDefaultValueProvider.Singleton
    ArrayDefaultValueProvider.Singleton
    TupleDefaultValueProvider.Singleton
    OptionDefaultValueProvider.Singleton
    ListDefaultValueProvider.Singleton
    SetDefaultValueProvider.Singleton
    MapDefaultValueProvider.Singleton
    UnionDefaultValueProvider.Singleton
    RecordDefaultValueProvider.Singleton
]