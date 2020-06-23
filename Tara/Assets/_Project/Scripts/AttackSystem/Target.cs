using UnityEngine;

namespace Tara.AttackSystem
{
	[RequireComponent(typeof(Collider2D))]
	public class Target : MonoBehaviour
	{
		[SerializeField] private int priorityLevel = 0;
		public int PriorityLevel { get => priorityLevel; }

		private TargetManager targetManager;

		private void Awake()
		{
			targetManager = FindObjectOfType<TargetManager>();
		}

		private void OnEnable() => Register();

		private void OnDisable() => Unregister();

		private void Register()
		{
			if (targetManager != null) { targetManager.RegisterAsTargetAble(this); }
		}

		private void Unregister()
		{
			if (targetManager != null) { targetManager.UnregisterAsTargetAble(this); }
		}
	}
}