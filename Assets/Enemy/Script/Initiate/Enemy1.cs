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

    // 컴포넌트 
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
        // 플레이어와의 거리
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

    // 원거리 공격.
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
            // 밑방향으로 힘을 줘야함.
            Debug.Log(bulletpos - transform.position);
            bullet.GetComponent<Rigidbody>().AddForce((bulletpos - transform.position) * 15, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        }
    }


    #region 상태 함수 애니메이션 여기다가.
    protected void F_idle()
    {
    }

    protected override void F_chase()
    {
        // 걷는 애니메이션
        anim.Play("Walk");
        base.F_chase();
    }

    protected override void F_patrol()
    {
        // 걷는 애니메이션
        anim.Play("Walk");

        base.F_patrol();
    }
    // 원거리 공격
    protected override void F_rangedattack()
    {
        // 원겨리 공격
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

        // 총 쏘기.. 
        // 밑에서 위로 쏘는 형식.

    }

    protected override void F_wait()
    {
        // 장전 애니메이션
        anim.SetBool("walk", false);
        anim.Play("Equip");

        base.F_wait();
    }

    // 근거리 공격
    protected override void F_meleeattack()
    {
        // 근거리 공격
        anim.Play("Melee_Attack");
        currrTime += Time.deltaTime;
        // 테스트용
        if (currrTime > 2)
        {
            currrTime = 0;
            E_state = EnemyState.chase;
        }
    }


    #endregion


    #region Animation Event
    // 애니메이션 event
    public void equip_end()
    {
        E_state = EnemyState.ranged_attack;
        Debug.Log("실행");
    }

    #endregion
}
