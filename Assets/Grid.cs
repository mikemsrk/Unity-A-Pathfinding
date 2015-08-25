using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	
	Node[,] grid; // 2D array of nodes
	
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
			// Use Vecto3.forward here because on 3D space we're looking for X and Z, not Y (which would be up)
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
		
		
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				// From bottom left -> (nodeSize*x) + (nodeSize*y)
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				// Collision check between node and layermask
				bool walkable = !(Physics.CheckSphere (worldPoint,nodeRadius,unwalkableMask));
				// Create the new node at the XY position
				grid[x,y] = new Node(walkable,worldPoint);
				
				
			}
		}
	
	
	}
	
	// Use gizmos to visualize
	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		
		if(grid != null){
			foreach (Node n in grid){
				// Vecto3.one = 1,1,1 (xyz)
				// Subtract .1f from diameter for accuracy.
				Gizmos.color = (n.walkable) ? Color.white : Color.red; 
				Gizmos.DrawCube (n.worldPosition,Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
		
	
}
