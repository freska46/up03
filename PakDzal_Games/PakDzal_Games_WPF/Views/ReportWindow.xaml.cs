using System.Windows;

namespace PakDzal_Games_WPF;

public partial class ReportWindow : Window
{
    public ReportWindow(string title, string reportContent)
    {
        InitializeComponent();
        txtTitle.Text = title;
        txtReport.Text = reportContent;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}