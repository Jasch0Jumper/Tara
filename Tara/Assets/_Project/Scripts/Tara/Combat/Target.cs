using UnityEngine;

namespace Tara.Combat
{
	[RequireComponent(typeof(Collider2D))]
	public class Target : MonoBehaviour
	{
		[SerializeField] private int priorityLevel = 0;
		public int PriorityLevel { get => priorityLevel; }

		private TargetManager _targetManager;

		private void Awake()
		{
			_targetManager = FindObjectOfType<TargetManager>();
		}

		private void OnEnable() => Register();

		private void OnDisable() => Unregister();

		private void Register()
		{
			if (_targetManager != null) { _targetManager.RegisterAsTargetAble(this); }
		}

		private void Unregister()
		{
			if (_targetManager != null) { _targetManager.UnregisterAsTargetAble(this); }
		}
	}
}