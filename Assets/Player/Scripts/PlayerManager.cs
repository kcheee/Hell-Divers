using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPun
{
    public static PlayerManager instace;
    //4���� ����並 ������ �ְ� public List<Photonview>
    //����Ʈ���� �ڽ��� ����並 �����ͼ� 
    //RPC �Լ��� �����Ѵ�.

    //�ƴϸ� �׳� Player List�� �����͵� �ǰڴ�.
    //Player�� ���¸� ���ؼ�? 
    //���� �÷��̾ ��ȯ�Ѵ�.

    public List<PlayerTest1> PlayerList = new List<PlayerTest1>();
    public AudioClip clip;

    public System.Action<Vector3> action;
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
    Vector3 playerSpawn = new Vector3(220, 0, 250);

    IEnumerator spawn()
    {
        yield return null;
        Debug.Log(PLAYER_LIST.Count);
        //if (PLAYER_LIST.Count == 0)
        //{
        //    Debug.Log("����");
        //    StartSpawn(new Vector3(0, 0, 0));
        //}
        //else
        //{
        //    action += StartSpawn;
        //    Debug.Log("�̰� ����Ǵ���");
        //}
        StartSpawn(playerSpawn + new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)));

    }
    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.instance.BgmPlay(clip);

        instace = this;
        //RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;
        //OnPhotonSeriallizeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        Debug.Log("��ŸƮ �Լ� ����!!");


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
    public void StartSpawn(Vector3 pos)
    {
        int rand = Random.Range(-10, 10);
        pos.x += rand;
        pos.z += rand;

        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != "Lobby")
        {

            GameObject PlatformObj = PhotonNetwork.Instantiate("Platform-Main", pos + Vector3.up * 30, Quaternion.Euler(-89.98f, 0, 0));
            //GameObject player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos , Quaternion.identity);
            //player.SetActive(false);

            Platform platform = PlatformObj.GetComponent<Platform>();

            platform.action = () =>
            {
                Debug.Log("�÷��̾� ��ȯ");
                GameObject player = PhotonNetwork.Instantiate("AlphaPlayer 1", pos, Quaternion.identity); PhotonNetwork.Destroy(PlatformObj);
            };

        }

        //action -= StartSpawn;

        //if (PLAYER_LIST.Count >= 1)
        //PlatformObj.transform.position = PLAYER_LIST[0].gameObject.transform.position;
    }

    public void Addlist(PlayerTest1 player)
    {
        Debug.Log("�������ƴ���");
        PLAYER_LIST.Add(player);
        if (action != null)
        {
            Debug.Log(PLAYER_LIST[0].transform.position);
            action(PLAYER_LIST[0].transform.position);
        }

    }
}
