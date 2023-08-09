using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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

    public GameObject I_bullet;
    public GameObject I_FirePos;

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
            case EnemyState.idle: F_idle(); break;
            case EnemyState.patrol: F_patrol(); break;
            case EnemyState.wait: F_wait(); break;
            case EnemyState.chase: F_chase(); break;
            case EnemyState.ranged_attack: F_rangedattack(); break;
            case EnemyState.melee_attack: F_meleeattack(); break;
        }
    }

    // ���Ÿ� ����.
    IEnumerator I_RangedAttack()
    {
        flag = true;
        yield return new WaitForSeconds(0.2f);
        Vector3 pos = new Vector3(transform.position.x, -1.2f, transform.position.z);

        float T = 3;
        for (int i = 0; i < 10; i++)
        {
            T += Random.Range(0.8f, 2f);

            Vector3 bulletpos = pos + transform.forward * T + transform.right * Random.Range(-1f, 1f);
            GameObject bullet = Instantiate(I_bullet, I_FirePos.transform.position, Quaternion.identity);

            //bullet.transform.forward = TsetG.transform.position - transform.position;
            // �ع������� ���� �����.
            Debug.Log(bulletpos - transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bulletpos - transform.position) * 15, ForceMode.Impulse);

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

        anim.Play("Ranged_Attack");
        if(!flag)
        StartCoroutine(I_RangedAttack());
        currrTime += Time.deltaTime;
        if (currrTime > 2)
        {
            flag = false;
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
        // �ٰŸ� ����
        anim.Play("Melee_Attack");
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
        Debug.Log("����");
    }

    #endregion
}
