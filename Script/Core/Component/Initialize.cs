using System;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IInitialize : IGameScript
    {
        public bool InitializeGame(object _obj = null);
        public Action<object> initializeAction { get; set; }
        public Action<UnityFramework_Core.Environment> evnAction { get; set; }
    }
    public class Initialize : MonoBehaviour, IInitialize
    {
        public string scriptName { get; set; } = "Initialize";
        public bool isMonoBehaviour { get; set; } = true;

        public Action<object> initializeAction { get; set; }
        public Action<UnityFramework_Core.Environment> evnAction { get; set; } = _e =>
        {
            _e.useConsole = true;
            _e.eventManager = new EventManager();
            _e.resourceManager = new ResourceManager() { replace = 8 };
            _e.scriptManager = new ScriptManager() { environment = _e, sandboxEnvironmentList = "sandboxEnv" };
            _e.scriptManager.CreateSandboxEnvironmentList();
            _e.console = new Console();
            var _g = GameObject.Find("GAMEENV");
            var _log = new GameLog() { env = _g.GetComponent<Environment>(), getTime = () => DateTime.Now };
            _e.scripts.Add("GameLog", _log);
            _g.AddComponent<ConsoleWindow>();
            _g.AddComponent<OldKeyInput>();
            _log.logs.Add("Initialization complete");
        };
        public bool AddScripts()
        {
            try
            {
                GameObject _gameObject = new GameObject("GAMEENV");
                DontDestroyOnLoad(_gameObject);
                Environment _env = _gameObject.AddComponent<Environment>();
                _env.environment = new UnityFramework_Core.Environment();
                evnAction?.Invoke(_env.environment);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InitializeGame(object _obj = null)
        {
            try
            {
                initializeAction?.Invoke(_obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
