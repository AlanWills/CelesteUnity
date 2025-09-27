using Celeste;
using Celeste.Tools;
using Celeste.Web.Messages;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Web
{
    public static class CelesteWebEditorMenuItems
    {
        [MenuItem("Assets/Create/" + CelesteMenuItemConstants.WEB_MENU_ITEM + "Create Factories", true, CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
        public static bool ValidateCreateFactories()
        {
            return Selection.activeObject != null && 
                   (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject)) || !string.IsNullOrEmpty(EditorOnly.GetAssetFolderPath(Selection.activeObject)));
        }
        
        [MenuItem("Assets/Create/" + CelesteMenuItemConstants.WEB_MENU_ITEM + "Create Factories", false, CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
        public static void CreateFactories()
        {
            string activeObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string assetFolderPath = AssetDatabase.IsValidFolder(activeObjectPath) ? activeObjectPath : EditorOnly.GetAssetFolderPath(Selection.activeObject);
            
            // Serialization Factory
            {
                NetworkingMessageSerializationFactory deserializerFactory =
                    ScriptableObject.CreateInstance<NetworkingMessageSerializationFactory>();
                deserializerFactory.name = nameof(NetworkingMessageSerializationFactory);
                EditorOnly.CreateAssetInFolder(deserializerFactory, assetFolderPath);
            }

            // Client Message Handler Factory
            {
                NetworkingMessageHandlerFactory handlerFactory =
                    ScriptableObject.CreateInstance<NetworkingMessageHandlerFactory>();
                handlerFactory.name = $"Client{nameof(NetworkingMessageHandlerFactory)}";
                EditorOnly.CreateAssetInFolder(handlerFactory, assetFolderPath);
            }

            // Server Message Handler Factory
            {
                NetworkingMessageHandlerFactory handlerFactory =
                    ScriptableObject.CreateInstance<NetworkingMessageHandlerFactory>();
                handlerFactory.name = $"Server{nameof(NetworkingMessageHandlerFactory)}";
                EditorOnly.CreateAssetInFolder(handlerFactory, assetFolderPath);
            }
            
            AssetDatabase.Refresh();
        }
    }
}