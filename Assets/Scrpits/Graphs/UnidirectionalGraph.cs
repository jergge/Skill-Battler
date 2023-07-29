using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnidirectionalGraph<T, V> : Graph<T, V> where V: Vertex<T>, new()
{
    public override bool AddEdge(V from, V to)
    {
        if (vertices.Contains(from) && vertices.Contains(to) && from is not null && to is not null)
        {
            from.connections.Add(to);
            from.OnNewConnectionTo(to);
            to.OnNewConnectonFrom(from);
            //GetVertex(to).connections.Add(dictonary.GetValueOrDefault(from));
            return true;
        }
        return false;
    }

    public bool AddEdge(ref V from, ref V to)
    {
        if (vertices.Contains(from) && vertices.Contains(to) && from is not null && to is not null)
        {
            Debug.Log("Adding a new edge through the graph class");
            from.connections.Add(to);
            from.OnNewConnectionTo(to);
            to.OnNewConnectonFrom(from);
            //GetVertex(to).connections.Add(dictonary.GetValueOrDefault(from));
            return true;
        }
        return false;
    }

    public override bool RemoveEdge(V from, V to)
    {
        if (dictonary.ContainsKey(from) && dictonary.ContainsKey(to) && from is not null && to is not null)
        {
            from.connections.Remove(to);
            from.OnRemovedConnectionTo(to);
            to.OnRemovedConnectonFrom(from);
            //GetVertex(to).connections.Remove(dictonary.GetValueOrDefault(from));
            return true;
        }
        return false;
    }
}
