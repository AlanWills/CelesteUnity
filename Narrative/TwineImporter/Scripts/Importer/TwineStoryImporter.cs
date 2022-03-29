using Celeste.FSM;
using Celeste.Twine;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Celeste.Narrative.TwineImporter
{
    public static class TwineStoryImporter
    {
        public static void Import(
            TwineStory twineStory, 
            TwineStoryImporterSettings importerSettings,
            FSMGraph fsmGraph,
            bool stopOnParserError)
        {
            // Remove any existing nodes to make a blank slate
            fsmGraph.RemoveAllNodes();

            Dictionary<int, FSMNode> nodeLookup = new Dictionary<int, FSMNode>();

            TwineNode startNode = twineStory.passages.Find(x => importerSettings.ContainsStartTag(x.Tags));
            if (startNode == null)
            {
                UnityEngine.Debug.LogAssertion($"Twine Story {twineStory.name} has no start node set.");
                return;
            }

            Vector2 startNodePosition = startNode != null ? startNode.Position : Vector2.zero;
            bool parserErrorOccurred = false;

            foreach (TwineNode twineNode in twineStory.passages)
            {
                if (importerSettings.TryParse(
                    twineNode,
                    fsmGraph,
                    startNodePosition,
                    out FSMNode fsmNode))
                {
                    nodeLookup.Add(twineNode.pid, fsmNode);

#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        UnityEditor.AssetDatabase.AddObjectToAsset(fsmNode, fsmGraph);
                    }
#endif
                }
                else if (!importerSettings.ContainsIgnoreTag(twineNode.Tags))
                {
                    UnityEngine.Debug.LogError($"Failed to parse node {twineNode.Name}.  Transitions will not be created properly...");
                    parserErrorOccurred = true;
                }

                if (stopOnParserError && parserErrorOccurred)
                {
                    break;
                }
            }

            // Now resolve transitions
            foreach (TwineNode node in twineStory.passages)
            {
                if (node.Links.Count > 0 && nodeLookup.TryGetValue(node.pid, out FSMNode graphNode))
                {
                    if (node.Links.Count > 1)
                    {
                        foreach (TwineNodeLink link in node.Links)
                        {
                            if (nodeLookup.TryGetValue(link.pid, out FSMNode target))
                            {
                                NodePort outputPort = graphNode.GetOutputPort(link.link);
                                UnityEngine.Debug.Assert(outputPort != null, $"Could not find output port {link.link} in node {graphNode.name}.");

                                NodePort inputPort = target.GetDefaultInputPort();
                                UnityEngine.Debug.Assert(inputPort != null, $"Could not find default input port in node {target.name}.");

                                outputPort.Connect(inputPort);
                            }
                            else
                            {
                                UnityEngine.Debug.LogAssertion($"Could not find node with pid {link.pid} for link on node {graphNode.name}.");
                            }
                        }
                    }
                    else
                    {
                        TwineNodeLink link = node.Links[0];
                        if (nodeLookup.TryGetValue(link.pid, out FSMNode target))
                        {
                            NodePort outputPort = graphNode.GetDefaultOutputPort();
                            UnityEngine.Debug.Assert(outputPort != null, $"Could not find default output port in node {graphNode.name}.");

                            NodePort inputPort = target.GetDefaultInputPort();
                            UnityEngine.Debug.Assert(inputPort != null, $"Could not find default input port in node {target.name}.");

                            outputPort.Connect(inputPort);
                        }
                        else
                        {
                            UnityEngine.Debug.LogAssertion($"Could not find node with pid {link.pid} for link on node {graphNode.name}.");
                        }
                    }
                }
            }

            // Set the start node using the pid from the twine story
            if (startNode == null || !nodeLookup.TryGetValue(startNode.pid, out fsmGraph.startNode))
            {
                UnityEngine.Debug.LogError($"Failed to set start node on narrative graph.");
            }
        }
    }
}
