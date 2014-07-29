using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] Prototypes;

    public float SpawnCD;

    public float MinRadius;
    public float MaxRadius;

    public float TargetRadius;
    private Vector3 center;
    
    private void Awake()
    {
        center = GameObject.Find("planet").transform.position;
    }
    
    private void Start()
    {
        StartCoroutine(Spawn(0));
    }

    private IEnumerator Spawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            Create();
            yield return new WaitForSeconds(Random.Range(SpawnCD - SpawnCD / 2f, SpawnCD + SpawnCD / 2f));
        }
    }

    private void Create()
    {
        float r = Random.Range(MinRadius, MaxRadius);
        Vector3 position = new Vector3(center.x + Mathf.Cos(Random.Range(0, Mathf.PI * 2)) * r, 0, center.z + Mathf.Sin(Random.Range(0, Mathf.PI * 2)) * r);
        Vector3 destination = new Vector3(center.x + Mathf.Cos(Random.Range(0, Mathf.PI * 2)) * TargetRadius, 0, center.z + Mathf.Sin(Random.Range(0, Mathf.PI * 2)) * TargetRadius);
        int n=Random.Range(0, Prototypes.Length);
        Vector3 startVelocity = (destination - position).normalized * 1;

        FloatingBody fBody = ((GameObject)Instantiate(Prototypes[n], position, Quaternion.identity)).GetComponent<FloatingBody>();
        fBody.Instantiate(startVelocity);
    }
}