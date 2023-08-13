using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RootMotion.Demos.Turret;

public class Enemy_Hit : MonoBehaviour
{

    public GameObject BloodEft;
    public EnemyInfo EnemyInfo;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // 총에 맞았을 경우 애니메이션과 blood 효과 발생.
    public void E_Hit(Vector3 pos)
    {
        anim.SetTrigger("Hit");
        GameObject gm =Instantiate(BloodEft, pos, Quaternion.identity);
        Destroy(gm,2);
        

        // 부모로 설정.
        gm.transform.parent = transform;
        Debug.Log("tlfgod");
    }

}
