using Nancy.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : UserControl
    {
        string category = null;
        bool flag = false;
        public EditPage(MenuItemControl menuItemControl)
        {
            InitializeComponent();
            Error.Visibility = Visibility.Hidden;
            if(menuItemControl != null) // данные блюда были отправлены со страницы MenuItemPage
            {
                Name.Text = menuItemControl.Name;
                Price.Text = menuItemControl.Price.ToString();
                Desc.Text = menuItemControl.Description;
                Category.SelectedValue = menuItemControl.Category;
                Load.Text = menuItemControl.Image;

                flag = true;
            }
        }
        // Обработка действия нажатия кнопки "Применить"
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
            MenuItemControl item = new MenuItemControl();
            // Поля "Название", "Цена", "Описание" и "Категория" должны быть обязательно заполнены
            if (Name.Text.Length > 0 && Price.Text.Length > 0 && Desc.Text.Length > 0 && category != null)
            {
                try
                {
                    var price = Convert.ToDouble(Price.Text); // перевести в формат double
                    if (Load.Text == "Ссылка на изображение" || Load.Text.Length < 0) // если не была выбрана картинка, задать стандартное изображение
                    {
                        Load.Text = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6b/Picture_icon_BLACK.svg/1200px-Picture_icon_BLACK.svg.png";
                    }
                    if (Load.Text.Length < 7 || Load.Text.Substring(0, 4) != "http") // если ссылка изображения была указана неправильно,
                    {
                        Error.Content = "Введите ссылку на изображение (https://...)!"; // вывести сообщение об ошибке
                        Error.Visibility = Visibility.Visible;
                    }
                    else //иначе,
                    {
                        item.Name = Name.Text;
                        item.Price = price;
                        item.Description = Desc.Text;
                        item.Category = category;
                        item.Image = Load.Text;

                        if (flag == false) // создается новый объект
                        {
                            SaveMenuItem(item);
                        }
                        else // обновляется существующий
                        {
                            UpdateMenuItem(item);
                        }
                    }
                }
                catch
                {
                    Error.Content = "Используйте <,> при написании цены!";
                    Error.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Error.Content = "Заполните поля, помеченные *!";
                Error.Visibility = Visibility.Visible;
            }
        }
        // Обработка события выбора элемента из выпадающего списка
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ListBoxItem selectedItem = (ListBoxItem)comboBox.SelectedValue;
            category = selectedItem.Content.ToString();
        }
        // Создать новое блюдо меню
        private async void SaveMenuItem(MenuItemControl item)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Common.Models.MenuItem menuItem = new Common.Models.MenuItem();
                menuItem = ConvertToMenuItem(item);
                await client.PostAsync("menuitems", new StringContent(new JavaScriptSerializer().Serialize(menuItem), Encoding.UTF8, "application/json"));
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить блюдо!", "Ошибка", MessageBoxButton.OK);
            }
        }
        // Изменить данные блюда
        private async void UpdateMenuItem(MenuItemControl item)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Common.Models.MenuItem menuItem = new Common.Models.MenuItem();
                menuItem = ConvertToMenuItem(item);
                await client.PutAsync("menuitems", new StringContent(new JavaScriptSerializer().Serialize(menuItem), Encoding.UTF8, "application/json"));
            }
            catch
            {
                MessageBox.Show("Не удалось обновить данные блюда!", "Ошибка", MessageBoxButton.OK);
            }
        }
        // Вернуть объект класса MenuItem
        private Common.Models.MenuItem ConvertToMenuItem(MenuItemControl menuItemControl)
        {
            Common.Models.MenuItem menuItem = new Common.Models.MenuItem();
            menuItem.Id = menuItemControl.Id;
            menuItem.LastChangedAt = menuItemControl.LastChangedAt;
            menuItem.Name = menuItemControl.Name;
            menuItem.Price = menuItemControl.Price;
            menuItem.Image = menuItemControl.Image;
            menuItem.Category = menuItemControl.Category;
            menuItem.Description = menuItemControl.Description;

            return menuItem;
        }
    }
}
