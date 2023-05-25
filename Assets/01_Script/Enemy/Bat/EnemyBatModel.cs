using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatModel : EnemyModel
{
    public EnemyBatModel(int health, int attackDamage)
    {
        this.health = health;
        this.attackDamage = attackDamage;
    }
    public EnemyBatModel() : base()
    {
        //만약 별도의 값 없이 해당 클래스를 생성시, EnemyModel의 기본 생성자에 따라 값이 설정되게 됨.
    }

}
