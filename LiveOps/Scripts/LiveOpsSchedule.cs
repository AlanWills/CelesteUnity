using Celeste.LiveOps.Persistence;
using Celeste.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(LiveOpsSchedule), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Live Ops Schedule", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class LiveOpsSchedule : ListScriptableObject<LiveOpDTO>
    {
        #region Properties and Fields

#if UNITY_EDITOR
        // Have a list of templates just for the editor, so it makes baking out the json a lot easier
        [SerializeField] private List<LiveOpTemplate> itemTemplates = new List<LiveOpTemplate>();
#endif

        #endregion

#if UNITY_EDITOR
        public void EditorOnly_BuildFromTemplates()
        {
            List<LiveOpDTO> dtos = new List<LiveOpDTO>();

            foreach (var template in itemTemplates)
            {
                dtos.Add(new LiveOpDTO(template));
            }

            SetItems(dtos);
        }
#endif
    }
}
