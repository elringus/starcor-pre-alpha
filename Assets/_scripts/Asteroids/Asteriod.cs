using UnityEngine;
using System.Collections;

public class Asteriod : MonoBehaviour, IAttackable
{
	public GameObject[] Prefabs;
	public Transform Transform;
	public Rigidbody Rigidbody;

    public OwnType OwnType = OwnType.None;

    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

	private void Awake () 
	{
		Transform = transform;
		Rigidbody = rigidbody;

		float randomScale = Random.Range(.001f, .2f);
		//Transform.localScale = new Vector3(randomScale, randomScale, randomScale);
		Transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
		Rigidbody.mass = randomScale * 20;
	}

    public void RecieveAtatck(Attack attack)
    {
        if (attack.ThrowPower.magnitude != 0)
            rigidbody.AddForce(attack.ThrowPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision col)
    {

    }


}