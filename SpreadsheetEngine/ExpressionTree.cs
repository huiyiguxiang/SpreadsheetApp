// <copyright file="ExpressionTree.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The class for ExpressionTree. It can build the tree, evaluates the tree, and add variable nodes' names and values to a dictionary.
    /// </summary>
    public class ExpressionTree
    {
        private BaseNode root; // root of tree

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression">Expression tree builds based off an expression.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            Variables = new Dictionary<string, double>();
            this.root = Compile(expression);
        }

        /// <summary>
        /// Gets or sets variable name and values.
        /// </summary>
        public static Dictionary<string, double> Variables { get; set; } // for name and value

        /// <summary>
        /// Gets tree expression.
        /// </summary>
        public string expression { get; private set; } // string expression

        /// <summary>
        /// Adds the name and value of the variable node to the dictionary.
        /// </summary>
        /// <param name="variableName">Name of variable node.</param>
        /// <param name="variableValue">Value of variable node.</param>
        public void SetVariable(string variableName, double variableValue) // add to dictionary library
        {
            try
            {
                Variables.Add(variableName, variableValue);
            }
            catch
            {
                Variables[variableName] = variableValue; // if variable already assigned then overwrite
            }
        }

        /// <summary>
        /// Stores variable names in array.
        /// </summary>
        /// <returns>Variables array.</returns>
        public string[] GetVariables()
        {
            return Variables.Keys.ToArray();
        }

        /// <summary>
        /// Evaluates the value of the root.
        /// </summary>
        /// <returns>Evaluated value of the root.</returns>
        public double Evaluate()
        {
            return this.root.Evaluate(); // return eval of root
        }

        /// <summary>
        /// Builds the tree for const and var nodes.
        /// </summary>
        /// <param name="expression">Expression string to build the tree.</param>
        /// <returns>Const or Var node.</returns>
        private static BaseNode MakeTree(string expression)
        {
            // determine if node is constant or variable
            if (double.TryParse(expression, out double value))
            {
                return new ConstNode(value);
            }
            else
            {
                return new VarNode(expression);
            }
        }

        /// <summary>
        /// Parse the infix expression string and build to postfix.
        /// </summary>
        /// <param name="expression">Expression string.</param>
        /// <returns>Node values.</returns>
        private static BaseNode Compile(string expression)
        {
            expression = expression.Replace(" ", string.Empty);

            // if empty expression
            if (expression == string.Empty)
            {
                return null;
            }

            // if expression enclosed by parentheses
            if (expression[0] == '(')
            {
                int paren = 1;
                for (int i = 1; i < expression.Length; i++)
                {
                    // if matching paren found
                    if (expression[i] == ')')
                    {
                        paren--; // decrease paren count

                        // if parens are all matched
                        if (paren == 0)
                        {
                            if (i == expression.Length - 1)
                            {
                                return Compile(expression.Substring(1, expression.Length - 2)); // solve exp inside paren
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    // if extra parentheses found
                    if (expression[i] == '(')
                    {
                        paren++; // add to paren count
                    }
                }
            }

            // find operators
            int index = OpOrder(expression);

            // if operator found
            if (index != -1)
            {
                OpNode node = OperatorNodeFactory.CreateOpNode(expression[index]);
                node.Lhs = Compile(expression.Substring(0, index));
                node.Rhs = Compile(expression.Substring(index + 1));
                return node;
            }

            // just return expression if no operators
            return MakeTree(expression);
        }

        /// <summary>
        /// Finds the index of the operator with priority to solve first.
        /// </summary>
        /// <param name="expression">Expression string.</param>
        /// <returns>Index of operator.</returns>
        private static int OpOrder(string expression)
        {
            int paren = 0;
            int index = -1;
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                switch (expression[i])
                {
                    case '(':
                        paren++;
                        break;
                    case ')':
                        paren--;
                        break;
                    case '*':
                    case '/':
                        if (paren == 0 && index == -1)
                        {
                            index = i;
                        }

                        break;
                    case '+':
                    case '-':
                        if (paren == 0)
                        {
                            return i;
                        }

                        break;
                }
            }

            return index;
        }
    }
}
