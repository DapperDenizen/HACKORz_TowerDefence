using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//THIS WAS MADE USING A YOUTUBE TUTORIAL BY SABASTIAN LAGUE- https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ - specifically the A* Pathfinding tutorial, i do not own this code, however i may have modified it.


public class PathRequestManager : MonoBehaviour {
	
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest> ();
	PathRequest currentPathRequest;

	public static PathRequestManager instance;
	[SerializeField] Pathfinding pathfinding;
	bool isProcessingPath;

	void Awake(){

		instance = this;
		//pathfinding = GetComponent<Pathfinding> ();
	}

	//request path
	public static void RequestPath(Vector2Int pathStart, Vector2Int pathEnd, BoardNode[,] inGrid,Action<Vector2Int[],bool> callback){
		PathRequest newRequest = new PathRequest (pathStart, pathEnd, inGrid, callback);
        
		instance.pathRequestQueue.Enqueue (newRequest);
		instance.TryProcessNext ();

	}

	void TryProcessNext(){
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue ();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.pathStart,  currentPathRequest.pathEnd, currentPathRequest.grid, currentPathRequest.callback);
		}
	
	}
	public void FinishedProcessingPath(Vector2Int[] path, bool success){
	
		currentPathRequest.callback (path, success);
		isProcessingPath = false;
		TryProcessNext ();
	}

	struct PathRequest {

		public Vector2Int pathStart;
		public Vector2Int pathEnd;
		public Action<Vector2Int[],bool> callback;
        public BoardNode[,] grid;


        public PathRequest(Vector2Int pathStart, Vector2Int pathEnd, BoardNode[,] grid, Action<Vector2Int[],bool> callback){
			this.pathStart = pathStart;
			this.pathEnd = pathEnd;
			this.callback = callback;
            this.grid = grid;
		}
	}

}
