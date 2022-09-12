// <copyright file="BaseNode.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class for nodes to inherit from.
    /// </summary>
    public abstract class BaseNode
    {
        /// <summary>
        /// For nodes to be evaluated.
        /// </summary>
        /// <returns>Evaluated value of node.</returns>
        public abstract double Evaluate();
    }
}
