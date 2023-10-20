using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class GameManager : NetworkBehaviour
{
    void Start()
    {
        if (Application.isEditor)
        { }
        else
        { Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }
    }
    private void OnGUI()
    {
        NetworkHelper.GUILayoutNetworkControls(); }
}
        


