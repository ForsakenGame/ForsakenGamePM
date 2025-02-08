using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthController : MonoBehaviour
{
    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT }
    private enum HeldItems { EMPTY = 0, SPEAR = 1, GUN = 2 }
    #endregion


    #region Editor Data
    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;
    [SerializeField] Animator _animator;
    #endregion
    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;
    private HeldItems _heldItem = HeldItems.EMPTY;

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
    #endregion
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Heal(1);
        }

        CalculateFacingDirection();
        SaveLastUsedItem();
    }

    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
        {
            _facingDirection = _moveDir.x > 0 ? Directions.RIGHT : Directions.LEFT;
        }
        else if (_moveDir.y != 0)
        {
            _facingDirection = _moveDir.y > 0 ? Directions.UP : Directions.DOWN;

        }
        // Debug.Log(_facingDirection);
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

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Heal(int health)
    {
        currentHealth += health;
        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        // Play death animation depending on held item and facing direction
        if (_heldItem == HeldItems.EMPTY)
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
        else if(_heldItem == HeldItems.GUN)
        {
            if (_facingDirection == Directions.LEFT || _facingDirection == Directions.RIGHT)
            {
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
        // Show death screen

    }
}