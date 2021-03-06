﻿using System;
using UnityEngine;

/// <summary>
/// Representation of the Dice Object
/// </summary>
public class Dice : MonoBehaviour
{
    private enum State { ReadyToThrow, Throwing, ReadyToPassOn, PassingOn }

    private readonly float Speed = 20;
    private readonly float MovementAccuracy = 0.01f;
    private readonly float ThrowingHeight = 3;
    
    private int value = 1;
    private bool isActive = false;

    private Vector3 throwingPosition;
    private Quaternion throwingRotation;

    private State actualState = State.PassingOn;

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
        PassingOn();
    }

    /// <summary>
    /// Checks whether the dice can be thrown or not
    /// </summary>
    /// <param name="hitPoint">Position of the touch (is not used, but implemented due to the TouchInput.cs)</param>
    void OnTouchDown(Vector3 hitPoint)
    {
        if (actualState.Equals(State.ReadyToThrow) && isActive)
            ThrowDice();
        else if (actualState.Equals(State.ReadyToPassOn) && isActive)
        {
            actualState = State.PassingOn;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    /// <summary>
    /// Checks all Collisions between dice and floor/ground after it has been thrown
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        MediaEventHandler.Notify(MediaEventHandler.SoundEvent.DiceFallsOnGround);
        if (actualState.Equals(State.Throwing))
            actualState = State.ReadyToPassOn;
    }

    /// <summary>
    /// performs a dice throw
    /// </summary>
    public void ThrowDice()
    {
        isActive = false;
        actualState = State.Throwing;
        GetComponent<Rigidbody>().isKinematic = false;
        value = UnityEngine.Random.Range(1, 7);
        Refresh();
        FinishThrow();
    }

    /// <summary>
    /// activates the dice
    /// </summary>
    public void ActivateDice()
    {
        isActive = true;
    }

    /// <summary>
    /// starts the movement of the dice to the next player
    /// </summary>
    /// <param name="target">Transform of the next dice position</param>
    public void StartPassingOn(Transform target)
    {
        Vector3 position = target.position;
        Quaternion rotation = target.rotation;
        throwingPosition = new Vector3(position.x, ThrowingHeight, position.z);
        throwingRotation = Quaternion.Euler(rotation.x + UnityEngine.Random.Range(0,30), rotation.y 
            + UnityEngine.Random.Range(0, 30), rotation.z 
            + UnityEngine.Random.Range(0, 30));
    }

    /// <summary>
    /// moves the dice to the next players position
    /// </summary>
    public void PassingOn()
    {
        if (actualState.Equals(State.PassingOn) && isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, throwingPosition, Speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, throwingRotation, Time.time * Speed);

            if (HasReachedTarget(transform.position, throwingPosition)) actualState = State.ReadyToThrow;
        }
    }

    /// <summary>
    /// finishes the throw
    /// </summary>
    public void FinishThrow()
    {
        DiceCtrl.Notify();
    }

    /// <summary>
    /// checks whether the dice has reached its target or not, when it's moved from one to another players position
    /// </summary>
    /// <param name="actualPosition"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private bool HasReachedTarget(Vector3 actualPosition, Vector3 targetPosition)
    {
        if (Math.Abs(actualPosition.x - targetPosition.x) < MovementAccuracy
            && Math.Abs(actualPosition.y - targetPosition.y) < MovementAccuracy
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