using System.Collections.Generic;
using UnityEngine;
using CITools;

namespace Tara.Combat
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
	public class Projectile : MonoBehaviour
	{
		public float Speed = default;
		public int Damage = 1;
		public float Lifetime = 10f;
		[Space]
		public List<EntityType> WhitelistType = new List<EntityType>();
		public EntityType Shooter = default;
		[Space]
		public Color TeamColor = Color.white;

		private Timer _timer;
		private Rigidbody2D _rigidbody;
		private SpriteRenderer _spriterenderer;

		private Vector2 _targetPosition;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_spriterenderer = GetComponent<SpriteRenderer>();
			_timer = new Timer(Lifetime);
			
			_timer.OnTimerEnd += EndOfLifeTime;

		}

		private void Start()
		{
			_spriterenderer.color = TeamColor;
		}

		private void Update()
		{
			_targetPosition = transform.up * Speed * Time.deltaTime;
			_rigidbody.position += _targetPosition;

			_timer.Tick(Time.deltaTime);
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
			
			if (projectileCollider == null) { return false; }

			Entity entity = collider.GetComponent<Entity>();
			if (entity != null)
			{
				foreach (var element in WhitelistType)
				{
					if (element == entity.Type) { return false; }
				}

				if (entity.Type == Shooter) { return false; }
			}

			return true;
		}

		private void EndOfLifeTime() => Destroy(gameObject);
	}
}
