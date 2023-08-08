using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEngine.GraphicsBuffer;

public class EnemyFSM : EnemyInfo
{
    // FSM
    public enum EnemyState
    {
        Idle,
        Chase,   // ���� 
        Attack,
        React,  // ������
        Die,    // ����
        Patrol, // ���� 
    }
    public EnemyState E_state;

    [SerializeField] private int distanceattack2;


    // ���� ����
    public GameObject[] patrol;
    private int patrolIndex;

    // agent
    NavMeshAgent agent;

    // target
    GameObject targetPlayer;

    
}
