using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineNodeLink
    {
        #region Properties and Fields

        public bool IsValid { get; private set; }

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

        #endregion

        public static TwineNodeLink[] CreateFromText(string text)
        {
            List<TwineNodeLink> createdLinks = new List<TwineNodeLink>();
            int delimiterStart = text.IndexOf("[[");

            while (delimiterStart != -1)
            {
                int delimiterEnd = text.IndexOf("]]", delimiterStart + 2);
                int startIndex = delimiterStart + 2;
                string linkText = text.Substring(startIndex, delimiterEnd - startIndex);
                int linkDelimiter = linkText.IndexOf("->");

                TwineNodeLink twineNodeLink = new TwineNodeLink();
                createdLinks.Add(twineNodeLink);

                if (linkDelimiter >= 0)
                {
                    // Need to split out display and internal text
                    twineNodeLink.name = linkText.Substring(0, linkDelimiter);
                    twineNodeLink.link = linkText.Substring(linkDelimiter + 2);
                }
                else
                {
                    // Just use the linkText for both name and link
                    twineNodeLink.name = linkText;
                    twineNodeLink.link = linkText;
                }

                delimiterStart = text.IndexOf("[[", delimiterEnd + 2);
            }

            return createdLinks.ToArray();
        }

        public bool Validate(TwineStory twineStory)
        {
            bool isValid = true;

            isValid &= !string.IsNullOrEmpty(name);
            isValid &= !string.IsNullOrEmpty(link);
            isValid &= twineStory.passages.Exists(x => x.pid == pid);

            IsValid = isValid;

            return isValid;
        }
    }
}