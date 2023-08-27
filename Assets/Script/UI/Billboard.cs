using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Billboard : MonoBehaviour
{

    public Transform mainCameraTransform; // 메인 카메라의 Transform 컴포넌트

    private void Start()
    {
        //mainCameraTransform = Camera.main.transform; // 메인 카메라의 Transform을 가져옴

    }

    private void LateUpdate()
    {
        //// UI 오브젝트의 월드 좌표를 계산
        //Vector3 worldPosition = transform.position;

        //if (GetComponent<RectTransform>() != null)
        //{
        //    worldPosition = GetComponent<RectTransform>().position;
        //}

        //// 카메라 방향을 향하는 회전 값을 계산
        //Quaternion lookRotation = Quaternion.LookRotation(mainCameraTransform.forward, Vector3.up);

        //// UI 오브젝트의 회전을 업데이트하여 빌보드 효과를 적용
        //transform.rotation = lookRotation;
        //lookPos = new Vector3 (0, lookPos.y-90, 0);

        //Vector3 lookPos = mainCameraTransform.position;
        //transform.DOLookAt(lookPos, 1);

        //transform.LookAt(transform.position + mainCameraTransform.transform.rotation * Vector3.forward,
        //         mainCameraTransform.transform.rotation * Vector3.up);

        Vector3 lookPos = mainCameraTransform.position;
        // x와 z 축의 회전 값을 0으로 고정시킴
        lookPos.x = transform.position.x;
        lookPos.z = transform.position.z;

        // 회전 애니메이션을 적용하여 UI 요소를 카메라 방향으로 회전시킴
        transform.DOLookAt(lookPos, 1);
    }
}
