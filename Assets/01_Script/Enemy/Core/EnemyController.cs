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

    protected void EnemyMove()
    {
        
    }

    protected void FindPlayer()
    {

    }

}
