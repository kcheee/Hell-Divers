using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Enemy 공통된 기능.
public class Enemy_Fun : EnemyInfo
{

    public enum EnemyState
    {
        idle,
        patrol,
        chase,
        wait,
        attack1,
        attack2,
        hit,
        die
    }
    public EnemyState E_state;

    protected GameObject target;
    protected NavMeshAgent agent;

    int targetIndex =0;
    protected virtual void F_patrol()
    {
        Vector3 target = patrolT[targetIndex].transform.position;
        Debug.Log(agent);
        agent.SetDestination(target);

        // 만약 목적지에 도착했다면(두지점의 거리가 0.1M이하라면)
        target.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, target);
        if (dist <= 0.1f)
        {
            // 인덱스를 1증가시키고싶다.
            targetIndex++;
            // 만약 인덱스가 points배열의 크기이상이되면 0으로 초기화 하고싶다.
            if (targetIndex >=2)
            {
                targetIndex = 0;
            }
        }
    }

    protected virtual void F_chase()
    {
        // agent야 너의 목적지는 target의 위치야
        agent.destination = target.transform.position;

        // 목적지와 나의 거리를 재고싶다.
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        // 만약 그 거리가 공격가능거리(attackRange)보다 작다면
        if (distance < ENEMYATTACK.attackRange)
        {
            // 공격상태로 전이하고싶다.
            E_state = EnemyState.wait;
            //anim.SetTrigger("Attack");
            // agent야 멈춰!!
            agent.isStopped = true;
        }
        // 만약 추격중에 플레이어와의 거리가 포기거리보다 크다면
        else if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // 순찰 상태로 전이하고싶다.
        }
    }
    // 공격1을 위한 대기시간.
    protected virtual void F_wait()
    {
        

    }

    protected virtual void F_rotation()
    {

    }

    protected virtual void F_attack1()
    {

    }

}
