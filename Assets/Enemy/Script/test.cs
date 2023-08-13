using DG.Tweening;
using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class test : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public float rotationAngle = 40f; // 회전할 각도 (40도)
    public float rotationDuration = 2f; // 회전할 시간 (2초)
    public  AudioSource audioSource;

    IEnumerator delay()
    {
        FireBaseScript.instance.FlameStart();
        yield return null;

    }

    private void OnEnable()
    {

        // 자신의 기준 forward 방향 벡터
        Vector3 forwardDirection = transform.forward;

        // 오른쪽으로 회전
        RotateRight(forwardDirection);
        StartCoroutine(delay());
    }
    private void OnDisable()
    {
        // 각도 0으로 만듦.
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    // 자신을 기준으로 오른쪽으로 회전
    private void RotateRight(Vector3 forwardDirection)
    {
        // 1초동안 오른쪽으로 갔다가 왼쪽으로 감
        Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);

        audioSource.Play();
        transform.DORotateQuaternion(targetRotation, 1f).OnComplete(() =>
        {
            audioSource.Play();
            targetRotation = Quaternion.Euler(0, -rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);
            transform.DORotateQuaternion(targetRotation, 1f).OnComplete(() =>
            {
                FireBaseScript.instance.FlameStop();
                transform.rotation = Quaternion.Euler(0, 0, 0);

            }); ;
        });
    }

}
