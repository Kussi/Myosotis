
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileCtrl {

    /// <summary>
    /// Checks whether the current RuntimePlatform is a windows/osx editor or not.
    /// </summary>
    private static bool InEditorMode
    {
        get
        {
            return Application.platform == RuntimePlatform.WindowsEditor 
                || Application.platform == RuntimePlatform.OSXEditor;
        }
    }

    /// <summary>
    /// Returns the Playerdata directory depending on the RuntimePlatform
    /// </summary>
    private static string PlayerdataDir
    {
        get
        {
            if (InEditorMode) return Application.dataPath + "/Playerdata/";
            else return Application.dataPath + "/../Playerdata/";
        }
    }

    /// <summary>
    /// Gets all names of player directories, that are valid
    /// </summary>
    /// <returns>player names with valid directories</returns>
	public static ArrayList GetPlayerList()
    {
        ArrayList players = new ArrayList();
        String[] dirs = Directory.GetDirectories(PlayerdataDir);

        foreach (String dir in dirs)
        {
            if (CheckPlayerDir(dir)) players.Add(new DirectoryInfo(dir).Name);
        }

        return players;
    }

    public static ArrayList GetImagesFromPlayer(string player)
    {
        // TODO
        return null;
    }

    public static ArrayList GetMusicFromPlayer(string player)
    {
        // TODO
        return null;
    }

    public static ArrayList GetTextsFromPlayer(string player)
    {
        // TODO
        return null;
    }

    /// <summary>
    /// checks the directory and file structure of the players data directory.
    /// </summary>
    /// <param name="playerDirURL">URL of the players data directory</param>
    /// <returns>true if structure is valid. False if not.</returns>
    private static bool CheckPlayerDir(string playerDirURL)
    {
        IEnumerable<string> dirs = Directory.GetDirectories(playerDirURL)
            .Select(d => new DirectoryInfo(d).Name);

        if (Enumerable.Contains<string>(dirs, "Images")
            && Enumerable.Contains<string>(dirs, "Music")
            && Enumerable.Contains<string>(dirs, "Texts"))
            return true;
        
        return false;
    }
}