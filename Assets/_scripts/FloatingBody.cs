using UnityEngine;
using System.Collections;

public class FloatingBody : MonoBehaviour, IAttackable
{
    [HideInInspector]
	public Transform Transform;
    [HideInInspector]
	public Rigidbody Rigidbody;

    const float G = 2.0f;

    public OwnType OwnType = OwnType.None;

    private Vector3 StartVelocity;

    private Transform GravityCenter;

    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

    [SerializeField]
    private float _hp;

    public float HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            if (_hp <= 0) Death();
        }
    }

	private void Awake () 
	{
		Transform = transform;
		Rigidbody = rigidbody;
        GravityCenter = GameObject.Find("planet").transform;
        Transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        StartVelocity = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f)).normalized * Random.Range(1f, 2f);
	}

    private void Start()
    {
        Rigidbody.AddForce(StartVelocity, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //rigidbody.AddForce(new Vector3(1, 0, 1) * Time.deltaTime, ForceMode.Force);
        
        Vector3 gDirection = (GravityCenter.position - Transform.position).normalized;
        float gValue = G / (Vector3.Distance(GravityCenter.position, Transform.position).P(2));
        //float mForce=
        Rigidbody.AddForce(gDirection * gValue, ForceMode.Force);
    }

    public void RecieveAtatck(Attack attack)
    {
        if (attack.ThrowPower.magnitude != 0)
            Rigidbody.AddForce(attack.ThrowPower, ForceMode.Impulse);
    }

    private void Death()
    {
        Destroy(gameObject);
    }

}