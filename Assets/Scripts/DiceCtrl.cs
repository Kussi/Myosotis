using UnityEngine;
using System.Collections;

public class DiceControlling : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	Ray ray;
	RaycastHit hit;
	
	// Update is called once per frame
	void Update () {
		bool Text1 = false;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		if(Input.GetMouseButtonDown(0)){
			Text1 = true;
			Debug.Log(Random.Range(1,7));
		}
	}
}
