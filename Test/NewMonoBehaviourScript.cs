using UnityEngine;
using UnityFramework_Core.Script;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var _g = GameObject.Find("Main Camera");
        var _s = _g.GetComponent<Initialize>();
        _s.AddScripts();
        _s.InitializeGame();
    }
}
