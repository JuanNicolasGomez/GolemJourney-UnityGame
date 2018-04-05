using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCube : MonoBehaviour {

	Rigidbody rb;
	public Transform respawn;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player") {
			StartCoroutine (_Fall());
			StartCoroutine (_Revive ());
			
		}
	}

	IEnumerator _Fall(){
		yield return new WaitForSeconds(0.4f);
		rb.useGravity = true;
		rb.isKinematic = false;

	}

	IEnumerator _Revive(){
		yield return new WaitForSeconds(6f);
		this.transform.position = respawn.position;
		rb.useGravity = false;
		rb.isKinematic = true;
	}

}
