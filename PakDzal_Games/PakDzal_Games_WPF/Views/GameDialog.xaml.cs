using System.Windows;
using PakDzal_Games_WPF.Models;
using PakDzal_Games_WPF.Services;

namespace PakDzal_Games_WPF;

public partial class GameDialog : Window
{
    private readonly ApiService _apiService = new();
    private readonly Game? _game;

    public GameDialog(Game? game = null)
    {
        InitializeComponent();
        _game = game;
        
        if (game != null)
        {
            txtTitle.Text = game.Title;
            txtGenre.Text = game.Genre;
            txtPrice.Text = game.PricePerHour.ToString();
            chkAvailable.IsChecked = game.Available;
        }
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtGenre.Text))
        {
            MessageBox.Show("Заполните обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(txtPrice.Text, out decimal price))
        {
            MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var game = _game ?? new Game();
            game.Title = txtTitle.Text;
            game.Genre = txtGenre.Text;
            game.PricePerHour = price;
            game.Available = chkAvailable.IsChecked == true;

            if (_game == null)
            {
                await _apiService.CreateGameAsync(game);
            }
            else
            {
                await _apiService.UpdateGameAsync(game);
            }

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}