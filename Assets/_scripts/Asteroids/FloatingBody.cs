using UnityEngine;
using System.Collections;

public class FloatingBody : MonoBehaviour, IAttackable
{
    [HideInInspector]
	public Transform Transform;
    [HideInInspector]
	public Rigidbody Rigidbody;

    public GameObject[] Shards;

    const float G = 10.0f;

    private float LifeTime = 300;

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

    public float CurrHP
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

        Destroy(gameObject, LifeTime);
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
        CurrHP -= attack.Damage;
    }

    public void Death()
    {
        CreateChildrens();
        ((GameObject)Instantiate(VFX[UnityEngine.Random.Range(0, VFX.Length)], Transform.position + new Vector3(0, 0, 0), Quaternion.identity)).transform.localScale = Transform.localScale * 4;
        Destroy(gameObject);
    }

    private void CreateChildrens()
    {
        if (Shards.Length == 0) return;

        for (int i = 0; i < Shards.Length; i++) 
			Instantiate(Shards[i], Transform.position, Quaternion.identity);
    }
}