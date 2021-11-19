using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Utils
{
    public enum FindConstraint
    {
        ActiveInHierarchy,
        ActiveSelf,
        NoConstraint
    }

    public static class GameObjectUtils
    {
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyAllChildren();
        }

        public static void DestroyAllChildrenImmediate(this GameObject gameObject)
        {
            gameObject.transform.DestroyAllChildrenImmediate();
        }

        public static GameObject FindGameObject(string[] splitName, FindConstraint findConstraint)
        {
            GameObject gameObject = GameObject.Find(splitName[0]);

            if (gameObject != null && FindConstraintMet(gameObject, findConstraint))
            {
                for (int i = 1; i < splitName.Length && gameObject != null; ++i)
                {
                    gameObject = FindGameObjectRecursive(gameObject.transform, splitName[i], findConstraint);
                }
            }

            return gameObject;
        }

        public static GameObject FindGameObjectRecursive(Transform transform, string childName, FindConstraint findConstraint)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                if (child.name == childName && FindConstraintMet(child.gameObject, findConstraint))
                {
                    return child.gameObject;
                }
            }

            for (int i = 0; i < transform.childCount; ++i)
            {
                GameObject child = FindGameObjectRecursive(transform.GetChild(i), childName, findConstraint);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        public static bool Click(this GameObject gameObject)
        {
            Button button = gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                return true;
            }

            Toggle toggle = gameObject.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.Invoke(!toggle.isOn);
                return true;
            }

            UnityEngine.Debug.LogFormat("No clickable component found on GameObject {0}", gameObject.name);
            return false;
        }

        private static bool FindConstraintMet(GameObject gameObject, FindConstraint findConstraint)
        {
            switch (findConstraint)
            {
                case FindConstraint.ActiveInHierarchy:
                    return gameObject.activeInHierarchy;

                case FindConstraint.ActiveSelf:
                    return gameObject.activeSelf;

                case FindConstraint.NoConstraint:
                    return true;

                default:
                    UnityEngine.Debug.LogErrorFormat("Unhandled FindConstraint: {0}", findConstraint);
                    return false;
            }
        }
    }
}
