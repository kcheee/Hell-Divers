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

        // master�� ���� �ѱ�� ���� �Ѿ.
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        if (playerReady >= 3&& !flag)
        {
            flag = true;
            Debug.Log("���� ������ �Ѿ.");
            PhotonNetwork.LoadLevel("MainScene");
        }
        //�׽�Ʈ�� �� �ѱ�.
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(LobbySceneChange.playerReady);
            // playermanager�� �ִ� ��� ��� ���� �� �̵�.
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
