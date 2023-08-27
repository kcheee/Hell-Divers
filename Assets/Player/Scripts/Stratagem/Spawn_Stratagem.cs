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

            player.reset();
            player.gameObject.transform.position = pos;
            PlayerManager.instace.DeathList.Remove(player);
            player.trBody.gameObject.SetActive(false);


            GameObject platform = Instantiate((GameObject)Resources.Load("Platform-Main"), pos + Vector3.up * 50 , Quaternion.Euler(-90,0,0));
            Platform plat = platform.GetComponent<Platform>();
            plat.action = () => {
                player.trBody.gameObject.SetActive(true);
                player.gameObject.SetActive(true); 
            
            };
            //실행자 == 죽은자 자기 자신일때 PhotonNetwork.Instantiate 를 실행해서 ismine이 된채로 시작.
/*            if (player.photonView.IsMine)
            {
                playerObj = PlayerManager.instace.StartSpawn(pos);
            }
*/
            //그리고 모든 PC에서 이 컴포넌트를 재사용한다.
            //player.reset(); 
            


            //    GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            //    PhotonView view = playerObj.GetComponent<PhotonView>();
            //    //주인교체 
            //    view.TransferOwnership(player.photonView.Owner);
            //}
            Debug.Log("Player" + player);
            //어짜피 소환되니까 Remove는 동시에

            //Destroy(player.gameObject);
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
