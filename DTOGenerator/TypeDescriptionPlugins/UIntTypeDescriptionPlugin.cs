using System;
using GenerationLibrary.TypeAssistance;

namespace TypeDescriptionPlugins
{
    public class UIntTypeDescriptionPlugin : ITypeDescription
    {
        public string TypeName { get; set; } = "integer";
        public string FormatName { get; set; } = "uint32";
        public Type NetType { get; set; } = typeof(uint);
    }
}
