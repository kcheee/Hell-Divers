using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerManager : MonoBehaviourPun
{
    public static PlayerManager instace;
    //4명의 포톤뷰를 가지고 있고 public List<Photonview>
    //리스트에서 자신의 포톤뷰를 가져와서 
    //RPC 함수를 실행한다.

    //아니면 그냥 Player List를 가져와도 되겠다.
    //Player의 상태를 비교해서? 
    //죽은 플레이어를 소환한다.

    public List<PlayerTest1> PlayerList = new List<PlayerTest1>();
    public AudioClip clip;

    public System.Action<Vector3> action;
    public List<PlayerTest1> PLAYER_LIST
    {
        get {  return PlayerList;  }
        set { PlayerList = value;
            
            
        }
    }
    //RPC 함수 실행?

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.BgmPlay(clip);
        Debug.Log(PlayerList.Count);
        instace = this;
        //RPC 호출 빈도
        PhotonNetwork.SendRate = 60;
        //OnPhotonSeriallizeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        Debug.Log("스타트 함수 실행!!");
        //너 혼자니?
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            Debug.Log("혼스폰 ㅠㅠ");
            StartSpawn(Vector3.zero);
        }
        else {
            Debug.Log("HOHOHOOHOHOHOHOHOO");
            action += StartSpawn;
        }

    }
    
    IEnumerator CheckList() {
        yield return null;
    }

    private void Update()
    {
        //Debug.Log(PLAYER_LIST[0].transform.position);
    }

    //잘못설계했음! 이건 RPC였다.
    public void StartSpawn(Vector3 pos)
    {
        int rand = Random.Range(-2, 2);
        pos.x += rand;
        pos.z += rand;

        GameObject PlatformObj = PhotonNetwork.Instantiate("Platform-Main", pos + Vector3.up * 30, Quaternion.Euler(-89.98f, 0, 0));
        //GameObject player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos , Quaternion.identity);
        //player.SetActive(false);

        Platform platform = PlatformObj.GetComponent<Platform>();
        platform.action = () => { GameObject player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos, Quaternion.identity); PhotonNetwork.Destroy(PlatformObj); };
        //action -= StartSpawn;

        //if (PLAYER_LIST.Count >= 1)
            //PlatformObj.transform.position = PLAYER_LIST[0].gameObject.transform.position;
    }

    public void Addlist(PlayerTest1 player ) {
        PLAYER_LIST.Add(player);
        if (action != null) {
            Debug.Log(PLAYER_LIST[0].transform.position);
            action(PLAYER_LIST[0].transform.position);
        }
            
    }
}
