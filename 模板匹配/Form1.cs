using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 模板匹配
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private HImage image = new HImage();

        private ModelTool modelTool = new ModelTool();

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图像文件(*.jpg,*.bmp,*.png,*.jpeg)|*.jpg;*.bmp;*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image.ReadImage(openFileDialog.FileName);

                uC_Window1.HobjectToHimage(image);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建模板
            var region = Blob(image);

            modelTool.CreateScaleModel((HImage)region);

            uC_Window1.DispObj(modelTool.HXLDCont);
        }

        private HObject Blob(HImage image)
        {
            //通过Blob
            var blob = image.Threshold(0d, 128);

            var blobs = blob.Connection();

            var region = blobs.SelectShape("area", "and", 10000, 20000);

            region = region.FillUp();

            region = region.DilationCircle(3.5);

            return image.ReduceDomain(region);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modelTool.FindScaleModel(image);

            uC_Window1.DispObj(modelTool.HXLDContList, "yellow");
        }

        HRegion region;

        private void button4_Click(object sender, EventArgs e)
        {
            //测试halcon对象 的 状态 什么时候为null 什么时候是 isinitialized

            //如果只是声明了对象 此时是Null
            var b = region is null;

            //如果初始化对象 此时对象不为null 但是是非isinitialized状态
            region = new HRegion();

            var b1 = region.IsInitialized();
            var b2 = !(region is null);

            //如果genEmptyRegion之后 此时是isinitialized状态
            region.GenEmptyRegion();

            var b3 = region.IsInitialized();

            //如果dispose之后 此时isinitialized状态为false
            region.Dispose();

            var b4 = region.IsInitialized();
        }
    }
}
