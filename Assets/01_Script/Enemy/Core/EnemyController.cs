using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected EnemyModel model;
    protected EnemyView view;
    protected EnemySound sound;

    [Header("Enemy Initialize Setting")]
    [SerializeField] protected int health;
    [SerializeField] protected int attackDamage;

    public void EnemyMove()
    {
        StartCoroutine(EnemyMoving(Vector2.down));
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

    protected void FindPlayer()
    {

    }

}
