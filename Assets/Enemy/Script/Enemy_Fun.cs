using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

// Enemy ����� ���.
public class Enemy_Fun : EnemyInfo
{
    // Enemy ����
    public enum EnemyState
    {
        idle,
        patrol,
        chase,
        wait,
        melee_attack,
        ranged_attack,
        hit,
        die
    }
    public EnemyState E_state;

    // �÷��̾� Ÿ�� (���߿� �����ؾ���.)
    protected GameObject target;

    // agent
    protected NavMeshAgent agent;

    // animator
    protected Animator anim;

    // patrol
    int targetIndex = 0;
    
    // �ð� �����̸� �ֱ� ���� ����
    float currTime = 0;

    protected virtual void F_patrol()
    {
        // �ȴ� �ִϸ��̼�

        // patrol �ϴ� target
        Vector3 target = patrolT[targetIndex].transform.position;
        agent.SetDestination(target);

        // ���� �������� �����ߴٸ�(�������� �Ÿ��� 0.1M���϶��)
        target.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, target);

        if (dist <= 0.1f)
        {
            // �ε����� 1������Ű��ʹ�.
            targetIndex++;
            // ���� �ε����� points�迭�� ũ���̻��̵Ǹ� 0���� �ʱ�ȭ �ϰ�ʹ�.
            if (targetIndex >= 2)
            {
                targetIndex = 0;
            }
        }

        // attackRange��ŭ ��������� ����.
        if (distance < ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
        }

    }

    protected virtual void F_chase()
    {
        agent.isStopped = false;

        // agent�� ���� �������� target�� ��ġ��
        agent.destination = target.transform.position;

        // �������� ���� �Ÿ��� ���ʹ�.
         distance = Vector3.Distance(this.transform.position, target.transform.position);

        // ���Ÿ� ���ݺ��� �۰� �ٰŸ� ���� �Ÿ����� ũ�� wait��
        if (distance < ENEMYATTACK.ranged_attack_possible &&
            distance > ENEMYATTACK.melee_attack_possible)
        {
            // ���ݻ��·� �����ϰ�ʹ�.
            E_state = EnemyState.wait;
            //anim.SetTrigger("Attack");
            // agent�� ����!!
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        // ���� �߰��߿� �÷��̾���� �Ÿ��� ����Ÿ����� ũ�ٸ�
        else if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // ���� ���·� �����ϰ�ʹ�.
        }

        // �ٰŸ� ���� �Ÿ����� ������ �ٰŸ� ���� ��Ȳ���� �ٲ�.
        if(distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            E_state = EnemyState.melee_attack;
        }
    }

    // ����1�� ���� ���ð�.
    protected virtual void F_wait()
    {
        // Enemy �չ��� Player�� ���ϰ� ����.
        //transform.forward = target.transform.position - transform.position;
        Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);
        currTime += Time.deltaTime;
        // ���� ������ �ð�.
        //Debug.Log("�� ����.");

        // �� ������ �ִϸ��̼�

        // ���� �ð��� ������ F_rangedattack����
        //if(currTime > 2)
        //{
        //    currTime = 0;
        //    E_state = EnemyState.ranged_attack;        
        //}

        transform.forward = target.transform.position- transform.position;

        // �ִϸ��̼� ��� �� �ٰŸ� ������ �ٲ�.
        // �����ð� �ȿ� �÷��̾ ���� �Ÿ� ���� chase �ϰ� �ٰŸ� �������� �ٲ�.
        // �ٰŸ� �����Ϸ� �i�ư�.
        if (distance < 8)
        {
            E_state =EnemyState.chase;
            currTime = 0;
        }

        // ���� �Ÿ� ����� ���
        if(distance > ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
            currTime = 0;
        }

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


    protected virtual void F_rotation()
    {

    }

}
