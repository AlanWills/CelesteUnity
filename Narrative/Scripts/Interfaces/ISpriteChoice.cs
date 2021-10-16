using System.Collections;
using UnityEngine;

namespace Celeste.Narrative
{
    public interface ISpriteChoice : IChoice
    {
        Sprite Sprite { get; }
    }
}