using Celeste.BoardGame.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Board Game Object UI Controller")]
    public class BoardGameObjectView : MonoBehaviour
    {
        #region Properties and Fields

        public BoardGameObjectRuntime BoardGameObjectRuntime { get; private set; }

        [SerializeField] private List<GameObject> componentViews = new();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            componentViews.Clear();

            foreach (var componentView in GetComponents<IBoardGameObjectComponentView>())
            {
                componentViews.Add((componentView as MonoBehaviour).gameObject);
            }
        }

        #endregion

        public void Hookup(BoardGameObjectRuntime runtime)
        {
            BoardGameObjectRuntime = runtime;

            foreach (var component in componentViews)
            {
                component.GetComponent<IBoardGameObjectComponentView>().Hookup(runtime);
            }
        }

        public void Shutdown()
        {
            foreach (var component in componentViews)
            {
                if (component != null)
                {
                    component.GetComponent<IBoardGameObjectComponentView>().Shutdown();
                }
            }

            BoardGameObjectRuntime = null;
        }
    }
}
