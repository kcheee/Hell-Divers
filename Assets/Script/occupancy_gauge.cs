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
    private Transform closestObject;
    float distance;

    bool flag=false;
    bool delayflag = false;
    // ���� �־�� ��. 
    public AudioClip[] audioclip;
    AudioSource audioSource;

    IEnumerator delay()
    {
        yield return new WaitForSeconds(5);
        delayflag = true;
    }
    private void Start()
    {
        occupationGaze = OccupationGaze.idle;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(delay());
    }
    protected Transform FindClosestObject()
    {

        if (PlayerManager.instace.PlayerList[0] == null)
        {
            Debug.Log(PlayerManager.instace.PlayerList);
            return null;
        }
        Transform closest = PlayerManager.instace.PlayerList[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (PlayerTest1 obj in PlayerManager.instace.PlayerList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closest = obj.transform;
                closestDistance = distance;
            }
        }
        return closest;
    }

    // ���� ������
    void FixedUpdate()
    {
        if (!delayflag) return;

        if (flag)
            return;

        closestObject = FindClosestObject();
        //Debug.Log(closestObject.position);

        distance = Vector3.Distance(this.transform.position, closestObject.transform.position);

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
            // finish �ϰ� ��ũ��Ʈ ������.
            if (!flag)
            StartCoroutine(component_off());

        }
    }

    IEnumerator component_off()
    {
        // �÷���
        flag = true;
        anim.SetTrigger("Finish");
        audioSource.PlayOneShot(audioclip[1]);
        text.text = "Ȯ�� �Ϸ�";
        yield return new WaitForSeconds(7);
        GetComponent<occupancy_gauge>().enabled = false;
    }

}
