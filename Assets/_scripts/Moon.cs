using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour,IAttackable
{
	[HideInInspector]
	public Transform Transform;

	public Transform Earth;

	private void Awake () 
	{
		Transform = transform;
	}

	private void Update () 
	{
		Transform.RotateAround(Earth.position, Vector3.up, Time.deltaTime);
		Transform.Rotate(new Vector3(0, 2 * Time.deltaTime, 0));
	}
    #region IAttackable
    public OwnType OwnType = OwnType.None;

    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

    public void RecieveAtatck(Attack attack)
    {

    }
    #endregion
}