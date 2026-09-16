using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IOldKeyInput : IGameScript
    {
        public UnityFramework_Core.Environment environment { get; set; }
        public string keyOptionName { get; set; }

        public bool CreateKeyOption();
        public bool CreateKeyOption(GameResource_Object _r);
        public bool SyncEvent();
        public bool SyncOption();
        public bool FromOptionSync();
    }
    public class OldKeyInput : MonoBehaviour, IOldKeyInput
    {
        public string scriptName { get; set; } = "OldKeyInput";
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
            environment.resourceManager.SetResource(keyOptionName, new GameResource_Object() { obj = keyOption});
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
        private bool IsTigger(KeyValuePair<string, KeyEvent> _keyEvent, bool _while = false)
        {
            foreach (KeyValuePair<KeyEvent.keyState, KeyCode> _kvp in _keyEvent.Value.keyValuePairs)
            {
                if (_kvp.Key == KeyEvent.keyState.Down && !Input.GetKeyDown(_kvp.Value)) return false;
                if (_kvp.Key == KeyEvent.keyState.Up && !Input.GetKeyUp(_kvp.Value)) return false;
                if (_kvp.Key == KeyEvent.keyState.Get && !Input.GetKey(_kvp.Value)) return false;
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
    public class KeyEvent
    {
        public enum keyState
        {
            Down,
            Up,
            Get
        }
        public List<KeyValuePair<keyState, KeyCode>> keyValuePairs = new List<KeyValuePair<keyState, KeyCode>>();
        public KeyValuePair<string, KeyEvent> notKeyValuePairs = new KeyValuePair<string, KeyEvent>();
    }
}
