using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageCtrl
{
    private static readonly string WelcomeImage = "Images/Welcome";
    private static readonly string ImageObjectSuffix = "Image";
    private static readonly string ImageDownloaderObject = "ImageSystem";

    private static bool isAvailable = false;

    private static Dictionary<Player, Image> images = new Dictionary<Player, Image>();
    private static ArrayList notYetDisplayedImages = new ArrayList();
    private static ArrayList textures;

    private static ArrayList players;

    public static bool IsAvailable
    {
        get { return isAvailable; }
    }

    public static void Reset()
    {
        isAvailable = false;
        foreach(Player player in images.Keys)
        {
            RawImage playerImage = GetPlayerImage(player).GetComponent<RawImage>();
            Color color = playerImage.color;
            playerImage.color = new Color(color.r, color.g, color.b, 0);
            RemoveImage(player);
        }

        foreach (Image image in images.Values)
        {
            GameObject.Destroy(image.gameObject.GetComponent<Image>());
        }
        images = new Dictionary<Player, Image>();
        notYetDisplayedImages = new ArrayList();
        textures = null;
    }

    public static void InitializeImages(ArrayList players)
    {
        ImageCtrl.players = players;

        SetupImages();
        GameObject imageDownloaderObject = GameObject.Find(ImageDownloaderObject);
        imageDownloaderObject.AddComponent<ImageDownloader>();
    }

    public static void SetupImages()
    {
        foreach (Player player in players)
        {
            string imageObjectName = player.Color + ImageObjectSuffix;
            GameObject imageObject = GameObject.Find(imageObjectName);
            imageObject.AddComponent<Image>();
            Image image = imageObject.GetComponent<Image>();
            images.Add(player, image);
            SetupInitialAppearance(player);
        }
    }

    public static void SetAvailable(ArrayList textures)
    {
        ImageCtrl.textures = textures;
        isAvailable = true;
        PersonalizationCtrl.Notify(typeof(ImageCtrl));
    }

    public static void SetImage(Player player, Texture newImage)
    { 
        Image image = GetPlayerImage(player);
        image.SetImage(newImage);
        RotateImage(player, image.gameObject);
        FitImageIntoFrame(player, image.gameObject);
    }

    private static void RemoveImage(Player player)
    {
        Image image = GetPlayerImage(player);
        image.SetImage(null);
    }

    private static void SetupInitialAppearance(Player player)
    {
        Texture image = Resources.Load(WelcomeImage) as Texture;
        if (image != null)
        {
            RawImage playerImage = GetPlayerImage(player).GetComponent<RawImage>();
            Color color = playerImage.color;
            playerImage.color = new Color(color.r, color.g, color.b, 255);
            SetImage(player, image);
        }
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

        if(IsWidePictureFrame(player))
        {
            float temp = maskWidth;
            maskWidth = maskHeight;
            maskHeight = temp;
        }

        if (pictureRatio > 1)
        {
            newPictureWidth = maskWidth;
            newPictureHeight = newPictureWidth / pictureRatio;
            if (newPictureHeight > maskHeight)
            {
                newPictureHeight = maskHeight;
                newPictureWidth = newPictureHeight * pictureRatio;
            }
        }
        else
        {
            newPictureHeight = maskHeight;
            newPictureWidth = newPictureHeight * pictureRatio;
            if (newPictureWidth > maskWidth)
            {
                newPictureWidth = maskWidth;
                newPictureHeight = newPictureWidth / pictureRatio;
            }
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

    private static bool IsWidePictureFrame(Player player)
    {
        return player.PlayerAngle % 180 != 0;
    }

    public static void ChangeImageRandomly(Player player)
    {
        if (IsAvailable)
        {
            if (notYetDisplayedImages.Count == 0)
                notYetDisplayedImages.AddRange(textures);
            int randomIndex = Random.Range(0, notYetDisplayedImages.Count);
            Texture image = (Texture)notYetDisplayedImages[randomIndex];
            SetImage(player, image);
            notYetDisplayedImages.Remove(image);
        }
    }

    public static void ChangeAllImages()
    {
        if (notYetDisplayedImages.Count == 0)
            notYetDisplayedImages.AddRange(textures);
        int randomIndex = Random.Range(0, notYetDisplayedImages.Count);
        Texture image = (Texture)notYetDisplayedImages[randomIndex];
        foreach(Player player in PlayerCtrl.players)
            SetImage(player, image);
        notYetDisplayedImages.Remove(image);
    }
}