using UnityEngine;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
    [HideInInspector]
	public Transform Transform;
    [HideInInspector]
	public GameObject GameObject;

    public GameObject Prototype;
    public float ColdDown;

    protected bool InProcess { get; set; }
    protected bool IsAborted { get; set; }
    protected virtual void Awake()
    {
        Transform = transform;
        GameObject = gameObject;
        InProcess = false;
        IsAborted = false;
    }
    protected virtual void Start()
	{

	}
    protected virtual void Update() 
	{
    	
	}

    public virtual void Targeting(TargetingType targetingType)
	{
        if(IsAborted)
        {
            StartTargeting();
            IsAborted = false;
        }
        else
            switch (targetingType)
            {
                case TargetingType.Start:
                    StartTargeting();
                    break;
                case TargetingType.InProcess:
                    OnTargeting();
                    break;
                case TargetingType.Finish:
                    FinishTargeting();
                    break;
            }
	}
    protected abstract void StartTargeting();
    protected abstract void OnTargeting();
    protected abstract void FinishTargeting();
}