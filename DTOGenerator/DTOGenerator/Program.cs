using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using GenerationLibrary;
using GenerationLibrary.JsonDescriptions;
using GenerationLibrary.TypeAssistance;
using Newtonsoft.Json;

namespace DTOGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                LoadPlugins(args[2]);
                var dtoWriter = new DtoWriter(args[1]);
                var classDescriptionList = GetClassDescriptionList(args[0]);
                var dtoClassList = GenerateDtoClassList(classDescriptionList);
                dtoClassList.ForEach(dto => dtoWriter.Write(dto));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static List<DtoClass> GenerateDtoClassList(JsonClassDescriptionList classDescriptionList)
        {
            var generator = new Generator(
                GenerationConfiguration.TaskCount,
                GenerationConfiguration.NameSpaceName
            );
            return generator.GetDtoClassList(classDescriptionList);
        }

        private static void LoadPlugins(string pluginDirectory)
        {
            new TypeDescriptionLoader(pluginDirectory).LoadPlugins();
        }

        private static JsonClassDescriptionList GetClassDescriptionList(string jsonFilePath)
        {
            var jsonFileContent = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<JsonClassDescriptionList>(jsonFileContent);
        }
    }
}
