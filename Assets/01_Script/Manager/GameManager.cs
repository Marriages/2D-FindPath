using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PlayerController playerController;

    //순서대로 몬스터를 제어할 목적으로.
    List<EnemyController> enemyList;
    int enemyCount;     //현재 활성화되어있는 적의 수를 기억해서 적이 모두 행동했는지 확인하기.


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        FindEnemys();
        playerController.PlayerTurnStart();
    }
    private void FindEnemys()
    {
        enemyList = new List<EnemyController>();

        EnemyController[] obj = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemy in obj)
        {
            Debug.Log($"GameManager : {enemy.gameObject.name}을 List에 넣었습니다.");
            enemyList.Add(enemy);
        }
    }
    public void PlayerTurnEnd()
    {
        // 플레이어 턴이 끝났으니, 리스트에 있는 모든 적에 대해 순차적으로 이동시키기.
        Debug.Log("GameManager : PlayerTurn종료. Enemy Turn시작");
        playerController.PlayerTurnEnd();

        enemyCount = enemyList.Count;
        if(enemyCount==0)
        {
            Debug.Log("GameManager : 모든 Enemy가 죽었습니다. PlayerTurn Start");
            playerController.PlayerTurnStart();
        }
        else
        {
            foreach(EnemyController enemy in enemyList)
            {
                Debug.Log($"GameManager : {enemy.gameObject.name} 행동 시작");
                enemy.EnemyMove();
            }
        }
    }
    public void EnemyTurnEnd()
    {
        Debug.Log($"GameManager : enemyCount : {enemyCount} -> {enemyCount - 1}");
        enemyCount--;
        if(enemyCount==0)
        {
            //모든 Enemey의 행동이 끝남.
            Debug.Log("Gamemanager : 모든 Enemy Turn종료. Player턴 시작");
            playerController.PlayerTurnStart();
        }
    }

    public void AttackEnemy(EnemyInterface enemy,int attackDamage)
    {
        if ( enemy != null )
        {
            EnemyController obj = enemy.TakeDamage(attackDamage);
            if (obj!=null)
            {
                Debug.Log("GameManager: enemy가 죽었군요!");
                enemyList.Remove(obj);              // 리스트에서 해당 객체를 삭제함.
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
