using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFSM : MonoBehaviour
{
    public static BossFSM instance;

    public enum BossState
    {
        Patrol,
        Chase,
        Attack,
        Wait,
        Die
    }

    public BossState B_state;

    //navigation
    NavMeshAgent nav;
    GameObject player;

    //Animation
    //public Animator anim;

    //Player���� �Ÿ�
    public float DistanceBoss2Player;
    public float Attack_SDistance = 1f;
    public float Attack_MDistance = 2f;
    public float Attack_LDistance = 3f;
    public float Attack_XLDistance = 5f;

    bool flag = false;

    //time
    float curTime = 0;
    float waitTime = 3f;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        //nav.transform.forward =transform.forward;

        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
        B_state = BossState.Chase;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceBoss2Player = Vector3.Distance(transform.position, player.transform.position);

        switch (B_state)
        {
            case BossState.Patrol:
                UpdatePatrol();
                break;
            case BossState.Chase:
                UpdateChase();
                break;
            case BossState.Wait:
                UpdateWait();
                break;
            case BossState.Attack:
                UpdateAttack();
                break;
            case BossState.Die:
                UpdateDie();
                break;
        }
    }



    public void ChangeState(BossState s)
    {
        if (B_state == s) return;

        B_state = s;

    }



    private void UpdatePatrol()
    {

    }


    private void UpdateChase()
    {
        OnNavMesh();
        //nav.SetDestination(player.transform.position);
        nav.destination = player.transform.position;


        //���ݰ��ɹ��� ������ ������ Attack
        if (DistanceBoss2Player <= Attack_XLDistance)
        {
            print("����XLDistance�� ���Ծ��");
            B_state = BossState.Wait;
            //anim.SetTrigger("Attack");
        }
    }

    private void UpdateAttack()
    {
        //DistanceBoss2Player = Vector3.Distance(transform.position, player.transform.position);

        //curTime = 0;
        //print("AttackState�� ���Խ��ϴ�");

        if (DistanceBoss2Player <= Attack_SDistance &&!flag )
        {
            print(" 1= " + DistanceBoss2Player);
            //�ٿ��ֱ⸸ �ϸ� ������� �ʰ���
           StartCoroutine(transform.GetComponent<EJBombFire>().MakeBomb());
            flag = true;
        }
    /*    else if (DistanceBoss2Player > Attack_SDistance && DistanceBoss2Player <= Attack_MDistance)
        {
            print("2= " + DistanceBoss2Player);
            StartCoroutine(GetComponent<EJBombFire>().MakeBomb());
        }
        else if (DistanceBoss2Player > Attack_MDistance && DistanceBoss2Player <= Attack_LDistance)
        {
            print("3= " + DistanceBoss2Player);
            StartCoroutine(GetComponent<EJMachineGun>().MachineGunFire());
        }
        else if (DistanceBoss2Player > Attack_LDistance && DistanceBoss2Player <= Attack_XLDistance)
        {
            print("4= " + DistanceBoss2Player);
            StartCoroutine(GetComponent<EJGausCannonFire>().CannonFire());
        }*/

        //���� �������� ����� Chase���
        if (DistanceBoss2Player > Attack_XLDistance)
        {
            print("Attack�� �� �ִ� �Ÿ��� �ƴմϴ�");
            B_state = BossState.Chase;
            //anim.SetTrigger("Chase");
        }
    }

    private void UpdateDie()
    {
        if (EJBossHP.instance.HP < 0)
            B_state = BossState.Die;
        //anim.SetTrigger("Die");
    }

    private void UpdateWait()
    {
        OffNavMesh();

        curTime += Time.deltaTime;

        if (curTime > waitTime)
        {
            //OffNavMesh();
            //���� Ʈ���Ÿ� �ߵ��Ѵ�? ���� ���� �����Ѵٸ� ��� �ϴ� ����?
            B_state = BossState.Attack;
            curTime = 0;
        }
    }

    void OnNavMesh()
    {
        this.nav.isStopped = false;
        this.nav.updatePosition = true;
        this.nav.updateRotation = true;
    }

    void OffNavMesh()
    {
        this.nav.velocity = Vector3.zero;
        this.nav.isStopped = true;
        this.nav.updatePosition = false;
        this.nav.updateRotation = false;

    }
}


