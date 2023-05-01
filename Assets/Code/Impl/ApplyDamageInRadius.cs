using System.Collections;
using System.Collections.Generic;
using Purgatory.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class ApplyDamageInRadius : MonoBehaviour
{
	public float Radius = 5f;
	public int Damage = 5;

	[SerializeField] private float lifetime = 4f;
	[SerializeField] private float visualScaleAdditive;
	[SerializeField] private Transform visualTransform;

	private float creationTime;
	
	private void Start()
	{
		creationTime = Time.time;
		visualTransform.localScale = Vector3.one * (Radius + visualScaleAdditive);
		var colliders = Physics.OverlapSphere(transform.position, Radius, 1 << 6);
		foreach (var collider in colliders)
		{
			if (!collider.TryGetComponent(out ITargetable target))
				continue;
			target.Damage(Damage);
		}
	}

	private void Update()
	{
		if (Time.time - creationTime > lifetime)
			Destroy(gameObject);
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, Radius);
	}
}
