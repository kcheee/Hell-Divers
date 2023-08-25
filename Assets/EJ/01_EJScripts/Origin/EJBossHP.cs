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


    //collider를 부모 오브젝트로 올렸더니 해결되었음
    [PunRPC]
    public void damaged(Vector3 pos, int damage = 0)
    {
        Debug.Log("damage함수 실행");
        if (HP > 0)
        {
            HP -= damage;
            Debug.Log("보스의 현재 체력은"+ currentHP);
        }
        else
        {
            print("BOSS HP가 0이하로 떨어졌다");

            photonView.RPC("InstantiateDeathFXbyRPC", RpcTarget.All, 1);
            
          
        }  
    }

    [PunRPC]
    public void InstantiateDeathFXbyRPC(int a)
    {
        print("DeathFXbyRPC 함수가 실행되었습니다");
        StartCoroutine(InstantiateDeathFX());
    }

    bool deathexploDone = false;
    IEnumerator InstantiateDeathFX()
    {
        if (!deathexploDone)
        {
            print("DeathFX가 실행되었습니다");
            //GameObject bodyexloImpact = PhotonNetwork.Instantiate("EJBossDeath", transform.position + Vector3.up, Quaternion.identity);

            GameObject bodyexloImpact = Instantiate(bodyExploPrefab);
            bodyexloImpact.transform.localScale = Vector3.one * 10;
            bodyexloImpact.transform.position = transform.position;
            bodyexloImpact.transform.up = transform.up;

            //bodyexloImpact.SetActive(true);

            yield return new WaitForSeconds(2f);
            deathexploDone = true;

            if (deathexploDone)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

    }

    public void die(Action action)
    {
        
        throw new NotImplementedException();
    }
}
