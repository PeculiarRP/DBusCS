using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DBusCS.ViewModels;
using System;

namespace DBusCS.Views;

public partial class AUGradePageView : UserControl
{
    public AUGradePageView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        var context = (AUGradePageViewModel)DataContext;
        

        if (context.Student != null)
        {

        }
    }
}