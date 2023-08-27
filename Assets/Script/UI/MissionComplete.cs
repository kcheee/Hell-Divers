using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionComplete : MonoBehaviour
{
    public GameObject endUI;
    public RectTransform[] logo;
    public RectTransform mission_T;

    private void Start()
    {
        StartCoroutine(delay());
    }
    
    IEnumerator delay()
    {
        yield return new WaitForSeconds(5f);
    }

    void missionCompleteText()
    {
        endUI.SetActive(true);
        logo[0].DOAnchorPos(new Vector2(370, 0), 0.4f);
        logo[1].DOAnchorPos(new Vector2(-370, 0), 0.4f);
        mission_T.DOAnchorPos(new Vector2(), 0.4f); // new vector2 (0,0)

        logo[0].DOAnchorPos(new Vector2(370, 0),0.4f);
        logo[1].DOAnchorPos(new Vector2(-370, 0), 0.4f);
        mission_T.DOAnchorPos(new Vector2(), 0.4f); // new vector2 (0,0)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            missionCompleteText();
        }
    }
}
