using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using POLISH2;
using System.Windows.Forms;



namespace polishing
{
    class Produce_x_b_F_C
    {
       public void non_sphere(double dist,double symbol, double n, double vc, double H, double D, double R, double K, double[] A, out double t)
        {                        //,n:抛光次数，vc:工件最大转速，H:模仁到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间
            double h = 9.66;
           dist = 0.1;//h = 9.66,
            t = 0;
          // POLISH2.Form1.d
            double Vs = 35;//U轴和C轴的线速度模
            double Vu = 20;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
            int a = Convert.ToInt16(D / 0.1);
            double[] X = new double[a + 1];
            double[] Z = new double[a + 1];  //原点变为转动点后的Z值
            double[] Z1 = new double[a + 1];//函数曲线Z值
            double[] Z2 = new double[a + 1];//曲线的一阶导数
            double[] B = new double[a + 1];//B转动角度
            double[] Vc = new double[a + 1];//C线速度
            double[] Nc = new double[a + 1];//C轴转速
            double[] F = new double[a + 1]; //X轴进给速度
            double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
            double[] T = new double[a + 1];  //两点间时间
            double[] X2 = new double[a + 1];//x移动坐标
            double[] r0 = new double[a + 1];//点到转动中心的夹角
            //  double[] beta1 = new double[a];


            // ArrayList ArrayList_X = new ArrayList();
            //ArrayList ArrayList_B = new ArrayList();
            // ArrayList ArrayList_F = new ArrayList();
            // ArrayList ArrayList_C = new ArrayList();
            // ArrayList ArrayList_Z1 = new ArrayList();
            // ArrayList ArrayList_Z2 = new ArrayList();

            //arry_X[0]=-D/2+0.1;
            for (int i = 0; i < a + 1; i++)//x坐标
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                X[i] = -D / 2 + i * 0.1;
            }

            //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

            for (int i = 0; i < a + 1; i++)//B角度，
            {
                //ArrayList_X.Add(-D / 2 + i * 0.001);
                //double[] values = ArrayList_X.Cast<double>().ToArray();
                //double[] d = Convert.ToDouble(X[i]);
                Z1[i] = symbol * (Math.Pow(X[i], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
                Z[i] = H + Z1[i] - h;// -Z1[a - 1];
                if (X[i] > 0)
                {
                    Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                    B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                }
                else if (X[i] == 0)
                {
                    Z2[i] = 0;
                    B[i] = 0;
                }
                else
                {
                    Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                    // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                    B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                }

            }

            for (int i = 0; i < a + 1; i++)//x移动
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                // X[i] = -D / 2 + i * 0.001;

                r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

                //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
                X2[i] = -r0[i] * Math.Cos(Math.PI / 2 + Math.Asin(X[i] / r0[i]) + Math.Atan(Z2[i]));//x移动坐标
                //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
                // dX=(r0*cos(dangle));
            }


            /*
                        Vs=35;%
                        Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                       x=abs(X_1);
                       Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                        Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                        %ff= Wc>500;%找出设定值大于500极限的数点
                       %Wc(ff)=500;
                       Nc=(30.*Wc)/pi;
                       ff= Nc>180;%找出设定值大于300极限的数点
                        Nc(ff)=180;
                        Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
             */
            for (int i = 0; i < a + 1; i++)//C转速
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Nc[i] = Math.Abs(Vc[i] / X[i]) * 30 / Math.PI * 7.70988 * 20;

            }

            for (int i = 0; i < a + 1; i++)//F1进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F1[i] = Math.Abs(dist * Vc[i] / (0.35 * 2 * Math.PI * (2 * X[i] + dist)));
                T[i] = dist / F1[i];

            }

            for (int i = 0; i < a; i++)//F进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F[i] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);


            }
            F[a] = F[a - 1];
            for (int i = 0; i < a + 1; i++)//加工时间
            {
                t = T[i] + t;

            }
            for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
            {
                if (Nc[i] > vc * 30 / Math.PI * 7.70988 * 20)
                {
                    Nc[i] = vc * 30 / Math.PI * 7.70988 * 20;
                }
            }


            /***************写入TXT********/

            string result1 = @"C:\result1.txt";//结果保存到F:\result1.txt

            /*先清空result1.txt文件内容*/
            FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();


            FileStream fs = new FileStream(result1, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs);
            wr.WriteLine("OPEN PROG 2");
            wr.WriteLine("CLEAR");
            wr.WriteLine("G90 G01");
            wr.WriteLine("WHILE(P25<=" + Convert.ToString(n) + ")");
            for (int i = 0; i < a + 1; i++)
            {
                wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            }
            // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            // 
            // }          
            wr.WriteLine("ENDWHILE");
            if (n % 2 == 1)
            {
                for (int i = 0; i < a + 1; i++)
                {
                    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
                }

            }
            wr.WriteLine("P21=1");
            wr.WriteLine("CLOSE");
            wr.Close();
            //test = Z2[2];



            /***************写入TXTZ1********/

            string result2 = @"C:\result2.txt";//结果保存到F:\result1.txt

            /*先清空result1.txt文件内容*/
            FileStream stream3 = File.Open(result2, FileMode.OpenOrCreate, FileAccess.Write);
            stream3.Seek(0, SeekOrigin.Begin);
            stream3.SetLength(0); //清空txt文件
            stream3.Close();


