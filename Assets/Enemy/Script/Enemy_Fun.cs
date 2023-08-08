using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

// Enemy 공통된 기능.
public class Enemy_Fun : EnemyInfo
{
    // Enemy 상태
    public enum EnemyState
    {
        idle,
        patrol,
        chase,
        wait,
        melee_attack,
        EnemyInSight,   // SquadLeader
        ranged_attack,
        hit,
        die
    }
    public EnemyState E_state;

    // 플레이어 타겟 (나중에 수정해야함.)
    protected GameObject target;

    // agent
    protected NavMeshAgent agent;

    // animator
    protected Animator anim;

    // patrol
    int targetIndex = 0;
    
    // 시간 딜레이를 주기 위한 변수
    float currTime = 0;

    protected virtual void F_patrol()
    {
        // 걷는 애니메이션

        // patrol 하는 target
        Vector3 target = patrolT[targetIndex].transform.position;
        agent.SetDestination(target);

        // 만약 목적지에 도착했다면(두지점의 거리가 0.1M이하라면)
        target.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, target);

        if (dist <= 0.3f)
        {
            // 인덱스를 1증가시키고싶다.
            targetIndex++;
            // 만약 인덱스가 points배열의 크기이상이되면 0으로 초기화 하고싶다.
            if (targetIndex >= 2)
            {
                targetIndex = 0;
            }
        }

        // attackRange만큼 가까워지면 추적.
        if (distance < ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
        }

    }

    protected virtual void F_chase()
    {
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
        if(distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            E_state = EnemyState.melee_attack;
        }
    }

    // 공격1을 위한 대기시간.
    protected virtual void F_wait()
    {
        // Enemy 앞방향 Player를 향하게 설정.
        //transform.forward = target.transform.position - transform.position;
        Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);
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

        transform.forward = target.transform.position- transform.position;

        // 애니메이션 취소 후 근거리 공격을 바꿈.
        // 일정시간 안에 플레이어가 일정 거리 오면 chase 하고 근거리 공격으로 바꿈.
        // 근거리 공격하러 쫒아감.
        if (distance < 8)
        {
            E_state =EnemyState.chase;
            currTime = 0;
        }

        // 공격 거리 벗어났을 경우
        if(distance > ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }

    protected virtual void F_rangedattack()
    {
        // 애니메이션 진행 후 chase로 바꿈.
        
        // chase 
    }
    protected virtual void F_meleeattack()
    {


        // 애니메이션 실행 후 

        // chase로 바꿈.
    }


    protected virtual void F_rotation()
    {

    }

}
