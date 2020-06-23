using System.Collections.Generic;
using UnityEngine;
using Tara.Core;

namespace Tara.AttackSystem
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
	public class Projectile : MonoBehaviour
	{
		public float speed = default;
		public int damage = 1;

		[Space]
		public List<EntityType> whitelistType = new List<EntityType>();

		public Color teamColor = Color.white;

		public EntityType shooter = default;

		private Rigidbody2D rb;
		private SpriteRenderer sprite;

		private Vector2 targetPosition;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			sprite = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			sprite.color = teamColor;
		}

		private void Update()
		{
			targetPosition = transform.up * speed * Time.deltaTime;
			rb.position += targetPosition;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (ValidateCollision(collider))
			{
				if (collider.GetComponent<Health>() != null)
				{
					collider.GetComponent<Health>().Damage(damage);
				}

				//Debug.Log($"Collision, {shooter}, {speed}");

				Destroy(gameObject);
			}
		}

		private bool ValidateCollision(Collider2D collider)
		{
			Entity entity = collider.GetComponent<Entity>();
			if (entity != null)
			{
				foreach (var element in whitelistType)
				{
					if (element == entity.Type) { return false; }
				}

				if (entity.Type != shooter) { return true; }
				if (collider.GetComponent<ICanCollideWithProjectiles>() == null) { return true; }
			}
			return false;
		}
	}
}