using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public Text ManganizeText;
    public Text BulletText;
    public Transform StratagemTime;
    public Image StratagemImage;
    public Animator anim;

    //Player Info UI
    public Transform PlayerInfo;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static PlayerUI Get() {
        if (instance == null) {
            GameObject UIObj = new GameObject("PlayerUI");
            instance = UIObj.AddComponent<PlayerUI>();
        }
        return instance;
    }
}
