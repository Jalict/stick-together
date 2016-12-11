using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pile : MonoBehaviour {
    public List<GameObject> sticks;
    public Billboard countSign;

	// Use this for initialization
	void Start () {
        sticks = new List<GameObject>();

        countSign.gameObject.SetActive(false);
	}

    void Update()
    {
        if (!countSign.gameObject.activeSelf && sticks.Count > 1)
        {
            countSign.gameObject.SetActive(true);
        } else if(countSign.gameObject.activeSelf) {
            countSign.text.text = sticks.Count + " STICKS";
        }

        for(int i = 0; i < sticks.Count;i++)
        {
            Rigidbody body = sticks[i].GetComponent<Rigidbody>();
            Collider collider = sticks[i].GetComponent<Collider>();
            if(body.velocity.magnitude < 0.5)
            {
                body.isKinematic = true;
            }
        }
    }

    // Make script that determines if this is becoming a pile a not

    public void AddStick(GameObject stick)
    {
        sticks.Add(stick);
    }

    void OnDrawGizmos()
    {

    }
}
