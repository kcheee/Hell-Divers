using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject go;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            go.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, go.transform.position, 1);
        }
    }

}
