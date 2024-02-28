using Celeste.Components;
using Celeste.Tools.Attributes.GUI;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Prefab Actor")]
    [CreateAssetMenu(
        fileName = nameof(PrefabActorBoardGameObjectComponent), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Board Game Object Components/Prefab Actor",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class PrefabActorBoardGameObjectComponent : BoardGameObjectComponent, IBoardGameObjectActor
    {
        #region Save Data

        [Serializable]
        private class SaveData : ComponentData
        {
            public string currentLocationName;
        }

        #endregion

        #region Properties and Fields

        public GameObject Prefab
        {
            get => prefab;
            set
            {
                prefab = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public bool CustomiseScale
        {
            get => customiseScale;
            set
            {
                customiseScale = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private GameObject prefab;
        [SerializeField] private bool customiseScale = true;
        [SerializeField, ShowIf(nameof(customiseScale))] private Vector3 scale = Vector3.one;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData();
        }

        public GameObject InstantiateActor(Instance instance, Transform parent)
        {
            GameObject gameObject = Instantiate(prefab, parent);

            if (customiseScale)
            {
                gameObject.transform.localScale = scale;
            }

            return gameObject;
        }

        public string GetCurrentLocationName(Instance instance)
        {
            SaveData saveData = instance.data as SaveData;
            return saveData.currentLocationName;
        }

        public void SetCurrentLocationName(Instance instance, string locationName)
        {
            SaveData saveData = instance.data as SaveData;
            saveData.currentLocationName = locationName;
            instance.events.ComponentDataChanged.Invoke();
        }
    }
}
