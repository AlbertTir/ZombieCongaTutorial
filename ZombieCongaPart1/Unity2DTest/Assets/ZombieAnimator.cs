using UnityEngine;
using System.Collections;

public class ZombieAnimator : MonoBehaviour {

	// public instance variables for sprite ID and speed at which to cycle through them
	public Sprite[] sprites;
	public float framesPerSecond;
	public float moveSpeed;	// stores the number of units the zombie will move per second
	public float turnSpeed; // controls how quickly the zombie reorients himself to a new direction

	private SpriteRenderer spriteRenderer;
	private Vector3 moveDirection;	// despite being a 2D game, Unity still uses a 3D coordinate system; we will just ignore the Z-axis here since that will not change

	// Use this for initialization
	void Start () {
		// initializes spriteRenderer by giving it access to a variable named renderer
		// we're typecasting renderer to type SpriteRenderer here before storing it
		spriteRenderer = renderer as SpriteRenderer;
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		// these 3 lines of code takes the amount of seconds since the level loaded and multiplies it by the number of frames per second
		// "If the frames were stored in an infinitely long array, that would give you the index into the array for the current frame."
		// "However, since the array is not infinite, we need to loop back to the start when we reach the end; this is done by performing the modulus operation."
		// What we get is a number between 0 and 1 that's less than the size of the array unless the array is size of zero
		int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
		index = index % sprites.Length;
		spriteRenderer.sprite = sprites[index];
		
		// these next chunk of code determines the zombie's destination, its current position and gets the inputs
		Vector3 currentPosition = transform.position; // we make a local variable for current position since we'll be using this a lot
		if(Input.GetButton ("Fire1")){ // Fire1 is a virtual button axes defined by default, it is button 0 on a joystick or mouse, or left control key on the keyboard
			// this next line converts the current mouse position to a world coordinate.
			// with orthographic projection chosen earlier, the z-value has no effect on x and y values, so we can pass mousePosition directly
			Vector3 moveToward = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0;
			moveDirection.Normalize();
		}

		// these next 2 lines handles the zombie movement/walking
		Vector3 target = moveDirection * moveSpeed + currentPosition; 
		// finds the target location if the zombie were to travel from its current position to a direction pointed to by moveDirection for a duration of one second
		transform.position = Vector3.Lerp (currentPosition, target, Time.deltaTime);
		// Lerp is a method to interpolate between values based on a third. We use deltaTime for our third value as it would give us the fractions between 0 and 1 (read more on tutorial site)

		// using Quaternions,we can find the rotation for our zombie when he goes to a new target location
		float targetAngle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; // finds the angle between the x-axis and moveDirection, then converts from rad to deg
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0,0,targetAngle),turnSpeed * Time.deltaTime);
		// similar to Lerp, except we're doing a spherical linear interpolation between two angles for rotation instead of position. turnSpeed determines the distance travelled
	}
}
