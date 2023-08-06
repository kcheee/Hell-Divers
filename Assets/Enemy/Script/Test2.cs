using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct stTest
{
    public string monsterName;
    // 필요한 컴포넌트
    public Animator anim;
    public  Rigidbody rigid;
    public  BoxCollider boxCol;

    public  AudioClip[] sound_Normal;
    public  AudioClip sound_Hurt;
    public  AudioClip sound_Dead;
}

[Serializable]
public struct TTT
{
    public string name;
}
public class Test2 : MonoBehaviour
{
    [SerializeField]
    protected stTest Test;

}
