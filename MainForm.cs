using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLifeForm
{
    public partial class MainForm : Form
    {
        public int cols;
        public int rows;
        Random rnd = new();
        public int Resolution = 1;
        BackgroundWorker bgWorker = new();
        Bitmap drawingSurface;
        Graphics? GFX;
        Cell[,] Cells;
        Cell[,] OldCells;
        public MainForm()
        {
            InitializeComponent();
            rows = 900 / Resolution;
            cols = 1000/Resolution;
            Cells = new Cell[cols, rows];
            OldCells = new Cell[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Cells[i, j] = new(i, j, CreateState());
                    OldCells[i, j] = new(i, j, CreateState());
                }
            }
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += DoWorkHandler;
            drawingSurface = new Bitmap(1000, 900);
            GFX = Graphics.FromImage(drawingSurface);
            GFX.FillRectangle(Brushes.White, 0, 0, 1000, 900);
            draw(null, null);
        }
        public int CreateState()
        {
            int State;
            int random = rnd.Next(101);
            if (random > 97)
            {
                State = 1;
            }
            else
            {
                State = 0;
            }
            return State;
        }
        private void Reset()
        {
            button1.Text = "Start";
            Cells = new Cell[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Cells[i, j] = new(i, j, CreateState());
                }
            }
            draw(null, null);
        }
        private void draw(object? sender, DoWorkEventArgs? e)
        {
            drawingSurface = new Bitmap(drawingSurface);
            GFX = Graphics.FromImage(drawingSurface);
            //GFX.FillRectangle(Brushes.White, 0, 0, 1000, 900);
            for (int i=0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    Cell cell = Cells[i, j];
                    if (cell.State != OldCells[i, j].State)
                        GFX.FillRectangle(cell.GetColor(), i * Resolution, j * Resolution, Resolution, Resolution);
                    
                }
            }
            
            pictureBox1.Image = drawingSurface;
        }
        private void DoWorkHandler(object? sender, DoWorkEventArgs e)
        {

            while (true)
            {
                draw(sender, e);
                UpdateState(sender, e);
                if (bgWorker.CancellationPending)
                {
                    break;
                }
            }
        }

        private void UpdateState(object? sender, DoWorkEventArgs? e)
        {
            Cell[,] CellsCopy = new Cell[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    CellsCopy[i, j] = new(i, j, Cells[i, j].State);
                    OldCells[i, j] = new(i, j, Cells[i, j].State);
                }
            }

            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    Cell cell = Cells[i, j];
                    int Neighbors = CountNeighbors(i, j);
                    if (cell.State == 0 && Neighbors == 3)
                    {
                        CellsCopy[i, j].State = 1;
                    } else if (cell.State == 1 && (Neighbors < 2 || Neighbors > 4))
                    {
                        CellsCopy[i, j].State = 0;
                    }
                }
            }
            OldCells = Cells;
            Cells = CellsCopy;

        }

        private int CountNeighbors(int x, int y)
        {
            int sum = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;
                    sum += Cells[col, row].State;
                }
            }
            sum -= Cells[x, y].State;
            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bgWorker.IsBusy) { bgWorker.CancelAsync(); button1.Text = "Start"; return; }
            bgWorker.RunWorkerAsync();
            button1.Text = "Stop";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            draw(sender, null);
            UpdateState(sender, null);
        }
    }
}
