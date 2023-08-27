using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyButton : Button
{
    Image img;
    AudioSource audioSource;
    protected override void Start()
    {
        img = transform.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        print("OnPointerEnter");
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        print("OnPointerExit");

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(LobbySceneChange.instance.Fade(false));
            //PhotonNetwork.LoadLevel("MainScene");
        }
    }

}
