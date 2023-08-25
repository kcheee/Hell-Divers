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
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Debug.Log("½ÇÇà");
            other.gameObject.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other.transform.position, 2);
        }
        if (other.gameObject.tag == "Floor")
        {
            //Debug.Log(collision.transform.position);
            Instantiate(eft, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
