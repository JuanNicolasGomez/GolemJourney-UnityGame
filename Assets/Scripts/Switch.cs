using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public GameObject obj;
	public Transform posObj;
	public GameObject objPref;
	public string tipo = "D";

	Renderer rend;
	// Use this for initialization
	void Start (){
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	public bool encima = false;
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "movableObject") {
			encima = true;
			rend.material.color = Color.green;

			//var tes = (GameObject)Instantiate(obj, posObj.position, posObj.rotation);
			if(tipo == "D"){
				//posObj.position = obj.transform.position;
				Destroy (obj);
			}


		}
	}

	/*void OnCollisioinExit(Collision other){
		if (other.gameObject.tag == "TeleportableObject") {
			encima = false;
			//this.GetComponent<Material> ().color = Color.green;

			if (tipo == "D") {
				obj = (GameObject)Instantiate (objPref, posObj.position, posObj.rotation);
			}
		}
	}*/
}
