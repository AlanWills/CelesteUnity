using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Objects
{
    public interface IInstantiatePrefab
    {
        GameObject InstantiatePrefab();
        GameObject InstantiatePrefab(Vector3 position);
        GameObject InstantiatePrefab(Quaternion rotation);
        GameObject InstantiatePrefab(Vector3 position, Quaternion rotation);

        GameObject InstantiatePrefab(Transform parent);
        GameObject InstantiatePrefab(Transform parent, Vector3 position);
        GameObject InstantiatePrefab(Transform parent, Quaternion rotation);
        GameObject InstantiatePrefab(Transform parent, Vector3 position, Quaternion rotation);
    }
}
