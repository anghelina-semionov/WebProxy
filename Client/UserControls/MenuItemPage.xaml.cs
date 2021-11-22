using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for MenuItemPage.xaml
    /// </summary>
    public partial class MenuItemPage : UserControl
    {
        public MenuItemPage(MenuItemControl item)
        {
            InitializeComponent();

            if(item != null)
            {
                Name.Text = item.Name;
                Price.Text = item.Price.ToString() + " L";
                Image.Source = new BitmapImage(new Uri(item.Image));
                Category.Text = item.Category;
                Description.Text = item.Description;
            }
            else
            {
                MenuItemPageError.Text = "Не удалось получить данные блюда!";
                MenuItemPageError.Visibility = System.Windows.Visibility.Visible;
            }
        }
        // Обработка события нажатия кнопки "Изменить"
        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItemControl item = ((Button)sender).Tag as MenuItemControl; // получить данные блюда, которые нужно изменить
            Content = new EditPage(item); // открыть страницу редактирования
        }
        // Обработка события нажатия кнопки "Удалить"
        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItemControl item = ((Button)sender).Tag as MenuItemControl; // получить данные блюда, которое нужно удалить
            DeleteMenuItem(item.Id); // удалить 
        }
        // Отпрfвить http запрос на удаление объекта по его id
        private async void DeleteMenuItem(Guid id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                await client.DeleteAsync("menuitems/" + id);
                Content = new MenuPage("");
            }
            catch
            {
                MessageBox.Show("Не удалось удалить данные блюда!", "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}
