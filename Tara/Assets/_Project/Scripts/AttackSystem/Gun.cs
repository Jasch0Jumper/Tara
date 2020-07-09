using UnityEngine;
using Tara.Core;

namespace Tara.AttackSystem
{
	public class Gun : MonoBehaviour
	{
		//public int ID;
		public EntityType ShooterType;

		[Space]
		[SerializeField] [Range(1f, 500f)] private float projectileSpeed = 50f;
		[Space]
		[SerializeField] [Range(1f, 100f)] private float projectileLifeTime = 10f;
		[SerializeField] [Range(1, 1000)] private int projectileDamage = 10;
		[Space]
		[SerializeField] private bool useSpriteColor = default;
		[Tooltip("Can be ignored if useSpriteColor is true.")]
		[SerializeField] private Color projectileColor = Color.white;
		[Space]
		[SerializeField] [Range(0.05f, 5f)] private float shootingCooldown = default;
		[SerializeField] private GameObject projectile = default;

		private float _cooldown;
		private bool _shooting;

		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			if (useSpriteColor) { projectileColor = _spriteRenderer.color; }
		}

		private void Update()
		{
			_cooldown += Time.deltaTime;

			if (_shooting && IsCooldownOver())
			{
				Shoot();
			}

			_shooting = false;
		}

		public void GiveShootInput() => _shooting = true;

		private bool IsCooldownOver()
		{
			if (_cooldown == 0f) { return true; }
			if (_cooldown >= shootingCooldown) { return true; }
			return false;
		}

		private void Shoot()
		{
			var newProjectile = Instantiate(projectile, transform);

			var newProjectileController = newProjectile.GetComponent<Projectile>();
			var projectileLifeTimer = newProjectile.GetComponent<Lifetime>();

			newProjectileController.Shooter = ShooterType;
			newProjectileController.Speed = projectileSpeed;
			newProjectileController.Damage = projectileDamage;
			newProjectileController.TeamColor = projectileColor;

			if (projectileLifeTimer != null) { projectileLifeTimer.Time = projectileLifeTime; }

			transform.DetachChildren();

			_cooldown = 0f;
		}
	}
}