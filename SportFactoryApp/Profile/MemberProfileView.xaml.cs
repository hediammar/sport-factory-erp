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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportFactoryApp.Profile
{
    /// <summary>
    /// Logique d'interaction pour MemberProfileView.xaml
    /// </summary>
    public partial class MemberProfileView : UserControl
    {
        public MemberProfileView(Member member)
        {
            InitializeComponent();
            DataContext = member; // Set the DataContext to the provided member
        }
    }
}

