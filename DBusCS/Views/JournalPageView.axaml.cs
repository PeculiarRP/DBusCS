using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DBusCS.ViewModels;
using DBusCS.utils;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;
using System.Diagnostics;

namespace DBusCS.Views;

public partial class JournalPageView : UserControl
{

    public JournalPageView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {
        var viewModel = (JournalPageViewModel)this.DataContext;
        OnPropertyChanged();
        viewModel.OnRefresh += OnPropertyChanged;
    }

    private void OnPropertyChanged(string flag = "j")
    {
        var dataGrid = this.FindControl<DataGrid>("dataGrid");
        var viewModel = (JournalPageViewModel)this.DataContext;
        var objectList = viewModel.DataObjects;
        dataGrid.Columns.Clear();

        if (objectList != null)
        {
            if (objectList[0] is Subject)
            {
                var column = new DataGridTextColumn()
                {
                    Header = "Предметы",
                    Binding = new Avalonia.Data.Binding("SubjectName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                };
                dataGrid.Columns.Add(column);
            }
            else
            {
                if (objectList[0] is Student st)
                {
                    dataGrid.Columns.Add(new DataGridTextColumn()
                    {
                        Header = "Имя",
                        Binding = new Avalonia.Data.Binding("Name"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
                    });
                    dataGrid.Columns.Add(new DataGridTextColumn()
                    {
                        Header = "Фамилия",
                        Binding = new Avalonia.Data.Binding("Surname"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
                    });
                    dataGrid.Columns.Add(new DataGridTextColumn()
                    {
                        Header = "Класс",
                        Binding = new Avalonia.Data.Binding("StudentClass"),
                        Width = flag == "s" ? new DataGridLength(1, DataGridLengthUnitType.Star) : new DataGridLength(1, DataGridLengthUnitType.Auto)

                    });
                    if (flag == "j")
                    {
                        for(int i=0; i < st.Grades.Count; i++)
                        {
                            dataGrid.Columns.Add(new DataGridTextColumn()
                            {
                                Header = st.Grades[i].SubjectName,
                                Binding = new Avalonia.Data.Binding($"Grades[{i}].Grade"),
                                Width = (i - 1) == st.Grades.Count ? new DataGridLength(1, DataGridLengthUnitType.Star) : new DataGridLength(0.3, DataGridLengthUnitType.Star)
                            });
                        }
                    }
                }
            }
        }
    }
}