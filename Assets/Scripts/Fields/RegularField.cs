using System;
using UnityEngine;

public class RegularField : GameFieldBase
{

    private const int figureCapacity = 2;
    private const string parentBenchObject = "BenchFields";
    private const string parentBendObject = "BendFields";

    // the first position is for one figure only. If there are two figures on the field, the regular position (0, 0, 0) may not be occupied
    private readonly Vector3[] regularPositions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0.5F), new Vector3(0, 0, -0.5F) };
    private readonly Vector3[] bendPositions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0.7F, 0, 0) };

    private bool isBench = false;
    private bool isBend = false;

    public bool IsBench
    {
        get { return isBench; }
    }

    private bool IsBend
    {
        get { return isBend; }
    }

    public bool IsBarrier
    {
        // if there are two figures present, FieldPosition[0] has to be free
        get { return FieldPositions[1].IsOccupied && FieldPositions[2].IsOccupied; }
    }

    protected override bool IsOccupied
    {
        get
        {
            if(IsBend)
            {
                return base.IsOccupied;
            }
            else
            {
                if (FieldPositions[1].GameFigure != null && FieldPositions[2].GameFigure != null) return true;
                return false;
            }
        }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void OnEnable()
    {
        if (gameObject.transform.parent.name.Equals(parentBenchObject)) isBench = true;
        if (gameObject.transform.parent.name.Equals(parentBendObject)) isBend = true;

        if(IsBend)
        {
            SetFieldPositions(new FieldPosition[figureCapacity]);
        }
        else
        {
            // a regular field has different positions, if there are one or two figures
            // present. One in the center and further two left an right of the center
            SetFieldPositions(new FieldPosition[figureCapacity + 1]);
        }        

        for (int i = 0; i < FieldPositions.Length; ++i)
        {
            GameObject positionObject = new GameObject();
            positionObject.name = "position" + i;
            positionObject.transform.parent = gameObject.transform;

            if(IsBend) positionObject.transform.localPosition = bendPositions[i];
            else positionObject.transform.localPosition = regularPositions[i];

            positionObject.AddComponent<FieldPosition>();
            FieldPositions[i] = positionObject.GetComponent<FieldPosition>();
        }
    }

    /// <summary>
    /// Removes the GameFigure from its actual field an places it here
    /// </summary>
    /// <param name="figure">Figure that has to be moved</param>
    public override void PlaceGameFigure(GameFigure figure)
    {
        if(IsOccupied) throw new InvalidGameStateException("Figure cannot be placed on this Field (" + gameObject.name + ")");

        if (IsBend)
        {
            for (int i = 0; i < FieldPositions.Length; ++i)
            {
                if (!FieldPositions[i].IsOccupied)
                {
                    FieldPositions[i].GameFigure = figure;
                    break;
                }
            }
        }
        else
        {
            if(FieldPositions[1].GameFigure == null && FieldPositions[2].GameFigure == null)
            {
                // if there is no figure on this field yet
                if (FieldPositions[0].GameFigure == null)
                {
                    FieldPositions[0].GameFigure = figure;
                }

                // if there is already one figure on this field, it has to be replaced
                else
                {
                    FieldPositions[1].GameFigure = FieldPositions[0].GameFigure;
                    FieldPositions[0].GameFigure = null;
                    FieldPositions[2].GameFigure = figure;
                }
            }
        }
        RefreshPositionsAfterPlacement();
    }

    /// <summary>
    /// adjusts the positions of the figures
    /// </summary>
    protected override void RefreshPositionsAfterRemoval()
    {
        if(IsBend)
        {
            if (FieldPositions[0] == null && FieldPositions[1] != null)
            {
                FieldPositions[0] = FieldPositions[1];
                FieldPositions[1] = null;
            }
        }
        else
        {
            // it doesn't matter whether FieldPosition[2] is null or not
            if (FieldPositions[1] == null) FieldPositions[0] = FieldPositions[2];
            
            // it doesn't matter whether FieldPosition[1] is null or not
            else if (FieldPositions[2] == null) FieldPositions[0] = FieldPositions[1];
        }
    }
}