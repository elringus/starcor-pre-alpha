using UnityEngine;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
	public Transform Transform;
	public GameObject GameObject;

	public virtual void Awake () 
	{
		Transform = transform;
		GameObject = gameObject;
	}

	public virtual void Start ()
	{

	}

	public virtual void Update () 
	{
    	
	}

	public virtual void Activate (TargetingType targetingType)
	{

	}
}