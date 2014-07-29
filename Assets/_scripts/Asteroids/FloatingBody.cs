using UnityEngine;
using System.Collections;

public class FloatingBody : MonoBehaviour, IAttackable
{
    [HideInInspector]
	public Transform Transform;
    [HideInInspector]
	public Rigidbody Rigidbody;

    const float G = 50.0f;

    private float LifeTime = 100;

    private Transform GravityCenter;
    public GameObject[] VFX;

    public OwnType OwnType = OwnType.None;
    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

    public float MaxHP;

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
        float randomScale = Random.Range(0.15f, 0.65f);
        Transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        Rigidbody.mass = randomScale.P(3) * 8; ;
        Destroy(gameObject, 100);
	}

    public void Instantiate(Vector3 startVelocity)
    {
        Rigidbody.AddForce(startVelocity, ForceMode.VelocityChange);
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 gDirection = (GravityCenter.position - Transform.position).normalized;
        float gValue = G / (Vector3.Distance(GravityCenter.position, Transform.position).P(2)) * Time.fixedDeltaTime;
        Rigidbody.AddForce(gValue * gDirection , ForceMode.Acceleration);
    }

    public void RecieveAtatck(Attack attack)
    {
        if (attack.ThrowPower.magnitude != 0)
            Rigidbody.AddForce(attack.ThrowPower, ForceMode.Impulse);
        HP -= attack.Damage;
    }

    public void Death()
    {
        Destroy(gameObject);
        Instantiate(VFX[UnityEngine.Random.Range(0, VFX.Length)], Transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

}