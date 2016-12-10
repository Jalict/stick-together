using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 rotation;
    public Rigidbody body;
    public float movementAmount;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();

        if (!GetComponent<Camera>())
        {
            Debug.LogError("Player has no direction (Missing Camera Transform)");
        }
    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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

        // === PICK UP MARKER
        // Get Current postion
        // Find anything near with tag "Pickup"
        // Find nearest one to player
        /*GameObject markedObject;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            markedObject.transform.parent = transform;
        }*/
    }
}
