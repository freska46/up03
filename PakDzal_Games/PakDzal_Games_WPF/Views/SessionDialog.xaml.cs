using System.Windows;
using PakDzal_Games_WPF.Models;
using PakDzal_Games_WPF.Services;

namespace PakDzal_Games_WPF;

public partial class SessionDialog : Window
{
    private readonly ApiService _apiService = new();
    private readonly Session? _session;

    public SessionDialog(Session? session = null)
    {
        InitializeComponent();
        _session = session;
        
        LoadDataAsync();
        
        if (session != null)
        {
            cmbUser.SelectedValue = session.UserId;
            cmbGame.SelectedValue = session.GameId;
            dtpStart.SelectedDate = session.StartTime;
            dtpEnd.SelectedDate = session.EndTime;
        }
        else
        {
            dtpStart.SelectedDate = DateTime.Now;
            dtpEnd.SelectedDate = DateTime.Now.AddHours(2);
        }
    }

    private async void LoadDataAsync()
    {
        var users = await _apiService.GetUsersAsync();
        var games = await _apiService.GetGamesAsync();

        cmbUser.ItemsSource = users;
        cmbUser.DisplayMemberPath = "Name";
        cmbUser.SelectedValuePath = "UserId";

        cmbGame.ItemsSource = games;
        cmbGame.DisplayMemberPath = "Title";
        cmbGame.SelectedValuePath = "GameId";
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (cmbUser.SelectedValue == null || cmbGame.SelectedValue == null)
        {
            MessageBox.Show("Выберите пользователя и игру", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!dtpStart.SelectedDate.HasValue || !dtpEnd.SelectedDate.HasValue)
        {
            MessageBox.Show("Выберите время начала и окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var session = _session ?? new Session();
            session.UserId = (int)cmbUser.SelectedValue;
            session.GameId = (int)cmbGame.SelectedValue;
            session.StartTime = dtpStart.SelectedDate.Value;
            session.EndTime = dtpEnd.SelectedDate.Value;

            if (_session == null)
            {
                await _apiService.CreateSessionAsync(session);
            }
            else
            {
                await _apiService.UpdateSessionAsync(session);
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