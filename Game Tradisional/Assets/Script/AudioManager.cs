using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Singleton;

    [Header("Reference")]
    [SerializeField] AudioSource[] audioSourceList;
    [SerializeField] AudioClip[] clipList;

    bool volumeUpdate;

    int indexSourceUpdate;
    float volumeSpeed = 0.3f;
    float volumeSet;
    float[] originalVolume;

    void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        originalVolume = new float[audioSourceList.Length];
        int n = 0;
        foreach (AudioSource item in audioSourceList)
        {
            originalVolume[n] = item.volume;
            n++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!volumeUpdate)
            return;

        if (audioSourceList[indexSourceUpdate].volume >= originalVolume[indexSourceUpdate] && volumeSet > 0)
        {
            audioSourceList[indexSourceUpdate].volume = originalVolume[indexSourceUpdate];
            volumeUpdate = false;
            return;
        }

        if (audioSourceList[indexSourceUpdate].volume <= 0 && volumeSet < 0)
        {
            audioSourceList[indexSourceUpdate].volume = 0;
            volumeUpdate = false;
            return;
        }

        audioSourceList[indexSourceUpdate].volume += volumeSpeed * volumeSet * Time.deltaTime;
    }

    public void SetClipFromList(int _clipIndex, int _indexSource, bool _volumeUp, bool _loop)
    {
        if (volumeUpdate)
            return;

        audioSourceList[_indexSource].clip = clipList[_clipIndex];
        audioSourceList[_indexSource].Play();
        audioSourceList[_indexSource].loop = _loop;

        if (_volumeUp)
        {
            volumeSet = 1;
            audioSourceList[_indexSource].volume = 0;
        }
            
        else { 
            volumeSet = -1;
            audioSourceList[_indexSource].volume = originalVolume[_indexSource];
        }

        indexSourceUpdate = _indexSource;

        volumeUpdate = true;
    }

    public void SetClip(AudioClip _clip, int _indexSource, bool _volumeUp, bool _loop)
    {
        if (volumeUpdate)
            return;

        audioSourceList[_indexSource].clip = _clip;
        audioSourceList[_indexSource].Play();
        audioSourceList[_indexSource].loop = _loop;

        if (_volumeUp)
        {
            volumeSet = 1;
            audioSourceList[_indexSource].volume = 0;
        }

        else
        {
            volumeSet = -1;
            audioSourceList[_indexSource].volume = originalVolume[_indexSource];
        }

        indexSourceUpdate = _indexSource;

        volumeUpdate = true;
    }

    public void SetClipForce(AudioClip _clip)
    {
        audioSourceList[0].Stop();

        audioSourceList[0].PlayOneShot(_clip);
        audioSourceList[0].loop = false;
        audioSourceList[0].volume = originalVolume[0];
    }
}