            FileStream fs1 = new FileStream(result2, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr1 = null;

            wr1 = new StreamWriter(fs1);

            for (int i = 0; i < a + 1; i++)
            {
                wr1.WriteLine(Convert.ToString("X" + X[i].ToString("f4") + "  " + "Z1" + Z1[i].ToString("f4") + "  " + "Z2" + Z2[i].ToString("f8") + "  " + "\n"));
            }
            wr1.Close();
        }
       public double[,] asphere(double constan_vc,double contan_F,bool vc_flag,bool F_flag,double dist,double symbol, double n, double vc, double H, double D, double R, double K, double[] A, out double t)
        {                        //,n:抛光次数，vc:工件最大转速，H:模仁到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间
             double h = -33.78;//h = 9.66,
            t = 0;
           // dist = 0.01;
            double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
            double Vs = Vu+5;//U轴和C轴的线速度模
             
            int a = Convert.ToInt16(D/dist);
            double[] X = new double[a + 1];
            double[] Z = new double[a+1];  //原点变为转动点后的Z值
            double[] Z1 = new double[a + 1];//函数曲线Z值
            double[] Z2 = new double[a+1];//曲线的一阶导数
            double[] B = new double[a+1];//B转动角度
            double[] Vc = new double[a+1];//C线速度
            double[] Nc = new double[a+1];//C轴转速
            double[] F = new double[a+1]; //X轴进给速度
            double[] F1 = new double[a+1]; //dist距离走一圈的进给速度
            double[] T = new double[a+1];  //两点间时间
            double[] X2 = new double[a+1];//x移动坐标
            double[] r0 = new double[a+1];//点到转动中心的夹角
          //  double[] beta1 = new double[a];


            // ArrayList ArrayList_X = new ArrayList();
            //ArrayList ArrayList_B = new ArrayList();
           // ArrayList ArrayList_F = new ArrayList();
           // ArrayList ArrayList_C = new ArrayList();
           // ArrayList ArrayList_Z1 = new ArrayList();
           // ArrayList ArrayList_Z2 = new ArrayList();

            //arry_X[0]=-D/2+0.1;
            for (int i = 0; i <a+1; i++)//x坐标
            {
               // ArrayList_X.Add(-D/2+i*0.001);
                X[i] = -D / 2 + i * dist;
            }
            
          //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

            for (int i = 0; i < a + 1; i++)//B角度，
            {
                //ArrayList_X.Add(-D / 2 + i * 0.001);
                //double[] values = ArrayList_X.Cast<double>().ToArray();
                //double[] d = Convert.ToDouble(X[i]);
                Z1[i] = symbol * (Math.Pow(X[i], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
                Z[i] = H + Z1[i] - h;// -Z1[a - 1];
                if (X[i] > 0)
                {
                    Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                    B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                }
                else if (X[i] == 0)
                {
                    Z2[i] = 0;
                    B[i] = 0;
                }
                else
                {
                    Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                    // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                    B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                }

            }

            for (int i = 0; i < a+1; i++)//x移动
            {
                // ArrayList_X.Add(-D/2+i*0.001);
               // X[i] = -D / 2 + i * 0.001;
                
                r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

                //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
                X2[i] = -r0[i] * Math.Cos(Math.PI/2+ Math.Asin(X[i] / r0[i]) +Math.Atan(Z2[i]));//x移动坐标
              //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
              // dX=(r0*cos(dangle));
            }


/*
            Vs=35;%
            Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
           x=abs(X_1);
           Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
            Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
            %ff= Wc>500;%找出设定值大于500极限的数点
           %Wc(ff)=500;
           Nc=(30.*Wc)/pi;
           ff= Nc>180;%找出设定值大于300极限的数点
            Nc(ff)=180;
            Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
 */
            for (int i = 0; i < a+1; i++)//C转速
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                 Vc[i] =Math.Sqrt(Math.Abs( - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI  *150.469-143.259;

                if(vc_flag==true)
                    Nc[i] = constan_vc * 153.1862 + 26.5573;
            }

            for (int i = 0; i < a+1; i++)//F1进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
                F1[i] = Math.Abs(0.1 * Vc[i] / (3*Math.PI * ( X[i] + dist/2)));
                T[i] = dist / F1[i];

            }
            F[0] = 100;
            for (int i = 0; i < a; i++)//F进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F[i+1] =Math.Abs( (X2[i + 1] - X2[i]) / T[i]*60);
                if (F_flag == true)
                    F[i] = contan_F;
               

            }
          //  F[a] = F[a - 1];
            for (int i = 0; i < a+1; i++)//加工时间
            {
                t = T[i] + t;

            }
            t = t / 60;
            for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
            {
                if (Nc[i] > vc * 153.1862 + 26.5573)
                {
                    Nc[i] = vc * 153.1862 + 26.5573;
                }
            }

            
            /***************写入TXT********/

            string result1 = @"C:\result1.txt";//结果保存到F:\result1.txt

            /*先清空result1.txt文件内容*/
            FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();


            FileStream fs = new FileStream(result1, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs);
            wr.WriteLine("OPEN PROG 2");
            wr.WriteLine("CLEAR");
            wr.WriteLine("G90 G01");
            wr.WriteLine("WHILE(P25<="+Convert.ToString(n)+")");
            for (int i = 0; i < a+1; i++)
            {
                wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            }
           // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
          // 
           // }          
          wr.WriteLine("ENDWHILE");
          if (n % 2 == 1)
          {
              for (int i = 0; i < a + 1; i++)
              {
                  wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
              }

          }
            wr.WriteLine("P21=1");
            wr.WriteLine("CLOSE");
            wr.Close();
            //test = Z2[2];

            
            /***************写入TXT********/
            /*SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "";
            sfd.InitialDirectory = @".\模仁加工代码\";
            sfd.Filter = "文本文件| *.txt";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
             * 
                return null;
            }

           // string result1 = @".\GCode.txt";//结果保存到F:\result1.txt

            //*先清空result1.txt文件内容
            FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();


            FileStream fs = new FileStream(path, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs);
            wr.WriteLine("OPEN PROG 2");
            wr.WriteLine("CLEAR");
            wr.WriteLine("G90 G01");
            wr.WriteLine("WHILE(P25<="+Convert.ToString(n)+")");
            for (int i = 0; i < a+1; i++)
            {
                wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            }
           // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
          // 
           // }    
      
          wr.WriteLine("ENDWHILE");
          if (n % 2 == 1)
          {
              for (int i = 0; i < a + 1; i++)
              {
                  wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
              }

          }
            wr.WriteLine("P21=1");
            wr.WriteLine("CLOSE");
            wr.Close();*/
            double[,] b = new double[a + 1,5];
            for (int i = 0; i < a + 1; i++)
            {
                b[i, 0] = X2[i];
                b[i, 1] =B[i];

                b[i, 2] =F[i];               
                b[i, 3] = Nc[i];

                b[i, 4] = X[i];


            }
            return b;
            //test = Z2[2];
        }
        /************x_z矢量和为进给速度********/
       public double[,] asphere_heitian(double curvature_compensate,double first_positon_feed,double D_workpiece, double Dp, double C_motor_scale_factor, double C_motor_offset, double Lworkpiece_h, double other_h, double SAG, double yuan_r, double ao_tu, double R_P_right, double tool_R, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double R_P_left, double R, double K, double[] A, out double t)
       {                        //,n:抛光次数，vc:工件最大转速，H:模仁底面到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间,D_end:加工范围的另一值直径；fixture_h夹具底面到模具承放面高度，SAG为矢高,Lworkpiece_h为L型件底面到中心孔高度，other_h为L型件底面到夹具底面高度,yuan_r为圆角直径，D_workpiece为模仁柱面直径，Dp为加工口径（图纸）
           //double h = -33.78;//h = 9.66,
           //double h = fixture_h + 12.666 + 7.177 - 46.147; //double h = fixture_h + 12.684 + 7.177 - 46.987;//h = -39.05;//h = -33.78;//h = 9.66,
          // double h = fixture_h - 0.999;// 45.999 - 44.83;
        //   vc_flag = true;//C轴恒速，变速功能不要
           double h =  - Lworkpiece_h + other_h;//B轴旋转中心到模具平面高度
           H = H + ao_tu * SAG;//求模仁底面到面型中心的距离
           t = 0;
            dist = 0.1;//间隔为0.1
           double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
           double C = 1 / R;
           double Vs = Vu + 5;//U轴和C轴的线速度模
           //加工口径左边缘的X坐标
           double X_p = -Dp / 2;

           R_P_left = Math.Truncate(R_P_left * 10) / 10;
           Dp = Math.Truncate(Dp * 10) / 10;
           D_workpiece = Math.Truncate(D_workpiece * 10) / 10;
           
          // if (R_P_left * 10 % 2 == 1)
         //      R_P_left = R_P_left + 0.1;

           if (Dp * 10 % 2 == 1)
               Dp = Dp + 0.1;

           if (D_workpiece * 10 % 2 == 1)
               D_workpiece = D_workpiece - 0.1;

           int c = Convert.ToInt16((R_P_right-R_P_left) / dist);//输出代码行数-1
           int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
           int d = Convert.ToInt16(R_P_left / dist);
           int e = Convert.ToInt16(R_P_right / dist);
           int f = Convert.ToInt16(Dp / dist);
           bool normal_flag = true;
           double[] X = new double[a + 1];
           double[] Z = new double[a + 1];  //原点变为转动点后的Z值
           double[] Z1 = new double[a + 1];//函数曲线Z值
           double[] Z2 = new double[a + 1];//曲线的一阶导数
           double[] ZZ = new double[a + 1];//Z轴移动坐标
           double[] B = new double[a + 1];//B转动角度
           double[] Vc = new double[a + 1];//C线速度
           double[] Nc = new double[a + 1];//C轴转速
           double[] F = new double[a + 1]; //X轴进给速度
           double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
           double[] T = new double[a + 1];  //两点间时间
           double[] X2 = new double[a + 1];//x移动坐标
           double[] r0 = new double[a + 1];//点到转动中心的夹角
           double[] flat_v_1p = new double[] { 27.804, 24.2575, 16.5767, 11.2156, 9.2657, 8.3414, 7.4582, 6.7514, 6.3777, 5.9989, 5.6724, 5.4287, 5.2395, 5.0569, 4.8999, 4.7708, 4.6671, 4.5711, 4.482, 4.3991, 4.3219, 4.2464, 4.1822, 4.1219, 4.0649, 4.011, 3.9599, 3.9113, 3.8651, 3.821, 3.7789, 3.7386, 3.7, 3.6629, 3.6273, 3.5931, 3.5602, 3.5284, 3.4978, 3.4682, 3.4396, 3.4119, 3.3852, 3.3592, 3.3341, 3.3096, 3.2859, 3.2629, 3.2405, 3.2188, 3.1976, 3.1769, 3.1568, 3.1372, 3.1181, 3.0994, 3.0812, 3.0634, 3.0461, 3.0291, 3.0125, 2.9963, 2.9804, 2.9648, 2.9496, 2.9347, 2.9201, 2.9057, 2.8917, 2.8779, 2.8644, 2.8512, 2.8381, 2.8254, 2.8128, 2.8005, 2.7884, 2.7765, 2.7648, 2.7533, 2.742, 2.7309, 2.72, 2.7092, 2.6986, 2.6882, 2.6779, 2.6678, 2.6579, 2.6481, 2.6384, 2.6289, 2.6195, 2.6103, 2.6012, 2.5922, 2.5833, 2.5746, 2.566, 2.5575, 2.5491, 2.5408, 2.5326, 2.5246, 2.5166, 2.5087, 2.501, 2.4933, 2.4857, 2.4782, 2.4709, 2.4635, 2.4563, 2.4492, 2.4422, 2.4352, 2.4283, 2.4215, 2.4148, 2.4081, 2.4015, 2.395, 2.3886, 2.3822, 2.3759, 2.3697, 2.3635, 2.3574, 2.3514, 2.3454, 2.3395, 2.3336, 2.3278, 2.3221, 2.3164, 2.3108, 2.3052, 2.2997, 2.2943, 2.2888, 2.2835, 2.2782, 2.2729, 2.2677, 2.2625, 2.2574, 2.2524, 2.2473, 2.2424, 2.2374, 2.2325, 2.2277, 2.2229, 2.2181, 2.2134, 2.2087, 2.2041, 2.1995, 2.1949, 2.1904, 2.1859, 2.1815, 2.1771, 2.1727, 2.1683, 2.164, 2.1598, 2.1555, 2.1513, 2.1472, 2.143, 2.1389, 2.1348, 2.1308, 2.1268, 2.1228, 2.1188, 2.1149, 2.111, 2.1072, 2.1033, 2.0995, 2.0957, 2.092, 2.0883, 2.0846, 2.0809, 2.0773, 2.0736, 2.07, 2.0665, 2.0629, 2.0594, 2.0559, 2.0524, 2.049, 2.0456, 2.0422, 2.0388, 2.0354, 2.0321, 2.0288, 2.0255, 2.0222, 2.019, 2.0158, 2.0126, 2.0094, 2.0062, 2.0031, 1.9999, 1.9968, 1.9937, 1.9907, 1.9876, 1.9846, 1.9816, 1.9786, 1.9756, 1.9727, 1.9697, 1.9668, 1.9639, 1.961, 1.9582, 1.9553, 1.9525, 1.9497, 1.9469, 1.9441, 1.9413, 1.9386, 1.9358, 1.9331, 1.9304, 1.9277, 1.925, 1.9224, 1.9197, 1.9171, 1.9145, 1.9119, 1.9093, 1.9067, 1.9042, 1.9016, 1.8991, 1.8966, 1.8941, 1.8916, 1.8891, 1.8866, 1.8842, 1.8817, 1.8793, 1.8769, 1.8745, 1.8721, 1.8697, 1.8674, 1.865, 1.8627, 1.8604, 1.858, 1.8557, 1.8534, 1.8512, 1.8489, 1.8466, 1.8444, 1.8422, 1.8399, 1.8377, 1.8355, 1.8333, 1.8311, 1.829, 1.8268, 1.8246, 1.8225, 1.8204, 1.8183, 1.8161, 1.814, 1.812, 1.8099, 1.8078, 1.8057, 1.8037, 1.8016, 1.7996, 1.7976, 1.7956, 1.7936, 1.7916, 1.7896, 1.7876, 1.7856, 1.7837, 1.7817, 1.7798, 1.7778, 1.7759, 1.774, 1.7721, 1.7702, 1.7683, 1.7664, 1.7645, 1.7626, 1.7608 };
           double[] flat_v_3p = new double[] { 40.5259, 40.5141, 32.9916, 23.4416, 18.0608, 14.41, 12.4473, 11.0399, 9.7753, 8.9723, 8.2413, 7.68, 7.3041, 6.9754, 6.6848, 6.4257, 6.1929, 5.9818, 5.7891, 5.6112, 5.4335, 5.2944, 5.1922, 5.0945, 5.001, 4.9113, 4.8249, 4.7487, 4.6754, 4.6049, 4.5371, 4.4723, 4.4103, 4.3511, 4.2949, 4.2415, 4.1911, 4.1434, 4.0986, 4.0564, 4.0168, 3.9811, 3.9466, 3.9132, 3.8808, 3.8494, 3.8189, 3.7893, 3.7606, 3.7327, 3.7055, 3.6791, 3.6534, 3.6283, 3.6038, 3.58, 3.5568, 3.5341, 3.5119, 3.4903, 3.4692, 3.4485, 3.4283, 3.4085, 3.3892, 3.3702, 3.3517, 3.3335, 3.3157, 3.2982, 3.2811, 3.2644, 3.2479, 3.2317, 3.2159, 3.2003, 3.185, 3.17, 3.1553, 3.1408, 3.1265, 3.1125, 3.0987, 3.0852, 3.0718, 3.0587, 3.0458, 3.0331, 3.0206, 3.0083, 2.9962, 2.9842, 2.9725, 2.9609, 2.9494, 2.9382, 2.9271, 2.9161, 2.9053, 2.8947, 2.8842, 2.8739, 2.8636, 2.8536, 2.8436, 2.8338, 2.8241, 2.8145, 2.8051, 2.7957, 2.7865, 2.7774, 2.7684, 2.7596, 2.7508, 2.7421, 2.7335, 2.7251, 2.7167, 2.7084, 2.7002, 2.6922, 2.6842, 2.6762, 2.6684, 2.6607, 2.653, 2.6455, 2.638, 2.6306, 2.6232, 2.616, 2.6088, 2.6017, 2.5946, 2.5877, 2.5808, 2.574, 2.5672, 2.5605, 2.5539, 2.5473, 2.5408, 2.5344, 2.528, 2.5217, 2.5154, 2.5092, 2.5031, 2.497, 2.491, 2.485, 2.4791, 2.4732, 2.4674, 2.4616, 2.4559, 2.4502, 2.4446, 2.4391, 2.4335, 2.4281, 2.4226, 2.4172, 2.4119, 2.4066, 2.4014, 2.3962, 2.391, 2.3859, 2.3808, 2.3757, 2.3707, 2.3658, 2.3608, 2.356, 2.3511, 2.3463, 2.3415, 2.3368, 2.3321, 2.3274, 2.3228, 2.3182, 2.3136, 2.3091, 2.3046, 2.3001, 2.2957, 2.2913, 2.287, 2.2826, 2.2783, 2.274, 2.2698, 2.2656, 2.2614, 2.2572, 2.2531, 2.249, 2.2449, 2.2409, 2.2369, 2.2329, 2.2289, 2.225, 2.2211, 2.2172, 2.2133, 2.2095, 2.2057, 2.2019, 2.1981, 2.1944, 2.1907, 2.187, 2.1833, 2.1797, 2.1761, 2.1723, 2.168, 2.163, 2.1585, 2.155, 2.1515, 2.148, 2.1446, 2.1412, 2.1378, 2.1344, 2.131, 2.1277, 2.1244, 2.121, 2.1178, 2.1145, 2.1112, 2.108, 2.1048, 2.1016, 2.0984, 2.0953, 2.0921, 2.089, 2.0859, 2.0828, 2.0798, 2.0767, 2.0737, 2.0707, 2.0677, 2.0647, 2.0617, 2.0587, 2.0558, 2.0529, 2.05, 2.0471, 2.0442, 2.0414, 2.0385, 2.0357, 2.0329, 2.0301, 2.0273, 2.0245, 2.0217, 2.019, 2.0163, 2.0136, 2.0108, 2.0082, 2.0055, 2.0028, 2.0002, 1.9975, 1.9949, 1.9923, 1.9897, 1.9871, 1.9845, 1.982, 1.9794, 1.9769, 1.9744, 1.9719, 1.9694, 1.9669, 1.9644, 1.962, 1.9595, 1.9571, 1.9546, 1.9522, 1.9498, 1.9474, 1.945, 1.9427, 1.9403, 1.9379, 1.9356, 1.9333, 1.9309, 1.9286, 1.9263, 1.9241, 1.9218, 1.9195, 1.9172, 1.915, 1.9128 };
           double[] flat_v_5p = new double[] { 45.201, 44.14, 39.91,31.5091, 25.0356, 20.3111, 16.5255, 14.4076, 12.827, 11.6089, 10.5568, 9.6852, 9.0155, 8.4652, 8.005, 7.6123, 7.2689, 6.9609, 6.678, 6.4284, 6.2069, 6.0652, 5.9219, 5.7785, 5.6365, 5.4985, 5.3666, 5.2474, 5.1562, 5.0698, 4.9876, 4.917, 4.8496, 4.7852, 4.7235, 4.6643, 4.6076, 4.553, 4.5005, 4.45, 4.4013, 4.3543, 4.3089, 4.2651, 4.2227, 4.1816, 4.1419, 4.1033, 4.0659, 4.0297, 3.9944, 3.9602, 3.9269, 3.8945, 3.863, 3.8323, 3.8024, 3.7733, 3.7448, 3.7171, 3.6901, 3.6636, 3.6378, 3.6126, 3.588, 3.5639, 3.5403, 3.5172, 3.4947, 3.4726, 3.4509, 3.4297, 3.4089, 3.3885, 3.3685, 3.3489, 3.3297, 3.3108, 3.2923, 3.2741, 3.2563, 3.2388, 3.2215, 3.2046, 3.188, 3.1716, 3.1555, 3.1397, 3.1241, 3.1088, 3.0938, 3.0789, 3.0643, 3.05, 3.0358, 3.0219, 3.0082, 2.9947, 2.9813, 2.9682, 2.9553, 2.9425, 2.9299, 2.9175, 2.9053, 2.8932, 2.8813, 2.8696, 2.858, 2.8466, 2.8353, 2.8242, 2.8132, 2.8024, 2.7916, 2.7811, 2.7706, 2.7603, 2.7501, 2.74, 2.7301, 2.7203, 2.7105, 2.7009, 2.6914, 2.6821, 2.6728, 2.6636, 2.6546, 2.6456, 2.6367, 2.628, 2.6193, 2.6107, 2.6022, 2.5938, 2.5855, 2.5773, 2.5692, 2.5611, 2.5531, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053 };
           double[] flat_v_vertical = new double[] { 29.474, 28.685, 27.348, 25.973, 17.534, 12.548, 11.556, 10.321, 8.903, 8.097, 7.608, 6.85, 6.639, 6.087, 5.697, 5.493, 5.426, 5.249, 5.031, 4.889, 4.774, 4.742, 4.563, 4.622, 4.32, 4.326, 4.269, 4.196, 4.27, 4.117, 4.073, 4.027, 3.912, 3.875, 3.945, 3.868, 3.828, 3.721, 3.806, 3.77, 3.741, 3.621, 3.754, 3.592, 3.563, 3.629, 3.495, 3.533, 3.535, 3.557, 3.358, 3.403, 3.375, 3.466, 3.357, 3.388, 3.319, 3.256, 3.277, 3.136, 3.174, 3.136, 3.151, 3.13, 3.103, 3.096, 3.116, 3.031, 3.022, 2.958, 3.014, 3.017, 3.066, 3.076, 3.067, 3.057, 3.047, 3.037, 3.027, 3.017, 3.007, 2.997, 2.987, 2.977, 2.967, 2.957, 2.947, 2.937, 2.927, 2.917, 2.907, 2.897, 2.887, 2.877, 2.867, 2.857, 2.847, 2.837, 2.827, 2.817, 2.807, 2.797, 2.787, 2.777, 2.767, 2.757, 2.747, 2.737, 2.727, 2.717, 2.707, 2.697, 2.687, 2.677, 2.667, 2.657, 2.647, 2.637, 2.627, 2.617, 2.607, 2.597, 2.587, 2.577, 2.567, 2.557, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053, 1.8027, 1.8002, 1.7976, 1.7951, 1.7925, 1.79, 1.7874, 1.7849, 1.7823, 1.7798, 1.7772, 1.7747, 1.7721, 1.7696, 1.767 };
           double[] C_AXI_feed = new double[] {100,99.225, 98.45, 97.675, 96.9, 96.125, 95.35, 94.575, 93.8, 93.025, 92.25, 91.475, 90.7, 89.925, 89.15, 88.375, 87.6, 86.825, 86.05, 85.275, 84.5, 83.725, 82.95, 82.175, 81.4, 80.625, 79.85, 79.075, 78.3, 77.525, 76.75, 75.975, 75.2, 74.425, 73.65, 72.875, 72.1, 71.325, 70.55, 69.775, 69, 68.225, 67.45, 66.675, 65.9, 65.125, 64.35, 63.575, 62.8, 62.025, 61.25, 60.475, 59.7, 58.925, 58.15, 57.375, 56.6, 55.825, 55.05, 54.275, 53.5, 52.725, 51.95, 51.175, 50.4, 49.625, 48.85, 48.075, 47.3, 46.525, 45.75, 44.975, 44.2, 43.425, 42.65, 41.875, 41.1, 40.325, 39.55, 38.775, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38 };
           double[] Change_C_V = new double[] {40.8000000000000,45.9855427242774, 33.4331484319391, 26.0030276405464, 18.736134626099, 15.8024085906988, 13.0702209169282, 11.9499264602815, 10.3150258356041, 9.58768693747091, 8.67617842146954, 8.09723635547309, 7.63214876487789, 7.25634432375164, 6.9162339137697, 6.59757888369261, 6.33693810801177, 6.06706694129412, 5.85725220521252, 5.62995984517347, 5.43356495019632, 5.26525471167813, 5.14307514798742, 5.02090135021251, 4.91448514875669, 4.80955554921228, 4.70828331235783, 4.62620442705041, 4.5411087018003, 4.46898688458629, 4.39484898808562, 4.33028096128562, 4.26907786950641, 4.20967728148731, 4.16328091714979, 4.10708755116163, 4.07702838380467, 4.02304918386251, 4.00861583418726, 3.95939485263124, 3.95461538253673, 3.92114737755821, 3.91129062697994, 3.91090280668656, 3.8658051229087, 3.93327788261413, 3.80755067507808, 4.00192262170292, 3.75484171394592, 4.07451611531537, 3.79235200212099, 3.80559091845718, 3.72035873200004, 3.63437474423236, 3.61318026674884, 3.50503813682881, 3.49628568037397, 3.3973256462859, 3.38196620542492, 3.30024331148086, 3.27653787708628, 3.20860131466308, 3.18117091414261, 3.12108165882688, 3.09414208786855, 3.03966039504838, 3.00890891511929, 2.9676405683572, 2.92984545272857, 2.89628323415507, 2.86022961181019, 2.82802250144546, 2.79582336552027, 2.76365759608204, 2.7352764257447, 2.70360016487247, 2.67787757316105, 2.64763009216716, 2.62332922369138, 2.59557879497467, 2.57114431407932, 2.54701870029433, 2.5213430434979, 2.50157743314829, 2.47410361369467, 2.45819892964401, 2.4304099502574, 2.41429230305731, 2.39149124713729, 2.37244683395485, 2.35299855325925, 2.33446512461261, 2.31577800474094, 2.29868462897768, 2.28020088683996, 2.26453050602613, 2.24652450695153, 2.23176689209113, 2.21453103977845, 2.20030672909179, 2.18434807908685, 2.16996118247832, 2.15575034532082, 2.14062758286191, 2.12862897427082, 2.11245479786732, 2.10247556976492, 2.08598929266652, 2.07588202404427, 2.06197105076723, 2.05020569912653, 2.03809455723667, 2.02655814117865, 2.01486432218885, 2.00402866484988, 1.99238982613546, 1.98225980069676, 1.97088884006614, 1.9612785846767, 1.95024048866297, 1.94085840418306, 1.93059111976728, 1.92108791855767, 1.91169908895673, 1.90174416238152, 1.89368621406858, 1.88306460975461, 1.87616799711793, 1.86537822704559, 1.85832523278637, 1.84899238977517, 1.84099686410825, 1.83270538059451, 1.82480323551155, 1.81667611068034, 1.80921899818495, 1.80120028890246, 1.79412493579527, 1.78618666796367, 1.77938524884478, 1.77172602487216, 1.76502376998502, 1.75775683377567, 1.75100954231475, 1.74433846950974, 1.73727147709734, 1.73134955959559, 1.72387179712552, 1.7187124035887, 1.71108989434116, 1.70584601173821, 1.69914499613454, 1.69327725893014, 1.68719741685127, 1.6814006363154, 1.67543707969286, 1.66988973500643, 1.6639642318326, 1.65867179475393, 1.65288022253637, 1.64770655624926, 1.64206563765574, 1.6369660821127, 1.63152867877953, 1.62643205506072, 1.62140014382655, 1.61610526811345, 1.61157938999938, 1.6059620507689, 1.60190291568412, 1.59621242098574, 1.59203207143284, 1.58698546148117, 1.58244513638199, 1.57779449924226, 1.57327456252327, 1.56876173848801, 1.56431271031066, 1.55987627289645, 1.55559585415756, 1.55120547124655, 1.54706001403344, 1.54266072275847, 1.53879142970964, 1.53421059758352, 1.53074574295954, 1.52589875461067, 1.52292461503748, 1.51767357494403, 1.51534199546183, 1.5096887665165, 1.50779644795731, 1.5019401492524, 1.50011436671094, 1.49495368537609, 1.49129693533527, 1.48999617278133, 1.48043688661795, 1.48754736762012, 1.46734834504067, 1.48762443270688, 1.45273745080896, 1.48861963335207, 1.43980017412823, 1.48477505149489, 1.43807206293338, 1.45852289391056, 1.4749358174079, 1.48501121064645, 1.48085093613835, 1.46977890787663, 1.42488289394389, 1.43980017412823, 1.48477505149489, 1.43807206293338, 1.45852289391056, 1.4749358174079, 1.48501121064645, 1.48085093613835, 1.36977890787663, 1.32488289394389};
           //  double[] beta1 = new double[a];


           // ArrayList ArrayList_X = new ArrayList();
           //ArrayList ArrayList_B = new ArrayList();
           // ArrayList ArrayList_F = new ArrayList();
           // ArrayList ArrayList_C = new ArrayList();
           // ArrayList ArrayList_Z1 = new ArrayList();
           // ArrayList ArrayList_Z2 = new ArrayList();

           //arry_X[0]=-D/2+0.1;
           for (int i = 0; i < a + 1; i++)//x坐标
           {
              // ArrayList_X.Add(-D/2+i*0.001);
               X[i] = -D_workpiece / 2 + i * dist;
           }
      //double Z11 = -symbol * (Math.Pow(X[2], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[2], 2))) + A[0] * Math.Abs(X[2]) + A[1] * Math.Pow((X[2]), 2) + A[2] * Math.Pow(Math.Abs(X[2]), 3) + A[3] * Math.Pow((X[2]), 4) + A[4] * Math.Pow(Math.Abs(X[2]), 5) + A[5] * Math.Pow((X[1]), 6) + A[6] * Math.Pow(Math.Abs(X[2]), 7) + A[7] * Math.Pow((X[2]), 8) + A[8] * Math.Pow(Math.Abs(X[2]), 9) + A[9] * Math.Pow((X[2]), 10) + A[10] * Math.Pow(Math.Abs(X[2]), 11) + A[11] * Math.Pow((X[2]), 12) + A[12] * Math.Pow(Math.Abs(X[2]), 13) + A[13] * Math.Pow((X[2]), 14) + A[14] * Math.Pow(Math.Abs(X[2]), 15) + A[15] * Math.Pow((X[2]), 16) + A[16] * Math.Pow(Math.Abs(X[2]), 17) + A[17] * Math.Pow((X[2]), 18) + A[18] * Math.Pow(Math.Abs(X[2]), 19) + A[19] * Math.Pow((X[2]), 20));
           double Z11 = symbol * (Math.Pow(X[f / 2 + a / 2], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[f / 2 + a / 2], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[f / 2 + a / 2]) + A[1] * Math.Pow((X[f / 2 + a / 2]), 2) + A[2] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 3) + A[3] * Math.Pow((X[f / 2 + a / 2]), 4) + A[4] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 5) + A[5] * Math.Pow((X[f / 2 + a / 2]), 6) + A[6] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 7) + A[7] * Math.Pow((X[f / 2 + a / 2]), 8) + A[8] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 9) + A[9] * Math.Pow((X[f / 2 + a / 2]), 10) + A[10] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 11) + A[11] * Math.Pow((X[f / 2 + a / 2]), 12) + A[12] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 13) + A[13] * Math.Pow((X[f / 2 + a / 2]), 14) + A[14] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 15) + A[15] * Math.Pow((X[f / 2 + a / 2]), 16) + A[16] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 17) + A[17] * Math.Pow((X[f / 2 + a / 2]), 18) + A[18] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 19) + A[19] * Math.Pow((X[f / 2 + a / 2]), 20));
               
                    
           if (Z11 > 0&&ao_tu>0)
          {
              H = H -2*ao_tu * SAG;
          }
          if (Z11 < 0 && ao_tu < 0)
          {

              H = H - 2 * ao_tu * SAG;

          }
           //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));


           //加工口径左边缘B角度
          double tan_B_p;
           if(X_p>=0)
           tan_B_p = symbol * ((2 * C * X_p * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + Math.Pow(X_p, 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X_p + A[2] * 3 * Math.Pow(X_p, 2) + A[3] * 4 * Math.Pow(X_p, 3) + A[4] * 5 * Math.Pow(X_p, 4) + A[5] * 6 * Math.Pow(X_p, 5) + A[6] * 7 * Math.Pow(X_p, 6) + A[7] * 8 * Math.Pow(X_p, 7) + A[8] * 9 * Math.Pow(X_p, 8) + A[9] * 10 * Math.Pow(X_p, 9) + A[10] * 11 * Math.Pow(X_p, 10) + A[11] * 12 * Math.Pow(X_p, 11) + A[12] * 13 * Math.Pow(X_p, 12) + A[13] * 14 * Math.Pow(X_p, 13) + A[14] * 15 * Math.Pow(X_p, 14) + A[15] * 16 * Math.Pow(X_p, 15) + A[16] * 17 * Math.Pow(X_p, 16) + A[17] * 18 * Math.Pow(X_p, 17) + A[18] * 19 * Math.Pow(X_p, 18) + A[19] * 20 * Math.Pow(X_p, 19));//正切值
           else
               tan_B_p = symbol * ((2 * C * X_p * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + Math.Pow(X_p, 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))), 2) - A[0] + A[1] * 2 * X_p - A[2] * 3 * Math.Pow(X_p, 2) + A[3] * 4 * Math.Pow(X_p, 3) - A[4] * 5 * Math.Pow(X_p, 4) + A[5] * 6 * Math.Pow(X_p, 5) - A[6] * 7 * Math.Pow(X_p, 6) + A[7] * 8 * Math.Pow(X_p, 7) - A[8] * 9 * Math.Pow(X_p, 8) + A[9] * 10 * Math.Pow(X_p, 9) - A[10] * 11 * Math.Pow(X_p, 10) + A[11] * 12 * Math.Pow(X_p, 11) - A[12] * 13 * Math.Pow(X_p, 12) + A[13] * 14 * Math.Pow(X_p, 13) - A[14] * 15 * Math.Pow(X_p, 14) + A[15] * 16 * Math.Pow(X_p, 15) - A[16] * 17 * Math.Pow(X_p, 16) + A[17] * 18 * Math.Pow(X_p, 17) - A[18] * 19 * Math.Pow(X_p, 18) + A[19] * 20 * Math.Pow(X_p, 19));//正切值
      

           double B_p = Math.Atan(tan_B_p) * 180 / Math.PI;  //B角度

          //加工口径左边缘Z坐标
          double Z_p = symbol * (Math.Pow(X_p, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X_p) + A[1] * Math.Pow((X_p), 2) + A[2] * Math.Pow(Math.Abs(X_p), 3) + A[3] * Math.Pow((X_p), 4) + A[4] * Math.Pow(Math.Abs(X_p), 5) + A[5] * Math.Pow((X_p), 6) + A[6] * Math.Pow(Math.Abs(X_p), 7) + A[7] * Math.Pow((X_p), 8) + A[8] * Math.Pow(Math.Abs(X_p), 9) + A[9] * Math.Pow((X_p), 10) + A[10] * Math.Pow(Math.Abs(X_p), 11) + A[11] * Math.Pow((X_p), 12) + A[12] * Math.Pow(Math.Abs(X_p), 13) + A[13] * Math.Pow((X_p), 14) + A[14] * Math.Pow(Math.Abs(X_p), 15) + A[15] * Math.Pow((X_p), 16) + A[16] * Math.Pow(Math.Abs(X_p), 17) + A[17] * Math.Pow((X_p), 18) + A[18] * Math.Pow(Math.Abs(X_p), 19) + A[19] * Math.Pow((X_p), 20));


          //左圆角圆心X坐标
          double X_circleCenter = -Math.Abs(yuan_r * Math.Sin(B_p/180*Math.PI)) + X_p;
         // double tempP = Math.Asin((X_p - X_circleCenter) / yuan_r) * 180 / Math.PI;
           
           
           //左圆角圆心Z坐标
          double Z_circleCenter = B_p/Math.Abs(B_p)*yuan_r * Math.Cos(B_p) + Z_p;


         //左圆角圆心左边取一位小数位
           double R_circleCenter=Math.Truncate(Math.Abs(X_circleCenter)*10)/10;

          for (int i = 0; i < a + 1; i++)//B角度，
          {

          
               //ArrayList_X.Add(-D / 2 + i * 0.001);
               //double[] values = ArrayList_X.Cast<double>().ToArray();
               //double[] d = Convert.ToDouble(X[i]);

               if (i < (D_workpiece / 2 - R_circleCenter)/dist-1)//平面左边部分
               {
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                   B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }
               if (i >= ((D_workpiece / 2 - R_circleCenter) / dist)-1 && i < (D_workpiece / 2 - Dp/2) / dist)//左边圆角部分
               {
                  // Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                   B[i] = B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (a / 2 - i) * dist) / yuan_r) * 180 / Math.PI;
                   if (B[i] * B_p < 0)
                       B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }
               if (i >=Convert.ToInt16((D_workpiece / 2 - Dp / 2) / dist) && i <= Convert.ToInt16((D_workpiece / 2 + Dp / 2)/dist))
              {

                  Z1[i] = symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
                //  Z1[i] = symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) - A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) - A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) - A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) - A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) - A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));

                  if (double.IsNaN(Z1[i]))
                  {
                      Z1[i] = Z1[i - 2];
                      //if()
                //      MessageBox.Show("异常Z1N");
                      normal_flag = false;
                  }
                  if (double.IsInfinity(Z1[i]))
                  {
                      Z1[i] = Z1[i - 1] + Z1[i - 1] - Z1[i - 2];
                      //if()
                   //   MessageBox.Show("异常Z1I");
                      normal_flag = false;
                  }

                  Z[i] = H + Z1[i] + h;// -Z1[a - 1];
                  /* 
                   * 
                   * 
                   * 
                   * 
                   * 
                   * if (X[i] > 0)
                   {
                       //Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                       Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3)  / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * X[i] * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                  
                       B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                   }
              
                   else
                   {
                      // Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                       Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + C * Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * X[i] * Math.Pow(C, 2)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                  
                       // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                       B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                   }
                   */
               //   Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                 if(X[i]<=0)
                  Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                 else
                    Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
           
                   //  Z2[i] = symbol * ((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) * C / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) / Math.Pow(1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)), 2) * Math.Pow(C, 2)*(K+1) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                  B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                  if (i == a / 2)
                  {
                      //MessageBox.Show("异常B");
                      Z2[i] = 0;
                      B[i] = 0;
                  }
             
             
              }
          

               if (i > (D_workpiece / 2 + Dp / 2) / dist && i <= (D_workpiece / 2+R_circleCenter )/dist+1)//右边圆角部分
               {
                  

                 //  Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));                
                   B[i] = -B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (i - a / 2) * dist) / yuan_r) * 180 / Math.PI;
                   if (B[i] * B_p > 0)
                       B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
                   
               }

               if (i >(D_workpiece / 2 + R_circleCenter) / dist+1)//右边平面部分
               {
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                   B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }



              
          //
         //                                                 
         //                                     
         // if(i>(D_workpiece / 2 + Dp / 2)/dist)
                  
               

           }

           double r0max = 0, r0min = 100, tempz = 0;
           for (int i = 0; i < a + 1; i++)//x移动
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               // X[i] = -D / 2 + i * 0.001;

               r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径
               
               //ZZ[i] = r0[i] * Math.Abs(Math.Cos(Math.Abs(B[i]) * Math.PI / 180 + Math.Abs(Math.Asin(X[i] / r0[i]))));//Z轴转动中心
               //MessageBox.Show(X[i].ToString());
               tempz = r0[i] * Math.Abs(Math.Sin(B[i] * Math.PI / 180));
               if (tempz > r0max)
                  r0max = tempz;
               if (tempz < r0min)
                   r0min = tempz;

            
               //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
               //X2[i] = -r0[i] * Math.Cos(Math.PI / 2 + Math.Asin(X[i] / r0[i]) + Math.Atan(Z2[i]));//x移动坐标
               X2[i] = r0[i] * Math.Cos(Math.Acos(X[i] / r0[i]) - B[i] * Math.PI / 180);//x移动坐标

               if (double.IsNaN(X2[i]))
               {
                   X2[i] = X2[i - 1];
                   //if()
                   //MessageBox.Show("异常X2N");
                   normal_flag = false;
               }
               if (double.IsInfinity(X2[i]))
               {
                   X2[i] = X2[i - 1];
                   //if()
                  // MessageBox.Show("异常X2I");
                   normal_flag = false;
               }
               
               //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
               // dX=(r0*cos(dangle));

               ZZ[i] = Math.Sqrt(Math.Pow(r0[i], 2) - Math.Pow(X2[i], 2));
           }


           /*
                       Vs=35;%
                       Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                      x=abs(X_1);
                      Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                       Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                       %ff= Wc>500;%找出设定值大于500极限的数点
                      %Wc(ff)=500;
                      Nc=(30.*Wc)/pi;
                      ff= Nc>180;%找出设定值大于300极限的数点
                       Nc(ff)=180;
                       Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
            */
           for (int i = 0; i < a + 1; i++)//C转速
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * C_motor_scale_factor + C_motor_offset;

               if (vc_flag == true)
                   Nc[i] = constan_vc * C_motor_scale_factor + C_motor_offset;
           }

           for (int i = 0; i < a + 1; i++)//F1进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
               T[i] = dist / F1[i];

           }
           F[0] = 200;
           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F[i + 1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i] = contan_F;


           }
           //  F[a] = F[a - 1];
           for (int i = 0; i < a + 1; i++)//加工时间
           {
               t = T[i] + t;

           }
           t = t / 60;
           for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
           {
               if (Nc[i] > vc * C_motor_scale_factor + C_motor_offset)
               {
                   Nc[i] = vc * C_motor_scale_factor + C_motor_offset;
               }
           }


           /**************根据面型来做曲率补正系数****************/

           /************************读取TXT里面的F************/
           double[] F_store = new double[a / 2 + 1];
           double[] VC_store = new double[a / 2 + 1];//c轴变速
           double curvature_coefficient;
           //string filepath;
           //if(tool_R==1)
           // filepath = System.Environment.CurrentDirectory + "\\Flat_V_1P.txt";
           //else if(tool_R==5)
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_5P.txt";
           //else
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_3P.txt";
          
          
           //StreamReader readfile = new StreamReader(filepath, System.Text.Encoding.UTF8);

         
           for (int i = 0; i <a / 2 + 1; i++)
           {
               //double temp=0;
               //for (int j = 0; j < 100; j++)
               //{
               //    temp = temp + Math.Sqrt(1 + Math.Pow((Math.Pow(i * 0.1 + j * 0.001, 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(i * 0.1 + j * 0.001, 2))) + A[0] * Math.Abs(i * 0.1 + j * 0.001) + A[1] * Math.Pow((i * 0.1 + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 3) + A[3] * Math.Pow((i * 0.1 + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 5) + A[5] * Math.Pow((i * 0.1 + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 7) + A[7] * Math.Pow((i * 0.1 + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 9) + A[9] * Math.Pow((i * 0.1 + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 11) + A[11] * Math.Pow((i * 0.1 + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 13) + A[13] * Math.Pow((i * 0.1 + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 15) + A[15] * Math.Pow((i * 0.1 + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 17) + A[17] * Math.Pow((i * 0.1 + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 19) + A[19] * Math.Pow((i * 0.1 + j * 0.001), 20)), 2)) * 0.001;
              
               //}
               //curvature_coefficient = temp / 0.1;
               //curvature_coefficient = 1;
               double temp = 0, temp2 = 0;
               // for (int j = 0; j < 100; j++)
               // {
               //if(j<10)
               int j = 10;
               temp2 = (Math.Pow(i * dist + j * 0.001, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(i * dist + j * 0.001, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(i * dist + j * 0.001) + A[1] * Math.Pow((i * dist + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * dist + j * 0.001), 3) + A[3] * Math.Pow((i * dist + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * dist + j * 0.001), 5) + A[5] * Math.Pow((i * dist + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * dist + j * 0.001), 7) + A[7] * Math.Pow((i * dist + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * dist + j * 0.001), 9) + A[9] * Math.Pow((i * dist + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * dist + j * 0.001), 11) + A[11] * Math.Pow((i * dist + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * dist + j * 0.001), 13) + A[13] * Math.Pow((i * dist + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * dist + j * 0.001), 15) + A[15] * Math.Pow((i * dist + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * dist + j * 0.001), 17) + A[17] * Math.Pow((i * dist + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * dist + j * 0.001), 19) + A[19] * Math.Pow((i * dist + j * 0.001), 20));
               j = 0;
               temp = (Math.Pow(i * dist + j * 0.001, 2)*C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(i * dist + j * 0.001, 2)*Math.Pow(C, 2))) + A[0] * Math.Abs(i * dist + j * 0.001) + A[1] * Math.Pow((i * dist + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * dist + j * 0.001), 3) + A[3] * Math.Pow((i * dist + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * dist + j * 0.001), 5) + A[5] * Math.Pow((i * dist + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * dist + j * 0.001), 7) + A[7] * Math.Pow((i * dist + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * dist + j * 0.001), 9) + A[9] * Math.Pow((i * dist + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * dist + j * 0.001), 11) + A[11] * Math.Pow((i * dist + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * dist + j * 0.001), 13) + A[13] * Math.Pow((i * dist + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * dist + j * 0.001), 15) + A[15] * Math.Pow((i * dist + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * dist + j * 0.001), 17) + A[17] * Math.Pow((i * dist + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * dist + j * 0.001), 19) + A[19] * Math.Pow((i * dist + j * 0.001), 20));
               temp = Math.Abs(temp - temp2);
               temp = Math.Sqrt(Math.Pow(temp, 2) + Math.Pow(0.01, 2));             
               curvature_coefficient = temp / 0.01;
               if (double.IsInfinity(curvature_coefficient))
                   curvature_coefficient = 1;
               if(double.IsNaN(curvature_coefficient))
                   curvature_coefficient = 1;

               if (curvature_compensate == 1)
                   curvature_coefficient = 1;
               else if (curvature_compensate == 2)
                   curvature_coefficient = 1 / curvature_coefficient;
               else
               { 

               }

               if (i > (Dp / 2) / dist)
               curvature_coefficient = 1;

        //    curvature_coefficient = 1;

               if (tool_R == 1)
                   F_store[i] = flat_v_1p[i] * curvature_coefficient;
               else if (tool_R == 5)
               
                   F_store[i] = flat_v_5p[i] * curvature_coefficient;
                else if(tool_R==7)//垂直抛
                   F_store[i] = flat_v_vertical[i] * curvature_coefficient;

               else
                   F_store[i] = flat_v_3p[i] * curvature_coefficient;

               if (vc_flag == false)
               {
                   F_store[i] = Change_C_V[i] * curvature_coefficient;
                   VC_store[i] = C_AXI_feed[i];
               }


           }

           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);

             //  if(i<a/2)
             //  F[i + 1] = F_store[a / 2-i-1]; 
             //  else
             //      F[i + 1] = F_store[i-a / 2];

               if (i < a / 2)
               {
                   F[i + 1] = F_store[a / 2 - i - 1];
                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));

                   // if (i == 32)
                   //     MessageBox.Show("");
               }
               else
               {
                   F[i + 1] = F_store[i - a / 2];

                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
               }
              
                   
                   //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i + 1] = Math.Abs(X2[i + 1] - X2[i])/(dist / contan_F);


               if (vc_flag == false)
               {
                   if (i < a / 2)
                   {
                       Nc[i + 1] = VC_store[a / 2 - i - 1] * C_motor_scale_factor + C_motor_offset;
                       if (Nc[i+1] > vc * C_motor_scale_factor + C_motor_offset)
                       {
                           Nc[i+1] = vc * C_motor_scale_factor + C_motor_offset;
                       }
                       //  F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));

                       // if (i == 32)
                       //     MessageBox.Show("");
                   }
                   else
                   {
                       Nc[i + 1] = VC_store[i - a / 2] * C_motor_scale_factor + C_motor_offset;
                       if (Nc[i + 1] > vc * C_motor_scale_factor + C_motor_offset)
                       {
                           Nc[i + 1] = vc * C_motor_scale_factor + C_motor_offset;
                       }

                       // F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
                   }
                  
 
               }


           }

           /***************写入TXT********/

           double[,] b = new double[c + 1, 7];
           double[,] b_null = new double[c + 1, 7];
           for (int i = 0; i < c + 1; i++)
           {


               b[i, 0] = X2[i+d+a/2];//x移动坐标
               b[i, 1] = B[i + d + a / 2];//B轴角度

               b[i, 2] = F[i + d + a / 2];//进给速度
               b[i, 3] = Nc[i + d + a / 2];//C轴转速

               b[i, 4] = X[i + d + a / 2];//X工件坐标
               b[i, 5] = ZZ[i + d + a / 2];//Z轴移动坐标
               b[i, 6] = Z1[i + d + a / 2];//函数曲线Z值


           }
           b[0, 2] = first_positon_feed;

           if (normal_flag == false)
           {
               MessageBox.Show("输入参数有误，数据异常，生成代码失败！");
               return b_null;
           }
           
           return b;
   
           //if (D_end == 0)
           //{
           //    double[,] b = new double[a + 1, 7];
           //    for (int i = 0; i < a + 1; i++)
           //    {

           //        b[i, 0] = X2[i];//x移动坐标
           //        b[i, 1] = B[i];//B轴角度

           //        b[i, 2] = F[i];//进给速度
           //        b[i, 3] = Nc[i];//C轴转速

           //        b[i, 4] = X[i];//X工件坐标
           //        b[i, 5] = ZZ[i];//Z轴移动坐标
           //        b[i, 6] = Z1[i];//函数曲线Z值
           //    }
           //    return b;
           //}
           //else
           //{
           //    double[,] b = new double[a / 2 - Convert.ToInt16(D_end * 10) / 2, 7];
           //    for (int i = 0; i < a / 2 - Convert.ToInt16(D_end * 10) / 2; i++)
           //    {
           //        b[i, 0] = X2[i];
           //        b[i, 1] = B[i];

           //        b[i, 2] = F[i];
           //        b[i, 3] = Nc[i];

           //        b[i, 4] = X[i];
           //        b[i, 5] = ZZ[i];
           //        b[i, 6] = Z1[i];
           //    }
           //    return b;
           //}
           //test = Z2[2];
       }
       public double[,] asphere_heitian_dist(double curvature_compensate, double first_positon_feed, double D_workpiece, double Dp, double C_motor_scale_factor, double C_motor_offset, double Lworkpiece_h, double other_h, double SAG, double yuan_r, double ao_tu, double R_P_right, double tool_R, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double R_P_left, double R, double K, double[] A, out double t)//0.01间隔代码
       {                        //,n:抛光次数，vc:工件最大转速，H:模仁底面到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间,D_end:加工范围的另一值直径；fixture_h夹具底面到模具承放面高度，SAG为矢高,Lworkpiece_h为L型件底面到中心孔高度，other_h为L型件底面到夹具底面高度,yuan_r为圆角直径，D_workpiece为模仁柱面直径，Dp为加工口径（图纸）
           //double h = -33.78;//h = 9.66,
           //double h = fixture_h + 12.666 + 7.177 - 46.147; //double h = fixture_h + 12.684 + 7.177 - 46.987;//h = -39.05;//h = -33.78;//h = 9.66,
           // double h = fixture_h - 0.999;// 45.999 - 44.83;
           vc_flag = true;//C轴恒速，变速功能不要
           double h = -Lworkpiece_h + other_h;//B轴旋转中心到模具平面高度
           H = H + ao_tu * SAG;//求模仁底面到面型中心的距离
           t = 0;
           dist = 0.01;//间隔为0.1
           double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
           double C = 1 / R;
           double Vs = Vu + 5;//U轴和C轴的线速度模
           //加工口径左边缘的X坐标
           double X_p = -Dp / 2;

           R_P_left = Math.Truncate(R_P_left * 100) / 100;
           Dp = Math.Truncate(Dp * 100) / 100;
           D_workpiece = Math.Truncate(D_workpiece * 100) / 100;

           // if (R_P_left * 10 % 2 == 1)
           //      R_P_left = R_P_left + 0.1;

           if (Dp * 100 % 2 == 1)
               Dp = Dp + dist;

           if (D_workpiece * 100 % 2 == 1)
               D_workpiece = D_workpiece - dist;

           int c = Convert.ToInt16((R_P_right - R_P_left) / dist);//输出代码行数-1
           int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
           int d = Convert.ToInt16(R_P_left / dist);
           int e = Convert.ToInt16(R_P_right / dist);
           int f = Convert.ToInt16(Dp / dist);
           bool normal_flag = true;
           double[] X = new double[a + 1];
           double[] Z = new double[a + 1];  //原点变为转动点后的Z值
           double[] Z1 = new double[a + 1];//函数曲线Z值
           double[] Z2 = new double[a + 1];//曲线的一阶导数
           double[] ZZ = new double[a + 1];//Z轴移动坐标
           double[] B = new double[a + 1];//B转动角度
           double[] Vc = new double[a + 1];//C线速度
           double[] Nc = new double[a + 1];//C轴转速
           double[] F = new double[a + 1]; //X轴进给速度
           double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
           double[] T = new double[a + 1];  //两点间时间
           double[] X2 = new double[a + 1];//x移动坐标
           double[] r0 = new double[a + 1];//点到转动中心的夹角
           double[] flat_v_1p = new double[] { 27.804, 24.2575, 16.5767, 11.2156, 9.2657, 8.3414, 7.4582, 6.7514, 6.3777, 5.9989, 5.6724, 5.4287, 5.2395, 5.0569, 4.8999, 4.7708, 4.6671, 4.5711, 4.482, 4.3991, 4.3219, 4.2464, 4.1822, 4.1219, 4.0649, 4.011, 3.9599, 3.9113, 3.8651, 3.821, 3.7789, 3.7386, 3.7, 3.6629, 3.6273, 3.5931, 3.5602, 3.5284, 3.4978, 3.4682, 3.4396, 3.4119, 3.3852, 3.3592, 3.3341, 3.3096, 3.2859, 3.2629, 3.2405, 3.2188, 3.1976, 3.1769, 3.1568, 3.1372, 3.1181, 3.0994, 3.0812, 3.0634, 3.0461, 3.0291, 3.0125, 2.9963, 2.9804, 2.9648, 2.9496, 2.9347, 2.9201, 2.9057, 2.8917, 2.8779, 2.8644, 2.8512, 2.8381, 2.8254, 2.8128, 2.8005, 2.7884, 2.7765, 2.7648, 2.7533, 2.742, 2.7309, 2.72, 2.7092, 2.6986, 2.6882, 2.6779, 2.6678, 2.6579, 2.6481, 2.6384, 2.6289, 2.6195, 2.6103, 2.6012, 2.5922, 2.5833, 2.5746, 2.566, 2.5575, 2.5491, 2.5408, 2.5326, 2.5246, 2.5166, 2.5087, 2.501, 2.4933, 2.4857, 2.4782, 2.4709, 2.4635, 2.4563, 2.4492, 2.4422, 2.4352, 2.4283, 2.4215, 2.4148, 2.4081, 2.4015, 2.395, 2.3886, 2.3822, 2.3759, 2.3697, 2.3635, 2.3574, 2.3514, 2.3454, 2.3395, 2.3336, 2.3278, 2.3221, 2.3164, 2.3108, 2.3052, 2.2997, 2.2943, 2.2888, 2.2835, 2.2782, 2.2729, 2.2677, 2.2625, 2.2574, 2.2524, 2.2473, 2.2424, 2.2374, 2.2325, 2.2277, 2.2229, 2.2181, 2.2134, 2.2087, 2.2041, 2.1995, 2.1949, 2.1904, 2.1859, 2.1815, 2.1771, 2.1727, 2.1683, 2.164, 2.1598, 2.1555, 2.1513, 2.1472, 2.143, 2.1389, 2.1348, 2.1308, 2.1268, 2.1228, 2.1188, 2.1149, 2.111, 2.1072, 2.1033, 2.0995, 2.0957, 2.092, 2.0883, 2.0846, 2.0809, 2.0773, 2.0736, 2.07, 2.0665, 2.0629, 2.0594, 2.0559, 2.0524, 2.049, 2.0456, 2.0422, 2.0388, 2.0354, 2.0321, 2.0288, 2.0255, 2.0222, 2.019, 2.0158, 2.0126, 2.0094, 2.0062, 2.0031, 1.9999, 1.9968, 1.9937, 1.9907, 1.9876, 1.9846, 1.9816, 1.9786, 1.9756, 1.9727, 1.9697, 1.9668, 1.9639, 1.961, 1.9582, 1.9553, 1.9525, 1.9497, 1.9469, 1.9441, 1.9413, 1.9386, 1.9358, 1.9331, 1.9304, 1.9277, 1.925, 1.9224, 1.9197, 1.9171, 1.9145, 1.9119, 1.9093, 1.9067, 1.9042, 1.9016, 1.8991, 1.8966, 1.8941, 1.8916, 1.8891, 1.8866, 1.8842, 1.8817, 1.8793, 1.8769, 1.8745, 1.8721, 1.8697, 1.8674, 1.865, 1.8627, 1.8604, 1.858, 1.8557, 1.8534, 1.8512, 1.8489, 1.8466, 1.8444, 1.8422, 1.8399, 1.8377, 1.8355, 1.8333, 1.8311, 1.829, 1.8268, 1.8246, 1.8225, 1.8204, 1.8183, 1.8161, 1.814, 1.812, 1.8099, 1.8078, 1.8057, 1.8037, 1.8016, 1.7996, 1.7976, 1.7956, 1.7936, 1.7916, 1.7896, 1.7876, 1.7856, 1.7837, 1.7817, 1.7798, 1.7778, 1.7759, 1.774, 1.7721, 1.7702, 1.7683, 1.7664, 1.7645, 1.7626, 1.7608 };
           double[] flat_v_3p = new double[] { 40.5259, 40.5141, 32.9916, 23.4416, 18.0608, 14.41, 12.4473, 11.0399, 9.7753, 8.9723, 8.2413, 7.68, 7.3041, 6.9754, 6.6848, 6.4257, 6.1929, 5.9818, 5.7891, 5.6112, 5.4335, 5.2944, 5.1922, 5.0945, 5.001, 4.9113, 4.8249, 4.7487, 4.6754, 4.6049, 4.5371, 4.4723, 4.4103, 4.3511, 4.2949, 4.2415, 4.1911, 4.1434, 4.0986, 4.0564, 4.0168, 3.9811, 3.9466, 3.9132, 3.8808, 3.8494, 3.8189, 3.7893, 3.7606, 3.7327, 3.7055, 3.6791, 3.6534, 3.6283, 3.6038, 3.58, 3.5568, 3.5341, 3.5119, 3.4903, 3.4692, 3.4485, 3.4283, 3.4085, 3.3892, 3.3702, 3.3517, 3.3335, 3.3157, 3.2982, 3.2811, 3.2644, 3.2479, 3.2317, 3.2159, 3.2003, 3.185, 3.17, 3.1553, 3.1408, 3.1265, 3.1125, 3.0987, 3.0852, 3.0718, 3.0587, 3.0458, 3.0331, 3.0206, 3.0083, 2.9962, 2.9842, 2.9725, 2.9609, 2.9494, 2.9382, 2.9271, 2.9161, 2.9053, 2.8947, 2.8842, 2.8739, 2.8636, 2.8536, 2.8436, 2.8338, 2.8241, 2.8145, 2.8051, 2.7957, 2.7865, 2.7774, 2.7684, 2.7596, 2.7508, 2.7421, 2.7335, 2.7251, 2.7167, 2.7084, 2.7002, 2.6922, 2.6842, 2.6762, 2.6684, 2.6607, 2.653, 2.6455, 2.638, 2.6306, 2.6232, 2.616, 2.6088, 2.6017, 2.5946, 2.5877, 2.5808, 2.574, 2.5672, 2.5605, 2.5539, 2.5473, 2.5408, 2.5344, 2.528, 2.5217, 2.5154, 2.5092, 2.5031, 2.497, 2.491, 2.485, 2.4791, 2.4732, 2.4674, 2.4616, 2.4559, 2.4502, 2.4446, 2.4391, 2.4335, 2.4281, 2.4226, 2.4172, 2.4119, 2.4066, 2.4014, 2.3962, 2.391, 2.3859, 2.3808, 2.3757, 2.3707, 2.3658, 2.3608, 2.356, 2.3511, 2.3463, 2.3415, 2.3368, 2.3321, 2.3274, 2.3228, 2.3182, 2.3136, 2.3091, 2.3046, 2.3001, 2.2957, 2.2913, 2.287, 2.2826, 2.2783, 2.274, 2.2698, 2.2656, 2.2614, 2.2572, 2.2531, 2.249, 2.2449, 2.2409, 2.2369, 2.2329, 2.2289, 2.225, 2.2211, 2.2172, 2.2133, 2.2095, 2.2057, 2.2019, 2.1981, 2.1944, 2.1907, 2.187, 2.1833, 2.1797, 2.1761, 2.1723, 2.168, 2.163, 2.1585, 2.155, 2.1515, 2.148, 2.1446, 2.1412, 2.1378, 2.1344, 2.131, 2.1277, 2.1244, 2.121, 2.1178, 2.1145, 2.1112, 2.108, 2.1048, 2.1016, 2.0984, 2.0953, 2.0921, 2.089, 2.0859, 2.0828, 2.0798, 2.0767, 2.0737, 2.0707, 2.0677, 2.0647, 2.0617, 2.0587, 2.0558, 2.0529, 2.05, 2.0471, 2.0442, 2.0414, 2.0385, 2.0357, 2.0329, 2.0301, 2.0273, 2.0245, 2.0217, 2.019, 2.0163, 2.0136, 2.0108, 2.0082, 2.0055, 2.0028, 2.0002, 1.9975, 1.9949, 1.9923, 1.9897, 1.9871, 1.9845, 1.982, 1.9794, 1.9769, 1.9744, 1.9719, 1.9694, 1.9669, 1.9644, 1.962, 1.9595, 1.9571, 1.9546, 1.9522, 1.9498, 1.9474, 1.945, 1.9427, 1.9403, 1.9379, 1.9356, 1.9333, 1.9309, 1.9286, 1.9263, 1.9241, 1.9218, 1.9195, 1.9172, 1.915, 1.9128 };
           double[] flat_v_5p = new double[] { 45.201, 44.14, 39.91, 31.5091, 25.0356, 20.3111, 16.5255, 14.4076, 12.827, 11.6089, 10.5568, 9.6852, 9.0155, 8.4652, 8.005, 7.6123, 7.2689, 6.9609, 6.678, 6.4284, 6.2069, 6.0652, 5.9219, 5.7785, 5.6365, 5.4985, 5.3666, 5.2474, 5.1562, 5.0698, 4.9876, 4.917, 4.8496, 4.7852, 4.7235, 4.6643, 4.6076, 4.553, 4.5005, 4.45, 4.4013, 4.3543, 4.3089, 4.2651, 4.2227, 4.1816, 4.1419, 4.1033, 4.0659, 4.0297, 3.9944, 3.9602, 3.9269, 3.8945, 3.863, 3.8323, 3.8024, 3.7733, 3.7448, 3.7171, 3.6901, 3.6636, 3.6378, 3.6126, 3.588, 3.5639, 3.5403, 3.5172, 3.4947, 3.4726, 3.4509, 3.4297, 3.4089, 3.3885, 3.3685, 3.3489, 3.3297, 3.3108, 3.2923, 3.2741, 3.2563, 3.2388, 3.2215, 3.2046, 3.188, 3.1716, 3.1555, 3.1397, 3.1241, 3.1088, 3.0938, 3.0789, 3.0643, 3.05, 3.0358, 3.0219, 3.0082, 2.9947, 2.9813, 2.9682, 2.9553, 2.9425, 2.9299, 2.9175, 2.9053, 2.8932, 2.8813, 2.8696, 2.858, 2.8466, 2.8353, 2.8242, 2.8132, 2.8024, 2.7916, 2.7811, 2.7706, 2.7603, 2.7501, 2.74, 2.7301, 2.7203, 2.7105, 2.7009, 2.6914, 2.6821, 2.6728, 2.6636, 2.6546, 2.6456, 2.6367, 2.628, 2.6193, 2.6107, 2.6022, 2.5938, 2.5855, 2.5773, 2.5692, 2.5611, 2.5531, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053 };
           double[] flat_v_vertical = new double[] { 29.474, 28.685, 27.348, 25.973, 17.534, 12.548, 11.556, 10.321, 8.903, 8.097, 7.608, 6.85, 6.639, 6.087, 5.697, 5.493, 5.426, 5.249, 5.031, 4.889, 4.774, 4.742, 4.563, 4.622, 4.32, 4.326, 4.269, 4.196, 4.27, 4.117, 4.073, 4.027, 3.912, 3.875, 3.945, 3.868, 3.828, 3.721, 3.806, 3.77, 3.741, 3.621, 3.754, 3.592, 3.563, 3.629, 3.495, 3.533, 3.535, 3.557, 3.358, 3.403, 3.375, 3.466, 3.357, 3.388, 3.319, 3.256, 3.277, 3.136, 3.174, 3.136, 3.151, 3.13, 3.103, 3.096, 3.116, 3.031, 3.022, 2.958, 3.014, 3.017, 3.066, 3.076, 3.067, 3.057, 3.047, 3.037, 3.027, 3.017, 3.007, 2.997, 2.987, 2.977, 2.967, 2.957, 2.947, 2.937, 2.927, 2.917, 2.907, 2.897, 2.887, 2.877, 2.867, 2.857, 2.847, 2.837, 2.827, 2.817, 2.807, 2.797, 2.787, 2.777, 2.767, 2.757, 2.747, 2.737, 2.727, 2.717, 2.707, 2.697, 2.687, 2.677, 2.667, 2.657, 2.647, 2.637, 2.627, 2.617, 2.607, 2.597, 2.587, 2.577, 2.567, 2.557, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053, 1.8027, 1.8002, 1.7976, 1.7951, 1.7925, 1.79, 1.7874, 1.7849, 1.7823, 1.7798, 1.7772, 1.7747, 1.7721, 1.7696, 1.767 };
           //  double[] beta1 = new double[a];


           // ArrayList ArrayList_X = new ArrayList();
           //ArrayList ArrayList_B = new ArrayList();
           // ArrayList ArrayList_F = new ArrayList();
           // ArrayList ArrayList_C = new ArrayList();
           // ArrayList ArrayList_Z1 = new ArrayList();
           // ArrayList ArrayList_Z2 = new ArrayList();

           //arry_X[0]=-D/2+0.1;
           for (int i = 0; i < a + 1; i++)//x坐标
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               X[i] = -D_workpiece / 2 + i * dist;
           }
           //double Z11 = -symbol * (Math.Pow(X[2], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[2], 2))) + A[0] * Math.Abs(X[2]) + A[1] * Math.Pow((X[2]), 2) + A[2] * Math.Pow(Math.Abs(X[2]), 3) + A[3] * Math.Pow((X[2]), 4) + A[4] * Math.Pow(Math.Abs(X[2]), 5) + A[5] * Math.Pow((X[1]), 6) + A[6] * Math.Pow(Math.Abs(X[2]), 7) + A[7] * Math.Pow((X[2]), 8) + A[8] * Math.Pow(Math.Abs(X[2]), 9) + A[9] * Math.Pow((X[2]), 10) + A[10] * Math.Pow(Math.Abs(X[2]), 11) + A[11] * Math.Pow((X[2]), 12) + A[12] * Math.Pow(Math.Abs(X[2]), 13) + A[13] * Math.Pow((X[2]), 14) + A[14] * Math.Pow(Math.Abs(X[2]), 15) + A[15] * Math.Pow((X[2]), 16) + A[16] * Math.Pow(Math.Abs(X[2]), 17) + A[17] * Math.Pow((X[2]), 18) + A[18] * Math.Pow(Math.Abs(X[2]), 19) + A[19] * Math.Pow((X[2]), 20));
           double Z11 = symbol * (Math.Pow(X[f / 2 + a / 2], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[f / 2 + a / 2], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[f / 2 + a / 2]) + A[1] * Math.Pow((X[f / 2 + a / 2]), 2) + A[2] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 3) + A[3] * Math.Pow((X[f / 2 + a / 2]), 4) + A[4] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 5) + A[5] * Math.Pow((X[f / 2 + a / 2]), 6) + A[6] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 7) + A[7] * Math.Pow((X[f / 2 + a / 2]), 8) + A[8] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 9) + A[9] * Math.Pow((X[f / 2 + a / 2]), 10) + A[10] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 11) + A[11] * Math.Pow((X[f / 2 + a / 2]), 12) + A[12] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 13) + A[13] * Math.Pow((X[f / 2 + a / 2]), 14) + A[14] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 15) + A[15] * Math.Pow((X[f / 2 + a / 2]), 16) + A[16] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 17) + A[17] * Math.Pow((X[f / 2 + a / 2]), 18) + A[18] * Math.Pow(Math.Abs(X[f / 2 + a / 2]), 19) + A[19] * Math.Pow((X[f / 2 + a / 2]), 20));


           if (Z11 > 0 && ao_tu > 0)
           {
               H = H - 2 * ao_tu * SAG;
           }
           if (Z11 < 0 && ao_tu < 0)
           {
               H = H - 2 * ao_tu * SAG;
           }
           //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));


           //加工口径左边缘B角度
           double tan_B_p;
           if (X_p >= 0)
               tan_B_p = symbol * ((2 * C * X_p * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + Math.Pow(X_p, 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X_p + A[2] * 3 * Math.Pow(X_p, 2) + A[3] * 4 * Math.Pow(X_p, 3) + A[4] * 5 * Math.Pow(X_p, 4) + A[5] * 6 * Math.Pow(X_p, 5) + A[6] * 7 * Math.Pow(X_p, 6) + A[7] * 8 * Math.Pow(X_p, 7) + A[8] * 9 * Math.Pow(X_p, 8) + A[9] * 10 * Math.Pow(X_p, 9) + A[10] * 11 * Math.Pow(X_p, 10) + A[11] * 12 * Math.Pow(X_p, 11) + A[12] * 13 * Math.Pow(X_p, 12) + A[13] * 14 * Math.Pow(X_p, 13) + A[14] * 15 * Math.Pow(X_p, 14) + A[15] * 16 * Math.Pow(X_p, 15) + A[16] * 17 * Math.Pow(X_p, 16) + A[17] * 18 * Math.Pow(X_p, 17) + A[18] * 19 * Math.Pow(X_p, 18) + A[19] * 20 * Math.Pow(X_p, 19));//正切值
           else
               tan_B_p = symbol * ((2 * C * X_p * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + Math.Pow(X_p, 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))), 2) - A[0] + A[1] * 2 * X_p - A[2] * 3 * Math.Pow(X_p, 2) + A[3] * 4 * Math.Pow(X_p, 3) - A[4] * 5 * Math.Pow(X_p, 4) + A[5] * 6 * Math.Pow(X_p, 5) - A[6] * 7 * Math.Pow(X_p, 6) + A[7] * 8 * Math.Pow(X_p, 7) - A[8] * 9 * Math.Pow(X_p, 8) + A[9] * 10 * Math.Pow(X_p, 9) - A[10] * 11 * Math.Pow(X_p, 10) + A[11] * 12 * Math.Pow(X_p, 11) - A[12] * 13 * Math.Pow(X_p, 12) + A[13] * 14 * Math.Pow(X_p, 13) - A[14] * 15 * Math.Pow(X_p, 14) + A[15] * 16 * Math.Pow(X_p, 15) - A[16] * 17 * Math.Pow(X_p, 16) + A[17] * 18 * Math.Pow(X_p, 17) - A[18] * 19 * Math.Pow(X_p, 18) + A[19] * 20 * Math.Pow(X_p, 19));//正切值


           double B_p = Math.Atan(tan_B_p) * 180 / Math.PI;  //B角度

           //加工口径左边缘Z坐标
           double Z_p = symbol * (Math.Pow(X_p, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X_p) + A[1] * Math.Pow((X_p), 2) + A[2] * Math.Pow(Math.Abs(X_p), 3) + A[3] * Math.Pow((X_p), 4) + A[4] * Math.Pow(Math.Abs(X_p), 5) + A[5] * Math.Pow((X_p), 6) + A[6] * Math.Pow(Math.Abs(X_p), 7) + A[7] * Math.Pow((X_p), 8) + A[8] * Math.Pow(Math.Abs(X_p), 9) + A[9] * Math.Pow((X_p), 10) + A[10] * Math.Pow(Math.Abs(X_p), 11) + A[11] * Math.Pow((X_p), 12) + A[12] * Math.Pow(Math.Abs(X_p), 13) + A[13] * Math.Pow((X_p), 14) + A[14] * Math.Pow(Math.Abs(X_p), 15) + A[15] * Math.Pow((X_p), 16) + A[16] * Math.Pow(Math.Abs(X_p), 17) + A[17] * Math.Pow((X_p), 18) + A[18] * Math.Pow(Math.Abs(X_p), 19) + A[19] * Math.Pow((X_p), 20));


           //左圆角圆心X坐标
           double X_circleCenter = -Math.Abs(yuan_r * Math.Sin(B_p / 180 * Math.PI)) + X_p;
           // double tempP = Math.Asin((X_p - X_circleCenter) / yuan_r) * 180 / Math.PI;


           //左圆角圆心Z坐标
           double Z_circleCenter = B_p / Math.Abs(B_p) * yuan_r * Math.Cos(B_p) + Z_p;

           //左圆角圆心左边取一位小数位
           double R_circleCenter = Math.Truncate((Math.Abs(X_circleCenter) * 100)) / 100;

           if (yuan_r == 0)//防止圆角为零时运算出错
               yuan_r = 0.01;
           for (int i = 0; i < a + 1; i++)//B角度，
           {


               //ArrayList_X.Add(-D / 2 + i * 0.001);
               //double[] values = ArrayList_X.Cast<double>().ToArray();
               //double[] d = Convert.ToDouble(X[i]);

               if (i < (D_workpiece / 2 - R_circleCenter) / dist - 1)//平面左边部分
               {
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                   B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }
               if (i >= ((D_workpiece / 2 - R_circleCenter) / dist) - 1 && i < (D_workpiece / 2 - Dp / 2) / dist)//左边圆角部分
               {
                   // Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                   B[i] = B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (a / 2 - i) * dist) / yuan_r) * 180 / Math.PI;
                   if (B[i] * B_p < 0)
                       B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }
               if (i >= Convert.ToInt16((D_workpiece / 2 - Dp / 2) / dist) && i <= Convert.ToInt16((D_workpiece / 2 + Dp / 2) / dist))
               {

                   Z1[i] = symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
                   //  Z1[i] = symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) - A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) - A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) - A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) - A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) - A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));

                   if (double.IsNaN(Z1[i]))
                   {
                       Z1[i] = Z1[i - 2];
                       //if()
                       //      MessageBox.Show("异常Z1N");
                       normal_flag = false;
                   }
                   if (double.IsInfinity(Z1[i]))
                   {
                       Z1[i] = Z1[i - 1] + Z1[i - 1] - Z1[i - 2];
                       //if()
                       //   MessageBox.Show("异常Z1I");
                       normal_flag = false;
                   }

                   Z[i] = H + Z1[i] + h;// -Z1[a - 1];
                   /* 
                    * 
                    * 
                    * 
                    * 
                    * 
                    * if (X[i] > 0)
                    {
                        //Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                        Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3)  / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * X[i] * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                  
                        B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                    }
              
                    else
                    {
                       // Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                        Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + C * Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * X[i] * Math.Pow(C, 2)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                  
                        // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                        B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                    }
                    */
                   //   Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                   if (X[i] <= 0)
                       Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                   else
                       Z2[i] = symbol * ((2 * C * X[i] * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + Math.Pow(X[i], 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                   //  Z2[i] = symbol * ((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) * C / Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) / Math.Pow(1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2)), 2) * Math.Pow(C, 2)*(K+1) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                   B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

                   if (i == a / 2)
                   {
                       //MessageBox.Show("异常B");
                       Z2[i] = 0;
                       B[i] = 0;
                   }


               }


               if (i > (D_workpiece / 2 + Dp / 2) / dist && i <= (D_workpiece / 2 + R_circleCenter) / dist + 1)//右边圆角部分
               {


                   //  Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));
                   B[i] = -B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (i - a / 2) * dist) / yuan_r) * 180 / Math.PI;
                   if (B[i] * B_p > 0)
                       B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值

               }

               if (i > (D_workpiece / 2 + R_circleCenter) / dist + 1)//右边平面部分
               {
                   Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                   B[i] = 0;
                   Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
               }




               //
               //                                                 
               //                                     
               // if(i>(D_workpiece / 2 + Dp / 2)/dist)



           }

           double r0max = 0, r0min = 100, tempz = 0;
           for (int i = 0; i < a + 1; i++)//x移动
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               // X[i] = -D / 2 + i * 0.001;

               r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

               //ZZ[i] = r0[i] * Math.Abs(Math.Cos(Math.Abs(B[i]) * Math.PI / 180 + Math.Abs(Math.Asin(X[i] / r0[i]))));//Z轴转动中心
               //MessageBox.Show(X[i].ToString());
               tempz = r0[i] * Math.Abs(Math.Sin(B[i] * Math.PI / 180));
               if (tempz > r0max)
                   r0max = tempz;
               if (tempz < r0min)
                   r0min = tempz;


               //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
               //X2[i] = -r0[i] * Math.Cos(Math.PI / 2 + Math.Asin(X[i] / r0[i]) + Math.Atan(Z2[i]));//x移动坐标
               X2[i] = r0[i] * Math.Cos(Math.Acos(X[i] / r0[i]) - B[i] * Math.PI / 180);//x移动坐标

               if (double.IsNaN(X2[i]))
               {
                   X2[i] = X2[i - 1];
                   //if()
                   //MessageBox.Show("异常X2N");
                   normal_flag = false;
               }
               if (double.IsInfinity(X2[i]))
               {
                   X2[i] = X2[i - 1];
                   //if()
                   // MessageBox.Show("异常X2I");
                   normal_flag = false;
               }

               //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
               // dX=(r0*cos(dangle));

               ZZ[i] = Math.Sqrt(Math.Pow(r0[i], 2) - Math.Pow(X2[i], 2));
           }


           /*
                       Vs=35;%
                       Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                      x=abs(X_1);
                      Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                       Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                       %ff= Wc>500;%找出设定值大于500极限的数点
                      %Wc(ff)=500;
                      Nc=(30.*Wc)/pi;
                      ff= Nc>180;%找出设定值大于300极限的数点
                       Nc(ff)=180;
                       Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
            */
           for (int i = 0; i < a + 1; i++)//C转速
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * C_motor_scale_factor + C_motor_offset;

               if (vc_flag == true)
                   Nc[i] = constan_vc * C_motor_scale_factor + C_motor_offset;
           }

           for (int i = 0; i < a + 1; i++)//F1进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
               T[i] = dist / F1[i];

           }
           F[0] = 200;
           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F[i + 1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i] = contan_F;


           }
           //  F[a] = F[a - 1];
           for (int i = 0; i < a + 1; i++)//加工时间
           {
               t = T[i] + t;

           }
           t = t / 60;
           for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
           {
               if (Nc[i] > vc * C_motor_scale_factor + C_motor_offset)
               {
                   Nc[i] = vc * C_motor_scale_factor + C_motor_offset;
               }
           }


           /**************根据面型来做曲率补正系数****************/

           /************************读取TXT里面的F************/
           double[] F_store = new double[a / 2 + 1];
           double curvature_coefficient;
           //string filepath;
           //if(tool_R==1)
           // filepath = System.Environment.CurrentDirectory + "\\Flat_V_1P.txt";
           //else if(tool_R==5)
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_5P.txt";
           //else
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_3P.txt";


           //StreamReader readfile = new StreamReader(filepath, System.Text.Encoding.UTF8);


           for (int i = 0; i < a / 2 + 1; i++)
           {
               //double temp=0;
               //for (int j = 0; j < 100; j++)
               //{
               //    temp = temp + Math.Sqrt(1 + Math.Pow((Math.Pow(i * 0.1 + j * 0.001, 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(i * 0.1 + j * 0.001, 2))) + A[0] * Math.Abs(i * 0.1 + j * 0.001) + A[1] * Math.Pow((i * 0.1 + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 3) + A[3] * Math.Pow((i * 0.1 + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 5) + A[5] * Math.Pow((i * 0.1 + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 7) + A[7] * Math.Pow((i * 0.1 + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 9) + A[9] * Math.Pow((i * 0.1 + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 11) + A[11] * Math.Pow((i * 0.1 + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 13) + A[13] * Math.Pow((i * 0.1 + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 15) + A[15] * Math.Pow((i * 0.1 + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 17) + A[17] * Math.Pow((i * 0.1 + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 19) + A[19] * Math.Pow((i * 0.1 + j * 0.001), 20)), 2)) * 0.001;

               //}
               //curvature_coefficient = temp / 0.1;
               //curvature_coefficient = 1;
               double temp = 0, temp2 = 0;
               // for (int j = 0; j < 100; j++)
               // {
               //if(j<10)
               int j = 10;
               temp2 = (Math.Pow(i * dist + j * 0.001, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(i * dist + j * 0.001, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(i * dist + j * 0.001) + A[1] * Math.Pow((i * dist + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * dist + j * 0.001), 3) + A[3] * Math.Pow((i * dist + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * dist + j * 0.001), 5) + A[5] * Math.Pow((i * dist + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * dist + j * 0.001), 7) + A[7] * Math.Pow((i * dist + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * dist + j * 0.001), 9) + A[9] * Math.Pow((i * dist + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * dist + j * 0.001), 11) + A[11] * Math.Pow((i * dist + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * dist + j * 0.001), 13) + A[13] * Math.Pow((i * dist + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * dist + j * 0.001), 15) + A[15] * Math.Pow((i * dist + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * dist + j * 0.001), 17) + A[17] * Math.Pow((i * dist + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * dist + j * 0.001), 19) + A[19] * Math.Pow((i * dist + j * 0.001), 20));
               j = 0;
               temp = (Math.Pow(i * dist + j * 0.001, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(i * dist + j * 0.001, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(i * dist + j * 0.001) + A[1] * Math.Pow((i * dist + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * dist + j * 0.001), 3) + A[3] * Math.Pow((i * dist + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * dist + j * 0.001), 5) + A[5] * Math.Pow((i * dist + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * dist + j * 0.001), 7) + A[7] * Math.Pow((i * dist + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * dist + j * 0.001), 9) + A[9] * Math.Pow((i * dist + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * dist + j * 0.001), 11) + A[11] * Math.Pow((i * dist + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * dist + j * 0.001), 13) + A[13] * Math.Pow((i * dist + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * dist + j * 0.001), 15) + A[15] * Math.Pow((i * dist + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * dist + j * 0.001), 17) + A[17] * Math.Pow((i * dist + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * dist + j * 0.001), 19) + A[19] * Math.Pow((i * dist + j * 0.001), 20));
               temp = Math.Abs(temp - temp2);
               temp = Math.Sqrt(Math.Pow(temp, 2) + Math.Pow(0.01, 2));
               curvature_coefficient = temp / 0.01;
               if (double.IsInfinity(curvature_coefficient))
                   curvature_coefficient = 1;
               if (double.IsNaN(curvature_coefficient))
                   curvature_coefficient = 1;

               if (curvature_compensate == 1)
                   curvature_coefficient = 1;
               else if (curvature_compensate == 2)
                   curvature_coefficient = 1 / curvature_coefficient;
               else
               { }

               if (i > (Dp / 2) / dist)
                   curvature_coefficient = 1;
              curvature_coefficient = 1;
               if (tool_R == 1)
                   F_store[i] = flat_v_1p[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10))] * curvature_coefficient;
               else if (tool_R == 5)

                   F_store[i] = flat_v_5p[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10))] * curvature_coefficient;
               else if (tool_R == 7)//垂直抛
                   F_store[i] = flat_v_vertical[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10))] * curvature_coefficient;

               else
                   F_store[i] = flat_v_3p[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10))] * curvature_coefficient;

           }

           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);

               //  if(i<a/2)
               //  F[i + 1] = F_store[a / 2-i-1]; 
               //  else
               //      F[i + 1] = F_store[i-a / 2];
               if (i < a / 2)
               {
                   F[i + 1] = F_store[a / 2 - i - 1];
                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(dist, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
                   // if (i == 32)
                   //     MessageBox.Show("");
               }
               else
               {
                   F[i + 1] = F_store[i - a / 2];

                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(dist, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
               }

               //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i + 1] = Math.Abs(X2[i + 1] - X2[i]) / (dist / contan_F);


           }

           /***************写入TXT********/

           double[,] b = new double[c + 1, 7];
           double[,] b_null = new double[c + 1, 7];
           for (int i = 0; i < c + 1; i++)
           {


               b[i, 0] = X2[i + d + a / 2];//x移动坐标
               b[i, 1] = B[i + d + a / 2];//B轴角度

               b[i, 2] = F[i + d + a / 2];//进给速度
               b[i, 3] = Nc[i + d + a / 2];//C轴转速

               b[i, 4] = X[i + d + a / 2];//X工件坐标
               b[i, 5] = ZZ[i + d + a / 2];//Z轴移动坐标
               b[i, 6] = Z1[i + d + a / 2];//函数曲线Z值


           }
           b[0, 2] = first_positon_feed;

           if (normal_flag == false)
           {
               MessageBox.Show("输入参数有误，数据异常，生成代码失败！");
               return b_null;
           }

           return b;

           //if (D_end == 0)
           //{
           //    double[,] b = new double[a + 1, 7];
           //    for (int i = 0; i < a + 1; i++)
           //    {

           //        b[i, 0] = X2[i];//x移动坐标
           //        b[i, 1] = B[i];//B轴角度

           //        b[i, 2] = F[i];//进给速度
           //        b[i, 3] = Nc[i];//C轴转速

           //        b[i, 4] = X[i];//X工件坐标
           //        b[i, 5] = ZZ[i];//Z轴移动坐标
           //        b[i, 6] = Z1[i];//函数曲线Z值
           //    }
           //    return b;
           //}
           //else
           //{
           //    double[,] b = new double[a / 2 - Convert.ToInt16(D_end * 10) / 2, 7];
           //    for (int i = 0; i < a / 2 - Convert.ToInt16(D_end * 10) / 2; i++)
           //    {
           //        b[i, 0] = X2[i];
           //        b[i, 1] = B[i];

           //        b[i, 2] = F[i];
           //        b[i, 3] = Nc[i];

           //        b[i, 4] = X[i];
           //        b[i, 5] = ZZ[i];
           //        b[i, 6] = Z1[i];
           //    }
           //    return b;
           //}
           //test = Z2[2];
       }

        /*********将F转化为X与B矢量和为路程**********/
       public double[,] asphere_heitian2(double SAG,double fixture_h,double  ao_tu, double D_end,double tool_R, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double D, double R, double K, double[] A, out double t)
       {                        //,n:抛光次数，vc:工件最大转速，H:模仁底到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间, D_end:加工范围的另一直径，
           //double h = fixture_h + 12.666 + 7.177 - 46.147;//h = -39.05;//h = -33.78;//h = 9.66,
           double h = fixture_h - 0.999;// 45.999 - 44.83;
           H = H + ao_tu * SAG;//求模仁底面到面型中心的距离
           t = 0;
           dist = 0.1;
           double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
           double Vs = Vu + 5;//U轴和C轴的线速度模
          // MessageBox.Show(ao_tu.ToString());
           D = Math.Truncate(D * 10) / 10;
           int a = Convert.ToInt16(D / dist);
           double[] X = new double[a + 1];
           double[] Z = new double[a + 1]; //原点变为转动点后的Z值
           double[] Z1 = new double[a + 1];//函数曲线Z值
           double[] Z2 = new double[a + 1];//曲线的一阶导数
           double[] B = new double[a + 1];//B转动角度
           double[] Vc = new double[a + 1];//C线速度
           double[] Nc = new double[a + 1];//C轴转速
           double[] F = new double[a + 1]; //X轴进给速度
           double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
           double[] T = new double[a + 1];  //两点间时间
           double[] X2 = new double[a + 1];//x移动坐标
           double[] ZZ = new double[a + 1];//Z轴移动坐标
           double[] r0 = new double[a + 1];//点到转动中心的夹角
           //  double[] beta1 = new double[a];


           // ArrayList ArrayList_X = new ArrayList();
           //ArrayList ArrayList_B = new ArrayList();
           // ArrayList ArrayList_F = new ArrayList();
           // ArrayList ArrayList_C = new ArrayList();
           // ArrayList ArrayList_Z1 = new ArrayList();
           // ArrayList ArrayList_Z2 = new ArrayList();

           //arry_X[0]=-D/2+0.1;
           for (int i = 0; i < a + 1; i++)//x坐标
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               X[i] = -D / 2 + i * dist;
           }

           //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

           for (int i = 0; i < a + 1; i++)//B角度，
           {
               //ArrayList_X.Add(-D / 2 + i * 0.001);
               //double[] values = ArrayList_X.Cast<double>().ToArray();
               //double[] d = Convert.ToDouble(X[i]);
               Z1[i] = symbol * (Math.Pow(X[i], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
               Z[i] = H + Z1[i] + h;// -Z1[a - 1];
              // Z[i] = H + Z1[i] - h;
               if (X[i] > 0)
               {
                   Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                   B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

               }
               else if (X[i] == 0)
               {
                   Z2[i] = 0;
                   B[i] = 0;
               }
               else
               {
                   Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                   // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                   B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;

               }

           }

           double r0max = 0, r0min = 100, tempz = 0;
           for (int i = 0; i < a + 1; i++)//x移动
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               // X[i] = -D / 2 + i * 0.001;

               r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

               ZZ[i] = r0[i] * Math.Abs(Math.Cos(B[i] * Math.PI / 180 + Math.Asin(X[i]/r0[i])));//Z轴转动中心
               tempz = r0[i] * Math.Abs(Math.Sin(B[i] * Math.PI/180));
               if (tempz > r0max)
                   r0max = tempz;
               if (tempz < r0min)
                   r0min = tempz;
               //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
                   X2[i] = -r0[i] * Math.Cos(Math.PI / 2 + Math.Asin(X[i] / r0[i]) + Math.Atan(Z2[i]));//x移动坐标
               //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
               // dX=(r0*cos(dangle));
           }


      /*
           for (int i = 0; i < a + 1; i++)//C转速
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * 150.469 - 143.259;

               if (vc_flag == true)
                   Nc[i] = constan_vc * 150.469 - 143.259;
           }*/

           string C_filepath = System.Environment.CurrentDirectory + "\\C_SPEED.txt";
           StreamReader C_readfile = new StreamReader(C_filepath, System.Text.Encoding.UTF8);

           double[] C_store = new double[a / 2 + 1];//c轴转速

           for (int i = 0; i < a / 2 + 1; i++)
           {
               C_store[i] = Convert.ToDouble(C_readfile.ReadLine());
           }

           Nc[0] = C_store[a / 2 ] *  153.1862 +26.5573;
           for (int i = 0; i < a; i++)//C转速
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               //  Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
               //  Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * 150.469 - 143.259;

               // Nc[i] =  * 150.469 - 143.259;
               if (i < a / 2)
                   Nc[i + 1] = C_store[a / 2 - i - 1] * 153.1862 + 26.5573;
               else
                   Nc[i + 1] = C_store[i - a / 2] * 153.1862 + 26.5573;

               if (vc_flag == true)
               {
                   Nc[i] = constan_vc * 153.1862 + 26.5573;
                   Nc[a] = constan_vc * 153.1862 + 26.5573;
               }

           }
           C_readfile.Close();
           for (int i = 0; i < a + 1; i++)//F1进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
               T[i] = dist / F1[i];

           }
           F[0] = 100;
           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F[i + 1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i] = contan_F;


           }
           //  F[a] = F[a - 1];
           for (int i = 0; i < a + 1; i++)//加工时间
           {
               t = T[i] + t;

           }
           t = t / 60;
           for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
           {
               if (Nc[i] > vc * 153.1862 + 26.5573)
               {
                   Nc[i] = vc * 153.1862 + 26.5573;
               }
           }
           /************************读取TXT里面的F************/
           double[] F_store = new double[a / 2 + 1];
           double[] curvature = new double[a / 2 + 1];
           double curvature_coefficient;
           string filepath;
           if (tool_R == 1)
               filepath = System.Environment.CurrentDirectory + "\\Flat_V_1P.txt";
           else if (tool_R == 5)
               filepath = System.Environment.CurrentDirectory + "\\Flat_V_5P.txt";
           else
               filepath = System.Environment.CurrentDirectory + "\\Flat_V_3P.txt";


           StreamReader readfile = new StreamReader(filepath, System.Text.Encoding.UTF8);


           for (int i = 0; i < a / 2 + 1; i++)//曲率补正
           {
              // F_store[i] = Convert.ToDouble(readfile.ReadLine());
               double temp = 0;

               for (int j = 1; j < 101; j++)
               {
                   temp = temp + Math.Sqrt(0.000001 + Math.Pow((Math.Pow(i * 0.1 + j * 0.001, 2) / (R + Math.Sqrt(Math.Pow(R, 2) - 
                       (K + 1) * Math.Pow(i * 0.1 + j * 0.001, 2))) + A[0] * Math.Abs(i * 0.1 + j * 0.001) + 
                       A[1] * Math.Pow((i * 0.1 + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 3) + 
                       A[3] * Math.Pow((i * 0.1 + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 5) + 
                       A[5] * Math.Pow((i * 0.1 + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 7) + 
                       A[7] * Math.Pow((i * 0.1 + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 9) + 
                       A[9] * Math.Pow((i * 0.1 + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 11) +
                       A[11] * Math.Pow((i * 0.1 + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 13) +
                       A[13] * Math.Pow((i * 0.1 + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 15) + 
                       A[15] * Math.Pow((i * 0.1 + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 17) + 
                       A[17] * Math.Pow((i * 0.1 + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 19) +
                       A[19] * Math.Pow((i * 0.1 + j * 0.001), 20)-
                       (Math.Pow(i * 0.1 + (j-1) * 0.001, 2) / (R + Math.Sqrt(Math.Pow(R, 2) -
                       (K + 1) * Math.Pow(i * 0.1 + (j - 1) * 0.001, 2))) + A[0] * Math.Abs(i * 0.1 + (j - 1) * 0.001) +
                       A[1] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 3) +
                       A[3] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 5) +
                       A[5] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 7) +
                       A[7] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 9) +
                       A[9] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 11) +
                       A[11] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 13) +
                       A[13] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 15) +
                       A[15] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 17) +
                       A[17] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 + (j - 1) * 0.001), 19) +
                       A[19] * Math.Pow((i * 0.1 + (j - 1) * 0.001), 20))
                       ), 2));

               }

              /* temp = Math.Sqrt(0.01 + Math.Pow(Math.Abs((Math.Pow(i * 0.1 + 0.1, 2) / (R + Math.Sqrt(Math.Pow(R, 2) -
                       (K + 1) * Math.Pow(i * 0.1 + 0.1, 2))) + A[0] * Math.Abs(i * 0.1 +  0.1) +
                       A[1] * Math.Pow((i * 0.1 + 0.1), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 +  0.1), 3) +
                       A[3] * Math.Pow((i * 0.1 +  0.1), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + 0.1), 5) +
                       A[5] * Math.Pow((i * 0.1 + 0.1), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 +  0.1), 7) +
                       A[7] * Math.Pow((i * 0.1 + 0.1), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + 0.1), 9) +
                       A[9] * Math.Pow((i * 0.1 + 0.1), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + 0.1), 11) +
                       A[11] * Math.Pow((i * 0.1 + 0.1), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + 0.1), 13) +
                       A[13] * Math.Pow((i * 0.1 +  0.1), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 +  0.1), 15) +
                       A[15] * Math.Pow((i * 0.1 +  0.1), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + 0.1), 17) +
                       A[17] * Math.Pow((i * 0.1 + 0.1), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 +  0.1), 19) +
                       A[19] * Math.Pow((i * 0.1 +  0.1), 20))) -
                       Math.Abs((Math.Pow(i * 0.1 , 2) / (R + Math.Sqrt(Math.Pow(R, 2) -
                       (K + 1) * Math.Pow(i * 0.1 , 2))) + A[0] * Math.Abs(i * 0.1 ) +
                       A[1] * Math.Pow((i * 0.1 ), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 ), 3) +
                       A[3] * Math.Pow((i * 0.1 ), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 ), 5) +
                       A[5] * Math.Pow((i * 0.1 ), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 ), 7) +
                       A[7] * Math.Pow((i * 0.1), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 ), 9) +
                       A[9] * Math.Pow((i * 0.1 ), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 ), 11) +
                       A[11] * Math.Pow((i * 0.1 ), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 ), 13) +
                       A[13] * Math.Pow((i * 0.1 ), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 ), 15) +
                       A[15] * Math.Pow((i * 0.1 ), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 ), 17) +
                       A[17] * Math.Pow((i * 0.1 ), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 ), 19) +
                       A[19] * Math.Pow((i * 0.1 ), 20)))
                       , 2));*/
               curvature_coefficient = temp / 0.1;
               curvature[i] = curvature_coefficient;
               curvature_coefficient = 1;
               F_store[i] = Convert.ToDouble(readfile.ReadLine()) / curvature_coefficient;
           }

           readfile.Close();
           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               if (i < a / 2)
               {
                   F[i + 1] = F_store[a / 2 - i - 1];
                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(B[i + 1] - B[i], 2)));
               }
               else
               {
                   F[i + 1] = F_store[i - a / 2];
                   F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(B[i + 1] - B[i], 2)));
               }

               //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i+1] = contan_F;


           }

           /***************写入TXT********/

           string result1 = @"D:\result1.txt";//结果保存到F:\result1.txt

          //*先清空result1.txt文件内容
          FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
           stream2.Seek(0, SeekOrigin.Begin);
           stream2.SetLength(0); //清空txt文件
           stream2.Close();


           FileStream fs = new FileStream(result1, FileMode.Append);
           // fs.Seek(0, SeekOrigin.Begin);
           // fs.SetLength(0);
           StreamWriter wr = null;

           wr = new StreamWriter(fs);
           wr.WriteLine("OPEN PROG 2");
           wr.WriteLine("CLEAR");
           wr.WriteLine("G90 G01");
           wr.WriteLine("WHILE(P25<=" + Convert.ToString(n) + ")");
           for (int i = 0; i < a + 1; i++)
           {
               wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
           }
           // for (int i = a-1; i >= 0; i--)
           //{
           //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
           // 
           // }          
           wr.WriteLine("ENDWHILE");
           if (n % 2 == 1)
           {
               for (int i = 0; i < a + 1; i++)
               {
                   wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
               
               }

           }
           wr.WriteLine("P21=1");
           wr.WriteLine("CLOSE");
           for (int i = 0; i < a/2 + 1; i++)
           {
               wr.WriteLine(Convert.ToString(curvature[i]));

           }
           wr.WriteLine(Convert.ToString(r0max));
           wr.WriteLine(Convert.ToString(r0min));
           wr.Close();         
     /**********************************************************************************************************/      

           if (D_end == 0)
           {
               double[,] b = new double[a + 1, 6];
               for (int i = 0; i < a + 1; i++)
               {
                   b[i, 0] = X2[i];
                   b[i, 1] = B[i];

                   b[i, 2] = F[i];
                   b[i, 3] = Nc[i];

                   b[i, 4] = X[i];
                   b[i, 5] = ZZ[i];
               }
               return b;
           }
           else
           {
               double[,] b = new double[a/2-Convert.ToInt16(D_end*10)/2 , 6];
               for (int i = 0; i < a / 2 - Convert.ToInt16(D_end*10) / 2; i++)
               {
                   b[i, 0] = X2[i];
                   b[i, 1] = B[i];

                   b[i, 2] = F[i];
                   b[i, 3] = Nc[i];

                   b[i, 4] = X[i];
                   b[i, 5] = ZZ[i];
               }
               return b;
           }
        
           
           //test = Z2[2];
       }
       public double[,] sphere(double first_position_feed ,double D_workpiece, double Dp, double C_motor_scale_factor, double C_motor_offset, double Lworkpiece_h, double other_h, double SAG, double yuan_r, double ao_tu, double R_P_right, double tool_R, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double R_P_left, double R, double K, double[] A, out double t)
       {
           // double h = fixture_h - 0.999;// 45.999 - 44.83;
            double h = -Lworkpiece_h + other_h;//B轴旋转中心到模具平面高度
       
            vc_flag = true;//C轴恒速，变速功能不要
            H = H -symbol * SAG;//求模仁底面到面型中心的距离
            t = 0;
            dist = 0.1;
            double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
            double C = 1 / R;
            double Vs = Vu + 5;//U轴和C轴的线速度模
            double X_p = -Dp / 2;

            R_P_left = Math.Truncate(R_P_left * 10) / 10;
            Dp = Math.Truncate(Dp * 10) / 10;
            D_workpiece = Math.Truncate(D_workpiece * 10) / 10;

            // if (R_P_left * 10 % 2 == 1)
            //      R_P_left = R_P_left + 0.1;

            if (Dp * 10 % 2 == 1)
                Dp = Dp + 0.1;

            if (D_workpiece * 10 % 2 == 1)
                D_workpiece = D_workpiece - 0.1;

            int c = Convert.ToInt16((R_P_right - R_P_left) / dist);//输出代码行数-1
            int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
            int d = Convert.ToInt16(R_P_left / dist);
            int e = Convert.ToInt16(R_P_right / dist);
            bool normal_flag = true;
            double[] X = new double[a + 1];
            double[] Z = new double[a + 1];  //原点变为转动点后的Z值
            double[] Z1 = new double[a + 1];//函数曲线Z值
            double[] Z2 = new double[a + 1];//曲线的一阶导数
            double[] ZZ = new double[a + 1];//Z轴移动坐标
            double[] B = new double[a + 1];//B转动角度
            double[] Vc = new double[a + 1];//C线速度
            double[] Nc = new double[a + 1];//C轴转速
            double[] F = new double[a + 1]; //X轴进给速度
            double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
            double[] T = new double[a + 1];  //两点间时间
            double[] X2 = new double[a + 1];//x移动坐标
            double[] r0 = new double[a + 1];//点到转动中心的夹角
            double[] flat_v_1p = new double[] { 27.804, 24.2575, 16.5767, 11.2156, 9.2657, 8.3414, 7.4582, 6.7514, 6.3777, 5.9989, 5.6724, 5.4287, 5.2395, 5.0569, 4.8999, 4.7708, 4.6671, 4.5711, 4.482, 4.3991, 4.3219, 4.2464, 4.1822, 4.1219, 4.0649, 4.011, 3.9599, 3.9113, 3.8651, 3.821, 3.7789, 3.7386, 3.7, 3.6629, 3.6273, 3.5931, 3.5602, 3.5284, 3.4978, 3.4682, 3.4396, 3.4119, 3.3852, 3.3592, 3.3341, 3.3096, 3.2859, 3.2629, 3.2405, 3.2188, 3.1976, 3.1769, 3.1568, 3.1372, 3.1181, 3.0994, 3.0812, 3.0634, 3.0461, 3.0291, 3.0125, 2.9963, 2.9804, 2.9648, 2.9496, 2.9347, 2.9201, 2.9057, 2.8917, 2.8779, 2.8644, 2.8512, 2.8381, 2.8254, 2.8128, 2.8005, 2.7884, 2.7765, 2.7648, 2.7533, 2.742, 2.7309, 2.72, 2.7092, 2.6986, 2.6882, 2.6779, 2.6678, 2.6579, 2.6481, 2.6384, 2.6289, 2.6195, 2.6103, 2.6012, 2.5922, 2.5833, 2.5746, 2.566, 2.5575, 2.5491, 2.5408, 2.5326, 2.5246, 2.5166, 2.5087, 2.501, 2.4933, 2.4857, 2.4782, 2.4709, 2.4635, 2.4563, 2.4492, 2.4422, 2.4352, 2.4283, 2.4215, 2.4148, 2.4081, 2.4015, 2.395, 2.3886, 2.3822, 2.3759, 2.3697, 2.3635, 2.3574, 2.3514, 2.3454, 2.3395, 2.3336, 2.3278, 2.3221, 2.3164, 2.3108, 2.3052, 2.2997, 2.2943, 2.2888, 2.2835, 2.2782, 2.2729, 2.2677, 2.2625, 2.2574, 2.2524, 2.2473, 2.2424, 2.2374, 2.2325, 2.2277, 2.2229, 2.2181, 2.2134, 2.2087, 2.2041, 2.1995, 2.1949, 2.1904, 2.1859, 2.1815, 2.1771, 2.1727, 2.1683, 2.164, 2.1598, 2.1555, 2.1513, 2.1472, 2.143, 2.1389, 2.1348, 2.1308, 2.1268, 2.1228, 2.1188, 2.1149, 2.111, 2.1072, 2.1033, 2.0995, 2.0957, 2.092, 2.0883, 2.0846, 2.0809, 2.0773, 2.0736, 2.07, 2.0665, 2.0629, 2.0594, 2.0559, 2.0524, 2.049, 2.0456, 2.0422, 2.0388, 2.0354, 2.0321, 2.0288, 2.0255, 2.0222, 2.019, 2.0158, 2.0126, 2.0094, 2.0062, 2.0031, 1.9999, 1.9968, 1.9937, 1.9907, 1.9876, 1.9846, 1.9816, 1.9786, 1.9756, 1.9727, 1.9697, 1.9668, 1.9639, 1.961, 1.9582, 1.9553, 1.9525, 1.9497, 1.9469, 1.9441, 1.9413, 1.9386, 1.9358, 1.9331, 1.9304, 1.9277, 1.925, 1.9224, 1.9197, 1.9171, 1.9145, 1.9119, 1.9093, 1.9067, 1.9042, 1.9016, 1.8991, 1.8966, 1.8941, 1.8916, 1.8891, 1.8866, 1.8842, 1.8817, 1.8793, 1.8769, 1.8745, 1.8721, 1.8697, 1.8674, 1.865, 1.8627, 1.8604, 1.858, 1.8557, 1.8534, 1.8512, 1.8489, 1.8466, 1.8444, 1.8422, 1.8399, 1.8377, 1.8355, 1.8333, 1.8311, 1.829, 1.8268, 1.8246, 1.8225, 1.8204, 1.8183, 1.8161, 1.814, 1.812, 1.8099, 1.8078, 1.8057, 1.8037, 1.8016, 1.7996, 1.7976, 1.7956, 1.7936, 1.7916, 1.7896, 1.7876, 1.7856, 1.7837, 1.7817, 1.7798, 1.7778, 1.7759, 1.774, 1.7721, 1.7702, 1.7683, 1.7664, 1.7645, 1.7626, 1.7608 };
            double[] flat_v_3p = new double[] { 40.5259, 40.5141, 32.9916, 23.4416, 18.0608, 14.41, 12.4473, 11.0399, 9.7753, 8.9723, 8.2413, 7.68, 7.3041, 6.9754, 6.6848, 6.4257, 6.1929, 5.9818, 5.7891, 5.6112, 5.4335, 5.2944, 5.1922, 5.0945, 5.001, 4.9113, 4.8249, 4.7487, 4.6754, 4.6049, 4.5371, 4.4723, 4.4103, 4.3511, 4.2949, 4.2415, 4.1911, 4.1434, 4.0986, 4.0564, 4.0168, 3.9811, 3.9466, 3.9132, 3.8808, 3.8494, 3.8189, 3.7893, 3.7606, 3.7327, 3.7055, 3.6791, 3.6534, 3.6283, 3.6038, 3.58, 3.5568, 3.5341, 3.5119, 3.4903, 3.4692, 3.4485, 3.4283, 3.4085, 3.3892, 3.3702, 3.3517, 3.3335, 3.3157, 3.2982, 3.2811, 3.2644, 3.2479, 3.2317, 3.2159, 3.2003, 3.185, 3.17, 3.1553, 3.1408, 3.1265, 3.1125, 3.0987, 3.0852, 3.0718, 3.0587, 3.0458, 3.0331, 3.0206, 3.0083, 2.9962, 2.9842, 2.9725, 2.9609, 2.9494, 2.9382, 2.9271, 2.9161, 2.9053, 2.8947, 2.8842, 2.8739, 2.8636, 2.8536, 2.8436, 2.8338, 2.8241, 2.8145, 2.8051, 2.7957, 2.7865, 2.7774, 2.7684, 2.7596, 2.7508, 2.7421, 2.7335, 2.7251, 2.7167, 2.7084, 2.7002, 2.6922, 2.6842, 2.6762, 2.6684, 2.6607, 2.653, 2.6455, 2.638, 2.6306, 2.6232, 2.616, 2.6088, 2.6017, 2.5946, 2.5877, 2.5808, 2.574, 2.5672, 2.5605, 2.5539, 2.5473, 2.5408, 2.5344, 2.528, 2.5217, 2.5154, 2.5092, 2.5031, 2.497, 2.491, 2.485, 2.4791, 2.4732, 2.4674, 2.4616, 2.4559, 2.4502, 2.4446, 2.4391, 2.4335, 2.4281, 2.4226, 2.4172, 2.4119, 2.4066, 2.4014, 2.3962, 2.391, 2.3859, 2.3808, 2.3757, 2.3707, 2.3658, 2.3608, 2.356, 2.3511, 2.3463, 2.3415, 2.3368, 2.3321, 2.3274, 2.3228, 2.3182, 2.3136, 2.3091, 2.3046, 2.3001, 2.2957, 2.2913, 2.287, 2.2826, 2.2783, 2.274, 2.2698, 2.2656, 2.2614, 2.2572, 2.2531, 2.249, 2.2449, 2.2409, 2.2369, 2.2329, 2.2289, 2.225, 2.2211, 2.2172, 2.2133, 2.2095, 2.2057, 2.2019, 2.1981, 2.1944, 2.1907, 2.187, 2.1833, 2.1797, 2.1761, 2.1723, 2.168, 2.163, 2.1585, 2.155, 2.1515, 2.148, 2.1446, 2.1412, 2.1378, 2.1344, 2.131, 2.1277, 2.1244, 2.121, 2.1178, 2.1145, 2.1112, 2.108, 2.1048, 2.1016, 2.0984, 2.0953, 2.0921, 2.089, 2.0859, 2.0828, 2.0798, 2.0767, 2.0737, 2.0707, 2.0677, 2.0647, 2.0617, 2.0587, 2.0558, 2.0529, 2.05, 2.0471, 2.0442, 2.0414, 2.0385, 2.0357, 2.0329, 2.0301, 2.0273, 2.0245, 2.0217, 2.019, 2.0163, 2.0136, 2.0108, 2.0082, 2.0055, 2.0028, 2.0002, 1.9975, 1.9949, 1.9923, 1.9897, 1.9871, 1.9845, 1.982, 1.9794, 1.9769, 1.9744, 1.9719, 1.9694, 1.9669, 1.9644, 1.962, 1.9595, 1.9571, 1.9546, 1.9522, 1.9498, 1.9474, 1.945, 1.9427, 1.9403, 1.9379, 1.9356, 1.9333, 1.9309, 1.9286, 1.9263, 1.9241, 1.9218, 1.9195, 1.9172, 1.915, 1.9128 };
            double[] flat_v_5p = new double[] { 45.201, 44.14, 39.91, 31.5091, 25.0356, 20.3111, 16.5255, 14.4076, 12.827, 11.6089, 10.5568, 9.6852, 9.0155, 8.4652, 8.005, 7.6123, 7.2689, 6.9609, 6.678, 6.4284, 6.2069, 6.0652, 5.9219, 5.7785, 5.6365, 5.4985, 5.3666, 5.2474, 5.1562, 5.0698, 4.9876, 4.917, 4.8496, 4.7852, 4.7235, 4.6643, 4.6076, 4.553, 4.5005, 4.45, 4.4013, 4.3543, 4.3089, 4.2651, 4.2227, 4.1816, 4.1419, 4.1033, 4.0659, 4.0297, 3.9944, 3.9602, 3.9269, 3.8945, 3.863, 3.8323, 3.8024, 3.7733, 3.7448, 3.7171, 3.6901, 3.6636, 3.6378, 3.6126, 3.588, 3.5639, 3.5403, 3.5172, 3.4947, 3.4726, 3.4509, 3.4297, 3.4089, 3.3885, 3.3685, 3.3489, 3.3297, 3.3108, 3.2923, 3.2741, 3.2563, 3.2388, 3.2215, 3.2046, 3.188, 3.1716, 3.1555, 3.1397, 3.1241, 3.1088, 3.0938, 3.0789, 3.0643, 3.05, 3.0358, 3.0219, 3.0082, 2.9947, 2.9813, 2.9682, 2.9553, 2.9425, 2.9299, 2.9175, 2.9053, 2.8932, 2.8813, 2.8696, 2.858, 2.8466, 2.8353, 2.8242, 2.8132, 2.8024, 2.7916, 2.7811, 2.7706, 2.7603, 2.7501, 2.74, 2.7301, 2.7203, 2.7105, 2.7009, 2.6914, 2.6821, 2.6728, 2.6636, 2.6546, 2.6456, 2.6367, 2.628, 2.6193, 2.6107, 2.6022, 2.5938, 2.5855, 2.5773, 2.5692, 2.5611, 2.5531, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053 };
            double[] flat_v_vertical = new double[] { 29.474, 28.685, 27.348, 25.973, 17.534, 12.548, 11.556, 10.321, 8.903, 8.097, 7.608, 6.85, 6.639, 6.087, 5.697, 5.493, 5.426, 5.249, 5.031, 4.889, 4.774, 4.742, 4.563, 4.622, 4.32, 4.326, 4.269, 4.196, 4.27, 4.117, 4.073, 4.027, 3.912, 3.875, 3.945, 3.868, 3.828, 3.721, 3.806, 3.77, 3.741, 3.621, 3.754, 3.592, 3.563, 3.629, 3.495, 3.533, 3.535, 3.557, 3.358, 3.403, 3.375, 3.466, 3.357, 3.388, 3.319, 3.256, 3.277, 3.136, 3.174, 3.136, 3.151, 3.13, 3.103, 3.096, 3.116, 3.031, 3.022, 2.958, 3.014, 3.017, 3.066, 3.076, 3.067, 3.057, 3.047, 3.037, 3.027, 3.017, 3.007, 2.997, 2.987, 2.977, 2.967, 2.957, 2.947, 2.937, 2.927, 2.917, 2.907, 2.897, 2.887, 2.877, 2.867, 2.857, 2.847, 2.837, 2.827, 2.817, 2.807, 2.797, 2.787, 2.777, 2.767, 2.757, 2.747, 2.737, 2.727, 2.717, 2.707, 2.697, 2.687, 2.677, 2.667, 2.657, 2.647, 2.637, 2.627, 2.617, 2.607, 2.597, 2.587, 2.577, 2.567, 2.557, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053, 1.8027, 1.8002, 1.7976, 1.7951, 1.7925, 1.79, 1.7874, 1.7849, 1.7823, 1.7798, 1.7772, 1.7747, 1.7721, 1.7696, 1.767 };
      

            // ArrayList ArrayList_X = new ArrayList();
            //ArrayList ArrayList_B = new ArrayList();
            // ArrayList ArrayList_F = new ArrayList();
            // ArrayList ArrayList_C = new ArrayList();
            // ArrayList ArrayList_Z1 = new ArrayList();
            // ArrayList ArrayList_Z2 = new ArrayList();

            //arry_X[0]=-D/2+0.1;
            for (int i = 0; i < a + 1; i++)//x坐标
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                X[i] = -D_workpiece / 2 + i * dist;
            }





            //加工口径左边缘B角度
          //  double tan_B_p = symbol * ((2 * C * X_p * (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + Math.Pow(X_p, 3) / Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2)) * (K + 1) * Math.Pow(C, 3)) / Math.Pow((1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))), 2) + A[0] + A[1] * 2 * X_p + A[2] * 3 * Math.Pow(X_p, 2) + A[3] * 4 * Math.Pow(X_p, 3) + A[4] * 5 * Math.Pow(X_p, 4) + A[5] * 6 * Math.Pow(X_p, 5) + A[6] * 7 * Math.Pow(X_p, 6) + A[7] * 8 * Math.Pow(X_p, 7) + A[8] * 9 * Math.Pow(X_p, 8) + A[9] * 10 * Math.Pow(X_p, 9) + A[10] * 11 * Math.Pow(X_p, 10) + A[11] * 12 * Math.Pow(X_p, 11) + A[12] * 13 * Math.Pow(X_p, 12) + A[13] * 14 * Math.Pow(X_p, 13) + A[14] * 15 * Math.Pow(X_p, 14) + A[15] * 16 * Math.Pow(X_p, 15) + A[16] * 17 * Math.Pow(X_p, 16) + A[17] * 18 * Math.Pow(X_p, 17) + A[18] * 19 * Math.Pow(X_p, 18) + A[19] * 20 * Math.Pow(X_p, 19));//正切值
            double B_p = symbol * Math.Asin(X_p / R) * 180 / Math.PI;
          // double B_p = Math.Atan(tan_B_p) * 180 / Math.PI;  //B角度
      //     B[i] = symbol * Math.Asin(X[i] / R) * 180 / Math.PI;
    //加工口径左边缘Z坐标
           // double Z_p = symbol * (Math.Pow(X_p, 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X_p, 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X_p) + A[1] * Math.Pow((X_p), 2) + A[2] * Math.Pow(Math.Abs(X_p), 3) + A[3] * Math.Pow((X_p), 4) + A[4] * Math.Pow(Math.Abs(X_p), 5) + A[5] * Math.Pow((X_p), 6) + A[6] * Math.Pow(Math.Abs(X_p), 7) + A[7] * Math.Pow((X_p), 8) + A[8] * Math.Pow(Math.Abs(X_p), 9) + A[9] * Math.Pow((X_p), 10) + A[10] * Math.Pow(Math.Abs(X_p), 11) + A[11] * Math.Pow((X_p), 12) + A[12] * Math.Pow(Math.Abs(X_p), 13) + A[13] * Math.Pow((X_p), 14) + A[14] * Math.Pow(Math.Abs(X_p), 15) + A[15] * Math.Pow((X_p), 16) + A[16] * Math.Pow(Math.Abs(X_p), 17) + A[17] * Math.Pow((X_p), 18) + A[18] * Math.Pow(Math.Abs(X_p), 19) + A[19] * Math.Pow((X_p), 20));
            double Z_p = -symbol * (Math.Sqrt((Math.Pow(R, 2) - Math.Pow(X_p, 2))) - R);

            //左圆角圆心X坐标
            double X_circleCenter = -Math.Abs(yuan_r * Math.Sin(B_p / 180 * Math.PI)) + X_p;
            // double tempP = Math.Asin((X_p - X_circleCenter) / yuan_r) * 180 / Math.PI;


            //左圆角圆心Z坐标
            double Z_circleCenter = B_p / Math.Abs(B_p) * yuan_r * Math.Cos(B_p) + Z_p;

            //左圆角圆心左边取一位小数位
          //  double R_circleCenter = Math.Round(Math.Abs(X_circleCenter), 1);
            double R_circleCenter = Math.Truncate((Math.Abs(X_circleCenter) * 10)) / 10;

            //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

            for (int i = 0; i < a + 1; i++)//B角度，
            {
                //ArrayList_X.Add(-D / 2 + i * 0.001);
                //double[] values = ArrayList_X.Cast<double>().ToArray();
                //double[] d = Convert.ToDouble(X[i]);
                //  Z1[i] = symbol * (Math.Pow(X[i], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
               



                if (i < (D_workpiece / 2 - R_circleCenter) / dist-1)//平面左边部分
                {
                    Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                    B[i] = 0;
                    Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值

                }
                if (i >=((D_workpiece / 2 - R_circleCenter) / dist-1) && i < (D_workpiece / 2 - Dp / 2) / dist)//左边圆角部分
                {
                    // Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                    Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (a / 2 - i) * dist, 2)));
                    B[i] = B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (a / 2 - i) * dist) / yuan_r) * 180 / Math.PI;
                    if (B[i] * B_p < 0)
                        B[i] = 0;
                    Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
                }
                if (i >= (D_workpiece / 2 - Dp / 2) / dist && i <= (D_workpiece / 2 + Dp / 2) / dist + 1)
                {
                    Z1[i] = -symbol * (Math.Sqrt(Math.Abs((Math.Pow(R, 2) - Math.Pow(X[i], 2)))) - R);
                    if (double.IsNaN(Z1[i]))
                    {
                        Z1[i] = Z1[i - 2];
                        //if()
                        //      MessageBox.Show("异常Z1N");
                        normal_flag = false;
                    }

                    Z[i] = H + Z1[i] + h;// -Z1[a - 1];

                 
                    if (X[i] > 0)
                    {
                        //Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                        // Z2[i]
                        B[i] = symbol * Math.Asin(X[i] / R) * 180 / Math.PI;

                    }
                    else if (X[i] == 0)
                    {
                        Z2[i] = 0;
                        B[i] = 0;
                    }
                    else
                    {
                        //Z2[i] = symbol * ((2 * X[i] * (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) + A[0] + A[1] * 2 * X[i] + A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) + A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) + A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) + A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) + A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) + A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) + A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) + A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) + A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));

                        // Z2[i] = symbol*((2 * X[i] * (Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) + Math.Pow(X[i], 3) * (K + 1) / Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2))) / Math.Pow(Math.Pow(R, 2) + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[i], 2)), 2) - A[0] + A[1] * 2 * X[i] - A[2] * 3 * Math.Pow(X[i], 2) + A[3] * 4 * Math.Pow(X[i], 3) - A[4] * 5 * Math.Pow(X[i], 4) + A[5] * 6 * Math.Pow(X[i], 5) - A[6] * 7 * Math.Pow(X[i], 6) + A[7] * 8 * Math.Pow(X[i], 7) - A[8] * 9 * Math.Pow(X[i], 8) + A[9] * 10 * Math.Pow(X[i], 9) - A[10] * 11 * Math.Pow(X[i], 10) + A[11] * 12 * Math.Pow(X[i], 11) - A[12] * 13 * Math.Pow(X[i], 12) + A[13] * 14 * Math.Pow(X[i], 13) - A[14] * 15 * Math.Pow(X[i], 14) + A[15] * 16 * Math.Pow(X[i], 15) - A[16] * 17 * Math.Pow(X[i], 16) + A[17] * 18 * Math.Pow(X[i], 17) - A[18] * 19 * Math.Pow(X[i], 18) + A[19] * 20 * Math.Pow(X[i], 19));
                        //B[i] = Math.Atan(Z2[i]) * 180 / Math.PI;
                        B[i] = symbol * Math.Asin(X[i] / R) * 180 / Math.PI;
                    }

                   
                }

                if (i > (D_workpiece / 2 + Dp / 2) / dist + 1 && i <= (D_workpiece / 2 + R_circleCenter) / dist)//右边圆角部分
                {


                    //  Z1[i] = Z[0] + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));
                    Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p + B_p / Math.Abs(B_p) * (yuan_r - Math.Sqrt(Math.Pow(yuan_r, 2) - Math.Pow(Math.Abs(X_circleCenter) - (i - a / 2) * dist, 2)));
                    B[i] = -B_p / Math.Abs(B_p) * Math.Asin((Math.Abs(X_circleCenter) - (i - a / 2) * dist) / yuan_r) * 180 / Math.PI;
                    if (B[i] * B_p > 0)
                        B[i] = 0;
                    Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值

                }

                if (i > (D_workpiece / 2 + R_circleCenter) / dist)//右边平面部分
                {
                    Z1[i] = -B_p / Math.Abs(B_p) * (yuan_r - yuan_r * Math.Cos(B_p / 180 * Math.PI)) + Z_p;
                    B[i] = 0;
                    Z[i] = H + Z1[i] + h;//原点变为转动点后的Z值
                }




          
            
            }

            for (int i = 0; i < a + 1; i++)//x移动
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                // X[i] = -D / 2 + i * 0.001;

                r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

                ZZ[i] = r0[i] * Math.Abs(Math.Cos(B[i] * Math.PI / 180 + Math.Asin(X[i] / r0[i])));//Z轴坐标
              
                //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
                X2[i] = r0[i] * Math.Cos(Math.Acos(X[i] / r0[i]) - B[i] * Math.PI / 180);//x移动坐标
                //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
                // dX=(r0*cos(dangle));
            }


            /*
                        Vs=35;%
                        Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                       x=abs(X_1);
                       Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                        Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                        %ff= Wc>500;%找出设定值大于500极限的数点
                       %Wc(ff)=500;
                       Nc=(30.*Wc)/pi;
                       ff= Nc>180;%找出设定值大于300极限的数点
                        Nc(ff)=180;
                        Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
             */
            for (int i = 0; i < a + 1; i++)//C转速
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * 153.1862 + 26.5573;

                if (vc_flag == true)
                    Nc[i] = constan_vc * 153.1862 + 26.5573;
            }

            for (int i = 0; i < a + 1; i++)//F1进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
                T[i] = dist / F1[i];

            }
            F[0] = 100;
            for (int i = 0; i < a; i++)//F进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F[i + 1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
                if (F_flag == true)
                    F[i] = contan_F;


            }
            //  F[a] = F[a - 1];
            for (int i = 0; i < a + 1; i++)//加工时间
            {
                t = T[i] + t;

            }
            t = t / 60;
            for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
            {
                if (Nc[i] > vc * 153.1862 + 26.5573)
                {
                    Nc[i] = vc * 153.1862 + 26.5573;

                }
            }


            /**************根据面型来做曲率补正系数****************/

            /************************读取TXT里面的F************/
            double[] F_store = new double[a / 2 + 1];
            double curvature_coefficient;
            //string filepath;
            //if (tool_r == 1)
            //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_1P.txt";
            //else if (tool_r == 5)
            //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_5P.txt";
            //else
            //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_3P.txt";


            //StreamReader readfile = new StreamReader(filepath, System.Text.Encoding.UTF8);


            for (int i = 0; i < a / 2 + 1; i++)
            {
                //double temp=0;
                //for (int j = 0; j < 100; j++)
                //{
                //    temp = temp + Math.Sqrt(1 + Math.Pow((Math.Pow(i * 0.1 + j * 0.001, 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(i * 0.1 + j * 0.001, 2))) + A[0] * Math.Abs(i * 0.1 + j * 0.001) + A[1] * Math.Pow((i * 0.1 + j * 0.001), 2) + A[2] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 3) + A[3] * Math.Pow((i * 0.1 + j * 0.001), 4) + A[4] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 5) + A[5] * Math.Pow((i * 0.1 + j * 0.001), 6) + A[6] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 7) + A[7] * Math.Pow((i * 0.1 + j * 0.001), 8) + A[8] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 9) + A[9] * Math.Pow((i * 0.1 + j * 0.001), 10) + A[10] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 11) + A[11] * Math.Pow((i * 0.1 + j * 0.001), 12) + A[12] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 13) + A[13] * Math.Pow((i * 0.1 + j * 0.001), 14) + A[14] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 15) + A[15] * Math.Pow((i * 0.1 + j * 0.001), 16) + A[16] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 17) + A[17] * Math.Pow((i * 0.1 + j * 0.001), 18) + A[18] * Math.Pow(Math.Abs(i * 0.1 + j * 0.001), 19) + A[19] * Math.Pow((i * 0.1 + j * 0.001), 20)), 2)) * 0.001;

                //}
                //curvature_coefficient = temp / 0.1;
                //curvature_coefficient = 1;
                double temp = 0, temp2 = 0;
                // for (int j = 0; j < 100; j++)
                // {
                //if(j<10)
                
                temp2 =  -symbol * (Math.Sqrt((Math.Pow(R, 2) - Math.Pow(X[i], 2))) - R);


                temp = -symbol * (Math.Sqrt((Math.Pow(R, 2) - Math.Pow(X[i]+0.01, 2))) - R);
                                 
                    
                temp = Math.Abs(temp - temp2);
                temp = Math.Sqrt(Math.Pow(temp, 2) + Math.Pow(0.01, 2));
                curvature_coefficient = temp / 0.01;
                if (double.IsInfinity(curvature_coefficient))
                    curvature_coefficient = 1;
                if (double.IsNaN(curvature_coefficient))
                    curvature_coefficient = 1;
                //curvature_coefficient = 1;
               // F_store[i] = Convert.ToDouble(readfile.ReadLine()) * curvature_coefficient;
                if (i > (Dp / 2) / dist)
                    curvature_coefficient = 1;
         
                if (tool_R == 1)
                    F_store[i] = flat_v_1p[i] * curvature_coefficient;
                else if (tool_R == 5)
                    F_store[i] = flat_v_5p[i] * curvature_coefficient;
                else if (tool_R == 7)//垂直抛
                    F_store[i] = flat_v_vertical[i] * curvature_coefficient;
                else
                    F_store[i] = flat_v_3p[i] * curvature_coefficient;

            }

            for (int i = 0; i < a; i++)//F进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);

                //  if(i<a/2)
                //  F[i + 1] = F_store[a / 2-i-1]; 
                //  else
                //      F[i + 1] = F_store[i-a / 2];
                if (i < a / 2)
                {
                    F[i + 1] = F_store[a / 2 - i - 1];
                    F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
                }
                else
                {
                    F[i + 1] = F_store[i - a / 2];

                    F[i + 1] = F[i + 1] * Math.Abs(X2[i + 1] - X2[i]) / (Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(Z1[i + 1] - Z1[i], 2)));
                }

                //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
              //  if (F_flag == true)
               //     F[i + 1] = contan_F;

                //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
                if (F_flag == true)
                    F[i + 1] = Math.Abs(X2[i + 1] - X2[i]) / (dist / contan_F);


            }

            /***************写入TXT********/
           
            double[,] b = new double[c + 1, 7];
            double[,] b_null = new double[c + 1, 7];
            for (int i = 0; i < c + 1; i++)
            {

                b[i, 0] = X2[i + d + a / 2];//x移动坐标
                b[i, 1] = B[i + d + a / 2];//B轴角度

                b[i, 2] = F[i + d + a / 2];//进给速度
                b[i, 3] = Nc[i + d + a / 2];//C轴转速

                b[i, 4] = X[i + d + a / 2];//X工件坐标
                b[i, 5] = ZZ[i + d + a / 2];//Z轴移动坐标
                b[i, 6] = Z1[i + d + a / 2];//函数曲线Z值
            }
            b[0, 2] = first_position_feed ;
            if (normal_flag == false)
            {
                MessageBox.Show("参数输入有误，数据异常，生成代码失败！");
                return b_null;
            }
            return b;
 
        }
       public double[,] plane(double C_motor_scale_factor,double C_motor_offset,double fixture_h, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double D, double R, double K, double[] A, out double t)
        {                        //,n:抛光次数，vc:工件最大转速，H:模仁到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间,fixture_h夹具高度
           // double h = fixture_h + 12.666 + 7.177 - 46.147;//h = 9.66,
            double h = fixture_h - 0.999;// 45.999 - 44.83;
            vc_flag = true;//C轴恒速，变速功能不要
            t = 0;
            // dist = 0.01;
            double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
            double Vs = Vu + 5;//U轴和C轴的线速度模

            int a = Convert.ToInt16(D / dist);
            double[] X = new double[a + 1];
            double[] Z = new double[a + 1];  //原点变为转动点后的Z值
            double[] Z1 = new double[a + 1];//函数曲线Z值
            double[] Z2 = new double[a + 1];//曲线的一阶导数
            double[] B = new double[a + 1];//B转动角度
            double[] Vc = new double[a + 1];//C线速度
            double[] Nc = new double[a + 1];//C轴转速
            double[] F = new double[a + 1]; //X轴进给速度
            double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
            double[] T = new double[a + 1];  //两点间时间
            double[] ZZ = new double[a + 1];//Z移动坐标
            double[] X2 = new double[a + 1];//x移动坐标
            double[] r0 = new double[a + 1];//点到转动中心的夹角
            //  double[] beta1 = new double[a];


            // ArrayList ArrayList_X = new ArrayList();
            //ArrayList ArrayList_B = new ArrayList();
            // ArrayList ArrayList_F = new ArrayList();
            // ArrayList ArrayList_C = new ArrayList();
            // ArrayList ArrayList_Z1 = new ArrayList();
            // ArrayList ArrayList_Z2 = new ArrayList();

            //arry_X[0]=-D/2+0.1;
            for (int i = 0; i < a + 1; i++)//x坐标
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                X[i] = -D / 2 + i * dist;
            }

            //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

            for (int i = 0; i < a + 1; i++)//B角度，
            {

                B[i] = 0;
              
            }

            for (int i = 0; i < a + 1; i++)//x移动
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                // X[i] = -D / 2 + i * 0.001;

                ZZ[i] = h+H;
                X2[i] = X[i];
                
            }


            /*
                        Vs=35;%
                        Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                       x=abs(X_1);
                       Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                        Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                        %ff= Wc>500;%找出设定值大于500极限的数点
                       %Wc(ff)=500;
                       Nc=(30.*Wc)/pi;
                       ff= Nc>180;%找出设定值大于300极限的数点
                        Nc(ff)=180;
                        Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
             */


          

            for (int i = 0; i < a + 1; i++)//C转速
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
                Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * 153.1862 + 26.5573;

              
                if (vc_flag == true)
                    Nc[i] = constan_vc * C_motor_scale_factor + C_motor_offset;

            }
          

            for (int i = 0; i < a + 1; i++)//F1进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
                T[i] = dist / F1[i];

            }
            F[0] = 10;
            for (int i = 0; i < a; i++)//F进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                F[i+1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
                if (F_flag == true)
                {
                    F[i] = contan_F;
                    F[a] = contan_F; 
                }
               

            }
           // F[a] = F[a - 1];
            for (int i = 0; i < a + 1; i++)//加工时间
            {
                t = T[i] + t;

            }
            t = t / 60;
            for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
            {
                if (Nc[i] >  constan_vc * C_motor_scale_factor + C_motor_offset)
                {
                   Nc[i] = constan_vc * C_motor_scale_factor + C_motor_offset;
                }
            }


            /***************写入TXT********/

            /*string result1 = @"C:\result1.txt";//结果保存到F:\result1.txt

            //先清空result1.txt文件内容/
            FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();


            FileStream fs = new FileStream(result1, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs);
            wr.WriteLine("OPEN PROG 2");
            wr.WriteLine("CLEAR");
            wr.WriteLine("G90 G01");
            wr.WriteLine("WHILE(P25<=" + Convert.ToString(n) + ")");
            for (int i = 0; i < a + 1; i++)
            {
                wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            }
            // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            // 
            // }          
            wr.WriteLine("ENDWHILE");
            if (n % 2 == 1)
            {
                for (int i = 0; i < a + 1; i++)
                {
                    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
                }

            }
            wr.WriteLine("P21=1");
            wr.WriteLine("CLOSE");
            wr.Close();
            //test = Z2[2];


            /***************写入TXT********/
            /*SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "";
            sfd.InitialDirectory = @".\模仁加工代码\";
            sfd.Filter = "文本文件| *.txt";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
             * 
                return null;
            }

           // string result1 = @".\GCode.txt";//结果保存到F:\result1.txt

            //*先清空result1.txt文件内容
            FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();


            FileStream fs = new FileStream(path, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs);
            wr.WriteLine("OPEN PROG 2");
            wr.WriteLine("CLEAR");
            wr.WriteLine("G90 G01");
            wr.WriteLine("WHILE(P25<="+Convert.ToString(n)+")");
            for (int i = 0; i < a+1; i++)
            {
                wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            }
           // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
          // 
           // }    
      
          wr.WriteLine("ENDWHILE");
          if (n % 2 == 1)
          {
              for (int i = 0; i < a + 1; i++)
              {
                  wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
              }

          }
            wr.WriteLine("P21=1");
            wr.WriteLine("CLOSE");
            wr.Close();*/
            double[,] b = new double[a + 1, 7];
            for (int i = 0; i < a + 1; i++)
            {
                b[i, 0] = X2[i];
                b[i, 1] = B[i];
                b[i, 2] = F[i];
                b[i, 3] = Nc[i];
                b[i, 4] = X[i];
                b[i, 5] = ZZ[i];
            }
            return b;
            //test = Z2[2];
        }

       public double[,] plane_heitian(double first_position_feed, double D_workpiece, double C_motor_scale_factor, double C_motor_offset, double Lworkpiece_h, double other_h, double R_P_right, double tool_R, double constan_vc, double contan_F, bool vc_flag, bool F_flag, double dist, double symbol, double n, double vc, double H, double R_P_left, double R, double K, double[] A, out double t)
       {                        //,n:抛光次数，vc:工件最大转速，H:模仁到平面高度；D:模仁口径，R,K,A为球面参数，t:加工时间
           //double h = fixture_h + 12.666 + 7.177 - 46.147;;//h = 9.66,
           //double h = fixture_h - 0.999;// 45.999 - 44.83;
           double h =  - Lworkpiece_h + other_h;//B轴旋转中心到模具平面高度
           vc_flag = true;//C轴恒速，变速功能不要
           t = 0;
           vc_flag = true;//C轴恒速，变速功能不要
           // dist = 0.01;
           double Vu = 19;//Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s
           double Vs = Vu + 5;//U轴和C轴的线速度模
           //D = Math.Truncate(D * 10) / 10;
           //if (D * 10 % 2 == 1)
           //    D = D + 0.1;
           //int a = Convert.ToInt16(D / dist);

           R_P_left = Math.Truncate(R_P_left * 10) / 10;
        //   Dp = Math.Truncate(Dp * 10) / 10;
           D_workpiece = Math.Truncate(D_workpiece * 10) / 10;

           // if (R_P_left * 10 % 2 == 1)
           //      R_P_left = R_P_left + 0.1;

          


           if (D_workpiece * 10 % 2 == 1)
               D_workpiece = D_workpiece - 0.1;

           int c = Convert.ToInt16((R_P_right - R_P_left) / dist);//输出代码行数-1
           int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
           int d = Convert.ToInt16(R_P_left / dist);
           int e = Convert.ToInt16(R_P_right / dist);
           double[] X = new double[a + 1];
           double[] Z = new double[a + 1];  //原点变为转动点后的Z值
           double[] Z1 = new double[a + 1];//函数曲线Z值
           double[] Z2 = new double[a + 1];//曲线的一阶导数
           double[] B = new double[a + 1];//B转动角度
           double[] Vc = new double[a + 1];//C线速度
           double[] Nc = new double[a + 1];//C轴转速
           double[] F = new double[a + 1]; //X轴进给速度
           double[] F1 = new double[a + 1]; //dist距离走一圈的进给速度
           double[] T = new double[a + 1];  //两点间时间
           double[] ZZ = new double[a + 1];//Z移动坐标
           double[] X2 = new double[a + 1];//x移动坐标
           double[] r0 = new double[a + 1];//点到转动中心的夹角
           //  double[] beta1 = new double[a];
           double[] flat_v = new double[] { 29.474, 28.685, 27.348, 25.973, 17.534, 12.548, 11.556, 10.321, 8.903, 8.097, 7.608, 6.85, 6.639, 6.087, 5.697, 5.493, 5.426, 5.249, 5.031, 4.889, 4.774, 4.742, 4.563, 4.622, 4.32, 4.326, 4.269, 4.196, 4.27, 4.117, 4.073, 4.027, 3.912, 3.875, 3.945, 3.868, 3.828, 3.721, 3.806, 3.77, 3.741, 3.621, 3.754, 3.592, 3.563, 3.629, 3.495, 3.533, 3.535, 3.557, 3.358, 3.403, 3.375, 3.466, 3.357, 3.388, 3.319, 3.256, 3.277, 3.136, 3.174, 3.136, 3.151, 3.13, 3.103, 3.096, 3.116, 3.031, 3.022, 2.958, 3.014, 3.017, 3.066, 3.076, 3.067, 3.057, 3.047, 3.037, 3.027, 3.017, 3.007, 2.997, 2.987, 2.977, 2.967, 2.957, 2.947, 2.937, 2.927, 2.917, 2.907, 2.897, 2.887, 2.877, 2.867, 2.857, 2.847, 2.837, 2.827, 2.817, 2.807, 2.797, 2.787, 2.777, 2.767, 2.757, 2.747, 2.737, 2.727, 2.717, 2.707, 2.697, 2.687, 2.677, 2.667, 2.657, 2.647, 2.637, 2.627, 2.617, 2.607, 2.597, 2.587, 2.577, 2.567, 2.557, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053,1.8027, 1.8002 ,1.7976 ,1.7950 ,1.7925 ,1.7899 ,1.7874 ,1.7848 ,1.7823 ,1.7797 ,1.7771 ,1.7746 ,1.7720 ,1.7695 ,1.7669 };//垂直抛平面的速度
        
           // double[] flat_v_vertical = new double[] { 29.474, 28.685, 27.348, 25.973, 17.534, 12.548, 11.556, 10.321, 8.903, 8.097, 7.608, 6.85, 6.639, 6.087, 5.697, 5.493, 5.426, 5.249, 5.031, 4.889, 4.774, 4.742, 4.563, 4.622, 4.32, 4.326, 4.269, 4.196, 4.27, 4.117, 4.073, 4.027, 3.912, 3.875, 3.945, 3.868, 3.828, 3.721, 3.806, 3.77, 3.741, 3.621, 3.754, 3.592, 3.563, 3.629, 3.495, 3.533, 3.535, 3.557, 3.358, 3.403, 3.375, 3.466, 3.357, 3.388, 3.319, 3.256, 3.277, 3.136, 3.174, 3.136, 3.151, 3.13, 3.103, 3.096, 3.116, 3.031, 3.022, 2.958, 3.014, 3.017, 3.066, 3.076, 3.067, 3.057, 3.047, 3.037, 3.027, 3.017, 3.007, 2.997, 2.987, 2.977, 2.967, 2.957, 2.947, 2.937, 2.927, 2.917, 2.907, 2.897, 2.887, 2.877, 2.867, 2.857, 2.847, 2.837, 2.827, 2.817, 2.807, 2.797, 2.787, 2.777, 2.767, 2.757, 2.747, 2.737, 2.727, 2.717, 2.707, 2.697, 2.687, 2.677, 2.667, 2.657, 2.647, 2.637, 2.627, 2.617, 2.607, 2.597, 2.587, 2.577, 2.567, 2.557, 2.5453, 2.5374, 2.5297, 2.5221, 2.5145, 2.507, 2.4996, 2.4922, 2.4849, 2.4777, 2.4706, 2.4635, 2.4565, 2.4495, 2.4426, 2.4358, 2.4291, 2.4224, 2.4157, 2.4092, 2.4026, 2.3962, 2.3898, 2.3834, 2.3771, 2.3709, 2.3647, 2.3586, 2.3525, 2.3465, 2.3405, 2.3346, 2.3287, 2.3229, 2.3171, 2.3114, 2.3057, 2.3001, 2.2945, 2.2889, 2.2834, 2.2779, 2.2725, 2.2671, 2.2618, 2.2565, 2.2513, 2.2461, 2.2409, 2.2357, 2.2306, 2.2256, 2.2206, 2.2156, 2.2106, 2.2057, 2.2009, 2.196, 2.1912, 2.1864, 2.1817, 2.177, 2.1723, 2.1677, 2.1631, 2.1585, 2.154, 2.1495, 2.145, 2.1406, 2.1361, 2.1318, 2.1274, 2.1231, 2.1188, 2.1145, 2.1103, 2.1061, 2.1019, 2.0977, 2.0925, 2.0875, 2.0834, 2.0794, 2.0753, 2.0713, 2.0674, 2.0634, 2.0595, 2.0556, 2.0517, 2.0479, 2.044, 2.0402, 2.0365, 2.0327, 2.029, 2.0252, 2.0216, 2.0179, 2.0142, 2.0106, 2.007, 2.0034, 1.9999, 1.9963, 1.9928, 1.9893, 1.9858, 1.9824, 1.9789, 1.9755, 1.9721, 1.9687, 1.9654, 1.962, 1.9587, 1.9554, 1.9521, 1.9488, 1.9456, 1.9423, 1.9391, 1.9359, 1.9328, 1.9296, 1.9264, 1.9233, 1.9202, 1.9171, 1.914, 1.911, 1.9079, 1.9049, 1.9019, 1.8989, 1.8959, 1.8929, 1.89, 1.887, 1.8841, 1.8812, 1.8783, 1.8754, 1.8725, 1.8697, 1.8669, 1.864, 1.8612, 1.8584, 1.8556, 1.8529, 1.8501, 1.8474, 1.8447, 1.8419, 1.8392, 1.8366, 1.8339, 1.8312, 1.8286, 1.8259, 1.8233, 1.8207, 1.8181, 1.8155, 1.8129, 1.8104, 1.8078, 1.8053, 1.8027, 1.8002, 1.7976, 1.7951, 1.7925, 1.79, 1.7874, 1.7849, 1.7823, 1.7798, 1.7772, 1.7747, 1.7721, 1.7696, 1.767 };
       
           // ArrayList ArrayList_X = new ArrayList();
           //ArrayList ArrayList_B = new ArrayList();
           // ArrayList ArrayList_F = new ArrayList();
           // ArrayList ArrayList_C = new ArrayList();
           // ArrayList ArrayList_Z1 = new ArrayList();
           // ArrayList ArrayList_Z2 = new ArrayList();

           //arry_X[0]=-D/2+0.1;
           for (int i = 0; i < a + 1; i++)//x坐标
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               X[i] = -D_workpiece / 2 + i * dist;
           }

           //  Z1[a-1] = -(Math.Pow(X[a-1], 2) / (R + Math.Sqrt(Math.Pow(R, 2) - (K + 1) * Math.Pow(X[a-1], 2))) + A[0] * Math.Abs(X[a-1]) + A[1] * Math.Pow(Math.Abs(X[a-1]), 2) + A[2] * Math.Pow(Math.Abs(X[a-1]), 3) + A[3] * Math.Pow(Math.Abs(X[a-1]), 4) + A[4] * Math.Pow(Math.Abs(X[a-1]), 5) + A[5] * Math.Pow(Math.Abs(X[a-1]), 6) + A[6] * Math.Pow(Math.Abs(X[a-1]), 7) + A[7] * Math.Pow(Math.Abs(X[a-1]), 8) + A[8] * Math.Pow(Math.Abs(X[a-1]), 9) + A[9] * Math.Pow(Math.Abs(X[a-1]), 10) + A[10] * Math.Pow(Math.Abs(X[a-1]), 11) + A[11] * Math.Pow(Math.Abs(X[a-1]), 12) + A[12] * Math.Pow(Math.Abs(X[a-1]), 13) + A[13] * Math.Pow(Math.Abs(X[a-1]), 14) + A[14] * Math.Pow(Math.Abs(X[a-1]), 15) + A[15] * Math.Pow(Math.Abs(X[a-1]), 16) + A[16] * Math.Pow(Math.Abs(X[a-1]), 17) + A[17] * Math.Pow(Math.Abs(X[a-1]), 18) + A[18] * Math.Pow(Math.Abs(X[a-1]), 19) + A[19] * Math.Pow(Math.Abs(X[a-1]), 20));

           for (int i = 0; i < a + 1; i++)//B角度，
           {

               B[i] = 0;



           }

           for (int i = 0; i < a + 1; i++)//x移动
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               // X[i] = -D / 2 + i * 0.001;

               ZZ[i] = h+H;
               X2[i] = X[i];

           }


           /*
                       Vs=35;%
                       Vu=20;%Vu=Wu.*Ru.*sin(U);抛光轴线速度 Vu大约等于20mm/s 
                      x=abs(X_1);
                      Vc=-Vu.*cos(alfa11)+sqrt(abs(Vu.^2.*cos(alfa11)-Vu.^2+Vs.^2));%工件旋转轴的线速度 注意开根号部分
                       Wc=abs(Vc./x);%工件轴实时线速度，x是工件实时半径
                       %ff= Wc>500;%找出设定值大于500极限的数点
                      %Wc(ff)=500;
                      Nc=(30.*Wc)/pi;
                      ff= Nc>180;%找出设定值大于300极限的数点
                       Nc(ff)=180;
                       Nc=Nc.*7.70988*20;%c轴比例系数8.274一转一分钟,20是减速比
            */

           //string C_filepath = System.Environment.CurrentDirectory + "\\C_SPEED.txt";
           //StreamReader C_readfile = new StreamReader(C_filepath, System.Text.Encoding.UTF8);

           //double[] C_store = new double[a / 2 + 1];//c轴转速

           //for (int i = 0; i < a / 2 + 1; i++)
           //{
           //    C_store[i] = Convert.ToDouble(C_readfile.ReadLine());
           //}

           for (int i = 0; i < a ; i++)//C转速
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               //Vc[i] = -Vu * Math.Cos(Z2[i]) + Math.Sqrt(Math.Abs(Math.Pow(Vu, 2) * Math.Cos(Z2[i]) - Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
             //  Vc[i] = Math.Sqrt(Math.Abs(-Math.Pow(Vu, 2) + Math.Pow(Vs, 2)));
             //  Nc[i] = Math.Abs(18 / X[i]) * 30 / Math.PI * 150.469 - 143.259;

              // Nc[i] =  * 150.469 - 143.259;
               //if (i < a / 2)
               //    Nc[i + 1] = C_store[a / 2 - i - 1] * 153.1862 + 26.5573;
               //else
               //    Nc[i + 1] = C_store[i - a / 2] * 153.1862 + 26.5573; 

               if (vc_flag == true)
               {
                   Nc[i] = constan_vc * C_motor_scale_factor + C_motor_offset;
                   Nc[a] = constan_vc * C_motor_scale_factor + C_motor_offset;
               }

           }
         //  C_readfile.Close();

           for (int i = 0; i < a + 1; i++)//F1进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F1[i] = Math.Abs(0.1 * Vc[i] / (3 * Math.PI * (X[i] + dist / 2)));
               T[i] = dist / F1[i];

           }
           F[0] = 200;
           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               F[i + 1] = Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i] = contan_F;


           }
           // F[a] = F[a - 1];
           for (int i = 0; i < a + 1; i++)//加工时间
           {
               t = T[i] + t;

           }
           t = t / 60;
           for (int i = 0; i < a + 1; i++)//替换c的超过最大速度值
           {
               if (Nc[i] > vc * C_motor_scale_factor +C_motor_offset)
               {
                   Nc[i] = vc * C_motor_scale_factor + C_motor_offset;
               }
           }

           /************************读取TXT里面的F************/
           double[] F_store = new double[a / 2 + 1];
           //string filepath;
           //if (tool_R == 1)
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_1P.txt";
           //else if (tool_R == 5)
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_5P.txt";
           //else
           //    filepath = System.Environment.CurrentDirectory + "\\Flat_V_3P.txt";


           //StreamReader readfile = new StreamReader(filepath, System.Text.Encoding.UTF8);


           for (int i = 0; i < a / 2 + 1; i++)
           {
             //  F_store[i] = Convert.ToDouble(readfile.ReadLine());
               F_store[i] = flat_v[i];
           }

           for (int i = 0; i < a; i++)//F进给速度
           {
               // ArrayList_X.Add(-D/2+i*0.001);
               //X[i] = -D / 2 + i * 0.001;
               // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
               //  T1=dist./F1;%工件的
               //  TT=sum(T1);
               if (i < a / 2)
                   F[i + 1] = F_store[a / 2 - i - 1];
               else
                   F[i + 1] = F_store[i - a / 2];


               //Math.Abs((X2[i + 1] - X2[i]) / T[i] * 60);
               if (F_flag == true)
                   F[i] = contan_F;


           }

           /***************写入TXT********/

           /***************写入TXT********/

          /* string result1 = @"C:\result1.txt";//结果保存到F:\result1.txt

           //先清空result1.txt文件内容/
           FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
           stream2.Seek(0, SeekOrigin.Begin);
           stream2.SetLength(0); //清空txt文件
           stream2.Close();


           FileStream fs = new FileStream(result1, FileMode.Append);
           // fs.Seek(0, SeekOrigin.Begin);
           // fs.SetLength(0);
           StreamWriter wr = null;

           wr = new StreamWriter(fs);
           wr.WriteLine("OPEN PROG 2");
           wr.WriteLine("CLEAR");
           wr.WriteLine("G90 G01");
           wr.WriteLine("WHILE(P25<=" + Convert.ToString(n) + ")");
           for (int i = 0; i < a + 1; i++)
           {
               wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
           }
           // for (int i = a-1; i >= 0; i--)
           //{
           //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
           // 
           // }          
           wr.WriteLine("ENDWHILE");
           if (n % 2 == 1)
           {
               for (int i = 0; i < a + 1; i++)
               {
                   wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
               }

           }
           wr.WriteLine("P21=1");
           wr.WriteLine("CLOSE");
           wr.Close();
           //test = Z2[2];


           /***************写入TXT********/
           /*SaveFileDialog sfd = new SaveFileDialog();
           sfd.Title = "";
           sfd.InitialDirectory = @".\模仁加工代码\";
           sfd.Filter = "文本文件| *.txt";
           sfd.ShowDialog();
           //sfd.FileName(textBox1.Text) ;
           string path = sfd.FileName;
           if (path == "")
           {
            * 
               return null;
           }

          // string result1 = @".\GCode.txt";//结果保存到F:\result1.txt

           //*先清空result1.txt文件内容
           FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
           stream2.Seek(0, SeekOrigin.Begin);
           stream2.SetLength(0); //清空txt文件
           stream2.Close();


           FileStream fs = new FileStream(path, FileMode.Append);
           // fs.Seek(0, SeekOrigin.Begin);
           // fs.SetLength(0);
           StreamWriter wr = null;

           wr = new StreamWriter(fs);
           wr.WriteLine("OPEN PROG 2");
           wr.WriteLine("CLEAR");
           wr.WriteLine("G90 G01");
           wr.WriteLine("WHILE(P25<="+Convert.ToString(n)+")");
           for (int i = 0; i < a+1; i++)
           {
               wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
           }
          // for (int i = a-1; i >= 0; i--)
           //{
           //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
         // 
          // }    
      
         wr.WriteLine("ENDWHILE");
         if (n % 2 == 1)
         {
             for (int i = 0; i < a + 1; i++)
             {
                 wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
             }

         }
           wr.WriteLine("P21=1");
           wr.WriteLine("CLOSE");
           wr.Close();*/
          /* double[,] b = new double[a + 1, 5];
           for (int i = 0; i < a + 1; i++)
           {
               b[i, 0] = X2[i];
               b[i, 1] = B[i];
               b[i, 2] = F[i];
               b[i, 3] = Nc[i];
               b[i, 4] = X[i];
           }
           return b;*/

           double[,] b = new double[c + 1, 7];
           for (int i = 0; i < c + 1; i++)
           {

               b[i, 0] = X2[i + d + a / 2];//x移动坐标
               b[i, 1] = B[i + d + a / 2];//B轴角度

               b[i, 2] = F[i + d + a / 2];//进给速度
               b[i, 3] = Nc[i + d + a / 2];//C轴转速

               b[i, 4] = X[i + d + a / 2];//X工件坐标
               b[i, 5] = ZZ[i + d + a / 2];//Z轴移动坐标
               b[i, 6] = Z1[i + d + a / 2];//函数曲线Z值
           }
           b[0, 2] = first_position_feed;
           return b;
           //if (D_end == 0)
           //{
           //    double[,] b = new double[a + 1, 7];
           //    for (int i = 0; i < a + 1; i++)
           //    {
           //        b[i, 0] = X2[i];
           //        b[i, 1] = B[i];

           //        b[i, 2] = F[i];
           //        b[i, 3] = Nc[i];

           //        b[i, 4] = X[i];
           //        b[i, 5] = ZZ[i];
           //    }
           //    return b;
           //}
           //else
           //{
           //    double[,] b = new double[a / 2 - Convert.ToInt16(D_end * 10) / 2+1, 7];
           //    for (int i = 0; i < a / 2 - Convert.ToInt16(D_end * 10) / 2+1; i++)
           //    {
           //        b[i, 0] = X2[i];
           //        b[i, 1] = B[i];

           //        b[i, 2] = F[i];
           //        b[i, 3] = Nc[i];

           //        b[i, 4] = X[i];
           //        b[i, 5] = ZZ[i];
           //    }
           //    return b;
           //}
           //test = Z2[2];
       }


        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }

            return ComputGauss(Guass, n);

        }

        public static double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }

        public static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];

            for (i = 0; i < n; i++) x[i] = 0.0;//初始化


            for (j = 0; j < n; j++)
            {
                max = 0;

                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }



                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;

                    }
                }

                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 

                    return x;
                }


                for (i = j + 1; i < n; i++)
                {

                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);

                    }
                }


            }//结束for (j=0;j<n;j++)


            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }

                x[i] = (Guass[i, n] - s) / Guass[i, i];

            }

            return x;
        }//返回值是函数的系数

        public void download_Gcode(double[] paramenter)
        {

                   
        }
        
       
      
    }
}
