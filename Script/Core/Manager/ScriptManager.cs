using System;
using System.Collections.Generic;

namespace UnityFramework_Core
{
    namespace Script
    {
        public interface IGameScript
        {
            public string scriptName { get; }
            public bool isMonoBehaviour { get; }
        }
    }
    public interface IConsole
    {
        public object Execute(string _text);
    }
    public class Console : IConsole
    {
        private byte _execute_ = 0;
        private int _pos_ = -1;
        private string[] _list_ = null;
        public byte mixExecute = 20;
        public Dictionary<string, Func<object, object>> token;

        public object Execute(string _text)
        {
            return ExecuteText(_text);
        }
        private void End()
        {
            _execute_ = 0;
            _pos_ = -1;
            _list_ = null;
        }
        public bool ExecuteText(string _code, object _obj = null)
        {
            _execute_++;
            if (_execute_ > mixExecute) return false;
            if (_list_ == null)
            {
                int _pos = _code.Length - 1;
                while (_pos > 0)
                {
                    if (char.IsWhiteSpace(_code[_pos]))
                    {
                        _pos--;
                        continue;
                    }
                    else if (_code[_pos] == '\"')
                    {
                        int _lastQuote = _pos;
                        int _firstQuote = _code.LastIndexOf('\"', _lastQuote - 1);
                        if (_firstQuote == -1) return false;
                        string _sub = _code.Substring(_firstQuote + 1, _lastQuote - _firstQuote - 1);
                        _code = _code.Substring(0, _firstQuote).TrimEnd();
                        _list_ = string.IsNullOrEmpty(_code) ? new string[0] : _code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        _pos_ = _list_.Length - 1;
                        return ExecuteText(_code, _sub);
                    }
                    break;
                }
                _list_ = _code.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                _pos_ = _list_.Length - 1;
                return ExecuteText(_code);
            }
            if (_pos_ >= 0 && _list_[_pos_] != null)
            {
                try
                {
                    if (token.TryGetValue(_list_[_pos_], out var _val))
                    {
                        _pos_--;
                        return ExecuteText(_code, _val?.Invoke(_obj));
                    }
                    _pos_--;
                    return ExecuteText(_code, _list_[_pos_]);
                }
                catch
                {
                    
                }
            }
            else End();
            return true;
        }
        public bool ExecuteLines(string _code)
        {
            string[] _lines = _code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string _line in _lines)
            {
                if (!ExecuteText(_line)) return false;
            }
            return true;
        }
    }
    public class Environment
    {
        public bool useConsole;
        public IEventManager eventManager;
        public IResourceManager resourceManager;
        public IScriptManager scriptManager;
        public IConsole console;
        public Dictionary<string, Script.IGameScript> scripts = new Dictionary<string, Script.IGameScript>();

        public List<string> GetAllGameScript()
        {
            List<string> _list = new List<string>();
            foreach (var _script in scripts.Values)
            {
                _list.Add(_script.scriptName);
            }
            return _list;
        }
    }
    public static class ScriptToolkit
    {
        public static (short _state, object _obj) GetResource(Environment _e, string _name, bool _replace = false)
        {
            if (_e.resourceManager == null) return (-1, null);
            return (_e.resourceManager.GetResource(_name, null, _replace, out var _obj), _obj);
        }
    }
    public interface IScriptManager
    {
        public Environment environment { get; set; }
        public string sandboxEnvironmentList { get; set; }
        public bool CreateSandboxEnvironmentList();
        public bool CreateSandboxEnvironmentList(GameResource_Object _r);
        public Environment GetEnvironment(string _name);
    }
    public class ScriptManager : IScriptManager
    {
        public Environment environment { get; set; }
        public string sandboxEnvironmentList { get; set; }
        public bool CreateSandboxEnvironmentList()
        {
            if (sandboxEnvironmentList == null) return false;
            return environment.resourceManager.SaveResource(sandboxEnvironmentList, new GameResource_Object() { obj = new Dictionary<string, Environment>(), state = 2 });
        }
        public bool CreateSandboxEnvironmentList(GameResource_Object _r)
        {
            if (sandboxEnvironmentList == null) return false;
            return environment.resourceManager.SaveResource(sandboxEnvironmentList, _r);
        }
        public Environment GetEnvironment(string _name)
        {
            environment.resourceManager.GetResource(_name, null, out var _v);
            if (_v is Environment)
                return _v as Environment;
            else
                return null;
        }
    }
}
