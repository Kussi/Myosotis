using UnityEngine;

public class Dice : MonoBehaviour {

    private Player parent;
    private bool isActive = false;
    private int value = 1;

    public Player Parent
    {
        get { return this.parent; }
        set
        {
            if (parent == null) parent = value;

        }
    }

    public int Value
    {
        get { return value; }
        private set { this.value = value; }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start () {
        Refresh();
	}

    /// <summary>
    /// Update is called once per frame
    /// </summary>
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

    /// <summary>
    /// performs a dice throw
    /// </summary>
    public void ThrowDice()
    {
        Value = Random.Range(5, 7);
        Refresh();
        GameLogic.ExecuteTurn(value);
    }

    /// <summary>
    /// activtaes the dice, that is can be thrown
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    /// <summary>
    /// adjusts the shown number on the dice
    /// </summary>
    public void Refresh()
    {
        gameObject.GetComponent<TextMesh>().text = value.ToString();
    }
}