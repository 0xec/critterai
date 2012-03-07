﻿/*
 * Copyright (c) 2012 Stephen A. Pratt
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;
using org.critterai.nmgen;

namespace org.critterai.nmbuild
{
    public sealed class LowObstaclesWalkable
        : NMGenProcessor
    {
        public static int ProcessorPriority = CAIUtil.MinPriority - 100;

        private static LowObstaclesWalkable mInstance = new LowObstaclesWalkable();

        public override bool IsThreadSafe { get { return true; } }

        private LowObstaclesWalkable()
            : base(typeof(LowObstaclesWalkable).Name, ProcessorPriority)
        {
        }

        public override bool ProcessBuild(NMGenState state, NMGenContext context)
        {
            if (state == NMGenState.HeightfieldBuild)
            {
                if (context.Heightfield.MarkLowObstaclesWalkable(context
                    , context.Config.WalkableStep))
                {
                    context.Log(Name + ": Marked low obstacles as walklable.", this);
                    return true;
                }
                else
                {
                    context.Log(Name + ": Mark low obstacles failed.", this);
                    return false;
                }
            }

            return true;
        }

        public static LowObstaclesWalkable Instance { get { return mInstance; } }
    }
}
