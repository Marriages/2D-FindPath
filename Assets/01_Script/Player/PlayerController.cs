using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private InputController inputActions;

    public bool isMoving = false;

    private Vector2 inputDir;

    private void Awake()
    {
        // PlayerModel 및 PlayerView 초기화
        playerModel = new PlayerModel();
        inputActions = new InputController();
        playerView = GetComponent<PlayerView>();
    }
    private void OnEnable()
    {
        PlayerInputConnect();
    }
    void PlayerInputConnect()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }
    private void OnDisable()
    {
        PlayerInputUnConnect();
    }
    void PlayerInputUnConnect()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void CheckPlayerFront()
    {
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        if(!isMoving)
        {
            isMoving = true;
            inputDir = context.ReadValue<Vector2>();
            int inputDirX = (int)inputDir.x;

            int inputDirY=(int)inputDir.y;
            if(inputDirX ==1 && inputDirY == 0)
            {
                StartCoroutine(PlayerMoving(Vector3.right));
            }
            else if (inputDirX == 0 && inputDirY == 1)
            {
                StartCoroutine(PlayerMoving(Vector3.up));
            }
            else if (inputDirX == 0 && inputDirY == -1)
            {
                StartCoroutine(PlayerMoving(Vector3.down));
            }
            else if (inputDirX == -1 && inputDirY == 0)
            {
                StartCoroutine(PlayerMoving(Vector3.left));
            }
            else
            {
                isMoving = false;
            }
        }
    }
    IEnumerator PlayerMoving(Vector3 dir)
    {
        playerModel.MoveDataUpdate(dir);      //playerModel에 이동 정보 입력
        Vector3 targetPosition;
        targetPosition = transform.position + dir;
        Debug.Log($"Target : {targetPosition}");
        
        playerView.PlayerMoveAnimationStart(dir);

        while((targetPosition - transform.position).sqrMagnitude>0.01f)
        {
            transform.Translate( playerModel.moveSpeed * Time.deltaTime * dir, Space.World);
            //transform.position = transform.position + moveSpeed * Time.deltaTime * (Vector3)dir;
            yield return null;
        }

        playerView.PlayerMoveAnimationEnd();

        transform.position = targetPosition;

        isMoving = false;
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
