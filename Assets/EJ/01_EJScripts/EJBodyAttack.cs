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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player가 boss에 닿아서 데미지를 입었습니다");
            collision.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, collision, 1);
        }
    }
}
