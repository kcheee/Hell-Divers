using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    protected PlayerTest1 Player;


    //아이템 

    //아이템의 근처에 갔을때.
    //플레이어는 동적인 객체니까 여기서 처리해주는게 좋다고 생각했다.
    //충돌했을때, 내 정보를 충돌한 객체한테 주고싶다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTest1 Pl = other.GetComponent<PlayerTest1>();
            Player = Pl;
            OnActive(Pl);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            PlayerTest1 Pl = other.GetComponent<PlayerTest1>();
            Player = null;
            OnDeactive(Pl);
        }
    }
    //자식한테 활성화를 보내겠죠? 
    protected virtual void OnActive(PlayerTest1 player)
    {
        Debug.Log("Get함수실행");
    }

    protected virtual void OnDeactive(PlayerTest1 player)
    {
        Debug.Log("Get함수실행");
    }



}
