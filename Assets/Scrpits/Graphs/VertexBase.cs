using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VertexBase<T>
{
    public List<VertexBase<T>> connections = new List<VertexBase<T>>();

    public virtual void OnNewConnectionTo(VertexBase<T> other) { }
    public virtual void OnNewConnectonFrom(VertexBase<T> other) { }
    public virtual void OnRemovedConnectionTo(VertexBase<T> other) { }
    public virtual void OnRemovedConnectonFrom(VertexBase<T> other) { }

    [SerializeReference] public T content;
}
