using System;
using System.Collections;
using UnityEngine;

public class RegularField : GameFieldBase
{

    private readonly int figureCapacity = 2;
    private readonly string parentBenchObject = "BenchFields";
    private readonly string parentBendObject = "BendFields";

    // the first position is for one figure only. If there are two figures on the 
    // field, the regular position (0, 0, 0) may not be occupied
    private readonly Vector3[] regularPositions = new Vector3[]
    { new Vector3(0, 0, 0), new Vector3(0, 0, 0.5F), new Vector3(0, 0, -0.5F) };
    private readonly Vector3[] bendPositions = new Vector3[]
    { new Vector3(0, 0, 0), new Vector3(0.7F, 0, 0) };

    private bool isBench = false;
    private bool isBend = false;

    public bool IsBarrier
    {
        get { return IsOccupied; }
    }

    protected override bool IsOccupied
    {
        get
        {
            if (isBend)
                return base.IsOccupied;
            else if (fieldPositions[1].IsOccupied && fieldPositions[2].IsOccupied)
                return true;
            return false;
        }
    }

    private bool IsFullyEmpty
    {
        get
        {
            if (isBend)
                return !fieldPositions[0].IsOccupied && !fieldPositions[1].IsOccupied;
            else
                return !fieldPositions[0].IsOccupied && !fieldPositions[1].IsOccupied 
                    && !fieldPositions[2].IsOccupied;
        }
    }

    public bool IsBend
    {
        get { return isBend; }
    }

    public bool IsBench
    {
        get { return isBench; }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        if (gameObject.transform.parent.name.Equals(parentBenchObject)) isBench = true;
        else if (gameObject.transform.parent.name.Equals(parentBendObject)) isBend = true;

        // a regular field has different positions, if there are one or two figures
        // present. One in the center and further two left an right of the center
        if (isBend)
        {
            fieldPositions = new FieldPosition[figureCapacity];
            InitializeFieldPositions(bendPositions);
        }
        else
        {
            fieldPositions = new FieldPosition[figureCapacity + 1];
            InitializeFieldPositions(regularPositions);
        }
    }

    /// <summary>
    /// Removes the Figure from its actual field an places it here
    /// </summary>
    /// <param name="figure">Figure that has to be moved</param>
    public override void PlaceFigure(Figure figure)
    {
        if (IsOccupied) throw new InvalidGameStateException();
        FieldPosition fieldPosition = null;
        if (isBend)
        {
            for (int i = 0; i < fieldPositions.Length; ++i)
            {
                if (!fieldPositions[i].IsOccupied)
                {
                    fieldPosition = fieldPositions[i];
                    break;
                }
            }
            if(fieldPosition == null)
                throw new InvalidGameStateException();
        }
        else
        {
            if (IsFullyEmpty)
                fieldPosition = fieldPositions[0];
            else if(fieldPositions[0].IsOccupied)
            {
                ChangePosition(fieldPositions[0], fieldPositions[1]);
                fieldPosition = fieldPositions[2];
            }
            else if (fieldPositions[1].IsOccupied)
                fieldPosition = fieldPositions[2];
            else fieldPosition = fieldPositions[1];
        }
        AdjustReferences(fieldPosition, figure);
        MoveFigureObject(fieldPosition, figure);
    }

    public Figure GetFigureToSendHome(Figure figure)
    {
        if (isBench) return null;
        ArrayList figuresOnField = GetFiguresOnField();
        if (figuresOnField.Count == 0) return null;
        Figure figureToSendHome = (Figure)figuresOnField[0];
        if (FigureCtrl.GetPlayer(figureToSendHome).Color.Equals(FigureCtrl.GetPlayer(figure).Color))
            return null;
        return figureToSendHome;
    }

    private void ChangePosition(FieldPosition from, FieldPosition to)
    {
        Figure figure = from.Figure;
        from.Figure = null;
        to.Figure = figure;
        MoveFigureObject(to, figure);
    }
}