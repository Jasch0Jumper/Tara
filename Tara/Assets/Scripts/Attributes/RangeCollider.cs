using UnityEngine;

public class RangeCollider : MonoBehaviour
{
	public RangeType RangeType;
	public TargetSelector TargetSelector;

	public delegate void RangeChange(RangeType rangeType, Target target);
	public static event RangeChange OnRangeEnter;
	public static event RangeChange OnRangeExit;

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
