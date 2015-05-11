using UnityEngine;
using System.Collections;

public class HideLogo : MonoBehaviour {
    private bool isHidden = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space) && !isHidden)
        {
            this.renderer.enabled = false;
            isHidden = true;
        }
	}
}
