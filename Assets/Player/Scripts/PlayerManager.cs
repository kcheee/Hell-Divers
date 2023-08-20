using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
        get {  return PlayerList;  }
        set { PlayerList = value;
            
            
        }
    }
    //RPC �Լ� ����?

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.BgmPlay(clip);
        Debug.Log(PlayerList.Count);
        instace = this;
        //RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;
        //OnPhotonSeriallizeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        Debug.Log("��ŸƮ �Լ� ����!!");

        //�� ȥ�ڴ�?
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            Debug.Log("ȥ���� �Ф�");
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

    //�߸���������! �̰� RPC����.
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
