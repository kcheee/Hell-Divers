using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
    public List<PlayerTest1> DeathList = new List<PlayerTest1>();
    public AudioClip clip;

    public System.Func<Vector3, GameObject> action;


    public List<PlayerTest1> PLAYER_LIST
    {
        get { return PlayerList; }
        set
        {

            PlayerList = value;


        }
    }
    //RPC 함수 실행?

    // mainscene에서 spawn
    Vector3 playerSpawn = new Vector3(225, 0, 245);

    IEnumerator spawn()
    {

        yield return null;
        Debug.Log(PLAYER_LIST.Count);
        if (PLAYER_LIST.Count == 0)
        {
            StartSpawn(playerSpawn+new Vector3(Random.Range(-5,5),0,Random.Range(-5, 5)));
        }
        else
        {
            action += StartSpawn;
        }

        //StartSpawn(playerSpawn + new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)));

    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Lobby") {
            JoinUI();
        }
            //SoundManager.instance.BgmPlay(clip);

            instace = this;
        //RPC 호출 빈도
        PhotonNetwork.SendRate = 30;
        //OnPhotonSeriallizeView 호출 빈도
        PhotonNetwork.SerializationRate = 30;

        //너 혼자니?
        StartCoroutine(spawn());

        //if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        //{
        //    Debug.Log("혼스폰 ㅠㅠ");
        //    StartSpawn(Vector3.zero);
        //}
        //else
        //{
        //    StartCoroutine(delay());
        //    Debug.Log("HOHOHOOHOHOHOHOHOO");
        //    action += StartSpawn;
        //}
        SoundManager.instance.BgmPlay(clip);
    }

    IEnumerator CheckList()
    {
        yield return null;
    }

    private void Update()
    {
        //Debug.Log(PLAYER_LIST[0].transform.position);
    }

    //잘못설계했음! 이건 RPC였다.
    public GameObject StartSpawn(Vector3 pos)
    {
        int rand = Random.Range(-10, 10);
        pos.x += rand;
        pos.z += rand;

        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != "Lobby")
        {

            GameObject PlatformObj = PhotonNetwork.Instantiate("Platform-Main", pos + Vector3.up * 30, Quaternion.Euler(-90f, 0, 0));
            //GameObject player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos , Quaternion.identity);
            //player.SetActive(false);

            Platform platform = PlatformObj.GetComponent<Platform>();
            GameObject player = null;
            platform.action = () =>
            {
                player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos, Quaternion.identity); PhotonNetwork.Destroy(PlatformObj);
            };
            return player;
        }
        return null;
        //action -= StartSpawn;

        //if (PLAYER_LIST.Count >= 1)
        //PlatformObj.transform.position = PLAYER_LIST[0].gameObject.transform.position;
    }

    public void Addlist(PlayerTest1 player)
    {
        PLAYER_LIST.Add(player);
        if (action != null)
        {
            action(PLAYER_LIST[0].transform.position);
        }
    }

    public GameObject UI_Obj;
    //들어올때 UI에 자신의 이름을 넣자.
    public void JoinUI() {
        Transform tr = PlayerUI.instance.PlayerInfo;
        GameObject obj =  Instantiate(UI_Obj, tr);
        PlayerInfoObj info = obj.GetComponent<PlayerInfoObj>();
        info.NameText.text = PhotonNetwork.LocalPlayer.NickName.ToString();

    }

}
