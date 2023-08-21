using DG.Tweening;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

public class GranadeLauncher : MonoBehaviourPun
{
    static public GranadeLauncher instance;
    private void Awake()
    {
        instance = this; 
    }

    private Transform bullet;   // 포물체
    private float tx;
    private float ty;
    private float tz;
    private float v;
    public float g = 9.8f;
    private float elapsed_time;
    public float max_height;
    private float t;

    // 폭탄 포지션
    static public Transform GrenadaPos;

    private Vector3 start_pos;
    private Vector3 end_pos;
    private float dat;
    Rigidbody rb;

    bool onground;


    public void value(Vector3 pos)
    {
        //photonView.RPC(nameof(Shoot), RpcTarget.All, gameObject, transform.position, pos, g, max_height);
        //photonView.RPC(nameof(deleShoot), RpcTarget.All, pos);
        Shoot(gameObject, transform.position, pos, g, max_height);
    }

    void deleShoot(Vector3 pos)
    {
        Debug.Log("tlfgod");
        Shoot(gameObject, transform.position, pos, g, max_height);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Vector3 destination
        //Shoot(gameObject, transform.position, new Vector3(0,0,0), g, max_height);
        //rot.transform.DORotate(transform.right * 30 + transform.forward * 50, 1.5f);
    }

    private void FixedUpdate()
    {
        // 바닥에 부딪혔을때 false

    }

    // 포물선 운동 공식
    public void Shoot(GameObject bullet, Vector3 startPos, Vector3 endPos, float g, float max_height)
    {
        start_pos = startPos;

        end_pos = endPos;

        this.g = g;

        // 현재 위치의 y
        this.max_height = max_height;

        this.bullet = bullet.transform;

        this.bullet.position = start_pos;

        // 방향
        var dh = endPos.y - startPos.y;

        // 현재위치와 목적지의 y 값
        var mh = max_height - startPos.y;

        ty = Mathf.Sqrt(2 * this.g * mh);

        float a = this.g;

        float b = -2 * ty;

        float c = 2 * dh;

        // 포물선 공식.
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

            // 나중에 
            //transform.Rotate(Vector3.up * 10 + Vector3.right * 10 * 0.02f); // 회전력 추가
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
            transform.GetComponent<GranadeLauncher>().enabled = false;
            //rb.useGravity = true;
        }
    }

}
