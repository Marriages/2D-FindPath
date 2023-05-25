using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    // 플레이어의 속성 및 데이터
    public int health;
    public int attackDamage;
    public Vector2 currentPosition;
    public Vector2 beforePosition;
    public float moveSpeed = 5f;

    // 생성자
    public EnemyModel()
    {
        health = 10;
        attackDamage = 1;
    }
    

    // 플레이어를 이동시키는 함수
    public void EnemyMoveDataUpdate(Vector2 direction)
    {
        beforePosition = currentPosition;       //이전 위치 저장. ( 혹시라도 게임이 튕길 경우, 롤백하기 위해서. )
        currentPosition += direction;
    }


    // 플레이어를 피해를 입히는 함수
    public bool EnemyTakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"EnemyModel : {damage}만큼 피해를 입음 Health : {health}");
        if (health <= 0)
        {
            return false;       //죽었음을 false로 표시
        }
        else
        {
            return true;        //살았음을 true로 표시
        }
    }
}
