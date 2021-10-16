using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public class LabelColourResetter : IDisposable
    {
        private Color oldTextColour;

        public LabelColourResetter(Color newTextColour)
        {
            oldTextColour = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = newTextColour;
        }

        public void Dispose()
        {
            EditorStyles.label.normal.textColor = oldTextColour;
        }
    }
}
