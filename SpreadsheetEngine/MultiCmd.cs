// <copyright file="MultiCmd.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// For dealing with both text and color commands.
    /// </summary>
    public class MultiCmd
    {
        private string cmdName;
        private ICommand[] commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCmd"/> class.
        /// </summary>
        public MultiCmd()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCmd"/> class.
        /// </summary>
        /// <param name="newCommands">New commands that gets passed into array.</param>
        /// <param name="name">Name of command.</param>
        public MultiCmd(ICommand[] newCommands, string name)
        {
            this.commands = newCommands;
            this.cmdName = name;
        }

        /// <summary>
        /// Gets command name.
        /// </summary>
        public string Command
        {
            get { return this.cmdName; }
        }

        /// <summary>
        /// Execute multi command.
        /// </summary>
        /// <returns>Inverse command array.</returns>
        public MultiCmd Execute()
        {
            // list to hold commands
            List<ICommand> cmdList = new List<ICommand>();

            // loop through command in commands array
            foreach (ICommand cmd in this.commands)
            {
                cmdList.Add(cmd.Execute()); // add to list
            }

            // return inverse command array
            return new MultiCmd(cmdList.ToArray(), this.cmdName);
        }
    }
}
