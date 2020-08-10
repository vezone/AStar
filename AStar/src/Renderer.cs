using static System.Console;

namespace AStar.src
{
    public class Renderer
    {
        public Renderer()
        { 
            CursorVisible = false;
        }

        public void Refresh()
        {
            SetCursorPosition(0, 0);
        }

        public void RenderGrid(Grid grid)
        {
            for (int y = 0; y < grid.RowLength; y++)
            {
                WriteLine(grid[y]);
            }
        }

    }
}
