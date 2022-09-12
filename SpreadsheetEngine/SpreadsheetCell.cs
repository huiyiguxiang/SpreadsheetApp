// <copyright file="SpreadsheetCell.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Instantiates instance of Cell.
    /// </summary>
    public class SpreadsheetCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="row">Row coordinate of cell.</param>
        /// <param name="col">Column coordinate of cell.</param>
        public SpreadsheetCell(int row, int col)
            : base(row, col)
        {
        }
    }
}
