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
            if (player.photonView.IsMine)
            {
                GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            }
            //    GameObject playerObj = PlayerManager.instace.StartSpawn(pos);
            //    PhotonView view = playerObj.GetComponent<PhotonView>();
            //    //���α�ü 
            //    view.TransferOwnership(player.photonView.Owner);
            //}
            Debug.Log("Player" + player);
            //��¥�� ��ȯ�Ǵϱ� Remove�� ���ÿ�
            PlayerManager.instace.DeathList.Remove(player);
            Destroy(player.gameObject);
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
