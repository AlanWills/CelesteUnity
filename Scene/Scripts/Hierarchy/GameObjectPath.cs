using Celeste.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Hierarchy
{
    [Serializable]
    public class GameObjectPath
    {
        #region Properties and Fields

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                Reset();
            }
        }

        public GameObject GameObject
        {
            get 
            {
                Split();

                if (gameObject == null)
                {
                    gameObject = GameObjectExtensions.FindGameObject(splitName, FindConstraint.ActiveInHierarchy);
                }

                return gameObject;
            }
        }

        [SerializeField]
        private string path;

        private string[] splitName;
        private GameObject gameObject;

        private static char[] delimiter = new char[1] { '.' };

        #endregion

        public void Split()
        {
            if (!string.IsNullOrEmpty(path) && (splitName == null || splitName.Length == 0))
            {
                splitName = path.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public void Reset()
        {
            splitName = null;
            gameObject = null;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
