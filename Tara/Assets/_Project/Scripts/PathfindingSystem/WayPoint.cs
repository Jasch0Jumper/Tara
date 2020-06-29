﻿using UnityEngine;

namespace Tara.PathfindingSystem
{
	[RequireComponent(typeof(Collider2D))]
	public class WayPoint : MonoBehaviour
	{
		public float HitboxRadius;
		private CircleCollider2D _hitbox;

		public bool Active = true;

		public Vector3 Position;

		private void Awake()
		{
			_hitbox = GetComponent<CircleCollider2D>();
		}

		private void Start()
		{
			_hitbox.radius = HitboxRadius;
			//transform.position = position;
		}
	}
}