using System;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using SpeedrunCommunity.Views.Pages;
using SpeedrunCommunity.ViewModels.Pages;

namespace SpeedrunCommunity;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        
        Loaded += (s, e) => { NavView.SelectedItem = NavView.MenuItems[2]; };
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            var tag = item.Tag?.ToString();
            NavigateToPage(tag);
        }
    }

    private void NavigateToPage(string? tag)
    {
        object? page = tag switch
        {
            "Players" => CreatePage<PlayersPage, PlayerViewModel>(),
            "Games" => CreatePage<GamesPage, GameViewModel>(),
            "Results" => CreatePage<ResultsPage, ResultViewModel>(),
            _ => null
        };

        if (page != null)
        {
            ContentFrame.Navigate(page);
        }
    }

    private TPage CreatePage<TPage, TViewModel>() 
        where TPage : UserControl, new()
        where TViewModel : class
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var page = new TPage { DataContext = vm };
        return page;
    }
}
