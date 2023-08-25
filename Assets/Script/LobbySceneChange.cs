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

        // master가 씬을 넘기면 같이 넘어감.
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }

    private void Update()
    {
        if (playerReady >= 4&& !flag)
        {
            flag = true;
            Debug.Log("Ui호출");
            photonView.RPC(nameof(PanelOn), RpcTarget.All);

            //PhotonNetwork.LoadLevel("MainScene");
        }
        //테스트용 씬 넘김.
        if (Input.GetKeyDown(KeyCode.U))
        {
            // playermanager에 있는 모든 요소 삭제 후 이동.
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
