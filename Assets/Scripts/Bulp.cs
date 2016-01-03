using UnityEngine;
using System.Collections;

public class Bulp : MonoBehaviour
{
    private readonly float lerpSpeed = 0.01f;

    private Color lerpedColor = Color.white;
    private Color color;

    private bool isActive = false;

    private float actualTime = 0;

    void Update()
    {
        
        if(isActive && actualTime < 1)
        {
            actualTime += lerpSpeed;
            lerpedColor = Color.Lerp(color, Color.white, actualTime);
            gameObject.GetComponent<MeshRenderer>().material.color = lerpedColor;
        }
        else if(!isActive && actualTime > 0)
        {
            actualTime -= lerpSpeed;
            lerpedColor = Color.Lerp(color, Color.white, actualTime);
            gameObject.GetComponent<MeshRenderer>().material.color = lerpedColor;
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        gameObject.GetComponent<MeshRenderer>().material.color = this.color;
    }
}