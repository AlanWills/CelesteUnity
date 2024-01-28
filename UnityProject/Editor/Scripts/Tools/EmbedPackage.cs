using System.IO;
using UnityEditor.PackageManager;
using UnityEditor;
using UnityEngine;

public static class EmbedPackage
{
    public static bool CanEmbed(Object selectedObjectFolder)
    {
        if (selectedObjectFolder == null)
        {
            return false;
        }

        var path = AssetDatabase.GetAssetPath(selectedObjectFolder);
        var folder = Path.GetDirectoryName(path);

        // We only deal with direct folders under Packages/
        return folder == "Packages";
    }

    public static void Embed(Object selectedPackageFolder)
    {
        var packageName = Path.GetFileName(AssetDatabase.GetAssetPath(selectedPackageFolder));

        Debug.Log($"Embedding package '{packageName}' into the project.");

        Client.Embed(packageName);
        AssetDatabase.Refresh();
    }
}