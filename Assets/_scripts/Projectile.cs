using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public GameObject GameObject;
	[HideInInspector]
	public Transform Transform;

    public GameObject[] VFX;

	public Vector3 Direction;
	public float LiveTime;

	public float Speed;

    public Attack attack;

	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

	private void Start ()
	{
		Destroy(GameObject, LiveTime);
	}

	private void Update () 
	{
		Transform.Translate(Direction * Time.deltaTime * Speed);
	}

	public void OnTriggerEnter (Collider col)
	{
        if (attack != null && attack.MakeAttack(col.transform))
        {
            if (VFX.Length > 0)
                Instantiate(VFX[UnityEngine.Random.Range(0, VFX.Length)], Transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(GameObject);
        }
	}
}