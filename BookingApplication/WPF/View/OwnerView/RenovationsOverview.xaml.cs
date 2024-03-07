using BookingApplication.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApplication.WPF.ViewModel.OwnerViewModel;
using NavigationService = BookingApplication.Services.NavigationService;
using System.Windows.Controls;
using BookingApplication.WPF.ViewModel;


namespace BookingApplication.WPF.View.OwnerView
{
    /// <summary>
    /// Interaction logic for RenovationsOverview.xaml
    /// </summary>
    public partial class RenovationsOverview
    {
        public RenovationsOverview()
        {
            InitializeComponent();
            var renovationsOverviewVM = new RenovationsOverviewVM();
            DataContext = renovationsOverviewVM;
        }
    }
}
