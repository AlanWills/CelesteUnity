using Celeste.BoardGame.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Board Game Object UI Controller")]
    public class BoardGameObjectUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private List<GameObject> componentUIControllers = new List<GameObject>();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            componentUIControllers.Clear();

            foreach (var componentUIController in GetComponents<IBoardGameObjectComponentUIController>())
            {
                componentUIControllers.Add((componentUIController as MonoBehaviour).gameObject);
            }
        }

        #endregion

        public void Hookup(BoardGameObjectRuntime runtime)
        {
            foreach (var component in componentUIControllers)
            {
                component.GetComponent<IBoardGameObjectComponentUIController>().Hookup(runtime);
            }
        }

        public void Shutdown()
        {
            foreach (var component in componentUIControllers)
            {
                if (component != null)
                {
                    component.GetComponent<IBoardGameObjectComponentUIController>().Shutdown();
                }
            }
        }
    }
}
