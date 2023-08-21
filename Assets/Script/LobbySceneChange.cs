using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // master가 씬을 넘기면 같이 넘어감.
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        if (playerReady >= 3&& !flag)
        {
            flag = true;
            Debug.Log("다음 씬으로 넘어감.");
            PhotonNetwork.LoadLevel("MainScene");
        }
        //테스트용 씬 넘김.
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(LobbySceneChange.playerReady);
            // playermanager에 있는 모든 요소 삭제 후 이동.
            //PlayerManager.instace.PlayerList.Clear();
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

    

    [PunRPC]
    public void changeScene()
    {
        playerReady++;
        Debug.Log(playerReady);
    }

}
