using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

// Boss ����� ���.
public class Boss_Fun: EnemyInfo
{
    // Boss ����
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

    // �÷��̾� Ÿ�� (���߿� �����ؾ���.)
    protected GameObject target;

    // agent
    protected NavMeshAgent agent;

    // animator
    protected Animator anim;

    // patrol
    int targetIndex = 0;

    // �ð� �����̸� �ֱ� ���� ����
    protected float currTime = 0;
    Vector3 P_targt;


    protected virtual void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        // patrol �ϴ� target

        // patrolT�� null�� ��Ȳ�� �д����� ȣ������ ��� �ۿ� ���⿡ �̷��� «.
        if (patrolT[targetIndex] == null)
        {
            B_state = BossState.Chase;
        }
        else
            P_targt = patrolT[targetIndex].transform.position;

        agent.SetDestination(P_targt);

        //Debug.Log(P_targt);
        // ���� �������� �����ߴٸ�(�������� �Ÿ��� 0.1M���϶��)
        P_targt.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, P_targt);

        if (dist <= 0.3f)
        {
            // �ε����� 1������Ű���ʹ�.
            targetIndex++;
            // ���� �ε����� points�迭�� ũ���̻��̵Ǹ� 0���� �ʱ�ȭ �ϰ��ʹ�.
            if (targetIndex >= 2)
            {
                targetIndex = 0;
            }
        }

        // attackRange��ŭ ��������� ����.
        if (distance < ENEMYATTACK.attackRange)
        {
            B_state = BossState.Chase;
        }

    }

    protected virtual void F_chase()
    {

    }

    // ����1�� ���� ���ð�.
    protected virtual void F_wait()
    {
    }

    protected virtual void F_rangedattack()
    {
        // �ִϸ��̼� ���� �� chase�� �ٲ�.

        // chase 
    }
    protected virtual void F_meleeattack()
    {
        // �ִϸ��̼� ���� �� 

        // chase�� �ٲ�.
    }

    // ȸ���ϴ� �Լ�
    protected virtual void f_rotation()
    {
        Vector3 currentDirection = transform.forward;
        Vector3 targetDirection = target.transform.position - transform.position;

        // ���� ���� ����� ��ǥ ������ �ٸ��ٸ� ȸ�� ����
        if (Vector3.Dot(currentDirection.normalized, targetDirection.normalized) < 0.99f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ENEMY.turnSpeed);

            transform.rotation = newRotation;
        }
    }


    // navmeshagent ����
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