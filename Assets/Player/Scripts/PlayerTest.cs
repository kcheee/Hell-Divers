using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTest : MonoBehaviour
{
    public Transform trBody;
    public float speed = 5;
    public Gun currentGun;
    public Gun mainGun;
    public Gun subGun;
    bool reload = false;


    Animator anim;
    void Start()
    {
        anim = trBody.GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();
        speed = 4;
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed", dir.magnitude);
        anim.SetFloat("RunSpeed", speed);


        //만약에 움직이고 있다면(sqr은 루트 ㄴㄴ)
        if (dir.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {

                speed = 10;
                anim.SetFloat("RunSpeed", speed);
            }

            // 이동 방향과 Body 의 오른쪽 벡터의 사이각을 구하자
            float angle = Vector3.Angle(dir, trBody.right);

            //만약에 각도가 90보다 작으면 오른쪽을 회전
            //원작에 회전이 생각보다 빠르게 된다.
            if (angle < 90)
            {
                trBody.Rotate(new Vector3(0, 5, 0));
            }
            //그렇지않으면 왼쪽으로 회전
            else
            {
                trBody.Rotate(new Vector3(0, -5, 0));
            }
        }

        transform.position += dir * speed * Time.deltaTime;

    }
}
