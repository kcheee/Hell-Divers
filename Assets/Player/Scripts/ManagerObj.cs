using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ManagerObj : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        //방장아 나 들어왔어! 방가 방가
        photonView.RPC(nameof(join), RpcTarget.MasterClient);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void join() {
        //PlayerManager.instace.JoinUI();               
    }   
}
