using Celeste.Persistence;
using Celeste.Wallet;
using CelesteEditor.Persistence;
using UnityEditor;

namespace CelesteEditor.Wallet
{
    public static class CelesteWalletMenuItems
    {
        [MenuItem("Celeste/Save/Open Wallet Save", priority = 0)]
        public static void OpenWalletSaveMenuItem()
        {
            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Wallet Save", priority = 100)]
        public static void DeleteWalletSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(WalletManager.FILE_NAME);
        }
    }
}