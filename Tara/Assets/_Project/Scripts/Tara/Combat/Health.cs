using UnityEngine;
using UnityEngine.Events;

namespace Tara
{
	[RequireComponent(typeof(Collider2D))]
	public class Health : MonoBehaviour
	{
		[SerializeField] private int maxHealth = 1;
		public int Value { get; private set; }

		public bool Invulnerable = false;

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
			if (Invulnerable) { return; }

			Value -= Mathf.Abs(damage);
		}

		public void Heal(int heal) => Value += Mathf.Abs(heal);

		public void DefaultDeath()
		{
			Destroy(gameObject);
		}
	}
}