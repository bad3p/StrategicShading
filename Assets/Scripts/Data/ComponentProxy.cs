
using Types;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class ComponentProxy : MonoBehaviour
{
    public void AwakeImmediate()
    {
        Awake();
    }

    protected virtual void Awake()
    {
    }
}
