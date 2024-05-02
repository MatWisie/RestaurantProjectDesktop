using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
using RestaurantDesktop.Aspect;
using RestaurantDesktop.Interface;
using RestaurantDesktop.Model;
using RestaurantDesktop.Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RestaurantDesktop.ViewModel
{
    public partial class TablesViewModel : ObservableObject
    {
        private readonly IGridService _gridService;
        private readonly ITableService _tableService;
        private readonly IConfigurationService _configurationService;
        private readonly IAuthService _authService;
        private readonly IJsonService _jsonService;

        public TablesViewModel(IGridService gridService, ITableService tableService, IConfigurationService configurationService, IAuthService authService, IJsonService jsonService)
        {
            _gridService = gridService;
            _tableService = tableService;
            _configurationService = configurationService;
            _authService = authService;
            _jsonService = jsonService;
            tables = new List<TableWithIdModel>();
            modifiedTables = new List<TableWithIdModel>();

            BuildTableGrid();

        }

        [ObservableProperty]
        private Grid tableGrid;

        [ObservableProperty]
        private TableWithIdModel? selectedTable;

        private TableGridModel tableGridModel;
        private List<TableWithIdModel> tables;
        private List<TableWithIdModel> modifiedTables;

        public string ModifiedTablesMessage
        {
            get
            {
                if (modifiedTables.Count > 0)
                {
                    ModifiedMessageVisibility = Visibility.Visible;
                    return $"Modified: {modifiedTables.Count}";
                }
                ModifiedMessageVisibility = Visibility.Hidden;
                return "";
            }
        }

        [ObservableProperty]
        private Visibility modifiedMessageVisibility = Visibility.Hidden;

        [AsyncLoading]
        [RelayCommand]
        private async Task BuildTableGrid()
        {
            var dishResponse = await _gridService.GetGrid(_configurationService.GetConfiguration("UserToken"));
            var tablesResponse = await _tableService.GetTables(_configurationService.GetConfiguration("UserToken"));

            _authService.CheckIfLogout(dishResponse.StatusCode);
            _authService.CheckIfLogout(tablesResponse.StatusCode);

            if (dishResponse.IsSuccessful && dishResponse.Content != null && tablesResponse.IsSuccessful && tablesResponse.Content != null)
            {
                tables.Clear();
                modifiedTables.Clear();

                tableGridModel = _jsonService.ExtractTableGridDataFromJson(dishResponse.Content);
                tables = _jsonService.ExtractTablesFromJson(tablesResponse.Content);
                TableGrid = _gridService.BuildGrid(tableGridModel, tables);
                SubscribeToDragDropEvents();
            }
        }

        private void SubscribeToDragDropEvents()
        {
            TableGrid.PreviewMouseDown += TableGrid_PreviewMouseMove;
            tables.Clear();
            foreach (var item in TableGrid.Children)
            {
                if (item is Rectangle rectangle)
                {
                    rectangle.Drop += Rectangle_Drop;
                }
                else if (item is PackIconBoxIcons tableIcon)
                {
                    tableIcon.MouseDown += TableIcon_MouseDown;
                    tableIcon.MouseLeftButtonDown += TableIcon_MouseLeftButtonDown;
                    if (tableIcon.DataContext is TableWithIdModel table)
                    {
                        tables.Add(_tableService.CopyTableModel(table));
                    }
                }
            }
        }

        [AsyncLoading]
        [RelayCommand]
        private async Task SaveModifiedTablesPlacement()
        {
            foreach (var table in modifiedTables.ToList())
            {
                var result = await _tableService.EditTable(_configurationService.GetConfiguration("UserToken"), table, table.Id);
                foreach (var mTable in modifiedTables)
                {
                    var tableInOrg = tables.Where(e => e.Id == mTable.Id).FirstOrDefault();
                    if (tableInOrg != null)
                    {
                        tables.Remove(tableInOrg);
                        tables.Add(_tableService.CopyTableModel(mTable));
                    }
                }
                modifiedTables.Remove(table);
            }
            OnPropertyChanged(nameof(ModifiedTablesMessage));
        }

        [AsyncLoading]
        [RelayCommand]
        private async Task DeleteTable()
        {
            if (MessageBox.Show("Are you sure you want to delete this table?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var tablesResponse = await _tableService.DeleteTable(_configurationService.GetConfiguration("UserToken"), SelectedTable.Id);
                if (tablesResponse.IsSuccessful)
                {
                    var tableToDelete = tables.FirstOrDefault(e => e.Id == SelectedTable.Id);
                    if (tableToDelete != null)
                        tables.Remove(tableToDelete);
                    var orgTableToDelete = tables.FirstOrDefault(e => e.Id == SelectedTable.Id);
                    if (orgTableToDelete != null)
                        tables.Remove(orgTableToDelete);
                    var modTableToDelete = modifiedTables.FirstOrDefault(e => e.Id == SelectedTable.Id);
                    if (modTableToDelete != null)
                    {
                        modifiedTables.Remove(modTableToDelete);
                        OnPropertyChanged(nameof(ModifiedTablesMessage));
                    }
                    var tableInGrid = TableGrid.Children.OfType<PackIconBoxIcons>().Where(e => e.DataContext is TableWithIdModel && ((TableWithIdModel)e.DataContext).Id == SelectedTable.Id).FirstOrDefault();
                    if (tableInGrid != null && tableInGrid.DataContext is TableWithIdModel model)
                    {
                        TableGrid.Children.Remove(tableInGrid);
                        SelectedTable = null;
                    }
                }
                _authService.CheckIfLogout(tablesResponse.StatusCode);
            }
        }

        private void TableIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is PackIconBoxIcons icon)
            {
                var tables = TableGrid.Children.OfType<PackIconBoxIcons>();
                foreach (var t in tables)
                {
                    t.Background = new SolidColorBrush(Colors.White);
                }

                icon.Background = new SolidColorBrush(Colors.LightBlue);
                if (icon.DataContext is TableWithIdModel table)
                {
                    SelectedTable = table;
                }
            }
        }

        private void UpdateTableGridDataContext(PackIconBoxIcons droppedTableIcon, int gridRow, int gridColumn)
        {
            if (droppedTableIcon.DataContext is TableWithIdModel tableWithIdModel)
            {
                tableWithIdModel.GridRow = gridRow;
                tableWithIdModel.GridColumn = gridColumn;

            }
        }
        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PackIconBoxIcons)))
            {
                PackIconBoxIcons droppedTableIcon = e.Data.GetData(typeof(PackIconBoxIcons)) as PackIconBoxIcons;
                if (droppedTableIcon != null)
                {
                    Point mousePosition = e.GetPosition(TableGrid);
                    HitTestResult hitTestResult = VisualTreeHelper.HitTest(TableGrid, mousePosition);

                    if (hitTestResult != null && hitTestResult.VisualHit is FrameworkElement targetElement)
                    {
                        int newRow = Grid.GetRow(targetElement);
                        int newColumn = Grid.GetColumn(targetElement);

                        foreach (var child in TableGrid.Children)
                        {
                            if (child is PackIconBoxIcons icon && Grid.GetRow(icon) == newRow && Grid.GetColumn(icon) == newColumn)
                            {
                                return;
                            }
                        }

                        Grid.SetRow(droppedTableIcon, newRow);
                        Grid.SetColumn(droppedTableIcon, newColumn);
                        UpdateTableGridDataContext(droppedTableIcon, newRow, newColumn);
                        if (droppedTableIcon.DataContext is TableWithIdModel model)
                        {
                            if (!modifiedTables.Any(e => e.Id == model.Id))
                            {
                                modifiedTables.Add(model);
                                OnPropertyChanged(nameof(ModifiedTablesMessage));
                                OnPropertyChanged(nameof(SelectedTable));
                            }
                            else
                            {
                                var mTable = modifiedTables.Where(e => e.Id == model.Id).FirstOrDefault();
                                if (mTable != null)
                                {
                                    var orgTable = tables.Where(e => e.Id == model.Id).FirstOrDefault();
                                    if (orgTable != null)
                                    {
                                        if (mTable.Id == orgTable.Id && mTable.GridRow == orgTable.GridRow && mTable.GridColumn == orgTable.GridColumn)
                                        {
                                            modifiedTables.Remove(mTable);
                                        }
                                    }
                                    OnPropertyChanged(nameof(ModifiedTablesMessage));
                                    OnPropertyChanged(nameof(SelectedTable));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TableGrid_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Grid grid)
            {
                if (e.OriginalSource is PackIconBoxIcons tableIcon)
                {
                    DataObject data = new DataObject(typeof(PackIconBoxIcons), tableIcon);
                    DragDrop.DoDragDrop(grid, data, DragDropEffects.Move);
                }
            }
        }

        private void TableIcon_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is PackIconBoxIcons tableIcon)
            {
                DragDrop.DoDragDrop(tableIcon, tableIcon, DragDropEffects.Move);
            }
        }

        [RelayCommand]
        private void GoToMainMenu()
        {
            MessageService.SendChangeViewMessage(App.Current.Services.GetService<MainMenuViewModel>());
        }

        [RelayCommand]
        private void GoToEditTable()
        {
            if (SelectedTable != null)
                MessageService.SendChangeViewMessage(new EditTableViewModel(SelectedTable, App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>(), App.Current.Services.GetService<ITableService>()));
        }

        [RelayCommand]
        private void GoToEditInfrastructure()
        {
            MessageService.SendChangeViewMessage(new EditInfrastructureViewModel(tableGridModel, App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>(), App.Current.Services.GetService<IGridService>()));
        }

        [RelayCommand]
        private void GoToAddTable()
        {
            MessageService.SendChangeViewMessage(new AddTableViewModel(tableGridModel, tables, App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>(), App.Current.Services.GetService<ITableService>()));
        }
    }
}
