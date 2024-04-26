#if UNITY_IOS
using Celeste.Application;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace CelesteEditor.Application
{
	public static class AddAlternativeIconsToAppPostProcessStep
	{
		#region Properties and Fields

		private const string k_contentsJsonText = @"{
  ""images"" : [
	iPhoneContents
	iPadContents
	{
	  ""filename"" : ""appStore1024px.png"",
	  ""idiom"" : ""ios-marketing"",
	  ""scale"" : ""1x"",
	  ""size"" : ""1024x1024""
	}
  ],
  ""info"" : {
    ""author"" : ""xcode"",
    ""version"" : 1
  }
}
";

		private const string k_contentsiPhoneJsonText = @"{
	  ""filename"" : ""iPhoneNotification40px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPhoneNotification60px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPhoneSettings58px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPhoneSettings87px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPhoneSpotlight80px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPhoneSpotlight120px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPhoneApp120px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""2x"",
	  ""size"" : ""60x60""
	},
	{
	  ""filename"" : ""iPhoneApp180px.png"",
	  ""idiom"" : ""iphone"",
	  ""scale"" : ""3x"",
	  ""size"" : ""60x60""
	},
";

		private const string k_contentsiPadJsonText = @"{
	  ""filename"" : ""iPadNotifications20px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPadNotifications40px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""20x20""
	},
	{
	  ""filename"" : ""iPadSettings29px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPadSettings58px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""29x29""
	},
	{
	  ""filename"" : ""iPadSpotlight40px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPadSpotlight80px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""40x40""
	},
	{
	  ""filename"" : ""iPadApp76px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""1x"",
	  ""size"" : ""76x76""
	},
	{
	  ""filename"" : ""iPadApp152px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""76x76""
	},
	{
	  ""filename"" : ""iPadProApp167px.png"",
	  ""idiom"" : ""ipad"",
	  ""scale"" : ""2x"",
	  ""size"" : ""83.5x83.5""
	},";

		private const string k_includeAllAppIconAssets = "ASSETCATALOG_COMPILER_INCLUDE_ALL_APPICON_ASSETS";
		private const string k_alternateAppIconNames = "ASSETCATALOG_COMPILER_ALTERNATE_APPICON_NAMES";

		#endregion

		[PostProcessBuild]
		public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
		{
			if (target != BuildTarget.iOS)
			{
				return;
			}

			AlternativeAppIcon[] alternativeAppIcons = AssetDatabase.FindAssets($"t:{nameof(AlternativeAppIcon)}")
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<AlternativeAppIcon>)
				.ToArray();

			if (alternativeAppIcons.Length == 0)
			{
				return;
			}

			string imagesXcassetsDirectoryPath = Path.Combine(pathToBuiltProject, "Unity-iPhone", "Images.xcassets");
			List<string> iconNames = new List<string>();

			foreach (AlternativeAppIcon alternativeAppIcon in alternativeAppIcons)
			{
				iconNames.Add(alternativeAppIcon.IconName);
				var iconDirectoryPath = Path.Combine(imagesXcassetsDirectoryPath, $"{alternativeAppIcon.IconName}.appiconset");
				Directory.CreateDirectory(iconDirectoryPath);

				string contentsJsonPath = Path.Combine(iconDirectoryPath, "Contents.json");
				string contentsJson = k_contentsJsonText;
				contentsJson = contentsJson.Replace("iPhoneContents", PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad ? k_contentsiPhoneJsonText : string.Empty);
				contentsJson = contentsJson.Replace("iPadContents", PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPadOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad ? k_contentsiPadJsonText : string.Empty);
				File.WriteAllText(contentsJsonPath, contentsJson, Encoding.UTF8);

				bool copyIphoneIcons = PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad;
				bool copyIpadIcons = PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPadOnly || PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneAndiPad;
                alternativeAppIcon.CopyIcons(iconDirectoryPath, copyIphoneIcons, copyIpadIcons);
			}

			var pbxProjectPath = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj", "project.pbxproj");
			var pbxProject = new PBXProject();
			pbxProject.ReadFromFile(pbxProjectPath);

			var targetGuid = pbxProject.GetUnityMainTargetGuid();
			pbxProject.SetBuildProperty(targetGuid, k_includeAllAppIconAssets, "YES");

			var joinedIconNames = string.Join(" ", iconNames);
			pbxProject.SetBuildProperty(targetGuid, k_alternateAppIconNames, joinedIconNames);

			pbxProject.WriteToFile(pbxProjectPath);
		}
	}
}
#endif