using UnityEngine;
using System.Collections;

public class FigureButton : MonoBehaviour {

    public Sprite selectedIcon;
    public Sprite deselectedIcon;

    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (isSelected)
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedIcon;
        else
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = deselectedIcon;
    }
}