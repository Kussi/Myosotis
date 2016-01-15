using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

/// <summary>
/// controls the personalized text object
/// </summary>
public static class TextCtrl
{
    private static string ParentObject = "TextSystem";
    private static readonly string TextPrefab = "Prefabs/TextBubble";
    private static readonly ArrayList ValidExtensions = new ArrayList { ".txt" };

    private static ArrayList texts = new ArrayList();
    private static ArrayList notYetDisplayedTexts = new ArrayList();

    private static bool isActive = false;

    /// <summary>
    /// resets the original setup
    /// </summary>
    public static void Reset()
    {
        texts = new ArrayList();
        notYetDisplayedTexts = new ArrayList();
        isActive = false;
        GameObject system = GameObject.Find(ParentObject);
        foreach (Transform child in system.transform)
        {
            GameObject.Destroy(child);
        }
    }

    /// <summary>
    /// initializes the texts if there are any
    /// </summary>
    /// <param name="playerName"></param>
    public static void InitializeTexts(string playerName)
    {
        FileInfo[] textFiles = FileCtrl.GetCheckedTextFileInfos(playerName, ValidExtensions);

        if (textFiles != null)
        {
            AddTexts(textFiles);
            isActive = true;
        }
    }

    /// <summary>
    /// fetches a text randomly and displays it
    /// </summary>
    /// <param name="figure"></param>
    public static void ShowRandomText(Figure figure)
    {
        if (isActive)
        {
            if (notYetDisplayedTexts.Count == 0)
                notYetDisplayedTexts.AddRange(texts);
            int randomIndex = UnityEngine.Random.Range(0, notYetDisplayedTexts.Count);
            string text = (string)notYetDisplayedTexts[randomIndex];
            ShowText(figure, text);
            notYetDisplayedTexts.Remove(text);
        }
    }

    /// <summary>
    /// displays a text positionned next to the figure
    /// </summary>
    /// <param name="figure"></param>
    /// <param name="text"></param>
    private static void ShowText(Figure figure, string text)
    {
        GameObject textObject = (GameObject)GameObject.Instantiate(Resources.Load(TextPrefab, typeof(GameObject)));
        textObject.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
        textObject.GetComponentInChildren<Text>().SetFigure(figure);
    }

    /// <summary>
    /// returns the position of the figure
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    private static Vector2 GetPosition(Figure figure)
    {
        Vector3 position = Camera.main.WorldToViewportPoint(figure.transform.position);
        float width = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.width;
        float height = GameObject.Find(ParentObject).GetComponent<RectTransform>().rect.height;
        position = new Vector2(position.x * width, position.y * height);
        return position;
    }

    /// <summary>
    /// adds all found texts in the list
    /// </summary>
    /// <param name="textFiles"></param>
    private static void AddTexts(FileInfo[] textFiles)
    {
        foreach (FileInfo textFile in textFiles)
        {
            StringBuilder text = new StringBuilder();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(textFile.FullName, System.Text.Encoding.Default))
                {
                    text.Append(sr.ReadToEnd());
                }
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