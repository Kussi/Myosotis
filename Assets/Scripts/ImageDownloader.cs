using UnityEngine;
using System.Collections;
using System.IO;

public class ImageDownloader : MonoBehaviour {

    public static ArrayList textures = new ArrayList();
    private static readonly string FileIndicator = "file://";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".png", ".jpg", ".bmp", ".tif", ".tga", ".psd", ".jpeg" };

    IEnumerator Start()
    {
        string playerName = PersonalizationCtrl.PlayerName;
        FileInfo[] imageFiles = FileCtrl.GetChekedImageFileInfos(playerName, ValidExtensions);

        if(imageFiles != null)
        {
            foreach(FileInfo imageFile in imageFiles)
            {
                string path = imageFile.FullName;
                WWW www = new WWW(FileIndicator + path);
                yield return www;
                // loading
                // loading done
                Texture texture = www.texture;

                texture.name = Path.GetFileName(path);
                textures.Add(texture);
            }
        }
        ImageCtrl.SetupImages();

        //foreach (Texture t in textures)
        //    Debug.Log(t.name);
    }

    //public void SetupImages(FileInfo[] imageFiles)
    //{
    //    LoadImages(imageFiles);
    //    foreach (Texture t in textures)
    //        Debug.Log(t.name);
    //}

    //private void LoadImages(FileInfo[] imageFiles)
    //{
    //    textures.Clear();
    //    if (imageFiles != null)
    //    {
    //        foreach (FileInfo imageFile in imageFiles)
    //            //LoadFile(imageFile.FullName);
    //         StartCoroutine(LoadFile(imageFile.FullName));
    //    }
    //}

    //private IEnumerator LoadFile(string path)
    //{
        
    //}

}
