using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBossHP : MonoBehaviourPun,I_Entity
{
    public static EJBossHP instance;

    float currentHP;
    float maxHP = 300;

    public GameObject bodyExploPrefab;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        //InstantiateDeathFX();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HP = 0;
        }
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
            print("BOSS HP�� 0���Ϸ� ��������");

            StartCoroutine(InstantiateDeathFX());
            if (deathexploDone)
            {
                Destroy(gameObject);
            }
        }  
    }

    bool deathexploDone = false;
    IEnumerator InstantiateDeathFX()
    {
        if (!deathexploDone)
        {
            print("DeathFX�� ����Ǿ����ϴ�");
            GameObject bodyexloImpact = PhotonNetwork.Instantiate("DeadExplo", transform.position + Vector3.up, Quaternion.identity);

            //GameObject bodyexloImpact = Instantiate(bodyExploPrefab);
            bodyexloImpact.transform.localScale = Vector3.one * 30;

            bodyexloImpact.SetActive(true);

            yield return new WaitForSeconds(0.1f);
            deathexploDone = true;
       
        }

    }

    public void die(Action action)
    {
        
        throw new NotImplementedException();
    }
}
