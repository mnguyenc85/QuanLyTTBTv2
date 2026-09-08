using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using QuanLyTTBTv2.Services;

namespace QuanLyTTBTv2.Views
{
    public partial class MainWindow : Window
    {
        private readonly LocalDbBridge _ldb = LocalDbBridge.Instance;
        
        #region Initialization & Startup & Closed 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnLoaded(object? sender, RoutedEventArgs e)
        {
            
        }

        private void Window_OnClosed(object? sender, EventArgs e)
        {
        }
        #endregion

        /// <summary>
        /// Khởi tạo db (local & server) -> khởi tạo khác 
        /// </summary>
        private void Init()
        {
            _ldb.SyscSchema();
        }
    }
}