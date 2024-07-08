using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceItself : MonoBehaviour
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
