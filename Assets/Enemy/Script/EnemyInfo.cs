using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{  
    [Serializable]
    public struct enemyinfo
    {
        public string monsterName;
        public int hp;
        public int Walkspeed;
        public float turnSpeed;

        // 필요한 컴포넌트
        public Animator anim;
        public Rigidbody rigid;
        public BoxCollider boxCol;

        public AudioClip[] sound_Normal;
        public AudioClip sound_Hurt;
        public AudioClip sound_Dead;
    }

    [Serializable]
    public struct enemyattackinfo
    {

        public float attackDistance;  // 공격인식거리
        public float attackRange;  // 공격가능거리
        public float farDistance;  // 공격포기거리

    }

    // enemy 기본정보
    [Header("Enemy 기본 정보")]
    [SerializeField]
    protected enemyinfo ENEMY;

    // enemy 공격정보
    [Header("Enemy 공격 정보")]
    [SerializeField]
    protected enemyattackinfo ENEMYATTACK;

    [Header("patrol test용")]
    public GameObject[] patrolT;
}