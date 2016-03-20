using System;
using System.IO;

namespace Blueprint.VisualStudio
{
    class Utility
    {
        public static string MakePathRelative(string basePath, string path)
        {
            Uri pathUri = new Uri(Path.GetFullPath(path), UriKind.Absolute);
            Uri cwdUri = new Uri(basePath, UriKind.Absolute);

            return cwdUri.MakeRelativeUri(pathUri).ToString();
        }
    }
}
