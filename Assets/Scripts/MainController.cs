using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour
{
    private const int nofGameFields = 68;

    private static ArrayList gameFields;

    private Player redPlayer, yellowPlayer, bluePlayer, greenPlayer;
    private bool redIsActive = false;
    private bool yellowIsActive = false;
    private bool blueIsActive = false;
    private bool greenIsActive = false;

	// Use this for initialization
	void Start ()
    {
        gameFields = new ArrayList();

        // Adding all Gamefields (except of the stairs) to the list
        for (int i = 1; i <= nofGameFields; ++i)
        {
            gameFields.Add(GameObject.Find("Field" + i));
        }

        // Instanciating Players
        redPlayer = new Player("red");
        yellowPlayer = new Player("yellow");
        bluePlayer = new Player("blue");
        greenPlayer = new Player("green");

    }
	
	// Update is called once per frame
	void Update ()
    {
	   
	}

    // returns the position of the gameField with the index
    public static Vector3 GetPositionFromField(int index)
    {
        if(index >= gameFields.Count || index < 0)
        {
            index = 0;
        }
        return ((GameObject)gameFields[index]).transform.position;
    }

    public static void PlayerInfo(Player player)
    {
        
    }
}
