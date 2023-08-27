using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class EJBossHP : MonoBehaviourPun,I_Entity
{
    public static EJBossHP instance;

    float currentHP;
    float maxHP = 300;

    //HP�����̴�
    public Slider bossHPBar;

    //boss ��� �� ������ ȿ��
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
        bossHPBar.maxValue = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HP = 0;
        }

        //???? HP�� ��� ���⼭ üũ���ִ°� �³�..?
        bossHPBar.value = HP;
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

            photonView.RPC("InstantiateDeathFXbyRPC", RpcTarget.All, 1);         

        }  
    }

    [PunRPC]
    public void InstantiateDeathFXbyRPC(int a)
    {
        print("DeathFXbyRPC �Լ��� ����Ǿ����ϴ�");
        StartCoroutine(InstantiateDeathFX());
    }

    bool deathexploDone = false;
    IEnumerator InstantiateDeathFX()
    {
        if (!deathexploDone)

                   
        {

            //Gamemanager.instance

            // Ÿ�� UI
            StartCoroutine(Gamemanager.instance.MissionUIOnOff());
            // Ÿ�� �̼� ���������� ����.
            Gamemanager.instance.Bossmission.sprite = Resources.Load<Sprite>("CheckBox");

            // endingscene�� �̵�
            StartCoroutine(Gamemanager.instance.ending());


            print("DeathFX�� ����Ǿ����ϴ�");

            //GameObject bodyexloImpact = PhotonNetwork.Instantiate("EJBossDeath", transform.position + Vector3.up, Quaternion.identity);

            GameObject bodyexloImpact = Instantiate(bodyExploPrefab);

            //bodyexloImpact.transform.localScale = Vector3.one * 10;
            bodyexloImpact.transform.position = transform.position + Vector3.up*1.6f;
            bodyexloImpact.transform.up = transform.up;

            Destroy(gameObject);   
            
            yield return null;
 
            deathexploDone = true;
        }

    }

    public void die(Action action)
    {
        
        throw new NotImplementedException();
    }
}
