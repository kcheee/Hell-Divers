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

        //방향을 만들기 전에 체크를 하고 
        //late에서 만들면 되나..

        //WASD를 누르면 이동한다.
        //Debug.Log(H);


        //방향을 만들고
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

        //뗏는지 체크를 한다.
        //근데, 이 프레임 안에서 Up이 체크가 안될수도 있다.
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


        //여기까지 이게 체크가 안된다고?
        Debug.Log("W : " + W + "A : " + A + "S : " + S + "D : " + D);



        //무언가 눌렀고, HV중 0이 아니면  (문제가 발생해서 anyKeyDown을 섞는다.)
        //if (/*Input.anyKeyDown && (H != 0 || V != 0)*/dir.magnitude!=0)
        //{
        //    //누르고 있을때 그 방향을 바라본다.
        //    transform.forward = dir;
        //}

        //방향과 속력으로 이동한다.

        dir = new Vector3(H, 0, V);
        //Debug.Log(dir + "Vector !");

        dir.Normalize();
        if (W != 0 || A != 0 || W != 0 || D != 0)
        {
            Debug.Log(dir + "방향입니다.");
            transform.forward = dir;
        }

        transform.position += dir * speed * Time.deltaTime;

    }



}
