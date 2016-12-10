using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {
    public Rigidbody body;
    public float movementAmount;
    public float itemSearchRadius;
    public GameObject holdingItem;
    public GameObject marker;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();

        if(!marker)
        {
            Debug.LogError("No marker!");
        }
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

        Collider[] objs = Physics.OverlapSphere(transform.position, itemSearchRadius);
        GameObject nearbyPile = null;

        if(objs.Length > 0) { 
            float smallestDist = float.MaxValue - 1;
            GameObject foundItem = null;


            for (int i = 0; i < objs.Length; i++)
            {
                if(objs[i].CompareTag("Pile"))
                {
                    nearbyPile = objs[i].gameObject;
                    continue;
                }

                if (!objs[i].CompareTag("Item") || holdingItem == objs[i])
                    continue;

                float dist = Vector3.Distance(objs[i].transform.position, transform.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    foundItem = objs[i].gameObject;
                }
            }

            if(foundItem && foundItem != holdingItem)
            {
                marker.transform.position = foundItem.transform.position;
                marker.SetActive(true);
            }
            else
            {
                marker.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (holdingItem && foundItem && holdingItem != foundItem)
                {
                    holdingItem.GetComponent<Rigidbody>().isKinematic = false;
                    holdingItem.transform.parent = null;
                    holdingItem.GetComponent<Rigidbody>().velocity = (transform.forward * -5f) + (transform.up * 7f);
                    holdingItem = null;
                    holdingItem = foundItem;
                    foundItem.GetComponent<Rigidbody>().isKinematic = true;
                    foundItem.transform.parent = transform;
                    foundItem.transform.position = transform.position + (transform.forward * 2);
                }
                else if (holdingItem)
                {
                    holdingItem.GetComponent<Rigidbody>().isKinematic = false;
                    holdingItem.transform.parent = null;
                    holdingItem.GetComponent<Rigidbody>().velocity = (transform.forward * -5f) + (transform.up * 7f);
                    holdingItem = null;
                }
                
                else if(!holdingItem && foundItem)
                {
                    holdingItem = foundItem;
                    foundItem.GetComponent<Rigidbody>().isKinematic = true;
                    foundItem.transform.parent = transform;
                    foundItem.transform.position = transform.position + (transform.forward * 2);
                }
                if(holdingItem && nearbyPile)
                {

                }
            }
        }
    }

    void OnGizmosDraw()
    {
        Gizmos.DrawWireSphere(transform.position, itemSearchRadius);
    }
}
