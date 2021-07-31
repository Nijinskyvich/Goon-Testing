// RandomPointOnNavMesh.cs
using UnityEngine;
using System.Collections;
public class PointFinder : MonoBehaviour
{
	//public float range = 10.0f;
	public bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 3; i++)
		{
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
	/*void Update()
	{
		Vector3 point;
		if (RandomPoint(transform.position, range, out point))
		{
			Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
		}
	}*/
}
