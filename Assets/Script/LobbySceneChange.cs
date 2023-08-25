using Photon.Pun;
using Photon.Realtime;
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
    public GameObject panel;
    bool flag = false;

    private void Start()
    {
        GameObject player =  PhotonNetwork.Instantiate("AlphaPlayer 1", spawnPos.transform.position, Quaternion.identity);
        player.transform.localScale = new Vector3(3,3,3);

        // master�� ���� �ѱ�� ���� �Ѿ.
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }

    private void Update()
    {
        if (playerReady >= 4&& !flag)
        {
            flag = true;
            Debug.Log("Uiȣ��");
            photonView.RPC(nameof(PanelOn), RpcTarget.All);

            //PhotonNetwork.LoadLevel("MainScene");
        }
        //�׽�Ʈ�� �� �ѱ�.
        if (Input.GetKeyDown(KeyCode.U))
        {
            // playermanager�� �ִ� ��� ��� ���� �� �̵�.
            //PlayerManager.instace.PlayerList.Clear();
            PhotonNetwork.LoadLevel("MainScene");
        }
    }


    [PunRPC]
    void PanelOn()
    {
        panel.SetActive(true);
    }

    [PunRPC]
    public void changeScene()
    {
        playerReady++;
        Debug.Log(playerReady);
    }


}
