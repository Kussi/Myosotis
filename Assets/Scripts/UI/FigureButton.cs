using UnityEngine;

/// <summary>
/// Representation of the figureselectionbuttons on the startmenu
/// </summary>
public class FigureButton : MonoBehaviour
{
    public Sprite selectedIcon;
    public Sprite deselectedIcon;

    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    /// <summary>
    /// turns colored if its selected and gray if not. Only one button can be colored
    /// at the time
    /// </summary>
    /// <param name="value"></param>
    public void SetSelected(bool value)
    {
        isSelected = value;

        if (isSelected)
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedIcon;
        else
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = deselectedIcon;
    }
}