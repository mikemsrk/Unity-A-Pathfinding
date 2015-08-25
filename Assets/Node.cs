using UnityEngine;
using System.Collections;

public class Node {

	public bool walkable;
	public Vector3 worldPosition;
	public int gridX; // let it remember its location in grid
	public int gridY;
	
	public int gCost; // movement cost from starting node
	public int hCost; // movement cost from ending node
	public Node parent;
	
	// Constructor
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY){
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}
	
	public int fCost {
		get{
			return gCost + hCost; // never assign F cost, get it by adding
		}
	}
	
}
