using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField,Tooltip("Make sure the shop music is in [3] position.")] private AudioSource[] audioSources;
    [SerializeField] private float crossFadeDuration = 2f;

    private int currentTrack = 0;

    public void ChangeTrack(int trackIndex)
    {

        if (trackIndex >= 0 && trackIndex < audioSources.Length && trackIndex != currentTrack)
        {
            audioSources[trackIndex].time = audioSources[currentTrack].time;
            audioSources[trackIndex].Play();

            StartCoroutine(Crossfade(trackIndex));
        }
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeOutAndStop()
    {
        float elapsedTime = 0f;
        float startVolume = audioSources[currentTrack].volume;

        while (elapsedTime < crossFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / crossFadeDuration;

            // Fade out the current track
            audioSources[currentTrack].volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }
    

        // Stop current track
        audioSources[currentTrack].Stop();
    }
    public void PlayShopMusic()
    {
        // Assuming the shop music is assigned to audioSources[3]
        ChangeTrack(3);
    }

    private IEnumerator Crossfade(int targetTrack)
    {
        float elapsedTime = 0f;

        while (elapsedTime < crossFadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Fade out current track
            audioSources[currentTrack].volume = 1 - (elapsedTime / crossFadeDuration);

            // Fade in target track
            audioSources[targetTrack].volume = elapsedTime / crossFadeDuration;

            yield return null;
        }

        // Stop current track and update currentTrack
        audioSources[currentTrack].Stop();
        currentTrack = targetTrack;
    }
}
