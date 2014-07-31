using UnityEngine;
using System.Collections;

public abstract class Attacker : MonoBehaviour, IAttackable
{
    #region Definition
    [HideInInspector]
	public Transform Transform;
	[HideInInspector]
	public GameObject GameObject;

	public GameObject Projectile;

	public float RechargeCD;

	private float timerCD;

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
	public float Range;

    public OwnType OwnType;

    public OwnType GetOwnType
    {
        get
        {
            return OwnType;
        }
    }

    private Attack attack;
	public NavMeshAgent navAgent;

	public GameObject VFXDeath;

	protected Transform mainTarget;
    protected IAttackable currTarget;
    #endregion
    protected virtual void Awake () 
	{
		Transform = transform;
		GameObject = gameObject;
        attack = new Attack(Damage, OwnType, AimType.Foe);
		navAgent = GetComponent<NavMeshAgent>();

        if (OwnType == OwnType.Allien)
            mainTarget = GameObject.Find("planet").transform;
	}

	protected virtual void Start ()
	{

	}

    protected virtual void Update()
    {
        if (navAgent != null)
            navAgent.SetDestination(mainTarget.position);

        timerCD = timerCD > 0 ? timerCD - Time.deltaTime : 0;

        if (timerCD == 0)
            if (FindTarget())
                Shoot();
    }

    protected virtual bool FindTarget()
    {
        IAttackable possibleTarget = null;
        IAttackable target = null;
        bool miss = true;
        foreach (var hit in Physics.OverlapSphere(transform.position, Range))
        {
            target = attack.CanAttack(hit.transform);
            if (target != null)
            {
                if (currTarget == null)
                {
                    currTarget = target;
                    miss = false;
                    break;
                }
                else
                {
                    if (currTarget == target)
                    {
                        miss = false;
                        break;
                    }
                    else
                        if (possibleTarget == null)
                            possibleTarget = target;
                }
            }
        }

        if (miss)
            currTarget = possibleTarget;

        return currTarget != null;

        //bool miss = true;
        //foreach (var hit in Physics.OverlapSphere(transform.position, Range))
        //{
        //    if (currTarget == null)
        //    {
        //        currTarget = attack.CanAttack(hit.transform);
        //        miss = false;
        //    }
        //    else
        //        if (currTarget == attack.CanAttack(hit.transform))
        //            miss = false;
        //}

        //if (miss)
        //    currTarget = null;

        //return !miss;
    }

	protected virtual void Shoot ()
	{
		timerCD = RechargeCD;
		var projectile = (Instantiate(Projectile, Transform.position, Quaternion.identity) as GameObject).GetComponent<Projectile>();
        projectile.attack = new Attack(Damage, OwnType, AimType.NonFriend);
        projectile.Transform.LookAt(((MonoBehaviour)currTarget).transform.position);
        projectile.Direction = new Vector3(0, 0, 1);
	}

	public virtual void RecieveAtatck (Attack attack)
	{
        HP -= attack.Damage;
	}

	protected virtual void Death ()
	{
		if (VFXDeath) Instantiate(VFXDeath, Transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
		Destroy(GameObject);
	}
}