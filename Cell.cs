using System.Drawing;

namespace GameOfLifeForm
{
    public partial class MainForm
    {
        class Cell
        {
            public int State;
            public int i, j;
            public Cell(int i, int j)
            {
                this.j = j;
                this.i = i;
                
            }
            public Cell(int i, int j, int State) :this(i, j)
            {
                this.State = State;
            }
            public Brush GetColor()
            {
                if (State == 0)
                {
                    return Brushes.White;
                }
                else
                {
                    return Brushes.Black;
                }
            }
        }
    }
}
