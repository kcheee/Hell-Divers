using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBodyAttack : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region player(trigger)�� boss(collision)�� collision���� �� �ε���
    private void OnCollisionEnter(Collision collision)
    {
        print("bossBody�� Collision�� ����" + collision);

        if (collision.gameObject.tag == "Player")
        {
            print("Player�� boss�� ��Ƽ� �������� �Ծ����ϴ�");
            collision.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, collision, 1);
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        print("bossBody�� Trigger�� ����" + other);

        if (other.gameObject.tag == "Player")
        {
            print("Player�� boss�� ��Ƽ� �������� �Ծ����ϴ�");
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other, 1);
        }
    }
}
