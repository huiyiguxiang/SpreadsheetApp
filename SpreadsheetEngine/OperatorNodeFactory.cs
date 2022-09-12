// <copyright file="OperatorNodeFactory.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    /// <summary>
    /// Creates an Operator Node.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// Creates operator node based on operator.
        /// </summary>
        /// <param name="op">Operator char.</param>
        /// <returns>Operator node based on operator.</returns>
        public static OpNode CreateOpNode(char op)
        {
            switch (op)
            {
                case '+':
                    return new OpNode('+');
                case '-':
                    return new OpNode('-');
                case '*':
                    return new OpNode('*');
                case '/':
                    return new OpNode('/');
                default:
                    break;
            }

            return null;
        }
    }
}
