using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Trajectory : MonoBehaviour
{
	#region SINGLETON
	private static Trajectory _instance;
	public static Trajectory I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(Trajectory)) as Trajectory;
			return _instance;
		}
	}


	private void OnApplicationQuit () { _instance = null; }
	#endregion

    public float MinDistance = 50;

    public GameObject Rocket;

    private List<Vector3> Points { get; set; }
    private LineRenderer lRenderer;
    private int maxLRC = 200;
    private int currLRC = 0;

	private void Awake () 
	{
        Points = new List<Vector3>();
	}

    private void Start()
    {
        StartCoroutine(GenPoints());
       
        //Line Renderer
        lRenderer = GetComponent<LineRenderer>();
        lRenderer.enabled = false;
        lRenderer.SetVertexCount(maxLRC);
    }

	private void Update () 
	{
        
	}

    private bool inConstruction = false;
    private IEnumerator GenPoints()
    {
        while (true)
        {

            if (Input.GetMouseButton(0))
            {

                if (!inConstruction)
                {
                    Refresh();
                    inConstruction = true;
                }
                var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                if (Points.Count < maxLRC)
                    AddVertex(new Vector3(p.x, 0, p.z));
            }

            if (Input.GetMouseButtonUp(0))
            {
                inConstruction = false;
                CreateRocket();
            }

            yield return new WaitForSeconds(0.002f);
        }
    }

    private void AddVertex(Vector3 p)
    {
        if (Points.Count==0||Vector3.Distance(Points.Last(), p) > MinDistance)
        {
            Points.Add(p);
            lRenderer.SetPosition(currLRC, p);
            currLRC++;
        }

        for (int i = currLRC; i < maxLRC; i++)
            lRenderer.SetPosition(i, p);
    }
   
    private void CreateRocket()
    {
        var rocky = ((GameObject)Instantiate(Rocket, Points[0], Quaternion.identity));
        rocky.GetComponent<Rocket>().SetWaypoints(Points);
        
    }

    private void Refresh()
    {
        Points.Clear();
        lRenderer.enabled = true;
        currLRC = 0;
        //lRender.set
    }
}