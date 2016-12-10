using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform toFollow;
    public float maxDistance;
    public float cameraHeight;

	// Use this for initialization
	void Start () {
	    if(!toFollow)
        {
            Debug.LogError("Camera have nothing to follow");
        }

        Vector3 fixedHeight = transform.position;
        fixedHeight.y = cameraHeight;
        transform.position = fixedHeight;
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion targetRotation = Quaternion.LookRotation(toFollow.position - transform.position);

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4f * Time.deltaTime);

        // Check if camera is too far away, else start moving it towards
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 followPosition = new Vector2(toFollow.position.x, toFollow.position.z);

        if(Vector2.Distance(myPosition, followPosition) > maxDistance)
        {
            Vector3 nextPosition = Vector3.Lerp(transform.position, toFollow.position, 0.3f * Time.deltaTime);
            nextPosition.y = cameraHeight;

            transform.position = nextPosition;       
        }
	}
}
