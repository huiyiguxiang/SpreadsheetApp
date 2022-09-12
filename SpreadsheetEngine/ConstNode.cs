// <copyright file="ConstNode.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class for the constant value node.
    /// </summary>
    public class ConstNode : BaseNode
    {
        private readonly double value; // value of constant

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstNode"/> class.
        /// </summary>
        /// <param name="value">Takes a numeric value as an argument.</param>
        public ConstNode(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Overrides the Evaluate functions. Returns the value of the constant value node.
        /// </summary>
        /// <returns>Returns the value of the constant value node.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
