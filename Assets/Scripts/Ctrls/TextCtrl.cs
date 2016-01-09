using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public static class TextCtrl
{
    private static string ParentObject = "TextSystem";
    private static readonly string TextPrefab = "Prefabs/TextBubble";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".txt" };

    private static ArrayList texts = new ArrayList();
    private static ArrayList notYetDisplayedTexts = new ArrayList();

    private static bool isActive = false;

    public static void InitializeTexts(string playerName)
    {
        FileInfo[] textFiles = FileCtrl.GetCheckedTextFileInfos(playerName, ValidExtensions);

        if (textFiles != null)
        {
            AddTexts(textFiles);
            isActive = true;
        }
    }

    public static void ShowRandomText(Figure figure)
    {
        if(isActive)
        {
            if (notYetDisplayedTexts.Count == 0)
                notYetDisplayedTexts.AddRange(texts);
            int randomIndex = UnityEngine.Random.Range(0, notYetDisplayedTexts.Count);
            string text = (string)notYetDisplayedTexts[randomIndex];
            ShowText(figure, text);
            notYetDisplayedTexts.Remove(text);
        }
    }

    private static void ShowText(Figure figure, string text)
    {
        GameObject textObject = (GameObject)GameObject.Instantiate(Resources.Load(TextPrefab, typeof(GameObject)));
        textObject.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
        textObject.GetComponentInChildren<Text>().SetFigure(figure);
    }

    private static Vector2 GetPosition(Figure figure)
    {
        Vector3 position = Camera.main.WorldToViewportPoint(figure.transform.position);
        float width = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.width;
        float height = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.height;
        position = new Vector2(position.x * width, position.y * height);
        return position;
    }

    private static void AddTexts(FileInfo[] textFiles)
    {
        foreach (FileInfo textFile in textFiles)
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