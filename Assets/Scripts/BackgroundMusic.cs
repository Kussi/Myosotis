using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class BackgroundMusic : MonoBehaviour {

    AudioSource audioSource;
    private ArrayList musicFiles;

    IEnumerator Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        musicFiles = new ArrayList();

        //String[] files = Directory.GetFiles(GetMusicDirectoryURL());
        //foreach (String file in files)
        //{
        //    if(Path.GetExtension(file).Equals(".ogg"))
        //    {
        //        musicFiles.Add(file);
        //    }
        //}

        foreach(String file in musicFiles)
        {
            WWW musicFile = new WWW("file:///" + file);
            yield return musicFile;

            AudioClip musicClip = musicFile.GetAudioClip(true);

            audioSource.clip = (AudioClip)musicFile.audioClip;
            audioSource.Play();

            yield return new WaitForSeconds(musicClip.length);
        }
    }
	
    //private static string GetMusicDirectoryURL()
    //{
    //    if (Application.platform == RuntimePlatform.WindowsEditor)
    //    {
    //        return Application.dataPath + "/Resources/Music/";
    //    }
    //    else // (Application.platform == RuntimePlatform.WindowsPlayer)
    //    {
    //        return Application.dataPath + "/../Music/";
    //    }
    //}
}