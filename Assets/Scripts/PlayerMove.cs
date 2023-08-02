using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //WASD를 누르면 이동한다.
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(H, 0, V);
        dir.Normalize();

        //누르고 있을때 그 방향을 바라본다.
        transform.position += dir * speed * Time.deltaTime;

        //마우스 오른쪽 클릭을 누르면 그 방향을 바라본다

        if (Input.GetMouseButton(0)) {
            Vector3 msPos = Input.mousePosition;
            Ray ray  = Camera.main.ScreenPointToRay(msPos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)) {
                Vector3 target = hitInfo.point - transform.position;
                transform.forward = target;
            }
           

        }

    }
}
