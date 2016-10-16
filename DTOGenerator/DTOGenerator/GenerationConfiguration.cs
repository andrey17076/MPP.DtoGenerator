using System.Configuration;

namespace DTOGenerator
{
    internal static class GenerationConfiguration
    {
        internal static int TaskCount => int.Parse(ConfigurationManager.AppSettings["taskCount"]);
        internal static string NameSpaceName => ConfigurationManager.AppSettings["namespaceName"];
    }
}
