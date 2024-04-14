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
        return CanEmbed(path);
    }

    public static bool CanEmbed(string packagePath)
    {
        if (string.IsNullOrEmpty(packagePath))
        {
            return false;
        }

        string parentFolderName = Path.GetDirectoryName(packagePath);
        return string.CompareOrdinal(parentFolderName, "Packages") == 0;
    }

    public static void Embed(Object selectedPackageFolder)
    {
        var packageName = Path.GetFileName(AssetDatabase.GetAssetPath(selectedPackageFolder));

        Debug.Log($"Embedding package '{packageName}' into the project.");
        
        Client.Embed(packageName);
        AssetDatabase.Refresh();
    }

    public static void Embed(string packagePath)
    {
        string packageName = Path.GetFileName(packagePath);
        Debug.Log($"Embedding package '{packageName}' into the project.");

        Client.Embed(packageName);
        AssetDatabase.Refresh();
    }
}