using UnityEngine;

namespace Tara
{
	[ExecuteInEditMode]
	public class FollowPlayer : MonoBehaviour
	{
		private Transform _player;
		[SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

		private void Start()
		{
			_player = GameObject.FindWithTag("Player").transform;
			if (_player == null)
			{ Debug.LogError("Player not found"); }
		}

		private void Update()
		{
			if (_player != null)
			{
				transform.position = _player.position + offset;
			}
		}
	}
}