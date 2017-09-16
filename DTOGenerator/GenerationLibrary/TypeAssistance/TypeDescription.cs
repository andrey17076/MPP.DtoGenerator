using System;

namespace GenerationLibrary.TypeAssistance
{
    public class TypeDescription : ITypeDescription
    {

        public TypeDescription(string typeName, string format, Type netType)
        {
            TypeName = typeName;
            FormatName = format;
            NetType = netType;
        }

        public string TypeName { get; set; }
        public string FormatName { get; set; }
        public Type NetType { get; set; }
    }
}
