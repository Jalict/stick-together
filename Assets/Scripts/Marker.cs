using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
    public float speed;
    public float minSize;
    public float maxSize;

	// Use this for initialization
	void Start () {
	    if(minSize > maxSize)
        {
            Debug.LogError(name + " has bigger minSize than maxSize");
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * (((Mathf.Sin(Time.time * speed)+1) * (maxSize-minSize))+minSize);
	}
}
