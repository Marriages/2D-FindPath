using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    //Enemy가 가지고 있어야 할 기본적인 사운드. 만약 추가가 필요하다면, 해당 스크립트를 상속받는 자식에서 구현할 것.
    private AudioSource audioSource;
    public AudioClip clipEnemyDie;
    public AudioClip clipEnemyAtack;
    public AudioClip clipEnemyDoorOpen;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource==null)
        {
            Debug.LogWarning($"{this.gameObject.name}의 AudioSource가 없어서 별도로 추가했습니다.");
            audioSource = transform.AddComponent<AudioSource>();        //혹시라도...AudioSource를 안넣는 실수를 할 수 있으니까!
        }
    }
    public void EnemySoundDoorOpen()
    {
        audioSource.clip = clipEnemyDoorOpen;
        audioSource.Play();
    }
    public void EnemySoundAtack()
    {
        audioSource.clip = clipEnemyAtack;
        audioSource.Play();
    }
    public void EnemySoundDie()
    {
        audioSource.clip = clipEnemyDie;
        audioSource.Play();
    }
}
