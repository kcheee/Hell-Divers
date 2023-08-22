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
        Debug.Log("모든 피씨에서 실행된다");
        int count = PlayerManager.instace.DeathList.Count;

        List<PlayerTest1> list = PlayerManager.instace.DeathList;
        for (int i = 0; i < list.Count; i++) {
            PlayerTest1 player = list[i];
            if (player.photonView.IsMine)
            {
                GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            }
            //    GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            //    PhotonView view = playerObj.GetComponent<PhotonView>();
            //    //주인교체 
            //    view.TransferOwnership(player.photonView.Owner);
            //}
            Debug.Log("Player" + player);
            //어짜피 소환되니까 Remove는 동시에
            PlayerManager.instace.DeathList.Remove(player);
            Destroy(player.gameObject);
        }
        //foreach (PlayerTest1 player in PlayerManager.instace.DeathList)
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        죽은놈이 나 자신이니 ?
        //    if (player.photonView.IsMine)
        //        {
        //            GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
        //        }
        //        GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
        //        PhotonView view = playerObj.GetComponent<PhotonView>();
        //        주인교체 
        //        view.TransferOwnership(player.photonView.Owner);
        //    }
        //    Debug.Log("Player" + player);
        //    어짜피 소환되니까 Remove는 동시에
        //        PlayerManager.instace.DeathList.Remove(player);
        //    Destroy(player.gameObject);


        //}
    }
}
