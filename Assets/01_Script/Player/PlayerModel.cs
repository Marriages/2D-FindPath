using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    // 플레이어의 속성 및 데이터
    public int health;
    public int attackDamage;
    public Vector2 position;

    // 생성자
    public PlayerModel()
    {
        health = 100;
        attackDamage = 10;
        position = Vector2.zero;
    }

    // 플레이어를 이동시키는 함수
    public void Move(Vector2 direction)
    {
        position += direction;
    }

    // 플레이어를 공격하는 함수
    public void Attack()
    {
        // 공격 로직 구현
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
