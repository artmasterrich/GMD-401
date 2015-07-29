using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {

	// Is this a mine?
	public bool mine;

	// Is it still covered?
	public bool isCovered() {
		return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
	}

	// Different Textures
	public Sprite[] emptyTextures;
	public Sprite mineTexture;
	public Sprite flagTexture;

	// Use this for initialization
	void Start () {
		// Randomly decide if it's a mine or not
		mine = Random.value < 0.15;

		// Register in Grid
		int x = (int)transform.position.x;
		int y = (int)transform.position.y;
		Grid.elements[x, y] = this;
	}

	// Load another texture
	public void loadTexture(int adjacentCount) {
		if (mine)
			GetComponent<SpriteRenderer>().sprite = mineTexture;
		else
			GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
	}

	void OnMouseUpAsButton() {
		// It's a mine
		if (mine) {
			// Uncover all mines
			Grid.uncoverMines();
			
			// game over
			print("you lose");
		}
		// It's not a mine
		else {
			// show adjacent mine number
			int x = (int)transform.position.x;
			int y = (int)transform.position.y;
			loadTexture(Grid.adjacentMines(x, y));
			
			// uncover area without mines
			Grid.FFuncover(x, y, new bool[Grid.w, Grid.h]);
			
			// find out if the game was won now
			if (Grid.isFinished())
				print("you win");
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			GetComponent<SpriteRenderer> ().sprite = flagTexture;
			//loadTexture (0);
		}
	}
}
