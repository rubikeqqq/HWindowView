using System;
using HalconDotNet;

namespace 模板匹配
{
    public class ModelTool
    {
        private HHomMat2D homMat2D = new HHomMat2D();

        public ModelTool()
        {
            ShapeModel = new HShapeModel();
            ModelParam = new ModelParam();
            FindModelParam = new FindModelParam();
        }

        public ModelParam ModelParam { get; set; }

        public FindModelParam FindModelParam { get; set; }

        public HShapeModel ShapeModel { get; set; }

        public HXLDCont OriXLDCont { get; set; } = new HXLDCont();

        public HXLDCont HXLDCont { get; set; } = new HXLDCont();

        public HXLD HXLDContList { get; set; } = new HXLDCont();

        public void CreateScaleModel(HImage image)
        {
            ShapeModel.CreateScaledShapeModel(
                image,
                ModelParam.NumLevels,
                ModelParam.AngleStart * Math.PI / 180,
                ModelParam.AngleExtent * Math.PI / 180,
                ModelParam.ScaleStep,
                ModelParam.ScaleMin,
                ModelParam.ScaleMax,
                ModelParam.ScaleStep,
                ModelParam.Optimization,
                ModelParam.Metric,
                ModelParam.Contrast,
                ModelParam.MinContrast
            );

            OriXLDCont = ShapeModel.GetShapeModelContours(1);

            HOperatorSet.AreaCenter(image,out HTuple _,out var row,out var col);

            homMat2D.VectorAngleToRigid(0,0,0,row,col,0);

            HXLDCont = OriXLDCont.AffineTransContourXld(homMat2D);

            ShapeModel.WriteShapeModel("shapeModel.shm");
        }

        public void FindScaleModel(HImage image)
        {

            if(!ShapeModel.IsInitialized())
            {
                ShapeModel.ReadShapeModel("shapeModel.shm");
            }

            ShapeModel.FindScaledShapeModel(
                image,
                FindModelParam.AngleStart * Math.PI / 180,
                FindModelParam.AngleExtent * Math.PI / 180,
                FindModelParam.ScaleMin,
                FindModelParam.ScaleMax,
                FindModelParam.MinScore,
                FindModelParam.NumMatches,
                FindModelParam.MaxOverlap,
                FindModelParam.SubPixel,
                FindModelParam.NumLevels,
                FindModelParam.Greediness,
                out FindModelParam.Row,
                out FindModelParam.Column,
                out FindModelParam.Angle,
                out FindModelParam.Scale,
                out FindModelParam.Score
            );

            HXLDContList.Dispose();

            if(FindModelParam.Score.Length > 0)
            {
                for(int i = 0;i < FindModelParam.Score.Length;i++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();

                    hHomMat2D.VectorAngleToRigid(0d,0,0,
                        FindModelParam.Row[i],FindModelParam.Column[i],FindModelParam.Angle[i]);

                    HHomMat2D scaleHomMat2D = hHomMat2D.HomMat2dScale(
                        FindModelParam.Scale[i].D,
                        FindModelParam.Scale[i],
                        FindModelParam.Row[i],
                        FindModelParam.Column[i]);

                    OriXLDCont = ShapeModel.GetShapeModelContours(1);
                    var xld = scaleHomMat2D.AffineTransContourXld(OriXLDCont);
                    if(HXLDContList != null && !HXLDContList.IsInitialized())
                    {
                        HXLDContList = xld.Clone();
                    }
                    else
                    {
                        HXLDContList = HXLDContList.ConcatObj(xld);
                    }
                }
            }
        }
    }
}