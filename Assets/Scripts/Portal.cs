using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Collider portalDestino;
	public Transform destination;

	// Use this for initialization
	void Start () {
		
	}
	IEnumerator OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "TeleportableObject") {
            portalDestino.enabled = false;
            other.transform.position = destination.position;
			yield return new WaitForSeconds (4);
			portalDestino.enabled = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
