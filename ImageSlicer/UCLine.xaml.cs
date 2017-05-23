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

namespace ImageSlicer
{
    /// <summary>
    /// UCLine.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCLine : UserControl
    {
        public int Y = 0;
        public UCLine()
        {
            InitializeComponent();
        }

        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            int offSet = (int)(Canvas.GetTop(this) + e.VerticalChange);
            Canvas.SetTop(this, offSet);

            setY();
        }

        public void setY()
        {
            int offSet = (int)Canvas.GetTop(this);
            Y = offSet + (int)(this.Height / 2);
        }
    }
}
