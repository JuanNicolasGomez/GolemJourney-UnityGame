using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {
	public Transform spawn;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "movableObject")
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "movableObject") {
            
        }
	}

}
