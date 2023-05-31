using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyBatController : EnemyController, EnemyInterface
{
    protected EnemyModel model;
    protected EnemyView view;
    protected EnemySound sound;

    [Header("Enemy Initialize Setting")]
    [SerializeField] protected int health;
    [SerializeField] protected int attackDamage;

    protected BoxCollider2D col;
    public int detectRange = 3;
    public int atackRange = 1;

    bool detectPlayer = false;

    // 본인만의 정찰 경로를 가지고 있을 필요가 있음.  -> EnemyModel에게 데이터를 저장하는것으로!

    public void EnemyMove()
    {
        Debug.Log($"{gameObject.name}의 행동 시작");
        //우선 플레이어를 공격할 수 있는 거리인지 확인하기.
        if(GameManager.Instance.EnemyCanDetectPlayer(transform.position,detectRange))
        {
            Debug.Log($"{gameObject.name}는 플레이어를 감지했다.");
            detectPlayer = true;
            List<Vector2Int> path = GameManager.Instance.FindPathPlayer(transform.position);
            if(path.Count <= atackRange+1)     //플레이어와 완전 붙어있는 경우!
            {
                Debug.Log("Atack!");
                Atack();
            }
            else                // 플레이어와 완전히 붙어있지 않으므로, 다음 경로를 받아와서 이동하기!
            {
                Debug.Log(GameManager.Instance.NextEnemyPosition(path));
                StartCoroutine(EnemyMoving(GameManager.Instance.NextEnemyPosition(path)));
            }
            //플레이어가 감지된 상황. 플레이어를 향해 이동할 것.

            //플레이어와의 path의 길이가 2라면? 공격하기!
        }
        else            //플레이어를 감지하지 못한 상태. 정찰을 진행할 것.
        {
            if(detectPlayer==true)  //이경우 플레이어를 감지하고 있다가, 플레이어를 감지하지 못하는 상태로 변함 -> 경로를 초기화해줘야함.
                model.EnemyScoutPathClear();

            //만약 플레이어 감지하지 못했다면, 정해진 루트대로 이동할 것.
            Debug.Log($"{gameObject.name} : 플레이어 감지 못함. 정찰진행");
            if(model.scoutPath ==null)          // 정찰할 경로가 없는 경우임. 게임오브젝트를 통해서 정찰용 path를 받아올 것.
            {
                model.EnemyScoutPathSetting(GameManager.Instance.FindPathNewScoutPoint(transform.position));    // 현재 위치를 시작으로 새로운 정찰포인트를 구해옴.
                Vector2 nextPosition = GameManager.Instance.NextEnemyPosition(model.EnemyScoutNextPoint());     // 받아온 경로로부터 다음 이동할 위치를 받아옴
                StartCoroutine(EnemyMoving(nextPosition));          // 그위치를 향해 움직이기 시작!
            }
            else                                //이미 정찰할 경로가 있는 경우임. 경로가 끝난건지 아닌지 확인을 꼭 할 것.
            {
                Vector2Int nextPosition = model.EnemyScoutNextPoint();          //model로부터 정찰할 다음 값을 받아옴.

                if(nextPosition != model.OUT_OF_RANGE)              // model로부터 받아온 값이 유효한 값이라면 그대로 진행
                {
                    StartCoroutine(EnemyMoving(GameManager.Instance.NextEnemyPosition(nextPosition)));
                }
                else                    //만약 그렇지 않다면, 경로가 끝난 것이므로,nextPosition을 게임매니저로부터 다시 경로를 받아올 것
                {
                    model.EnemyScoutPathSetting(GameManager.Instance.FindPathNewScoutPoint(transform.position));    //path새로 받아와서 model에 셋팅하기
                    nextPosition = model.EnemyScoutNextPoint();         //model로부터 경로 받아오기.
                    StartCoroutine(EnemyMoving(GameManager.Instance.NextEnemyPosition(nextPosition)));
                }
            }
            GameManager.Instance.EnemyTurnEnd();

        }
    }
    IEnumerator EnemyMoving(Vector3 targetPosition)
    {
        //이동이끝나고 해당 그리드맵에 몬스터 본인이 있따고 알려야 할 필요가 있음.  -> 게임매니저에게..!

        //Debug.Log(targetPosition);
        //dir = dir.normalized;
        Vector3 dir;
        dir = (targetPosition - transform.position).normalized;
        //Debug.Log(dir);
        //Debug.Log($"Target : {targetPosition}");
        model.EnemyMoveDataUpdate(dir);  //EnemyModel에 이동 정보 입력

        view.EnemyAnimationMoveStart();

        while ((targetPosition - transform.position).sqrMagnitude > 0.01f)
        {
            transform.Translate(model.moveSpeed * Time.deltaTime * dir, Space.World);
            //transform.position = transform.position + moveSpeed * Time.deltaTime * (Vector3)dir;
            yield return null;
        }

        view.EnemyAnimationMoveEnd();

        transform.position = targetPosition;

        GameManager.Instance.EnemyTurnEnd();

    }



    private void Awake()
    {
        FindModelViewSound();
    }
    //인터페이스로 인해 구현함
    public void FindModelViewSound()
    {
        model = new EnemyBatModel(health, attackDamage);
        view = GetComponent<EnemyBatView>();
        sound = GetComponent<EnemyBatSound>();
    }
    //인터페이스로 인해 구현함
    public void Atack()
    {
        GameManager.Instance.AttackPlayer(attackDamage);
        view.EnemyAnimationAtack();
        sound.EnemySoundAtack();
        GameManager.Instance.EnemyTurnEnd();
    }

    //인터페이스로 인해 구현함
    public void Die()
    {
        view.EnemyAnimationDie();
        sound.EnemySoundDie();
    }

    //인터페이스로 인해 구현함
    public EnemyController TakeDamage(int damage)
    {
        Debug.Log($"EnemyBatController : {this.gameObject.name} 이 {damage}만큼 공격당함");
        if (!model.EnemyTakeDamage(damage))
        {
            Die();
            return this;
        }
        else
        {
            return null;
        }
    }
}
