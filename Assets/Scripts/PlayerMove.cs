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
        //WASD�� ������ �̵��Ѵ�.
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(H, 0, V);
        dir.Normalize();

        //������ ������ �� ������ �ٶ󺻴�.
        transform.position += dir * speed * Time.deltaTime;

        //���콺 ������ Ŭ���� ������ �� ������ �ٶ󺻴�

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
