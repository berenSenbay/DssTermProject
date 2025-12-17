using System;
using System.Collections.Generic;
using System.Windows.Forms;
using weka.core;
using weka.classifiers;
using weka.classifiers.meta;
using weka.filters;
using weka.filters.unsupervised.attribute;
using System.Drawing;
using System.IO;

namespace csharp_ml_algorithm_comparator
{
    public partial class Form1 : Form
    {
        // Global variables to store the loaded dataset and the best performing model
        private Instances originalData;
        private Classifier cl_BEST;

        // Configuration for the train/test split percentage (66% training, 34% testing)
        private const int percentSplit = 66;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles file selection (ARFF or CSV) and initiates the learning process.
        /// </summary>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Data files (*.arff, *.csv)|*.arff;*.csv" };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtFilePath.Text = ofd.FileName;

                    // User information
                    lblStatus.Text = "Processing... The program may freeze for a moment, please wait.";
                    lblStatus.ForeColor = Color.Red;

                    Application.DoEvents();

                    // Load the dataset
                    if (ofd.FileName.ToLower().EndsWith(".csv"))
                    {
                        weka.core.converters.CSVLoader loader = new weka.core.converters.CSVLoader();
                        loader.setSource(new java.io.File(ofd.FileName));
                        originalData = loader.getDataSet();
                    }
                    else
                    {
                        originalData = new Instances(new java.io.FileReader(ofd.FileName));
                    }

                    if (originalData.classIndex() == -1)
                        originalData.setClassIndex(originalData.numAttributes() - 1);

                    // Evaluate the metrics
                    EvaluateAlgorithms();
                    BuildInterface();

                    // Results
                    lblStatus.ForeColor = Color.FromArgb(127, 140, 141);
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
            // Data shuffle
            java.util.Random rand = new java.util.Random(1);
            originalData.randomize(rand);

            double maxAcc = -1;
            string winnerName = "";

            // 11 Classification Models
            var configs = new List<(string Name, Classifier Cl, string FilterMode)> {
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
                    // Apply normalization
                    MultiFilter multi = new MultiFilter();
                    multi.setFilters(new Filter[] { new Normalize(), new NominalToBinary() });
                    fc.setFilter(multi);
                }
                else if (config.FilterMode == "Discretize")
                {
                    // Discreatization
                    fc.setFilter(new Discretize());
                }
                else
                {
                    fc.setFilter(new weka.filters.AllFilter());
                }

                double acc = CalculateAccuracy(fc);

                // Update the highest acc
                if (acc > maxAcc)
                {
                    maxAcc = acc;
                    winnerName = config.Name;
                    cl_BEST = fc;
                }
            }

            // Train the final winner model on the full dataset
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
                // Train test split
                int trainSize = originalData.numInstances() * percentSplit / 100;
                int testSize = originalData.numInstances() - trainSize;
                if (trainSize < 1 || testSize < 1) return 0;

                Instances train = new Instances(originalData, 0, trainSize);
                Instances test = new Instances(originalData, trainSize, testSize);

                //This the model built on test dataset
                fc.buildClassifier(train);

                int correct = 0;
                for (int i = 0; i < test.numInstances(); i++)
                {
                    if (fc.classifyInstance(test.instance(i)) == test.instance(i).classValue())
                        correct++;
                }
                return (double)correct / testSize * 100.0;
            }
            catch { return 0; }
        }

        private void BuildInterface()
        {
            pnlAttributes.Controls.Clear();
            pnlAttributes.AutoScroll = true;

            for (int i = 0; i < originalData.numAttributes() - 1; i++)
            {
                var attr = originalData.attribute(i);
                Label lbl = new Label { Text = attr.name(), Width = 150 };
                pnlAttributes.Controls.Add(lbl);

                if (attr.isNominal())
                {
                    // Dropdown options
                    ComboBox cb = new ComboBox { Name = "attr_" + i, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                    for (int j = 0; j < attr.numValues(); j++) cb.Items.Add(attr.value(j));
                    if (cb.Items.Count > 0) cb.SelectedIndex = 0;
                    pnlAttributes.Controls.Add(cb);
                }
                else
                {
                    // Numeric options
                    TextBox tb = new TextBox { Name = "attr_" + i, Width = 150, Text = "0" };
                    pnlAttributes.Controls.Add(tb);
                }
                pnlAttributes.SetFlowBreak(pnlAttributes.Controls[pnlAttributes.Controls.Count - 1], true);
            }
        }

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            if (cl_BEST == null || originalData == null) return;

            try
            {
                Instance inst = new DenseInstance(originalData.numAttributes());
                inst.setDataset(originalData);

                for (int i = 0; i < originalData.numAttributes() - 1; i++)
                {
                    Control c = pnlAttributes.Controls.Find("attr_" + i, true)[0];
                    if (originalData.attribute(i).isNominal())
                        inst.setValue(i, ((ComboBox)c).Text);
                    else
                    {
                        double val = 0;
                        double.TryParse(c.Text, out val);
                        inst.setValue(i, val);
                    }
                }

                // Predict the class using the best model
                double resultIndex = cl_BEST.classifyInstance(inst);
                string resultLabel = originalData.classAttribute().value((int)resultIndex);

                lblResult.Text = "PREDICTION: " + resultLabel;
                lblResult.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prediction Error: " + ex.Message);
            }
        }
    }
}