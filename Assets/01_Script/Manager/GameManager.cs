using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //순서대로 몬스터를 제어할 목적으로.
    Queue<GameObject> enemyQueue = new Queue<GameObject>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnQueueObject(GameObject obj)
    {
        enemyQueue.Enqueue(obj);
    }
}
