using UnityEngine;
using System.Collections;

public class ObstacleThrower : MonoBehaviour
{
	public float ThrowPower;
    public float ExplosionRadius;
	private void Awake () 
	{
    	
	}

	private void Update () 
	{
    	
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.collider.CompareTag("Obstacle"))
        {
            var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (var hit in colliders)
                if (hit && hit.rigidbody)
                    hit.rigidbody.AddExplosionForce(ThrowPower, transform.position, ExplosionRadius, 0.5f, ForceMode.Impulse);
        }
    }
}