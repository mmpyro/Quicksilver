using System;

namespace ConfigManager.Helpers
{
    public static class PathOsConverter
    {
        public static string ConvertPath(this string path)
        {
            string systemName = Environment.OSVersion.Platform.ToString();
            if (systemName.Equals("unix", StringComparison.OrdinalIgnoreCase))
            {
                return path.Replace(@"\", "/");
            }
            else
            {
                return path.Replace("/", @"\");
            }
        }
    }
}
