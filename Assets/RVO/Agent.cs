/*
 * Agent.cs
 * RVO2 Library C#
 *
 * Copyright 2008 University of North Carolina at Chapel Hill
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Please send all bug reports to <geom@cs.unc.edu>.
 *
 * The authors may be contacted via:
 *
 * Jur van den Berg, Stephen J. Guy, Jamie Snape, Ming C. Lin, Dinesh Manocha
 * Dept. of Computer Science
 * 201 S. Columbia St.
 * Frederick P. Brooks, Jr. Computer Science Bldg.
 * Chapel Hill, N.C. 27599-3175
 * United States of America
 *
 * <http://gamma.cs.unc.edu/RVO2/>
 */

using System;
using System.Collections.Generic;
using XPool;

namespace RVO
{
    /**
     * <summary>Defines an agent in the simulation.</summary>
     */
    public partial class Agent : IDisposable
    {
        internal IList<KeyValuePair<float, Agent>> agentNeighbors_ = new List<KeyValuePair<float, Agent>>(32);
        internal IList<KeyValuePair<float, Obstacle>> obstacleNeighbors_ = new List<KeyValuePair<float, Obstacle>>(32);
        internal IList<Line> orcaLines_ = new List<Line>(32);

        public Vec2 position { get; internal set; }
        public Vec2 prefVelocity { get; set; }

        internal Vec2 velocity_;
        internal int id_ = 0;
        internal int maxNeighbors_ = 0;
        internal float maxSpeed_ = 0.0f;
        internal float neighborDist_ = 0.0f;
        internal float radius_ = 0.0f;
        internal float timeHorizon_ = 0.0f;
        internal float timeHorizonObst_ = 0.0f;
        internal bool needDelete_ = false;

        private Vec2 newVelocity_;


        internal void compute(float deltaTime)
        {
            computeNeighbors();
            computeNewVelocity(deltaTime);
        }

        /**
         * <summary>Updates the two-dimensional position and two-dimensional
         * velocity of this agent.</summary>
         */
        internal void update(float deltaTime)
        {
            if (float.IsNaN(newVelocity_.x) || float.IsNaN(newVelocity_.y))
            {
                Log.W("newVelocity is NaN");
                newVelocity_ = Vec2.zero;
            }
            velocity_ = newVelocity_;
            position += velocity_ * deltaTime;
        }

        public static Agent Get()
        {
            return AnyPool<Agent>.Get();
        }

        public void Dispose()
        {
            agentNeighbors_.Clear();
            obstacleNeighbors_.Clear();
            orcaLines_.Clear();
            position = Vec2.zero;
            prefVelocity = Vec2.zero;
            velocity_ = Vec2.zero;
            id_ = 0;
            maxNeighbors_ = 0;
            maxSpeed_ = 0.0f;
            neighborDist_ = 0.0f;
            radius_ = 0.0f;
            timeHorizon_ = 0.0f;
            timeHorizonObst_ = 0.0f;
            needDelete_ = false;
            newVelocity_ = Vec2.zero;

            AnyPool<Agent>.Release(this);
        }
    }
}
