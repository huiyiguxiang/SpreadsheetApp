// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;

    /// <summary>
    /// The console menu for Expression Tree.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string input;
            string name;
            string val;
            ExpressionTree tree = new ExpressionTree("A1-12-C1"); // expression shown in example
            while (true)
            {
                // print the menu
                Console.WriteLine("Menu (current expression=\"" + tree.expression + "\")");
                Console.WriteLine("1 = Enter a new expression");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate tree");
                Console.WriteLine("4 = Quit");

                // get the user's input
                input = Console.ReadLine();

                switch (input)
                {
                    // prompt for new expression
                    case "1":
                        Console.WriteLine("Enter a new expression: ");
                        tree = new ExpressionTree(Console.ReadLine()); // add new expression to tree
                        break;

                    // get name and value
                    case "2":
                        Console.WriteLine("Enter the name of the variable: ");
                        name = Console.ReadLine(); // read name of variable
                        Console.WriteLine("Enter the value of the variable: ");
                        val = Console.ReadLine(); // read value of variable
                        tree.SetVariable(name, Convert.ToDouble(val)); // convert val to double and set var name in tree
                        break;

                    // eval tree
                    case "3":
                        Console.WriteLine(tree.Evaluate());
                        break;

                    // exit menu
                    case "4":
                        System.Environment.Exit(1);
                        break;

                    // invalid input ignore
                    default:
                        continue;
                }
            }
        }
    }
}
