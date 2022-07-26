using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POLISH2;
using System.Collections;


namespace POLISH2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//干涉检查计算
        {
          // POLISH2.Form1 F1 =  new  POLISH2.Form1 ;

            double []  ShapeParameter= Form1.intefere_check_paramenter;//f非球面参数

            double symbol = ShapeParameter[0];
            double RR = ShapeParameter[5];
            double C = 1 / RR;
                  double KK=ShapeParameter[6];
            double[] A = Form1.intere_A;//f非球面参数
            bool interfer_flag=false;//、干涉标志
            double R=Convert.ToDouble(this.textBox50.Text.Trim());
            int COUNT = Convert.ToInt16(R / 0.01);//间隔数
            int toolLength = 90;//9mm的杆
           //  ArrayList x_list,temp;

            List<double> X=new List<double>();//模具X坐标
            List<int> X2 = new List<int>();//模具X2坐标
            List<double> Zd = new List<double>();//模具Z坐标
             List<double> RHO = new List<double>();//
             List<double> THETA = new List<double>();//
             List<double> Z = new List<double>();//
             List<double> X_ROT = new List<double>();//旋转后X
             List<double> Y_ROT = new List<double>();//旋转后Y
             List<double> Z_ROT = new List<double>();//旋转后Z
             double turn_angle = Convert.ToDouble(this.textBox1.Text.Trim()) / 180 * Math.PI;//旋转角度,弧度制
            int Workpiece_count=Convert.ToInt16(ShapeParameter[2]/0.01/2);//

            double Y_MOVE = R * Math.Sin(turn_angle );//旋转后移动Y坐标
            double Z_MOVE = R - R * Math.Cos(turn_angle);//旋转后移动Z坐标
            // double[,] RHO = new double[COUNT, COUNT];
            // double[,]THETA = new double[COUNT, COUNT];
            this.button1.Text = "干涉检查中...";

            for (int i = 0; i < Workpiece_count;i++ )//工件X坐标
            {
                X.Add(i * 0.01);

            }

            for (int i = 0; i < Workpiece_count; i++)//工件X坐标标号
            {
                X2.Add(i);

            }

            for (int i = 0; i < Workpiece_count; i++)//工件z坐标
            {
               
              //   double Z1 = symbol * (Math.Pow(X[i], 2) / (RR + Math.Sqrt(Math.Pow(RR, 2) - (KK + 1) * Math.Pow(X[i], 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
                double Z1 =symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (KK + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
               
               

                Zd.Add(Z1);

            }


             for (int j = 0; j < COUNT; j++)//RHO矩阵,抛光工具坐标
             {
                // temp.Clear();
                 for (int i = 0; i <COUNT; i++)
                 {
                     RHO.Add(i * 0.01);
                   //  temp.push_back(i * 0.01);
                 }
               //  RHO.Add(temp);

             }

            

             for (int j = 0; j <COUNT; j++)//THETA矩阵，抛光工具坐标
             {
                // temp.Clear();
                 for (int i = 0; i < COUNT; i++)
                 {
                     THETA.Add(0 + 2 * (Math.PI) / (COUNT - 1) * j);
                    // temp.push_back(0 + 2 * (M_PI) / (RHO.size() - 1) * j);
                 }
               //  THETA.push_back(temp);

             }

             for (int j = 0; j < COUNT; j++)//Z矩阵，抛光工具坐标
             {
                 //temp.clear();
                 for (int i = 0; i < COUNT; i++)
                 {
                     //radius-qSqrt(
                    // if (tool_type == 0)//球头
                    // double a=Math.Pow(R, 2) - Math.Pow(RHO[(j + 1) * i + i] * Math.Cos(THETA[(j + 1) * i + i]), 2) - Math.Pow(RHO[(j + 1) * i + i] * Math.Sin(THETA[(j + 1) * i + i]), 2);
                     Z.Add(R - Math.Sqrt(Math.Abs(Math.Pow(R, 2) - Math.Pow(RHO[j * COUNT + i] * Math.Cos(THETA[j * COUNT + i]), 2) - Math.Pow(RHO[j * COUNT + i] * Math.Sin(THETA[j * COUNT + i]), 2))));
                   //  Z.Add(R - Math.Sqrt(Math.Abs(Math.Pow(R, 2) - Math.Pow(RHO.at(j).at(i) * qCos(THETA.at(j).at(i)), 2) - qPow(RHO.at(j).at(i) * qSin(THETA.at(j).at(i)), 2))));

                     
                     // double a= RHO[(j+1)*i+i];
                 }
                

             }



             for (int j = 0; j < toolLength; j++)//RHO矩阵增加圆柱杆部分
             {
                //temp.clear();
                 for (int i = 0; i < COUNT; i++)
                 {


                     RHO.Add(R);

                 }
               //  RHO.push_back(temp);

             }

             for (int j = 0; j < toolLength; j++)//THETA矩阵增加圆柱杆部分
             {

                 for (int i = 0; i < COUNT; i++)
                 {

                    THETA.Add(0 + 2 * (Math.PI) / (COUNT - 1) * i);
                 }
               //  THETA.push_back(temp);

             }

             for (int j = 0; j < toolLength; j++)//Z矩阵增加圆柱杆部分
             {
                // temp.clear();
                 for (int i = 0; i < COUNT; i++)
                 {
                     //radius-qSqrt(
                //   if (tool_type == 0)//球头
                         Z.Add(R + (j + 1) * 0.1);
                   
                 }
               //  Z.push_back(temp);

             }

//旋转平移

            //旋转平移后X值
             X_ROT.Clear();
             for (int i = 0; i < RHO.Count; i++)
             {
                 X_ROT.Add(RHO[i] * Math.Cos(THETA[i]));
             }


             //旋转平移后Y值
            Y_ROT.Clear();
            for (int i = 0; i < RHO.Count; i++)
             {
                 Y_ROT.Add(RHO[i] * Math.Sin(THETA[i])*Math.Cos(turn_angle)-Z[i]*Math.Sin(turn_angle)-Y_MOVE);
             }

             //旋转平移后Z值
             Z_ROT.Clear();
             for (int i = 0; i < RHO.Count; i++)
             {
                 Z_ROT.Add(RHO[i] * Math.Sin(THETA[i])*Math.Sin(turn_angle)+Z[i]*Math.Cos(turn_angle)+Z_MOVE);
             }           
        
       //对比是否干涉

             int inerfer_nuber=0;
             for (int i = 0; i < RHO.Count; i++)
             {
                 int rho = Convert.ToInt16(Math.Sqrt(Math.Pow(X_ROT[i], 2) + Math.Pow(Y_ROT[i], 2))*100);//对比位置
                 if (rho > Zd.Count - 1)
                     rho = Zd.Count - 1;

                 if (Z_ROT[i] - Zd[rho] < -0.4)
                 {
                     interfer_flag = true;//干涉
                     inerfer_nuber++;
                   //  MessageBox.Show("干涉！" + (Z_ROT[i]).ToString()+"-"+(Zd[rho]).ToString());
                 }

             }


             this.button1.Text = "干涉校验计算";

             if (interfer_flag == true)
                 MessageBox.Show("抛光头与模具干涉，减小抛头尺寸或者调整抛光头角度！");
             else
                 MessageBox.Show("校验完成，抛光头与模具不干涉！");


            
        }

        private void label138_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.textBox50.Text = "1.5";
            this.textBox1.Text = "45";
        }
    }
}
