using System.Collections;
using System.Collections.Generic;
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

        //우선 플레이어를 공격할 수 있는 거리인지 확인하기.
        //플레이어를 공격할 
        if (GameManager.Instance.EnemyCanDetectPlayer(transform.position, detectRange))
            //레이를 쏴서 ENemy근처에 플레이어가 있다면? 공격을 할 수 있도록 할 것.
            StartCoroutine(EnemyMoving(Vector2.down));      //테스트 용도로 넣었음. 추후  A*알고리즘의 결과로 dir를 넣을 것!
    }
    IEnumerator EnemyMoving(Vector3 dir)
    {
        model.EnemyMoveDataUpdate(dir);  //EnemyModel에 이동 정보 입력
        Vector3 targetPosition;
        targetPosition = transform.position + dir;
        //Debug.Log($"Target : {targetPosition}");

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
        //throw new System.NotImplementedException();
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
