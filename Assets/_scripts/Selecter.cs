using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Selecter : MonoBehaviour
{
    public Rocket SelectedRocket;
	private void Start ()
	{
		StartCoroutine(InputSampler());
	}

	private void Update () 
	{

	}

	private IEnumerator InputSampler ()
	{
		while (true)
		{
            var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            foreach(var hit in Physics.OverlapSphere(p,0.25f))
                if (hit.GetComponent<Rocket>())
                    SelectedRocket = hit.GetComponent<Rocket>();

			yield return null;
		}
	}
}