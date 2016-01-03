using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ImageCtrl
{
    
    private static readonly string ImageObjectSuffix = "Image";
    private static readonly string ImageSystemObjectName = "ImageSystem";

    private static readonly Color ImageMaskInitialColor = new Color(1, 1, 1, 0.004f);
    private static readonly Color ImageMaskActiveColor = new Color(1, 1, 1, 1);
    private static readonly Color ImageInitialColor = new Color(0, 0, 0, 0.4f);
    private static readonly Color ImageActiveColor = new Color(1, 1, 1, 1);

    private static bool isAvailable = false;

    private static Dictionary<Player, Image> images = new Dictionary<Player, Image>();
    private static ImageDownloader imageSystem;

    private static ArrayList players;

    public static bool IsAvailable
    {
        get { return isAvailable; }
    }

    public static void InitializeImages(ArrayList players)
    {
        ImageCtrl.players = players;

        GameObject imageSystemObject = GameObject.Find(ImageSystemObjectName);
        imageSystemObject.AddComponent<ImageDownloader>();
        imageSystem = imageSystemObject.GetComponent<ImageDownloader>(); 
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
            //image.SetupImage(....);
        }
        isAvailable = true;
        SetImage((Player)players[0], (Texture)ImageDownloader.textures[0]);
        SetImage((Player)players[1], (Texture)ImageDownloader.textures[1]);
    }

    private static void SetupInitialAppearance(Player player)
    {
        SetColor(player, ImageMaskInitialColor, ImageInitialColor);
    }

    public static void SetImage(Player player, Texture newImage)
    {
        Image image = GetPlayerImage(player);
        image.SetImage(newImage);
        SetColor(player, ImageMaskActiveColor, ImageActiveColor);
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
}