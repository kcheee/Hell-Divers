using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class SquadLeader : Enemy_Fun
{

    #region sigleton
    static public SquadLeader Instance;

    // 공격
    public GameObject Flare;
    public GameObject FirePos;
    public GameObject GranadeLancher;
    bool flare_flag = true;

    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion

    float currrTime = 0;

    bool flag = false;

    // 컴포넌트 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        target = GameObject.Find("Player");
        E_state = EnemyState.patrol;
    }

    private void Update()
    {
        // 플레이어와의 거리
        distance = Vector3.Distance(this.transform.position, target.transform.position);
        switch (E_state)
        {
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.ranged_attack: F_rangedattack(); break;
            case EnemyState.melee_attack: F_meleeattack(); break;
            case EnemyState.escape: F_escape(); break;
        }
    }


    #region 유탄 발사

    // 플레어건 발사
    IEnumerator Squad_Flare()
    {
        flare_flag = false;
        yield return new WaitForSeconds(2); // 임의로


        Instantiate(Flare, FirePos.transform.position, Quaternion.identity);

        // 애니메이션 실행 후
        yield return new WaitForSeconds(2); // 임의로
        flag = true;
    }

    #endregion
    #region 상태함수
    protected override void F_chase()
    {
        // 걷는 애니메이션
        anim.Play("Walk");

        agent.isStopped = false;

        // agent야 너의 목적지는 target의 위치야
        agent.destination = target.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        distance = Vector3.Distance(this.transform.position, target.transform.position);

        // 원거리 공격보다 작고 근거리 공격 거리보다 크면 wait로
        if (distance < ENEMYATTACK.ranged_attack_possible &&
            distance > ENEMYATTACK.melee_attack_possible)
        {
            // 공격상태로 전이하고싶다.
            E_state = EnemyState.wait;
            //anim.SetTrigger("Attack");
            // agent stop
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        // 만약 추격중에 플레이어와의 거리가 포기거리보다 크다면
        else if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // 순찰 상태로 전이하고싶다.
        }

        // 근거리 공격 거리보다 작으면 근거리 공격 상황으로 바꿈.
    }
    protected override void F_patrol()
    {
        // 걷는 애니메이션
        anim.Play("Walk");
        base.F_patrol();
    }

    protected override void F_wait()
    {
        anim.SetBool("walk", false);
        anim.Play("Equip");

        // agent 미끄러짐 방지
        StopNavSetting();

        // Enemy 앞방향 Player를 향하게 설정.
        // 회전
        f_rotation();

        currTime += Time.deltaTime;
        // 총을 꺼내는 시간.
        //Debug.Log("총 꺼냄.");

        // 총 꺼내는 애니메이션

        // 일정 시간이 지나면 F_rangedattack으로
        if (currTime > 2)
        {
            currTime = 0;
            E_state = EnemyState.ranged_attack;
        }


        // 애니메이션 취소 후 근거리 공격을 바꿈.
        // 일정시간 안에 플레이어가 일정 거리 오면 chase 하고 근거리 공격으로 바꿈.
        // 근거리 공격하러 쫒아감.
        if (distance < 18)
        {
            // agent 다시 세팅
            TraceNavSetting();
            E_state = EnemyState.escape;
            currTime = 0;
        }

        // 공격 거리 벗어났을 경우
        if (distance > ENEMYATTACK.attackRange)
        {
            TraceNavSetting();
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }
    protected void F_escape()
    {
        anim.Play("Walk");

        // 도망가기 위한 코드
        Vector3 fleeDirection = transform.position - target.transform.position;
        Vector3 targetPosition = transform.position + fleeDirection.normalized * 10;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 10, NavMesh.AllAreas))
        {
            agent.destination = hit.position;

        }
        if (distance >= ENEMYATTACK.ranged_attack_possible-10)
        {
            E_state = EnemyState.wait;
        }
        if (distance < 3)
        {
            StopNavSetting();
            E_state = EnemyState.melee_attack;
        }
        //break;
    }

    protected override void F_rangedattack()
    {
        // 원겨리 공격
        base.F_rangedattack();
        agent.velocity= Vector3.zero;

        f_rotation();

        // 플레어 건 | 한번만 실행하기 위한 코드
        if (flare_flag)
            StartCoroutine(Squad_Flare());
        // 유탄 발사.
        if(flag&& !flare_flag)
        {
            // 유탄발사
            Instantiate(GranadeLancher,FirePos.transform.position, Quaternion.identity);
            flag = false;
        }

        //anim.Play("Flare");

        currrTime += Time.deltaTime;
        if (currrTime > 3.5f)
        {
            flag = true;
            currrTime = 0;
            TraceNavSetting();        
            E_state = EnemyState.chase;
        }

        // 총 쏘기.. 
        // 밑에서 위로 쏘는 형식.
    }
    Quaternion newRotation;
    protected override void F_meleeattack()
    {
        anim.SetBool("walk", false);

        if (transform.forward != (target.transform.position - transform.position))
        {
            f_rotation();
            //Debug.Log(transform.rotation + " " + newRotation);
        }
        else
        {
            Debug.Log("tlfgo");
        }
        // 근접공격
        Debug.Log("근접공격");
        if(distance > 4)
        {
            TraceNavSetting();
            E_state = EnemyState.escape;
        }
        // escape 설정해야함.
    }
    #endregion

    #region animation event

    public void equip_end()
    {
        //E_state = EnemyState.ranged_attack;
    }

    #endregion

}


