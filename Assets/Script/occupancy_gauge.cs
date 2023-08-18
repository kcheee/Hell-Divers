using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider slider;
    public float radius;
    public float AddGauze=0.1f;
    public Text text;
    Animator anim;
    float gaze;
    float distance;

    bool flag=false;

    // 사운드 넣어야 함. 
    public AudioClip[] audioclip;
    AudioSource audioSource;

    private void Start()
    {
        occupationGaze = OccupationGaze.idle;
        anim = GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
    }


    // 점령 게이지
    void FixedUpdate()
    {
        if (flag)
            return;
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
            //audioSource.clip = audioclip[0];
            audioSource.PlayOneShot(audioclip[0]);
        }

    }
    void GazeStart()
    {
        if (distance >= 5)
        {
            occupationGaze = OccupationGaze.idle;
            anim.SetTrigger("Stop");
            audioSource.Stop();
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
            gaze += AddGauze;
            slider.value = gaze;
            //Debug.Log(gaze);
        }
        else
        {
            // finish 하고 스크립트 꺼버림.
            if (!flag)
            StartCoroutine(component_off());

        }
    }

    IEnumerator component_off()
    {
        // 플래그
        flag = true;
        anim.SetTrigger("Finish");
        audioSource.PlayOneShot(audioclip[1]);
        text.text = "확보 완료";
        yield return new WaitForSeconds(7);
        GetComponent<occupancy_gauge>().enabled = false;
    }

}
