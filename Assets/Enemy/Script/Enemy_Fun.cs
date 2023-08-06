using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Enemy ����� ���.
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
        // agent�� ���� �������� target�� ��ġ��
        agent.destination = target.transform.position;

        // �������� ���� �Ÿ��� ���ʹ�.
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        // ���� �� �Ÿ��� ���ݰ��ɰŸ�(attackRange)���� �۴ٸ�
        if (distance < ENEMYATTACK.attackRange)
        {
            // ���ݻ��·� �����ϰ�ʹ�.
            E_state = EnemyState.attack1;
            //anim.SetTrigger("Attack");
            // agent�� ����!!
            agent.isStopped = true;
        }
        // ���� �߰��߿� �÷��̾���� �Ÿ��� ����Ÿ����� ũ�ٸ�
        else if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // ���� ���·� �����ϰ�ʹ�.
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
