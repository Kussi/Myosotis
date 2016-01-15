using UnityEngine;
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

    /// <summary>
    /// Resets the original setup
    /// </summary>
    public static void Reset()
    {
        isAvailable = false;
        foreach (Player player in images.Keys)
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

    /// <summary>
    /// starts the image download via imagedownloader class
    /// </summary>
    /// <param name="players"></param>
    public static void InitializeImages(ArrayList players)
    {
        ImageCtrl.players = players;

        SetupImages();
        GameObject imageDownloaderObject = GameObject.Find(ImageDownloaderObject);
        imageDownloaderObject.AddComponent<ImageDownloader>();
    }

    /// <summary>
    /// sets up all image objects and fills them with the start image
    /// </summary>
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

    /// <summary>
    /// sets the personalized images as available, which enables the event fields
    /// </summary>
    /// <param name="textures"></param>
    public static void SetAvailable(ArrayList textures)
    {
        ImageCtrl.textures = textures;
        isAvailable = true;
        PersonalizationCtrl.Notify(typeof(ImageCtrl));
    }

    /// <summary>
    /// Sets an image of a player
    /// </summary>
    /// <param name="player"></param>
    /// <param name="newImage"></param>
    public static void SetImage(Player player, Texture newImage)
    {
        Image image = GetPlayerImage(player);
        image.SetImage(newImage);
        RotateImage(player, image.gameObject);
        FitImageIntoFrame(player, image.gameObject);
    }

    /// <summary>
    /// removes the image of a player
    /// </summary>
    /// <param name="player"></param>
    private static void RemoveImage(Player player)
    {
        Image image = GetPlayerImage(player);
        image.SetImage(null);
    }

    /// <summary>
    /// sets the players image to the start image
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// returns the players image
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private static Image GetPlayerImage(Player player)
    {
        Image image;
        images.TryGetValue(player, out image);
        if (image == null) throw new InvalidGameStateException();
        return image;
    }

    /// <summary>
    /// changes the pictures size to fit into the frame
    /// </summary>
    /// <param name="player"></param>
    /// <param name="image"></param>
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

        if (IsWidePictureFrame(player))
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

    /// <summary>
    /// rotates the image acoording to the players angle
    /// </summary>
    /// <param name="player"></param>
    /// <param name="image"></param>
    private static void RotateImage(Player player, GameObject image)
    {
        int rotationAngle = player.PlayerAngle;
        Vector3 rotation = image.GetComponent<RectTransform>().eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, rotationAngle);
        image.GetComponent<RectTransform>().eulerAngles = rotation;
    }

    /// <summary>
    /// checks whether the picture is wide (width > height) or not
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private static bool IsWidePictureFrame(Player player)
    {
        return player.PlayerAngle % 180 != 0;
    }

    /// <summary>
    /// changes the image of a player to a random one of the personalized directory
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// changes the images of all players randomly
    /// </summary>
    public static void ChangeAllImages()
    {
        if (notYetDisplayedImages.Count == 0)
            notYetDisplayedImages.AddRange(textures);
        foreach (Player player in PlayerCtrl.players)
            ChangeImageRandomly(player);
    }
}