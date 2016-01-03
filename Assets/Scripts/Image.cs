using UnityEngine;
using UnityEngine.UI;

public class Image : MonoBehaviour {

    private GameObject imageMaskObject;
    private Texture image;

	void Awake () {
        image = gameObject.GetComponent<RawImage>().texture;
        imageMaskObject = gameObject.transform.parent.gameObject;
	}

    public void SetImage(Texture image)
    {
        this.image = image;
        gameObject.GetComponent<RawImage>().texture = image;
    }

    public void SetColor(Color imageMaskColor, Color imageColor)
    {
        imageMaskObject.GetComponent<UnityEngine.UI.Image>().color = imageMaskColor;
        gameObject.GetComponent<RawImage>().color = imageColor;
    }
}