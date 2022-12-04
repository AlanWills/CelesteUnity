using Celeste.Components;
using Celeste.Tools.Attributes.GUI;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Prefab Actor")]
    [CreateAssetMenu(fileName = nameof(PrefabActorBoardGameComponent), menuName = "Celeste/Board Game/Board Game Components/Prefab Actor")]
    public class PrefabActorBoardGameComponent : BoardGameComponent, IBoardGameActor
    {
        #region Properties and Fields

        [SerializeField] private GameObject prefab;
        [SerializeField] private bool customiseScale = true;
        [SerializeField, ShowIf(nameof(customiseScale))] private Vector3 scale = Vector3.one;

        #endregion

        public GameObject InstantiateActor(Instance instance, Transform parent)
        {
            GameObject gameObject = Instantiate(prefab, parent);

            if (customiseScale)
            {
                gameObject.transform.localScale = scale;
            }

            return gameObject;
        }
    }
}
