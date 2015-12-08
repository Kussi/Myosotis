using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    private const float RotationSpeed = 0.5F;
    private Vector3 currentAngle;
    private Vector3 newAngle;

    public float Angle
    {
        get { return currentAngle.y; }
        set
        {
            // according to calculation inaccuracies of Unity a range has to be catched
            while ((newAngle.y % 360 + 360) % 360 < value - 1 || (newAngle.y % 360 + 360) % 360 > value + 1) newAngle.y -= 90;
        }
    }

    void Start()
    {
        currentAngle = gameObject.transform.eulerAngles;
        newAngle = currentAngle;
    }

    void Update()
    {
        currentAngle = Vector3.Lerp(currentAngle, newAngle, Time.deltaTime * RotationSpeed);
        transform.eulerAngles = currentAngle;
    }
}