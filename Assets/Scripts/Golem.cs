using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour {

	public Transform player;
	public Transform rockThrowingPosition;

	public GameObject rock;

	float timer = -1.2f;
	int waitingTime = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPostition = new Vector3( player.position.x, 
			this.transform.position.y, 
			player.position.z ) ;
		this.transform.LookAt( targetPostition ) ;
		timer += Time.deltaTime;
		if(timer > waitingTime){
			Shoot ();
			timer = 0;
		}
	}

	void Shoot(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			rock,
			rockThrowingPosition.position,
			rockThrowingPosition.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 40;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 4.0f); 
	}
}
