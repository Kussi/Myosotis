using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Music : MonoBehaviour
{
    public Sprite pauseIcon;
    public Sprite playIcon;

    private static readonly string FileIndicator = "file://";

    public enum ChangeDirection { Next, Previous }
    private AudioSource source;
    private List<AudioClip> clips = new List<AudioClip>();
    private bool isPlaying = false;
    private bool isPaused = false;

    private int index = 0;

    public void Reset()
    {
        GameObject.Destroy(source.gameObject.GetComponent<AudioSource>());
        source = gameObject.AddComponent<AudioSource>();
        clips = new List<AudioClip>();
        isPlaying = false;
        isPaused = false;
    }

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlaying)
        {
            if (!source.isPlaying)
            {
                Next();
            }
        }
    }

    public void SetupMusic(FileInfo[] musicFiles)
    {
        LoadMusic(musicFiles);
    }

    public void PlayPause()
    {
        if (!isPlaying) Play();
        else Pause();
    }

    private void Pause()
    {
        AudioListener.pause = true;
        isPlaying = false;
        isPaused = true;
    }

    private void Play()
    {
        if (source.clip == null)
            source.clip = clips[index];
        AudioListener.pause = false;
        if (isPaused) isPaused = false;
        else source.Play();
        isPlaying = true;
    }

    public Sprite GetPlayPauseIcon()
    {
        if (isPaused) return playIcon;
        return pauseIcon;
    }

    public void Previous()
    {
        ChangeClip(ChangeDirection.Previous);
        Play();
    }

    public void Next()
    {
        ChangeClip(ChangeDirection.Next);
        Play();
    }

    private void ChangeClip(ChangeDirection direction)
    {
        if (direction == ChangeDirection.Next)
        {
            index = (index + 1) % clips.Count;
        }
        else if (--index < 0)
            index = clips.Count - 1;

        if (source.isPlaying)
        {
            source.clip = clips[index];
            source.Play();
        }
        else source.clip = clips[index];
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void LoadMusic(FileInfo[] musicFiles)
    {
        clips.Clear();
        if (musicFiles != null)
        {
            foreach (FileInfo musicFile in musicFiles)
                StartCoroutine(LoadFile(musicFile.FullName));
        }
    }

    private IEnumerator LoadFile(string path)
    {
        WWW www = new WWW(FileIndicator + path);
        // loading

        AudioClip clip = www.GetAudioClip(false);
        while (clip.loadState == AudioDataLoadState.Unloaded)
            yield return www;

        // loading done
        clip.name = Path.GetFileName(path);
        clips.Add(clip);

        if (!isPlaying)
            Play();
        PersonalizationCtrl.Notify(typeof(MusicCtrl));
    }

    public string GetTrackName()
    {
        return clips[index].name;
    }
}