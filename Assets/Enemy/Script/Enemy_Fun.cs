using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;

// Enemy ����� ���.
public class Enemy_Fun : EnemyInfo, IPunObservable,I_Entity
{
    // Enemy ����
    public enum EnemyState
    {
        idle,
        patrol,
        chase,
        wait,
        escape,
        melee_attack,
        EnemyInSight,   // SquadLeader
        ranged_attack,
        hit,
        die
    }
    public EnemyState E_state;

    // agent
    protected NavMeshAgent agent;

    // animator
    protected Animator anim;

    // patrol
    int targetIndex = 0;

    // �ð� �����̸� �ֱ� ���� ����
    protected float currTime = 0;
    Vector3 P_targt;

    // �÷��̾� ��ġ
    public List<Transform> objects; // ������Ʈ���� Transform�� ����Ʈ�� ����

    protected Transform closestObject;

    protected virtual void F_patrol()
    {
        // �ȴ� �ִϸ��̼�
        // patrol �ϴ� target

        // patrolT�� null�� ��Ȳ�� �д����� ȣ������ ��� �ۿ� ���⿡ �̷��� «.
        if (patrolT[targetIndex] == null)
        {
            E_state = EnemyState.chase;
        }
        else
            P_targt = patrolT[targetIndex].transform.position;

        agent.SetDestination(P_targt);

        //Debug.Log(P_targt);
        // ���� �������� �����ߴٸ�(�������� �Ÿ��� 0.1M���϶��)
        P_targt.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, P_targt);

        if (dist <= 0.3f)
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
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }

    }

    protected virtual void F_chase()
    {

    }

    // ����1�� ���� ���ð�.
    protected virtual void F_wait()
    {
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

    // ȸ���ϴ� �Լ�
    protected virtual void f_rotation()
    {
        Vector3 currentDirection = transform.forward;
        Vector3 targetDirection = closestObject.transform.position - transform.position;

        // ���� ���� ����� ��ǥ ������ �ٸ��ٸ� ȸ�� ����
        if (Vector3.Dot(currentDirection.normalized, targetDirection.normalized) < 0.99f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ENEMY.turnSpeed);

            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.x = 0; // x���� 0���� ����
            eulerAngles.z = 0; // z���� 0���� ����

            Quaternion adjustedRotation = Quaternion.Euler(eulerAngles);
            transform.rotation = adjustedRotation;

            //transform.rotation = newRotation;
        }
    }

    public void E_Hit(Vector3 pos)
    {
        anim.SetTrigger("Hit");
        GameObject gm = Instantiate(BloodEft, pos, Quaternion.identity);
        Destroy(gm, 2);

        ENEMY.hp -= 10;

        if (ENEMY.hp <= 0)
        {
            //die
            Debug.Log("die");

            GameObject diegm = Instantiate(DieEft, pos, Quaternion.identity);

            anim.Play("Die");
            Destroy(gameObject);

        }
        // �θ�� ����.
        gm.transform.parent = transform;
    }

    // navmeshagent ����
    #region
    protected void TraceNavSetting()
    {
        this.agent.isStopped = false;
        this.agent.updatePosition = true;
        this.agent.updateRotation = true;
    }

    protected void StopNavSetting()
    {
        this.agent.isStopped = true;
        this.agent.updatePosition = false;
        this.agent.updateRotation = false;
        this.agent.velocity = Vector3.zero;
    }
    #endregion

    // ���� ����� �÷��̾� ã��
    protected Transform FindClosestObject()
    {
        Transform closest = PlayerManager.instace.PlayerList[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (PlayerTest1 obj in PlayerManager.instace.PlayerList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closest = obj.transform;
                closestDistance = distance;
            }
        }
        return closest;
    }


    #region photon ����

    [PunRPC]
    public void PlayAnimT(string name)
    {
        anim.SetTrigger(name);
        Debug.LogWarning(name+"������ ���� ����");
    }

    [PunRPC]
    public void PlayAnimF(string name, float value)
    {
        anim.SetFloat(name, value);
        Debug.LogWarning(name + "������ ���� ����");

    }

    [PunRPC]
    public void PlayAnimB(string name, bool value)
    {

        anim.SetBool(name, value);
        Debug.LogWarning(name + "������ ���� ����");

    }
    [PunRPC]
    public void PlayAnimP(string name)
    {
        anim.Play(name);
        Debug.LogWarning(name + "������ ���� ����");

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ////�ʰ� ����
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(h);
        //    stream.SendNext(v);
        //    stream.SendNext(speed);
        //}
        ////������
        //else
        //{

        //    h = (float)stream.ReceiveNext();
        //    v = (float)stream.ReceiveNext();
        //    speed = (float)stream.ReceiveNext();

        //}
    }
    #endregion
    [PunRPC]
    public void damaged(Vector3 pos,int damage = 0)
    {
        Debug.Log("Damaged");
        anim.SetTrigger("Hit");
        ENEMY.hp -= damage;
        GameObject gm = Instantiate(BloodEft, pos, Quaternion.identity);
        Destroy(gm, 2);
        if (ENEMY.hp <= 0)
        {
            //die
            Debug.Log("die");
            GameObject diegm = Instantiate(DieEft, pos, Quaternion.identity);
            //GameObject diegm = Instantiate(DieEft, pos, Quaternion.identity);

            anim.Play("Die");
            Destroy(gameObject);

        }
    }

    public void die(Action action)
    {
        Debug.Log("DAmage!!");
    }


}
