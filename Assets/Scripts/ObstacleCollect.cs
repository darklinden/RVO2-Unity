using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;

public class ObstacleCollect : MonoBehaviour
{
    private int m_obstacleId = -1;

    IList<Vec2> obstacle = new List<Vec2>();
    private void OnEnable()
    {
        var boxCollider = GetComponent<BoxCollider>();
        float minX = boxCollider.transform.position.x -
                     boxCollider.size.x * boxCollider.transform.lossyScale.x * 0.5f;
        float minZ = boxCollider.transform.position.z -
                     boxCollider.size.z * boxCollider.transform.lossyScale.z * 0.5f;
        float maxX = boxCollider.transform.position.x +
                     boxCollider.size.x * boxCollider.transform.lossyScale.x * 0.5f;
        float maxZ = boxCollider.transform.position.z +
                     boxCollider.size.z * boxCollider.transform.lossyScale.z * 0.5f;

        obstacle.Clear();
        obstacle.Add(new Vec2(maxX, maxZ));
        obstacle.Add(new Vec2(minX, maxZ));
        obstacle.Add(new Vec2(minX, minZ));
        obstacle.Add(new Vec2(maxX, minZ));
        m_obstacleId = Simulator.Instance.AddObstacle(obstacle);
    }

    private void OnDisable()
    {
        if (m_obstacleId != -1)
            Simulator.Instance.RemoveObstacle(m_obstacleId);
    }
}