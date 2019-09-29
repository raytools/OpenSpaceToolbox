using System.IO;
using System.Reflection;

namespace OpenSpaceCore.DataModels
{
    public static class ProgramPaths
    {
        public static string ExePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string Games = Path.Combine(ExePath, "Games");
        public static string Bookmarks = Path.Combine(ExePath, "Bookmarks");
    }
}