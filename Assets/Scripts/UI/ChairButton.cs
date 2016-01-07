using UnityEngine;
using UnityEngine.UI;

public class ChairButton : MonoBehaviour {

    public Button buttonObject;
    public Color32 visibleColor;
    public Color32 invisibleColor;
    public Sprite selectedIcon;
    public Sprite deselectedIcon;

    private bool isActive = false;
    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if(isSelected)
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedIcon;
        else
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = deselectedIcon;
    }

    public void Show(bool isActive)
    {
        ColorBlock result = buttonObject.colors;
        if (isActive) result.normalColor = result.highlightedColor
                = result.pressedColor = visibleColor;
        else result.normalColor = result.highlightedColor
                = result.pressedColor = invisibleColor;
        buttonObject.colors = result;
    }
}