using System;
using UnityEngine;

namespace UnityFramework_Core.Script
{
    public class Environment : MonoBehaviour
    {
        public Int64 gameTick { get; private set; } = 0;
        public UnityFramework_Core.Environment environment;
        private void FixedUpdate()
        {
            gameTick++;
        }
    }
}
