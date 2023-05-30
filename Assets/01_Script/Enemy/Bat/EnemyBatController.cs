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

    public void EnemyMove()
    {
        Debug.Log($"{gameObject.name}의 행동 시작");
        //우선 플레이어를 공격할 수 있는 거리인지 확인하기.
        if(GameManager.Instance.EnemyCanDetectPlayer(transform.position,detectRange))
        {
            Debug.Log($"{gameObject.name}는 플레이어글 감지했다.");
            List<Vector2Int> path = GameManager.Instance.FindPathPlayer(transform.position);
            if(path.Count <= atackRange+1)     //플레이어와 완전 붙어있는 경우!
            {
                Debug.Log("Atack!");
                Atack();
            }
            else
            {
                Debug.Log(GameManager.Instance.NextEnemyPosition(path));
                StartCoroutine(EnemyMoving(GameManager.Instance.NextEnemyPosition(path)));
            }
            //플레이어가 감지된 상황. 플레이어를 향해 이동할 것.

            //플레이어와의 path의 길이가 2라면? 공격하기!
        }
        else
        {
            //만약 플레이어 감지하지 못했다면, 정해진 루트대로 이동할 것.
            Debug.Log("플레이어 감지 실패. 턴 종료");
            GameManager.Instance.EnemyTurnEnd();

        }
                  //테스트 용도로 넣었음. 추후  A*알고리즘의 결과로 dir를 넣을 것!
    }
    IEnumerator EnemyMoving(Vector3 targetPosition)
    {
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
