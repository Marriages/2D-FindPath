using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        // 플레이어의 위치와 애니메이션 업데이트
    }

    // 플레이어의 공격 애니메이션 재생 함수
    public void PlayAttackAnimation()
    {
        // 공격 애니메이션 재생 코드
    }

    // 플레이어의 사망 애니메이션 재생 함수
    public void PlayDeathAnimation()
    {
        // 사망 애니메이션 재생 코드
    }
    public void PlayerMoveAnimationStart(Vector3 dir)
    {
        anim.SetFloat("PlayerDirX", dir.x);
        anim.SetBool("PlayerMove", true);
    }
    public void PlayerMoveAnimationEnd()
    {
        anim.SetBool("PlayerMove", false);
    }
}
