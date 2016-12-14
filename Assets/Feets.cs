using UnityEngine;
using System.Collections;

public class Feets : MonoBehaviour {
    public Transform player;
    public AudioClip clip;
    public GameObject footprint;

	// Use this for initialization
	void Start () {

	}

    void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);

        Instantiate(footprint, transform.position + (Vector3.up * 0.01f), footprint.transform.rotation * Quaternion.Euler(1f,1f,-player.transform.eulerAngles.y));
    }
}
