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
        else {
            //anim.SetBool("Walk", false);
        }

        //���콺 ��Ŭ��
        if (Input.GetMouseButton(1))
        {
            speed = 1;
            //���콺 ��ġ��
            Vector3 msPos = Input.mousePosition;

            //��ũ�� ��ġ�� �ٲٰ� �ű⿡ �ش��ϴ� ���̸� �����.
            Ray ray = Camera.main.ScreenPointToRay(msPos);
            RaycastHit hitInfo;
            //���̸� ���.
            if (Physics.Raycast(ray, out hitInfo))
            {

                //������ - �ڽ��� ��ġ�� target���� �Ѵ�. y���� ������� ������ 0���� �Ѵ�.
                Vector3 target = hitInfo.point - transform.position;
                target.y = 0;
                trBody.forward = target;
            }

        }
        transform.position += dir * speed * Time.deltaTime;


        if (Input.GetMouseButtonDown(0)) {
            gun.Fire();
        }

        //1���� ������ ���� ����

        //2���� ������ ���� ����


    }
}
