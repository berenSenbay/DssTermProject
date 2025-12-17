using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using weka.core;
using weka.classifiers;
using weka.filters;

namespace csharp_ml_algorithm_comparator
{
    public partial class Form1 : Form
    {
        private Instances originalData;
        private Classifier cl_BEST;
        private const int percentSplit = 66;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "ARFF files (*.arff)|*.arff" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                originalData = new Instances(new java.io.FileReader(ofd.FileName));
                originalData.setClassIndex(originalData.numAttributes() - 1);
                EvaluateAlgorithms();
                BuildInterface();
            }
        }

        private void EvaluateAlgorithms()
        {
            double maxAcc = -1;
            string winner = "";
            var algos = new List<(string Name, Classifier Cl, bool Num, bool Nom)> {
                ("1-NN", new weka.classifiers.lazy.IBk(1), true, false),
                ("3-NN", new weka.classifiers.lazy.IBk(3), true, false),
                ("Naive Bayes", new weka.classifiers.bayes.NaiveBayes(), false, true),
                ("J48", new weka.classifiers.trees.J48(), false, false),
                ("SVM", new weka.classifiers.functions.SMO(), true, false),
                ("MLP", new weka.classifiers.functions.MultilayerPerceptron(), true, false),
                ("Logistic", new weka.classifiers.functions.Logistic(), true, false),
                ("RandomForest", new weka.classifiers.trees.RandomForest(), false, false),
                ("RandomTree", new weka.classifiers.trees.RandomTree(), false, false),
                ("7-NN", new weka.classifiers.lazy.IBk(7), true, false)
            };

            foreach (var a in algos)
            {
                double acc = TestAlgo(new Instances(originalData), a.Cl, a.Num, a.Nom);
                if (acc > maxAcc) { maxAcc = acc; winner = a.Name; cl_BEST = a.Cl; }
            }
            cl_BEST.buildClassifier(originalData);
            lblStatus.Text = $"{winner} is best (%{maxAcc:F2})";
        }

        private double TestAlgo(Instances insts, Classifier cl, bool num, bool nom)
        {
            try
            {
                if (num)
                {
                    var n = new weka.filters.unsupervised.attribute.Normalize();
                    n.setInputFormat(insts); insts = Filter.useFilter(insts, n);
                    var b = new weka.filters.unsupervised.attribute.NominalToBinary();
                    b.setInputFormat(insts); insts = Filter.useFilter(insts, b);
                }
                if (nom)
                {
                    var d = new weka.filters.unsupervised.attribute.Discretize();
                    d.setInputFormat(insts); insts = Filter.useFilter(insts, d);
                }
                int trainSize = insts.numInstances() * percentSplit / 100;
                cl.buildClassifier(new Instances(insts, 0, trainSize));
                int correct = 0;
                for (int i = trainSize; i < insts.numInstances(); i++)
                    if (cl.classifyInstance(insts.instance(i)) == insts.instance(i).classValue()) correct++;
                return (double)correct / (insts.numInstances() - trainSize) * 100.0;
            }
            catch { return 0; }
        }

        private void BuildInterface()
        {
            pnlAttributes.Controls.Clear();
            for (int i = 0; i < originalData.numAttributes() - 1; i++)
            {
                var attr = originalData.attribute(i);
                pnlAttributes.Controls.Add(new Label { Text = attr.name(), Width = 80 });
                if (attr.isNominal())
                {
                    var cb = new ComboBox { Name = "a" + i, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
                    for (int j = 0; j < attr.numValues(); j++) cb.Items.Add(attr.value(j));
                    pnlAttributes.Controls.Add(cb);
                }
                else pnlAttributes.Controls.Add(new TextBox { Name = "a" + i, Width = 120 });
                pnlAttributes.SetFlowBreak(pnlAttributes.Controls[pnlAttributes.Controls.Count - 1], true);
            }
        }

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            Instance inst = new DenseInstance(originalData.numAttributes());
            inst.setDataset(originalData);
            for (int i = 0; i < originalData.numAttributes() - 1; i++)
            {
                Control c = pnlAttributes.Controls.Find("a" + i, true)[0];
                if (originalData.attribute(i).isNominal()) inst.setValue(i, ((ComboBox)c).Text);
                else inst.setValue(i, double.Parse(c.Text));
            }
            lblResult.Text = "RESULT: " + originalData.classAttribute().value((int)cl_BEST.classifyInstance(inst));
        }
    }
}