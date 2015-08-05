using UnityEngine;
using System.Collections;

public class ScreenRelativePosition : MonoBehaviour {

	public enum ScreenEdge {LEFT, RIGHT, TOP, BOTTOM};
	public ScreenEdge screenEdge;
	public float yOffset;
	public float xOffset;

	// Use this for initialization
	void Start () {
		Vector3 newPosition = transform.position; // copies the object's current position
		Camera camera = Camera.main;			  // gets a reference to the scene's main camera

		switch (screenEdge) 
		{
		case ScreenEdge.RIGHT: // if right edge of screen, reposition the object's x value
			newPosition.x = camera.aspect * camera.orthographicSize + xOffset;
			newPosition.y = yOffset;
			break;
			// orthographicSize is half the view's height, multiplying it by camera.aspect gives us half the width
			// camera.aspect is the aspect ratio of the camera
		case ScreenEdge.TOP:
			newPosition.x = xOffset;
			newPosition.y = camera.orthographicSize + yOffset;
			break;

		case ScreenEdge.LEFT:
			newPosition.x = -camera.aspect * camera.orthographicSize + xOffset;
			newPosition.y = yOffset;
			break;

		case ScreenEdge.BOTTOM:
			newPosition.x = xOffset;
			newPosition.y = -camera.orthographicSize + yOffset;
			break;
		}

		transform.position = newPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
