using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneChange : MonoBehaviour
{
    public static int playerReady = 0;
    bool flag = false;

    private void Update()
    {
        if (playerReady == 3&& !flag)
        {
            flag = true;
            Debug.Log("다음 씬으로 넘어감.");
        }
    }
}
