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

    public int Value
    {
        get { return value; }
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
                if (hit.collider.name == gameObject.name)
                {
                    if (isActive)
                    {
						SetActive(false);
                        ThrowDice();
                    }
                }
            }
        }
    }

    public void ThrowDice()
    {
        value = Random.Range(5, 7);
        Refresh();
        GameLogic.ExecuteTurn(value);
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void Refresh()
    {
        gameObject.GetComponent<TextMesh>().text = value.ToString();
    }
}