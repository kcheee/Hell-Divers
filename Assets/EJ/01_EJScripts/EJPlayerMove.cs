using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerMove : MonoBehaviour
{
    //Move
    float speed = 5;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }
}
