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

    // ����
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

    // ������Ʈ 
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
        // �÷��̾���� �Ÿ�
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


    #region ��ź �߻�

    // �÷���� �߻�
    IEnumerator Squad_Flare()
    {
        flare_flag = false;
        yield return new WaitForSeconds(2); // ���Ƿ�


        Instantiate(Flare, FirePos.transform.position, Quaternion.identity);

        // �ִϸ��̼� ���� ��
        yield return new WaitForSeconds(2); // ���Ƿ�
        flag = true;
    }

    #endregion
    #region �����Լ�
    protected override void F_chase()
    {
        // �ȴ� �ִϸ��̼�
        anim.Play("Walk");

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
            // agent stop
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
    }
    protected override void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        anim.Play("Walk");
        base.F_patrol();
    }

    protected override void F_wait()
    {
        anim.SetBool("walk", false);
        anim.Play("Equip");

        // agent �̲����� ����
        StopNavSetting();

        // Enemy �չ��� Player�� ���ϰ� ����.
        // ȸ��
        f_rotation();

        currTime += Time.deltaTime;
        // ���� ������ �ð�.
        //Debug.Log("�� ����.");

        // �� ������ �ִϸ��̼�

        // ���� �ð��� ������ F_rangedattack����
        if (currTime > 2)
        {
            currTime = 0;
            E_state = EnemyState.ranged_attack;
        }


        // �ִϸ��̼� ��� �� �ٰŸ� ������ �ٲ�.
        // �����ð� �ȿ� �÷��̾ ���� �Ÿ� ���� chase �ϰ� �ٰŸ� �������� �ٲ�.
        // �ٰŸ� �����Ϸ� �i�ư�.
        if (distance < 18)
        {
            // agent �ٽ� ����
            TraceNavSetting();
            E_state = EnemyState.escape;
            currTime = 0;
        }

        // ���� �Ÿ� ����� ���
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

        // �������� ���� �ڵ�
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
        // ���ܸ� ����
        base.F_rangedattack();
        agent.velocity= Vector3.zero;

        f_rotation();

        // �÷��� �� | �ѹ��� �����ϱ� ���� �ڵ�
        if (flare_flag)
            StartCoroutine(Squad_Flare());
        // ��ź �߻�.
        if(flag&& !flare_flag)
        {
            // ��ź�߻�
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

        // �� ���.. 
        // �ؿ��� ���� ��� ����.
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
        // ��������
        Debug.Log("��������");
        if(distance > 4)
        {
            TraceNavSetting();
            E_state = EnemyState.escape;
        }
        // escape �����ؾ���.
    }
    #endregion

    #region animation event

    public void equip_end()
    {
        //E_state = EnemyState.ranged_attack;
    }

    #endregion

}


