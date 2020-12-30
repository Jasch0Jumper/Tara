using UnityEngine;
using UnityEngine.Events;

namespace Tara
{
	[RequireComponent(typeof(Collider2D))]
	public class Health : MonoBehaviour, ICanCollideWithProjectiles
	{
		[SerializeField] private int maxHealth = 1;
		public int Value { get; private set; } 

		[SerializeField] private UnityEvent onNoHealthLeft = new UnityEvent();

		private void Start()
		{
			Value = maxHealth;
		}

		private void Update()
		{
			if (Value <= 0)
			{
				onNoHealthLeft?.Invoke();
			}
		}

		public void Damage(int damage)
		{
			Value -= damage;
		}

		public void Heal(int heal)
		{
			Value += heal;
		}

		public void DefaultDeath()
		{
			Destroy(gameObject);
		}
	}
}