using Celeste.Components.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Celeste.Components
{
    public static class ComponentExtensions
    {
        public static Dictionary<string, ComponentDTO> ToLookup(this List<ComponentDTO> componentDTOs)
        {
            Dictionary<string, ComponentDTO> lookup = new Dictionary<string, ComponentDTO>(StringComparer.Ordinal);
            
            for (int i = 0, n = componentDTOs.Count; i < n; ++i)
            {
                ComponentDTO componentDTO = componentDTOs[i];
#if KEY_CHECKS
                Debug.Assert(!lookup.ContainsKey(componentDTO.typeName), $"Duplicate DTO type name found: {componentDTO.typeName}.  Ignoring from lookup...");
                if (!lookup.ContainsKey(componentDTO.typeName))
#endif
                {
                    lookup.Add(componentDTO.typeName, componentDTO);
                }
            }

            return lookup;
        }
    }
}
