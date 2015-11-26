using UnityEngine;
using System.Collections;

public class GameFigure : MonoBehaviour
{
    private Player parent;
    private int index;
    private int field;
    private bool isActive = false;
    

    public Player Parent
    {
        get { return this.parent; }
        set { parent = value; }
    }

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public int Field
    {
        get { return field; }
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log(parent.Color + index);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider.tag == parent.Color)
                //{
                //    if (isActive)
                //    {
                //        ThrowDice();
                //    }
                //}
            }
        }
    }
}
