using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   [SerializeField] private float bulletDespawnTime;

   private void Start()
   {
      Destroy(gameObject, bulletDespawnTime);
   }

   private void OnTriggerEnter2D(Collider2D target)
   {
      if (target.TryGetComponent<IDamageable>(out IDamageable entity))
      {
         entity.TakeDamage();
      }
      Destroy(gameObject);
   }
}
