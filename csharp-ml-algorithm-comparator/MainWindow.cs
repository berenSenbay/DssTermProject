using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using weka.classifiers;
using weka.classifiers.meta;
using weka.core;
using weka.filters;
using weka.filters.unsupervised.attribute;

namespace csharp_ml_algorithm_comparator
{
    public class MainWindow : Window
    {
        private Instances originalData;
        private Classifier cl_BEST;
        private const int percentSplit = 66;

        private TextBox txtFilePath;
        private TextBlock lblStatus;
        private StackPanel pnlAttributes;
        private TextBlock lblResult;

        private readonly Dictionary<int, Control> attributeInputs = new Dictionary<int, Control>();

        public MainWindow()
        {
            BuildLayout();
        }

        private void BuildLayout()
        {
            Title = "Machine Learning Algorithm Comparator";
            Width = 960;
            Height = 620;
            MinWidth = 900;
            MinHeight = 560;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Background = new LinearGradientBrush(
                Color.FromRgb(28, 16, 44),
                Color.FromRgb(20, 34, 64),
                45);
            FontFamily = new FontFamily("Segoe UI");

            var root = new Grid();

            var contentGrid = new Grid
            {
                Margin = new Thickness(0)
            };
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var header = new Border
            {
                Height = 86,
                Background = new LinearGradientBrush(
                    Color.FromRgb(214, 94, 255),
                    Color.FromRgb(74, 132, 255),
                    20)
            };
            header.Child = new TextBlock
            {
                Text = "AI Classifier",
                FontSize = 28,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(22, 0, 0, 0)
            };
            Grid.SetRow(header, 0);
            contentGrid.Children.Add(header);

            var fileRow = new Grid { Margin = new Thickness(24, 18, 24, 0) };
            fileRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            fileRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            txtFilePath = new TextBox
            {
                IsReadOnly = true,
                Height = 36,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(12, 0, 12, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(229, 234, 244)),
                Background = new SolidColorBrush(Color.FromRgb(34, 43, 66)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(75, 88, 118)),
                BorderThickness = new Thickness(1)
            };
            Grid.SetColumn(txtFilePath, 0);
            fileRow.Children.Add(txtFilePath);

            var btnBrowse = new Button
            {
                Content = "Load Dataset",
                Width = 140,
                Height = 36,
                Margin = new Thickness(10, 0, 0, 0),
                Background = new LinearGradientBrush(
                    Color.FromRgb(214, 94, 255),
                    Color.FromRgb(92, 120, 255),
                    0),
                Foreground = Brushes.White,
                FontWeight = FontWeights.SemiBold,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            ApplyGradientButtonStyle(btnBrowse,
                Color.FromRgb(214, 94, 255),
                Color.FromRgb(92, 120, 255),
                Color.FromRgb(228, 118, 255),
                Color.FromRgb(112, 140, 255));
            btnBrowse.Click += btnBrowse_Click;
            Grid.SetColumn(btnBrowse, 1);
            fileRow.Children.Add(btnBrowse);

            Grid.SetRow(fileRow, 1);
            contentGrid.Children.Add(fileRow);

            lblStatus = new TextBlock
            {
                Text = "Ready to analyze...",
                Margin = new Thickness(24, 10, 24, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(184, 196, 216)),
                FontSize = 13
            };
            Grid.SetRow(lblStatus, 2);
            contentGrid.Children.Add(lblStatus);

            var bodyGrid = new Grid { Margin = new Thickness(24, 12, 24, 20) };
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.2, GridUnitType.Star) });
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });

            var attributesBorder = new Border
            {
                Margin = new Thickness(0, 0, 10, 0),
                CornerRadius = new CornerRadius(12),
                BorderBrush = new SolidColorBrush(Color.FromRgb(75, 88, 118)),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Color.FromRgb(28, 36, 56)),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 16,
                    ShadowDepth = 3,
                    Opacity = 0.22,
                    Color = Color.FromRgb(8, 12, 26)
                }
            };

            var scroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(2)
            };

            pnlAttributes = new StackPanel { Margin = new Thickness(12) };
            scroll.Content = pnlAttributes;
            attributesBorder.Child = scroll;
            Grid.SetColumn(attributesBorder, 0);
            bodyGrid.Children.Add(attributesBorder);

            var rightCard = new Border
            {
                Margin = new Thickness(10, 0, 0, 0),
                CornerRadius = new CornerRadius(12),
                BorderBrush = new SolidColorBrush(Color.FromRgb(75, 88, 118)),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Color.FromRgb(28, 36, 56)),
                Padding = new Thickness(14),
                Effect = new DropShadowEffect
                {
                    BlurRadius = 16,
                    ShadowDepth = 3,
                    Opacity = 0.22,
                    Color = Color.FromRgb(8, 12, 26)
                }
            };

            var rightPanel = new StackPanel();

            var btnDiscover = new Button
            {
                Content = "Discover Class",
                Height = 46,
                Foreground = Brushes.White,
                FontSize = 17,
                FontWeight = FontWeights.SemiBold,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            ApplyGradientButtonStyle(btnDiscover,
                Color.FromRgb(228, 102, 255),
                Color.FromRgb(75, 136, 255),
                Color.FromRgb(238, 128, 255),
                Color.FromRgb(98, 154, 255));
            btnDiscover.Click += btnDiscover_Click;
            rightPanel.Children.Add(btnDiscover);

            var resultTitle = new TextBlock
            {
                Text = "Analysis Result",
                Margin = new Thickness(0, 16, 0, 0),
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                Foreground = new SolidColorBrush(Color.FromRgb(184, 196, 216))
            };
            rightPanel.Children.Add(resultTitle);

            var resultBorder = new Border
            {
                Margin = new Thickness(0, 8, 0, 0),
                MinHeight = 54,
                Padding = new Thickness(10, 8, 10, 8),
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(Color.FromRgb(34, 43, 66)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(75, 88, 118)),
                BorderThickness = new Thickness(1)
            };
            lblResult = new TextBlock
            {
                Text = "- Waiting -",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(222, 231, 247)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
            resultBorder.Child = lblResult;
            rightPanel.Children.Add(resultBorder);

            rightCard.Child = rightPanel;
            Grid.SetColumn(rightCard, 1);
            bodyGrid.Children.Add(rightCard);

            Grid.SetRow(bodyGrid, 3);
            contentGrid.Children.Add(bodyGrid);

            root.Children.Add(contentGrid);
            Content = root;
        }

        private void ApplyGradientButtonStyle(Button button, Color normalStart, Color normalEnd, Color hoverStart, Color hoverEnd)
        {
            var normalBrush = new LinearGradientBrush(normalStart, normalEnd, 0);
            var hoverBrush = new LinearGradientBrush(hoverStart, hoverEnd, 0);

            button.Background = normalBrush;

            button.MouseEnter += (s, e) =>
            {
                button.Background = hoverBrush;
                button.Effect = new DropShadowEffect
                {
                    BlurRadius = 14,
                    ShadowDepth = 2,
                    Opacity = 0.28,
                    Color = Color.FromRgb(16, 20, 36)
                };
            };

            button.MouseLeave += (s, e) =>
            {
                button.Background = normalBrush;
                button.Effect = null;
            };
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog { Filter = "Data files (*.arff;*.csv)|*.arff;*.csv" };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    txtFilePath.Text = ofd.FileName;
                    lblStatus.Text = "Processing... The program may freeze for a moment, please wait.";
                    lblStatus.Foreground = Brushes.Red;

                    if (ofd.FileName.ToLower().EndsWith(".csv"))
                    {
                        var loader = new weka.core.converters.CSVLoader();
                        loader.setSource(new java.io.File(ofd.FileName));
                        originalData = loader.getDataSet();
                    }
                    else
                    {
                        originalData = new Instances(new java.io.FileReader(ofd.FileName));
                    }

                    if (originalData.classIndex() == -1)
                        originalData.setClassIndex(originalData.numAttributes() - 1);

                    EvaluateAlgorithms();
                    BuildInterface();

                    lblStatus.Foreground = new SolidColorBrush(Color.FromRgb(127, 140, 141));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File Load Error: " + ex.Message);
                    lblStatus.Text = "Error loading file.";
                }
            }
        }

        private void EvaluateAlgorithms()
        {
            java.util.Random rand = new java.util.Random(1);
            originalData.randomize(rand);

            double maxAcc = -1;
            string winnerName = "";

            var configs = new List<(string Name, Classifier Cl, string FilterMode)>
            {
                ("3-NN (IBk)", new weka.classifiers.lazy.IBk(3), "Numeric"),
                ("5-NN (IBk)", new weka.classifiers.lazy.IBk(5), "Numeric"),
                ("7-NN (IBk)", new weka.classifiers.lazy.IBk(7), "Numeric"),
                ("9-NN (IBk)", new weka.classifiers.lazy.IBk(9), "Numeric"),
                ("Naive Bayes", new weka.classifiers.bayes.NaiveBayes(), "Discretize"),
                ("Logistic Regression", new weka.classifiers.functions.Logistic(), "Numeric"),
                ("J48 Decision Tree", new weka.classifiers.trees.J48(), "None"),
                ("Random Forest", new weka.classifiers.trees.RandomForest(), "None"),
                ("Random Tree", new weka.classifiers.trees.RandomTree(), "None"),
                ("SVM (SMO)", new weka.classifiers.functions.SMO(), "Numeric"),
                ("MLP (Neural Net)", new weka.classifiers.functions.MultilayerPerceptron(), "Numeric")
            };

            foreach (var config in configs)
            {
                FilteredClassifier fc = new FilteredClassifier();
                fc.setClassifier(config.Cl);

                if (config.FilterMode == "Numeric")
                {
                    MultiFilter multi = new MultiFilter();
                    multi.setFilters(new Filter[] { new Normalize(), new NominalToBinary() });
                    fc.setFilter(multi);
                }
                else if (config.FilterMode == "Discretize")
                {
                    fc.setFilter(new Discretize());
                }
                else
                {
                    fc.setFilter(new weka.filters.AllFilter());
                }

                double acc = CalculateAccuracy(fc);

                if (acc > maxAcc)
                {
                    maxAcc = acc;
                    winnerName = config.Name;
                    cl_BEST = fc;
                }
            }

            if (cl_BEST != null)
            {
                cl_BEST.buildClassifier(originalData);
                lblStatus.Text = $"Best: {winnerName} (%{maxAcc:F2} Acc)";
            }
        }

        private double CalculateAccuracy(Classifier fc)
        {
            try
            {
                int trainSize = originalData.numInstances() * percentSplit / 100;
                int testSize = originalData.numInstances() - trainSize;
                if (trainSize < 1 || testSize < 1) return 0;

                Instances train = new Instances(originalData, 0, trainSize);
                Instances test = new Instances(originalData, trainSize, testSize);

                fc.buildClassifier(train);

                int correct = 0;
                for (int i = 0; i < test.numInstances(); i++)
                {
                    if (fc.classifyInstance(test.instance(i)) == test.instance(i).classValue())
                        correct++;
                }

                return (double)correct / testSize * 100.0;
            }
            catch
            {
                return 0;
            }
        }

        private void BuildInterface()
        {
            pnlAttributes.Children.Clear();
            attributeInputs.Clear();

            for (int i = 0; i < originalData.numAttributes() - 1; i++)
            {
                var attr = originalData.attribute(i);

                var row = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var lbl = new TextBlock
                {
                    Text = attr.name(),
                    Width = 190,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(208, 219, 238))
                };

                row.Children.Add(lbl);

                if (attr.isNominal())
                {
                    ComboBox cb = new ComboBox
                    {
                        Width = 230,
                        Height = 30,
                        IsEditable = false,
                        Foreground = new SolidColorBrush(Color.FromRgb(229, 234, 244)),
                        Background = new SolidColorBrush(Color.FromRgb(42, 52, 78)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(88, 102, 136)),
                        BorderThickness = new Thickness(1)
                    };

                    for (int j = 0; j < attr.numValues(); j++)
                        cb.Items.Add(attr.value(j));

                    if (cb.Items.Count > 0)
                        cb.SelectedIndex = 0;

                    attributeInputs[i] = cb;
                    row.Children.Add(cb);
                }
                else
                {
                    var tb = new TextBox
                    {
                        Width = 230,
                        Height = 30,
                        Text = "0",
                        Padding = new Thickness(8, 0, 8, 0),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromRgb(229, 234, 244)),
                        Background = new SolidColorBrush(Color.FromRgb(42, 52, 78)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(88, 102, 136)),
                        BorderThickness = new Thickness(1)
                    };

                    attributeInputs[i] = tb;
                    row.Children.Add(tb);
                }

                pnlAttributes.Children.Add(row);
            }
        }

        private void btnDiscover_Click(object sender, RoutedEventArgs e)
        {
            if (cl_BEST == null || originalData == null) return;

            try
            {
                Instance inst = new DenseInstance(originalData.numAttributes());
                inst.setDataset(originalData);

                for (int i = 0; i < originalData.numAttributes() - 1; i++)
                {
                    Control c = attributeInputs[i];
                    if (originalData.attribute(i).isNominal())
                    {
                        inst.setValue(i, ((ComboBox)c).Text);
                    }
                    else
                    {
                        double val = 0;
                        double.TryParse(((TextBox)c).Text, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                        inst.setValue(i, val);
                    }
                }

                double resultIndex = cl_BEST.classifyInstance(inst);
                string resultLabel = originalData.classAttribute().value((int)resultIndex);

                lblResult.Text = "PREDICTION: " + resultLabel;
                lblResult.Foreground = Brushes.DarkGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prediction Error: " + ex.Message);
            }
        }
    }
}
