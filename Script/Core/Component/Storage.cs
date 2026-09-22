using System;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IStorage : IGameScript
    {
        // Json
        public string ToJson(object _o, bool _p = false);
        public T FromJson<T>(string _j);
        public object FromJson(string _j, System.Type _t);
        public void FromJsonOverwrite(string _j, object _o);
        // Storage
        public Func<string, string, bool> saveString { get; set; }
        public Func<string, bool, bool> saveBoolean { get; set; }
        public Func<string, int, bool> saveInt { get; set; }
        public Func<string, float, bool> saveFloat { get; set; }
        public Func<string, string> getString { get; set; }
        public Func<string, bool?> getBoolean { get; set; }
        public Func<string, int?> getInt32 { get; set; }
        public Func<string, float?> getFloat { get; set; }
        public Func<string, bool> deleteKey { get; set; }
    }
    public class Storage : IStorage
    {
        public string scriptName { get; set; } = "Storage";
        public bool isMonoBehaviour { get; set; } = false;

        public Func<string, string, bool> saveString { get; set; } = (_k, _v) =>
        {
            try
            {
                PlayerPrefs.SetString(_k, _v);
                PlayerPrefs.Save();
                return true;
            }
            catch
            {
                return false;
            }
        };
        public Func<string, bool, bool> saveBoolean { get; set; } = (_k, _v) =>
        {
            try
            {
                PlayerPrefs.SetInt(_k, _v == true ? 1:0);
                PlayerPrefs.Save();
                return true;
            }
            catch
            {
                return false;
            }
        };
        public Func<string, int, bool> saveInt { get; set; } = (_k, _v) =>
        {
            try
            {
                PlayerPrefs.SetInt(_k, _v);
                PlayerPrefs.Save();
                return true;
            }
            catch
            {
                return false;
            }
        };
        public Func<string, float, bool> saveFloat { get; set; } = (_k, _v) =>
        {
            try
            {
                PlayerPrefs.SetFloat(_k, _v);
                PlayerPrefs.Save();
                return true;
            }
            catch
            {
                return false;
            }
        };
        public Func<string, string> getString { get; set; } = _k =>
        {
            try
            {
                return PlayerPrefs.GetString(_k);
            }
            catch
            {
                return null;
            }
        };
        public Func<string, bool?> getBoolean { get; set; } = _k =>
        {
            try
            {
                return Convert.ToBoolean(PlayerPrefs.GetInt(_k));
            }
            catch
            {
                return null;
            }
        };
        public Func<string, int?> getInt32 { get; set; } = _k =>
        {
            try
            {
                return PlayerPrefs.GetInt(_k);
            }
            catch
            {
                return null;
            }
        };
        public Func<string, float?> getFloat { get; set; } = _k =>
        {
            try
            {
                return PlayerPrefs.GetFloat(_k);
            }
            catch
            {
                return null;
            }
        };
        public Func<string, bool> deleteKey { get; set; } = _k =>
        {
            PlayerPrefs.DeleteKey(_k);
            return true;
        };

        public string ToJson(object _o, bool _p = false)
        {
            return JsonUtility.ToJson(_o, _p);
        }
        public T FromJson<T>(string _j)
        {
            return JsonUtility.FromJson<T>(_j);
        }
        public object FromJson(string _j, System.Type _t)
        {
            return JsonUtility.FromJson(_j, _t);
        }
        public void FromJsonOverwrite(string _j, object _o)
        {
            JsonUtility.FromJsonOverwrite(_j, _o);
        }
    }
}
