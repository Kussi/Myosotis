using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of a textbubble element
/// </summary>
public class Text : MonoBehaviour
{
    private static string ParentObject = "TextSystem";
    private enum Quadrant { RightTop, LeftTop, LeftBottom, RightBottom, Default = 0 };

    public float minAspectRatio;
    public float maxAspectRatio;
    public int step;
    public Figure figure;
    // 0 = rightTop, 1 = leftTop, 2 = leftBottom, 3 = rightBottom
    public Sprite[] bubbleFrames;

    private Quadrant actualQuadrant;

    private float verticalOffset = 0;
    private float horizontalOffset = 0;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        gameObject.transform.parent.gameObject.transform.SetParent(GameObject.Find(ParentObject).transform);
        gameObject.transform.parent.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// resizes the bubble according to its content
    /// </summary>
    void Update()
    {
        float aspectRatio = GetMinWidth() / GetPreferredHeight();

        if (aspectRatio < minAspectRatio)
            IncreaseAspectRatio();
        else if (aspectRatio > maxAspectRatio)
            DecreaseAspectRatio();

        if (figure != null)
        {
            actualQuadrant = GetQuadrant(GetNormalizedPosition(figure));
            Sprite bubble;
            switch (actualQuadrant)
            {
                case Quadrant.RightTop:
                    bubble = bubbleFrames[0];
                    gameObject.transform.parent.GetComponent<LayoutGroup>().padding.top = 50;
                    verticalOffset = -(GetHeight() / 2);
                    horizontalOffset = -(GetWidth() / 2);
                    break;
                case Quadrant.LeftTop:
                    bubble = bubbleFrames[1];
                    gameObject.transform.parent.GetComponent<LayoutGroup>().padding.top = 50;
                    verticalOffset = -(GetHeight() / 2);
                    horizontalOffset = (GetWidth() / 2);
                    break;
                case Quadrant.LeftBottom:
                    bubble = bubbleFrames[2];
                    gameObject.transform.parent.GetComponent<LayoutGroup>().padding.bottom = 50;
                    verticalOffset = (GetHeight() / 2);
                    horizontalOffset = (GetWidth() / 2);
                    break;
                case Quadrant.RightBottom:
                    bubble = bubbleFrames[3];
                    gameObject.transform.parent.GetComponent<LayoutGroup>().padding.bottom = 50;
                    verticalOffset = (GetHeight() / 2);
                    horizontalOffset = -(GetWidth() / 2);
                    break;
                default:
                    throw new InvalidGameStateException();
            }
            gameObject.transform.parent.GetComponent<UnityEngine.UI.Image>().sprite = bubble;
            gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition = GetViewportPosition(figure);
        }

        SetVisible();
    }

    /// <summary>
    /// assigns a figure to the bubble
    /// </summary>
    /// <param name="figure"></param>
    public void SetFigure(Figure figure)
    {
        this.figure = figure;
    }

    /// <summary>
    /// returns the position of a figue on the gameboard
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    private Vector2 GetViewportPosition(Figure figure)
    {
        Vector2 position = GetNormalizedPosition(figure);
        float width = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.width;
        float height = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.height;
        position = new Vector2(position.x * width + horizontalOffset, position.y * height + verticalOffset);
        return position;
    }

    /// <summary>
    /// returns the normalized position of a figure (for the
    /// placement on the canvas level)
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    private static Vector2 GetNormalizedPosition(Figure figure)
    {
        return Camera.main.WorldToViewportPoint(figure.transform.position);
    }

    private Quadrant GetQuadrant(Vector2 normalizedPosition)
    {
        if (normalizedPosition.x < 0.5)
        {
            if (normalizedPosition.y < 0.5)
                return Quadrant.LeftBottom;
            else return Quadrant.LeftTop;
        }
        else
        {
            if (normalizedPosition.y < 0.5)
                return Quadrant.RightBottom;
            else return Quadrant.RightTop;
        }
    }

    private float GetMinWidth()
    {
        return gameObject.GetComponent<LayoutElement>().minWidth;
    }

    private float GetPreferredHeight()
    {
        return LayoutUtility.GetPreferredHeight(gameObject.GetComponent<RectTransform>());
    }

    /// <summary>
    /// returns the width of the bubble as a sum of minwidth and paddings
    /// </summary>
    /// <returns></returns>
    private float GetWidth()
    {
        LayoutGroup group = gameObject.transform.parent.GetComponent<LayoutGroup>();
        float width = GetMinWidth();
        width += group.padding.left;
        width += group.padding.right;
        return width;
    }

    /// <summary>
    /// returns the height of the bubble as a sum of preferredheight and paddings
    /// </summary>
    /// <returns></returns>
    private float GetHeight()
    {
        LayoutGroup group = gameObject.transform.parent.GetComponent<LayoutGroup>();
        float height = GetPreferredHeight();
        height += group.padding.top;
        height += group.padding.bottom;
        return height;
    }

    /// <summary>
    /// increases the aspectRatio by increasing the width
    /// </summary>
    private void IncreaseAspectRatio()
    {
        ChangeMinWidth(step);
    }

    /// <summary>
    /// decreases the aspectRatio by decreasing the width
    /// </summary>
    private void DecreaseAspectRatio()
    {
        ChangeMinWidth(-step);
    }

    /// <summary>
    /// change the width of the bubble
    /// </summary>
    /// <param name="difference"></param>
    private void ChangeMinWidth(int difference)
    {
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
        layoutElement.minWidth += difference;
    }

    /// <summary>
    /// display the bubble
    /// </summary>
    private void SetVisible()
    {
        gameObject.GetComponent<UnityEngine.UI.Text>().color = new Color32(0, 0, 0, 255);
        gameObject.transform.parent.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
    }
}