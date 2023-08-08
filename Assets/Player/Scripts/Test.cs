using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    PlayerMove move;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.W))
        {
            move.W = 0;
        }
        if (!Input.GetKey(KeyCode.A))
        {
            move.A = 0;
        }
        if (!Input.GetKey(KeyCode.S))
        {
            move.S = 0;
        }
        if (!Input.GetKey(KeyCode.D))
        {
            move.D = 0;
        }
    }

    private void LateUpdate()
    {
        if (!Input.GetKey(KeyCode.W))
        {
            move.W = 0;
        }
        if (!Input.GetKey(KeyCode.A))
        {
            move.A = 0;
        }
        if (!Input.GetKey(KeyCode.S))
        {
            move.S = 0;
        }
        if (!Input.GetKey(KeyCode.D))
        {
            move.D = 0;
        }
    }
}
