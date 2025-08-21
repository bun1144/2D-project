using UnityEngine;
using UnityEngine.Audio;

public class AudioDebug : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        if (mixer == null) { Debug.LogError("Mixer is NULL"); return; }

        float db;
        if (mixer.GetFloat("MusicVol", out db))
            Debug.Log($"MusicVol exists. Current dB = {db}");
        else
            Debug.LogError("MusicVol parameter NOT found. Check Expose name!");

        if (mixer.GetFloat("SFXVol", out db))
            Debug.Log($"SFXVol exists. Current dB = {db}");
        else
            Debug.LogWarning("SFXVol parameter NOT found (skip if you don't use).");
    }
}
