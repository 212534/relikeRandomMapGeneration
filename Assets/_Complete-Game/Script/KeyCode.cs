using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCode : MonoBehaviour {
    private static KeyCode instance;

    public string Wkey="w";
    public string Skey="s";
    public string Akey = "a";
    public string Dkey = "d";
    public float moveX;
    public float moveY;
    public float A;
    public float D;
    public Vector3 local;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        local = transform.localScale;
        A = local.x;
    }
	
	// Update is called once per frame
	void Update () {
        moveX = (Input.GetKey(Dkey) ? 1.0f : 0) - (Input.GetKey(Akey) ? 1.0f : 0); //按下A键时为-1 按下D键时为+1
        moveY = (Input.GetKey(Wkey) ? 1.0f : 0) - (Input.GetKey(Skey) ? 1.0f : 0);//按下W时为1 按下S时为负1

        if (Input.GetKeyDown(Akey) == true)
        {
            A = -local.x;
        }
        if (Input.GetKey(Akey) == true)
        {
            A = -local.x;
        }
        if (Input.GetKeyDown(Dkey) == true)
        {
            A = local.x ;
        }
        if (Input.GetKey(Dkey) == true)
        {
            A = local.x;
        }
    }
}
