using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IGameLog : IGameScript
    {
        public void AddLogs(string _text);
        public List<string> GetLogs();
    }
    public class GameLog : IGameLog
    {
        public string scriptName { get; set; } = "GameLog";
        public bool isMonoBehaviour { get; set; } = false;

        public Environment env { get; set; }
        public Func<object> getTime { get; set; }
        public List<string> logs = new List<string>();
        public void AddLogs(string _text)
        {
            logs.Add(env.gameTick + "-" + getTime?.Invoke().ToString() + "" + _text);
        }
        public List<string> GetLogs()
        {
            return new List<string>(logs);
        }
    }
}
