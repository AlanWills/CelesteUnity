using System.Collections;
using UnityEngine;

namespace Celeste.Narrative
{
    public interface IChoice
    {
        string name { get; }

        void OnSelected();
    }
}