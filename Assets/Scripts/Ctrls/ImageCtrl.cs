using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageCtrl
{
    private static readonly string ImageObjectSuffix = "Image";
    private static readonly string ImageDownloaderObject = "ImageSystem";

    private static readonly Dictionary<string, bool> PlayerHasWidePictureFrame = new Dictionary<string, bool>
    {
        { "Red", false }, { "Yellow", true }, { "Blue", false }, { "Green", true }
    };

    private static readonly Color ImageMaskInitialColor = new Color(1, 1, 1, 0.004f);
    private static readonly Color ImageMaskActiveColor = new Color(1, 1, 1, 0.004f);
    private static readonly Color ImageInitialColor = new Color(0, 0, 0, 0.4f);
    private static readonly Color ImageActiveColor = new Color(1, 1, 1, 1);

    private static bool isAvailable = false;
    private static bool isAligned = false;

    private static Dictionary<Player, Image> images = new Dictionary<Player, Image>();
    private static ArrayList alreadyDisplayedImages = new ArrayList();
    private static ImageDownloader imageSystem;
    private static ArrayList textures;

    private static ArrayList players;

    public static bool IsAvailable
    {
        get { return isAvailable; }
    }

    public static void InitializeImages(ArrayList players)
    {
        ImageCtrl.players = players;

        GameObject imageDownloaderObject = GameObject.Find(ImageDownloaderObject);
        imageDownloaderObject.AddComponent<ImageDownloader>();
        imageSystem = imageDownloaderObject.GetComponent<ImageDownloader>();
    }

    public static void SetupImages(ArrayList textures)
    {
        ImageCtrl.textures = textures;

        foreach (Player player in players)
        {
            string imageObjectName = player.Color + ImageObjectSuffix;
            GameObject imageObject = GameObject.Find(imageObjectName);
            imageObject.AddComponent<Image>();
            Image image = imageObject.GetComponent<Image>();
            images.Add(player, image);
            SetupInitialAppearance(player);
        }
        isAvailable = true;
        PersonalizationCtrl.Notify();
    }

    public static void SetImage(Player player, Texture newImage)
    { 
        Image image = GetPlayerImage(player);
        image.SetImage(newImage);
        SetColor(player, ImageMaskActiveColor, ImageActiveColor);
        if (!isAligned) RotateImage(player, image.gameObject);
        FitImageIntoFrame(player, image.gameObject);
    }

    private static void SetupInitialAppearance(Player player)
    {
        SetColor(player, ImageMaskInitialColor, ImageInitialColor);
    }

    private static void SetColor(Player player, Color imageMaskColor, Color imageColor)
    {
        Image image = GetPlayerImage(player);
        image.SetColor(imageMaskColor, imageColor);
    }

    private static Image GetPlayerImage(Player player)
    {
        Image image;
        images.TryGetValue(player, out image);
        if (image == null) throw new InvalidGameStateException();
        return image;
    }

    private static void FitImageIntoFrame(Player player, GameObject image)
    {
        Rect mask = image.transform.parent.GetComponent<RectTransform>().rect;
        float maskWidth = mask.width;
        float maskHeight = mask.height;
        Texture picture = image.GetComponent<RawImage>().texture;
        float pictureWidth = picture.width;
        float pictureHeight = picture.height;
        float pictureRatio = pictureWidth / pictureHeight;
        float newPictureWidth, newPictureHeight;

        if(HasWidePictureFrame(player))
        {
            float temp = maskWidth;
            maskWidth = maskHeight;
            maskHeight = temp;
        }

        if (pictureRatio > 1)
        {
            newPictureWidth = maskWidth;
            newPictureHeight = newPictureWidth / pictureRatio;
        }
        else
        {
            newPictureHeight = maskHeight;
            newPictureWidth = newPictureHeight * pictureRatio;
        }
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(newPictureWidth, newPictureHeight);
    }

    private static void RotateImage(Player player, GameObject image)
    {
        int rotationAngle = player.PlayerAngle;
        Vector3 rotation = image.GetComponent<RectTransform>().eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, rotationAngle);
        image.GetComponent<RectTransform>().eulerAngles = rotation;
    }

    private static bool HasWidePictureFrame(Player player)
    {
        bool result;
        PlayerHasWidePictureFrame.TryGetValue(player.Color, out result);
        return result;
    }

    public static void ChangeImageRandomly(Player player)
    {
        if (IsAvailable)
        {
            if (alreadyDisplayedImages.Count == 0)
                alreadyDisplayedImages.AddRange(textures);
            int randomIndex = Random.Range(0, alreadyDisplayedImages.Count);
            Texture image = (Texture)alreadyDisplayedImages[randomIndex];
            SetImage(player, image);
            alreadyDisplayedImages.Remove(image);
        }
    }

    public static void ChangeAllImages()
    {
        if (alreadyDisplayedImages.Count == 0)
            alreadyDisplayedImages.AddRange(textures);
        int randomIndex = Random.Range(0, alreadyDisplayedImages.Count);
        Texture image = (Texture)alreadyDisplayedImages[randomIndex];
        foreach(Player player in PlayerCtrl.players)
            SetImage(player, image);
        alreadyDisplayedImages.Remove(image);
    }
}