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
                    Debug.Log("������ ����");
                    return;

                }
                    */
        Debug.Log("��� �Ǿ����� ����ȴ�");
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
            //������ == ������ �ڱ� �ڽ��϶� PhotonNetwork.Instantiate �� �����ؼ� ismine�� ��ä�� ����.
/*            if (player.photonView.IsMine)
            {
                playerObj = PlayerManager.instace.StartSpawn(pos);
            }
*/
            //�׸��� ��� PC���� �� ������Ʈ�� �����Ѵ�.
            //player.reset(); 
            


            //    GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            //    PhotonView view = playerObj.GetComponent<PhotonView>();
            //    //���α�ü 
            //    view.TransferOwnership(player.photonView.Owner);
            //}
            Debug.Log("Player" + player);
            //��¥�� ��ȯ�Ǵϱ� Remove�� ���ÿ�

            //Destroy(player.gameObject);
        }
        //foreach (PlayerTest1 player in PlayerManager.instace.DeathList)
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        �������� �� �ڽ��̴� ?
        //    if (player.photonView.IsMine)
        //        {
        //            GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
        //        }
        //        GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
        //        PhotonView view = playerObj.GetComponent<PhotonView>();
        //        ���α�ü 
        //        view.TransferOwnership(player.photonView.Owner);
        //    }
        //    Debug.Log("Player" + player);
        //    ��¥�� ��ȯ�Ǵϱ� Remove�� ���ÿ�
        //        PlayerManager.instace.DeathList.Remove(player);
        //    Destroy(player.gameObject);


        //}
    }
}
