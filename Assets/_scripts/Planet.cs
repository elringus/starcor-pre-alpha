using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour, IAttackable
{
	public Transform Transform;
	public float HP;
    public OwnType OwnType = OwnType.Terran; 

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
	}

	private void Update () 
	{
    	
	}

	private void OnGUI ()
	{
		GUILayout.Box("Planet HP: " + HP);
	}

    public void RecieveAtatck(Attack attack)
    {
        HP -= attack.Damage;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<FloatingBody>())
        {
            HP -= col.rigidbody.mass * 50;
            col.GetComponent<FloatingBody>().Death();
        }
    }
}