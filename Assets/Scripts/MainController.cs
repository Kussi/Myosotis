using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour
{

    private static ArrayList gameFields;

	// Use this for initialization
	void Start ()
    {
        gameFields = new ArrayList();

        // Adding all Gamefields (except of the stairs) to the list
        for (int i = 1; i <= 68; ++i)
        {
            gameFields.Add(GameObject.Find("Field" + i));
        }
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
}
