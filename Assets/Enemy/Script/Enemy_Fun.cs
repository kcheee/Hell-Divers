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

    int targetIndex =0;
    protected virtual void F_patrol()
    {
        Vector3 target = patrolT[targetIndex].transform.position;
        Debug.Log(agent);
        agent.SetDestination(target);

        // ���� �������� �����ߴٸ�(�������� �Ÿ��� 0.1M���϶��)
        target.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, target);
        if (dist <= 0.1f)
        {
            // �ε����� 1������Ű��ʹ�.
            targetIndex++;
            // ���� �ε����� points�迭�� ũ���̻��̵Ǹ� 0���� �ʱ�ȭ �ϰ�ʹ�.
            if (targetIndex >=2)
            {
                targetIndex = 0;
            }
        }
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
            E_state = EnemyState.wait;
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
    // ����1�� ���� ���ð�.
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
