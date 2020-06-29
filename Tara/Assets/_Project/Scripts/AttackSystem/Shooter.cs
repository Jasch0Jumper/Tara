using System.Collections.Generic;
using UnityEngine;
using Tara.Core;
using Tara.InputSystem;

namespace Tara.AttackSystem
{
	[RequireComponent(typeof(Entity), typeof(IShootInput))]
	public class Shooter : MonoBehaviour
	{
		private Entity _entity;
		private IShootInput _shootInput;

		//public int ID;
		[SerializeField] private List<Gun> guns = new List<Gun>();

		private void Awake()
		{
			_entity = GetComponent<Entity>();
			_shootInput = GetComponent<IShootInput>();
		}

		private void Start()
		{
			if (guns.Count != 0)
			{
				foreach (var gun in guns)
				{
					//gun.ID = ID;
					gun.shooterType = _entity.Type;
				}
			}
		}

		private void Update()
		{
			if (_shootInput.IsShooting())
			{
				foreach (var gun in guns)
				{
					gun.GiveShootInput();
				}
			}
		}
	}
}
