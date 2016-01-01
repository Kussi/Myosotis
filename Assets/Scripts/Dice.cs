using UnityEngine;

public class Dice : MonoBehaviour {

    private bool isActive = false;
    private int value = 1;

    public int Value
    {
        get { return value; }
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
                        ThrowDice();
                        DiceCtrl.Notify(this);
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
        value = Random.Range(5, 7);
        Refresh();   
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
        GameObject d = GameObject.Find(gameObject.name + "/default");
        Material number;

        switch(Value)
        {
            case 1:
                number = Resources.Load("Materials/Dice_Texture1", typeof(Material)) as Material;
                break;
            case 2:
                number = Resources.Load("Materials/Dice_Texture2", typeof(Material)) as Material;
                break;
            case 3:
                number = Resources.Load("Materials/Dice_Texture3", typeof(Material)) as Material;
                break;
            case 4:
                number = Resources.Load("Materials/Dice_Texture4", typeof(Material)) as Material;
                break;
            case 5:
                number = Resources.Load("Materials/Dice_Texture5", typeof(Material)) as Material;
                break;
            case 6:
                number = Resources.Load("Materials/Dice_Texture6", typeof(Material)) as Material;
                break;
            default:
                number = Resources.Load("Materials/Dice_Texture1", typeof(Material)) as Material;
                break;
        }

        Material[] mats = new Material[2];
        mats[0] = d.GetComponent<MeshRenderer>().materials[0];
        mats[1] = number;
        d.GetComponent<MeshRenderer>().materials = mats;
    }
}