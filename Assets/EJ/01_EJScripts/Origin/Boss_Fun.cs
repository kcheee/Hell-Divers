using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

// Boss 공통된 기능.
public class Boss_Fun: EnemyInfo
{
    // Boss 상태
    public enum BossState
    {
        Idle,
        Patrol,
        Chase,
        Wait,
        Attack_GausCannon,
        Attack_machineGun,
        Attack_Bomb,
        Die
    }

    public BossState B_state;

    // 플레이어 타겟 (나중에 수정해야함.)
    protected GameObject target;

    // agent
    protected NavMeshAgent agent;

    // animator
    protected Animator anim;

    // patrol
    int targetIndex = 0;

    // 시간 딜레이를 주기 위한 변수
    protected float currTime = 0;
    Vector3 P_targt;


    protected virtual void F_patrol()
    {
        // 걷는 애니메이션
        // patrol 하는 target

        // patrolT가 null인 상황은 분대장이 호출했을 경우 밖에 없기에 이렇게 짬.
        if (patrolT[targetIndex] == null)
        {
            B_state = BossState.Chase;
        }
        else
            P_targt = patrolT[targetIndex].transform.position;

        agent.SetDestination(P_targt);

        //Debug.Log(P_targt);
        // 만약 목적지에 도착했다면(두지점의 거리가 0.1M이하라면)
        P_targt.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, P_targt);

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
            B_state = BossState.Chase;
        }

    }

    protected virtual void F_chase()
    {

    }

    // 공격1을 위한 대기시간.
    protected virtual void F_wait()
    {
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

    // 회전하는 함수
    protected virtual void f_rotation()
    {
        Vector3 currentDirection = transform.forward;
        Vector3 targetDirection = target.transform.position - transform.position;

        // 만약 현재 방향과 목표 방향이 다르다면 회전 실행
        if (Vector3.Dot(currentDirection.normalized, targetDirection.normalized) < 0.99f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ENEMY.turnSpeed);

            transform.rotation = newRotation;
        }
    }


    // navmeshagent 설정
    #region
    protected void TraceNavSetting()
    {
        this.agent.isStopped = false;
        this.agent.updatePosition = true;
        this.agent.updateRotation = true;
    }

    protected void StopNavSetting()
    {
        this.agent.isStopped = true;
        this.agent.updatePosition = false;
        this.agent.updateRotation = false;
        this.agent.velocity = Vector3.zero;
    }
    #endregion
}
