using UnityEngine;
using Tara.AI.MovementStates;
using Tara.Pathfinding;
using System.Collections.Generic;

namespace Tara.AI
{
	[RequireComponent(typeof(Movement), typeof(PathFinderBehavior))]
	public class AIMovement : StateMachine<AIMovement>
	{
		[SerializeField] [Range(30f, 100f)] private float viewDistance;
		[SerializeField] [Range(0f, 360f)] private float fieldOfView;
		[SerializeField] [Range(1f, 45f)] private float spacing;
		[SerializeField] private LayerMask layerMask;

		public Movement Movement { get; private set; }
		public PathFinderBehavior PathFinder { get; private set; }

		public Vector3 TargetPosition { get; set; }

		protected override State<AIMovement> DefaultState { get => new Idle(this); }

		private void Awake()
		{
			Movement = GetComponent<Movement>();
			PathFinder = GetComponent<PathFinderBehavior>();
		}

		private void Start()
		{
			SwitchToDefaultState();
		}

		private void Update()
		{
			State.Update();
		}

		public List<RaycastHit2D> CheckSoroundings()
		{
			var hitInfos = new List<RaycastHit2D>();

			var vectors = GetFieldOfViewVectors();

			if (vectors.Count < 1) return hitInfos;

			foreach (var vector in vectors)
			{
				var	hitInfo = Physics2D.Raycast(transform.position, vector, viewDistance, layerMask);
				
				if (hitInfo.collider == null) continue;
				
				hitInfos.Add(hitInfo);
			}

			return hitInfos;
		}

		private List<Vector2> GetFieldOfViewVectors()
		{
			var directions = new List<Vector2>();

			var offsetAngel = -(fieldOfView - 180f) / 2f;

			var vectorCount = Mathf.RoundToInt(fieldOfView / spacing);

			directions.Add(GetHeadingVector(90f.RotateWith(this)));

			for (int i = 0; i < (vectorCount / 2); i++)
			{
				var angel = i * spacing + offsetAngel;

				directions.Add(GetHeadingVector(angel.RotateWith(this)));
				directions.Add(GetHeadingVector((180f - angel).RotateWith(this)));
			}

			return directions;
		}

		private Vector2 GetHeadingVector(float angel)
		{
			var angelInRad = Mathf.Deg2Rad * angel;
			return new Vector2(Mathf.Cos(angelInRad), Mathf.Sin(angelInRad));
		}
		
		#region Gizmos
#if UNITY_EDITOR

		[Header("Gizmos")]
		[SerializeField] private bool showTargetPosition;
		[SerializeField] private bool showFOVRays;
		[SerializeField] private bool showHitMarkers;

		private void OnDrawGizmos()
		{
			if (showTargetPosition) DrawTargetPosition();
			if (showFOVRays) DrawFieldOfViewRays();
			if (showHitMarkers) DrawHitMarkers();
		}

		private void OnDrawGizmosSelected()
		{
			DrawTargetPosition();
			DrawFieldOfViewRays();
			DrawHitMarkers();
		}

		private void DrawTargetPosition()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(TargetPosition, 2f);
		}

		private void DrawFieldOfViewRays()
		{
			var vectors = GetFieldOfViewVectors();

			if (vectors.Count < 1) return;


			foreach (var vector in vectors)
			{
				var pos = (vector * viewDistance).RelativeTo(this);

				Gizmos.color = Color.white;
				Gizmos.DrawLine(transform.position, pos);
			}
		}

		private void DrawHitMarkers()
		{
			var hits = CheckSoroundings();
			
			foreach (var hit in hits)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(transform.position, hit.point.ToVector3(-0.1f));
				Gizmos.DrawSphere(hit.point, 0.75f);

				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, hit.transform.position);
				Gizmos.DrawSphere(hit.transform.position, 0.75f);
			}
		}
#endif
		#endregion

	}
}
