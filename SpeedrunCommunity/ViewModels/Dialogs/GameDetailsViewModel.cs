using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;
using SpeedrunCommunity.Models;

namespace SpeedrunCommunity.ViewModels.Dialogs;

public class SelectableCategory : ViewModelBase
{
    public Category Category { get; }
    public bool IsSelected { get; set { field = value; OnPropertyChanged(); } }

    public SelectableCategory(Category category, bool isSelected = false)
    {
        Category = category;
        IsSelected = isSelected;
    }
}

public class GameDetailsViewModel : ViewModelBase
{
    public string Name { get; set { field = value; OnPropertyChanged(); } } = string.Empty;
    public string Developer { get; set { field = value; OnPropertyChanged(); } } = string.Empty;
    public int? ReleaseYear { get; set { field = value; OnPropertyChanged(); } }
    public Platform? SelectedPlatform { get; set { field = value; OnPropertyChanged(); } }

    public ObservableCollection<Platform> Platforms { get; }
    public ObservableCollection<SelectableCategory> Categories { get; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event EventHandler<bool>? RequestClose;

    public GameDetailsViewModel(IEnumerable<Platform> platforms, IEnumerable<Category> allCategories, IEnumerable<Category> selectedCategories)
    {
        Platforms = new ObservableCollection<Platform>(platforms);
        Categories = [];

        var selectedIds = new HashSet<int>(selectedCategories.Select(c => c.ID));
        foreach (var cat in allCategories)
            Categories.Add(new SelectableCategory(cat, selectedIds.Contains(cat.ID)));

        if (Platforms.Count > 0) 
            SelectedPlatform = Platforms[0];

        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    public IList<Category> GetSelectedCategories() => Categories.Where(c => c.IsSelected).Select(c => c.Category).ToList();

    private void Save(object? obj)
    {
        var selectedCats = GetSelectedCategories();
        
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Developer) || 
            ReleaseYear is null or <= 0 || SelectedPlatform == null || selectedCats.Count == 0)
        {
            DialogService.ShowError("Please ensure all fields are valid.\n- Name & Developer required\n- Valid Year\n- Platform selected\n- At least one Category selected", "Invalid Game Details");
            return;
        }

        RequestClose?.Invoke(this, true);
    }

    private void Cancel(object? obj) => RequestClose?.Invoke(this, false);
}
