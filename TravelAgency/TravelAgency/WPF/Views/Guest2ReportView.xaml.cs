using System.Windows.Controls;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest2ReportView.xaml
    /// </summary>
    public partial class Guest2ReportView : Page
    {
        public Guest2ReportView()
        {
            InitializeComponent();
            pdfViewer.Load(@"../../../ReportsPDF/Guest2Report.pdf");
        }
    }
}
