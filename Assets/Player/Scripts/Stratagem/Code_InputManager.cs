using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_InputManager : MonoBehaviour
{
    public List<KeyType.Key> KeyInputList;
    // Start is called before the first frame update
    void Start()
    {
        KeyInputList = new List<KeyType.Key>();
    }

    //���⼭ �Է��� �޴´�.
    void Update()
    {
        
    }

    //�Է��� �޴´�.
    public void input(System.Action action) {
        
        //Debug.Log(string.Join("," ,KeyInputList));
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            KeyInputList.Add(KeyType.Key.Up);
            action();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyInputList.Add(KeyType.Key.Left);
            action();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyInputList.Add(KeyType.Key.Down);
            action();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            KeyInputList.Add(KeyType.Key.Right);
            action();
        }
    }
    //�ʱ�ȭ �Ѵ�.
    public void init() {
        KeyInputList = new List<KeyType.Key>();
    }
}
