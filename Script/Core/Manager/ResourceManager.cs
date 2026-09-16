using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace UnityFramework_Core
{
    public interface IResourceManager
    {
        public int replace { get; set; }
        public ConcurrentDictionary<string, GameResource> GetAllResouce();
        public bool DeleteAllResouce();
        public short GetResource(string _name, Action<object> _callback, out object _obj);
        public short GetResource(string _name, Action<object> _callback, bool _replace, out object _obj);
        public bool SaveResource(string _name, GameResource _r);
        public bool SetResource(string _name, GameResource _r);
        public bool DeleteResource(string _name);
    }
    public class ResourceManager : IResourceManager
    {
        private ConcurrentDictionary<string, GameResource> _gameResource_ = new ConcurrentDictionary<string, GameResource>();
        public int replace { get; set; } = 16;

        public ConcurrentDictionary<string, GameResource> GetAllResouce()
        {
            return new ConcurrentDictionary<string, GameResource>(_gameResource_);
        }
        public bool DeleteAllResouce()
        {
            try
            {
                foreach (var _kvp in GetAllResouce())
                {
                    _kvp.Value.Delete();
                }
            }
            catch
            {
                return false;
            }
            finally
            {

            }
            return true;
        }

        public short GetResource(string _name, Action<object> _callback, out object _obj)
        {
            if (!_gameResource_.TryGetValue(_name, out var _v))
            {
                _obj = null;
                return -1;
            }
            switch (_v.state)
            {
                case 0:
                    _obj = null;
                    _v.waitLoaded += _callback;
                    return _v.state;
                case 1:
                    _obj = null;
                    _v.waitLoaded += _callback;
                    return _v.state;
                case 2:
                    _obj = _v.obj;
                    _v.holder++;
                    return _v.state;
                case 3:
                    _obj = null;
                    return _v.state;
                default:
                    _obj = null;
                    return _v.state;
            }
        }
        public short GetResource(string _name, Action<object> _callback, bool _replace, out object _obj)
        {
            GameResource _v = null;
            _obj = null;
            for (int i = replace; i > 0; i--)
            {
                if (!_gameResource_.TryGetValue(_name, out _v))
                {
                    return -1;
                }
                if (_v.replace == "") break;
                _name = _v.replace;
            }
            if (_v == null) return -2;
            switch (_v.state)
            {
                case 0:
                    _v.waitLoaded += _callback;
                    return _v.state;
                case 1:
                    _v.waitLoaded += _callback;
                    return _v.state;
                case 2:
                    _obj = _v.obj;
                    _v.holder++;
                    return _v.state;
                case 3:
                    return _v.state;
                default:
                    return _v.state;
            }
        }
        public bool SaveResource(string _name, GameResource _r)
        {
            if (_gameResource_.TryAdd(_name, _r)) return true;
            return false;
        }
        public bool SetResource(string _name, GameResource _r)
        {
            if (!_gameResource_.TryGetValue(_name, out var _v))
            {
                return false;
            }
            _gameResource_.TryUpdate(_name, _r, _v);
            return true;
        }
        public bool DeleteResource(string _name)
        {
            if (!_gameResource_.TryGetValue(_name, out var _v)) return false;
            _v.Delete();
            return true;
        }
    }
    public abstract class GameResource
    {
        public readonly object r_lock = new object();
        public object obj;
        // state => 0 = NotLoaded, 1 = Loading, 2 = Loaded, 3 = failed
        public short state;
        public uint holder;
        public string replace;
        public Action<object> waitLoaded;
        public abstract void Loaded(object _obj);
        public abstract void Delete();
    }
    public class GameResource_Object : GameResource
    {
        public override void Loaded(object _obj)
        {
            lock (r_lock)
            {
                if (_obj == null)
                {
                    state = 3;
                    return;
                }
                waitLoaded?.Invoke(_obj);
                state = 2;
            }
        }
        public override void Delete()
        {

        }
    }
    public class GameResource_AssetBundle : GameResource
    {
        public override void Loaded(object _obj)
        {
            lock (r_lock)
            {
                if (_obj == null)
                {
                    state = 3;
                    return;
                }
                waitLoaded?.Invoke(_obj);
                state = 2;
            }
        }
        public override void Delete()
        {
            if (obj != null && obj is AssetBundle) (obj as AssetBundle).Unload(false);
        }
    }
    public class GameResource_UnityEngineObject : GameResource
    {
        public override void Loaded(object _obj)
        {
            lock (r_lock)
            {
                if (_obj == null)
                {
                    state = 3;
                    return;
                }
                waitLoaded?.Invoke(_obj);
                state = 2;
            }
        }
        public override void Delete()
        {
            UnityEngine.Object.Destroy((obj as UnityEngine.Object));
        }
    }
}