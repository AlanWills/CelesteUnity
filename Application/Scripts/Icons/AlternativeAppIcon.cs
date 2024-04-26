using Celeste.Tools;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Celeste.Application
{
    [CreateAssetMenu(fileName = nameof(AlternativeAppIcon), menuName = CelesteMenuItemConstants.APPLICATION_MENU_ITEM + "App Icons/Alternative App Icon", order = CelesteMenuItemConstants.APPLICATION_MENU_ITEM_PRIORITY)]
    public class AlternativeAppIcon : ScriptableObject
    {
        #region Properties and Fields

        public string IconName => iconName;
        public bool HasSource => source != null;

        [SerializeField, Tooltip("This is the name that will appear in App Store Connect when you run a campaign to customise the app icon.")] private string iconName;
        [SerializeField] private Texture2D source;
        [SerializeField] private Texture2D iPhoneNotification40px;
        [SerializeField] private Texture2D iPhoneNotification60px;
        [SerializeField] private Texture2D iPhoneSettings58px;
        [SerializeField] private Texture2D iPhoneSettings87px;
        [SerializeField] private Texture2D iPhoneSpotlight80px;
        [SerializeField] private Texture2D iPhoneSpotlight120px;
        [SerializeField] private Texture2D iPhoneApp120px;
        [SerializeField] private Texture2D iPhoneApp180px;
        [SerializeField] private Texture2D iPadNotifications20px;
        [SerializeField] private Texture2D iPadNotifications40px;
        [SerializeField] private Texture2D iPadSettings29px;
        [SerializeField] private Texture2D iPadSettings58px;
        [SerializeField] private Texture2D iPadSpotlight40px;
        [SerializeField] private Texture2D iPadSpotlight80px;
        [SerializeField] private Texture2D iPadApp76px;
        [SerializeField] private Texture2D iPadApp152px;
        [SerializeField] private Texture2D iPadProApp167px;
        [SerializeField] private Texture2D appStore1024px;

        #endregion

        [Conditional("UNITY_EDITOR")]
        public void GenerateAndSaveIcons()
        {
            if (source == null)
            {
                UnityEngine.Debug.LogError($"Attempting to try and generate icons for the alternative app icon {name} with no source texture set.");
                return;
            }

            iPhoneNotification40px = GenerateAndSaveIcon(nameof(iPhoneNotification40px), 40);
            iPhoneNotification60px = GenerateAndSaveIcon(nameof(iPhoneNotification60px), 60);
            iPhoneSettings58px = GenerateAndSaveIcon(nameof(iPhoneSettings58px), 58);
            iPhoneSettings87px = GenerateAndSaveIcon(nameof(iPhoneSettings87px), 87);
            iPhoneSpotlight80px = GenerateAndSaveIcon(nameof(iPhoneSpotlight80px), 80);
            iPhoneSpotlight120px = GenerateAndSaveIcon(nameof(iPhoneSpotlight120px), 120);
            iPhoneApp120px = GenerateAndSaveIcon(nameof(iPhoneApp120px), 120);
            iPhoneApp180px = GenerateAndSaveIcon(nameof(iPhoneApp180px), 180);
            iPadNotifications20px = GenerateAndSaveIcon(nameof(iPadNotifications20px), 20);
            iPadNotifications40px = GenerateAndSaveIcon(nameof(iPadNotifications40px), 40);
            iPadSettings29px = GenerateAndSaveIcon(nameof(iPadSettings29px), 29);
            iPadSettings58px = GenerateAndSaveIcon(nameof(iPadSettings58px), 58);
            iPadSpotlight40px = GenerateAndSaveIcon(nameof(iPadSpotlight40px), 40);
            iPadSpotlight80px = GenerateAndSaveIcon(nameof(iPadSpotlight80px), 80);
            iPadApp76px = GenerateAndSaveIcon(nameof(iPadApp76px), 76);
            iPadApp152px = GenerateAndSaveIcon(nameof(iPadApp152px), 152);
            iPadProApp167px = GenerateAndSaveIcon(nameof(iPadProApp167px), 167);
            appStore1024px = GenerateAndSaveIcon(nameof(appStore1024px), 1024);

            EditorOnly.SaveAsset(this);
        }

        [Conditional("UNITY_EDITOR")]
        public void CopyIcons(string targetDirectory, bool copyIphoneIcons, bool copyIpadIcons)
        {
            if (copyIphoneIcons)
            {
                UnityEngine.Debug.Log($"Copying iPhone icons for alternative app icon '{name}'.");
                CopyIcon(iPhoneNotification40px, Path.Combine(targetDirectory, $"{nameof(iPhoneNotification40px)}.png"));
                CopyIcon(iPhoneNotification60px, Path.Combine(targetDirectory, $"{nameof(iPhoneNotification60px)}.png"));
                CopyIcon(iPhoneSettings58px, Path.Combine(targetDirectory, $"{nameof(iPhoneSettings58px)}.png"));
                CopyIcon(iPhoneSettings87px, Path.Combine(targetDirectory, $"{nameof(iPhoneSettings87px)}.png"));
                CopyIcon(iPhoneSpotlight80px, Path.Combine(targetDirectory, $"{nameof(iPhoneSpotlight80px)}.png"));
                CopyIcon(iPhoneSpotlight120px, Path.Combine(targetDirectory, $"{nameof(iPhoneSpotlight120px)}.png"));
                CopyIcon(iPhoneApp120px, Path.Combine(targetDirectory, $"{nameof(iPhoneApp120px)}.png"));
                CopyIcon(iPhoneApp180px, Path.Combine(targetDirectory, $"{nameof(iPhoneApp180px)}.png"));
            }

            if (copyIpadIcons)
            {
                UnityEngine.Debug.Log($"Copying iPad icons for alternative app icon '{name}'.");
                CopyIcon(iPadNotifications20px, Path.Combine(targetDirectory, $"{nameof(iPadNotifications20px)}.png"));
                CopyIcon(iPadNotifications40px, Path.Combine(targetDirectory, $"{nameof(iPadNotifications40px)}.png"));
                CopyIcon(iPadSettings29px, Path.Combine(targetDirectory, $"{nameof(iPadSettings29px)}.png"));
                CopyIcon(iPadSettings58px, Path.Combine(targetDirectory, $"{nameof(iPadSettings58px)}.png"));
                CopyIcon(iPadSpotlight40px, Path.Combine(targetDirectory, $"{nameof(iPadSpotlight40px)}.png"));
                CopyIcon(iPadSpotlight80px, Path.Combine(targetDirectory, $"{nameof(iPadSpotlight80px)}.png"));
                CopyIcon(iPadApp76px, Path.Combine(targetDirectory, $"{nameof(iPadApp76px)}.png"));
                CopyIcon(iPadApp152px, Path.Combine(targetDirectory, $"{nameof(iPadApp152px)}.png"));
                CopyIcon(iPadProApp167px, Path.Combine(targetDirectory, $"{nameof(iPadProApp167px)}.png"));
            }

            CopyIcon(appStore1024px, Path.Combine(targetDirectory, $"{nameof(appStore1024px)}.png"));
        }

        private Texture2D GenerateAndSaveIcon(string generatedIconName, int size)
        {
#if UNITY_EDITOR
            string sourceAssetPath = UnityEditor.AssetDatabase.GetAssetPath(source);
            string sourceTextureDirectory = Path.GetDirectoryName(sourceAssetPath);

            Texture2D resizedTexture = new Texture2D(size, size);
            RenderTexture renderTexture = new RenderTexture(size, size, 24);
            RenderTexture tmpRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Graphics.Blit(source, renderTexture);
            resizedTexture.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            resizedTexture.Apply();
            RenderTexture.active = tmpRenderTexture;
            renderTexture.Release();

            string savePath = Path.Combine(sourceTextureDirectory, $"{generatedIconName}.png");
            var pngBytes = resizedTexture.EncodeToPNG();
            File.WriteAllBytes(savePath, pngBytes);
            UnityEditor.AssetDatabase.Refresh();

            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
#else
            return default;
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void CopyIcon(Texture2D manualTexture, string savePath)
        {
#if UNITY_EDITOR
            if (manualTexture != null)
            {
                var path = UnityEditor.AssetDatabase.GetAssetPath(manualTexture);
                File.Copy(path, savePath, true);
            }
#endif
        }
    }
}