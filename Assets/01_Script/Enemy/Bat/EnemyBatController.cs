using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatController : EnemyController, EnemyInterface
{
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
        throw new System.NotImplementedException();
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
