using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour {
    public float Slow = 0.15f;
    private Animator anim;
    private float X = 0;
    private float Y = 0;
    private KeyCode keycode;
    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        keycode = this.GetComponent<KeyCode>();

    }
	
	// Update is called once per frame
	void Update () {
        X = Mathf.Lerp(X, keycode.moveX, Slow);
        Y = Mathf.Lerp(Y, keycode.moveY, Slow);
        gameObject.transform.position = new Vector2(transform.position.x + X* Slow*0.3f, transform.position.y + Y* Slow*0.3f);
        gameObject.transform.localScale = new Vector3(keycode.A, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        anim.SetFloat("Blend", X >= 0.1F || Y >= 0.1F || X<=-0.1F || Y<=-0.1F ? 1 : 0);
        
	}
}
