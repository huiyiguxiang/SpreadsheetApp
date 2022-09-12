// <copyright file="RestoreColor.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class that store color history of the cell.
    /// </summary>
    public class RestoreColor : ICommand
    {
        private uint color;
        private Cell cell;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreColor"/> class.
        /// </summary>
        /// <param name="name">The name of the cell.</param>
        /// <param name="cellColor">The color of the cell.</param>
        public RestoreColor(Cell name, uint cellColor)
        {
            this.color = cellColor;
            this.cell = name;
        }

        /// <summary>
        /// Undo the color.
        /// </summary>
        /// <returns>The previous color of the cell.</returns>
        public ICommand Execute()
        {
            var undo = new RestoreColor(this.cell, this.cell.BGColor); // restore text
            this.cell.BGColor = this.color; // set text
            return undo; // return stored value
        }
    }
}
