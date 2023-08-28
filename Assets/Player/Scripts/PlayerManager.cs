using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager instace;
    //4���� ����並 ������ �ְ� public List<Photonview>
    //����Ʈ���� �ڽ��� ����並 �����ͼ� 
    //RPC �Լ��� �����Ѵ�.

    //�ƴϸ� �׳� Player List�� �����͵� �ǰڴ�.
    //Player�� ���¸� ���ؼ�? 
    //���� �÷��̾ ��ȯ�Ѵ�.

    public List<PlayerTest1> PlayerList = new List<PlayerTest1>();
    public List<PlayerTest1> DeathList = new List<PlayerTest1>();
    public AudioClip clip;

    public PlayerInfoObj PlayerInfoUI;

    public System.Func<Vector3, GameObject> action;


    public List<PlayerTest1> PLAYER_LIST
    {
        get { return PlayerList; }
        set
        {

            PlayerList = value;


        }
    }
    //RPC �Լ� ����?

    // mainscene���� spawn
    Vector3 playerSpawn = new Vector3(225, 0, 245);

    IEnumerator spawn()
    {

        yield return null;
        Debug.Log(PLAYER_LIST.Count);
        if (PLAYER_LIST.Count == 0)
        {

            StartSpawn(playerSpawn);

        }
        else
        {
            action += StartSpawn;
        }

        //StartSpawn(playerSpawn + new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)));

    }

    private void Awake()
    {
        instace = this;
    }

    void Start()
    {


        if (SceneManager.GetActiveScene().name != "Lobby") {
            //PhotonNetwork.Instantiate("PlayerInfoObj",Vector3.zero,Quaternion.identity);
            //PlayerInfoUI = JoinUI(PhotonNetwork.LocalPlayer.NickName);
            //JoinUI();
        }
            //SoundManager.instance.BgmPlay(clip);

            
        //RPC ȣ�� ��
        PhotonNetwork.SendRate = 30;
        //OnPhotonSeriallizeView ȣ�� ��
        PhotonNetwork.SerializationRate = 30;

        //�� ȥ�ڴ�?
        StartCoroutine(spawn());

        //if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        //{
        //    Debug.Log("ȥ���� �Ф�");
        //    StartSpawn(Vector3.zero);
        //}
        //else
        //{
        //    StartCoroutine(delay());
        //    Debug.Log("HOHOHOOHOHOHOHOHOO");
        //    action += StartSpawn;
        //}
    }

    IEnumerator CheckList()
    {
        yield return null;
    }

    private void Update()
    {
        //Debug.Log(PLAYER_LIST[0].transform.position);
    }

    //�߸���������! �̰� RPC����.
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
            //addspawnPos += 5;
            Platform platform = PlatformObj.GetComponent<Platform>();
            GameObject player = null;
            platform.action = () =>
            {
                player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos, Quaternion.identity); //PhotonNetwork.Destroy(PlatformObj);
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
    //���ö� UI�� �ڽ��� �̸��� ����.
    public PlayerInfoObj JoinUI(string name) {
        Transform tr = PlayerUI.instance.PlayerInfo;
        GameObject obj =  Instantiate(UI_Obj, tr);
        PlayerInfoObj info = obj.GetComponent<PlayerInfoObj>();
        info.NameText.text = name;
        return info;
    }
}
