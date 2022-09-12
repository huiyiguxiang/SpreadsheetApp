// <copyright file="RestoreText.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class that store text history of the cell.
    /// </summary>
    public class RestoreText : ICommand
    {
        private string text;
        private Cell cell;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreText"/> class.
        /// </summary>
        /// <param name="name">The name of the cell.</param>
        /// <param name="cellText">The text of the cell.</param>
        public RestoreText(Cell name, string cellText)
        {
            this.text = cellText;
            this.cell = name;
        }

        /// <summary>
        /// Undo the text.
        /// </summary>
        /// <returns>The previous text of the cell.</returns>
        public ICommand Execute()
        {
            ICommand undo = new RestoreText(this.cell, this.cell.Text); // restore text
            this.cell.Text = this.text; // set text
            return undo; // return stored value
        }
    }
}
