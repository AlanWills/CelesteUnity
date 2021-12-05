using Celeste.Wallet;
using CelesteEditor.Scene;
using UnityEditor;

namespace CelesteEditor.Wallet
{
    public static class CelesteWalletMenuItems
    {
        [MenuItem("Celeste/Save/Open Wallet Save", priority = 0)]
        public static void OpenWalletSaveMenuItem()
        {
            MenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Wallet Save", priority = 100)]
        public static void DeleteWalletSaveMenuItem()
        {
            MenuItemUtility.DeletePersistentDataFile(WalletManager.FILE_NAME);
        }
    }
}