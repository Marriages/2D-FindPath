using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    //단일 책임 원칙을 준수하기위해 PlayerView에서 PlayerSound를 분리하여 코드의 가독성과 유지보수성을 향상시킴.

    [SerializeField] private AudioSource audioSourceStep;
    [SerializeField] private AudioSource audioSource;

    public AudioClip clipDoorOpen;
    public AudioClip clipStepSound;
    public AudioClip clipAtackSound;

    //------------ Sound관련 -------------
    public void PlayerSoundDoorOpen()
    {
        audioSource.clip = clipDoorOpen;
        audioSource.Play();
    }
    public void PlayerSoundStep()
    {
        audioSourceStep.clip = clipStepSound;
        audioSourceStep.Play();
    }
    public void PlayerSoundAtack()
    {
        audioSource.clip = clipAtackSound;
        audioSource.Play();
    }
}
