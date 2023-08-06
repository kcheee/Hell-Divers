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

    protected virtual void F_patrol()
    {


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
            E_state = EnemyState.attack1;
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
