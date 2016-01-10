using UnityEngine;
using System.Collections;
using System.IO;

public class ImageDownloader : MonoBehaviour {

    public static Texture startImage;

    private static readonly string FileIndicator = "file://";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".png", ".jpg", ".bmp", ".tif", ".tga", ".psd", ".jpeg" };

    IEnumerator Start()
    {
        string playerName = PersonalizationCtrl.PlayerName;
        ArrayList textures = null;
        FileInfo[] imageFiles = FileCtrl.GetCheckedImageFileInfos(playerName, ValidExtensions); 

        if (imageFiles != null)
        {
            textures = new ArrayList();
            foreach (FileInfo imageFile in imageFiles)
            {
                string path = imageFile.FullName;
                WWW www = new WWW(FileIndicator + path);
                yield return www;

                Texture texture = www.texture;
                texture.name = Path.GetFileName(path);
                textures.Add(texture);
            }
            ImageCtrl.SetAvailable(textures);
        }
        else PersonalizationCtrl.Notify(typeof(ImageCtrl)); 
    }
}