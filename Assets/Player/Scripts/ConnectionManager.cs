using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string StartScene;
    public CanvasGroup loadingUI;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        int rand = Random.Range(0, 4);
        string nick = "";
        switch (rand) {
            case 0:
                nick = "�����Ǳ��";
                break;
            case 1:
                nick = "�ٶ��ǻ�ó";
                break;
            case 2:
                nick = "����ǵ巡��";
                break;
            case 3:
                nick = "�Ҳ���ī����";
                break;
                
        }
        PhotonNetwork.LocalPlayer.NickName = nick;
        StartCoroutine(Fade(true));

        SoundManager.instance.BGMSrc.DOFade(0,1);
    }
    IEnumerator Fade(bool In)
    {
        if (In)
            loadingUI.DOFade(1, 0.5f);
        else
        {
            SoundManager.instance.BGMSrc.clip = SoundManager.instance.BGMClip.Lobby;
            SoundManager.instance.BGMSrc.Play();
            SoundManager.instance.BGMSrc.DOFade(1, 1);
            loadingUI.DOFade(0, 0.5f);
            yield return new WaitForSeconds(1);
            PhotonNetwork.LoadLevel(StartScene);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master");
        base.OnConnectedToMaster();
        //�г��� ����
        //Ÿ�����κ�� ���� ����Ÿ�ӿ� �ִ�.
        TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);

        //Ư�� �κ� ����
        //PhotonNetwork.JoinLobby(typedLobby);

        //�⺻ �κ����Կ�û
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("JOin");
        base.OnJoinedLobby();
        //�κ������ �̵�
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.JoinOrCreateRoom("meta_unity_room", new RoomOptions() { MaxPlayers = 20 }, TypedLobby.Default);  //meta_uniry room ������ ����� ������ ����



    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }
    //�� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
        //�� ���� ���� ������ �����ִ� �˾��� ����.
        Debug.LogError("������ ����,");
    }

    //�� ���� ������ ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        //Game Scene �̵�

        
        StartCoroutine(Fade(false));
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(Fade(false));
        }
    }
}
