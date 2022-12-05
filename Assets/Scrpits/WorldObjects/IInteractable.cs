using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoInteract<T> where T : MonoBehaviour
{
    public float range {get; set;}

    public abstract void Interact(T caller);
}