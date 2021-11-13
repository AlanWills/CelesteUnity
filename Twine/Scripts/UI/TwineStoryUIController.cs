using Celeste.DataStructures;
using Celeste.Memory;
using CelesteEditor.Twine;
using System.Collections;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Twine Story UI Controller")]
    public class TwineStoryUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TwineStory twineStory;
        [SerializeField] private GameObjectAllocator twineNodeUIAllocator;

        #endregion

        #region Unity Methods

        private void Start()
        {
            foreach (TwineNode twineNode in twineStory.passages)
            {
                TwineNodeUIController.From(twineNode, twineNodeUIAllocator);
            }

            // Centre the starting node in the middle of the screen, but adjusting the offset of the parent
            twineNodeUIAllocator.transform.position = -twineStory.passages.Find(x => x.pid == twineStory.startnode).position;
        }

        #endregion
    }
}