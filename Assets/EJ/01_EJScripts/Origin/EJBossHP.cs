using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBossHP : MonoBehaviourPun,I_Entity
{
    public static EJBossHP instance;

    float currentHP;
    float maxHP = 300000;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float HP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
        }
    }

    public void BOSS_HP_DAMAGE(float damage)
    {
        if (HP > 0)
        {
            HP -= damage;
        }
    }


    //collider�� �θ� ������Ʈ�� �÷ȴ��� �ذ�Ǿ���
    [PunRPC]
    public void damaged(Vector3 pos, int damage = 0)
    {
        Debug.Log("damage�Լ� ����");
        if (HP > 0)
        {
            HP -= damage;
            Debug.Log("������ ���� ü����"+ currentHP);
        }
        else
        {
            Destroy(gameObject);
        }
  
    }

    public void die(Action action)
    {
        throw new NotImplementedException();
    }
}
