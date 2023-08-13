using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Immolator : Enemy_Fun
{

    #region sigleton
    static public Immolator Instance;
    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion

    float currrTime = 0;

public 

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
        }
    }

    // 화염방사 공격.
    IEnumerator I_RangedAttack()
    {
        FlameAttack.instance.enabled = true;
        yield return  null;
    }


    #region 상태 함수 애니메이션 여기다가.

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
        if (distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션
        anim.Play("Walk");

        base.F_patrol();
    }
    // 원거리 공격
    protected override void F_rangedattack()
    {
        // 원겨리 공격
        base.F_rangedattack();

        anim.Play("Ranged_Attack");
        if (!flag)
            StartCoroutine(I_RangedAttack());
        currrTime += Time.deltaTime;

        if (currrTime > 2)
        {
            flag = false;
            currrTime = 0;
            E_state = EnemyState.chase;
        }

        // 총 쏘기.. 
        // 밑에서 위로 쏘는 형식.
    }

    protected override void F_wait()
    {
        // 장전 애니메이션
        anim.SetBool("walk", false);
        anim.Play("Equip");
        //Debug.Log("tlfgod");
        // Enemy 앞방향 Player를 향하게 설정.
        //transform.forward = target.transform.position - transform.position;
        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);

        f_rotation();
        currTime += Time.deltaTime;
        // 총을 꺼내는 시간.
        //Debug.Log("총 꺼냄.");

        // 총 꺼내는 애니메이션

        // 일정 시간이 지나면 F_rangedattack으로
        //if(currTime > 2)
        //{
        //    currTime = 0;
        //    E_state = EnemyState.ranged_attack;        
        //}

        transform.forward = target.transform.position - transform.position;

        // 애니메이션 취소 후 근거리 공격을 바꿈.
        // 일정시간 안에 플레이어가 일정 거리 오면 chase 하고 근거리 공격으로 바꿈.
        // 근거리 공격하러 쫒아감.
        //if (distance < 8)
        //{
        //    E_state = EnemyState.chase;
        //    currTime = 0;
        //}

        // 공격 거리 벗어났을 경우
        if (distance > ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }

    // 근거리 공격
    protected override void F_meleeattack()
    {
        // 근거리 공격
        anim.Play("Melee_Attack");
        currrTime += Time.deltaTime;
        // 테스트용
        if (currrTime > 2)
        {
            currrTime = 0;
            E_state = EnemyState.chase;
        }
    }


    #endregion

    #region Animation Event
    // 애니메이션 event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        //Debug.Log("실행");
    }

    #endregion

}
