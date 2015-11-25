using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    private bool isActive = false;
    private int facedNumber = 1;
    private Player parent;

    private Ray ray;
    private RaycastHit hit;

    // Use this for initialization
    void Start ()
    {
        parent = new Player("red");
        Refresh();
	}

	
	// Update is called once per frame
	void Update ()
    {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit)){
			if(Input.GetMouseButtonDown(0)){

                Debug.Log(parent.ToString() + " Dice has been touched");

                if (isActive)
                {
                    try
                    {
                        facedNumber = ThrowDice();
                        Refresh();
                        Notify();
                    }
                    catch (UnityException) { }
                }
            }
		}
	}

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        Debug.Log(parent.ToString() + " dice is active = " + isActive);
    }

    private int ThrowDice()
    {
        int thrownNumber = Random.Range(1, 7);

        Debug.Log(parent.ToString() + " dice has thrown a " + thrownNumber);

        return thrownNumber;
    }

    private void Refresh()
    {
        gameObject.GetComponent<TextMesh>().text = facedNumber.ToString();
    }

    private void Notify ()
    {
        MainController.PlayerInfo(parent);
    }
}
