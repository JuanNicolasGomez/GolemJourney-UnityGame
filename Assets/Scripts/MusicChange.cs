using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChange : MonoBehaviour {

    public AudioSource[] ad;
    public AudioClip clip;
	// Use this for initialization
	void Start () {
		ad = Camera.main.gameObject.GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ad[0].clip = clip;
            ad[0].Play();
        }
    }
}
