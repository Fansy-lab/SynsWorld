using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    private static int m_referenceCount = 0;

    public AudioClip[] equippedItemSounds;
    public AudioClip[] unEquippedItemSounds;


    public AudioClip pickUpItemSound;
    public AudioClip goldPickUpSound;
    public AudioClip miscPickedUpSound;
    public AudioClip dashSound;
    public AudioClip releaseArrowSound;
    public AudioClip hitSound;
    public AudioClip levelUpSound;
    public AudioClip letterType;
    public AudioClip openQuestSound;
    public AudioClip closeQuestSound;
    public AudioClip newQuestSound;
    public AudioClip doneQuestSound;
    public AudioClip menuClickSound;
    public AudioClip speechBubblePop;

    AudioSource source;

    private void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void PlayEquippedItemSound()
    {
        int random = UnityEngine.Random.Range(0, equippedItemSounds.Length);
        source.PlayOneShot(equippedItemSounds[random]);
    }

    internal void PlayOpenQuestSound()
    {
        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();
        source.PlayOneShot(openQuestSound);
        Invoke("ResetPitch", 0.5f);
    }

    internal void PlayUnEquippedItemSound()
    {
        int random = UnityEngine.Random.Range(0, unEquippedItemSounds.Length);
        source.PlayOneShot(unEquippedItemSounds[random]);
    }

    public void PlayPickedUpItemSound()
    {
        if (!source.isPlaying)
        source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(pickUpItemSound);
        Invoke("ResetPitch", 0.5f);

    }
    public void PlayGoldPickedUpSound()
    {
        if (!source.isPlaying)
        source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(goldPickUpSound);
        Invoke("ResetPitch", 0.5f);

    }

    internal void PlayPickedMiscItemSound()
    {
        if(!source.isPlaying)
        source.pitch = RNGGod.GetRandomPitch();
        source.PlayOneShot(miscPickedUpSound);
        Invoke("ResetPitch", 0.5f);
    }

    internal void PlayCloseQuestSound()
    {
        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();
        source.PlayOneShot(closeQuestSound);
        Invoke("ResetPitch", 0.5f);
    }

    public void ResetPitch()
    {
        source.pitch = 1;
    }

    public void PlayLevelUpSound()
    {
        ResetPitch();
        source.PlayOneShot(levelUpSound);
    }
    public void PlayMenuClickSound()
    {

        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(menuClickSound);
        Invoke("ResetPitch", 0.5f);
    }
    public void PlayDashSound()
    {
        if (!source.isPlaying)
        source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(dashSound);
        Invoke("ResetPitch", 0.5f);
    }
    public void PlayReleaseArrowSound()
    {
        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(releaseArrowSound);
        Invoke("ResetPitch", 0.5f);
    }
    public void PlayHitSound()
    {
        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(hitSound);
        Invoke("ResetPitch", 0.5f);
    }
    public void PlayNewQuestSound()
    {


        source.PlayOneShot(newQuestSound);

    }
    public void PlayDoneQuestSound()
    {


        source.PlayOneShot(doneQuestSound);

    }
    public void PlayLetter()
    {
        if (!source.isPlaying)
            source.pitch = RNGGod.GetRandomPitch();

        source.PlayOneShot(letterType);
    }

    internal void PlaySpeechBubblePop()
    {
        //find better sound

        //if (!source.isPlaying)
        //    source.pitch = RNGGod.GetRandomPitch();

        //source.PlayOneShot(speechBubblePop);
        //Invoke("ResetPitch", 0.5f);
    }
}
