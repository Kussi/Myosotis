using UnityEngine;

public class Dice : MonoBehaviour {

    private Player parent;
    private bool isActive = false;
    private int value = 1;

    public Player Parent
    {
        get { return this.parent; }
        set { parent = value; }
    }

	// Use this for initialization
	void Start () {
        Refresh();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name + " has been touched");
                if (hit.collider.name == gameObject.name)
                {
                    if (isActive)
                    {
                        ThrowDice();
                    }
                }
            }
        }
    }

    public void ThrowDice()
    {
        value = Random.Range(1, 6);
        SetActive(false);
        Refresh();
        GameLogic.ExecuteTurn(value);

        Debug.Log(parent.ToString() + " dice has thrown a " + value);
    }

    public void SetActive(bool isActive)
    {
        Debug.Log("set active " + isActive);
        this.isActive = isActive;
    }

    public void Refresh()
    {
        gameObject.GetComponent<TextMesh>().text = value.ToString();
    }
}