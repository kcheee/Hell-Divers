using GLTF.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : Enemy_Fun
{
    #region sigleton
    static public Enemy1 Instance;
    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion

    float currrTime = 0;

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
            case EnemyState.idle: F_idle(); break;
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.ranged_attack: F_rangedattack(); break;
            case EnemyState.melee_attack: F_meleeattack(); break;
        }

    }


    #region ���� �Լ� �ִϸ��̼� ����ٰ�.
    protected void F_idle()
    {
    }

    protected override void F_chase()
    {
        // �ȴ� �ִϸ��̼�
        anim.Play("Walk");
        base.F_chase();
    }

    protected override void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        anim.Play("Walk");

        base.F_patrol();
    }
    // ���Ÿ� ����
    protected override void F_rangedattack()
    {
        // ���ܸ� ����
        base.F_rangedattack();

        currrTime += Time.deltaTime;
        if (currrTime > 2)
        {
            currrTime = 0;
            E_state = EnemyState.chase;
        }

        // �� ���.. 
        // �ؿ��� ���� ��� ����.

    }

    protected override void F_wait()
    {

        // ���� �ִϸ��̼�
        anim.SetBool("walk", false);
        anim.Play("Equip");

        base.F_wait();
    }

    // �ٰŸ� ����
    protected override void F_meleeattack()
    {
        currrTime += Time.deltaTime;

        // �׽�Ʈ��
        if (currrTime > 2)
        {
            currrTime = 0;
            E_state = EnemyState.chase;
        }
    }


    #endregion

    // �ִϸ��̼� event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        Debug.Log("����");
    }
}
