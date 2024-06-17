using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip round;
    public AudioClip roundPlague;
    public AudioClip people;
    public AudioClip city;
    public AudioClip wheat;
    public AudioClip plague;
    public AudioClip click;
    public AudioClip endingBad;
    public AudioClip endingGood;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void GetSound(int animNum, bool isPlague)
    {
        if(animNum == 0)
        {
            if (!isPlague)
            {
                source.clip = round;
                source.Play();
            }
            else
            {
                source.clip = roundPlague;
                source.Play();
            }
        }
        else if(animNum == 1)
        {
            if (!isPlague)
            {
                source.clip = people;
                source.Play();
            }
            else
            {
                source.clip = plague;
                source.Play();
            }
        }
        else if(animNum == 2)
        {
            source.clip = wheat;
            source.Play();
        }
        else if(animNum == 3)
        {
            source.clip = city;
            source.Play();
        }
    }

    public void ClickSound()
    {
        source.clip = click;
        source.Play();
    }

    public void BadEnding()
    {
        source.clip = endingBad;
        source.Play();
    }
    public void GoodEnding()
    {
        source.clip = endingGood;
        source.Play();
    }

}
