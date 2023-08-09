using System.IO;
using UnityEngine;

namespace BadAppleSkulls.Util
{
    public static class PathsUtil
    {
        public static readonly string UltrakillPath =
            Directory.GetParent(Application.dataPath)?.FullName ?? FallbackUltrakillPath;
        private const string FallbackUltrakillPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\ULTRAKILL";
        
        public static readonly DirectoryInfo BadAppleFolder = new DirectoryInfo(Path.Combine(UltrakillPath, "badapple"));
    }
}