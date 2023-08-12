using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EJBossStateMgr : MonoBehaviour
{
    public static EJBossStateMgr instance;

    public enum State
    {
       Patrol,
       Chase,
       Attack,
       Die
    }

    public State state;

    //navigation
    NavMeshAgent nav;
    GameObject player;

    //Animation
    public Animator anim;

    //Player와의 거리
    float DistanceBoss2Player;
    float Attack_SDistance = 1f;
    float Attack_MDistance = 2f;
    float Attack_LDistance = 3f;
    float Attack_XLDistance = 5f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        DistanceBoss2Player = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                UpdateDie();
                break;
        }
    }



    public void ChangeState(State s)
    {
        if (state == s) return;

        state = s;

    }



    private void UpdatePatrol()
    {
    }


    private void UpdateChase()
    {
        nav.SetDestination(player.transform.position);
        
        //공격가능범위 안으로 들어오면 Attack
        if (DistanceBoss2Player <= Attack_XLDistance)
        {
            state = State.Attack;
            anim.SetTrigger("Attack");
        }
    }

    private void UpdateAttack()
    {
        if (DistanceBoss2Player <= Attack_SDistance)
        {
            //붙여주기만 하면 실행되지 않겠지
            GetComponent<EJBodyExplosion>();
        }
        else if (DistanceBoss2Player > Attack_SDistance && DistanceBoss2Player <= Attack_MDistance)
        {
            StartCoroutine(GetComponent<EJBombFire>().MakeBomb());
        }
        else if (DistanceBoss2Player > Attack_MDistance && DistanceBoss2Player <= Attack_LDistance)
        {
            StartCoroutine(GetComponent<EJMachineGun>().MachineGunFire());
        }
        else if (DistanceBoss2Player > Attack_LDistance && DistanceBoss2Player <= Attack_XLDistance)
        {
            StartCoroutine(GetComponent<EJGausCannonFire>().CannonFire());
        }

        //공격 범위에서 벗어나면 Chase모드
        if (DistanceBoss2Player> Attack_XLDistance)                
        {
            state = State.Chase;
            anim.SetTrigger("Chase");
        }
    }

    private void UpdateDie()
    {
       if(EJBossHP.instance.HP<0)
        state = State.Die;
        anim.SetTrigger("Die");
    }
}













































