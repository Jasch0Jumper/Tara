using UnityEngine;

namespace Tara
{
	public class AIInput : MonoBehaviour
	{
		[SerializeField] [Range(1f, 5f)] private float fastSpeedMultiplier = 1f;
		[SerializeField] [Range(0.1f, 1f)] private float slowSpeedMultiplier = 1f;
		
		private IAIInput _aIInput;
		
		private Vector2 _input;
		private Vector2 _targetRotationPosition;

		private Vector3 _aiTargetPosition;
		private Vector3 _pathFindingTargetPosition;

		private void Awake()
		{
			_aIInput = GetComponent<IAIInput>();
		}

		private void Update()
		{
			if (_aIInput.UseFastSpeed()) 
			{ 
				//SpeedMultiplier = fastSpeedMultiplier; 
			}
			else if (_aIInput.UseSlowSpeed()) 
			{ 
				//SpeedMultiplier = slowSpeedMultiplier; 
			}
			else 
			{ 
				//SpeedMultiplier = 1f; 
			}

			_aiTargetPosition = _aIInput.GetTargetPosition();

			Vector2 targetInput;

			_targetRotationPosition = _pathFindingTargetPosition;
			targetInput = _pathFindingTargetPosition - transform.position;

			if (_aIInput.HasArrived())
			{ 
				_input = Vector2.zero; 
			}
			else
			{ 
				_input = Vector2.ClampMagnitude(targetInput, 1f); 
			}

			if (_aIInput.CanShoot()) 
			{ 
				//IsShooting = true; 
			}
			else 
			{ 
				//IsShooting = false; 
			}
		}

		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showTargetPoints = default;

		private void OnDrawGizmos()
		{
			if (showTargetPoints) { DrawTargetPoints(); }
		}
		private void OnDrawGizmosSelected()
		{
			DrawTargetPoints();
		}

		private void DrawTargetPoints()
		{
			if (_aiTargetPosition == Vector3.zero || _pathFindingTargetPosition == Vector3.zero) { return; }

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_aiTargetPosition, 2.5f);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(_pathFindingTargetPosition, 2.5f);
		}

#endif
		#endregion
	}
}
