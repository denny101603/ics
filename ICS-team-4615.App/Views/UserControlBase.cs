using System.Windows;
using System.Windows.Controls;
using ICS_team_4615.App.ViewModels;

namespace ICS_team_4615.App.Views
{
    public class UserControlBase : UserControl
    {
        protected UserControlBase()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewModel viewModel)
            {
                viewModel.Load();
            }
        }
    }

    
}