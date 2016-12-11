using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Billboard : MonoBehaviour {
    public Text text;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
