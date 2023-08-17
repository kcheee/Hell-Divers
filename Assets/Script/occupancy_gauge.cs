using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class occupancy_gauge : MonoBehaviour
{

    public enum OccupationGaze
    {
        idle,
        start,
        finish
    }

    OccupationGaze occupationGaze;

    public GameObject player;
    public float radius;
    Animator anim;
    float gaze;
    float distance;

    // ���� �־�� ��. 
    public AudioClip[] audioclip;

    private void Start()
    {
        occupationGaze = OccupationGaze.idle;
        anim = GetComponent<Animator>();
    }


    // ���� ������
    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        switch (occupationGaze)
        {
            case OccupationGaze.idle:
                GazeIdle();
                break;
            case OccupationGaze.start:
                GazeStart();
                break;
            case OccupationGaze.finish:
                GazeFinish();
                break;
        }
    }

    private void GazeIdle()
    {
        if (distance < 5)
        {
            occupationGaze = OccupationGaze.start;
            anim.SetTrigger("Start");
        }

    }
    void GazeStart()
    {
        if (distance >= 5)
        {
            occupationGaze = OccupationGaze.idle;
            anim.SetTrigger("Stop");
        }
        Occupation();
    }
    void GazeFinish()
    {

    }

    void Occupation()
    {
        if (100 >= gaze)
        {
            gaze += 0.1f;
            //Debug.Log(gaze);
        }
        else
        {
            // finish �ϰ� ��ũ��Ʈ ������.
            anim.SetTrigger("Finish");
            GetComponent<occupancy_gauge>().enabled = false;
        }
    }

}
