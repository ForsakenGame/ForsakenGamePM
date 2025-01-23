using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GreenZombie_Controller : MonoBehaviour
{

    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT }
    #endregion

    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("Player Reference")]
    [SerializeField] Transform _player; // Referencia al jugador
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Run_Right");
    private readonly int _animMoveLeft = Animator.StringToHash("Anim_Run_Left");
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Run_Back");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Run_Front");
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Idle_Right");
    private readonly int _animIdleLeft = Animator.StringToHash("Anim_Idle_Left");
    private readonly int _animIdleFront = Animator.StringToHash("Anim_Idle_Front");
    private readonly int _animIdleBack = Animator.StringToHash("Anim_Idle_Back");
    #endregion

    #region Tick

    void Update()
    {
        if (_player != null)
        {

            CalculateMoveDirection();


            UpdateAnimation();
        }
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {

            MovementUpdate();
        }
    }
    #endregion

    #region Movement Logic
    private void CalculateMoveDirection()
    {

        Vector2 directionToPlayer = (_player.position - transform.position).normalized;


        _moveDir = directionToPlayer;
    }

    private void MovementUpdate()
    {

        _rb.velocity = _moveDir * _moveSpeed * Time.fixedDeltaTime;
    }
    #endregion

    #region Animation Logic
    private void UpdateAnimation()
    {

        Vector2 directionToPlayer = (_player.position - transform.position).normalized;


        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            if (directionToPlayer.x > 0)
            {
                _facingDirection = Directions.RIGHT;
            }
            else if (directionToPlayer.x < 0)
            {
                _facingDirection = Directions.LEFT;
            }
        }
        else
        {
            if (directionToPlayer.y > 0)
            {
                _facingDirection = Directions.UP;
            }
            else if (directionToPlayer.y < 0)
            {
                _facingDirection = Directions.DOWN;
            }
        }


        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }


        if (_moveDir.sqrMagnitude > 0)
        {
            if (_facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animMoveRight, 0);
            }
            else if (_facingDirection == Directions.LEFT)
            {
                _animator.CrossFade(_animMoveLeft, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animMoveUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animMoveDown, 0);
            }
        }
        else
        {

            if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animIdleBack, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animIdleFront, 0);
            }
            else if (_facingDirection == Directions.LEFT)
            {
                _animator.CrossFade(_animIdleLeft, 0);
            }
            else if (_facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animIdleRight, 0);
            }
        }
    }
    #endregion

    public void SetPlayer(Transform player)
    {
        _player = player;
    }
}

