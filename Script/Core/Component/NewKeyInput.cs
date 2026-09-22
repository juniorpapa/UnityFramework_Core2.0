using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFramework_Core.Script
{
    public class NewKeyInput : MonoBehaviour, IKeyInput
    {
        public string scriptName { get; set; } = "NewKeyInput";
        public bool isMonoBehaviour { get; set; } = true;

        public UnityFramework_Core.Environment environment { get; set; }
        public string keyOptionName { get; set; } = "keyOption";
        public ConcurrentDictionary<string, KeyEvent> keyOption = new ConcurrentDictionary<string, KeyEvent>();
        public bool CreateKeyOption()
        {
            if (keyOptionName == null) return false;
            environment.resourceManager.SaveResource(keyOptionName, new GameResource_Object() { obj = keyOption, state = 2 });
            return true;
        }
        public bool CreateKeyOption(GameResource_Object _r)
        {
            if (keyOptionName == null) return false;
            environment.resourceManager.SaveResource(keyOptionName, _r);
            return true;
        }
        public bool SyncEvent()
        {
            foreach (var _kvp in keyOption)
            {
                if (!environment.eventManager.CreateEvent(_kvp.Key, out var _v)) return false;
            }
            return true;
        }
        public bool SyncOption()
        {
            if (keyOptionName == null) return false;
            environment.resourceManager.SetResource(keyOptionName, new GameResource_Object() { obj = keyOption });
            return true;
        }
        public bool FromOptionSync()
        {
            if (keyOptionName == null) return false;
            environment.resourceManager.GetResource(keyOptionName, null, out object _obj);
            keyOption = _obj as ConcurrentDictionary<string, KeyEvent>;
            return true;
        }
        private List<string> _tiggerKeys_ = new List<string>();
        private static Key ToKey(KeyCode _k)
        {
            if (_k >= KeyCode.Alpha0 && _k <= KeyCode.Alpha9)
                return (Key)((int)Key.Digit0 + (_k - KeyCode.Alpha0));
            if (_k >= KeyCode.Keypad0 && _k <= KeyCode.Keypad9)
                return (Key)((int)Key.Numpad0 + (_k - KeyCode.Keypad0));
            switch (_k)
            {
                case KeyCode.None: return Key.None;
                case KeyCode.Return: return Key.Enter;
                case KeyCode.LeftControl: return Key.LeftCtrl;
                case KeyCode.RightControl: return Key.RightCtrl;
                case KeyCode.LeftCommand: return Key.LeftMeta;
                case KeyCode.RightCommand: return Key.RightMeta;
                case KeyCode.KeypadDivide: return Key.NumpadDivide;
                case KeyCode.KeypadMultiply: return Key.NumpadMultiply;
                case KeyCode.KeypadMinus: return Key.NumpadMinus;
                case KeyCode.KeypadPlus: return Key.NumpadPlus;
                case KeyCode.KeypadEnter: return Key.NumpadEnter;
                case KeyCode.KeypadPeriod: return Key.NumpadPeriod;
                case KeyCode.KeypadEquals: return Key.NumpadEquals;
            }
            return System.Enum.TryParse<Key>(_k.ToString(), out var k) ? k : Key.None;
        }
        private bool IsTigger(KeyValuePair<string, KeyEvent> _keyEvent, bool _while = false)
        {
            var _k = Keyboard.current;
            if (_k == null) return false;
            foreach (var _kvp in _keyEvent.Value.keyValuePairs)
            {
                var _c = _k[ToKey(_kvp.Value)];
                if (_c == null) return false;
                switch (_kvp.Key)
                {
                    case KeyEvent.keyState.Down:
                        if (!_c.wasPressedThisFrame) return false;
                        break;
                    case KeyEvent.keyState.Up:
                        if (!_c.wasReleasedThisFrame) return false;
                        break;
                    case KeyEvent.keyState.Get:
                        if (!_c.isPressed) return false;
                        break;
                }
            }
            if (!_while && IsTigger(_keyEvent, true)) return false;
            return true;
        }
        void Update()
        {
            foreach (var _kvp in keyOption)
            {
                if (_tiggerKeys_.Contains(_kvp.Key)) continue;
                if (IsTigger(_kvp)) _tiggerKeys_.Add(_kvp.Key);
            }
        }
        void FixedUpdate()
        {
            foreach (var _k in _tiggerKeys_)
            {
                environment.eventManager.InvokeEvent(_k, this);
            }
            _tiggerKeys_.Clear();
        }
    }
}
