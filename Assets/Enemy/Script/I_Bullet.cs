using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Bullet : MonoBehaviour
{
    public GameObject eft;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.ToString());
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log(collision.transform.position);
            Instantiate(eft,gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("½ÇÇà");    

            collision.gameObject.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, collision.transform.position, 2);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if()
    }
}
