using System.Reflection;

namespace OpenSpaceCore.Helpers
{
    public static class AppInfo
    {
        public static string CallingName => Assembly.GetCallingAssembly().GetName().Name;

        public static string CallingVersion => Assembly.GetCallingAssembly().GetName().Version.ToString();
    }
}