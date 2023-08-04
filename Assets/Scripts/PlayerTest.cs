using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Transform trBody;
    public float speed = 5;
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
        speed = 5;



        //���࿡ �����̰� �ִٸ�(sqr�� ��Ʈ ����)
        if (dir.sqrMagnitude > 0)
        {
            
            
            // �̵� ����� Body �� ������ ������ ���̰��� ������
            float angle = Vector3.Angle(dir, trBody.right);

            //���࿡ ������ 90���� ������ �������� ȸ��
            if (angle < 90)
            {
                trBody.Rotate(new Vector3(0, 3, 0));
            }
            //�׷��������� �������� ȸ��
            else
            {
                trBody.Rotate(new Vector3(0, -3, 0));
            }
        }
        else {
            //anim.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 10;
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
    }
}
