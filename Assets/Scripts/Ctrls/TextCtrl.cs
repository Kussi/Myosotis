using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public static class TextCtrl
{
    private static readonly string TextPrefab = "Prefabs/Text";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".txt"/*, ".doc", ".docx", ".odt"*/ };

    private static ArrayList texts = new ArrayList();

    public static void InitializeTexts(string playerName)
    {
        FileInfo[] textFiles = FileCtrl.GetCheckedTextFileInfos(playerName, ValidExtensions);

        if (textFiles != null)
        {
            AddTexts(textFiles);
        }
    }

    public static void ShowText(Figure figure, string text)
    {
        GameObject textObject = (GameObject)GameObject.Instantiate(Resources.Load(TextPrefab, typeof(GameObject)));
    }

    private static void AddTexts(FileInfo[] textFiles)
    {
        foreach(FileInfo textFile in textFiles)
        {
            StringBuilder text = new StringBuilder();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(textFile.FullName))
                {
                    // Read the stream to a string, and write the string to the console.
                   text.Append(sr.ReadToEnd());
                }
                Debug.Log(text.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            texts.Add(text.ToString());
        }
    }
}