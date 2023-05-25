using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    //Enemy의 HP도 구현하면 좋을 듯.

    Animator anim;
    virtual protected void Awake()
    {
        anim=GetComponent<Animator>();
    }

    public void EnemyAnimationMoveStart()
    {
        anim.SetBool("Move", true);
    }

    public void EnemyAnimationMoveEnd()
    {
        anim.SetBool("Move", false);
    }

    public void EnemyAnimationAtack()
    {
        anim.SetTrigger("Atack");
    }

    public void EnemyAnimationDie()
    {
        anim.SetTrigger("Die");
    }
    public void EnemyDisable()
    {
        gameObject.SetActive(false);
    }
}
