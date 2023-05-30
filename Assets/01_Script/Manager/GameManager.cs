using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    PlayerController playerController;

    Tilemap background;
    Tilemap obstacle;
    GridMap map;

    //순서대로 몬스터를 제어할 목적으로.
    List<EnemyController> enemyList;
    int enemyCount;     //현재 활성화되어있는 적의 수를 기억해서 적이 모두 행동했는지 확인하기.


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        background = FindObjectOfType<Background>().GetComponent<Tilemap>();
        obstacle= FindObjectOfType<Wall>().GetComponent<Tilemap>();

        map = new GridMap(background, obstacle);
    }

    private void Start()
    {
        FindEnemys();
        if (playerController != null)
            playerController.PlayerTurnStart();
        else
        { 
            playerController = FindObjectOfType<PlayerController>();
            playerController.PlayerTurnStart();
        }
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
            foreach(EnemyBatController enemy in enemyList)
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
    public void AttackPlayer(int damage)
    {
        Debug.Log($"GameManager : player는 {damage}만큼 피해를 입었습니다.");
        playerController.OnTakeDamage(damage);
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
    public bool EnemyCanDetectPlayer(Vector2 enemyPos,int distance)
    {
        Debug.Log($"GameManager : 적과 플레이어간 거리 :  {(enemyPos - (Vector2)playerController.transform.position).magnitude}");
        if( (enemyPos-(Vector2)playerController.transform.position).sqrMagnitude < distance*distance )       //플레이어가 사정거리까지 들어왔는지 확인.
        {
            
            
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            int wallLayer = 1 << LayerMask.NameToLayer("Door");
            int excludedLayers = enemyLayer | wallLayer;
            int layerMask = ~excludedLayers;
            Vector2 dir = (playerController.transform.position - (Vector3)enemyPos).normalized;
            RaycastHit2D hit = Physics2D.Raycast(enemyPos+dir, dir, 10f, layerMask);     //혹시몰라 10f까지 넉넉하게 레이를 쏴봄.
            Debug.Log($"GameManager : RayCast결과 :{enemyPos+dir} -> {dir} /  {hit.collider.name} ");
            if (hit.collider.gameObject.CompareTag("Player"))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }

    }
    public List<Vector2Int> FindPathPlayer(Vector3 enemyPos)
    {
        List<Vector2Int> path = Astar.PathFind(map,map.WorldToGrid(enemyPos),map.WorldToGrid(playerController.transform.position));
        foreach (var p in path)
            Debug.Log(p);
        return path;
    }
    public Vector2 NextEnemyPosition(List<Vector2Int> path)
    {
        Vector2 worldPos = map.GridToWorld(path[1]);
        Vector2 localPos = (Vector3)worldPos - transform.position;
        return localPos;
    }

}
