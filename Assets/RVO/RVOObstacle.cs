using System;
using UnityEngine;
using System.Collections.Generic;
using XPool;

namespace RVO
{
    public class RVOObstacle : IDisposable
    {
        public int m_obstacleId = -1;
        private IList<Vec2> obstacle = new List<Vec2>();

        public static RVOObstacle Get(RVOSimulator simulator, Bounds bounds)
        {
            RVOObstacle obstacle = AnyPool<RVOObstacle>.Get();
            obstacle.Initialize(simulator, bounds);
            return obstacle;
        }

        public void Dispose()
        {
            Disable();
            obstacle.Clear();
            simulator = null;
            AnyPool<RVOObstacle>.Release(this);
        }

        private RVOSimulator simulator = null;
        private void Initialize(RVOSimulator simulator, Bounds bounds)
        {
            this.simulator = simulator;
            obstacle.Clear();
            obstacle.Add(new Vec2(bounds.max.x, bounds.max.z));
            obstacle.Add(new Vec2(bounds.min.x, bounds.max.z));
            obstacle.Add(new Vec2(bounds.min.x, bounds.min.z));
            obstacle.Add(new Vec2(bounds.max.x, bounds.min.z));
            Enable();
        }

        public void Enable()
        {
            if (m_obstacleId == -1)
            {
                m_obstacleId = simulator.AddObstacle(obstacle);
            }
        }

        public void Disable()
        {
            if (m_obstacleId != -1)
            {
                simulator.RemoveObstacle(m_obstacleId);
                m_obstacleId = -1;
            }
        }
    }
}
