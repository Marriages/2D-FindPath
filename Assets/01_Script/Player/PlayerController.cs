using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerView playerView;

    private void awake()
    {
        // PlayerModel 및 PlayerView 초기화
        playerModel = new PlayerModel();
        playerView = GetComponent<PlayerView>();
    }

    private void Update()
    {
        // 사용자 입력 감지 및 플레이어 동작 제어
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput);
        playerModel.Move(moveDirection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerModel.Attack();
            playerView.PlayAttackAnimation();
        }
    }

    // 플레이어가 피해를 입을 때 호출되는 함수
    public void OnTakeDamage(int damage)
    {
        playerModel.TakeDamage(damage);
        if (playerModel.health <= 0)
        {
            playerView.PlayDeathAnimation();
        }
    }
}
