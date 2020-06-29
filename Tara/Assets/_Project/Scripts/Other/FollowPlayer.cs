using UnityEngine;

namespace Tara
{
	[ExecuteInEditMode]
	public class FollowPlayer : MonoBehaviour
	{
		private Transform player;

		private void Start()
		{
			player = GameObject.FindWithTag("Player").transform;
			if (player == null)
			{ Debug.LogError("Player not found"); }
		}

		private void Update()
		{
			if (player != null)
			{
				transform.position = player.position + new Vector3(0f, 0f, -10f);
			}
		}
	}
}