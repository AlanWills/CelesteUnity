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

        Debug.Log($"Selected object folder found!");
        var path = AssetDatabase.GetAssetPath(selectedObjectFolder);
        Debug.Log($"Selected object folder path: {path}");
        var folder = Path.GetDirectoryName(path);
        Debug.Log($"Selected object folder parent folder path: {folder}");

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