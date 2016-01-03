using UnityEngine;
using UnityEngine.UI;

public class Image : MonoBehaviour {

    private GameObject imageMaskObject;

	void Awake () {
        imageMaskObject = gameObject.transform.parent.gameObject;
	}

    public void SetImage(Texture image)
    {
        gameObject.GetComponent<RawImage>().texture = image;
    }

    public void SetColor(Color imageMaskColor, Color imageColor)
    {
        imageMaskObject.GetComponent<UnityEngine.UI.Image>().color = imageMaskColor;
        gameObject.GetComponent<RawImage>().color = imageColor;
    }
}