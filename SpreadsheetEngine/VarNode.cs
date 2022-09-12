// <copyright file="VarNode.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class for the variable node.
    /// </summary>
    public class VarNode : BaseNode
    {
        private readonly string varName;
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarNode"/> class.
        /// </summary>
        /// <param name="name">Name that represents the node.</param>
        public VarNode(string name)
        {
            this.varName = name;
            this.value = 0.0;
            ExpressionTree.Variables.Add(name, this.value); // add to dict as 0 value
        }

        /// <summary>
        /// Adds the name of the variable to the dictionary.
        /// </summary>
        /// <returns>Value of the node.</returns>
        public override double Evaluate()
        {
            this.value = ExpressionTree.Variables[this.varName];
            return this.value; // return value of node
        }
    }
}
