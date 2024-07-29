using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Transform parent;
    public void PlayUIDeath()
    {
        if (parent.TryGetComponent(out PlayerController playerController))
        {
            playerController.PlayUIDeath();
        }

    }
}
