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
                nick = "무명의기사";
                break;
            case 1:
                nick = "바람의상처";
                break;
            case 2:
                nick = "어둠의드래곤";
                break;
            case 3:
                nick = "불꽃의카이저";
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
