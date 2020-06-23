using UnityEngine;

namespace Tara.AttackSystem
{
	public class RangeCollider : MonoBehaviour
	{
		public RangeType RangeType;
		
		public delegate void RangeChange(RangeType rangeType, Target target);
		public event RangeChange OnRangeEnter;
		public event RangeChange OnRangeExit;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Target target = collision.GetComponent<Target>();
			if (target != null)
			{
				OnRangeEnter?.Invoke(this.RangeType, target);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			Target target = collision.GetComponent<Target>();
			if (target != null)
			{
				OnRangeExit?.Invoke(this.RangeType, target);
			}
		}
	}
}
