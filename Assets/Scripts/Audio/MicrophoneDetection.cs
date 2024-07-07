using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneDetection : MonoBehaviour
{
    public AudioSource audioSource;
    int sampleWindow = 64;
    float currentLoudness;
    [SerializeField] private bool useMicrophone;
    public AudioClip microphoneClip;
    string microphoneName;
    public int bufferSizeMicrophone;
    public bool isPlaying;
    KeyCode toggleMicrophoneKey = KeyCode.Z;

    [Range(0f, 1f)]
    public float currentVolume;

    public float silenceThreshold = 0.01f;
    float stopDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (useMicrophone)
        {
            InitializeMicrophone();
        }

    }

    // Update is called once per frame
    void Update()
    {
        ToggleMicrophone();

        if (useMicrophone)
        {
            MicrophonePlaying();
        }
    }

    void ToggleMicrophone()
    {
        if (Input.GetKeyDown(toggleMicrophoneKey))
        {
            useMicrophone = !useMicrophone;

            if (useMicrophone)
            {
                InitializeMicrophone();
            }
            else
            {
                StopMicrophone();
            }
        }
    }

    void StopMicrophone()
    {
        Microphone.End(microphoneName);
        audioSource.Stop();
        audioSource.clip = null;
    }

    void InitializeMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            microphoneClip = Microphone.Start(microphoneName, true, bufferSizeMicrophone, AudioSettings.outputSampleRate);
            audioSource.clip = microphoneClip;
            audioSource.loop = true;

            while (!(Microphone.GetPosition(microphoneName) > 0)) { }  // Wait until the microphone starts recording

            audioSource.Play();
        }
        else
        {
            useMicrophone = false;
        }
    }

    void MicrophonePlaying() 
    {
        SetMicrophoneVolume(currentVolume);

        currentLoudness = GetLoudnessFromMicrophone();
        //audioSource.clip = microphoneClip;
        Debug.Log(currentLoudness);

        if (currentLoudness > silenceThreshold && !isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
        else if (currentLoudness <= silenceThreshold && isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
    }

    void SetMicrophoneVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void MicrophoneToAudioClip() 
    {
        microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, bufferSizeMicrophone, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {

        return GetLoudnessFromAudioClip(Microphone.GetPosition(microphoneName), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float loudness = 0;


        for (int i = 0; i < sampleWindow; i++)
        {
            loudness += Mathf.Abs(waveData[i]);
        }

        return loudness / sampleWindow ;
    }

}
