using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Imaging.Filters;

namespace Flat_Edge_Detection
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image0.png");
            Tuple<double, double> variances0 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image1.png");
            Tuple<double, double> variances1 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image2.png");
            Tuple<double, double> variances2 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image3.png");
            Tuple<double, double> variances3 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image4.png");
            Tuple<double, double> variances4 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image5.png");
            Tuple<double, double> variances5 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image6.png");
            Tuple<double, double> variances6 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image7.png");
            Tuple<double, double> variances7 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image8.png");
            Tuple<double, double> variances8 = analyzeShred(shred);

            shred = AForge.Imaging.Image.FromFile("C:\\Users\\jacob\\Pictures\\image9.png");
            Tuple<double, double> variances9 = analyzeShred(shred);


            Console.Write("shred0:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances0.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances0.Item2);
            Console.WriteLine();

            Console.Write("shred1:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances1.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances1.Item2);
            Console.WriteLine();

            Console.Write("shred2:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances2.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances2.Item2);
            Console.WriteLine();

            Console.Write("shred3:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances3.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances3.Item2);
            Console.WriteLine();

            Console.Write("shred4:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances4.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances4.Item2);
            Console.WriteLine();

            Console.Write("shred5:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances5.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances5.Item2);
            Console.WriteLine();

            Console.Write("shred6:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances6.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances6.Item2);
            Console.WriteLine();

            Console.Write("shred7:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances7.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances7.Item2);
            Console.WriteLine();

            Console.Write("shred8:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances8.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances8.Item2);
            Console.WriteLine();

            Console.Write("shred9:");
            Console.WriteLine();
            Console.Write("variance for left side: ");
            Console.Write(variances9.Item1);
            Console.WriteLine();
            Console.Write("variance for right side: ");
            Console.Write(variances9.Item2);
            Console.WriteLine();

            Console.ReadLine();
        }

        static Tuple<double, double> analyzeShred(Bitmap shred)
        {
            //parameters
            double queueLength = 10;
            
            double lExpected = 0;
            double rExpected = 0;

            double lrunsum = 0;
            double rrunsum = 0;

            double lStdDev = 0;
            double rStdDev = 0;

            double lLowBound = 0;
            double lHighBound = 0;
            double rLowBound = 0;
            double rHighBound = 0;

            int count = 0;

            Queue<Tuple<Point, double>> lData = new Queue<Tuple<Point, double>>();
            Queue<Tuple<Point, double>> rData = new Queue<Tuple<Point, double>>();
            ArrayList lsamples = new ArrayList();
            ArrayList rsamples = new ArrayList();


            for (int j = (int)(shred.Height * 0.10); j < (int)(shred.Height * 0.9); j++)
            {
                if (lData.Count ==  queueLength)
                {
                    lExpected = getPrediction(lData, j);
                    rExpected = getPrediction(rData, j);

                    lLowBound = lExpected - lStdDev * 3;
                    lHighBound = lExpected + lStdDev * 3;

                    rLowBound = rExpected - rStdDev * 3;
                    rHighBound = rExpected + rStdDev * 3;
                }
                ArrayList xHits = new ArrayList();

                //traverse each row to record edge location
                for (int i = 0; i < shred.Width; i++)
                {
                    if (shred.GetPixel(i, j).R >= 1 || shred.GetPixel(i, j).G >= 1 || shred.GetPixel(i, j).B >= 1)
                    {
                        xHits.Add(i);
                    }
                }

                int currentLowest = 99999;
                int currentHighest = 0;

                if (xHits.Count >= 2)
                {
                    //abstract edges from xHits
                    foreach (int x in xHits)
                    {
                        if (x < currentLowest)
                        {
                            currentLowest = x;
                        }
                        if (x > currentHighest)
                        {
                            currentHighest = x;
                        }
                    }

                    //add data to queue's
                    if (lData.Count < queueLength)
                    {
                        lData.Enqueue(new Tuple<Point, double>(new Point(currentLowest, j), 0));
                        rData.Enqueue(new Tuple<Point, double>(new Point(currentHighest, j), 99999));
                    }
                    else
                    {
                            lData.Enqueue(new Tuple<Point, double>(new Point(currentLowest, j), lExpected));
                            lData.Dequeue();
                            rData.Enqueue(new Tuple<Point, double>(new Point(currentHighest, j), rExpected));
                            rData.Dequeue();
                            
                            lrunsum += (currentLowest - lExpected) * (currentLowest - lExpected);
                            rrunsum += (currentHighest - rExpected) * (currentHighest - rExpected);
                            count++;
                    }
                }
            }

            //find average variance
            double lVariance = lrunsum / (double)count;
            double rVariance = rrunsum / (double)count;

            Tuple<double, double> output = new Tuple<double, double>(lVariance, rVariance);

            return output;
        }


        static double getPrediction(Queue<Tuple<Point, double>> input, int j)
        {
            int xrunsum = 0;
            int yrunsum = 0;
            int xSquaredRunsum = 0;
            int productRunsum = 0;
            int counter = 0;

            foreach (Tuple<Point, double> x in input)
            {
                Point p = x.Item1;
                int X = p.Y;
                int Y = p.X;
                xrunsum += X;
                yrunsum += Y;
                xSquaredRunsum += X * X;
                productRunsum += X * Y;
                counter++;
            }

            double xAve = (double)xrunsum / (double)counter;
            double yAve = (double)yrunsum / (double)counter;


            double m = (double)(counter * productRunsum - xrunsum * yrunsum) / (double)(counter * xSquaredRunsum - xrunsum * xrunsum);
            double b = yAve - m * xAve;
            double output = m * j + b;
            return output;

        }
    }
}
