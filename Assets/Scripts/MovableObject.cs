using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {
    public Transform spawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "KillZone") {
            transform.position = spawn.position;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "KillZone")
        {
            transform.position = spawn.position;
        }
    }

}
