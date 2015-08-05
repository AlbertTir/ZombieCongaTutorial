using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	public float moveSpeed;
	public float turnSpeed;

	private Vector3 moveDirection;

	[SerializeField] // tells Unity to expose the instance variable(colliders) below in the inspector. 
	//It basically lets us make a variable private in code, but still accessible from Unity's editor
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;


	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {

		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if( Input.GetButton("Fire1") ) {
			// 3
			Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();
		}

		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = 
			Quaternion.Slerp( transform.rotation, 
			                 Quaternion.Euler( 0, 0, targetAngle ), 
			                 turnSpeed * Time.deltaTime );
		EnforceBounds ();
	}

	public void SetColliderForSprite(int spriteNum)
	{
		// disables the current collider, then enables the new sprite's collider to whichever frame is being shown at the moment
		colliders[currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders[currentColliderIndex].enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Hit " + other.gameObject);
	}

	private void EnforceBounds()
	{
		Vector3 newPosition = transform.position; //copies the zombie's current position, ensuring the zombie maintains its z position
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;

		float xDist = mainCamera.aspect * mainCamera.orthographicSize; // calculates distance from the center of the screen to one of its edges
		float xMax = cameraPosition.x + xDist;						   // adds the distance to the camera's x position
		float xMin = cameraPosition.x - xDist;
		// essentially the 3 above lines finds the max and min range of the x-axis

		if(newPosition.x < xMin || newPosition.x > xMax)
		{
			// checks if the zombie is going out of bound, if it is, then reverse the direction the zombie is going
			newPosition.x = Mathf.Clamp (newPosition.x, xMin, xMax);
			moveDirection.x = -moveDirection.x;
		}

		// since the camera is at the center of the screen, the yMin and yMax is just the negative and positive values of the same value
		// therefore we can condense this to just one line
		float yMax = mainCamera.orthographicSize;

		if(newPosition.y < -yMax || newPosition.y > yMax)
		{
			newPosition.y = Mathf.Clamp(newPosition.y, -yMax, yMax);
			moveDirection.y = -moveDirection.y;
		}

		transform.position = newPosition; // updates the zombie's position with newPosition
		// this will be the same position the zombie had when this method is called if the zombie is still within horizontal bounds
	}
}