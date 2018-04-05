using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
	public int index = 1;

	private bool isActivo;
	public Material noActivo;
	public Transform spawnPoint;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		isActivo = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player" && isActivo) {
			SceneManager.LoadScene (index,LoadSceneMode.Single);


			other.gameObject.GetComponent<RPGCharacterControllerFREE> ().respawnCharacter = spawnPoint;
			this.tag = "Untagged";
			rend.material = noActivo;
			isActivo = false;
		}
	}
}
