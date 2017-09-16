using System;

namespace GenerationLibrary.TypeAssistance
{
    public interface ITypeDescription
    {
        string TypeName { get; set; }
        string FormatName { get; set; }
        Type NetType {get; set; }
    }
}
