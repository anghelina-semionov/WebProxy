using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using Common.Models;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : UserControl
    {
        HttpClient client = new HttpClient();
        List<MenuItemControl> list = new List<MenuItemControl>();
        MenuItemControl menuItemControl = new MenuItemControl();
        public MenuPage(string category)
        {
            InitializeComponent();

            if(category == "")
            {
                GetMenuItems();
            }
            else
            {
                GetMenuItemsByCategory(category);
            }
            if (list.Count > 0) ListViewProducts.ItemsSource = list;
            else
            {
                Error.Text = "Не удалось получить список блюд!";
                Error.Visibility = Visibility.Visible;
            }
        }
        private async void GetMenuItems()
        {
            //var l = new Menu();
            //l.Name = "Мистер Нико";
            //l.Price = 30.00;
            //l.Image = "https://www.pngkey.com/png/full/97-970955_wafer-ice-cream-transparent-background-png-waffle-cup.png";
            //l.Category = "Десерт";
            //l.Description = "Мороженое Mister Нико в вафельной корзинке с шоколадным соусом и кусочками брауни.";

            //list.Add(l);

            client.BaseAddress = new Uri("http://localhost:9000/api/menu/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetStringAsync("menuitem");
                if (response != null)
                {
                    var items = JsonConvert.DeserializeObject<List<Common.Models.MenuItem>>(response);
                    if (items.Count > 0)
                    {
                        foreach (var i in items)
                        {
                            list.Add(ConvertToMenuItemControl(i));
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось установить подключение!","Ошибка", MessageBoxButton.OK);
            }
        }
        private async void GetMenuItemsByCategory(string category)
        {
            client.BaseAddress = new Uri("http://localhost:9000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetStringAsync($"menuitems/{category}");
                if (response != null)
                {
                    var items = JsonConvert.DeserializeObject<List<Common.Models.MenuItem>>(response);
                    if (items.Count > 0)
                    {
                        foreach (var i in items)
                        {
                            list.Add(ConvertToMenuItemControl(i));
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось установить подключение!", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuItemControl item = ((Button)sender).Tag as MenuItemControl;
            Content = new MenuItemPage(item);
        }
        // Вернуть объект класса MenuItemControl
        private MenuItemControl ConvertToMenuItemControl(Common.Models.MenuItem menuItem)
        {
            menuItemControl.Id = menuItem.Id;
            menuItemControl.LastChangedAt = menuItem.LastChangedAt;
            menuItemControl.Name = menuItem.Name;
            menuItemControl.Price = menuItem.Price;
            menuItemControl.Image = menuItem.Image;
            menuItemControl.Category = menuItem.Category;
            menuItemControl.Description = menuItem.Description;

            return menuItemControl;
        }
    }
}
