using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PlayerController playerController;

    //순서대로 몬스터를 제어할 목적으로.
    Queue<GameObject> enemyQueue = new Queue<GameObject>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EnQueueObject(GameObject obj)
    {
        enemyQueue.Enqueue(obj);
    }

    public void AttackEnemy(EnemyInterface enemy,int attackDamage)
    {
        if (enemy != null)
        {
            if (!enemy.TakeDamage(attackDamage))
            {
                Debug.Log("GameManager: enemy가 죽었군요!");
            }
        }
        else
        {
            Debug.LogWarning("GamaManager : Enemy가 Null이네요. 확인해주세요");
        }
    }

    public PlayerController GetPlayer()
    {
        if (playerController != null)
            return playerController;
        else
        {
            Debug.LogWarning("GamaManager : 가지고있는 플레이어가 없습니다.");
            return null;
        }
    }
    public void GivePlayer(PlayerController playerController)
    {
        Debug.Log("GamaManager : Player 등록 완료.");
        this.playerController = playerController;
    }
}
