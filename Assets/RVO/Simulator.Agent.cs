using System.Collections.Generic;

namespace RVO
{
    public partial class Simulator
    {
        internal IDictionary<int, int> agentNo2indexDict_;
        internal IDictionary<int, int> index2agentNoDict_;

        public void delAgent(int agentNo)
        {
            agents_[agentNo2indexDict_[agentNo]].needDelete_ = true;
        }

        void updateDeleteAgent()
        {
            bool isDelete = false;
            for (int i = agents_.Count - 1; i >= 0; i--)
            {
                if (agents_[i].needDelete_)
                {
                    agents_.RemoveAt(i);
                    isDelete = true;
                }
            }
            if (isDelete)
                onDelAgent();
        }

        void onDelAgent()
        {
            agentNo2indexDict_.Clear();
            index2agentNoDict_.Clear();

            for (int i = 0; i < agents_.Count; i++)
            {
                int agentNo = agents_[i].id_;
                agentNo2indexDict_.Add(agentNo, i);
                index2agentNoDict_.Add(i, agentNo);
            }
        }

        void onAddAgent()
        {
            if (agents_.Count == 0)
                return;

            int index = agents_.Count - 1;
            int agentNo = agents_[index].id_;
            agentNo2indexDict_.Add(agentNo, index);
            index2agentNoDict_.Add(index, agentNo);
        }

        private static int sm_AgentId = 0;
        private static int GenAgentId()
        {
            int id = sm_AgentId;
            sm_AgentId++;
            if (sm_AgentId > int.MaxValue - 10)
                sm_AgentId = 0;
            return id;
        }

        /**
         * <summary>Adds a new agent to the simulation.</summary>
         *
         * <returns>The number of the agent.</returns>
         *
         * <param name="position">The two-dimensional starting position of this
         * agent.</param>
         * <param name="neighborDist">The maximum distance (center point to
         * center point) to other agents this agent takes into account in the
         * navigation. The larger this number, the longer the running time of
         * the simulation. If the number is too low, the simulation will not be
         * safe. Must be non-negative.</param>
         * <param name="maxNeighbors">The maximum number of other agents this
         * agent takes into account in the navigation. The larger this number,
         * the longer the running time of the simulation. If the number is too
         * low, the simulation will not be safe.</param>
         * <param name="timeHorizon">The minimal amount of time for which this
         * agent's velocities that are computed by the simulation are safe with
         * respect to other agents. The larger this number, the sooner this
         * agent will respond to the presence of other agents, but the less
         * freedom this agent has in choosing its velocities. Must be positive.
         * </param>
         * <param name="timeHorizonObst">The minimal amount of time for which
         * this agent's velocities that are computed by the simulation are safe
         * with respect to obstacles. The larger this number, the sooner this
         * agent will respond to the presence of obstacles, but the less freedom
         * this agent has in choosing its velocities. Must be positive.</param>
         * <param name="radius">The radius of this agent. Must be non-negative.
         * </param>
         * <param name="maxSpeed">The maximum speed of this agent. Must be
         * non-negative.</param>
         * <param name="velocity">The initial two-dimensional linear velocity of
         * this agent.</param>
         */
        public int addAgent(
            Vec2 position,
            float neighborDist,
            int maxNeighbors,
            float timeHorizon,
            float timeHorizonObst,
            float radius,
            float maxSpeed,
            Vec2 velocity)
        {
            Agent agent = new Agent();
            agent.id_ = sm_AgentId;
            sm_AgentId++;
            agent.maxNeighbors_ = maxNeighbors;
            agent.maxSpeed_ = maxSpeed;
            agent.neighborDist_ = neighborDist;
            agent.position = position;
            agent.radius_ = radius;
            agent.timeHorizon_ = timeHorizon;
            agent.timeHorizonObst_ = timeHorizonObst;
            agent.velocity_ = velocity;
            agents_.Add(agent);
            onAddAgent();
            return agent.id_;
        }

        public Agent getAgent(int agentNo)
        {
            return agents_[agentNo2indexDict_[agentNo]];
        }

        // public int getAgentAgentNeighbor(int agentNo, int neighborNo)
        // {
        //     return agents_[agentNo2indexDict_[agentNo]].agentNeighbors_[neighborNo].Value.id_;
        // }

        // public int getAgentObstacleNeighbor(int agentNo, int neighborNo)
        // {
        //     return agents_[agentNo2indexDict_[agentNo]].obstacleNeighbors_[neighborNo].Value.id_;
        // }
    }
}