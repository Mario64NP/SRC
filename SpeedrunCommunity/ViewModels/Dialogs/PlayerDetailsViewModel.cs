using System;
using System.Windows.Input;
using SpeedrunCommunity.Core;
using SpeedrunCommunity.Core.Services;

namespace SpeedrunCommunity.ViewModels.Dialogs;

public class PlayerDetailsViewModel : ViewModelBase
{
    public string Nick { get; set { field = value; OnPropertyChanged(); } } = string.Empty;
    public int? Age { get; set { field = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event EventHandler<bool>? RequestClose;

    public PlayerDetailsViewModel()
    {
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save(object? obj)
    {
        if (string.IsNullOrWhiteSpace(Nick) || Age is null or <= 0)
        {
            DialogService.ShowError("The player details are invalid.\nNick must not be empty and Age must be > 0.", "Invalid Details");
            return;
        }

        RequestClose?.Invoke(this, true);
    }

    private void Cancel(object? obj) => RequestClose?.Invoke(this, false);
}

