using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
	//Name of the room targeted by the play button et value that helps with lowering the title music sound
	public string playTargetRoom;
	private bool tempSwitch = false;
	private float tempVolume = 1f;

	//Sound variable for the masterVolume
	private float masterVolume;

	//Variables used for playing sounds with the interface buttons
	private AudioSource audioSource;
	public AudioClip playAClip;
	public AudioClip genericAClip;
	public AudioClip leaveAClip;

	// Variables for the options menu
	public Slider masterVolumeSlider;
	public Toggle subtitlesToggle;

	// Blackscreen for fade in/fade out
	public GameObject blackScreen;

	//--------------------------START---------------------------//
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		//Mise à jour du volume si un paramètre de préférence existe
		if (PlayerPrefs.HasKey ("masterVolumePref")) {
			masterVolume = PlayerPrefs.GetFloat ("masterVolumePref");
			masterVolumeSlider.value = masterVolume;
			Camera.main.GetComponent<AudioSource> ().volume = masterVolume / 4;
		}
	}
	//----------------------------------------------------------//


	//-------------------------UPDATE---------------------------//
	void Update ()
	{
		if (tempSwitch == true) {
			tempVolume -= Time.deltaTime / 2f;
			Camera.main.GetComponent<AudioSource> ().volume = masterVolume * tempVolume / 4;
		} else {
			Camera.main.GetComponent<AudioSource> ().volume = masterVolume / 4;
		}

		if (!PlayerPrefs.HasKey ("masterVolumePref")) {
			PlayerPrefs.SetFloat ("masterVolumePref", 0.5f);
			masterVolume = 0.5f;
		}
	}
	//----------------------------------------------------------//


	//----------------------VOLUME_UPDATE-----------------------//
	//Called whenever the Master Volume Slider changes value
	public void UpdateVolume ()
	{
		//masterVolumePref = Stockage du masterVolume préférentiel
		PlayerPrefs.SetFloat ("masterVolumePref", masterVolume);
		masterVolume = masterVolumeSlider.value;
	}
	//----------------------------------------------------------//

	//----------------------LAUNCH_GAME-------------------------//
	public void LaunchGame ()
	{
		tempSwitch = true;
		blackScreen.GetComponent<Animation> ().Play ("FadeIn");
		StartCoroutine (GoToGame ());
	}
	//----------------------------------------------------------//
	//Those two are linked -------------------------------------//
	//----------------------GOTO_GAME---------------------------//
	public IEnumerator GoToGame ()
	{
		audioSource.volume = masterVolume;
		audioSource.PlayOneShot (playAClip);

		yield return new WaitForSeconds (
			blackScreen.GetComponent<Animation> ().clip.length); //Lets the FadeIn animation play out
		SceneManager.LoadScene (playTargetRoom);
		yield break;
	}
	//----------------------------------------------------------//


	//------------------INIT_QUIT_GAME---------------------------//
	public void InitQuitGame ()
	{
		audioSource.volume = masterVolume;
		audioSource.PlayOneShot (leaveAClip);

		Debug.Log ("Game closing");
		blackScreen.GetComponent<Animation> ().Play ("FadeIn");
		StartCoroutine (QuitGame ());
	}
	//----------------------------------------------------------//
	//Those two are linked -------------------------------------//
	//----------------------QUIT_GAME---------------------------//
	IEnumerator QuitGame ()
	{
		yield return new WaitForSeconds (2);
		Debug.Log ("Game closed");
		Application.Quit ();
		yield break;
	}
	//----------------------------------------------------------//

	//---------------------SUBTITLES_TOGGLE---------------------//
	//Called whenever the Subtitle Toggle changes value
	public void ToggleSubtitles ()
	{
		//This all just changes the text displayed next to the Toggle. Effectively does nothing.
		if (subtitlesToggle.isOn) {
			subtitlesToggle.GetComponentInChildren<TextMeshProUGUI> ().SetText ("...there is no dialog");
		} else {
			subtitlesToggle.GetComponentInChildren<TextMeshProUGUI> ().SetText ("Subtitles");
		}
//		audioSource.volume = masterVolume;
//		audioSource.PlayOneShot (audioClip);
	}
	//----------------------------------------------------------//


	//------------------------NAVIGATION------------------------//
	//Called when certain buttons are pressed (Return buttons, Options button, Compendum button)
	public void MenuNavigation (GameObject whereToGo)
	{
		foreach (Transform children in transform) {
			if (children.name == "ScreenMain" || /////
			    children.name == "ScreenOptions" ||	// Condition checks that only Screen Menu Groups get disabled
			    children.name == "ScreenAtlas") {	//
				children.gameObject.SetActive (false); //The menu deactivated itself before activating the target menu
			}
		}
		whereToGo.SetActive (true); //Target menu activates
		audioSource.volume = masterVolume;
		audioSource.PlayOneShot (genericAClip);
	}
	//----------------------------------------------------------//

}
