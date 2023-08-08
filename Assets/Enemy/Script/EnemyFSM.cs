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
        Chase,   // 추적 
        Attack,
        React,  // 데미지
        Die,    // 죽음
        Patrol, // 순찰 
    }
    public EnemyState E_state;

    [SerializeField] private int distanceattack2;


    // 추적 변수
    public GameObject[] patrol;
    private int patrolIndex;

    // agent
    NavMeshAgent agent;

    // target
    GameObject targetPlayer;

    
}
