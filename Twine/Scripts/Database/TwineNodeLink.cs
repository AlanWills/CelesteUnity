using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineNodeLink
    {
        /// <summary>
        /// Node to link to
        /// </summary>
        public int pid;
        
        /// <summary>
        /// Display text
        /// </summary>
        public string name;
        
        /// <summary>
        /// Internal name text
        /// </summary>
        public string link;
        public bool broken;
    }
}