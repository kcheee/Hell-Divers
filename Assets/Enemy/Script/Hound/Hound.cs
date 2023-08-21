using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;
using Photon.Pun;

public class Hound : Enemy_Fun
{

    #region �̱���
    static public Hound instance;
    private void Awake()
    {
        instance = this;
        getcomponent();
    }
    #endregion

    // ���� ����
    float currrTime = 0;
    bool flag = false;

    #region ������Ʈ ������
    // ������Ʈ ��������
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    #endregion

    private void Start()
    {
        // ���ľ���
        //closestObject = GameObject.Find("Player");
        E_state = EnemyState.patrol;
    }

    private void FixedUpdate()
    {
        // �÷��̾ ������ ����
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if (photonView.IsMine)
        {
            // ����� �÷��̾� ����.
            closestObject = FindClosestObject();

            // �÷��̾���� �Ÿ�
            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
            switch (E_state)
            {
                case EnemyState.patrol: F_patrol(); break;
                case EnemyState.wait: F_wait(); break;
                case EnemyState.chase: F_chase(); break;
                case EnemyState.melee_attack: F_meleeattack(); break;
            }
        }
    }


    #region ���� �Լ� �ִϸ��̼� ����ٰ�.
    protected override void F_chase()
    {
        // �ȴ� �ִϸ��̼�

        agent.speed = 8;

        if(agent != null) 
        agent.isStopped = false;

        // agent�� ���� �������� target�� ��ġ��
        agent.destination = closestObject.transform.position;

        // �������� ���� �Ÿ��� ���ʹ�.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);


        Vector3 toPlayer = closestObject.transform.position - transform.position;
        toPlayer.y = 0f; // y�� ����

        // �÷��̾�� ���� ������ ���� ���
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // �þ߰� 30�� �ȿ� �÷��̾ ������
        if (angleToPlayer < 30)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run",true);
            //anim.SetBool("Run", true);
        }
        else
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Run", false);
            //anim.SetBool("Run", false);
            E_state = EnemyState.wait;
        }


        // ���� �߰��߿� �÷��̾���� �Ÿ��� ����Ÿ����� ũ�ٸ�
        if (distance > ENEMYATTACK.farDistance)
        {
            E_state = EnemyState.patrol;
            // ���� ���·� �����ϰ�ʹ�.
        }

        // �ٰŸ� ���� �Ÿ����� ������ �ٰŸ� ���� ��Ȳ���� �ٲ�.
        if (distance < 5 && angleToPlayer < 25)
        {
            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
        //anim.SetBool("Walk", true);

        agent.speed = 4;
        base.F_patrol();
    }
    // ���Ÿ� ����

    protected override void F_wait()
    {

        // agent ����
        StopNavSetting();

        Vector3 toPlayer = closestObject.transform.position - transform.position;
        toPlayer.y = 0f; // y�� ����

        // �÷��̾�� ���� ������ ���� ���
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        f_rotation();

        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.2f);

        // �÷��̾�� ������ ����ϸ� agent�ٽ� enable
        if (angleToPlayer < 10)
        {
            // agent �ٽ� ����.
            TraceNavSetting();
            E_state = EnemyState.chase;
        }
    }

    // �ٰŸ� ����
    protected override void F_meleeattack()
    {
        // ��Ÿ��.

        Vector3 toPlayer = closestObject.transform.position - transform.position;
        toPlayer.y = 0f; // y�� ����

        // �÷��̾�� ���� ������ ���� ���
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // ���� ����
        if (angleToPlayer <= 10&&!flag)
        {
            // 1. agent ���߰�
            // 2. ���� ����
            // 3. agent �ٽ� ����.
            StopNavSetting();

            HoundAttack_anim.Hound_anim_falg = false;

            Vector3 targetPosition = transform.position + transform.forward * 8;
            agent.enabled = false;

            transform.DOMove(targetPosition, 1.3f).OnComplete(() => {
                //Debug.Log(agent.transform.position);
                agent.enabled = true;
                // �� ���ǹ��� �ѹ� �����ϱ� ���� flag
                flag = false;
                TraceNavSetting();
                E_state = EnemyState.chase;

            });

            photonView.RPC(nameof(PlayAnim_T), RpcTarget.All, "Attack");
            //anim.SetTrigger("Attack");

            // attack �ִϸ��̼��� ������
            // �ݶ��̴� on
            flag = true;
        }
        //else if (angleToPlayer > 10)
        //{
        //    TraceNavSetting();
        //    E_state = EnemyState.chase;
        //}

        // �ִϸ��̼��� ����ǰ� ������ �����̸� �ȉ�.
        if (distance > 7&&HoundAttack_anim.Hound_anim_falg)
        {
            agent.enabled = true;
            TraceNavSetting();

            E_state = EnemyState.chase;
        }

    }


    #endregion

    #region Animation Event

    #endregion



}
