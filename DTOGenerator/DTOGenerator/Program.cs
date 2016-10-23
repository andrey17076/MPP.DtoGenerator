using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var classDescriptionList = GetClassDescriptionList(args[0]);
                GenerateDtoClasses(classDescriptionList).ForEach(dto => dto.Write(args[1]));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        private static List<DtoClass> GenerateDtoClasses(JsonClassDescriptionList classDescriptionList)
        {
            var generator = new Generator(
                GenerationConfiguration.TaskCount, 
                GenerationConfiguration.NameSpaceName
            );
            return classDescriptionList.ClassDescriptions
                .Select(d => new DtoClass(d.ClassName, generator.GetDtoCode(d)))
                .ToList();
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
