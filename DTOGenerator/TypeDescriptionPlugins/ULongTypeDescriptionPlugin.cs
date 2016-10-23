using System;
using GenerationLibrary.TypeAssistance;

namespace TypeDescriptionPlugins
{
    public class ULongTypeDescriptionPlugin : ITypeDescription
    {
        public string TypeName { get; set; } = "integer";
        public string FormatName { get; set; } = "uint64";
        public Type NetType { get; set; } = typeof(ulong);
    }
}
