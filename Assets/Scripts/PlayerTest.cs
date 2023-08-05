using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Transform trBody;
    public float speed = 5;
    public Gun gun;
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
        else {
            //anim.SetBool("Walk", false);
        }

        //마우스 우클릭
        if (Input.GetMouseButton(1))
        {
            speed = 1;
            //마우스 위치를
            Vector3 msPos = Input.mousePosition;

            //스크린 위치로 바꾸고 거기에 해당하는 레이를 만든다.
            Ray ray = Camera.main.ScreenPointToRay(msPos);
            RaycastHit hitInfo;
            //레이를 쏜다.
            if (Physics.Raycast(ray, out hitInfo))
            {

                //맞은곳 - 자신의 위치를 target으로 한다. y값은 사용하지 않으니 0으로 한다.
                Vector3 target = hitInfo.point - transform.position;
                target.y = 0;
                trBody.forward = target;
            }

        }
        transform.position += dir * speed * Time.deltaTime;


        if (Input.GetMouseButtonDown(0)) {
            gun.Fire();
        }

        //1번을 누르면 메인 무기

        //2번을 누르면 보조 무기


    }
}
