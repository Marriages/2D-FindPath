using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    // 플레이어의 속성 및 데이터
    public int health;
    public int attackDamage;
    public Vector2 currentPosition;
    public Vector2 beforePosition;
    public float moveSpeed = 3f;

    // 생성자
    public PlayerModel()
    {
        health = 20;
        attackDamage = 1;
        currentPosition = Vector2.zero;
        beforePosition= Vector2.zero;
    }

    // 플레이어를 이동시키는 함수
    public void MoveDataUpdate(Vector2 direction)
    {
        beforePosition = currentPosition;       //이전 위치 저장. ( 혹시라도 게임이 튕길 경우, 롤백하기 위해서. )
        currentPosition += direction;
    }


    // 플레이어를 피해를 입히는 함수
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // 플레이어가 사망한 경우 처리
        }
    }
}
