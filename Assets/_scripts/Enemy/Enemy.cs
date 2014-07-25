using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour, IAttackable
{
	[HideInInspector]
	public Transform Transform;
	[HideInInspector]
	public GameObject GameObject;

	public GameObject Projectile;

	public float CD;

	private float timer;

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
	public float Damage;
	public float HitDistance;

    public OwnType OwnType = OwnType.Allien;

    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

	public NavMeshAgent navMesh;
	public Planet targetPlanet;

	protected virtual void Awake () 
	{
		Transform = transform;
		GameObject = gameObject;

		navMesh = GetComponent<NavMeshAgent>();
		targetPlanet = GameObject.Find("planet").GetComponent<Planet>();
	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update () 
	{
		navMesh.SetDestination(targetPlanet.Transform.position);

		timer = timer > 0 ? timer - Time.deltaTime : 0;

		if (Vector3.Distance(Transform.position, targetPlanet.Transform.position) <= HitDistance && timer == 0) Shoot();
	}

	private void Shoot ()
	{
		timer = CD;

		var projectile = (Instantiate(Projectile, Transform.position, Quaternion.identity) as GameObject).GetComponent<Projectile>();
		projectile.AimType = AimType.Foe;
		projectile.OwnType = OwnType.Allien;
		projectile.Direction = (targetPlanet.transform.position - Transform.position).normalized;
	}

	public virtual void RecieveAtatck (Attack attack)
	{
        HP -= attack.Damage;
	}

	protected virtual void Death ()
	{
		Destroy(GameObject);
	}
}