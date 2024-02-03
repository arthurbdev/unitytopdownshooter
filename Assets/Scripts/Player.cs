using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
  public int health;
  public static Action OnPlayerDeath;

  [SerializeField] private float moveSpeed;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private Weapon weapon;

  private Rigidbody2D rb;
  private float velocity;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    HandleShooting();
  }


  private void FixedUpdate()
  {
    HandleMovement();
    HandleRotation();
  }

  private void HandleShooting()
  {
    if (gameInput.IsMouseClicked())
    {
      weapon.Shoot();
    }
  }

  private void HandleRotation()
  {
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(gameInput.GetMousePosition());
    Vector2 aimDirection = mousePosition - rb.position;
    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    rb.MoveRotation(angle);
  }

  private void HandleMovement()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    rb.velocity = new Vector3(inputVector.x, inputVector.y, 0f) * moveSpeed;
  }

  public void TakeDamage()
  {
    health--;
    if (health <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    OnPlayerDeath?.Invoke();
    Destroy(gameObject);
  }

}
