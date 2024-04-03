﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
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
            orgintalTables = new List<TableWithIdModel>();
            modifiedTables = new List<TableWithIdModel>();

            BuildTableGrid();

        }

        [ObservableProperty]
        private Grid tableGrid;

        [ObservableProperty]
        private TableWithIdModel? selectedTable;

        private TableGridModel tableGridModel;
        private List<TableWithIdModel> tables;
        private List<TableWithIdModel> orgintalTables;
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


        [RelayCommand]
        private async Task BuildTableGrid()
        {
            MessageService.SendLoadingBegin();
            var dishResponse = await _gridService.GetGrid(_configurationService.GetConfiguration("UserToken"));
            var tablesResponse = await _tableService.GetTables(_configurationService.GetConfiguration("UserToken"));

            _authService.CheckIfLogout(dishResponse.StatusCode);
            _authService.CheckIfLogout(tablesResponse.StatusCode);

            if (dishResponse.IsSuccessful && dishResponse.Content != null && tablesResponse.IsSuccessful && tablesResponse.Content != null)
            {
                tableGridModel = _jsonService.ExtractTableGridDataFromJson(dishResponse.Content);
                TableGrid = _gridService.BuildGrid(tableGridModel);
                BuildRectanglesForGrid(TableGrid);
                tables = _jsonService.ExtractTablesFromJson(tablesResponse.Content);
                BuildTablesForGrid(tables, TableGrid);
            }
            MessageService.SendLoadingEnd();
        }

        [RelayCommand]
        private async Task SaveModifiedTablesPlacement()
        {
            MessageService.SendLoadingBegin();
            foreach (var table in modifiedTables.ToList())
            {
                var result = await _tableService.EditTable(_configurationService.GetConfiguration("UserToken"), table, table.Id);
                modifiedTables.Remove(table);
            }
            OnPropertyChanged(nameof(ModifiedTablesMessage));
            MessageService.SendLoadingEnd();
        }

        [RelayCommand]
        private async Task DeleteTable()
        {
            if (MessageBox.Show("Are you sure you want to delete this table?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageService.SendLoadingBegin();
                var tablesResponse = await _tableService.DeleteTable(_configurationService.GetConfiguration("UserToken"), SelectedTable.Id);
                if (tablesResponse.IsSuccessful)
                {
                    var tableToDelete = tables.FirstOrDefault(e => e.Id == SelectedTable.Id);
                    if (tableToDelete != null)
                        tables.Remove(tableToDelete);
                    var orgTableToDelete = orgintalTables.FirstOrDefault(e => e.Id == SelectedTable.Id);
                    if (orgTableToDelete != null)
                        orgintalTables.Remove(orgTableToDelete);
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
                MessageService.SendLoadingEnd();
            }
        }

        [RelayCommand]
        private void GoToAddTable()
        {
            MessageService.SendChangeViewMessage(new AddTableViewModel(tableGridModel, tables, App.Current.Services.GetService<IConfigurationService>(), App.Current.Services.GetService<IAuthService>(), App.Current.Services.GetService<ITableService>()));
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

        private void BuildRectanglesForGrid(Grid grid)
        {
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                {
                    Rectangle rectangle = _gridService.BuildDropRectangle();
                    Grid.SetRow(rectangle, i);
                    Grid.SetColumn(rectangle, j);
                    rectangle.Drop += Rectangle_Drop;
                    grid.Children.Add(rectangle);
                }
            }
        }

        private void BuildTablesForGrid(List<TableWithIdModel> tables, Grid grid)
        {
            foreach (var table in tables)
            {
                var tableIcon = _gridService.BuildPackIcon(table);

                Grid.SetRow(tableIcon, table.GridRow);
                Grid.SetColumn(tableIcon, table.GridColumn);

                tableIcon.MouseDown += TableIcon_MouseDown;
                tableIcon.MouseLeftButtonDown += TableIcon_MouseLeftButtonDown;
                grid.PreviewMouseMove += TableGrid_PreviewMouseMove;
                orgintalTables.Add(_tableService.CopyTableModel(table));

                grid.Children.Add(tableIcon);
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
                                    var orgTable = orgintalTables.Where(e => e.Id == model.Id).FirstOrDefault();
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
    }
}
