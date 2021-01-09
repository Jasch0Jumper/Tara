using UnityEngine;
using CITools;

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
		[SerializeField] private bool useSpriteColor = default;
		[Tooltip("Can be ignored if useSpriteColor is true.")]
		[SerializeField] private Color projectileColor = Color.white;
		[Space]
		[Range(0.05f, 5f)] public float ReloadCooldown = default;
		public GameObject Projectile = default;

		private Timer _timer;

		private bool _shooting;

		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();

			_timer = new Timer(ReloadCooldown, true);
			_timer.OnTimerEnd += InstantiateProjectile;
		}

		private void Start()
		{
			if (useSpriteColor) { projectileColor = _spriteRenderer.color; }
		}

		private void Update()
		{
			_timer.Tick(Time.deltaTime);

			if (_shooting && IsCooldownOver())
			{
				InstantiateProjectile();
			}

			_shooting = false;
		}

		public void Shoot()
		{
			_shooting = true; 
			
		}

		private bool IsCooldownOver()
		{
			//if (_cooldown == 0f) { return true; }
			//if (_cooldown >= ReloadCooldown) { return true; }

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

			//_cooldown = 0f;
		}
	}
}