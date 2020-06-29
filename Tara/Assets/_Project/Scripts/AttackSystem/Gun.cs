using UnityEngine;
using Tara.Core;

namespace Tara.AttackSystem
{
	public class Gun : MonoBehaviour
	{
		//public int ID;
		public EntityType shooterType;

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

		private float cooldown;
		private bool shooting;

		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			if (useSpriteColor) { projectileColor = spriteRenderer.color; }
		}

		private void Update()
		{
			cooldown += Time.deltaTime;

			if (shooting && IsCooldownOver())
			{
				Shoot();
			}

			shooting = false;
		}

		public void GiveShootInput() => shooting = true;

		private bool IsCooldownOver()
		{
			if (cooldown == 0f) { return true; }
			if (cooldown >= shootingCooldown) { return true; }
			return false;
		}

		private void Shoot()
		{
			var newProjectile = Instantiate(projectile, transform);

			var newProjectileController = newProjectile.GetComponent<Projectile>();
			var projectileLifeTimer = newProjectile.GetComponent<Lifetime>();

			newProjectileController.shooter = shooterType;
			newProjectileController.speed = projectileSpeed;
			newProjectileController.damage = projectileDamage;
			newProjectileController.teamColor = projectileColor;

			if (projectileLifeTimer != null) { projectileLifeTimer.time = projectileLifeTime; }

			transform.DetachChildren();

			cooldown = 0f;
		}
	}
}