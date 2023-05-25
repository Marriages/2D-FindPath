using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private InputController inputActions;
    private PlayerSound playerSound;

    public bool isMoving = false;
    public bool myTurn = false;

    private Vector2 inputDir;

    private void Awake()
    {
        // PlayerModel 및 PlayerView 초기화
        playerModel = new PlayerModel();
        inputActions = new InputController();
        playerSound = GetComponent<PlayerSound>();
        playerView = GetComponent<PlayerView>();

        GameManager.Instance.GivePlayer(this);
    }
    private void OnEnable()
    {
        PlayerInputConnect();
    }
    void PlayerInputConnect()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
    }
    private void OnDisable()
    {
        PlayerInputUnConnect();
    }
    void PlayerInputUnConnect()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        
    }
    public void PlayerTurnStart()
    {
        myTurn = true;
    }
    public void PlayerTurnEnd()
    {
        myTurn = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        if(isMoving==false && myTurn==true)
        {
            isMoving = true;
            inputDir = context.ReadValue<Vector2>();

            int inputDirX = (int)inputDir.x;
            int inputDirY=(int)inputDir.y;
            Vector3 dir;

            if (inputDirX == 1 && inputDirY == 0)
            {
                dir = Vector3.right;
            }
            else if (inputDirX == 0 && inputDirY == 1)
            {
                dir = Vector3.up;
            }
            else if (inputDirX == 0 && inputDirY == -1)
            {
                dir = Vector3.down;
            }
            else if (inputDirX == -1 && inputDirY == 0)
            {
                dir = Vector3.left;
            }
            else
            {
                Debug.Log("잘못된 입력입니다.");
                isMoving = false;
                return;         //아래의 로직을 더이상 수행하지 않고 입력 처리 끝.
            }
            
            if (!CheckPlayerFront(dir))     //true일 경우 이동성공하고 isMoving 초기화 성공. False일 경우 이동이 끝났으므로 직접 초기화
                isMoving = false;
        }
    }
    private bool CheckPlayerFront(Vector3 dir)
    {
        //Debug.Log("레이발사!");
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));        //플레이어 자신은 레이에 닿지 않도록 레이어 조정.
        RaycastHit2D hit = Physics2D.Raycast(transform.position+dir, dir, 0.1f, layerMask);
        Debug.DrawRay(transform.position+dir, dir, Color.white);
        if (hit.collider == null)
        {
            //아무것도 없다 => 이동할 수 있다.
            Debug.Log("아무것도 없네요!");
            StartCoroutine(PlayerMoving(dir));
            return true;
        }
        else if (hit.collider.CompareTag("Enemy"))
        {
            //문을 통과했기에 문을 여는 소리
            Debug.Log("적 발견!");
            playerSound.PlayerSoundAtack();
            GameManager.Instance.AttackEnemy(hit.collider.GetComponent<EnemyInterface>(),playerModel.attackDamage);
            GameManager.Instance.PlayerTurnEnd();       //공격했으니 플레이어 턴 끝!
            return false;
        }
        else if(hit.collider.CompareTag("Door"))
        {
            //문을 통과했기에 문을 여는 소리
            Debug.Log("문 통과!");
            playerSound.PlayerSoundDoorOpen();
            StartCoroutine(PlayerMoving(dir));
            return true;
        }
        

        //2를 돌렺루 경우 아이템

        //3을 돌려줄 경우 몬스터

        //4를 돌려줄 경우 다음 지형

        return false;
    }

    IEnumerator PlayerMoving(Vector3 dir)
    {
        playerModel.MoveDataUpdate(dir);      //playerModel에 이동 정보 입력
        Vector3 targetPosition;
        targetPosition = transform.position + dir;
        //Debug.Log($"Target : {targetPosition}");
        
        playerView.PlayerMoveAnimationStart(dir);

        while((targetPosition - transform.position).sqrMagnitude>0.01f)
        {
            transform.Translate( playerModel.moveSpeed * Time.deltaTime * dir, Space.World);
            //transform.position = transform.position + moveSpeed * Time.deltaTime * (Vector3)dir;
            yield return null;
        }

        playerView.PlayerMoveAnimationEnd();

        transform.position = targetPosition;

        GameManager.Instance.PlayerTurnEnd();       //이동했으니 플레이어 턴 끝!
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
