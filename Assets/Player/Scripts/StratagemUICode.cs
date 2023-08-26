using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StratagemUICode : MonoBehaviour
{
    public List<Image> Code_images;
    public static Sprite Up_texture;
    public static Sprite Right_texture;
    public static Sprite Left_texture;
    public static Sprite Down_texture;

    // Start is called before the first frame update

    public Sprite GetTexture(KeyType.Key key) {

        Sprite sprite = null;
        switch (key) {
            case KeyType.Key.Up:
                sprite = Up_texture;
                break;
            case KeyType.Key.Right:
                sprite = Right_texture;
                break;
            case KeyType.Key.Left:
                sprite = Left_texture;
                break;
            case KeyType.Key.Down:
                sprite = Down_texture;
                break;
        }
        return sprite;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetColor()
    {
        foreach (Image img in Code_images) {
            img.color = Color.white;
        }

    }
}
