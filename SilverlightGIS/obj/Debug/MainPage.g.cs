﻿#pragma checksum "F:\Demo\SLGIS\SilverlightGIS\SilverlightGIS\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "507ED4C947ED45DEC7E4A2AF22B43223"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SilverlightGIS {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ESRI.ArcGIS.Client.Map MainMap;
        
        internal ESRI.ArcGIS.Client.Toolkit.ScaleLine MyScaleLine;
        
        internal ESRI.ArcGIS.Client.Toolkit.Navigation MyNavgation;
        
        internal ESRI.ArcGIS.Client.Toolkit.OverviewMap MyOverviewMap;
        
        internal System.Windows.Controls.TextBox tbSearchWords;
        
        internal System.Windows.Controls.Button btSearchMap;
        
        internal System.Windows.Controls.Button btLine;
        
        internal System.Windows.Controls.Button btPoint;
        
        internal System.Windows.Controls.Button btOrder;
        
        internal System.Windows.Controls.Button btInfoList;
        
        internal System.Windows.Controls.Button btClear;
        
        internal System.Windows.Controls.Button btChPassword;
        
        internal System.Windows.Controls.Button btXuanran;
        
        internal System.Windows.Controls.Button btAbout;
        
        internal System.Windows.Controls.Grid LegendGrid;
        
        internal System.Windows.Shapes.Path LegendCollapsedTriangle;
        
        internal System.Windows.Shapes.Path LegendExpandedTriangle;
        
        internal System.Windows.Controls.TextBlock LegendCollapsedTitle;
        
        internal System.Windows.Controls.TextBlock LegendTitle;
        
        internal System.Windows.Controls.StackPanel LegendStackPanel;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightGIS;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MainMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("MainMap")));
            this.MyScaleLine = ((ESRI.ArcGIS.Client.Toolkit.ScaleLine)(this.FindName("MyScaleLine")));
            this.MyNavgation = ((ESRI.ArcGIS.Client.Toolkit.Navigation)(this.FindName("MyNavgation")));
            this.MyOverviewMap = ((ESRI.ArcGIS.Client.Toolkit.OverviewMap)(this.FindName("MyOverviewMap")));
            this.tbSearchWords = ((System.Windows.Controls.TextBox)(this.FindName("tbSearchWords")));
            this.btSearchMap = ((System.Windows.Controls.Button)(this.FindName("btSearchMap")));
            this.btLine = ((System.Windows.Controls.Button)(this.FindName("btLine")));
            this.btPoint = ((System.Windows.Controls.Button)(this.FindName("btPoint")));
            this.btOrder = ((System.Windows.Controls.Button)(this.FindName("btOrder")));
            this.btInfoList = ((System.Windows.Controls.Button)(this.FindName("btInfoList")));
            this.btClear = ((System.Windows.Controls.Button)(this.FindName("btClear")));
            this.btChPassword = ((System.Windows.Controls.Button)(this.FindName("btChPassword")));
            this.btXuanran = ((System.Windows.Controls.Button)(this.FindName("btXuanran")));
            this.btAbout = ((System.Windows.Controls.Button)(this.FindName("btAbout")));
            this.LegendGrid = ((System.Windows.Controls.Grid)(this.FindName("LegendGrid")));
            this.LegendCollapsedTriangle = ((System.Windows.Shapes.Path)(this.FindName("LegendCollapsedTriangle")));
            this.LegendExpandedTriangle = ((System.Windows.Shapes.Path)(this.FindName("LegendExpandedTriangle")));
            this.LegendCollapsedTitle = ((System.Windows.Controls.TextBlock)(this.FindName("LegendCollapsedTitle")));
            this.LegendTitle = ((System.Windows.Controls.TextBlock)(this.FindName("LegendTitle")));
            this.LegendStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("LegendStackPanel")));
        }
    }
}

