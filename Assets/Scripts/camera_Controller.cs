using UnityEngine;
using System.Collections;

public class camera_Controller : MonoBehaviour
{

	//public GameObject player;       //Public variable to store a reference to the player game object
	//private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	public Transform player; // Mario's Transform
	public Transform endLimit; // GameObject that indicates end of map
	private float offset; // initial x-offset between camera and Mario
	private float startX; // smallest x-coordinate of the Camera
	private float endX; // largest x-coordinate of the camera
	private float viewportHalfWidth;

	// Use this for initialization
	void Start()
	{
		// get coordinate of the bottomleft of the viewport
		// z doesn't matter since the camera is orthographic
		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
		viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

		offset = this.transform.position.x - player.position.x;
		startX = this.transform.position.x;
		endX = endLimit.transform.position.x - viewportHalfWidth;
	}
	void Update()
	{
		float desiredX = player.position.x + offset;
		// check if desiredX is within startX and endX
		if (desiredX > startX && desiredX < endX)
		this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
	}
}
