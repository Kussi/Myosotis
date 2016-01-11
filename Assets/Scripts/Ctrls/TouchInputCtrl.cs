using UnityEngine;
using System.Collections;

public static class TouchInputCtrl {

	private static Camera camera = Camera.main;

    public static bool isActive()
    {
        return camera.GetComponent<TouchInput>().IsActive;
    }

    public static void Activate()
    {
        SetActive(true);
    }

    public static void Deactivate()
    {
        SetActive(false);
    }

    private static void SetActive(bool value)
    {
        camera.GetComponent<TouchInput>().SetActive(value);
    }
}
