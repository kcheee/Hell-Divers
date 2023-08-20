using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbySceneChange : MonoBehaviourPun
{
    static public LobbySceneChange instance;

    private void Awake()
    {
        instance = this; 
    }
    public static int playerReady = 0;
    public GameObject spawnPos;
    bool flag = false;

    private void Start()
    {
        GameObject player =  PhotonNetwork.Instantiate("LobbyPlayer", spawnPos.transform.position, Quaternion.identity);
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        if (playerReady == 3&& !flag)
        {
            flag = true;
            Debug.Log("다음 씬으로 넘어감.");
            PhotonNetwork.LoadLevel("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(LobbySceneChange.playerReady);

            //PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    

    [PunRPC]
    public void changeScene()
    {
        playerReady++;
        Debug.Log(playerReady);
    }

}
