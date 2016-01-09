using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text : MonoBehaviour {

    public float minAspectRatio;
    public float maxAspectRatio;
    public int step;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float aspectRatio = GetMinWidth() / GetPreferredHeight();

        if (aspectRatio < minAspectRatio)
            IncreaseAspectRatio();
        else if (aspectRatio > maxAspectRatio)
            DecreaseAspectRatio();
    }

    private float GetMinWidth()
    {
        return gameObject.GetComponent<LayoutElement>().minWidth;
    }

    private float GetPreferredHeight()
    {
        return LayoutUtility.GetPreferredHeight(gameObject.GetComponent<RectTransform>());
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
