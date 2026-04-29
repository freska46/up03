using System.Windows;
using PakDzal_Games_WPF.Models;
using PakDzal_Games_WPF.Services;

namespace PakDzal_Games_WPF;

public partial class UserDialog : Window
{
    private readonly ApiService _apiService = new();
    private readonly User? _user;

    public UserDialog(User? user = null)
    {
        InitializeComponent();
        _user = user;
        
        if (user != null)
        {
            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            txtCity.Text = user.City;
        }
        else
        {
            txtCity.Text = "Курск";
        }
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            MessageBox.Show("Заполните обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var user = _user ?? new User();
            user.Name = txtName.Text;
            user.Email = txtEmail.Text;
            user.Phone = txtPhone.Text;
            user.City = txtCity.Text;

            if (_user == null)
            {
                await _apiService.CreateUserAsync(user);
            }
            else
            {
                await _apiService.UpdateUserAsync(user);
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