using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StartupManager_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
        }

        public void refresh()
        {
            this.Controls.Clear();

            PictureBox exitButton = new PictureBox() { Name = "pictureBox2", Size = new Size(48, 16), Location = new Point(this.Width - 48, 0), BackColor = Color.FromArgb(64, 64, 64) };
            exitButton.Click += (s, e) => { Environment.Exit(0); }; exitButton.MouseEnter += (s, e) => { exitButton.BackColor = Color.FromArgb(30, 30, 30); }; exitButton.MouseLeave += (s, e) => { exitButton.BackColor = Color.FromArgb(64, 64, 64); };
            this.Controls.Add(exitButton);

            Button addProgram = new Button() { Name = "AddProgram", Text = "Add new program", Size = new Size(this.Width + 4, 30), Location = new Point(-2, this.Height - 28), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.Black };
            addProgram.Click += (s, e) =>
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Title = "Select file to add to startup";
                openFileDialog1.ShowDialog();
                try
                {
                    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), openFileDialog1.SafeFileName))) File.Copy(openFileDialog1.FileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), openFileDialog1.SafeFileName));
                    else
                    {
                        File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), openFileDialog1.SafeFileName), File.ReadAllBytes(openFileDialog1.FileName));
                    }
                }
                catch
                {
                    MessageBox.Show("An error occured why trying to add the program the startup. Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                refresh();
            };
            this.Controls.Add(addProgram);

            Label title = new Label() { Name = "label1", Location = new Point(10, 7), Size = new Size(222, 28), BackColor = Color.DodgerBlue, Text = "StartupManager+", ForeColor = Color.White, Font = new System.Drawing.Font("Gotham Bold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };
            title.MouseDown += (s, e) =>
            {
                formO = this.Location;
                mouseO = MousePosition;
                timer1.Start();
            };
            title.MouseUp += (s, e) =>
            {
                timer1.Stop();
                if (mouseO == MousePosition) refresh();
            };
            this.Controls.Add(title);

            PictureBox topBar = new PictureBox() { Name = "pictureBox1", Location = new Point(0, 0), Size = new Size(this.Width, 43), BackColor = Color.DodgerBlue };
            topBar.MouseDown += (s, e) =>
            {
                formO = this.Location;
                mouseO = MousePosition;
                timer1.Start();
            };
            topBar.MouseUp += (s, e) =>
            {
                timer1.Stop();
            };
            this.Controls.Add(topBar);

            int count = 0;
            var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Startup)).Where(x => new FileInfo(x).Length != 0 && !x.ToLower().Contains("desktop.ini")).ToList();
            if(files.Count > 0) foreach (var file in files)
                {
                    Label name = new Label();
                    name.AutoSize = true;
                    name.Font = new Font("Gotham Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    name.Location = new Point(10, 43 + 10 + count * 40);
                    name.Size = new Size(100, 200);
                    name.TabIndex = 1;
                    name.Text = file.Split('\\').LastOrDefault();
                    name.Name = $"application{count}";
                    name.ForeColor = Color.Black;
                    name.BackColor = Color.White;
                    this.Controls.Add(name);

                    PictureBox splitter = new PictureBox();
                    splitter.Name = $"splitter{count}";
                    splitter.BackColor = Color.Gray;
                    splitter.Location = new Point(0, 43 + 37 + count * 40);
                    splitter.Size = new Size(this.Width, 1);
                    this.Controls.Add(splitter);

                    Button remove = new Button();
                    remove.Name = $"remove{count}";
                    remove.Text = "Remove";
                    remove.Location = new Point(this.Width - 70, 43 + 6 + count * 40);
                    remove.Click += (s, e) => { File.WriteAllBytes(file, new byte[0]); refresh(); };
                    remove.Size = new Size(60, 25);
                    this.Controls.Add(remove);

                    count++;
                }
            else
            {
                Label noPrograms = new Label() { Name = "NoProgramWarning", Text = "No programs yet :(\nYou can add some by pressing the button below", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Gotham Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))), ForeColor = Color.Gray, Location = new Point(0, 0) };
                this.Controls.Add(noPrograms);
            }
        }


        Point formO;
        Point mouseO;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            formO = this.Location;
            mouseO = MousePosition;
            timer1.Start();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Left = MousePosition.X - (mouseO.X - formO.X);
            this.Top = MousePosition.Y - (mouseO.Y - formO.Y);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
