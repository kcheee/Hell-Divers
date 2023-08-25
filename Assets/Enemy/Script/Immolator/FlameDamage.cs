using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Debug.Log("½ÇÇà");
            other.gameObject.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other.transform.position, 2);
        }

    }
}
