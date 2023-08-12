using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    protected PlayerTest1 Player;


    //������ 

    //�������� ��ó�� ������.
    //�÷��̾�� ������ ��ü�ϱ� ���⼭ ó�����ִ°� ���ٰ� �����ߴ�.
    //�浹������, �� ������ �浹�� ��ü���� �ְ�ʹ�.
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
    //�ڽ����� Ȱ��ȭ�� ��������? 
    protected virtual void OnActive(PlayerTest1 player)
    {
        Debug.Log("Get�Լ�����");
    }

    protected virtual void OnDeactive(PlayerTest1 player)
    {
        Debug.Log("Get�Լ�����");
    }



}
