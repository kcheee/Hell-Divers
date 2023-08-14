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

    // �ѿ� �¾��� ��� �ִϸ��̼ǰ� blood ȿ�� �߻�.
    public void E_Hit(Vector3 pos)
    {
        anim.SetTrigger("Hit");
        GameObject gm =Instantiate(BloodEft, pos, Quaternion.identity);
        Destroy(gm,2);
        

        // �θ�� ����.
        gm.transform.parent = transform;
        Debug.Log("tlfgod");
    }

}
