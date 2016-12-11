using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {
    public Rigidbody body;
    public float movementAmount;
    public float itemSearchRadius;
    public GameObject holdingItem;
    public GameObject marker;
    private Animator anim;
    public Transform hand;
    public GameObject pile;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();

        if(!marker)
        {
            Debug.LogError("No marker!");
        }

        anim = GetComponent<Animator>();
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

            anim.SetBool("isRunning", true);
        } else
        {
            anim.SetBool("isRunning", false);
        }

        Collider[] objs = Physics.OverlapSphere(transform.position, itemSearchRadius);

        if(objs.Length > 0) { 
            float smallestDist = float.MaxValue - 1;
            GameObject nearestItem = null;


            for (int i = 0; i < objs.Length; i++)
            {
                if(objs[i].CompareTag("Pile"))
                {
                    nearestItem = objs[i].gameObject;
                }

                if (!objs[i].CompareTag("Item") || holdingItem == objs[i])
                    continue;

                float dist = Vector3.Distance(objs[i].transform.position, transform.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    nearestItem = objs[i].gameObject;
                }
            }

            if((nearestItem && nearestItem != holdingItem))
            {
                marker.transform.position = nearestItem.transform.position + (Vector3.up*0.025f);
                marker.SetActive(true);

                if(nearestItem.CompareTag("Pile"))
                {
                    marker.GetComponent<Renderer>().material.color = Color.red;
                } else
                {
                    marker.GetComponent<Renderer>().material.color = Color.white;
                }
            }
            else
            {
                marker.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (holdingItem)
                {
                    holdingItem.GetComponent<Rigidbody>().isKinematic = false;
                    holdingItem.GetComponent<Rigidbody>().detectCollisions = true;
                    holdingItem.GetComponent<BoxCollider>().enabled = true;
                    holdingItem.transform.parent = null;
                    holdingItem.GetComponent<Rigidbody>().velocity = (transform.forward * 7f) + (transform.up * 2f);

                    StartCoroutine(CreatePile(holdingItem));

                    holdingItem = null;
                }
                else if (!holdingItem && nearestItem && nearestItem.CompareTag("Pile"))
                {
                    holdingItem = nearestItem.GetComponent<Pile>().RemoveStick();
                    holdingItem.transform.parent = hand;
                    holdingItem.transform.position = hand.position;
                    holdingItem.GetComponent<Rigidbody>().isKinematic = true;
                    holdingItem.GetComponent<Rigidbody>().detectCollisions = false;
                    holdingItem.GetComponent<BoxCollider>().enabled = false;
                }
                else if(!holdingItem && nearestItem && nearestItem.CompareTag("Item"))
                {
                    holdingItem = nearestItem;
                    nearestItem.GetComponent<Rigidbody>().isKinematic = true;
                    nearestItem.GetComponent<Rigidbody>().detectCollisions = false;
                    nearestItem.transform.parent = hand;
                    nearestItem.transform.position = hand.position;
                    nearestItem.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }

    private IEnumerator CreatePile(GameObject item)
    {
        yield return new WaitForSeconds(2f);

        Instantiate(pile, item.transform.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, itemSearchRadius);
    }
}
