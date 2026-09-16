using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public interface IConsoleWindow : IGameScript
    {
        public UnityFramework_Core.Environment environment { get; set; }
        public int consoleID { get; set; }
        public string consoleTitle { get; set; }
        public bool consoleState { get; set; }
        public float xMin { get; set; }
        public float yMin { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public Color backgroundColor { get; set; }
        public Color contentColor { get; set; }
        public bool OpenConsole();
        public bool CloseConsole();
        public bool Enter();
        public bool ClearConsole();
    }
    public class ConsoleWindow : MonoBehaviour, IConsoleWindow
    {
        public string scriptName { get; set; } = "ConsoleWindow";
        public bool isMonoBehaviour { get; set; } = true;

        public int consoleID { get; set; } = 100;
        public string consoleTitle { get; set; } = "console";
        public string consoleInput { get; set; }
        public bool consoleLock { get; set; }
        public bool consoleState { get; set; } = true;
        public float xMin { get; set; } = Screen.width * 0.1f;
        public float yMin { get; set; } = Screen.height * 0.1f; 
        public float width { get; set; } = 800f; 
        public float height { get; set; } = 600f;
        public Color backgroundColor { get; set; } = Color.black;
        public Color contentColor { get; set; } = Color.green;
        public List<string> consoleLogs { get; set; } = new List<string>();
        public UnityFramework_Core.Environment environment { get; set; }
        public bool OpenConsole()
        {
            consoleState = true;
            return true;
        }
        public bool CloseConsole()
        {
            consoleState = false;
            return true;
        }
        public bool Enter()
        {
            try
            {
                var _r = environment.console?.Execute(consoleInput);
                if (_r != null) consoleLogs.Add(_r.ToString());
                return true;
            }
            catch (Exception ex)
            {
                consoleLogs.Add("cant execute");
                consoleLogs.Add("> " + ex.Message);
                return false;
            }
            finally
            {
                consoleLogs.Add("-" + consoleInput);
            }
        }
        public bool ClearConsole()
        {
            consoleLogs.Clear();
            return true;
        }
        private Rect _rect_ = new Rect();
        private Rect _newRect_ = new Rect();
        private Vector2 _v2_ = new Vector2();
        private void OnGUI()
        {
            if (consoleState)
            {
                var _bc = GUI.backgroundColor;
                var _cc = GUI.contentColor;
                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = contentColor;
                _rect_.xMin = xMin;
                _rect_.yMin = yMin;
                _rect_.width = width;
                _rect_.height = height;
                try
                {
                    _newRect_ = GUILayout.Window(consoleID, _rect_, WindowAction, consoleTitle);
                }
                finally
                {
                    GUI.backgroundColor = _bc;
                    GUI.contentColor = _cc;
                }
                if (!consoleLock)
                {
                    xMin = _newRect_.xMin;
                    yMin = _newRect_.yMin;
                    width = _newRect_.width;
                    height = _newRect_.height;
                }
            }
        }
        void WindowAction(int _id)
        {
            GUILayout.BeginVertical();
            _v2_ = GUILayout.BeginScrollView(_v2_);
            GUILayout.Label(string.Join("\n", consoleLogs));
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            consoleInput = GUILayout.TextField(consoleInput);
            if (GUILayout.Button("Enter", GUILayout.ExpandWidth(false)))
            {
                Enter();



                consoleInput = "";
            }
            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                if (ClearConsole())
                {
                    
                }
                else
                {
                    consoleLogs.Add("cant clear");
                }
            }
            if (GUILayout.Button("Close", GUILayout.ExpandWidth(false)))
            {
                if (CloseConsole())
                {
                
                }
                else
                {
                    consoleLogs.Add("cant close");
                }
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}
