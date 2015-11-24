using UnityEngine;
using System.Collections;

public class DiceCtrl : MonoBehaviour {

	public bool isClicked;
	
	// Use this for initialization
	void Start () {

	}

	Ray ray;
	RaycastHit hit;
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit)){
			if(Input.GetMouseButtonDown(0)){
				try {
					gameObject.GetComponent<TextMesh>().text = "" + Random.Range(1,7);
				} catch (UnityException e) {} 
				//Debug.Log(Random.Range(1,7));
			}
		}
	}
}
