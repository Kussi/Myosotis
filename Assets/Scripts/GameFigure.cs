using UnityEngine;
using System.Collections;

public class GameFigure : MonoBehaviour
{
    private Player parent;
    private int index;
    private AbstractGameField field;
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
    public AbstractGameField Field
    {
        get { return field; }
        set { field = value; }
    }

    // Use this for initialization
    void Start ()
    {

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
                if (hit.collider.name == gameObject.name)
                {
                    if (isActive)
                    {
                        Debug.Log(gameObject.name + " does something cool." );
                        SetActive(false);
                        GameLogic.Move(this);
                    }
                }
            }
        }
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }
}