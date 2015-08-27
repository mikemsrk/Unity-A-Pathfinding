using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public Transform player;
	
	Node[,] grid;
	
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	
	void Start(){
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}
	
	void CreateGrid(){
		grid = new Node[gridSizeX,gridSizeY];
		
		// Find bottom left corner of grid, from center - x/2 - y/2
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
		
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				// Collision check between node and unwalkable layer
				bool walkable = !(Physics.CheckSphere (worldPoint,nodeRadius,unwalkableMask));
				// Create the new node at the XY position
				grid[x,y] = new Node(walkable,worldPoint,x,y);
			}
		}
	}
	
	public List<Node> GetNeighbors(Node node){
		List<Node> neighbors = new List<Node>();
		
		// Searching 3x3 block around the Node
		// [1,1,1]
		// [1,0,1]
		// [1,1,1]
		
		for(int x = -1; x <= 1; x++){
			for(int y = -1; y <= 1; y++){
				// Skip itself
				if(x == 0 && y == 0){
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				
				// Check for inbounds
				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbors.Add (grid[checkX,checkY]);
				}
			}
		}
		return neighbors;
	}
	
	// Returns the node from the world position by checking vs. grid
	public Node NodeFromWorldPoint(Vector3 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
		// Clamp it so if node position is outside of grid, it won't give you invalid position.
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);
		
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
		return grid[x,y];
	}
	
	public List<Node> path;
	
	// Use gizmos to visualize
	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));
		if(grid != null){
			Node playerNode = NodeFromWorldPoint(player.position);
			foreach (Node n in grid){
				// Vecto3.one = 1,1,1 (xyz)
				// Subtract .1f from diameter for separation.
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if(path != null){
					if(path.Contains (n)){
						Gizmos.color = Color.black;
					}
				}
				Gizmos.DrawCube (n.worldPosition,Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
		
	
}
