using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GranadeLauncher : MonoBehaviour
{
    static public GranadeLauncher instance;
    private void Awake()
    {
        instance = this; 
    }

    private Transform bullet;   // ����ü
    private float tx;
    private float ty;
    private float tz;
    private float v;
    public float g = 9.8f;
    private float elapsed_time;
    public float max_height;
    private float t;

    private Vector3 start_pos;
    private Vector3 end_pos;
    private float dat;
    Rigidbody rb;

    bool onground;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject Player = GameObject.Find("Player");
        Shoot(gameObject, transform.position, Player.transform.position, g, max_height);
    }

    private void FixedUpdate()
    {
        // �ٴڿ� �ε������� false

    }
    // ������ � ����
    public void Shoot(GameObject bullet, Vector3 startPos, Vector3 endPos, float g, float max_height)
    {
        start_pos = startPos;

        end_pos = endPos;

        this.g = g;

        // ���� ��ġ�� y
        this.max_height = max_height;

        this.bullet = bullet.transform;

        this.bullet.position = start_pos;

        // ����
        var dh = endPos.y - startPos.y;

        // ������ġ�� �������� y ��
        var mh = max_height - startPos.y;

        ty = Mathf.Sqrt(2 * this.g * mh);

        float a = this.g;

        float b = -2 * ty;

        float c = 2 * dh;

        // ������ ����.
        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        tx = -(startPos.x - endPos.x) / dat;

        tz = -(startPos.z - endPos.z) / dat;

        this.elapsed_time = 0;

        StartCoroutine(this.ShootImpl());

    }

    public IEnumerator ShootImpl()
    {
        while (true)

        {
            this.elapsed_time += Time.deltaTime;

            var tx = start_pos.x + this.tx * elapsed_time;

            var ty = start_pos.y + this.ty * elapsed_time - 0.5f * g * elapsed_time * elapsed_time;

            var tz = start_pos.z + this.tz * elapsed_time;

            var tpos = new Vector3(tx, ty, tz);

            bullet.transform.LookAt(tpos);

            bullet.transform.position = tpos;

            // ���߿� 
            //transform.Rotate(Vector3.up * 10 + Vector3.right * 10 * 0.02f); // ȸ���� �߰�
            if (this.elapsed_time >= this.dat)

                break;

            yield return null;
            
        }
        //Debug.Log("tkfgo");
        rb.useGravity = true;
        //rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Plane")
        {
            onground = true;
            //rb.useGravity = true;
        }
    }

}
