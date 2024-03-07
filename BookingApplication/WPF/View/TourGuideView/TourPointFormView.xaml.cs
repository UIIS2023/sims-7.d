using BookingApplication.Domain.Model;
using BookingApplication.WPF.ViewModel.TourGuideViewModel;
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
using Image = BookingApplication.Domain.Model.Image;

namespace BookingApplication.WPF.View.TourGuideView
{
    /// <summary>
    /// Interaction logic for TourPointFormView.xaml
    /// </summary>
    public partial class TourPointFormView : Window
    {
        public TourPointFormView(Tour tour, TourPoint tourPoint, Image image)
        {
            InitializeComponent();
            TourPointFormVM tourPointFormVM = new TourPointFormVM(tour, tourPoint, image);
            DataContext = tourPointFormVM;
            if (tourPointFormVM.CloseAction != null)
            {
                tourPointFormVM.CloseAction = new Action(() => this.Close());
            }
        }
    }
}
