using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master");
        base.OnConnectedToMaster();
        //닉네임 설정
        //타이프로비는 포톤 리얼타임에 있다.
        TypedLobby typedLobby = new TypedLobby("Meta Lobby", LobbyType.Default);

        //특정 로비 진입
        //PhotonNetwork.JoinLobby(typedLobby);

        //기본 로비진입요청
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("JOin");
        base.OnJoinedLobby();
        //로비씬으로 이동
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.JoinOrCreateRoom("meta_unity_room", new RoomOptions() { MaxPlayers = 20 }, TypedLobby.Default);  //meta_uniry room 없으면 만들고 있으면 들어가고



    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(nameof(OnCreatedRoom));
    }
    //방 생성 실패시 호출되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(nameof(OnCreateRoomFailed));
        //방 생성 실패 원인을 보여주는 팝업을 띄운다.
        Debug.LogError("에러가 났음,");
    }

    //방 참여 성공시 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(nameof(OnJoinedRoom));
        //Game Scene 이동
        PhotonNetwork.LoadLevel("Lobby");
    }
}
