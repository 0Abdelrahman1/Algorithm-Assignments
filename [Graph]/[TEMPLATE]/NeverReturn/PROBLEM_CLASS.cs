using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Problem
{
	public static class PROBLEM_CLASS
	{
		public class Landmark
		{
			public int Id;
			public int X, Y;
			public bool IsInside;

			public Landmark(int id, int x, int y, bool isInside)
			{
				Id = id;
				X = x;
				Y = y;
				IsInside = isInside;
			}
        }


		#region YOUR CODE IS HERE
		//Your Code is Here:
		//==================
		/// <summary>
		/// Find the shortest path from "goerge" to any of the landmarks that is outside the Honor Stone 
		/// </summary>
		/// <param name="landmarks">list of Landmarks, each with Id, x, y, IsInside </param>
		/// <param name="trails">list of all trails, each consists of landmark1, landmark2, length</param>
		/// <param name="N">number of landmarks</param>
		/// <returns>value of the shortest path from goerge to outside </returns>
		/// 


		public class Graph
		{
			public class Node : IComparable<Node>
            {
				public enum Color { White, Grey, Black, DarkBlack }
				public Color state { get; set; }
				public int discoverTime { get; set; }
                public int finishTime { get; set; }
                public long sourceDistance { get; set; }
                public Landmark location { get; set; }
                public Node(Landmark location, Color state = Color.White, int discoverTime = -1, int finishTime = -1, long sourceDistance = int.MaxValue)
				{
					this.state = state; this.discoverTime = discoverTime; this.finishTime = finishTime; this.location = location; this.sourceDistance = sourceDistance;
				}
                public int CompareTo(Node other)
                {
                    if (other == null)
                        return 1;
                    return -(this.finishTime).CompareTo(other.finishTime);
                }
            }

			public class Edge
            {
                public Node node;
                public long cost;
				public Edge (Node node, long cost) { this.node = node; this.cost = cost; }
            }

			uint size;
			int timeStamp = 0;
			int src = 0;
			public List<Node> nodes = new List<Node>();
			public List<List<Edge>> adjacentEdges;

			public Graph(uint size, List<Landmark> landmarks, List<Tuple<int, int, int>> trails)
			{
				this.size = size;
				foreach (var loc in landmarks)
					nodes.Add(new Node(loc));
				nodes[src].sourceDistance = 0;

				adjacentEdges = FilterEdges(trails, size); // DAG edges
			}

			private List<List<Edge>> FilterEdges(List<Tuple<int, int, int>> trails, uint size)
			{
                List<List<Edge>> ret = new List<List<Edge>>();

				for (int i = 0; i < size; i++) ret.Add(new List<Edge>());

				int s = 0;
				Node node_s = nodes[s];
                foreach (var e in trails)
				{
					int u = e.Item1;
					int v = e.Item2;
					long cost = e.Item3;

					Node node_u = nodes[u];
					Node node_v = nodes[v];

					int dxu = node_u.location.X - node_s.location.X;
					int dyu = node_u.location.Y - node_s.location.Y;
					long du = (long)dxu * dxu + (long)dyu * dyu;

                    int dxv = node_v.location.X - node_s.location.X;
					int dyv = node_v.location.Y - node_s.location.Y;
					long dv = (long)dxv * dxv + (long)dyv * dyv;

					if (du > dv)
						ret[v].Add(new Edge(nodes[u], cost));
					else if (du < dv)
						ret[u].Add(new Edge(nodes[v], cost));
				}
				return ret;
			}

			public long ShortestPath_ToOutside()
			{
				Topological_Sort();
				DAG_Relaxation();

				long ret = int.MaxValue;
				foreach (var node in nodes)
					if (node.location.IsInside == false)
						ret = Math.Min(ret, node.sourceDistance);
				return ret;
            }
			private void Topological_Sort()
			{
				DFS(0);
				nodes.Sort();
			}
			private void DFS(int x)
			{
				nodes[x].state = Node.Color.Grey;
				nodes[x].discoverTime = timeStamp++;

				foreach (Edge e in adjacentEdges[x])
					if (e.node.state == Node.Color.White)
						DFS(e.node.location.Id);
				
				nodes[x].state = Node.Color.Black;
				nodes[x].finishTime = timeStamp++;
			}

			private void DAG_Relaxation()
			{
				foreach (var parent in nodes)
					foreach (var e in adjacentEdges[parent.location.Id])
						if (parent.sourceDistance + e.cost < e.node.sourceDistance)
							e.node.sourceDistance = parent.sourceDistance + e.cost;
			}

			#region Another Approch:
			public int AnotherApproch_ShortestPath_ToOutside()
			{
				Transpose();
                long ret = int.MaxValue;
                foreach (var node in nodes)
                    if (node.location.IsInside == false)
                        ret = Math.Min(ret, DP(node.location.Id));
                return (int)ret;
            }

			private void Transpose()
			{
                List<List<Edge>> adj = new List<List<Edge>>();

                for (int i = 0; i < size; i++) adj.Add(new List<Edge>());

				for (int u = 0; u < size; u++)
					foreach (var e in adjacentEdges[u])
						adj[e.node.location.Id].Add(new Edge(nodes[u], e.cost));

				adjacentEdges = adj;
            }
			private int DP(int x)
			{
				long ret = int.MaxValue;

				if (nodes[x].sourceDistance != int.MaxValue)
					return (int)nodes[x].sourceDistance;

				foreach (Edge e in adjacentEdges[x])
					ret = Math.Min(ret, e.cost + DP(e.node.location.Id));
				return (int)(nodes[x].sourceDistance = ret);
			}
            #endregion
        }


        public static int RequiredFunction(List<Landmark> landmarks, List<Tuple<int, int, int>> trails, int N)
		{

			Graph g = new Graph((uint)N, landmarks, trails);

			//return (int)Math.Min(g.AnotherApproch_ShortestPath_ToOutside(), g.ShortestPath_ToOutside());
			return (int)g.ShortestPath_ToOutside();
			//return (int)g.AnotherApproch_ShortestPath_ToOutside();
        }
        #endregion
    }

}
