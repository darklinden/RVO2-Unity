/*
 * Simulator.cs
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

namespace RVO
{
    /**
     * <summary>Defines the simulation.</summary>
     */
    public partial class RVOSimulator
    {
        // static instance
        // private static Lazy<RVOSimulator> sm_LazyInstance = new Lazy<RVOSimulator>(() => new RVOSimulator());
        // public static RVOSimulator Instance => sm_LazyInstance.Value;


        internal List<Agent> agents_;
        // public IReadOnlyList<Agent> Agents => agents_.AsReadOnly();
        internal List<Obstacle> obstacles_;
        internal KdTree kdTree_;
        private float globalTime_;
        public float GlobalTime => globalTime_;

        /**
         * <summary>Constructs and initializes a simulation.</summary>
         */
        public RVOSimulator()
        {
            Clear();
        }

        /**
         * <summary>Clears the simulation.</summary>
         */
        public void Clear()
        {
            agents_ = new List<Agent>(128);
            agentNo2indexDict_ = new Dictionary<int, int>(128);
            index2agentNoDict_ = new Dictionary<int, int>(128);
            kdTree_ = new KdTree();
            obstacles_ = new List<Obstacle>(32);
            globalTime_ = 0.0f;
        }

        /**
         * <summary>Performs a simulation step and updates the two-dimensional
         * position and two-dimensional velocity of each agent.</summary>
         *
         * <returns>The global time after the simulation step.</returns>
         */
        public float StepUpdate(float deltaTime)
        {
            updateObstacles();

            updateDeleteAgent();

            kdTree_.buildAgentTree(this);

            for (int block = 0; block < agents_.Count; block++)
            {
                agents_[block].compute(this, deltaTime);
            }

            for (int block = 0; block < agents_.Count; block++)
            {
                agents_[block].update(deltaTime);
            }

            globalTime_ += deltaTime;

            return globalTime_;
        }

        /**
         * <summary>Performs a visibility query between the two specified points
         * with respect to the obstacles.</summary>
         *
         * <returns>A boolean specifying whether the two points are mutually
         * visible. Returns true when the obstacles have not been processed.
         * </returns>
         *
         * <param name="point1">The first point of the query.</param>
         * <param name="point2">The second point of the query.</param>
         * <param name="radius">The minimal distance between the line connecting
         * the two points and the obstacles in order for the points to be
         * mutually visible (optional). Must be non-negative.</param>
         */
        public bool queryVisibility(Vec2 point1, Vec2 point2, float radius)
        {
            return kdTree_.queryVisibility(point1, point2, radius);
        }

        public int queryNearAgent(Vec2 point, float radius)
        {
            if (agents_.Count == 0) return -1;
            return kdTree_.queryNearAgent(point, radius);
        }
    }
}
