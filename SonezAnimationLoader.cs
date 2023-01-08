using Common;
using FezEngine.Tools;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sonez
{
    static class SonezAnimationLoader
    {

        public static void Load()
        {
            var cachedAssets = typeof(MemoryContentManager).GetField("cachedAssets", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, byte[]>;

            if (cachedAssets == null) return;

            const string assemblyName = "Sonez.Animations.";
            var assembly = Assembly.GetAssembly(typeof(SonezAnimationLoader));

            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (!resourceName.StartsWith(assemblyName)) continue;

                var assetName = "character animations\\gomez\\" 
                    + resourceName.Substring(assemblyName.Length).Replace(".xnb", "").Replace('.', '\\');

                
                Stream resourceStream = assembly.GetManifestResourceStream(resourceName);

                if (resourceStream == null) continue;

                byte[] resourceData = new byte[resourceStream.Length];
                resourceStream.Read(resourceData, 0, resourceData.Length);

                cachedAssets[assetName] = resourceData;
            }

            Logger.Log("Sonez", "Successfully injected Sonez assets.");
        }
    }
}
