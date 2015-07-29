using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public GameObject plane;
	public GameObject beacon;
	public int width = 10;
	public int height = 10;
	public GameObject goHit;
	public GameObject Player;
	public AudioClip collect;
	public Camera cam;
	//public RaycastHit hit;
	//public Ray ray;

	private GameObject[,] grid;
	private Vector3 position;

	void Awake () {
		grid = new GameObject[width, height];

		for (int x = 0; x < width; x ++) {
			for(int z = 0; z < height; z++){
				GameObject gridPlane = (GameObject)Instantiate (plane);
				gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x, gridPlane.transform.position.y, gridPlane.transform.position.z + z);
				grid[x,z] = gridPlane;
			}
		}

		position = new Vector3(Random.Range (0, width), 0, Random.Range (0, height));
		Instantiate (beacon, position, Quaternion.identity);

	}

	/*void onGUI(){
		if (GUI.Button (new Rect (10, 10, 150, 100), "Delete grid [3,3]"))
			Destroy (grid [3, 3]);
	}*/

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
				if(hit.collider.name == "Beacon(Clone)")
				{
					goHit = hit.collider.gameObject;
					AudioSource.PlayClipAtPoint(collect, transform.position);
					Player.transform.position = goHit.transform.position;
					Destroy (goHit);
					GameObject[] planes = GameObject.FindGameObjectsWithTag("Plane");
					foreach (GameObject target in planes)
					{
						if (target.transform.position.x < goHit.transform.position.x + 2 && target.transform.position.x > goHit.transform.position.x - 2 && target.transform.position.z < goHit.transform.position.z + 2 && target.transform.position.z > goHit.transform.position.z - 2)
						GameObject.Destroy (target);
					}
				}
			}
		}

		if (Input.GetMouseButtonDown (1))
		{
			position = new Vector3(Random.Range (0, width), 0, Random.Range (0, height));
			Instantiate (beacon, position, Quaternion.identity);
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			width = width+1;
			height = height+1;

			GameObject[] planes = GameObject.FindGameObjectsWithTag("Plane");
			foreach (GameObject target in planes)
				{
				GameObject.Destroy (target);
				}

			GameObject[] beacons = GameObject.FindGameObjectsWithTag("Beacon");
			foreach (GameObject target2 in beacons)
			{
				GameObject.Destroy (target2);
			}

			grid = new GameObject[width, height];

			for (int x = 0; x < width; x ++) {
				for(int z = 0; z < height; z++){
					GameObject gridPlane = (GameObject)Instantiate (plane);
					gridPlane.transform.position = new Vector3(gridPlane.transform.position.x + x, gridPlane.transform.position.y, gridPlane.transform.position.z + z);
					grid[x,z] = gridPlane;
				}
			}
			cam.transform.position = new Vector3(cam.transform.position.x + .50f, cam.transform.position.y + .75f, cam.transform.position.z + .25f);
		}
	}
}
