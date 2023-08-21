using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Spawn_Stratagem : Stratagems
{
    
    // Start is called before the first frame update
    void Start()
    {
        start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SpawnAction(Vector3 pos, Quaternion rot)
    {
        base.SpawnAction(pos, rot);
/*        if (!PhotonNetwork.IsMasterClient) {
            Debug.Log("마스터 실행");
            return;
            
        }
            */
        Debug.Log("rkwk");
        int count = PlayerManager.instace.DeathList.Count;
        foreach (PlayerTest1 player in PlayerManager.instace.DeathList) {
            Debug.Log("가자ㅏ!!");
            if (player.GetComponent<PhotonView>().IsMine) {
                GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
                PhotonView view = playerObj.GetComponent<PhotonView>();
                view = null;
                view = player.GetComponent<PhotonView>();
                PlayerManager.instace.DeathList.Remove(player);
                Destroy(player.gameObject);
            }
                
        }
    }
}
