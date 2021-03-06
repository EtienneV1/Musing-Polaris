using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinks : MonoBehaviour
{
	//This is all stuff related to how and where to draw the starlinks
	public GameObject[] linkedStars;
	public GameObject linePrefab;

	private GameObject parentConstellation;
	private Color linksColor;
	private float linkAlpha = 0f;

	//--------------------------START---------------------------//
	void Start ()
	{
		parentConstellation = gameObject.GetComponentInParent<AreChildsActive> ().gameObject;
		linksColor = parentConstellation.GetComponent<AreChildsActive> ().constColor;

		for (int i = 0; i < linkedStars.Length; i++) {
			DrawLineBetween (transform.position, linkedStars [i].transform.position, linksColor);
		}
	}
	//----------------------------------------------------------//


	//-------------------------UPDATE---------------------------//
	void Update ()
	{
//		print (linksColor);
		if (gameObject.activeSelf && linkAlpha < 1) {
			linkAlpha += Time.deltaTime / 100;
		}
	}
	//----------------------------------------------------------//


	//---------------DRAW_LINE_BETWEEN_TWO_POINTS---------------//
	void DrawLineBetween (Vector3 start, Vector3 end, Color color)
	{
		print ("startLineDraw"); 
		GameObject myLine = Instantiate (linePrefab);
		myLine.transform.position = start;
		LineRenderer lr = myLine.GetComponent<LineRenderer> ();
		lr.startColor = new Color (color.r, color.g, color.b, 0);
		lr.endColor = new Color (color.r, color.g, color.b, 0);
		lr.startWidth = 0.125f;
		lr.endWidth = 0.125f;
		lr.SetPosition (0, start);
		lr.SetPosition (1, end);
	}
	//----------------------------------------------------------//
}
