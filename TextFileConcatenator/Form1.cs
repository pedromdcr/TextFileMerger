using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextFileConcatenator
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        string path = @"C:\Users\Pedro\unityWorkspace\ExpertInterface\ExpertInterface\Logs";

        public Form1()
        {
            InitializeComponent();
        }

        bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]");

            if (containsABadCharacter.IsMatch(testName)) { return false; };

            // other checks for UNC, drive-path format, etc

            return true;
        }

        private void mergeFiles(string newFilename)
        {
            if(ofd.FileNames.Length > 0)
            {
                string[] fileNames = ofd.FileNames;
                
                if(IsValidFilename(newFilename) && newFilename.Length > 0)
                {
                    if (!newFilename.EndsWith(".txt"))
                        newFilename += ".txt";
                    const int chunkSize = 2 * 1024; // 2KB
                    using (var output = File.Create(path + "\\" + newFilename))
                    {
                        for(int i = 0; i < fileNames.Length; i++)
                        {
                            if(i != fileNames.Length - 1)
                                File.AppendAllText(fileNames[i], "\n");
                            using (var input = File.OpenRead(fileNames[i]))
                            {
                                var buffer = new byte[chunkSize];
                                int bytesRead;
                                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                }
                            }
                            
                        }
                    }

                }
            }
            MessageBox.Show(this, "Your files have been successfully combined into " + newFilename + "!", "Operation successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Multiselect = true; //sets to multiple selects
            if (Directory.Exists(path))
                ofd.InitialDirectory = path;
            ofd.Filter = "Text files (*.txt)|*.txt";
            ofd.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(ofd.FileNames.Length == 0)
                MessageBox.Show(this, "Please select which files to combine before proceding.", "No files selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox1.Text.Length == 0)
                MessageBox.Show(this, "A name for the new file is required. Try again.", "Missing file name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else mergeFiles(textBox1.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Icon made by Freepik from www.flaticon.com.", "Credit info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}