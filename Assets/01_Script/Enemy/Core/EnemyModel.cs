using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    // 플레이어의 속성 및 데이터
    public int health;
    public int attackDamage;
    public Vector2 currentPosition;
    public Vector2 beforePosition;
    public float moveSpeed = 5f;    //원래 5였지만, 테스트가 잘 되는지 확인하기 위해 속도를 1로 낮췄음. 모든 테스트가 끝나고 5로 돌려놓을 것.

    public List<Vector2Int> scoutPath=null;      //정찰 목적용의 길찾기 경로
    int index = 0;                          // 정찰 목적용의 길찾기 인덱스

    public Vector2Int OUT_OF_RANGE = new(int.MaxValue, int.MaxValue);


    // 생성자
    public EnemyModel()
    {
        health = 10;
        attackDamage = 1;
    }

    public void EnemyScoutPathSetting(List<Vector2Int> scoutPath)
    {
        //Debug.Log($"EnemyMode : 정찰용 경로 저장 완료");
        if(scoutPath !=null)
        {
            this.scoutPath = scoutPath;             //여기서 제대로 저장이 안된거같은 느낌
            index = 0;
        }
        else
        {
            Debug.LogWarning("왜그런진 모르곘지만 scoutPath가 널이네요?");
        }
    }
    public void EnemyScoutPathClear()
    {
        Debug.Log($"EnemyMode : 정찰용 경로 초기화 완료");
        this.scoutPath = null;
        index = 0;
    }
    public Vector2Int EnemyScoutNextPoint()
    {
        if(scoutPath == null)
            Debug.LogWarning($"scoutPath가 Null인데, 오류 발생에정임! ");
        //Debug.Log($"Index+전 : {index} / scoutPath.count : {scoutPath.Count} / path.VectorInt : {scoutPath[index]}");
        index++;
        if( index > (scoutPath.Count-1) )
        {
            Debug.Log("Out Of Range");
            return OUT_OF_RANGE;
        }
        else
        {
            Vector2Int path = scoutPath[index];         //ArgumentOutOfRangeException  발생함. 인덱스를 벗어난 Path를 구하려 해서 발생한 문제임. 씨발씨발씨발씨발
            return path;
        }
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
