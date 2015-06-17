using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public GameObject plane;
	public int width = 10;
	public int height = 10;
	public GameObject goHit;
	public GameObject Player;
	public AudioClip collect;
	//public RaycastHit hit;
	//public Ray ray;

	private GameObject[,] grid;

	void Awake () {
		grid = new GameObject[width, height];

		for (int x = 0; x < width; x ++) {
			for(int z = 0; z < height; z++){
				GameObject gridPlane = (GameObject)Instantiate (plane);
				gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x, gridPlane.transform.position.y, gridPlane.transform.position.z + z);
				grid[x,z] = gridPlane;
			}
		}
	}

	void onGUI(){
		if (GUI.Button (new Rect (10, 10, 150, 100), "Delete grid [3,3]"))
			Destroy (grid [3, 3]);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0))
		{
			goHit = null;
			//var hit = new RaycastHit();
			//var ray = new Ray();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100))
			{  
				if(hit.collider.name == "Plane(Clone)")
				{
					goHit = hit.collider.gameObject;
					AudioSource.PlayClipAtPoint(collect, transform.position);
					Player.transform.position = goHit.transform.position;
					Destroy (goHit);
				}
			}
		}
	}
}
