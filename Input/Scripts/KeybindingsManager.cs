using Celeste.DataStructures;
using Celeste.Parameters.Input;
using Celeste.Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Input
{
    [CreateAssetMenu(fileName = "New KeybindingsManager", menuName = "Celeste/Input/Keybindings Manager")]
    public class KeybindingsManager : PersistentSceneManager<KeybindingsManager, KeybindingsManagerDTO>
    {
        #region Properties and Fields

        private const string FILE_NAME = "Keybindings.dat";

        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        public int NumKeyCodes
        {
            get { return keyCodes.Count; }
        }

        [SerializeField]
        private List<KeyCodeValue> keyCodes = new List<KeyCodeValue>();

        #endregion

        private KeybindingsManager() { }

        #region Save/Load Methods

        protected override KeybindingsManagerDTO Serialize()
        {
            return new KeybindingsManagerDTO(this);
        }

        protected override void Deserialize(KeybindingsManagerDTO keybindingsManagerDTO)
        {
            for (int i = 0, n = keybindingsManagerDTO.keyCodes.Count; i < n; ++i)
            {
                KeyBinding keyBinding = keybindingsManagerDTO.keyCodes[i];
                KeyCodeValue keyCodeValue = keyCodes.Find(x => x.name == keyBinding.name);
                Debug.AssertFormat(keyCodeValue != null, "Unable to find key code {0}", keyBinding.name);

                if (keyCodeValue != null)
                {
                    keyCodeValue.Value = keyBinding.keyCode;
                }
            }
        }

        protected override void SetDefaultValues() { }

        #endregion

        public KeyCodeValue GetKeyCode(int index)
        {
            return keyCodes.Get(index);
        }
    }

    [Serializable]
    public struct KeyBinding
    {
        public string name;
        public KeyCode keyCode;

        public KeyBinding(KeyCodeValue keyCodeValue)
        {
            name = keyCodeValue.name;
            keyCode = keyCodeValue.Value;
        }
    }

    [Serializable]
    public class KeybindingsManagerDTO
    {
        public List<KeyBinding> keyCodes = new List<KeyBinding>();

        public KeybindingsManagerDTO(KeybindingsManager keybindingsManager)
        {
            keyCodes.Capacity = keybindingsManager.NumKeyCodes;

            for (int i = 0, n = keybindingsManager.NumKeyCodes; i < n; ++i)
            {
                keyCodes.Add(new KeyBinding(keybindingsManager.GetKeyCode(i)));
            }
        }
    }
}