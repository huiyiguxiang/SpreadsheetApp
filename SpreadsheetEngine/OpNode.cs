// <copyright file="OpNode.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The class for operator nodes.
    /// </summary>
    public class OpNode : BaseNode
    {
        /// <summary>
        /// lefthand side of the tree.
        /// </summary>
        public BaseNode Lhs;

        /// <summary>
        /// righthand side of the tree.
        /// </summary>
        public BaseNode Rhs;
        private readonly char sign; // operator

        /// <summary>
        /// Initializes a new instance of the <see cref="OpNode"/> class.
        /// </summary>
        /// <param name="op">Takes an operator as an argument.</param>
        public OpNode(char op)
        {
            this.sign = op;
            this.Lhs = null;
            this.Rhs = null;
        }

        /// <summary>
        /// Overrides the Evaluate function. Determines the value of two nodes after an operator is applied.
        /// </summary>
        /// <returns>Returns the value of the left hand side and right hand side value after the operator is applied.</returns>
        public override double Evaluate()
        {
            // operations to perform based on operator
            switch (this.sign)
            {
                case '+':
                    return this.Lhs.Evaluate() + this.Rhs.Evaluate();
                case '-':
                    return this.Lhs.Evaluate() - this.Rhs.Evaluate();
                case '*':
                    return this.Lhs.Evaluate() * this.Rhs.Evaluate();
                case '/':
                    return this.Lhs.Evaluate() / this.Rhs.Evaluate();
                default:
                    break;
            }

            return 0.0;
        }
    }
}
