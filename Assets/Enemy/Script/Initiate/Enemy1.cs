using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;
using System.Diagnostics;

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

    // �Ѱ� final_IK_enable
    private void OnEnable()
    {
        Gun.SetActive(true);
        //gameObject.GetComponent<FullBodyBipedIK>().enabled = true;
    }

    float currrTime = 0;

    public GameObject Gun;
    public GameObject I_bullet;
    public GameObject I_Muzzle;
    public GameObject I_FirePos;

    bool flag = false;

    AudioSource audioSource;
    // ������Ʈ 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {        
        //target = GameObject.FindWithTag("Player");
        E_state = EnemyState.patrol;
    }

    private void Update()
    {
        // �÷��̾ ������ ����
        if (PlayerManager.instace.PlayerList.Count == 0)
            return;

        if (photonView.IsMine)
        {
            closestObject = FindClosestObject();
            //Debug.Log(closestObject.position);

            distance = Vector3.Distance(this.transform.position, closestObject.transform.position);
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
    }

    // ���Ÿ� ����.
    [PunRPC]
    public void pun_I_RangedAttack()
    {
        StartCoroutine(I_RangedAttack());
    }

    [PunRPC]
    IEnumerator I_RangedAttack()
    {      
        flag = true;
        yield return new WaitForSeconds(0.2f);
        Vector3 pos = new Vector3(transform.position.x, -1.2f, transform.position.z);
        float T = 3;
        for (int i = 0; i < 10; i++)
        {
            T += Random.Range(0.8f, 2f);
            audioSource.Play();

            Vector3 bulletpos = pos + transform.forward * T + transform.right * Random.Range(-1f, 1f);
            GameObject bullet = Instantiate(I_bullet, I_FirePos.transform.position, Quaternion.identity);
            Instantiate(I_Muzzle, I_FirePos.transform.position, Quaternion.identity).transform.parent = transform;

            bullet.transform.parent = transform;

            //Debug.Log(bullet.transform.position);

            //bullet.transform.forward = TsetG.transform.position - transform.position;
            // �ع������� ���� �����.
            //Debug.Log(bulletpos - transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bulletpos - transform.position) * 12, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        }
    }


    #region ���� �Լ� �ִϸ��̼� ����ٰ�.
    protected void F_idle()
    {

    }

    protected override void F_chase()
    {
        // �ȴ� �ִϸ��̼�
        //anim.Play("Walk");
        photonView.RPC(nameof(PlayAnim_T), RpcTarget.All,"Walk");


        agent.isStopped = false;

        // agent�� ���� �������� target�� ��ġ��
        if(agent.destination!=null)
        agent.destination = closestObject.transform.position;

        // �������� ���� �Ÿ��� ���ʹ�.
        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);

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
        if (distance < 3)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            E_state = EnemyState.melee_attack;
        }
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

        //anim.Play("Ranged_Attack");
        anim.SetTrigger("Ranged_Attack");
        if(!flag)
        {
            photonView.RPC(nameof(pun_I_RangedAttack), RpcTarget.All);
            //StartCoroutine(I_RangedAttack());
        }
        currrTime += Time.deltaTime;
        if (currrTime > 2)
        {
            equip_flag = false;
            flag = false;
            currrTime = 0;
            E_state = EnemyState.chase;
        }

        // �� ���.. 
        // �ؿ��� ���� ��� ����.
    }

    bool equip_flag= false;
    protected override void F_wait()
    {
        // ���� �ִϸ��̼�
        anim.SetBool("walk", false);
        if (!equip_flag)
        {
            anim.Play("Equip");
            //anim.SetTrigger("equip");
            //equip_flag=true;
        }

        // Enemy �չ��� Player�� ���ϰ� ����.
        //transform.forward = target.transform.position - transform.position;
        Vector3.Lerp(transform.forward, closestObject.transform.position - transform.position, 0.1f);
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

        transform.forward = closestObject.transform.position - transform.position;

        // �ִϸ��̼� ��� �� �ٰŸ� ������ �ٲ�.
        // �����ð� �ȿ� �÷��̾ ���� �Ÿ� ���� chase �ϰ� �ٰŸ� �������� �ٲ�.
        // �ٰŸ� �����Ϸ� �i�ư�.
        if (distance < 5)
        {
            E_state = EnemyState.chase;
            currTime = 0;
        }

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
        // �ٰŸ� ����
        //anim.Play("Melee_Attack");
        //photonView.RPC(nameof(PlayAnim_T), RpcTarget.All, "Melee_Attack");

        currrTime += Time.deltaTime;
        // �׽�Ʈ��
        if (currrTime > 2)
        {
            currrTime = 0;
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
