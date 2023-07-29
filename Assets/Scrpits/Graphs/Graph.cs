using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Graph<T, V> where V : Vertex<T>, new()
{
    [SerializeField] protected List<V> vertices = new List<V>();

    protected Dictionary<V, T> dictonary = new Dictionary<V, T>();

    public List<V> GetVerticies()
    {
        return vertices;
    }

    public void ClearVerticies()
    {
        vertices.Clear();
    }

    public bool AddElement(T element)
    {
        if(dictonary.ContainsValue(element)) 
        {
            return false;
        }
        V vertex = new V();
        vertex.content = element;

        //vertices.Add(vertex);
        dictonary.Add(vertex, element);
        vertices.Add(vertex);

        return true;
    }

    public bool AddVertex(V vertex)
    {
        if (dictonary.ContainsKey(vertex))
        {
            return false;
        }

        //dictonary.Add(vertex, null);
        vertices.Add(vertex);

        return true;
    }

    public bool RemoveVertex(V vertex)
    {
        if (vertices.Contains(vertex))
        {
            vertices.Remove(vertex);
            return true;
        }
        return false;
    }

    public bool Contains(T element)
    {
        throw new System.NotImplementedException();
        // return dictonary.ContainsKey(element);
    }

    public abstract bool AddEdge(V a, V b);

    public abstract bool RemoveEdge(V a, V b);

    // protected V GetVertex(T element)
    // {
    //     return dictonary.GetValueOrDefault(element);
    // }
}
