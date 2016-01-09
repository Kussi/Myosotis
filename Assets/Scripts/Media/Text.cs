using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text : MonoBehaviour
{
    private static int VerticalDistanceToBubble = 0;
    private static int HorizontalDistanceToBubble = 0;
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

    // Use this for initialization
    void Awake()
    {
        gameObject.transform.parent.gameObject.transform.SetParent(GameObject.Find(ParentObject).transform);
        gameObject.transform.parent.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float aspectRatio = GetMinWidth() / GetPreferredHeight();

        if (aspectRatio < minAspectRatio)
            IncreaseAspectRatio();
        else if (aspectRatio > maxAspectRatio)
            DecreaseAspectRatio();

        if (figure != null)
        {
            if (actualQuadrant.Equals(Quadrant.Default) || !(GetQuadrant(GetNormalizedPosition(figure))).Equals(actualQuadrant))
            {
                actualQuadrant = GetQuadrant(GetNormalizedPosition(figure));
                Sprite bubble;
                switch (actualQuadrant)
                {
                    case Quadrant.RightTop:
                        bubble = bubbleFrames[0];
                        gameObject.transform.parent.GetComponent<LayoutGroup>().padding.top = 50;
                        verticalOffset = -GetBubbleHeight() / 2 + VerticalDistanceToBubble;
                        horizontalOffset = -GetBubbleWidth() / 2 + HorizontalDistanceToBubble;
                        break;
                    case Quadrant.LeftTop:
                        bubble = bubbleFrames[1];
                        gameObject.transform.parent.GetComponent<LayoutGroup>().padding.top = 50;
                        verticalOffset = -GetBubbleHeight() / 2 + VerticalDistanceToBubble;
                        horizontalOffset = GetBubbleWidth() / 2 + HorizontalDistanceToBubble;
                        break;
                    case Quadrant.LeftBottom:
                        bubble = bubbleFrames[2];
                        gameObject.transform.parent.GetComponent<LayoutGroup>().padding.bottom = 50;
                        verticalOffset = GetBubbleHeight() / 2 + VerticalDistanceToBubble;
                        horizontalOffset = GetBubbleWidth() / 2 + HorizontalDistanceToBubble;
                        break;
                    case Quadrant.RightBottom:
                        bubble = bubbleFrames[3];
                        gameObject.transform.parent.GetComponent<LayoutGroup>().padding.bottom = 50;
                        verticalOffset = GetBubbleHeight() / 2 + VerticalDistanceToBubble;
                        horizontalOffset = -GetBubbleWidth() / 2 + HorizontalDistanceToBubble;
                        break;
                    default:
                        throw new InvalidGameStateException();
                }
                gameObject.transform.parent.GetComponent<UnityEngine.UI.Image>().sprite = bubble;
            }
            gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition = GetViewportPosition(figure);
        }
    }

    public void SetFigure(Figure figure)
    {
        this.figure = figure;
    }

    private Vector2 GetViewportPosition(Figure figure)
    {
        Vector2 position = GetNormalizedPosition(figure);
        float width = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.width;
        float height = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.height;
        position = new Vector2(position.x * width + horizontalOffset, position.y * height + verticalOffset);
        Debug.Log(position);
        return position;
    }

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

    private float GetBubbleWidth()
    {
        return gameObject.transform.parent.GetComponent<RectTransform>().rect.width;
    }

    private float GetBubbleHeight()
    {
        return gameObject.transform.parent.GetComponent<RectTransform>().rect.height;
    }

    private void IncreaseAspectRatio()
    {
        ChangeMinWidth(step);
    }

    private void DecreaseAspectRatio()
    {
        ChangeMinWidth(-step);
    }

    private void ChangeMinWidth(int difference)
    {
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
        layoutElement.minWidth += difference;
    }
}
