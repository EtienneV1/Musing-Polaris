﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstMusicControl : MonoBehaviour
{
	public bool testVar = false;
	public float musicVolumeRampSpeed = 0.02f;
	public float musicRampAtStart = 0f;
	public float maxVolume = 0.5f;

	private Transform polaris;
	private Collider localCollider;

	private bool isMusicActive = false;
	private float localVolume = 0;

	//"musicRampAtStart" is a variable that makes the music ramps up much slower the first time it is invoked.
	//It is subsequently set to 1 when the player first exits a constellation.

	//--------------------------START---------------------------//
	void Start ()
	{
		polaris = Camera.main.GetComponent<LerpToPolaris> ().polaris;
		localCollider = GetComponent<Collider> ();
		if (localCollider.bounds.Contains (polaris.position)) {
			isMusicActive = true;
			Camera.main.GetComponent<LerpToPolaris> ().currentConst = gameObject.name;
		}
		StartCoroutine (RemoveStartRamp ());
	}
	//----------------------------------------------------------//

	//--------------------------UPDATE--------------------------//
	void Update ()
	{
		//This part controls what the local volume should be.
		if (isMusicActive && localVolume < maxVolume) {
			localVolume += Time.deltaTime * musicVolumeRampSpeed * musicRampAtStart;
			if (localVolume > maxVolume) {
				localVolume = maxVolume;
				musicRampAtStart = 1;
			}
		} else if (!isMusicActive && localVolume > 0) {
			localVolume -= Time.deltaTime * musicVolumeRampSpeed * musicRampAtStart;
			if (localVolume < 0) {
				localVolume = 0;
				musicRampAtStart = 1;
			}
		}// End of this part -----------------------------------------------------------

		//The audiosource's volume should always be equal to localVolume.
		GetComponent<AudioSource> ().volume = localVolume; 

		//This checks what constellation Polaris is currently in. That parameter is determined in this very same script
		//But is sent to the camera, as it is simultaneously sent by the different constellations of the game.
		if (Camera.main.GetComponent<LerpToPolaris> ().currentConst == gameObject.name) {
			isMusicActive = true; //Then it says whether it should be playing music...
		} else {
			isMusicActive = false; //...or if it shouldn't.
		}// End of the current constellation check.--------------------------------------

		//This is a check that checks wether or not polaris is within the collider's bounds (As of now it works.)
		if (localCollider.bounds.Contains (polaris.position)) {
			testVar = true;
		} else
			testVar = false;
		//-------------------------------------------------------------------------------
	}
	//-------------------------------------------------------//

	//------------------ON_TRIGGER_ENTER---------------------//
	void OnTriggerEnter (Collider other)
	{
		if (other == polaris.GetComponent<Collider> ()) {
			Camera.main.GetComponent<LerpToPolaris> ().currentConst = gameObject.name;
		}
	}
	//-------------------------------------------------------//

	//------------------REMOVE_START_RAMP---------------------//
	IEnumerator RemoveStartRamp ()
	{
		yield return new WaitForSeconds (5);
		musicRampAtStart = 1;
		yield return null;
	}
	//-------------------------------------------------------//
}
