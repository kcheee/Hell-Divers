using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;

// Enemy 공통된 기능.
public class Enemy_Fun : EnemyInfo, IPunObservable,I_Entity
{
    // Enemy 상태
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

    // 시간 딜레이를 주기 위한 변수
    protected float currTime = 0;
    Vector3 P_targt;

    // 플레이어 위치
    public List<Transform> objects; // 오브젝트들의 Transform을 리스트로 저장

    protected Transform closestObject;

    protected virtual void F_patrol()
    {
        // 걷는 애니메이션
        // patrol 하는 target

        // patrolT가 null인 상황은 분대장이 호출했을 경우 밖에 없기에 이렇게 짬.
        if (patrolT[targetIndex] == null)
        {
            E_state = EnemyState.chase;
        }
        else
            P_targt = patrolT[targetIndex].transform.position;

        agent.SetDestination(P_targt);

        //Debug.Log(P_targt);
        // 만약 목적지에 도착했다면(두지점의 거리가 0.1M이하라면)
        P_targt.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, P_targt);

        if (dist <= 0.3f)
        {
            // 인덱스를 1증가시키고싶다.
            targetIndex++;
            // 만약 인덱스가 points배열의 크기이상이되면 0으로 초기화 하고싶다.
            if (targetIndex >= 2)
            {
                targetIndex = 0;
            }
        }

        // attackRange만큼 가까워지면 추적.
        if (distance < ENEMYATTACK.attackRange)
        {
            photonView.RPC(nameof(PlayAnimB), RpcTarget.All, "Walk", true);
            E_state = EnemyState.chase;
        }

    }

    protected virtual void F_chase()
    {

    }

    // 공격1을 위한 대기시간.
    protected virtual void F_wait()
    {
    }

    protected virtual void F_rangedattack()
    {
        // 애니메이션 진행 후 chase로 바꿈.

        // chase 
    }
    protected virtual void F_meleeattack()
    {
        // 애니메이션 실행 후 

        // chase로 바꿈.
    }

    // 회전하는 함수
    protected virtual void f_rotation()
    {
        Vector3 currentDirection = transform.forward;
        Vector3 targetDirection = closestObject.transform.position - transform.position;

        // 만약 현재 방향과 목표 방향이 다르다면 회전 실행
        if (Vector3.Dot(currentDirection.normalized, targetDirection.normalized) < 0.99f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ENEMY.turnSpeed);

            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.x = 0; // x값을 0으로 설정
            eulerAngles.z = 0; // z값을 0으로 설정

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
        // 부모로 설정.
        gm.transform.parent = transform;
    }

    // navmeshagent 설정
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

    // 가장 가까운 플레이어 찾기
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


    #region photon 설정

    [PunRPC]
    public void PlayAnimT(string name)
    {
        anim.SetTrigger(name);
        Debug.LogWarning(name+"여러번 실행 방지");
    }

    [PunRPC]
    public void PlayAnimF(string name, float value)
    {
        anim.SetFloat(name, value);
        Debug.LogWarning(name + "여러번 실행 방지");

    }

    [PunRPC]
    public void PlayAnimB(string name, bool value)
    {

        anim.SetBool(name, value);
        Debug.LogWarning(name + "여러번 실행 방지");

    }
    [PunRPC]
    public void PlayAnim_T(string name)
    {
        anim.Play(name);
        Debug.LogWarning(name + "여러번 실행 방지");

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ////너가 나냐
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(h);
        //    stream.SendNext(v);
        //    stream.SendNext(speed);
        //}
        ////누구냐
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
