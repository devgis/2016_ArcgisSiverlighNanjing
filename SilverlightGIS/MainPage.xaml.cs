using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Tasks;
using SilverlightGIS.Information;

namespace SilverlightGIS
{
    public partial class MainPage : UserControl
    {
        public static string UserName = string.Empty;
        List<ThematicItem> ThematicItemList = new List<ThematicItem>();
        List<List<SolidColorBrush>> ColorList = new List<List<SolidColorBrush>>();
        int _colorShadeIndex = 0;
        int _thematicListIndex = 0;
        FeatureSet _featureSet = null;
        int _classType = 0; // EqualInterval = 1; Quantile = 0;
        int _classCount = 6;
        int _lastGeneratedClassCount = 0;
        bool _legendGridCollapsed;
        bool _classGridCollapsed;
        public MainPage()
        {
            InitializeComponent();
            GraphicsLayer layer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;
            //layer.SpatialReference = MainMap.SpatialReference;
            //MyNavgation.Map = MainMap;
            //MyScaleLine.Map = MainMap;
            //MyOverviewMap.Map = MainMap;
            this.Loaded += MainPage_Loaded;
            

        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        Random rd = new Random();
        private void SetRangeValues()
        {
            
            GraphicsLayer graphicsLayer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;

            // if necessary, update ColorList based on current number of classes.
            if (_lastGeneratedClassCount != _classCount) CreateColorList();

            // Field on which to generate a classification scheme.  
            ThematicItem thematicItem = new ThematicItem() { Name = "县区景点数量", Description = "县区景点数量 ", CalcField = "" }; //ThematicItemList[_thematicListIndex];

            // Calculate value for classification scheme
            bool useCalculatedValue = !string.IsNullOrEmpty(thematicItem.CalcField);

            // Store a list of values to classify
            List<double> valueList = new List<double>();

            // Get range, min, max, etc. from features
            for (int i = 0; i < _featureSet.Features.Count; i++)
            {
                Graphic graphicFeature = _featureSet.Features[i];

                double graphicValue = Convert.ToDouble(graphicFeature.Attributes[thematicItem.Name]);
                string graphicName = graphicFeature.Attributes["NAME"].ToString();

                //if (useCalculatedValue)
                //{
                //    double calcVal = Convert.ToDouble(graphicFeature.Attributes[thematicItem.CalcField]);
                //    graphicValue = Math.Round(graphicValue / calcVal * 100, 2);
                //}
                double calcVal = Convert.ToDouble(graphicFeature.Attributes[thematicItem.CalcField]);
                graphicValue = Math.Round(graphicValue / calcVal * 100, 2);

                if (i == 0)
                {
                    thematicItem.Min = graphicValue;
                    thematicItem.Max = graphicValue;
                    thematicItem.MinName = graphicName;
                    thematicItem.MaxName = graphicName;
                }
                else
                {
                    if (graphicValue < thematicItem.Min) { thematicItem.Min = graphicValue; thematicItem.MinName = graphicName; }
                    if (graphicValue > thematicItem.Max) { thematicItem.Max = graphicValue; thematicItem.MaxName = graphicName; }
                }

                valueList.Add(graphicValue);
            }

            // Set up range start values
            thematicItem.RangeStarts = new List<double>();

            double totalRange = thematicItem.Max - thematicItem.Min;
            double portion = totalRange / _classCount;

            thematicItem.RangeStarts.Add(thematicItem.Min);
            double startRangeValue = thematicItem.Min;

            //for (int i = 1; i < _classCount; i++)
            //{
            //    startRangeValue += portion;
            //    thematicItem.RangeStarts.Add(startRangeValue);
            //}
            thematicItem.RangeStarts.Add(50);
            thematicItem.RangeStarts.Add(100);
            thematicItem.RangeStarts.Add(200);
            thematicItem.RangeStarts.Add(400);
            thematicItem.RangeStarts.Add(600);
            thematicItem.RangeStarts.Add(800);

            //// Equal Interval
            //if (_classType == 1)
            //{
            //    for (int i = 1; i < _classCount; i++)
            //    {
            //        startRangeValue += portion;
            //        thematicItem.RangeStarts.Add(startRangeValue);
            //    }
            //}
            //// Quantile
            //else
            //{
            //    // Enumerator of all values in ascending order
            //    IEnumerable<double> valueEnumerator =
            //    from aValue in valueList
            //    orderby aValue //"ascending" is default
            //    select aValue;

            //    int increment = Convert.ToInt32(Math.Ceiling(_featureSet.Features.Count / _classCount));
            //    for (int i = increment; i < valueList.Count; i += increment)
            //    {
            //        double value = valueEnumerator.ElementAt(i);
            //        thematicItem.RangeStarts.Add(value);
            //    }
            //}

            // Create graphic features and set symbol using the class range which contains the value 
            List<SolidColorBrush> brushList = ColorList[_colorShadeIndex];
            if (_featureSet != null && _featureSet.Features.Count > 0)
            {
                // Clear previous graphic features
                graphicsLayer.Graphics.Clear();

                for (int i = 0; i < _featureSet.Features.Count; i++)
                {
                    Graphic graphicFeature = _featureSet.Features[i];

                    //double graphicValue = Convert.ToDouble(graphicFeature.Attributes[thematicItem.Name]);
                    //if (useCalculatedValue)
                    //{
                    //    double calcVal = Convert.ToDouble(graphicFeature.Attributes[thematicItem.CalcField]);
                    //    graphicValue = Math.Round(graphicValue / calcVal * 100, 2);
                    //}
                    double graphicValue = rd.NextDouble() * 1000;
                    int brushIndex = GetRangeIndex(graphicValue, thematicItem.RangeStarts);

                    SimpleFillSymbol symbol = new SimpleFillSymbol()
                    {
                        Fill = brushList[brushIndex],
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        BorderThickness = 1
                    };

                    Graphic graphic = new Graphic();
                    graphic.Geometry = graphicFeature.Geometry;
                    graphic.Attributes.Add("Name", graphicFeature.Attributes["NAME"].ToString());
                    graphic.Attributes.Add("Description", thematicItem.Description);
                    graphic.Attributes.Add("Value", graphicValue);
                    graphic.Symbol = symbol;

                    graphicsLayer.Graphics.Add(graphic);
                }


                // Create new legend with ranges and swatches.
                LegendStackPanel.Children.Clear();

                ListBox legendList = new ListBox();
                LegendTitle.Text = thematicItem.Description;

                for (int c = 0; c < _classCount; c++)
                {
                    Rectangle swatchRect = new Rectangle()
                    {
                        Width = 20,
                        Height = 20,
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = brushList[c]
                    };

                    TextBlock classTextBlock = new TextBlock();

                    // First classification
                    if (c == 0)
                        classTextBlock.Text = String.Format("  Less than {0}", Math.Round(thematicItem.RangeStarts[1], 2));
                    // Last classification
                    else if (c == _classCount - 1)
                        classTextBlock.Text = String.Format("  {0} and above", Math.Round(thematicItem.RangeStarts[c], 2));
                    // Middle classifications
                    else
                        classTextBlock.Text = String.Format("  {0} to {1}", Math.Round(thematicItem.RangeStarts[c], 2), Math.Round(thematicItem.RangeStarts[c + 1], 2));

                    StackPanel classStackPanel = new StackPanel();
                    classStackPanel.Orientation = Orientation.Horizontal;
                    classStackPanel.Children.Add(swatchRect);
                    classStackPanel.Children.Add(classTextBlock);

                    legendList.Items.Add(classStackPanel);
                }

                //TextBlock minTextBlock = new TextBlock();
                //StackPanel minStackPanel = new StackPanel();
                //minStackPanel.Orientation = Orientation.Horizontal;
                //minTextBlock.Text = String.Format("Min: {0} ({1})", thematicItem.Min, thematicItem.MinName);
                //minStackPanel.Children.Add(minTextBlock);
                //legendList.Items.Add(minStackPanel);

                //TextBlock maxTextBlock = new TextBlock();
                //StackPanel maxStackPanel = new StackPanel();
                //maxStackPanel.Orientation = Orientation.Horizontal;
                //maxTextBlock.Text = String.Format("Max: {0} ({1})", thematicItem.Max, thematicItem.MaxName);
                //maxStackPanel.Children.Add(maxTextBlock);
                //legendList.Items.Add(maxStackPanel);

                LegendStackPanel.Children.Add(legendList);
            }
        }

        private void CreateColorList()
        {
            ColorList = new List<List<SolidColorBrush>>();

            List<SolidColorBrush> BlueShades = new List<SolidColorBrush>();
            List<SolidColorBrush> RedShades = new List<SolidColorBrush>();
            List<SolidColorBrush> GreenShades = new List<SolidColorBrush>();
            List<SolidColorBrush> YellowShades = new List<SolidColorBrush>();
            List<SolidColorBrush> MagentaShades = new List<SolidColorBrush>();
            List<SolidColorBrush> CyanShades = new List<SolidColorBrush>();

            int rgbFactor = 255 / _classCount;

            for (int j = 0; j < 256; j = j + rgbFactor)
            {
                BlueShades.Add(new SolidColorBrush(Color.FromArgb(192, (byte)j, (byte)j, 255)));
                RedShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, (byte)j, (byte)j)));
                GreenShades.Add(new SolidColorBrush(Color.FromArgb(192, (byte)j, 255, (byte)j)));
                YellowShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, 255, (byte)j)));
                MagentaShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, (byte)j, 255)));
                CyanShades.Add(new SolidColorBrush(Color.FromArgb(192, (byte)j, 255, 255)));
            }

            ColorList.Add(BlueShades);
            ColorList.Add(RedShades);
            ColorList.Add(GreenShades);
            ColorList.Add(YellowShades);
            ColorList.Add(MagentaShades);
            ColorList.Add(CyanShades);

            foreach (List<SolidColorBrush> brushList in ColorList)
            {
                brushList.Reverse();
            }

            List<SolidColorBrush> MixedShades = new List<SolidColorBrush>();
            if (_classCount > 5) MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 0, 255, 255)));
            if (_classCount > 4) MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, 0, 255)));
            if (_classCount > 3) MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, 255, 0)));
            MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 0, 255, 0)));
            MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 0, 0, 255)));
            MixedShades.Add(new SolidColorBrush(Color.FromArgb(192, 255, 0, 0)));
            ColorList.Add(MixedShades);

            _lastGeneratedClassCount = _classCount;
        }

        private int GetRangeIndex(double val, List<double> ranges)
        {
            int index = _classCount - 1;
            for (int r = 0; r < _classCount - 1; r++)
            {
                if (val >= ranges[r] && val < ranges[r + 1]) index = r;
            }
            return index;
        }


        private void btSearchMap_Click(object sender, RoutedEventArgs e)
        {

            //新建一个Find task
            FindTask findTask = new FindTask("http://liyafei-server:6080/arcgis/rest/services/Nanjing/MapServer");

            //异步执行，绑定事件
            findTask.ExecuteCompleted += FindTask_ExecuteCompleted;
            findTask.Failed += FindTask_Failed; ;

            //初始化FindParameters参数
            FindParameters findParameters = new FindParameters();
            findParameters.LayerIds.AddRange(new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 }); //查找的图层
            findParameters.SearchFields.AddRange(new string[] { "NAME" }); //查找的字段范围
            findParameters.ReturnGeometry = true;
            findParameters.SearchText = tbSearchWords.Text; //查找的“属性值”

            //设置查询的LayerDefinitions
            ESRI.ArcGIS.Client.LayerDefinition myDefinition = new ESRI.ArcGIS.Client.LayerDefinition();
            //myDefinition.LayerID = 7;
            //设置LayerDefinition，属性字段“Name”属于ID为0的图层
            //LayerDefinition的设置语句和Query中的Where语句一样
            myDefinition.Definition = "NAME = 'XXX'";

            //创建一个ObservableCollection，add设置的LayerDefinition
            System.Collections.ObjectModel.ObservableCollection<LayerDefinition> myObservableCollection =
               new System.Collections.ObjectModel.ObservableCollection<LayerDefinition>();
            myObservableCollection.Add(myDefinition);
            //findParameters.LayerDefinitions = myObservableCollection; //设置查询的LayerDefinitions

            //异步执行
            findTask.ExecuteAsync(findParameters);
        }

        private void FindTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("查询结果为空！");
        }
        private static ESRI.ArcGIS.Client.Projection.WebMercator mercator =
            new ESRI.ArcGIS.Client.Projection.WebMercator();
        private void FindTask_ExecuteCompleted(object sender, FindEventArgs e)
        {
            //MessageBox.Show(e.FindResults.Count.ToString());
            if (e.FindResults.Count > 0)
            {
                GraphicsLayer layer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;
                layer.Graphics.Clear();

                Graphic graphic = new Graphic()
                {
                    Geometry=new ESRI.ArcGIS.Client.Geometry.MapPoint(e.FindResults[0].Feature.Geometry.Extent.GetCenter().X, e.FindResults[0].Feature.Geometry.Extent.GetCenter().Y),
                    //Geometry = mercator.FromGeographic(e.FindResults[0].Feature.Geometry.Extent.GetCenter()), //new MapPoint((double)listGPoint[i].X, (double)listGPoint[i].Y)
                    Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol //LayoutRoot.Resources["Symbol_Point"] as Symbol
                };
                //graphic.Geometry.SpatialReference = MainMap.SpatialReference;
                layer.Graphics.Add(graphic);

                /*
                MainMap.Extent = new Envelope() {
                    XMax = e.FindResults[0].Feature.Geometry.Extent.GetCenter().X * (1.1),
                    XMin = e.FindResults[0].Feature.Geometry.Extent.GetCenter().X * (0.9),
                    YMax = e.FindResults[0].Feature.Geometry.Extent.GetCenter().Y * (1.1),
                    YMin = e.FindResults[0].Feature.Geometry.Extent.GetCenter().Y * (0.9)
                };
                */
                //MainMap.Extent = new Envelope()
                //{
                //    XMax = e.FindResults[0].Feature.Geometry.Extent.GetCenter().X * (1.1),
                //    XMin = e.FindResults[0].Feature.Geometry.Extent.GetCenter().X * (0.9),
                //    YMax = e.FindResults[0].Feature.Geometry.Extent.GetCenter().Y * (1.1),
                //    YMin = e.FindResults[0].Feature.Geometry.Extent.GetCenter().Y * (0.9)
                //};

                //MainMap.PanTo(graphic.Geometry);
                MainMap.PanTo(graphic.Geometry);
            }
            else
            {
                MessageBox.Show("查询结果为空！");
            }
        }

        private void btInfoList_Click(object sender, RoutedEventArgs e)
        {
            InfoList frmInfoList = new InfoList();
            frmInfoList.Show();
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer layer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;
            layer.Graphics.Clear();
        }

        private void btAbout_Click(object sender, RoutedEventArgs e)
        {
            Other.About frmAbout = new Other.About();
            frmAbout.Show();
        }

        private void btOrder_Click(object sender, RoutedEventArgs e)
        {
            Order.OrderList frmOrderList = new Order.OrderList();
            frmOrderList.Show();
        }

        private void btChPassword_Click(object sender, RoutedEventArgs e)
        {
            Modify frmModify = new Modify();
            frmModify.Show();
        }

        private void btXuanran_Click(object sender, RoutedEventArgs e)
        {
            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                Where = "1>0",
                OutSpatialReference = MainMap.SpatialReference,
                ReturnGeometry = true
            };
            query.OutFields.Add("*");

            QueryTask queryTask =
                new QueryTask("http://liyafei-server:6080/arcgis/rest/services/Nanjing/MapServer/22");

            queryTask.ExecuteCompleted += (evtsender, args) =>
            {
                if (args.FeatureSet == null)
                    return;
                _featureSet = args.FeatureSet;
                SetRangeValues();
                //RenderButton.IsEnabled = true;
            };
            queryTask.Failed += QueryTask_Failed;

            queryTask.ExecuteAsync(query);
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
        }


        private void btLine_Click(object sender, RoutedEventArgs e)
        {
            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                Where = "name like '%" + tbSearchWords.Text + "%'",
                OutSpatialReference = MainMap.SpatialReference,
                ReturnGeometry = true
            };
            query.OutFields.Add("*");

            QueryTask queryTask =
                new QueryTask("http://liyafei-server:6080/arcgis/rest/services/Nanjing/MapServer/0");

            queryTask.ExecuteCompleted += (evtsender, args) =>
            {
                if (args.FeatureSet == null|| args.FeatureSet.Count<Graphic>()<=0)
                {
                    MessageBox.Show("没有符合条件的结果！");
                    return;
                }
                    
                //_featureSet = args.FeatureSet;
                GraphicsLayer graphicsLayer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;
                graphicsLayer.Graphics.Clear();

                Graphic graphicFeature = args.FeatureSet.Features[0];

                ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol symbol = new SimpleLineSymbol()
                {
                    Color = new SolidColorBrush(Color.FromArgb(123, 0, 32, 32)),
                    Width=2
                };

                Graphic graphic = new Graphic();
                graphic.Geometry = graphicFeature.Geometry;
                graphic.Attributes.Add("name", graphicFeature.Attributes["name"].ToString());
                graphic.Symbol = symbol;

                graphicsLayer.Graphics.Add(graphic);
                MainMap.Extent = graphic.Geometry.Extent;
            };
            queryTask.Failed += QueryTask_Failed;

            queryTask.ExecuteAsync(query);
        }

        private void btPoint_Click(object sender, RoutedEventArgs e)
        {
            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                Where = "name like '%"+tbSearchWords.Text+"%'",
                OutSpatialReference = MainMap.SpatialReference,
                ReturnGeometry = true
            };
            query.OutFields.Add("*");

            QueryTask queryTask =
                new QueryTask("http://liyafei-server:6080/arcgis/rest/services/Nanjing/MapServer/1");

            queryTask.ExecuteCompleted += (evtsender, args) =>
            {
                if (args.FeatureSet == null || args.FeatureSet.Count<Graphic>() <= 0)
                {
                    MessageBox.Show("没有符合条件的结果！");
                    return;
                }
                //_featureSet = args.FeatureSet;
                GraphicsLayer graphicsLayer = MainMap.Layers["QueryResultLayer"] as GraphicsLayer;
                graphicsLayer.Graphics.Clear();

                Graphic graphicFeature = args.FeatureSet.Features[0];

                ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol symbol = new SimpleMarkerSymbol()
                {
                    Color = new SolidColorBrush(Color.FromArgb(123, 0, 32,32)),
                    Size=18
                };

                Graphic graphic = new Graphic();
                graphic.Geometry = graphicFeature.Geometry;
                graphic.Attributes.Add("name", graphicFeature.Attributes["name"].ToString());
                graphic.Symbol = symbol;

                graphicsLayer.Graphics.Add(graphic);
                MainMap.Extent = graphic.Geometry.Extent;
            };
            queryTask.Failed += QueryTask_Failed;

            queryTask.ExecuteAsync(query);
        }
    }

    public struct ThematicItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CalcField { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string MinName { get; set; }
        public string MaxName { get; set; }
        public List<double> RangeStarts { get; set; }

    }
}
