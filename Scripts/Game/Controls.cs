using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    static public Dictionary<string,int> PlayerControls = new Dictionary<string, int>();
    private void OnEnable() 
    {
        PlayerControls.Add("Use",0);
        PlayerControls.Add("Throw",1);
        PlayerControls.Add("Pause",27);
    }
    private void OnDisable() 
    {
        PlayerControls.Clear();
    }
}
