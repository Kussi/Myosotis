using System;
using UnityEngine;
using UnityEngine.UI;

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

    // Use this for initialization
    void Awake()
    {
        ChangePlayerButtonColors(isSelected);
        ShowChairSelection(isSelected);
    }

    public void SelectPlayer()
    {
        isSelected = !isSelected;
        ChangePlayerButtonColors(isSelected);
        ChangePlayerIcon(isSelected);
        ShowChairSelection(isSelected);
    }

    public void SelectChair(int number)
    {
        for (int i = 0; i < playerChairs.Length; ++i)
        {
            ChairButton chair = playerChairs[i];
            if (i == number) chair.SetSelected(true);
            else chair.SetSelected(false);
        }

    }

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

    private void ChangePlayerButtonColors(bool isSelected)
    {
        ColorBlock result = buttonObject.colors;
        if (isSelected) result.normalColor = result.highlightedColor
                = result.pressedColor = selectedColor;
        else result.normalColor = result.highlightedColor
                = result.pressedColor = deselectedColor;
        buttonObject.colors = result;
    }

    private void ChangePlayerIcon(bool isSelected)
    {
        if (isSelected) icon.sprite = selectedIcon;
        else icon.sprite = deselectedIcon;
    }

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