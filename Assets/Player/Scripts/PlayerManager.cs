using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instace;
    //4���� ����並 ������ �ְ� public List<Photonview>
    //����Ʈ���� �ڽ��� ����並 �����ͼ� 
    //RPC �Լ��� �����Ѵ�.

    //�ƴϸ� �׳� Player List�� �����͵� �ǰڴ�.
    //Player�� ���¸� ���ؼ�? 
    //���� �÷��̾ ��ȯ�Ѵ�.

    public List<PlayerTest1> PlayerList = new List<PlayerTest1>();
    //RPC �Լ� ����?

    // Start is called before the first frame update
    void Start()
    {
        instace = this;
        //RPC ȣ�� ��
        PhotonNetwork.SendRate = 30;
        //OnPhotonSeriallizeView ȣ�� ��
        PhotonNetwork.SerializationRate = 30;
        GameObject player =  PhotonNetwork.Instantiate("AlphaPlayer 1", Vector3.zero,Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
