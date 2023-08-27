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

            // lerp를 쓰면 clmap가 안먹네
            transform.DOLookAt(PLAYER.transform.position, 2);

            //// 현재 카메라의 회전값을 Euler 각도로 변환
            //Vector3 currentRotation = transform.eulerAngles;

            //// X 축 회전 값을 클램핑
            //currentRotation.x = Mathf.Clamp(currentRotation.x, minXAngle, maxXAngle);

            //// Y 축 회전 값을 클램핑
            //currentRotation.y = Mathf.Clamp(currentRotation.y, minYAngle, maxYAngle);

            //// z축 회전 값 clamp
            //currentRotation.z = Mathf.Clamp(currentRotation.z, minZAngle, maxZAngle);

            //// 클램핑된 회전값을 적용
            ////transform.LookAt(PLAYER.transform.position);

            //transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);

        }
    }
}
