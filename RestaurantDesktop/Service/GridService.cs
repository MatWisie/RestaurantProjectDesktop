using MahApps.Metro.IconPacks;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestSharp;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RestaurantDesktop.Service
{
    public class GridService : IGridService
    {
        private readonly IGridRepository _gridRepository;
        public GridService(IGridRepository gridRepository)
        {
            _gridRepository = gridRepository;
        }
        public async Task<RestResponse> GetGrid(string userToken)
        {
            return await _gridRepository.GetGrid(userToken);
        }
        public RestResponse EditGrid(string userToken, TableGridModel tableEditModel)
        {
            string json = JsonSerializer.Serialize(tableEditModel);
            return _gridRepository.EditGrid(userToken, json);
        }

        public Grid BuildGrid(TableGridModel gridData, List<TableWithIdModel> tables)
        {
            Grid tmpGrid = new Grid();
            for (int i = 0; i < gridData.NumberOfRows; i++)
            {
                tmpGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < gridData.NumberOfColumns; i++)
            {
                tmpGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            tmpGrid.ShowGridLines = true;


            for (int i = 0; i < tmpGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < tmpGrid.ColumnDefinitions.Count; j++)
                {
                    Rectangle rectangle = BuildDropRectangle();
                    SetElementOnGrid(rectangle, tmpGrid, i, j);
                    tmpGrid.Children.Add(rectangle);
                }
            }

            foreach (var table in tables)
            {
                var tableIcon = BuildPackIcon(table);

                SetElementOnGrid(tableIcon, tmpGrid, table.GridRow, table.GridColumn);

                tmpGrid.Children.Add(tableIcon);
            }


            return tmpGrid;
        }

        public Rectangle BuildDropRectangle()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.HorizontalAlignment = HorizontalAlignment.Stretch;
            rectangle.VerticalAlignment = VerticalAlignment.Stretch;
            rectangle.Fill = new SolidColorBrush(Colors.White);
            rectangle.AllowDrop = true;
            return rectangle;
        }

        public PackIconBoxIcons BuildPackIcon(TableWithIdModel? table)
        {
            PackIconBoxIcons tableIcon = new PackIconBoxIcons()
            {
                Kind = PackIconBoxIconsKind.RegularChair,
                DataContext = table,
                Width = 35,
                Height = 35,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch
            };
            return tableIcon;
        }

        public bool SetElementOnGrid(FrameworkElement element, Grid grid, int row, int column)
        {
            if (column < grid.ColumnDefinitions.Count && row < grid.RowDefinitions.Count)
            {
                Grid.SetRow(element, row);
                Grid.SetColumn(element, column);
                return true;
            }
            return false;
        }
    }
}