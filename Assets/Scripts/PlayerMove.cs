using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    float H;
    float V;
    public float W, A, S, D;
    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 dir;
    private void Update()
    {

        //������ ����� ���� üũ�� �ϰ� 
        //late���� ����� �ǳ�..

        //WASD�� ������ �̵��Ѵ�.
        //Debug.Log(H);


        //������ �����
        if (Input.GetKeyDown(KeyCode.W))
        {
            W = 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            A = -1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            S = -1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            D = 1;
        }

        //�´��� üũ�� �Ѵ�.
        //�ٵ�, �� ������ �ȿ��� Up�� üũ�� �ȵɼ��� �ִ�.
        if (Input.GetKeyUp(KeyCode.W))
        {
            W = 0;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            A = 0;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            S = 0;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            D = 0;
        }

        H = A + D;
        V = W + S;

        


    }



    // Update is called once per frame
    void FixedUpdate()
    {


    }

    //Ctrl + K + F 
    private void LateUpdate()
    {


        //������� �̰� üũ�� �ȵȴٰ�?
        Debug.Log("W : " + W + "A : " + A + "S : " + S + "D : " + D);



        //���� ������, HV�� 0�� �ƴϸ�  (������ �߻��ؼ� anyKeyDown�� ���´�.)
        //if (/*Input.anyKeyDown && (H != 0 || V != 0)*/dir.magnitude!=0)
        //{
        //    //������ ������ �� ������ �ٶ󺻴�.
        //    transform.forward = dir;
        //}

        //����� �ӷ����� �̵��Ѵ�.

        dir = new Vector3(H, 0, V);
        //Debug.Log(dir + "Vector !");

        dir.Normalize();
        if (W != 0 || A != 0 || W != 0 || D != 0)
        {
            Debug.Log(dir + "�����Դϴ�.");
            transform.forward = dir;
        }

        transform.position += dir * speed * Time.deltaTime;

    }



}
