using System;
using System.Collections.Concurrent;

namespace UnityFramework_Core.Script
{
    public interface IGameComponent
    {
        public bool AddComponent(string _n, ComponentBase _c);
        public bool RemoveComponent(string _n);
        public ComponentBase GetComponent(string _n);
        public bool AddComponent(ConcurrentDictionary<string, ComponentBase> _d, string _cn);
    }
    public class GameComponent : IGameScript
    {
        public string scriptName { get; set; } = "GameComponent";
        public bool isMonoBehaviour { get; set; } = false;

        private ConcurrentDictionary<string, ComponentBase> _components_;
        public bool AddComponent(string _n, ComponentBase _c)
        {
            return _components_.TryAdd(_n, _c);
        }
        public bool RemoveComponent(string _n)
        {
            return _components_.TryRemove(_n, out _);
        }
        public ComponentBase GetComponent(string _n)
        {
            if (!_components_.TryGetValue(_n, out var _c)) return null;
            return _c;
        }
        public bool AddComponent(ConcurrentDictionary<string, ComponentBase> _d, string _cn)
        {
            if (!_components_.TryGetValue(_cn, out var _c)) return false;
            _c.Constructor();
            if (!_d.TryAdd(_cn, _c)) return false;
            return true;
        }
    }
    public abstract class ComponentBase
    {
        public abstract Type interfaceType { get; set; }
        public abstract Type type { get; set; }
        public abstract (MemberType, Type) GetPublic();
        public abstract void Constructor();
    }
    public enum MemberType
    {
        Variable,
        Method,
        Class,
        Interface,
        Enum
    }
}
