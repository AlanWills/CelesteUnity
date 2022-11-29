using Celeste.Components;
using Celeste.Tools.Attributes.GUI;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Prefab Actor")]
    [CreateAssetMenu(fileName = nameof(PrefabActorBoardGameObjectComponent), menuName = "Celeste/Board Game/Components/Prefab Actor")]
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

        [SerializeField] private string defaultLocationName;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool customiseScale = true;
        [SerializeField, ShowIf(nameof(customiseScale))] private Vector3 scale = Vector3.one;

        #endregion

        public override ComponentData CreateData()
        {
            SaveData saveData = new SaveData();
            saveData.currentLocationName = defaultLocationName;

            return saveData;
        }

        public GameObject InstantiateActor(BoardGame boardGame, Instance instance)
        {
            SaveData saveData = instance.data as SaveData;
            GameObject location = boardGame.FindBoardLocation(saveData.currentLocationName);
            Debug.Assert(location != null, $"Could not find board location {saveData.currentLocationName}.");

            GameObject gameObject = Instantiate(prefab, location.transform);

            if (customiseScale)
            {
                gameObject.transform.localScale = scale;
            }

            return gameObject;
        }
    }
}
