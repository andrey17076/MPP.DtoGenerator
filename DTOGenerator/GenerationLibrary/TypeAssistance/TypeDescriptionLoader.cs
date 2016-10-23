using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GenerationLibrary.TypeAssistance
{
    public class TypeDescriptionLoader
    {
        private readonly string _pluginDirectory;

        public TypeDescriptionLoader(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
        }

        public void LoadPlugins()
        {
            if (!Directory.Exists(_pluginDirectory)) return;
            Directory.EnumerateFiles(_pluginDirectory).ToList().ForEach(LoadAssembly);
        }

        private static void LoadAssembly(string file)
        {
            var assembly = Assembly.LoadFrom(file);
            GetExportedTypes(assembly).ForEach(LoadPlugin);
        }

        private static List<Type> GetExportedTypes(Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Where(type => type.GetInterface(typeof(ITypeDescription).Name) != null)
                .ToList();
        }

        private static void LoadPlugin(Type exportedType)
        {
            var typeDescription = (ITypeDescription) Activator.CreateInstance(exportedType);
            TypeTable.Instance.AddOrUpdate(typeDescription);
        }
    }
}
