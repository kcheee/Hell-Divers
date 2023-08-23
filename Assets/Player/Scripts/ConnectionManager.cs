using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string StartScene;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = "USER1234";
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
        PhotonNetwork.LoadLevel(StartScene);
    }
}
