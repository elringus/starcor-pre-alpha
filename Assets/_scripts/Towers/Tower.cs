using UnityEngine;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
    #region Definition
    [HideInInspector]
	public Transform Transform;
    [HideInInspector]
	public GameObject GameObject;
    
    public GameObject Prototype;
    public float RechargeCD;
    protected float RechargeTimer;

    protected bool InProcess { get; set; }
    #endregion
    #region Unity
    protected virtual void Awake()
    {
        Transform = transform;
        GameObject = gameObject;
        InProcess = false;
    }

    protected virtual void Start()
	{

	}

    protected virtual void Update() 
	{
        if (RechargeTimer > 0)
            RechargeTimer -= Time.deltaTime;
        else
            RechargeTimer = 0;
	}
    #endregion
    #region Targeting
    public virtual void Targeting(TargetingType targetingType)
    {
        switch (targetingType)
        {
            case TargetingType.Start:
                StartTargeting();
                break;
            case TargetingType.InProcess:
                if (InProcess)
                    OnTargeting();
                break;
            case TargetingType.Finish:
                FinishTargeting();
                break;
            case TargetingType.None:
                if (InProcess)
                    CancelTargeting();
                break;
        }
    }

    protected abstract void StartTargeting();
    protected abstract void OnTargeting();
    protected abstract void FinishTargeting();
    protected abstract void CancelTargeting();
    protected virtual void Produce()
    {
        RechargeTimer = RechargeCD;
    }
    #endregion
    #region Properties
    public virtual bool Ready
    {
        get
        {
            return RechargeTimer == 0;
        }
    }

    public virtual float Progress
    {
        get
        {
            return 1 - RechargeTimer / RechargeCD;
        }
    }
    #endregion


}