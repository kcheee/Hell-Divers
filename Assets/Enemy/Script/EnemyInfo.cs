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

        // �ʿ��� ������Ʈ
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
        public float melee_attack_possible;  // �ٰŸ� �����νİŸ�
        public float ranged_attack_possible; // ���Ÿ� �����νİŸ�
        public float attackRange;  // ���ݰ��ɰŸ�
        public float farDistance;  // ��������Ÿ�
    }

    // enemy �⺻����
    [Header("Enemy �⺻ ����")]
    [SerializeField]
    protected enemyinfo ENEMY;

    // enemy ��������
    [Header("Enemy ���� ����")]
    [SerializeField]
    protected enemyattackinfo ENEMYATTACK;

    [Header("patrol test��")]
    public GameObject[] patrolT;

    // �÷��̾���� �Ÿ�.
    public float distance;
}