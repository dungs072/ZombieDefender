using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float rayDistance = 25f;
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private LineRenderer trailRenderer;
    private void Start()
    {
        trailRenderer.gameObject.SetActive(false);
    }
    public void UpdateLaser(bool canUseLineRenderer)
    {
        if (canUseLineRenderer)
        {

            trailRenderer.gameObject.SetActive(true);
            ShootLaser();
        }
        else
        {
            trailRenderer.gameObject.SetActive(false);
        }

    }
    private void ShootLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
        if (hit.collider != null)
        {
            DrawRay(laserFirePoint.position, hit.point);
        }
        else
        {
            DrawRay(laserFirePoint.position, laserFirePoint.transform.right * rayDistance);
        }
    }
    private void DrawRay(Vector2 startPos, Vector2 endPos)
    {
        trailRenderer.SetPosition(0, startPos);
        trailRenderer.SetPosition(1, endPos);
    }
}
