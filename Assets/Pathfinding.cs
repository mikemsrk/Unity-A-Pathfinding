using UnityEngine;
using System.Collections;
using System.Collections.Generic; // needed for List

public class Pathfinding : MonoBehaviour {

	Grid grid;
	public Transform seeker, target;
	
	void Awake(){
		grid = GetComponent<Grid>();
	}
	
	void Update(){
		FindPath (seeker.position,target.position);
	}
	
	void FindPath(Vector3 startPos, Vector3 endPos){
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(endPos);
		
		// Open set needs to be able to search for lowest f cost = expensive operation
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		// Add start node to Open
		openSet.Add (startNode);

		// Main Loop
		// From starting node --
		// Search "open" for lowest F cost node
		// Travel to best node and add it to "closed"
			// --> If reached end, retrace path and return
		// Search all neighbors of current node
			// Calculate g & h costs for each neighbor
				// g cost might change depending on previously found nodes
				// If lower g cost is found, replace it for that neighbor
				// set new h cost as well
			// Set neighbor's parent to current node = for retracing
			// Add neighbor to "open" if not already in for next iteration
			
		while(openSet.Count > 0){
			Node currentNode = openSet[0]; 
			
			// Search through open set, find lowest F node
			for(int i = 1; i < openSet.Count; i++){
				if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
					// Current = node in Open with lowest F cost
					currentNode = openSet[i];
				}
			}
			// Remove current from Open
			openSet.Remove (currentNode);
			// Add current to closed
			closedSet.Add (currentNode);
			
			// **Found** - Base case
			if(currentNode == targetNode){
				RetracePath(startNode,targetNode);
				return;
			}
			
			foreach(Node neighbor in grid.GetNeighbors(currentNode)){
				if(!neighbor.walkable || closedSet.Contains (neighbor)){
					// Don't bother with closed nodes
					continue;
				}		
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode,neighbor);
				if(newMovementCostToNeighbour < neighbor.gCost || !openSet.Contains (neighbor)){
					neighbor.gCost = newMovementCostToNeighbour;
					neighbor.hCost = GetDistance (neighbor,targetNode);
					neighbor.parent = currentNode;
					if(!openSet.Contains (neighbor)){
						openSet.Add (neighbor);
					}
				}
			}
		}
	}
	
	void RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while(currentNode != startNode){
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();
		
		// For visualization
		grid.path = path;
	}
	
	int GetDistance(Node nodeA, Node nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		
		// Use 14 for diagonal distance of a square = sqrt(n^2 + n^2) 
		
		// [14,10,14]
		// [10, 0,10]
		// [14,10,14]
		
		if(distX > distY){
			return 14 * distY + 10 * (distX - distY);
		}else{ 
			return 14 * distX + 10 * (distY - distX);
		}
	}
}
