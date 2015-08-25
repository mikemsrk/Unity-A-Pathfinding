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
		
		// Create Open and Closed set (visited, not visited / can't be visited)
		// Use hash table
		// Open set needs to be able to search for lowest f cost = expensive operation
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		// Add start node to Open
		openSet.Add (startNode);

		// Loop
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
			
			// Found - Base case
			if(currentNode == targetNode){
				RetracePath(startNode,targetNode);
				return;
			}
			
			// for each neighbor of the current
			foreach(Node neighbour in grid.GetNeighbours(currentNode)){
				// if neighbor is unwalkable or closed
				if(!neighbour.walkable || closedSet.Contains (neighbour)){
					// skip to next neighbor	
					continue;
				}		
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode,neighbour);
				// if new path to neighbor is shorter or is not in open
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)){
					// set costs of neighbor
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour,targetNode);
					// set parent of neighbor to current
					neighbour.parent = currentNode;
					// if neighbor is not in open
					if(!openSet.Contains (neighbour)){
						// add neighbor to open
						openSet.Add (neighbour);
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
		
		// Visualize path
		grid.path = path;
	}
	
	int GetDistance(Node nodeA, Node nodeB){
		// 10 for Left,Right,Up,Down
		// 14 for Diagonal moves
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		
		// [14,10,14]
		// [10, 0,10]
		// [14,10,14]
		
		// X is greater than Y
		if(distX > distY){
			return 14 * distY + 10 * (distX - distY);
		}else{ // Y is greater than X
			return 14 * distX + 10 * (distY - distX);
		}
	}
}
