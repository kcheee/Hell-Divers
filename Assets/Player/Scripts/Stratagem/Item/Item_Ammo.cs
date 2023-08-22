using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Item_Ammo : Item,I_StratagemObject
{
    PlayerTest1 test;
    public void Add()
    {
        test.mainGun.Current_Manganize = 10;
        I_Destroy();
    }
    public void I_Destroy()
    {

        Player.currentGemObj = null;
        //Destroy(gameObject);
        //네트워크측에서 생성하니까 네트워크측에서 삭제한다
        //이로서 완벽한 생성과 삭제가 보장된다.
        PhotonNetwork.Destroy(gameObject);
    }

    //활성화 되었을때
    //Player의 활성화 객체를 자기 자신으로 한다.
    protected override void OnActive(PlayerTest1 player)
    {
        base.OnActive(player);
        Debug.Log("액티브 함수가 실행되었다");
        player.currentGemObj = this;
        test = player;
    }

    protected override void OnDeactive(PlayerTest1 player)
    {
        base.OnDeactive(player);
        Debug.Log("디액티브 함수가 실행되었다");
        player.currentGemObj = null;
    }

}
