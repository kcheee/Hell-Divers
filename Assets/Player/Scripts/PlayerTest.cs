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


        //���࿡ �����̰� �ִٸ�(sqr�� ��Ʈ ����)
        if (dir.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {

                speed = 10;
                anim.SetFloat("RunSpeed", speed);
            }

            // �̵� ����� Body �� ������ ������ ���̰��� ������
            float angle = Vector3.Angle(dir, trBody.right);

            //���࿡ ������ 90���� ������ �������� ȸ��
            //���ۿ� ȸ���� �������� ������ �ȴ�.
            if (angle < 90)
            {
                trBody.Rotate(new Vector3(0, 5, 0));
            }
            //�׷��������� �������� ȸ��
            else
            {
                trBody.Rotate(new Vector3(0, -5, 0));
            }
        }

        transform.position += dir * speed * Time.deltaTime;

    }
}
