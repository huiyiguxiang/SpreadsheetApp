// <copyright file="Form1.cs" company="Linh Stitsel">
// Copyright (c) Linh Stitsel. All rights reserved.
// </copyright>

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// The class Form1 for the GUI.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet sheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();

            this.sheet = new Spreadsheet(50, 26);

            this.sheet.CellPropertyChanged += this.CellPropertyChangedHandler;

            // this.dataGridView1.CellBeginEdit += this.dataGridView1_CellBeginEdit;
            // this.dataGridView1.CellEndEdit += this.dataGridView1_CellEndEdit;
            this.UpdateEditMenu();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // variables
            int letter = 0; // for column count
            this.dataGridView1.ColumnCount = 26; // for column count
            this.dataGridView1.RowCount = 50; // for row count

            // make cols a to z
            for (int i = 65; i < 91; i++)
            {
                this.dataGridView1.Columns[letter].Name = ((char)i).ToString();
                ++letter;
            }

            // make rows 1 to 50
            for (int i = 1; i <= 50; i++)
            {
                // make sure row index 0 is labelled as 1 and so on then convert to string and add
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        private void CellPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CellChanged")
            {
                // get cell to update
                Cell updatedCell = sender as Cell;

                // only change if cell is changed
                if (updatedCell != null)
                {
                    int row = updatedCell.RowIndex;
                    int column = updatedCell.ColumnIndex;
                    this.dataGridView1.Rows[row].Cells[column].Value = updatedCell.Value;
                }
            }
            else if (e.PropertyName == "ColorChanged")
            {
                // get cell to update
                Cell updatedCell = sender as Cell;

                if (updatedCell != null)
                {
                    // find row and column
                    int row = updatedCell.RowIndex;
                    int col = updatedCell.ColumnIndex;

                    // get the color from the cell
                    int intColor = (int)updatedCell.BGColor;
                    Color color = Color.FromArgb(intColor);

                    // update cell in the form
                    this.dataGridView1.Rows[row].Cells[col].Style.BackColor = color;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // variables for random
            Random rnd = new Random();

            // add text to 50 random cells
            for (int i = 0; i < 50; i++)
            {
                int randRow = rnd.Next(0, 50);
                int randCol = rnd.Next(0, 26);

                Cell cell = this.sheet.GetCell(randRow, randCol);
                cell.Text = "Hello World!";
            }

            // add text to column B
            for (int i = 0; i < this.sheet.RowCount; i++)
            {
                Cell cell = this.sheet.GetCell(i, 1);
                cell.Text = "This is cell B" + (i + 1);
            }

            // add text to column A
            for (int i = 0; i < this.sheet.RowCount; i++)
            {
                Cell cell = this.sheet.GetCell(i, 0);
                cell.Text = "=B" + (i + 1);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            // get cell to update
            Cell updatedCell = this.sheet.GetCell(row, col);

            if (updatedCell != null)
            {
                // match value to text
                this.dataGridView1.Rows[row].Cells[col].Value = updatedCell.Text;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            // get cell to update
            Cell updatedCell = this.sheet.GetCell(row, col);

            // check if cell was actually changed
            bool checkEdit = true;

            // create for text change
            RestoreText[] undo = new RestoreText[1];

            // store if undo later
            string oldText = updatedCell.Text;

            // instantiate
            undo[0] = new RestoreText(updatedCell, oldText);

            if (updatedCell != null)
            {
                try
                {
                    // match value to text
                    if (updatedCell.Text == this.dataGridView1.Rows[row].Cells[col].Value.ToString())
                    {
                        checkEdit = false;
                    }

                    updatedCell.Text = this.dataGridView1.Rows[row].Cells[col].Value.ToString();
                }
                catch (NullReferenceException)
                {
                    if (updatedCell.Text == null)
                    {
                        checkEdit = false;
                    }

                    updatedCell.Text = " ";
                }

                this.dataGridView1.Rows[row].Cells[col].Value = updatedCell.Value;

                if (checkEdit == true)
                {
                    this.sheet.AddUndo(new MultiCmd(undo, "cell text change"));
                }

                this.UpdateEditMenu();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void changeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // prompt user for color
            ColorDialog colorDialog = new ColorDialog();

            // for color change
            List<RestoreColor> undo = new List<RestoreColor>();

            // if OK
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // loop through all selected cells
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    // convert the form cell to a spreadsheet class cell
                    Cell updatedCell = this.sheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    // for color undo
                    uint oldColor = updatedCell.BGColor;

                    // set to white if 0
                    if (oldColor == 0)
                    {
                        oldColor = (uint)Color.White.ToArgb();
                    }

                    // update color
                    updatedCell.BGColor = (uint)colorDialog.Color.ToArgb();

                    // add to undo list
                    RestoreColor undoColor = new RestoreColor(updatedCell, oldColor);
                    undo.Add(undoColor);
                }
            }

            // add all of the color changes to the undo stack
            this.sheet.AddUndo(new MultiCmd(undo.ToArray(), "changing cell background color"));
            this.UpdateEditMenu();
        }

        /// <summary>
        /// Undo menu item click.
        /// </summary>
        /// <param name="sender">Reference to object.</param>
        /// <param name="e">Information about the event.</param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Undo();
            this.UpdateEditMenu();
        }

        /// <summary>
        /// Redo menu item click.
        /// </summary>
        /// <param name="sender">Reference to object.</param>
        /// <param name="e">Information about the event.</param>
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Redo();
            this.UpdateEditMenu();
        }

        private void UpdateEditMenu()
        {
            // retrieve menu item
            ToolStripMenuItem editMenuItems = this.menuStrip1.Items[1] as ToolStripMenuItem;

            // loop through undo and redo option
            foreach (ToolStripMenuItem menuItem in editMenuItems.DropDownItems)
            {
                // if undo item
                if (menuItem.Text.Substring(0, 4) == "Undo")
                {
                    menuItem.Enabled = this.sheet.CanUndo; // enable the undo cmd

                    menuItem.Text = "Undo " + this.sheet.UndoCmd; // update text option
                }

                // if redo item
                else if (menuItem.Text.Substring(0, 4) == "Redo")
                {
                    menuItem.Enabled = this.sheet.CanRedo; // enable the redo cmd

                    menuItem.Text = "Redo " + this.sheet.RedoCmd; // update text option
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new open dialog
            OpenFileDialog open = new OpenFileDialog();

            // filter for xml
            open.Filter = "XML files (*.xml)|*.xml";

            // if ok
            if (open.ShowDialog() == DialogResult.OK)
            {
                // clear the sheet
                this.Clear();

                // make new outfile for saving
                Stream infile = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);

                // load file
                this.sheet.Load(infile);

                // release resources
                infile.Dispose();

                // clear undo redo stack
                this.sheet.ClearStack();
            }

            // update undo redo
            this.UpdateEditMenu();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new save dialog
            SaveFileDialog save = new SaveFileDialog();

            // filter xml
            save.Filter = "XML files (*.xml)|*.xml";

            // if ok
            if (save.ShowDialog() == DialogResult.OK)
            {
                // make new outfile for saving
                Stream outfile = new FileStream(save.FileName, FileMode.Create, FileAccess.Write);

                // save
                this.sheet.Save(outfile);

                // release resources
                outfile.Dispose();
            }
        }

        // clear undo redo
        private void Clear()
        {
            // loop through every cell in the spreadsheet
            for (int i = 0; i < this.sheet.RowCount; i++)
            {
                for (int j = 0; j < this.sheet.ColumnCount; j++)
                {
                    // clear every cell
                    Cell updateCell = this.sheet.GetCell(i, j);
                    updateCell.Clear();
                }
            }
        }
    }
}
