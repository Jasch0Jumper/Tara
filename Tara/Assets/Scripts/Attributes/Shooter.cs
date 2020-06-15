using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Entity), typeof(IShootInput))]
public class Shooter : MonoBehaviour
{
	Entity entity;
	IShootInput shootInput;

	//public int ID;
	[SerializeField] private List<Gun> guns = new List<Gun>();

	private void Awake()
	{
		entity = GetComponent<Entity>();
		shootInput = GetComponent<IShootInput>();
	}
	private void Start()
	{
		if (guns.Count != 0)
		{
			foreach (var gun in guns)
			{
				//gun.ID = ID;
				gun.shooterType = entity.Type;
			}
		}
	}

	private void Update()
	{
		if (shootInput.IsShooting())
		{
			foreach (var gun in guns)
			{
				gun.GiveShootInput();
			}
		}
	}
}
