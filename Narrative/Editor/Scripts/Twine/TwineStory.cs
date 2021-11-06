using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine
{
    [Serializable]
    public class TwineStory : ScriptableObject
    {
        public int startnode;
        public TwineNode[] passages;
    }

    [Serializable]
    public class TwineNode
    {
        public int pid;
        public string name;
        public string text;
        public string[] tags;
        public Vector2 position;
        public TwineNodeLink[] links;
    }

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