using System.Collections.Generic;
using UnityEngine;

namespace Tara.Combat
{
	[RequireComponent(typeof(Entity))]
	public class Shooter : MonoBehaviour
	{
		private Entity _entity;

		//public int ID;
		[SerializeField] private List<Weapon> weapons = new List<Weapon>();

		private void Awake()
		{
			_entity = GetComponent<Entity>();
		}

		private void Start()
		{
			if (weapons.Count != 0)
			{
				foreach (var gun in weapons)
				{
					gun.ShooterType = _entity.Type;
				}
			}
		}

		public void Shoot()
		{
			foreach (var weapon in weapons)
			{
				weapon.Shoot();
			}
		}
	}
}
