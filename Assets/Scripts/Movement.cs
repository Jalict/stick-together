using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public Vector3 rotation;
    public Rigidbody body;
    public Transform camera;
    public float movementAmount;
    public ForceMode forceMode;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();

        if (!camera)
        {
            Debug.LogError("Player has no direction (Missing Camera Transform)");
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 movementForce = new Vector3();
        movementForce.x = Input.GetAxis("Horizontal") * movementAmount;
        movementForce.z = Input.GetAxis("Vertical") * movementAmount;
        body.AddForce(movementForce, forceMode);

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
