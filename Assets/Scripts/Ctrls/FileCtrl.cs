
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileCtrl {

    private static readonly string PlayerdataDirectory = "Playerdata";
    private static readonly string[] MediaDirectories = new string[] { "Images", "Music", "Texts" };

    private enum MediaType : int { Images = 0, Music = 1, Texts = 2 }

    public static bool InEditorMode
    {
        get
        {
            return Application.platform == RuntimePlatform.WindowsEditor 
                || Application.platform == RuntimePlatform.OSXEditor;
        }
    }

    private static string RootDir
    {
        get
        {
            if (InEditorMode) return Application.dataPath + "/";
            else return Application.dataPath + "/../";
        }
    }

	public static ArrayList GetPlayerList()
    {
        String[] dirs = null;
        ArrayList players = new ArrayList();
        string playerdataDir = GetPlayerdataDir();
        if(playerdataDir != null)
            dirs = Directory.GetDirectories(playerdataDir); 

        foreach (String dir in dirs)
            players.Add(new DirectoryInfo(dir).Name);
        return players;
    }

    public static FileInfo[] GetChekedImageFileInfos(string player, ArrayList validExtensions)
    {
        return GetCheckedFileInfos(player, MediaType.Images, validExtensions);
    }

    public static FileInfo[] GetCheckedMusicFileInfos(string player, ArrayList validExtensions)
    {
        return GetCheckedFileInfos(player, MediaType.Music, validExtensions);
    }

    public static FileInfo[] GetChekedTextFileInfos(string player, ArrayList validExtensions)
    {
        return GetCheckedFileInfos(player, MediaType.Texts, validExtensions);
    }

    private static FileInfo[] GetCheckedFileInfos(string player, MediaType type, ArrayList validExtensions)
    {
        string personalMediaDir = GetPersonalMediaDir(player, type);
        if(personalMediaDir != null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(personalMediaDir);
            FileInfo[] fileInfos = directoryInfo.GetFiles()
                .Where(f => IsValidFileType(f.Name, validExtensions))
                .ToArray();
            if (fileInfos.Length > 0)
                return fileInfos;   
            return null;
        }
        return null;
    }

    private static string GetPlayerdataDir()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(RootDir);
        bool containsPlayerdataDir = directoryInfo.GetDirectories()
            .Any(d => d.Name.Equals(PlayerdataDirectory));
        if (containsPlayerdataDir)
            return RootDir + PlayerdataDirectory + "/";
        return null;
    }

    private static string GetPersonalPlayerdataDir(string player)
    {
        string PlayerdataDir = GetPlayerdataDir();
        if(PlayerdataDir != null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PlayerdataDir);
            bool containsPersonalPlayerdataDir = directoryInfo.GetDirectories()
            .Any(d => d.Name.Equals(player));
            if (containsPersonalPlayerdataDir)
                return PlayerdataDir + player + "/";   
            return null;
        }
        return null;
    }

    private static string GetPersonalMediaDir(string player, MediaType type)
    {
        string personalPlayerdataDir = GetPersonalPlayerdataDir(player);
        if(personalPlayerdataDir != null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(personalPlayerdataDir);
            bool containsPersonalMediaDir = directoryInfo.GetDirectories()
            .Any(d => d.Name.Equals(MediaDirectories[(int)type]));
            if (containsPersonalMediaDir)
                return personalPlayerdataDir + MediaDirectories[(int)type] + "/";             
            return null;
        }
        return null;
    }

    private static bool IsValidFileType(string fileName, ArrayList validExtensions)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        return validExtensions.Contains(extension);
    }
}