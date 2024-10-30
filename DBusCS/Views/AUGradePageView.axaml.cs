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
        var stackPanel = this.FindControl<StackPanel>("stackPanel");
        var context = (AUGradePageViewModel)DataContext;
        var subjects = context.SubjectList;

        for (int i=0; i< subjects.Count; i++) {
            var textBlock = new TextBlock() { 
                Text = subjects[i].SubjectName,
                Margin = new Thickness(10),
            };
            var numberBox = new NumericUpDown()
            {
                Watermark = "Введите оценку",
                Margin = new Thickness(10),
                FormatString = "0",
                Value = 0,
                Minimum = 0,
                Maximum = 5,
            };
            numberBox.Bind(NumericUpDown.ValueProperty, new Avalonia.Data.Binding($"SubjectList[{i}].Grade"));
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(numberBox);
        }
    }
}