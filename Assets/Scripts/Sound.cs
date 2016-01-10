using UnityEngine;

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

    public void PlayApplause()
    {
        source.PlayOneShot(applause, 0.7F);
    }

    public void PlayStep()
    {
        source.PlayOneShot(step, 0.7F);
    }

    public void PlayDice()
    {
        source.PlayOneShot(dice, 0.4F);
    }
}
