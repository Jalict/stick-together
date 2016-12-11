using UnityEngine;
using System.Collections;

public class Feets : MonoBehaviour {
    public AudioClip clip;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
