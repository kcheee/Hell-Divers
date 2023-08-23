using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorImg1;
    public Texture2D cursorImg2;
    int size;
    public int wsize;
    public int hsize;
    // Start is called before the first frame update
    void Start()
    {
        wsize = cursorImg2.width / 2;
        hsize = cursorImg2.height / 2;
        Debug.Log(hsize +  " ssss" + wsize);
        Cursor.visible = false;
        Cursor.SetCursor(cursorImg1, Vector3.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) {
            Cursor.SetCursor(cursorImg2, Vector3.zero + (Vector3.up * hsize) + (Vector3.right * wsize) , CursorMode.ForceSoftware);
        }
        if (Input.GetMouseButtonUp(1)) {
            Cursor.SetCursor(cursorImg1, Vector3.zero + (Vector3.up * hsize) + (Vector3.right * wsize), CursorMode.ForceSoftware);
        }
    }
}
