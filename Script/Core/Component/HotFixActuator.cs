using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IHotFixActuator : IGameScript
    {
        public string hotFixPath { get; set; }
        public void Update();
        public Dictionary<string, PackageInfor> GetAllPackage();
        public PackageInfor GetPackageInfor(string _d);
        public byte[] LoadFile(string _d, string _p);
        public byte[] LoadPackageFile(string _n, string _p);
    }
    public class HotFixActuator : IHotFixActuator
    {
        public string scriptName { get; set; } = "HotFixActuator";
        public bool isMonoBehaviour { get; set; } = false;

        private Dictionary<string, PackageInfor> _cache_ = new Dictionary<string, PackageInfor>();
        public string hotFixPath { get; set; }
        public void Update()
        {
            if (hotFixPath == null || hotFixPath == "") return;
            _cache_.Clear();
            foreach (string _n in Directory.EnumerateDirectories(hotFixPath, "*", new EnumerationOptions { IgnoreInaccessible = true, }))
            {
                var _p = GetPackageInfor(_n);
                if (_p == null || _p.name == null) continue;
                _cache_.Add(_n, _p);
            }
        }
        public Dictionary<string, PackageInfor> GetAllPackage()
        {
            Update();
            return _cache_;
        }
        public PackageInfor GetPackageInfor(string _d)
        {
            if (!File.Exists(Path.Combine(hotFixPath, _d, "Infor.txt"))) return null;
            try
            {
                string _t = File.ReadAllText(_d);
                var _r = JsonUtility.FromJson<PackageInfor>(_t);
                return _r;
            }
            catch
            {
                return null;
            }
        }
        public byte[] LoadFile(string _d, string _p)
        {
            var _rp = Path.Combine(hotFixPath, _d, _p);
            if (!File.Exists(_rp)) return null;
            try
            {
                return File.ReadAllBytes(_rp);
            }
            catch
            {
                return null;
            }
        }
        public byte[] LoadPackageFile(string _n, string _p)
        {
            foreach (var _kvp in _cache_)
            {
                if (_kvp.Value.name == _n)
                {
                    var _fp = Path.Combine(hotFixPath, _kvp.Key, _p);
                    if (!File.Exists(_fp)) return null;
                    return File.ReadAllBytes(_fp);
                }
            }
            return null;
        }
    }
    public class PackageInfor
    {
        public string name { get; set; }
        public string packgeVersion { get; set; }
        public int[] HSV { get; set; } // Highest supported version
        public int[] LSV { get; set; } // lowest supported version
        public string[] creator { get; set; }
        public string[] preInstalledPackage { get; set; }
        public string[] startScript { get; set; } // 0 CSharp 1 Lua 2 JavaScript 3 Python
    }
}
