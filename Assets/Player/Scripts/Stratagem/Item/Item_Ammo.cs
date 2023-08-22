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
        //��Ʈ��ũ������ �����ϴϱ� ��Ʈ��ũ������ �����Ѵ�
        //�̷μ� �Ϻ��� ������ ������ ����ȴ�.
        PhotonNetwork.Destroy(gameObject);
    }

    //Ȱ��ȭ �Ǿ�����
    //Player�� Ȱ��ȭ ��ü�� �ڱ� �ڽ����� �Ѵ�.
    protected override void OnActive(PlayerTest1 player)
    {
        base.OnActive(player);
        Debug.Log("��Ƽ�� �Լ��� ����Ǿ���");
        player.currentGemObj = this;
        test = player;
    }

    protected override void OnDeactive(PlayerTest1 player)
    {
        base.OnDeactive(player);
        Debug.Log("���Ƽ�� �Լ��� ����Ǿ���");
        player.currentGemObj = null;
    }

}
