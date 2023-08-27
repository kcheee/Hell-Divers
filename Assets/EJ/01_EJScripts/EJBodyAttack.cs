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

    #region player(trigger)와 boss(collision)는 collision으로 안 부딪힘
    private void OnCollisionEnter(Collision collision)
    {
        print("bossBody에 Collision된 것은" + collision);

        if (collision.gameObject.tag == "Player")
        {
            print("Player가 boss에 닿아서 데미지를 입었습니다");
            collision.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, collision, 1);
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        print("bossBody에 Trigger된 것은" + other.gameObject);

        if (other.gameObject.tag == "Player")
        {
            print("Player가 boss에 닿아서 데미지를 입었습니다");
            other.gameObject.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other, 1);
        }
    }
}
