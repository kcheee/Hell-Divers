using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    bool flag = false;
    GameObject PLAYER;

    private float minXAngle=47;
    private float maxXAngle= 53;
    private int minYAngle = 75;
    private int maxYAngle= 100;
    private int minZAngle = -1;
    private int maxZAngle = 10;

    void Update()
    {
        if (PlayerManager.instace.PlayerList.Count == 0) return;

        if(!flag)
            foreach (PlayerTest1 player in PlayerManager.instace.PlayerList)
            {
                if (player.transform.GetComponent<PhotonView>().IsMine)
                {
                    PLAYER = player.gameObject;
                    flag= true;
                    Debug.Log(PLAYER);

                }
            }
        else
        {

            //transform.LookAt(PLAYER.transform);

            // lerp�� ���� clmap�� �ȸԳ�
            transform.DOLookAt(PLAYER.transform.position, 2);

            //// ���� ī�޶��� ȸ������ Euler ������ ��ȯ
            //Vector3 currentRotation = transform.eulerAngles;

            //// X �� ȸ�� ���� Ŭ����
            //currentRotation.x = Mathf.Clamp(currentRotation.x, minXAngle, maxXAngle);

            //// Y �� ȸ�� ���� Ŭ����
            //currentRotation.y = Mathf.Clamp(currentRotation.y, minYAngle, maxYAngle);

            //// z�� ȸ�� �� clamp
            //currentRotation.z = Mathf.Clamp(currentRotation.z, minZAngle, maxZAngle);

            //// Ŭ���ε� ȸ������ ����
            ////transform.LookAt(PLAYER.transform.position);

            //transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);

        }
    }
}
