using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents possible player chair positions on the startmenu
/// </summary>
public class ChairButton : MonoBehaviour {

    public Button buttonObject;
    public Color32 visibleColor;
    public Color32 invisibleColor;
    public Sprite selectedIcon;
    public Sprite deselectedIcon;

    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    /// <summary>
    /// sets a button selected and deselects the other one of the same player and the
    /// other way round
    /// </summary>
    /// <param name="value"></param>
    public void SetSelected(bool value)
    {
        isSelected = value;

        if(isSelected)
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedIcon;
        else
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = deselectedIcon;
    }

    /// <summary>
    /// makes the chairbuttons visible or hides them
    /// </summary>
    /// <param name="isActive"></param>
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