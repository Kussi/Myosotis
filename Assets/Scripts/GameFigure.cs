using UnityEngine;
using System.Collections;

public class GameFigure : MonoBehaviour
{
    private const float GameFigueScale = 0.6F;

    private Player parent;
    private int field;
    private bool isActive = false;

    public Player Parent
    {
        get { return this.parent; }
        set
        {
            if(parent == null) parent = value;
        }
    }

    public int Field
    {
        get { return field; }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        SetOnGround();

        // Getting Touch/Mouse Inputs if the game figure is activated
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.name == gameObject.name)
                    {
                        SetActive(false);
                        GameCtrl.Notify(this);
                    }
                }
            }
        }
    }

    /// <summary>
    /// activtaes a figure, that is can be moved
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive (bool isActive)
    {
        this.isActive = isActive;
    }

    /// <summary>
    /// places the figure on top of a surface
    /// </summary>
    public void SetOnGround() {
        gameObject.transform.position += new Vector3(0, 2, 0);
        float distance = 10;
        Vector3 direction = new Vector3(0, -1, 0);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction * distance, Color.green);

        if (Physics.Raycast(transform.position, direction, out hit, distance)) {
            Vector3 newPos = new Vector3(gameObject.transform.position.x,
                hit.point.y + GameFigueScale,
                gameObject.transform.position.z);
            gameObject.transform.position = newPos;
        }
    }
}