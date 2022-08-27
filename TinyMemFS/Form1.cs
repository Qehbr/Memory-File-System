using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyMemFS;
using Microsoft.VisualBasic;

namespace TinyMemFS
{
    public partial class Form1 : Form
    {
        private TinyMemFSConsole fs = new TinyMemFSConsole();
        public Form1()
        {
            InitializeComponent();
        }

        private void updateGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (string file in fs.listFiles())
            {
                string[] a = file.Split(',');
                dataGridView1.Rows.Add(file.Split(','));
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string fileName = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                fs.remove(fileName);
                updateGridView();

            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Save")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
                string filter = extension + " files (*." + extension + ")|*." + extension + "|All files (*.*)|*.*";
                dialog.Filter = filter;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fs.save(fileName, dialog.FileName);
                }
            }
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string fileToAdd = openFileDialog1.FileName;
                string fileName = fileToAdd.Substring(fileToAdd.LastIndexOf("\\") + 1);
                fs.add(fileName, fileToAdd);
                updateGridView();

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void encryptAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = Interaction.InputBox("Write Key To Encrypt All Files", "Encrypt All Files", "My Key", 400, 400);
            if (key == "")
            {
                return;
            }
            if (!fs.encrypt(key))
            {
                MessageBox.Show("Couldn't encrypt (Try to add some files or decrypt firstly)", "Error");
            }
            else
            {
                MessageBox.Show("Files are encrypted successfully", "Success");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by: 340915149 and 322823261 for Operating Systems Course 2022", "About");
        }

        private void decryptAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = Interaction.InputBox("Write Key To Decrypt All Files", "Decrypt All Files", "My Key", 400, 400);
            if (key == "")
            {
                return;
            }
            if (!fs.decrypt(key))
            {
                MessageBox.Show("Couldn't decrypt (Try different key, decrypt firstly, or add some files)", "Error");
            }
            else
            {
                MessageBox.Show("Files are encrypted successfully", "Success");
            }
        }
    }
}
