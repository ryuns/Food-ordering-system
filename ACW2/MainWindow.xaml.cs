using FoodOrderingSystemClasses;
using System;
using System.Windows;

namespace ACW2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FoodManager m_FoodManager;
        private OrderManager m_OrderManager;

        public MainWindow()
        {
            InitializeComponent();

            m_OrderManager = new OrderManager();
            m_FoodManager = new FoodManager();
            try
            {
                //Reads in the files.
                m_FoodManager.ReadInventory();
                m_FoodManager.ReadMenu();
            }
            catch (Exception exc)
            {
                MessageBox.Show("File Read Failed: " + exc.Message.ToString());
                Close();
            }
        }

        private void inventoryButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow wnd = new InventoryWindow(m_FoodManager);
            wnd.ShowDialog();
        }

        private void foodMenuButton_Click(object sender, RoutedEventArgs e)
        {
            FoodMenuWindow wnd = new FoodMenuWindow(m_FoodManager);
            wnd.ShowDialog();
        }

        private void newOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow wnd = new OrderWindow(m_FoodManager, m_OrderManager);
            wnd.ShowDialog();
        }

        private void completedOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            CompletedOrdersWindow wnd = new CompletedOrdersWindow(m_OrderManager);
            wnd.ShowDialog();
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            m_FoodManager.UpdateInventory();
            Close();
        }
    }
}