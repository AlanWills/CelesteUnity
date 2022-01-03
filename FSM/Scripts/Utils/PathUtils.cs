using System.Collections.Generic;
using UnityEngine;

namespace Celeste.FSM.Utils
{
    public static class PathUtils
    {
        public static int CalculateShortestDistance(
            this IFSMGraph nodeGraph,
            FSMNode start,
            FSMNode end)
        {
            Debug.Assert(start != null, $"Start is null in shortest distance calculation.");
            Debug.Assert(end != null, $"End is null in shortest distance calculation.");

            // Adjacency list for storing
            // which vertices are connected
            Dictionary<FSMNode, List<FSMNode>> adj = new Dictionary<FSMNode, List<FSMNode>>();

            AddAdjacency(adj, nodeGraph);
            AddEdges(adj, nodeGraph);

            return CalculateShortestDistance(adj, start, end);
        }

        private static void AddAdjacency(Dictionary<FSMNode, List<FSMNode>> adj, IFSMGraph nodeGraph)
        {
            foreach (FSMNode node in nodeGraph.Nodes)
            {
                if (node is IFSMGraph)
                {
                    AddAdjacency(adj, node as IFSMGraph);
                }
                else
                {
                    adj.Add(node, new List<FSMNode>());
                }
            }
        }

        private static void AddEdges(Dictionary<FSMNode, List<FSMNode>> adj, IFSMGraph nodeGraph)
        {
            foreach (FSMNode node in nodeGraph.Nodes)
            {
                if (node is IFSMGraph)
                {
                    AddEdges(adj, node as IFSMGraph);
                }
                else
                {
                    foreach (var output in node.Outputs)
                    {
                        for (int i = 0, n = output.ConnectionCount; i < n; ++i)
                        {
                            FSMNode targetNode = output.GetConnection(i).node as FSMNode;
                            Debug.Assert(targetNode != null, $"TargetNode null for {node.name}.{output.fieldName}");

                            // If we are connecting to a sub FSM we need to attach to the start node of the sub fsm instead
                            IFSMGraph targetFSMGraph = targetNode as IFSMGraph;
                            Debug.Assert(targetFSMGraph == null || targetFSMGraph.StartNode != null, $"No start node on {targetNode.name}.");
                            adj[node].Add(targetFSMGraph != null ? targetFSMGraph.StartNode : targetNode);
                        }
                    }

                    if (adj[node].Count == 0 && nodeGraph.ParentFSMGraph != null)
                    {
                        // We are in a sub fsm and we need to attach any end nodes to the next node after the sub fsm
                        FSMNode targetNode = nodeGraph.ParentFSMGraph.FindNode((nodeGraph as FSMNode).Guid).GetConnectedNodeFromDefaultOutput();
                        Debug.Assert(targetNode != null, $"TargetNode null for {node.name}");

                        // If we are connecting to a sub FSM we need to attach to the start node of the sub fsm instead
                        IFSMGraph targetFSMGraph = targetNode as IFSMGraph;
                        Debug.Assert(targetFSMGraph == null || targetFSMGraph.StartNode != null, $"No start node on {targetNode.name}.");
                        adj[node].Add(targetFSMGraph != null ? targetFSMGraph.StartNode : targetNode);
                    }
                }
            }
        }

        private static int CalculateShortestDistance(
            Dictionary<FSMNode, List<FSMNode>> adj,
            FSMNode s, 
            FSMNode dest)
        {
            // predecessor[i] array stores
            // predecessor of i and distance
            // array stores distance of i
            // from s
            Dictionary<FSMNode, FSMNode> pred = new Dictionary<FSMNode, FSMNode>();
            Dictionary<FSMNode, int> dist = new Dictionary<FSMNode, int>();

            if (BFS(adj, s, dest, pred, dist) == false)
            {
                Debug.LogError("Given source and destination are not connected");
                return int.MaxValue;
            }

            // List to store path
            List<FSMNode> path = new List<FSMNode>();
            FSMNode crawl = dest;
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
            Dictionary<FSMNode, List<FSMNode>> adj,
            FSMNode src,
            FSMNode dest,
            Dictionary<FSMNode, FSMNode> pred,
            Dictionary<FSMNode, int> dist)
        {
            // a queue to maintain queue of
            // vertices whose adjacency list
            // is to be scanned as per normal
            // BFS algorithm using List of int type
            List<FSMNode> queue = new List<FSMNode>();

            // bool array visited[] which
            // stores the information whether
            // ith vertex is reached at least
            // once in the Breadth first search
            Dictionary<FSMNode, bool> visited = new Dictionary<FSMNode, bool>();

            // now source is first to be
            // visited and distance from
            // source to itself should be 0
            visited[src] = true;
            dist[src] = 0;
            queue.Add(src);

            // bfs Algorithm
            while (queue.Count != 0)
            {
                FSMNode u = queue[0];
                queue.RemoveAt(0);

                if (adj.TryGetValue(u, out List<FSMNode> adjacents))
                {
                    for (int i = 0; i < adjacents.Count; i++)
                    {
                        FSMNode ithAdjacent = adjacents[i];
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
                else
                {
                    Debug.LogAssertion($"Could not find node {u.name} in adj.");
                    return false;
                }
            }
            return false;
        }
    }
}