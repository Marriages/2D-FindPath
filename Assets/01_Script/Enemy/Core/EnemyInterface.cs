using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface
{
    void FindModelViewSound();
    void Atack();
    bool TakeDamage(int damage);    //bool이 true라면 살아있고, false라면 죽은 것
    void Die();
}
