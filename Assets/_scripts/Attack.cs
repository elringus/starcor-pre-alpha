using UnityEngine;

public class Attack
{
    public OwnType GetOwnType { get; set; }

    public AimType GetAimType { get; set; }
	public float Damage { get; set; }

    public Vector3 ThrowPower = Vector3.zero;

	public Attack (float damage, OwnType _ownType, AimType _aimType)
	{
		this.Damage = damage;
		this.GetOwnType = _ownType;
        this.GetAimType = _aimType;
	}

    public IAttackable CanAttack(IAttackable target)
    {
        bool canAttack = false;
        switch(GetAimType)
        {
            case AimType.All:
                canAttack = true;
                break;
            case AimType.Foe:
                canAttack = GetOwnType != target.GetOwnType && target.GetOwnType != OwnType.None;
                break;
            case AimType.NonFriend:
                canAttack = GetOwnType != target.GetOwnType;
                break;
        }

        if (canAttack)
            return target;
        else
            return null;
    }

    public IAttackable CanAttack(Transform transform)
    {
        if (transform == null)
            return null;

        if (transform.GetComponent(typeof(IAttackable)))
            return CanAttack(transform.GetComponent(typeof(IAttackable)) as IAttackable);
        else
            return null;
    }

    public bool MakeAttack(Transform transform)
    {
        var target = CanAttack(transform);
        if (target != null)
        {
            target.RecieveAtatck(this);
            return true;
        }
        else
            return false;
    }

    public bool MakeAttack(Transform transfrom, Vector3 throwPower, float customDamage)
    {
        ThrowPower = throwPower;
        Damage = customDamage;
        return MakeAttack(transfrom);
    }
}