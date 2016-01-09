using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private readonly int Speed = 15;
    private readonly float MovementAccuracy = 0.01f;

    private bool isActive = false;
    private int value = 1;

    private bool isMoving = false;
    private Transform targetToMove;


    public int Value
    {
        get { return value; }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        Refresh();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        Move();

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
                        DiceCtrl.Notify();
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
        //value = UnityEngine.Random.Range(1, 7);
        value = UnityEngine.Random.Range(5, 6);
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

    public void StartMoving(Transform target)
    {
        targetToMove = target;
        isMoving = true;
    }

    private void StopMoving()
    {
        isMoving = false;
        targetToMove = null;
        DiceCtrl.DiceStopsMoving();
    }

    public void Move()
    {
        Debug.Log(Speed*Time.deltaTime);
        if (isMoving && targetToMove != null)
            transform.position = Vector3.MoveTowards(transform.position, targetToMove.position, Speed * Time.deltaTime);
        if (targetToMove != null && HasReachedTarget(transform.position, targetToMove.position))
            StopMoving();
    }

    private bool HasReachedTarget(Vector3 actualPosition, Vector3 targetPosition)
    {
        if (Math.Abs(actualPosition.x - targetPosition.x) < MovementAccuracy
            && Math.Abs(actualPosition.z - targetPosition.z) < MovementAccuracy)
            return true;
        return false;
    }

    /// <summary>
    /// adjusts the shown number on the dice
    /// </summary>
    public void Refresh()
    {
        GameObject d = GameObject.Find(gameObject.name + "/default");
        Material number;

        switch (Value)
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