using UnityEngine;
using System.Collections;

public class GameFigure : MonoBehaviour
{
    private Player parent;
    private int index;
    private GameFieldBase field;
    private bool isActive = false;
    private float gameFigueScale = 0.6F;

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

    public GameFieldBase Field
    {
        get { return field; }
        set { field = value; }
    }
	
	// Update is called once per frame
	void Update ()
    {

        SetOnGround();
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
                        SetActive(false);

                        if (this.Field.GetType() == typeof(HomeField)) GameLogic.ReleaseFigure(this);
                        else GameLogic.MoveFigure(this);
                    }
                }
            }
        }
    }

    public void SetActive (bool isActive)
    {
        this.isActive = isActive;
    }

    public void SetOnGround() {
        float distance = 10;
        Vector3 direction = new Vector3(0, -1, 0);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction * distance, Color.green);

        if (Physics.Raycast(transform.position, direction, out hit, distance)) {
            Vector3 newPos = new Vector3(gameObject.transform.position.x,
                hit.point.y + gameFigueScale,
                gameObject.transform.position.z);
            gameObject.transform.position = newPos;
            Debug.Log(newPos);
        }
    }
}