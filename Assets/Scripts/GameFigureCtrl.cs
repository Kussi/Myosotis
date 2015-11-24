using UnityEngine;
using System.Collections;

public class GameFigureCtrl : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    public int field;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            ++field;
            gameObject.transform.position = MainController.GetPositionFromField(field);
        }
    }
}
