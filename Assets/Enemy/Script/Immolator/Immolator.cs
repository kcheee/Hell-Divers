using Photon.Pun;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Immolator : Enemy_Fun
{

    #region sigleton
    static public Immolator Instance;
    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion

    float currrTime = 0;

    public FlameAttack flameattack;

    bool flag = false;

    // ������Ʈ 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // ���ľ���
        //closestObject = GameObject.Find("Player");
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk",true);
        E_state = EnemyState.patrol;
    }

    private void FixedUpdate()
    {

        // �÷��̾ ������ ����
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if (photonView.IsMine)
        {
            closestObject = FindClosestObject();

            // �÷��̾���� �Ÿ�
            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
            switch (E_state)
            {
                case EnemyState.patrol: F_patrol(); break;
                case EnemyState.wait: F_wait(); break;
                case EnemyState.chase: F_chase(); break;
                case EnemyState.ranged_attack: F_rangedattack(); break;
                case EnemyState.melee_attack: F_meleeattack(); break;
            }
        }
    }

    [PunRPC]
    void pun_I_RangedAttack()
    {
        StartCoroutine(I_RangedAttack());
        //Debug.Log("����");
    }
    // ȭ����� ����.
    [PunRPC]
    IEnumerator I_RangedAttack()
    {

        flameattack.enabled = true;
        yield return  null;
    }


    #region ���� �Լ� �ִϸ��̼� ����ٰ�.

    protected override void F_chase()
    {
        // �ȴ� �ִϸ��̼�
        //anim.Play("Walk");
        agent.isStopped = false;

        // agent�� ���� �������� target�� ��ġ��
        agent.destination = closestObject.transform.position;

        // �������� ���� �Ÿ��� ���ʹ�.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);

        // ���Ÿ� ���ݺ��� �۰� �ٰŸ� ���� �Ÿ����� ũ�� wait��
        if (distance < ENEMYATTACK.ranged_attack_possible &&
            distance > ENEMYATTACK.melee_attack_possible)
        {
            photonView.RPC(nameof(PlayAnim_T), RpcTarget.All, "Equip");

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
        if (distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);

            E_state = EnemyState.melee_attack;
        }
    }

    protected override void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        //anim.Play("Walk");
        base.F_patrol();
    }
    // ���Ÿ� ����
    protected override void F_rangedattack()
    {
        // ���ܸ� ����
        base.F_rangedattack();

        anim.Play("Ranged_Attack");
        if (!flag)
        {
            photonView.RPC(nameof(pun_I_RangedAttack), RpcTarget.All);

            //StartCoroutine(I_RangedAttack());
        }
        currrTime += Time.deltaTime;

        if (currrTime > 2)
        {
            flag = false;
            currrTime = 0;
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);

            E_state = EnemyState.chase;
        }

        // �� ���.. 
        // �ؿ��� ���� ��� ����.
    }

    protected override void F_wait()
    {
        // ���� �ִϸ��̼�
        //anim.Play("Equip");

        //Debug.Log("tlfgod");
        // Enemy �չ��� Player�� ���ϰ� ����.
        //transform.forward = target.transform.position - transform.position;
        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);

        f_rotation();
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

        // �ִϸ��̼� ��� �� �ٰŸ� ������ �ٲ�.
        // �����ð� �ȿ� �÷��̾ ���� �Ÿ� ���� chase �ϰ� �ٰŸ� �������� �ٲ�.
        // �ٰŸ� �����Ϸ� �i�ư�.
        //if (distance < 8)
        //{
        //    E_state = EnemyState.chase;
        //    currTime = 0;
        //}

        // ���� �Ÿ� ����� ���
        if (distance > ENEMYATTACK.attackRange)
        {
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }

    // �ٰŸ� ����
    protected override void F_meleeattack()
    {
        // �ٰŸ� ���� ���ϴ� �ɷ�.
         
        currrTime += Time.deltaTime;
        // �׽�Ʈ��
        if (currrTime > 2)
        {
            currrTime = 0;
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk",true);

            E_state = EnemyState.chase;
        }
    }


    #endregion

    #region Animation Event
    // �ִϸ��̼� event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        //Debug.Log("����");
    }

    #endregion

}
