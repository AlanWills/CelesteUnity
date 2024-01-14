using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [ScriptedImporter(1, "jenkinsfile")]
    public class JenkinsFileAssetImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string fileContents = File.ReadAllText(ctx.assetPath);
            TextAsset subAsset = new TextAsset(fileContents);
            ctx.AddObjectToAsset("main", subAsset);
            ctx.SetMainObject(subAsset);
        }
    }
}
