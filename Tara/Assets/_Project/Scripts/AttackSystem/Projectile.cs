using System.Collections.Generic;
using UnityEngine;
using Tara.Core;

namespace Tara.AttackSystem
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
	public class Projectile : MonoBehaviour
	{
		public float Speed = default;
		public int Damage = 1;
		[Space]
		public List<EntityType> WhitelistType = new List<EntityType>();
		public EntityType Shooter = default;
		[Space]
		public Color TeamColor = Color.white;

		private Rigidbody2D _rigidbody;
		private SpriteRenderer _spriterenderer;

		private Vector2 _targetPosition;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_spriterenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			_spriterenderer.color = TeamColor;
		}

		private void Update()
		{
			_targetPosition = transform.up * Speed * Time.deltaTime;
			_rigidbody.position += _targetPosition;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (ValidateCollision(collider))
			{
				var health = collider.GetComponent<Health>();
				if (health != null)
				{
					health.Damage(Damage);
				}

				//Debug.Log($"Collision, {shooter}, {speed}");

				Destroy(gameObject);
			}
		}

		private bool ValidateCollision(Collider2D collider)
		{
			var projectileCollider = collider.GetComponent<ICanCollideWithProjectiles>();
			if (projectileCollider == null) 
			{ 
				return false; 
			}
			else
			{
				Entity entity = collider.GetComponent<Entity>();
				if (entity != null)
				{
					foreach (var element in WhitelistType)
					{
						if (element == entity.Type) { return false; }
					}

					if (entity.Type == Shooter) { return false; }
				}

				if (projectileCollider != null) { return true; }
			}

			return false;
		}
	}
}
