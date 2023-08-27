using DG.Tweening;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SquadLeader : Enemy_Fun
{

    #region sigleton & ����
    static public SquadLeader Instance;

    public float escape=15;
    // ����
    public GameObject Flare;
    public GameObject FirePos;
    public GameObject Granade;
    public GameObject E_Initiate;
    public int spawnPos = 5;
    bool flare_flag = true;
    bool flag = false;
    bool Equip_flag = false;
    float currrTime = 0;

    AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        getcomponent();
    }
    #endregion


    

    // ������Ʈ 
    void getcomponent()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // ���ľ���
        //closestObject = GameObject.Find("Player");
        photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
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
                case EnemyState.escape: F_escape(); break;
            }
        }

    }


    #region ��ź �߻�

    // �÷���� �߻�
    [PunRPC]
    void pun_Squad_Flare()
    {
        StartCoroutine(Squad_Flare());
    }

    IEnumerator Squad_Flare()
    {
        flare_flag = false;
        anim.SetTrigger("Flare");
        yield return new WaitForSeconds(1.5f); // ���Ƿ�

        yield return new WaitForSeconds(1f); // ���Ƿ�

        audioSource.PlayOneShot(ENEMY.sound_Normal[0], 1);
        //ENEMY.sound_Normal[0].
        Instantiate(Flare, FirePos.transform.position, Quaternion.identity);

        // �÷������ ���� ��ġ ���

        Vector3 FlarePo = transform.position;
        Quaternion FlareRo = transform.rotation;
        // �ִϸ��̼� ���� ��
        yield return new WaitForSeconds(2); // ���Ƿ�
        flag = true;
        yield return new WaitForSeconds(4);
        // �̵����� ��ȯ.

        //float angle = 360 / spawnPos;
        //Transform tf = gameObject.transform;
        //for (int i = 0; i < spawnPos; i++)
        //{
        //    Vector3 Po = FlarePo + new Vector3(Random.Range(-4f, 4), Random.Range(-4f, 4), Random.Range(-4f, 4));

        //    PhotonNetwork.Instantiate("Initiate_E-main", Po, Quaternion.identity);

        //    yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        //}

    }

    [PunRPC]
    void pun_FireGrenada(Vector3 pos)
    {
        StartCoroutine(FireGrenada(pos));
    }
    IEnumerator FireGrenada(Vector3 POS)
    {
        anim.SetTrigger("Ranged_Attack");
        flag = false;
        yield return new WaitForSeconds(1.1f);
        audioSource.PlayOneShot(ENEMY.sound_Normal[1], 1);
        GameObject Grenda = Instantiate(Granade, FirePos.transform.position, Quaternion.identity);
        Grenda.GetComponent<GranadeLauncher>().value(POS);
    }

    #endregion
    #region �����Լ�
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
            // ���ݻ��·� �����ϰ�ʹ�.
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);

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
        //anim.Play("Walk");

        base.F_patrol();
    }
    
    protected override void F_wait()
    {

        //anim.Play("Equip");
        
        // �ѹ��� �����ؾ���.
        if(!Equip_flag)
        {
            photonView.RPC(nameof(PlayAnimP), RpcTarget.All, "Equip");
            Equip_flag = true;
        }


        // agent �̲����� ����
        StopNavSetting();

        // Enemy �չ��� Player�� ���ϰ� ����.
        // ȸ��
        f_rotation();

        //Vector3.Lerp(transform.forward, target.transform.position - transform.position, 0.1f);

        currTime += Time.deltaTime;
        // ���� ������ �ð�.
        //Debug.Log("�� ����.");

        // �� ������ �ִϸ��̼�

        // ���� �ð��� ������ F_rangedattack����
        if (currTime > 2f)
        {
            currTime = 0;
            Equip_flag = false;
            E_state = EnemyState.ranged_attack;
        }


        // �ִϸ��̼� ��� �� �ٰŸ� ������ �ٲ�.
        // �����ð� �ȿ� �÷��̾ ���� �Ÿ� ���� chase �ϰ� �ٰŸ� �������� �ٲ�.
        // �ٰŸ� �����Ϸ� �i�ư�.
        if (distance < escape)
        {
            Equip_flag = false;

            // agent �ٽ� ����
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk",true);
            E_state = EnemyState.escape;
            currTime = 0;
        }

        // ���� �Ÿ� ����� ���
        if (distance > ENEMYATTACK.attackRange)
        {
            Equip_flag = false;
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
            currTime = 0;
        }

    }
    protected void F_escape()
    {
        //anim.Play("Walk");

        // �������� ���� �ڵ�
        Vector3 fleeDirection = transform.position - closestObject.transform.position;
        Vector3 targetPosition = transform.position + fleeDirection.normalized * 10;

        NavMeshHit hit;
        // ����ħ
        if (NavMesh.SamplePosition(targetPosition, out hit, 10, NavMesh.AllAreas))
        {
            agent.destination = hit.position;

        }
        if (distance >= ENEMYATTACK.ranged_attack_possible )
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);
            E_state = EnemyState.wait;
        }
        if (distance < 3)
        {
            StopNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", false);
            photonView.RPC(nameof(PlayAnimT), RpcTarget.All, "MeleeAttack");
            E_state = EnemyState.melee_attack;
        }
        //break;
    }

    protected override void F_rangedattack()
    {
        // ���ܸ� ����
        base.F_rangedattack();
        agent.velocity = Vector3.zero;

        f_rotation();

        // �÷��� �� | �ѹ��� �����ϱ� ���� �ڵ�
        if (flare_flag)
        {
            photonView.RPC(nameof(pun_Squad_Flare), RpcTarget.All);

            //StartCoroutine(Squad_Flare());
        }

        // ��ź �߻�.
        if (flag && !flare_flag)
        {
            // ��ź�߻�
            //StartCoroutine(FireGrenada());

            photonView.RPC(nameof(pun_FireGrenada), RpcTarget.All,closestObject.position);

        }

        //anim.Play("Flare");

        currrTime += Time.deltaTime;
        if (currrTime > 2.5f)
        {
            flag = true;
            currrTime = 0;
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }

        // �� ���.. 
        // �ؿ��� ���� ��� ����.
    }
    protected override void F_meleeattack()
    {
        currrTime += Time.deltaTime;
        f_rotation();

        if (transform.forward != (closestObject.transform.position - transform.position))
        {
            f_rotation();
            //Debug.Log(transform.rotation + " " + newRotation);
        }
        else
        {
            Debug.Log("tlfgo");
        }
        // �������� �ڵ� ���� ��«. �ִϸ��̼� ����
        Debug.Log("��������");

        if (distance < 3 && currrTime > 2.5f)
        {
            currrTime = 0;
            photonView.RPC(nameof(PlayAnimT), RpcTarget.All, "MeleeAttack");

        }
        if (distance > 3)
        {
            TraceNavSetting();
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.escape;
        }
        // escape �����ؾ���.
    }
    #endregion

    protected override void Die()
    {
        transform.GetComponent<SquadLeader>().enabled = false;
        //GetComponent<BoxCollider>().enabled = false;

        base.Die();
        Debug.Log("log");
        StartCoroutine(GetComponent<Die>().delay());
    }

    #region animation event

    public void equip_end()
    {
        //E_state = EnemyState.ranged_attack;
    }

    #endregion

}


