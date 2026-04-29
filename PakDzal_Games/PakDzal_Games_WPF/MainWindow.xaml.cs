using System.Windows;
using System.Windows.Controls;
using PakDzal_Games_WPF.Models;
using PakDzal_Games_WPF.Services;

namespace PakDzal_Games_WPF;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            var users = await _apiService.GetUsersAsync();
            dgUsers.ItemsSource = users;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }

        try
        {
            var games = await _apiService.GetGamesAsync();
            dgGames.ItemsSource = games;
            cmbGameGenre.ItemsSource = games.Select(g => g.Genre).Distinct().ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }

        try
        {
            var sessions = await _apiService.GetSessionsAsync();
            dgSessions.ItemsSource = sessions;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }

        try
        {
            var allUsers = await _apiService.GetUsersAsync();
            var allGames = await _apiService.GetGamesAsync();
            cmbSessionUser.ItemsSource = allUsers;
            cmbSessionUser.DisplayMemberPath = "Name";
            cmbSessionUser.SelectedValuePath = "UserId";
            cmbSessionGame.ItemsSource = allGames;
            cmbSessionGame.DisplayMemberPath = "Title";
            cmbSessionGame.SelectedValuePath = "GameId";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnUserSearch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var users = await _apiService.GetUsersAsync(txtUserSearch.Text, txtUserCity.Text);
            dgUsers.ItemsSource = users;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnUserReset_Click(object sender, RoutedEventArgs e)
    {
        txtUserSearch.Text = "";
        txtUserCity.Text = "";
        try
        {
            var users = await _apiService.GetUsersAsync();
            dgUsers.ItemsSource = users;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
    {
        if (dgUsers.SelectedItem is User user)
        {
            var result = MessageBox.Show($"Удалить {user.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteUserAsync(user.UserId);
                    var users = await _apiService.GetUsersAsync();
                    dgUsers.ItemsSource = users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }
    }

    private void btnAddUser_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new UserDialog();
        dialog.Owner = this;
        if (dialog.ShowDialog() == true)
        {
            var users = _apiService.GetUsersAsync();
            dgUsers.ItemsSource = users.Result;
        }
    }

    private void btnEditUser_Click(object sender, RoutedEventArgs e)
    {
        if (dgUsers.SelectedItem is User user)
        {
            var dialog = new UserDialog(user);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                var users = _apiService.GetUsersAsync();
                dgUsers.ItemsSource = users.Result;
            }
        }
    }

    private async void btnUserReport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var report = await _apiService.GetUsersReportAsync(txtUserCity.Text);
            var dialog = new ReportWindow("ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ", report.ToString());
            dialog.Owner = this;
            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnGameSearch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            bool? available = chkGameAvailable.IsChecked == true ? true : null;
            var games = await _apiService.GetGamesAsync(txtGameSearch.Text, cmbGameGenre.SelectedItem?.ToString(), available);
            dgGames.ItemsSource = games;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnGameReset_Click(object sender, RoutedEventArgs e)
    {
        txtGameSearch.Text = "";
        cmbGameGenre.SelectedItem = null;
        chkGameAvailable.IsChecked = false;
        var games = await _apiService.GetGamesAsync();
        dgGames.ItemsSource = games;
    }

    private async void btnDeleteGame_Click(object sender, RoutedEventArgs e)
    {
        if (dgGames.SelectedItem is Game game)
        {
            var result = MessageBox.Show($"Удалить {game.Title}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteGameAsync(game.GameId);
                    var games = await _apiService.GetGamesAsync();
                    dgGames.ItemsSource = games;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }
    }

    private void btnAddGame_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new GameDialog();
        dialog.Owner = this;
        if (dialog.ShowDialog() == true)
        {
            var games = _apiService.GetGamesAsync();
            dgGames.ItemsSource = games.Result;
        }
    }

    private void btnEditGame_Click(object sender, RoutedEventArgs e)
    {
        if (dgGames.SelectedItem is Game game)
        {
            var dialog = new GameDialog(game);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                var games = _apiService.GetGamesAsync();
                dgGames.ItemsSource = games.Result;
            }
        }
    }

    private async void btnGameReport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var report = await _apiService.GetGamesReportAsync();
            var dialog = new ReportWindow("ОТЧЕТ ПО ИГРАМ", report.ToString());
            dialog.Owner = this;
            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }

    private async void btnSessionFilter_Click(object sender, RoutedEventArgs e)
    {
        int? userId = cmbSessionUser.SelectedValue as int?;
        int? gameId = cmbSessionGame.SelectedValue as int?;
        var sessions = await _apiService.GetSessionsAsync(userId, gameId);
        dgSessions.ItemsSource = sessions;
    }

    private async void btnSessionReset_Click(object sender, RoutedEventArgs e)
    {
        cmbSessionUser.SelectedItem = null;
        cmbSessionGame.SelectedItem = null;
        var sessions = await _apiService.GetSessionsAsync();
        dgSessions.ItemsSource = sessions;
    }

    private async void btnDeleteSession_Click(object sender, RoutedEventArgs e)
    {
        if (dgSessions.SelectedItem is Session session)
        {
            var result = MessageBox.Show($"Удалить сессию #{session.SessionId}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteSessionAsync(session.SessionId);
                    var sessions = await _apiService.GetSessionsAsync();
                    dgSessions.ItemsSource = sessions;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }
    }

    private void btnAddSession_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SessionDialog();
        dialog.Owner = this;
        if (dialog.ShowDialog() == true)
        {
            var sessions = _apiService.GetSessionsAsync();
            dgSessions.ItemsSource = sessions.Result;
        }
    }

    private void btnEditSession_Click(object sender, RoutedEventArgs e)
    {
        if (dgSessions.SelectedItem is Session session)
        {
            var dialog = new SessionDialog(session);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                var sessions = _apiService.GetSessionsAsync();
                dgSessions.ItemsSource = sessions.Result;
            }
        }
    }

    private async void btnSessionReport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var report = await _apiService.GetSessionsReportAsync();
            var dialog = new ReportWindow("ОТЧЕТ ПО СЕССИЯМ", report.ToString());
            dialog.Owner = this;
            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
        }
    }
}