using UnityEngine;

/// <summary>
/// responsible for sounds
/// </summary>
public class Sound : MonoBehaviour
{
    public AudioClip applause;
    public AudioClip step;
    public AudioClip dice;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
    }

    /// <summary>
    /// Plays an applause sound
    /// </summary>
    public void PlayApplause()
    {
        source.PlayOneShot(applause, 0.7F);
    }

    /// <summary>
    /// Plays an step sound
    /// </summary>
    public void PlayStep()
    {
        source.PlayOneShot(step, 1);
    }

    /// <summary>
    /// Plays an dice sound
    /// </summary>
    public void PlayDice()
    {
        source.PlayOneShot(dice, 0.4F);
    }
}
