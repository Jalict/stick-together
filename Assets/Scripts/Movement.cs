using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {
    public Rigidbody body;
    public float movementAmount;
    public float itemSearchRadius;
    public bool searching;
    public GameObject holdingItem;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal != 0 || vertical != 0) { 
            // Player looking direction
            Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Lerp(body.rotation, targetRotation, 15f * Time.deltaTime);

            body.MoveRotation(newRotation);

            Vector3 movement = Vector3.zero;
            movement += transform.forward * vertical;
            movement += transform.right * horizontal * 0.5f;

            body.velocity = movement * movementAmount;
        }


        // === PICK UP MARKER
        // Get Current postion
        // Find anything near with tag "Pickup"
        // Find nearest one to player
        Collider[] objs = Physics.OverlapSphere(transform.position, itemSearchRadius);

        if(objs.Length > 0) { 
            float smallestDist = float.MaxValue - 1;
            GameObject foundItem = null;


            for (int i = 0; i < objs.Length; i++)
            {
                if (!objs[i].CompareTag("Item"))
                    continue;

                float dist = Vector3.Distance(objs[i].transform.position, transform.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    foundItem = objs[i].gameObject;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if(holdingItem && holdingItem == foundItem)
                {
                    holdingItem.transform.parent = null;
                    foundItem.GetComponent<Rigidbody>().isKinematic = false;
                    holdingItem = null;
                } else
                {
                    holdingItem = foundItem;

                    foundItem.transform.parent = transform;
                    foundItem.GetComponent<Rigidbody>().isKinematic = true;
                    foundItem.transform.position = transform.position + (transform.forward * 2);
                }
            }
        }
    }
}
