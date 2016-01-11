using UnityEngine;

public class SettingsButton : MonoBehaviour {

    public SettingsMenu settingsMenu;

    void OnTouchDown(Vector3 hitPoint)
    {
        Debug.LogWarning("hit=)");
        settingsMenu.Show();
    }
}