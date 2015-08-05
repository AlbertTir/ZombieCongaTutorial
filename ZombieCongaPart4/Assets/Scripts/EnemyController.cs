using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float speed = -2;
	private Transform spawnPoint;

	// Use this for initialization
	void Start () {
		rigidbody2D.velocity = new Vector2 (speed, 0);
		spawnPoint = GameObject.Find ("SpawnPoint").transform; // finds an object named SpawnPoint and gets its transform component
		// note: GameObject.Find is slower at runtime than referencing a value set in the inspector, so avoid using this in repetitive calls like Update
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible(){
		if(Camera.main == null){
			// this paborts the method if the camera is not present
			return;
		}
		// this calculates a new position for the enemy. It uses the same x value from spawnPoint, but randomizes the y value
		// the -0.5 is for margin purposes to keep the enemy doesn't spawn too close to the top or bottom of the screen
		float yMax = Camera.main.orthographicSize - 0.5f;
		transform.position = new Vector3 (spawnPoint.position.x, Random.Range (-yMax, yMax), transform.position.z);
	}
}
