using System.Collections.Generic;
using UnityEngine;

namespace Celeste.FSM.Utils
{
    public static class PathUtils
    {
        public static int CalculateShortestDistance(
            this XNode.NodeGraph nodeGraph,
            XNode.Node start,
            XNode.Node end)
        {
            Debug.Assert(start != null, $"Start is null in shortest distance calculation.");
            Debug.Assert(end != null, $"End is null in shortest distance calculation.");

            // Adjacency list for storing
            // which vertices are connected
            Dictionary<XNode.Node, List<XNode.Node>> adj = new Dictionary<XNode.Node, List<XNode.Node>>(nodeGraph.nodes.Count);
            
            foreach (var node in nodeGraph.nodes)
            {
                adj.Add(node, new List<XNode.Node>());
            }

            // Set up edges between nodes
            foreach (var node in nodeGraph.nodes)
            {
                foreach (var output in node.Outputs)
                {
                    for (int i = 0, n = output.ConnectionCount; i < n; ++i)
                    {
                        adj[node].Add(output.GetConnection(i).node);
                    }
                }
            }

            return CalculateShortestDistance(nodeGraph, adj, start, end);
        }

        private static int CalculateShortestDistance(
            XNode.NodeGraph nodeGraph,
            Dictionary<XNode.Node, List<XNode.Node>> adj,
            XNode.Node s, 
            XNode.Node dest)
        {
            // predecessor[i] array stores
            // predecessor of i and distance
            // array stores distance of i
            // from s
            Dictionary<XNode.Node, XNode.Node> pred = new Dictionary<XNode.Node, XNode.Node>(nodeGraph.nodes.Count);
            Dictionary<XNode.Node, int> dist = new Dictionary<XNode.Node, int>(nodeGraph.nodes.Count);

            if (BFS(adj, s, dest, pred, dist) == false)
            {
                Debug.LogError("Given source and destination are not connected");
                return int.MaxValue;
            }

            // List to store path
            List<XNode.Node> path = new List<XNode.Node>();
            XNode.Node crawl = dest;
            path.Add(crawl);

            while (pred.ContainsKey(crawl) && pred[crawl] != null)
            {
                path.Add(pred[crawl]);
                crawl = pred[crawl];
            }

            return dist[dest];
        }

        // a modified version of BFS that
        // stores predecessor of each vertex
        // in array pred and its distance
        // from source in array dist
        private static bool BFS(
            Dictionary<XNode.Node, List<XNode.Node>> adj,
            XNode.Node src,
            XNode.Node dest,
            Dictionary<XNode.Node, XNode.Node> pred,
            Dictionary<XNode.Node, int> dist)
        {
            // a queue to maintain queue of
            // vertices whose adjacency list
            // is to be scanned as per normal
            // BFS algorithm using List of int type
            List<XNode.Node> queue = new List<XNode.Node>();

            // bool array visited[] which
            // stores the information whether
            // ith vertex is reached at least
            // once in the Breadth first search
            Dictionary<XNode.Node, bool> visited = new Dictionary<XNode.Node, bool>();

            // now source is first to be
            // visited and distance from
            // source to itself should be 0
            visited[src] = true;
            dist[src] = 0;
            queue.Add(src);

            // bfs Algorithm
            while (queue.Count != 0)
            {
                XNode.Node u = queue[0];
                queue.RemoveAt(0);

                List<XNode.Node> adjacents = adj[u];
                for (int i = 0; i < adjacents.Count; i++)
                {
                    XNode.Node ithAdjacent = adjacents[i];
                    if (!visited.ContainsKey(ithAdjacent) || !visited[ithAdjacent])
                    {
                        visited[ithAdjacent] = true;
                        dist[ithAdjacent] = dist[u] + 1;
                        pred[ithAdjacent] = u;
                        queue.Add(ithAdjacent);

                        // stopping condition (when we
                        // find our destination)
                        if (ithAdjacent == dest)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}