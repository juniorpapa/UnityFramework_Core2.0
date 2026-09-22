using System;
using System.Collections.Generic;
using UnityFramework_Core.Script;

namespace UnityFramework_Core
{
    public interface IEventManager
    {
        public bool CreateEvent (string _name, out EventManager.Event _event);
        public bool GetEvent (string _name, out EventManager.Event _event);
        public bool RemoveEvent (string _name);
        public bool InvokeEvent (string _name, object _object = null);
        public bool AddMonitor (string _eventName, Action<object> _action);
        public bool RemoveMonitor (string _eventName, Action<object> _action);
    }
    public class EventManager : IEventManager, IGameScript
    {
        public class Event
        {
            public Action<object> action;
            public bool whenRemoveCall { get; private set; }
            public Int64 createTick { get; private set; }
            public object createObject { get; private set; }
        }
        public string scriptName { get; set; } = "EventManager";
        public bool isMonoBehaviour { get; set; } = false;
        private Dictionary<string, Event> _events_ = new Dictionary<string, Event>();

        public Dictionary<string, Event> GetAllEvent()
        {
            return new Dictionary<string, Event>(_events_);
        }
        public bool RemoveAllEvent()
        {
            try
            {
                foreach (var _kvp in _events_)
                {
                    if (_kvp.Value.whenRemoveCall) _kvp.Value.action?.Invoke(null);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                _events_.Clear();
            }
            return true;
        }

        public bool CreateEvent(string _name, out Event _event)
        {
            if (_events_.ContainsKey(_name))
            {
                _event = null;
                return false;
            }
            _event = new Event();
            _events_.Add(_name, _event);
            return true;
        }
        public bool GetEvent(string _name, out Event _event)
        {
            if (_events_.TryGetValue(_name, out var _e))
            {
                _event = _e;
                return true;
            }
            _event = null;
            return false;
        }
        public bool RemoveEvent(string _name)
        {
            if (!_events_.TryGetValue(_name, out var _e))
            {
                return false;
            }
            if (_e != null && _e.whenRemoveCall)
                _e.action?.Invoke(null);
            _events_.Remove(_name);
            return true;
        }
        public bool InvokeEvent(string _name, object _object = null)
        {
            if (!_events_.TryGetValue(_name, out var _e))
            {
                return false;
            }
            _e.action?.Invoke(_object);
            return true;
        }

        public bool AddMonitor(string _eventName, Action<object> _action)
        {
            if (!_events_.TryGetValue(_eventName, out var _e))
            {
                return false;
            }
            _e.action += _action;
            return true;
        }
        public bool RemoveMonitor(string _eventName, Action<object> _action)
        {
            if (!_events_.TryGetValue(_eventName, out var _e))
            {
                return false;
            }
            _e.action -= _action;
            return true;
        }
    }
}
