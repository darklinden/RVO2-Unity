using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;

public class ObstacleCollect : MonoBehaviour
{
    private RVOObstacle m_obstacle = null;

    private RVOObstacle obstacle
    {
        get
        {
            if (m_obstacle == null)
            {
                m_obstacle = RVOObstacle.Get(GameMainManager.Instance.GetSimulator(), GetComponent<Collider>().bounds);
            }
            return m_obstacle;
        }
    }

    private void OnEnable()
    {
        obstacle.Enable();
    }

    private void OnDisable()
    {
        obstacle.Disable();
    }

    private void OnDestroy()
    {
        obstacle.Dispose();
    }
}