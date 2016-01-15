using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of the player on the startmenu
/// </summary>
public class PlayerButton : MonoBehaviour
{
    public Button buttonObject;
    public ChairButton[] playerChairs;
    public Color32 selectedColor;
    public Color32 deselectedColor;
    public UnityEngine.UI.Image icon;
    public Sprite selectedIcon;
    public Sprite deselectedIcon;
    public string color;

    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Awake()
    {
        ChangePlayerButtonColors(isSelected);
        ShowChairSelection(isSelected);
    }

    /// <summary>
    /// sets the button selected or not and therefore displays the chairpositions or not
    /// </summary>
    /// <param name="value"></param>
    public void SelectPlayer()
    {
        isSelected = !isSelected;
        ChangePlayerButtonColors(isSelected);
        ChangePlayerIcon(isSelected);
        ShowChairSelection(isSelected);
    }

    /// <summary>
    /// selects one of the chairbuttons
    /// </summary>
    /// <param name="number"></param>
    public void SelectChair(int number)
    {
        for (int i = 0; i < playerChairs.Length; ++i)
        {
            ChairButton chair = playerChairs[i];
            if (i == number) chair.SetSelected(true);
            else chair.SetSelected(false);
        }

    }

    /// <summary>
    /// makes the chairbuttons visible or not
    /// </summary>
    /// <param name="isSelected"></param>
    private void ShowChairSelection(bool isSelected)
    {
        for (int i = 0; i < playerChairs.Length; ++i)
        {
            ChairButton chair = playerChairs[i];
            chair.Show(isSelected);
            if (isSelected)
            {
                if (i == 0) chair.SetSelected(true);
                else chair.SetSelected(false);
            }
        }
    }

    /// <summary>
    /// changes the color of the button in case of selection/deselction
    /// </summary>
    /// <param name="isSelected"></param>
    private void ChangePlayerButtonColors(bool isSelected)
    {
        ColorBlock result = buttonObject.colors;
        if (isSelected) result.normalColor = result.highlightedColor
                = result.pressedColor = selectedColor;
        else result.normalColor = result.highlightedColor
                = result.pressedColor = deselectedColor;
        buttonObject.colors = result;
    }

    /// <summary>
    /// changes the icon of the button in case of selection/deselction
    /// </summary>
    /// <param name="isSelected"></param>
    private void ChangePlayerIcon(bool isSelected)
    {
        if (isSelected) icon.sprite = selectedIcon;
        else icon.sprite = deselectedIcon;
    }

    /// <summary>
    /// Checks which chairbutton is selected and returns it
    /// </summary>
    /// <returns></returns>
    public int GetSelectedChair()
    {
        int result;
        foreach (ChairButton button in playerChairs)
        {
            if (button.IsSelected)
            {
                if(!Int32.TryParse(button.name.Substring(button.name.Length - 1, 1), out result))
                    throw new InvalidGameStateException();
                return result;
            }
        }
        throw new InvalidGameStateException();
    }
}