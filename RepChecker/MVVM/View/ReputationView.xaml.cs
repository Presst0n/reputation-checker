﻿using RepChecker.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RepChecker.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReputationView.xaml
    /// </summary>
    public partial class ReputationView : UserControl
    {
        public ReputationView(/*ReputationViewModel reputationViewModel*/)
        {
            InitializeComponent();
            //DataContext = reputationViewModel;
        }
    }
}