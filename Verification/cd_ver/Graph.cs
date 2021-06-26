using System;
using System.Collections.Generic;

namespace Verification.cd_ver
{
	public class Graph
	{
		private Dictionary<string, List<string>> _adjacency;

		public Graph()
		{
			_adjacency = new Dictionary<string, List<string>>();
		}

		public void AddVertex(string vertexId1, string vertexId2)
		{
			if (!_adjacency.ContainsKey(vertexId1))
				_adjacency.Add(vertexId1, new List<string>() { vertexId2 });
			else
				_adjacency[vertexId1].Add(vertexId2);

			if (!_adjacency.ContainsKey(vertexId2))
				_adjacency.Add(vertexId2, new List<string>() { vertexId1 });
			else
				_adjacency[vertexId2].Add(vertexId1);
		}

		private bool IsCyclicUtil(string vertexId, Dictionary<string, bool> visited, string parentId)
		{
			visited[vertexId] = true;

			foreach (var vertex in _adjacency[vertexId])
			{
				if (!visited[vertex])
				{
					if (IsCyclicUtil(vertex, visited, vertexId))
						return true;
				}
				else if (vertex != parentId)
					return true;
			}

			return false;
		}

		public bool IsCycleExist()
		{
			var visited = new Dictionary<string, bool>();
			foreach (var vertex in _adjacency)
				visited.Add(vertex.Key, false);

			foreach (var vertex in visited)
				if (!vertex.Value)
					if (IsCyclicUtil(vertex.Key, visited, string.Empty))
						return true;

			return false;
		}
	}
}
