using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform mainCameraTransform; // 메인 카메라의 Transform 컴포넌트

    private void Start()
    {
        //mainCameraTransform = Camera.main.transform; // 메인 카메라의 Transform을 가져옴

    }

    private void LateUpdate()
    {
        // UI 오브젝트의 월드 좌표를 계산
        Vector3 worldPosition = transform.position;

        if (GetComponent<RectTransform>() != null)
        {
            worldPosition = GetComponent<RectTransform>().position;
        }

        // 카메라 방향을 향하는 회전 값을 계산
        Quaternion lookRotation = Quaternion.LookRotation(mainCameraTransform.forward, Vector3.up);

        // UI 오브젝트의 회전을 업데이트하여 빌보드 효과를 적용
        transform.rotation = lookRotation;
    }
}
