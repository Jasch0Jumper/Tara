using UnityEngine;
using CI.Tools;

namespace Tara.Combat
{
	public class Weapon : MonoBehaviour
	{
		//public int ID;
		public EntityType ShooterType;
		[Space]
		[Range(1f, 500f)] public float projectileSpeed = 50f;
		[Space]
		[Range(1f, 100f)] public float projectileLifeTime = 10f;
		[Range(1, 1000)] public int projectileDamage = 10;
		[Space]
		[SerializeField] private Color projectileColor = Color.white;
		[Space]
		[Range(0.05f, 5f)] public float ReloadCooldown = default;
		public GameObject Projectile = default;

		private Timer _timer;

		[HideInInspector] public bool Shooting;

		private void Awake()
		{
			_timer = new Timer(ReloadCooldown);
		}

		private void Update()
		{
			_timer.Tick(Time.deltaTime);

			if (Shooting && IsCooldownOver())
			{
				InstantiateProjectile();
				_timer.Reset();
			}
		}

		private bool IsCooldownOver()
		{
			return _timer.RemainingSeconds <= 0f;
		}

		private void InstantiateProjectile()
		{
			var newProjectile = Instantiate(Projectile, transform);

			var newProjectileController = newProjectile.GetComponent<Projectile>();

			newProjectileController.Shooter = ShooterType;
			newProjectileController.Speed = projectileSpeed;
			newProjectileController.Damage = projectileDamage;
			newProjectileController.Lifetime = projectileLifeTime;
			newProjectileController.TeamColor = projectileColor;

			transform.DetachChildren();
		}
	}
}
