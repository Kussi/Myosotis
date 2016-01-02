using UnityEngine;
using System.IO;
using System.Collections;

public class ImageCtrl {

    private static readonly string FileIndicator = "file://";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".png", ".jpg", ".bmp", ".tif", ".tga", ".psd", ".jpeg" };

    public static void InitializeImages(string player)
    {
        FileInfo[] imageFiles = FileCtrl.GetChekedImageFileInfos(player, ValidExtensions);
        if (imageFiles != null)
        {

        }
    }

    //private IEnumerator LoadFile(string path)
    //{
        //WWW www = new WWW(FileIndicator + path);
        //// loading

        //Texture texture = www.texture;
        //while (clip.loadState == AudioDataLoadState.Unloaded)
        //    yield return www;

        //// loading done
        //clip.name = Path.GetFileName(path);
        //clips.Add(clip);
    //}
}