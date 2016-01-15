using UnityEngine;

/// <summary>
/// Representation of the Settingsbutton in the middle of the
/// goal field.
/// </summary>
public class SettingsButton : MonoBehaviour {

    public SettingsMenu settingsMenu;

    /// <summary>
    /// Chekcs whether the button has been hit or not and shows the 
    /// settingsmenu
    /// </summary>
    /// <param name="hitPoint"></param>
    void OnTouchDown(Vector3 hitPoint)
    {
        settingsMenu.Show();
    }
}