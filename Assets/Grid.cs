using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	
	Node[,] grid; // 2D array of nodes
	
	// Use gizmos to visualize
	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		
	
	
	}
		
	
}
