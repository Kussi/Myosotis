using UnityEngine;
using System;

/// <summary>
/// Representation of the figure objects
/// </summary>
public class Figure : MonoBehaviour
{
    public bool drawRays = true;

    private static readonly LayerMask CollisionLayer = LayerMask.GetMask("GameBoard", "GameTable");
    private readonly float GameFigueScale = 0.6F;
    private readonly float MovementAccuracy = 0.01f;
    private readonly float SoundAccuracy = 0.1f;
    private readonly int Speed = 5;

    private int field;
    private bool isActive = false;
    private bool isWalking = false;
    private Transform targetToWalk;
    private bool soundPlayed = false;

    private bool isMainTurnFigure = false;

    public int Field
    {
        get { return field; }
        set { field = value; }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        SetOnGround();
        Walk();
    }

    /// <summary>
    /// performs after a touch and checks whether the figure is activated or not.
    /// Notifies the FigureCtrl if it is active
    /// </summary>
    /// <param name="hitPoint"></param>
    void OnTouchDown(Vector3 hitPoint)
    {
        if (isActive)
        {
            isMainTurnFigure = true;
            FigureCtrl.Notify(this);
        }
    }

    /// <summary>
    /// activtaes a figure, that is can be moved
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    /// <summary>
    /// starts the moving process of the figure
    /// </summary>
    /// <param name="target"></param>
    public void StartWalking(Transform target)
    {
        FigureCtrl.FigureStartsWalking(this);
        targetToWalk = target;
        isWalking = true;
        soundPlayed = false;
    }
    /// <summary>
    /// stops the moving process of the figure
    /// </summary>
    private void StopWalking()
    {
        isWalking = false;
        targetToWalk = null;
        isMainTurnFigure = false;
        FigureCtrl.FigureStopsWalking(this);
    }

    /// <summary>
    /// performes the moving process of the figure
    /// </summary>
    private void Walk()
    {
        if (isWalking)
            transform.position = Vector3.MoveTowards(transform.position, targetToWalk.position, Speed * Time.deltaTime);
        if (targetToWalk != null && HasReachedTarget(transform.position, targetToWalk.position))
            StopWalking();
    }

    /// <summary>
    /// checks whether the figure has reached its target or not, when it's moved from one to another players position
    /// </summary>
    /// <param name="actualPosition"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private bool HasReachedTarget(Vector3 actualPosition, Vector3 targetPosition)
    {
        if (soundPlayed == false && Math.Abs(actualPosition.x - targetPosition.x) < SoundAccuracy
            && Math.Abs(actualPosition.z - targetPosition.z) < SoundAccuracy)
        {
            MediaEventHandler.Notify(MediaEventHandler.SoundEvent.FigureMakesStep);
            soundPlayed = true;
        }


        if (Math.Abs(actualPosition.x - targetPosition.x) < MovementAccuracy
            && Math.Abs(actualPosition.z - targetPosition.z) < MovementAccuracy)
            return true;


        return false;
    }

    /// <summary>
    /// places the figure on top of a surface (the ground or Gamefields)
    /// </summary>
    public void SetOnGround()
    {
        gameObject.transform.position += new Vector3(0, 2, 0);
        float distance = 10;
        Vector3 direction = new Vector3(0, -1, 0);
        RaycastHit hit;

        if (drawRays)
            Debug.DrawRay(transform.position, direction * distance, Color.green);

        if (Physics.Raycast(transform.position, direction, out hit, distance, CollisionLayer))
        {
            Vector3 newPos = new Vector3(gameObject.transform.position.x,
                hit.point.y + GameFigueScale,
                gameObject.transform.position.z);
            gameObject.transform.position = newPos;
        }
    }
}