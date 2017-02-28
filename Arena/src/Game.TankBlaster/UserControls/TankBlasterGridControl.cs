using System.Windows;
using System.Windows.Controls;

namespace Game.TankBlaster.UserControls
{
    public class TankBlasterGridControl : Grid
    {
        public void Init(int xSize, int ySize, int tileSize)
        {
            for (int i = 0; i < xSize; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition(){Width = new GridLength(tileSize)});
            }

            for (int i = 0; i < ySize; i++)
            {
                RowDefinitions.Add(new RowDefinition(){Height = new GridLength(tileSize)});
            }
        }
        
        public void AddElement(UIElement element, int x, int y)
        {
            if(element == null)
                return;
            
            element.SetValue(ColumnProperty, x);
            element.SetValue(RowProperty, y);

            if (element is FrameworkElement)
            {
                var elementWithTooltip = (FrameworkElement) element;
                elementWithTooltip.ToolTip = new ToolTip()
                {
                    Content = new Point(x, y),
                    ContentStringFormat = "({0})"
                };
            }
            
            Children.Add(element);
        }
    }
}
