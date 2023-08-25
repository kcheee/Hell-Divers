using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MissionUI : MonoBehaviour
{
    // 헤더 설정
    [Header ("EndUI")]
    public GameObject endUI;
    public RectTransform[] End_logo;
    public RectTransform mission_T;

    [Space (10f)]
    [Header("MissionUI")]
    //public GameObject MissionUI;
    public RectTransform[] logo;
    //public RectTransform mission_T;

    void missionCompleteText(GameObject Go,RectTransform[] rt1,RectTransform rt2)
    {
        Go.SetActive(true);
        rt1[0].DOAnchorPos(new Vector2(370, 0), 0.4f);
        rt1[1].DOAnchorPos(new Vector2(-370, 0), 0.4f);
        rt2.DOAnchorPos(new Vector2(), 0.4f); // new vector2 (0,0)

        rt1[0].DOAnchorPos(new Vector2(370, 0), 0.4f);
        rt1[1].DOAnchorPos(new Vector2(-370, 0), 0.4f);
        rt2.DOAnchorPos(new Vector2(), 0.4f); // new vector2 (0,0)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            missionCompleteText(endUI, End_logo ,mission_T);
        }
    }
}
