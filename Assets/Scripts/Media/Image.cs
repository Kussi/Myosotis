using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representation of a personalized image
/// </summary>
public class Image : MonoBehaviour {

    private GameObject imageMaskObject;

	void Awake () {
        imageMaskObject = gameObject.transform.parent.gameObject;
	}

    /// <summary>
    /// sets an image
    /// </summary>
    /// <param name="image"></param>
    public void SetImage(Texture image)
    {
        gameObject.GetComponent<RawImage>().texture = image;
    }

    /// <summary>
    /// sets a color
    /// </summary>
    /// <param name="imageMaskColor"></param>
    /// <param name="imageColor"></param>
    public void SetColor(Color imageMaskColor, Color imageColor)
    {
        imageMaskObject.GetComponent<UnityEngine.UI.Image>().color = imageMaskColor;
        gameObject.GetComponent<RawImage>().color = imageColor;
    }
}