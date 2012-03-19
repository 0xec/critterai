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
using System.Collections.Generic;
using org.critterai.nmbuild;
using org.critterai.nmbuild.u3d;
using UnityEngine;

public sealed class OFMConnectionCompiler
    : InputBuildProcessor
{
    public string Name { get { return name; } }
    public override int Priority { get { return NMBuild.MinPriority; } }
    public override bool DuplicatesAllowed { get { return false; } }

    public override bool ProcessInput(InputBuildState state, InputBuildContext context)
    {
        if (context != null)
        {
            switch (state)
            {
                case InputBuildState.CompileInput:

                    Compile(context);
                    break;

                case InputBuildState.LoadComponents:

                    Load(context);
                    break;
            }
        }

        return true;
    }

    private void Load(InputBuildContext context)
    {
        context.info.loaderCount++;

        int count = context.LoadFromScene<OFMConnection>();

        context.Log(string.Format("{0}: Loaded {1} terrain.", Name, count), this);
    }

    private void Compile(InputBuildContext context)
    {
        context.info.compilerCount++;

        ConnectionSetCompiler compiler = context.connCompiler;
        List<Component> items = context.components;
        List<byte> areas = context.areas;

        int count = 0;

        for (int i = 0; i < items.Count; i++)
        {
            Component item = items[i];

            if (item is OFMConnection)
            {
                OFMConnection conn = (OFMConnection)item;
                byte area = (conn.OverrideArea ? conn.Area : areas[i]);

                compiler.Add(conn.StartPoint, conn.EndPoint
                    , conn.Radius
                    , conn.IsBidirectional
                    , area
                    , 0  // Flags not supported.
                    , (uint)conn.UserId);

                count++;
            }
        }

        context.Log(string.Format("{0}: Compiled Connections: {1}", Name, count), this);
    }
}