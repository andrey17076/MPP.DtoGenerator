using System;
using System.Collections.Generic;

namespace GenerationLibrary.TypeAssistance
{
    public class TypeTable
    {
        private readonly Dictionary<string, Dictionary<string, Type>> _formats;
       
        private TypeTable()
        {
            _formats = new Dictionary<string, Dictionary<string, Type>>();
            InitWithBuildInTypes();
        }

        public static TypeTable Instance { get; } = new TypeTable();

        public void AddOrUpdate(ITypeDescription description)
        {
            Dictionary<string, Type> netTypes;
            if (_formats.TryGetValue(description.TypeName, out netTypes))
            {
                netTypes.Add(description.FormatName, description.NetType);
            }
            else
            {
                AddFormatWithNetType(description);
            }
        }

        public bool TryGetNetType(string type, string format, out Type netType)
        {
            Dictionary<string, Type> netTypes;
            if (_formats.TryGetValue(type, out netTypes))
                return netTypes.TryGetValue(format, out netType);
            netType = null;
            return false;
        }

        private void AddFormatWithNetType(ITypeDescription description)
        {
            var netTypes = new Dictionary<string, Type>
            {
                { description.FormatName, description.NetType }
            };
            _formats.Add(description.TypeName, netTypes);
        }

        private void InitWithBuildInTypes()
        {
            AddOrUpdate(new TypeDescription("integer", "int32", typeof(int)));
            AddOrUpdate(new TypeDescription("integer", "int64", typeof(long)));
            AddOrUpdate(new TypeDescription("number", "float", typeof(float)));
            AddOrUpdate(new TypeDescription("number", "double", typeof(double)));
            AddOrUpdate(new TypeDescription("string", "byte", typeof(byte)));
            AddOrUpdate(new TypeDescription("boolean", "", typeof(bool)));
            AddOrUpdate(new TypeDescription("string", "date", typeof(DateTime)));
            AddOrUpdate(new TypeDescription("string", "string", typeof(string)));
        }
    }
}
