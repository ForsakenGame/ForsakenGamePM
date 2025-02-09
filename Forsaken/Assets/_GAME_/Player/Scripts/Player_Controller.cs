using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT }
    private enum HeldItems { EMPTY = 0, SPEAR = 1, GUN = 2 }
    #endregion

    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;
    [SerializeField] float _runSpeed = 70f;
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject deathScreenCanvas;
    #endregion

    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;
    private HeldItems _heldItem = HeldItems.EMPTY;
    private bool _isDead = false;

    // Walking animations
    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Walk_Right"); // Readonly to not edit later by accident
    private readonly int _animMoveUp = Animator.StringToHash("Anim_Player_Walk_Up");
    private readonly int _animMoveDown = Animator.StringToHash("Anim_Player_Walk_Down");

    private readonly int _animWalkGunRight = Animator.StringToHash("Anim_Player_WalkGun_Right");
    private readonly int _animWalkGunUp = Animator.StringToHash("Anim_Player_WalkGun_Up");
    private readonly int _animWalkGunDown = Animator.StringToHash("Anim_Player_WalkGun_Down");

    private readonly int _animWalkSpearRight = Animator.StringToHash("Anim_Player_WalkSpear_Right");
    private readonly int _animWalkSpearUp = Animator.StringToHash("Anim_Player_WalkSpear_Up");
    private readonly int _animWalkSpearDown = Animator.StringToHash("Anim_Player_WalkSpear_Down");

    // Reloading animation
    private readonly int _animReloadRight = Animator.StringToHash("Anim_Player_Reload_Right");
    private readonly int _animReloadUp = Animator.StringToHash("Anim_Player_Reload_Up");
    private readonly int _animReloadDown = Animator.StringToHash("Anim_Player_Reload_Down");
    // Running animations
    private readonly int _animRunNormalRight = Animator.StringToHash("Anim_Player_RunNormal_Right");
    private readonly int _animRunNormalUp = Animator.StringToHash("Anim_Player_RunNormal_Up");
    private readonly int _animRunNormalDown = Animator.StringToHash("Anim_Player_RunNormal_Down");

    private readonly int _animRunSpearRight = Animator.StringToHash("Anim_Player_RunSpear_Right");
    private readonly int _animRunSpearUp = Animator.StringToHash("Anim_Player_RunSpear_Up");
    private readonly int _animRunSpearDown = Animator.StringToHash("Anim_Player_RunSpear_Down");

    private readonly int _animRunGunRight = Animator.StringToHash("Anim_Player_RunGun_Right");
    private readonly int _animRunGunUp = Animator.StringToHash("Anim_Player_RunGun_Up");
    private readonly int _animRunGunDown = Animator.StringToHash("Anim_Player_RunGun_Down");
    // Idle animations
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle_Right");
    private readonly int _animIdleUp = Animator.StringToHash("Anim_Player_Idle_Up");
    private readonly int _animIdleDown = Animator.StringToHash("Anim_Player_Idle_Down");

    private readonly int _animIdleGunRight = Animator.StringToHash("Anim_Player_IdleGun_Right");
    private readonly int _animIdleGunUp = Animator.StringToHash("Anim_Player_IdleGun_Up");
    private readonly int _animIdleGunDown = Animator.StringToHash("Anim_Player_IdleGun_Down");

    private readonly int _animIdleSpearRight = Animator.StringToHash("Anim_Player_IdleSpear_Right");
    private readonly int _animIdleSpearUp = Animator.StringToHash("Anim_Player_IdleSpear_Up");
    private readonly int _animIdleSpearDown = Animator.StringToHash("Anim_Player_IdleSpear_Down");

    // Static GUN & SPEAR
    private readonly int _animStaticGunRight = Animator.StringToHash("Anim_Player_StaticGun_Right");
    private readonly int _animStaticGunUp = Animator.StringToHash("Anim_Player_StaticGun_Up");
    private readonly int _animStaticGunDown = Animator.StringToHash("Anim_Player_StaticGun_Down");

    private readonly int _animSpearRight = Animator.StringToHash("Anim_Player_Spear_Right");
    private readonly int _animSpearUp = Animator.StringToHash("Anim_Player_Spear_Up");
    private readonly int _animSpearDown = Animator.StringToHash("Anim_Player_Spear_Down");
    // Running while using gun
    private readonly int _animRunningShootingRight = Animator.StringToHash("Anim_Player_RunningGun_Right");
    private readonly int _animRunningShootingUp = Animator.StringToHash("Anim_Player_RunningGun_Up");
    private readonly int _animRunningShootingDown = Animator.StringToHash("Anim_Player_RunningGun_Down");

    // Dying animations 
    private readonly int _animDeathGunRight = Animator.StringToHash("Anim_Player_DeathGun_Right");
    private readonly int _animDeathGunUp = Animator.StringToHash("Anim_Player_DeathGun_Up");
    private readonly int _animDeathGunDown = Animator.StringToHash("Anim_Player_DeathGun_Down");

    private readonly int _animDeathSpearRight = Animator.StringToHash("Anim_Player_DeathSpear_Right");
    private readonly int _animDeathSpearUp = Animator.StringToHash("Anim_Player_DeathSpear_Up");
    private readonly int _animDeathSpearDown = Animator.StringToHash("Anim_Player_DeathSpear_Down");

    private readonly int _animDeathNormalRight = Animator.StringToHash("Anim_Player_DeathNormal_Right");
    private readonly int _animDeathNormalUp = Animator.StringToHash("Anim_Player_DeathNormal_Up");
    private readonly int _animDeathNormalDown = Animator.StringToHash("Anim_Player_DeathNormal_Down");
    #endregion

    #region Animator Data
    public bool isRightClickHeld = false;
    public bool isLeftClicked = false;
    public bool isRunning = false; // Whether shift is pressed or not
    #endregion
    #region Tick
    private void Update()
    {
        GatherInput();   
        CalculateFacingDirection();
        UpdateAnimation();
        SaveLastUsedItem();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.PauseGame();
        }
    }


    private void FixedUpdate()
    {
        MovementUpdate();
    }
    #endregion

    #region Input Logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");

        isLeftClicked = Input.GetMouseButton(0);
        isRightClickHeld = Input.GetMouseButton(1);

        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void SaveLastUsedItem()
    {
        if (isLeftClicked)
        {
            _heldItem = HeldItems.SPEAR;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
        if (isRightClickHeld)
        {
            _heldItem = HeldItems.GUN;
            _animator.SetInteger("HeldItem", (int)_heldItem);
        }
    }
    #endregion

    #region Movement Logic
    private void MovementUpdate()
    {
        if(!isRunning)
        {
            _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
        } 
        else
        {
            _rb.velocity = _moveDir.normalized * _runSpeed * Time.fixedDeltaTime;
        }
    }
    #endregion

    #region Animation Logic
    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            _facingDirection = _moveDir.x > 0 ? Directions.RIGHT : Directions.LEFT;
        }
        else if (_moveDir.y != 0)
        {
            _facingDirection = _moveDir.y > 0 ? Directions.UP :  Directions.DOWN;

        }
        // Debug.Log(_facingDirection);
    }
    private void UpdateAnimation()
    {
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if (_moveDir.SqrMagnitude() > 0 && !isRunning && !_isDead) // We're walking
        {
            if(_heldItem == HeldItems.EMPTY) // Walking Normal
            {
                if(_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animMoveRight, 0);
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
            else if (_heldItem == HeldItems.SPEAR) // Walking with spear
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animWalkSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animWalkSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animWalkSpearDown, 0);
                }
            }
            else if (_heldItem == HeldItems.GUN) // Walking with gun
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animWalkGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animWalkGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animWalkGunDown, 0);
                }
            }
        }
        else if (_moveDir.SqrMagnitude() > 0 && isRunning && !isRightClickHeld && !_isDead) // Running 
        {
            if(_heldItem == HeldItems.EMPTY) // Running normal
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animRunNormalRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animRunNormalUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animRunNormalDown, 0);
                }
            }
            else if (_heldItem == HeldItems.SPEAR) // Running Spear
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animRunSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animRunSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animRunSpearDown, 0);
                }
            }
            else if (_heldItem == HeldItems.GUN) // Running Gun
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animRunGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animRunGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animRunGunDown, 0);
                }
            }
        }
        else if (_moveDir.SqrMagnitude() > 0 && isRunning && isRightClickHeld && !_isDead) // Shooting while running
        {
            if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animRunningShootingRight, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animRunningShootingUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animRunningShootingDown, 0);
            }
        }
        else if(!_isDead)// Idle and static attacks
        {
            if(!isRightClickHeld && !isLeftClicked && _heldItem == HeldItems.EMPTY) // Normal IDLE
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleDown, 0);
                }
            }
            else if (!isRightClickHeld && !isLeftClicked && _heldItem == HeldItems.GUN) // Holding Gun Idle 
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleGunDown, 0);
                }
            }
            else if (!isRightClickHeld && !isLeftClicked && _heldItem == HeldItems.SPEAR) // Holding Spear Idle  
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animIdleSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animIdleSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animIdleSpearDown, 0);
                }
            }
            else if (isLeftClicked) // Static Spear attack
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animSpearRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animSpearUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animSpearDown, 0);
                }
            }
            else if (isRightClickHeld) // Static shootings
            {
                if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
                {
                    _animator.CrossFade(_animStaticGunRight, 0);
                }
                else if (_facingDirection == Directions.UP)
                {
                    _animator.CrossFade(_animStaticGunUp, 0);
                }
                else if (_facingDirection == Directions.DOWN)
                {
                    _animator.CrossFade(_animStaticGunDown, 0);
                }
            }
        }
    }

    public void Die()
    {
        _isDead = true;
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        // Play death animation depending on held item and facing direction
        if (_heldItem == HeldItems.EMPTY)
        {
            if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
                Debug.Log("Dead");
                _animator.CrossFade(_animDeathNormalRight, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animDeathNormalUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animDeathNormalDown, 0);
            }
        }
        else if (_heldItem == HeldItems.SPEAR)
        {
            if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animDeathSpearRight, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animDeathSpearUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animDeathSpearDown, 0);
            }
        }
        else if (_heldItem == HeldItems.GUN)
        {
            if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
                _animator.CrossFade(_animDeathGunRight, 0);
            }
            else if (_facingDirection == Directions.UP)
            {
                _animator.CrossFade(_animDeathGunUp, 0);
            }
            else if (_facingDirection == Directions.DOWN)
            {
                _animator.CrossFade(_animDeathGunDown, 0);
            }
        }
        // Show death screen and pause game
        // Checks if current playing animation has ended
   
    }
    #endregion

    public void ShowDeathMenu()
    {
        Time.timeScale = 0;
        deathScreenCanvas.SetActive(true);
    }
}
