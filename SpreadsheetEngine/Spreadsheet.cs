// <copyright file="Spreadsheet.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>
namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// The spreadsheet class thats consists of a 2D array spreadsheet made up of cell objects.
    /// </summary>
    public class Spreadsheet
    {
        private readonly Cell[,] spreadsheetArray; // new spreadsheet array
        private Dictionary<Cell, List<Cell>> dependencies;

        private Stack<MultiCmd> undo = new Stack<MultiCmd>();
        private Stack<MultiCmd> redo = new Stack<MultiCmd>();

        private HashSet<Cell> circCells = new HashSet<Cell>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="numRows">Number of rows in the sheet.</param>
        /// <param name="numCols"> Number of columns in the sheet.</param>
        public Spreadsheet(int numRows, int numCols)
        {
            this.dependencies = new Dictionary<Cell, List<Cell>>();
            this.spreadsheetArray = new Cell[numRows, numCols]; // initialize new array
            this.ColumnCount = numCols; // adjusts ColumnCount to number of columns in spreadsheet
            this.RowCount = numRows; // adjusts ColumnCount to number of columns in spreadsheet

            // forms the 2d array
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    Cell cell = new SpreadsheetCell(i, j); // make new cell

                    this.spreadsheetArray[i, j] = cell; // add to spreadsheet

                    cell.PropertyChanged += this.CellText; // subscribe to cell
                }
            }
        }

        /// <summary>
        /// The CellPropertyChanged event handler. To notify when the cell is modified.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { }; // event handler

        /// <summary>
        /// Gets or sets columns in spreadsheet.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Gets or sets rows in spreadsheet.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether undo stack is empty or not.
        /// </summary>
        public bool CanUndo
        {
            get { return this.undo.Count != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether redo stack is empty or not.
        /// </summary>
        public bool CanRedo
        {
            get { return this.redo.Count != 0; }
        }

        /// <summary>
        /// Gets the undo command name type.
        /// </summary>
        public string UndoCmd
        {
            get
            {
                if (this.CanUndo)
                {
                    return this.undo.Peek().Command;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the redo command name type.
        /// </summary>
        public string RedoCmd
        {
            get
            {
                // check if can redo
                if (this.CanRedo)
                {
                    // return command name
                    return this.redo.Peek().Command;
                }

                // else return nothing
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the cell specified at its coordinate location.
        /// </summary>
        /// <param name="row">The row location of the cell.</param>
        /// <param name="col">The column location of the cell.</param>
        /// <returns>The cell in the spreadsheet array or null when invalid.</returns>
        public Cell GetCell(int row, int col)
        {
            // check if valid cell within bounds
            if (row <= this.RowCount && col <= this.ColumnCount)
            {
                return this.spreadsheetArray[row, col];
            }
            else
            {
                return null; // else return null for invalid
            }
        }

        /// <summary>
        /// Overload GetCell to get cell from address.
        /// </summary>
        /// <param name="name">Address name of cell.</param>
        /// <returns>The cell at the address.</returns>
        public Cell GetCell(string name)
        {
            if (!char.IsLetter(name[0]))
            {
                return null;
            }

            if (!char.IsUpper(name[0]))
            {
                return null;
            }

            // the column is always the first character of the name
            int col = name[0] - 'A';

            int row;
            Cell cell;

            if (!int.TryParse(name.Substring(1), out row))
            {
                return null;
            }

            try
            {
                cell = this.GetCell(row - 1, col);
            }
            catch (Exception e)
            {
                return null;
            }

            return cell;
        }

        /// <summary>
        /// Change the cell's text.
        /// </summary>
        /// <param name="sender">Reference to object.</param>
        /// <param name="e">Information about the event.</param>
        public void CellText(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                // call the overloaded method
                this.CellText(cellToUpdate);
            }
            else if (e.PropertyName == "Color")
            {
                Cell cellToUpdate = sender as Cell;

                this.CellPropertyChanged(cellToUpdate, new PropertyChangedEventArgs("ColorChanged"));
            }
        }

        /// <summary>
        /// Add to undo stack.
        /// </summary>
        /// <param name="undos">Undo history.</param>
        public void AddUndo(MultiCmd undos)
        {
            // add to undo stack
            this.undo.Push(undos);
        }

        /// <summary>
        /// Pop off the undo stack and add to redo stack.
        /// </summary>
        public void Undo()
        {
            // check if can undo
            if (this.CanUndo)
            {
                // get cmd
                MultiCmd commands = this.undo.Pop();

                // push to stack
                this.redo.Push(commands.Execute());
            }
        }

        /// <summary>
        /// Pop off the redo stack and add to the undo stack.
        /// </summary>
        public void Redo()
        {
            // check if can redo
            if (this.CanRedo)
            {
                // get cmd
                MultiCmd commands = this.redo.Pop();

                // push to stack
                this.undo.Push(commands.Execute());
            }
        }

        /// <summary>
        /// Loads an xml file.
        /// </summary>
        /// <param name="infile">Input file.</param>
        public void Load(Stream infile)
        {
            // load xml file
            XDocument file = XDocument.Load(infile);

            // loop through cell tags
            foreach (XElement tag in file.Root.Elements("cell"))
            {
                // get cell
                Cell xmlCell = this.GetCell(tag.Element("name").Value);

                if (tag.Element("text") != null)
                {
                    xmlCell.Text = tag.Element("text").Value.ToString(); // load cell text
                }

                if (tag.Element("backgroundColor") != null)
                {
                    uint xmlColor = Convert.ToUInt32(tag.Element("backgroundColor").Value.ToString()); // convert bgcolor to uint

                    // load the cell's background color
                    xmlCell.BGColor = xmlColor;
                }
            }
        }

        /// <summary>
        /// Saves an xml file from the spreadsheet.
        /// </summary>
        /// <param name="outfile">Output file.</param>
        public void Save(Stream outfile)
        {
            // make new xml file
            XmlWriter file = XmlWriter.Create(outfile);

            // write start tag
            file.WriteStartElement("spreadsheet");

            // loop through every cell
            for (int i = 0; i < this.ColumnCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    Cell curCell = this.spreadsheetArray[i, j];

                    // save modified cell
                    if (curCell.Text != string.Empty || curCell.Value != string.Empty || curCell.BGColor != 0)
                    {
                        file.WriteStartElement("cell");

                        file.WriteElementString("name", curCell.Name.ToString());
                        file.WriteElementString("text", curCell.Text.ToString());
                        file.WriteElementString("backgroundColor", curCell.BGColor.ToString());

                        file.WriteEndElement();
                    }
                }
            }

            // close tag
            file.WriteEndElement();

            // close file
            file.Close();
        }

        /// <summary>
        /// Clears the undo redo stacks.
        /// </summary>
        public void ClearStack()
        {
            this.undo.Clear();
            this.redo.Clear();
        }

        /// <summary>
        /// Set cell value into exptree or create exptree from another cell's value if starting with '='.
        /// </summary>
        /// <param name="updateCell">Cell to be updated.</param>
        private void CellText(Cell updateCell)
        {
            // clear cell
            this.RemoveDep(updateCell);

            // if cell empty
            if (string.IsNullOrEmpty(updateCell.Text))
            {
                updateCell.Value = string.Empty;
            }

            // if cell does not start with =
            else if (updateCell.Text[0] != '=')
            {
                double cellValue;

                // if the cell is double
                if (double.TryParse(updateCell.Text, out cellValue))
                {
                    // create new expression tree with cell
                    ExpressionTree userExpTree = new ExpressionTree(updateCell.Text);

                    // evaluate the tree
                    cellValue = userExpTree.Evaluate();

                    // set the cell value to the cell name
                    userExpTree.SetVariable(updateCell.Name, cellValue);

                    // set the cell value to string
                    updateCell.Value = cellValue.ToString();
                }
                else
                {
                    // if not double then text, set to text
                    updateCell.Value = updateCell.Text;
                }
            }

            // if cell starts with =
            else
            {
                bool error = false;

                // set expression to everything after =
                string exp = updateCell.Text.Substring(1);

                // new exptree
                ExpressionTree userExpTree = new ExpressionTree(exp);

                // evaluate the tree
                userExpTree.Evaluate();

                // all variable names
                string[] varNames = userExpTree.GetVariables();

                // loop every var
                foreach (string variable in varNames)
                {
                    double value = 0.0;
                    Cell curCell = this.GetCell(variable); // get cell based on name

                    // if cell referenced is null
                    if (curCell == null)
                    {
                        updateCell.Value = "!(bad reference)";
                        CellPropertyChanged(updateCell, new PropertyChangedEventArgs("CellChanged"));

                        error = true;
                    }

                    // if cell is self referencing
                    else if (updateCell.Name == variable)
                    {
                        updateCell.Value = "!(self reference)";
                        CellPropertyChanged(updateCell, new PropertyChangedEventArgs("CellChanged"));

                        error = true;
                    }

                    if (error == true)
                    {
                        // if there are cells that depend on the one updated
                        if (dependencies.ContainsKey(updateCell))
                        {
                            // update all dependent cells
                            UpdateDep(updateCell);
                        }

                        return;
                    }

                    double.TryParse(curCell.Value, out value); // get the cell's value

                    userExpTree.SetVariable(variable, value); // set the value to variable

                    // add to list of cells possible for circ references
                    this.circCells.Add(curCell);
                }

                updateCell.Value = userExpTree.Evaluate().ToString(); // set eval of exptree as cell value

                this.AddDep(updateCell, varNames); // add dependencies

                // check for circular reference
                if (this.CheckCell(updateCell))
                {
                    updateCell.Value = "!(circular reference)";
                    this.CellPropertyChanged(updateCell, new PropertyChangedEventArgs("CellChanged"));

                    error = true;
                }

                if (error == true)
                {
                    return;
                }
            }

            // if cell in dependencies
            if (this.dependencies.ContainsKey(updateCell))
            {
                this.UpdateDep(updateCell); // update
            }

            // notify
            this.CellPropertyChanged(updateCell, new PropertyChangedEventArgs("CellChanged"));
            this.circCells.Clear();
        }

        public bool CheckCell(Cell cell)
        {
            if (this.circCells.Add(cell) == false)
            {
                return true;
            }
            return false;
        }

        private void AddDep(Cell refCell, string[] indeps)
        {
            foreach (string indCell in indeps)
            {
                // get the independent cell
                Cell indepCell = this.GetCell(indCell);

                // create a new list if the indeCell is a new key
                if (!this.dependencies.ContainsKey(indepCell))
                {
                    this.dependencies[indepCell] = new List<Cell>();
                }

                // add the referenced cell to the independent cell's references
                this.dependencies[indepCell].Add(refCell);
            }
        }

        private void RemoveDep(Cell referencedCell)
        {
            // loop through each list of dependent cells
            foreach (List<Cell> depCells in this.dependencies.Values)
            {
                // if the dependent list contains the referenced cell
                if (depCells.Contains(referencedCell))
                {
                    // remove it
                    depCells.Remove(referencedCell);
                }
            }
        }

        private void UpdateDep(Cell indepCell)
        {
            // loop through dependent cells
            foreach (Cell depCell in this.dependencies[indepCell].ToArray())
            {
                this.CellText(depCell); // update
            }
        }
    }
}