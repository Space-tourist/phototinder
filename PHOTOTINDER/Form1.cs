using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace PHOTOTINDER
{
    public partial class Form1 : Form
    {
        private List<string> MainFiles { get; set; }
        private List<string> SkippedFiles { get; set; }
        private string SelectedFile { get; set; }

        private readonly FileService _fileService;


        public Form1()
        {
            InitializeComponent();
            MainFiles = new List<string>();
            SkippedFiles = new List<string>();
            _fileService = new FileService("LIKE", "DISLIKE");
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            _fileService.SetSourceDirectory(folderBrowserDialog1.SelectedPath);


            if (!_fileService.SourceDirectoryExists())
            {
                MessageBox.Show("Каталог не найден");
                return;
            }

            _fileService.CreateWorkDirectories();

            FillContentToProcess();

            dislikeButton.Enabled = true;
            skipButton.Enabled = true;
            likeButton.Enabled = true;
        }
        private void FillContentToProcess()
        {
            MainFiles.Clear();
            FileInfo[] files = _fileService.PrintDirectoryContent();

            if (!files.Any())
            {
                dislikeButton.Enabled = false;
                skipButton.Enabled = false;
                likeButton.Enabled = false;
                toolStripStatusLabel2.Text = "Оставшиеся файлы: 0";
                MessageBox.Show("Файлы для обработки отсутствуют");
            }
            else
            {
                if (files.Length == SkippedFiles.Count)
                {
                    SkippedFiles.Clear();
                }
                foreach (FileInfo file in files)
                {
                    if (!SkippedFiles.Contains(file.Name))
                    {
                        MainFiles.Add(file.Name);
                    }
                }

                SelectedFile = MainFiles[0];
                pictureBox.Image = Image.FromFile(_fileService.SourceDirectoryInfo + "\\" + SelectedFile);
                toolStripStatusLabel1.Text = _fileService.SourceDirectoryInfo.FullName;
                toolStripStatusLabel2.Text = "Оставшиеся файлы: " + files.Length.ToString();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dislikeButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image.Dispose();
            pictureBox.Image = null;
            _fileService.MoveFileToDislikeSubDirectory(SelectedFile);
            FillContentToProcess();
        }


        private void skipButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image.Dispose();
            pictureBox.Image = null;
            SkippedFiles.Add(SelectedFile);
            FillContentToProcess();
        }

        private void likeButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image.Dispose();
            pictureBox.Image = null;
            _fileService.MoveFileToLikeSubDirectory(SelectedFile);
            FillContentToProcess();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    dislikeButton.PerformClick();
                    break;
                case Keys.Space:
                    skipButton.PerformClick();
                    break;
                case Keys.Right:
                    likeButton.PerformClick();
                    break;
                case Keys.Escape:
                    exitToolStripMenuItem.PerformClick();
                    break;
            }
            return true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Страдала, но делала \n- А.В. :)", "About");
        }
    }
}