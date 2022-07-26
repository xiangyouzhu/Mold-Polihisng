using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using PCOMMSERVERLib;
using System.IO;
using System.Collections;

namespace test
{
    public class PmacApi
    {
        private int m_nDevice;
        private PCOMMSERVERLib.IPmacDevice Pmac;
        private bool m_bDeviceOpen;
        
        public static int pmacNumber = 0;
       // private StreamReader _rstream = null;
       // private StreamWriter _wstream = null;

        public void reset_P()
        {
           // string command;
            string response;
            int status;

           

            Pmac.GetResponseEx(m_nDevice, "P15=0", false, out response, out status);
            Pmac.GetResponseEx(m_nDevice, "P14=0", false, out response, out status);
            Pmac.GetResponseEx(m_nDevice, "P20=0", false, out response, out status);
 
        }
        public PmacApi(int device)
        {
            this.m_nDevice = device;
           // try
            //{
            Pmac = new PmacDevice();
                /*
            }
            catch(Exception Ex)
            {
                Trace.WriteLine("初始化异常");

            }
            */
        }

        /// <summary>
        /// 打开控制卡
        /// </summary>
        /// <returns></returns>
        public bool Connect()//连接
        {
            Pmac.Open(m_nDevice, out m_bDeviceOpen);
            return m_bDeviceOpen;
        }

        /// <summary>
        /// 断开卡连接
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            Pmac.Close(m_nDevice);
            return m_bDeviceOpen;
        }

        /*
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="name"></param>
        public  void RunProgram(string name)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "b" + name + " R";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

        }
        */

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="name"></param>
        public void RunProgram(int coordinatesystem,string name)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "&"+coordinatesystem.ToString()+" b" + name + " R";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

        }

        /// <summary>
        /// 停止程序
        /// </summary>
        /// <param name="name"></param>
        public void StopProgram(string name)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "b" + name + " a";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// 停止运动
        /// </summary>
        /// <param name="nAxis"></param>
        public void StopAxis(int nAxis)//急停
        {
            //发送命令
            string command;
            string response;
            int status;

          //  command = "#"+nAxis.ToString()+"j/";
            command = "#"+nAxis.ToString()+"k";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// 闭环
        /// </summary>
        /// <param name="nAxis"></param>
        public void ClosedLoop(int nAxis)//停止
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "#" + nAxis.ToString() + "j/";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetM(int id)//获得M的值
        {
            //得到参数值
            string command;
            string response;
            int status;

            command = "M" + id.ToString();
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            if (response != "")
            {
                response = response.TrimEnd();

                return Convert.ToInt32(response);
            }
            else
            {
                return 0;
            }
        }

        public int GetI(int id)//获得M的值
        {
            //得到参数值
            string command;
            string response;
            int status;

            command = "I" + id.ToString();
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            if (response != "")
            {
                response = response.TrimEnd();

                return Convert.ToInt32(response);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
     
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetP(int id)   /// 得到P变量的值
        {
            //得到参数值
            string command;
            string response;
            int status;

            command = "P" + id.ToString();
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

            if (response != "")
            {
                response = response.TrimEnd();

                return double.Parse(response); //测试一下可能有问题
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 设置P变量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetP(int id, double value) /// 设置P变量
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "P" + id.ToString() + "=" + value.ToString();

            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

        }

        /// <summary>
        /// 设置M变量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetM(int id, int value)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "M" + id.ToString() + "=" + value.ToString();

            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

        }

        /// <summary>
        /// JOG+
        /// </summary>
        /// <param name="nAxis"></param>
        public void JogForward(int nAxis)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "#" + nAxis.ToString() + "j+";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// JOG-
        /// </summary>
        /// <param name="nAxis"></param>
        public void JogBackward(int nAxis)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "#" + nAxis.ToString() + "j-";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// 寸动
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="step"></param>
        public void JogStep(int nAxis,int step)
        {
            //发送命令
            string command;
            string response;
            int status;
            command = "#" + nAxis.ToString() + "j:"+step.ToString();
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="nAxis"></param>
        /// <param name="speed"></param>
        public void SetVel(int nAxis, double speed)
        {
            //发送命令
            string command;
            string response;
            int status;

            command = "I" + nAxis.ToString()+"22" + "=" + Convert.ToString(speed);
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }


        /// <summary>
        /// 获取当前位置
        /// </summary>
        /// <param name="nAxis"></param>
        /// <returns></returns>
        public bool GetPos(int nAxis, out double pos) /// 每个电机的置换量可能不一样
        {
            //得到参数值
            string command1, command2;
            string response1, response2;
            int status;

            command1 = "M" + nAxis.ToString() + "62";
            command2 = "M" + nAxis.ToString() + "69";
            Pmac.GetResponseEx(m_nDevice, command1, false, out response1, out status);
            Pmac.GetResponseEx(m_nDevice, command2, false, out response2, out status);
            //response
            if (response1 != "")
            {
                response1 = response1.TrimEnd();
                response2 = response2.TrimEnd();
                pos = double.Parse(response1) / 96 / 32+double.Parse(response2) / 96 / 32;
             //   return double.Parse(response) / 96 / 32;
                return true;
            }
            else
            {
                pos = 0;
              //  return 0;
                return false;
            }
        }

        public bool GetPosAngle(int nAxis, out double pos)
        {
            //得到参数值d
            string command;
            string response;
            int status;

            command = "M" + nAxis.ToString() + "62";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            if (response != "")
            {
                response = response.TrimEnd();
                pos = double.Parse(response) / 96 / 32;
                pos = pos / (3276800 / 360);
                return true;
            }
            else
            {
                pos = 0;
                //  return 0;
                return false;
            }

        }

        /// <summary>
        /// 下载程序，path为程序名称
        /// </summary>
        /// <param name="path"></param>
        public void Downloadprograme(string path)
        {
            bool success;
            Pmac.Download(m_nDevice, path, true, true, false, true, out success);
            if (!success)
            {
                MessageBox.Show("Download programmed failed!");
            }
        }

        /// <summary>
        /// 退出卡连接
        /// </summary>
        public void AbortPmac()
        {
            Pmac.Abort(m_nDevice);
        }

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="Axis_A"></param>
        /// <param name="Axis_B"></param>
        public void GotoHome(int Axis_A,int Axis_B)
        {
             //得到参数值
            string command;
            string response;
            int status;

            command = "#" + Axis_A.ToString() + "HOME";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

            command = "#" + Axis_B.ToString() + "HOME";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        public string SendCommand(string command)
        {
            string response;
            int status;

            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            return response;
        }

        /// <summary>
        /// 检测到位信号
        /// </summary>
        /// <param name="nAxis"></param>
        /// <returns></returns>
        public int GetInPos(int nAxis)
        {
            //得到参数值
            string command;
            string response;
            int status;

            command = "M" + nAxis.ToString() + "37";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            response = response.TrimEnd();

            return Convert.ToInt32(response);
        }

        public void SavePos()
        {
            //发送命令
            string command;
            string response;
            int status;
            command = "save";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        public void Reset()
        {
            //发送命令
            string command;
            string response;
            int status;
            command = "$$$";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        public void PmacCommand(string command)
        {
            string response;
            int status;
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
            
        }

        public int IsPauseState()
        {

            //得到参数值
            string command;
            string response;
            int status;

            command = "%";
            Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);

            response = response.TrimEnd();

            return Convert.ToInt32(response);
        }
        public void ReadTxtToList(ListBox G代码,string spath1,string spath2)
        {
            StreamReader _rstream = null;//为Gcode所创建的流
            //StreamWriter _wstream = null;
            StreamReader _rstream2 = null;//为运动程序前的设置开辟缓存区等代码所创建的流
            _rstream = new StreamReader(spath1, System.Text.Encoding.UTF8);
            _rstream2 = new StreamReader(spath2, System.Text.Encoding.UTF8);

            string line;
            while ((line = _rstream2.ReadLine()) != null)//读取到listbox
            {
                G代码.Items.Add(line);
            }
            while((line = _rstream.ReadLine())!=null )
            {
                G代码.Items.Add(line);//读取到listbox
            }
            _rstream.Close();//关闭流
            _rstream2.Close();

            //将listbox:G代码存入一个txt中   
          /*  FileStream fs = new FileStream(@".\finalgcode.txt", FileMode.Create);
            string str3 = System.Windows.Forms.Application.StartupPath;   //获取启动了应用程序的可执行文件的路径，不包括可执行文件的名称
            finalspath = str3+@"/finalgcode.txt";
            StreamWriter sw = new StreamWriter(fs);//创建流
            int iCount = G代码.Items.Count - 1;
            for (int i = 0; i <= iCount; i++)//将listbox中的文件不断写入txt
            {
                sw.WriteLine(G代码.Items[i].ToString());
            }
                sw.Close();//关闭流*/
            
                
        }
         public void reverseGcode(string spath3,out string spath5,out string spath6,out int n, out string t )//
       {/********************先读取文件************************/
           ArrayList list1 = new ArrayList();//创建一个arraylis对象
           StreamReader _rstream = null;
           _rstream = new StreamReader(spath3, System.Text.Encoding.UTF8);
           string line;
           while ((line = _rstream.ReadLine())!= null)
             {
                 list1.Add(line);
             }
           _rstream.Close();//对流的必要关闭操作
        /*****************读取操作结束****************************/

      
            FileStream fs1 = File.Open(@".\addgcode.txt", FileMode.Create);// 从第三行开始读取的 运动至初始位置的GCODE
            StreamWriter _wstream1 = new StreamWriter(fs1,System.Text.Encoding.UTF8);
            _wstream1.WriteLine("CLOSE");
            _wstream1.WriteLine("OPEN PROG 5 CLEAR");
            _wstream1.WriteLine("&1");
            _wstream1.WriteLine("#1->1000X");
            _wstream1.WriteLine("M1=0");
            _wstream1.WriteLine(list1[2]);
            _wstream1.WriteLine("M1=1");            
            _wstream1.Close();
          //  string spath4 = @".\addgcode.txt";
           
             n = Convert.ToInt16(list1[0]);//将加工次数定义为t
             t = Convert.ToString(list1[1]);//将加工时间定义为n
             FileStream fs2 = File.Open(@".\gcode1.txt", FileMode.Create);//创建一个流存放顺序加工时的GCODE
             spath5 = @".\gcode1.txt";
             StreamWriter _wstream2 = new StreamWriter(fs2,System.Text.Encoding.UTF8);
             _wstream2 = new StreamWriter(fs2);

             FileStream fs3 = File.Open(@".\gcode2.txt", FileMode.Create);//创建一个路径存放逆序加工的Gcode）
             spath6 = @".\gcode2.txt";
             StreamWriter _wstream3 = new StreamWriter(fs3, System.Text.Encoding.UTF8);
             _wstream3 = new StreamWriter(fs3);
             /********************对G代码进行转化，写入文件****************/          
             _wstream2.WriteLine("CLOSE");
             _wstream2.WriteLine("OPEN PROG 6 CLEAR");
             _wstream2.WriteLine("&1");
             _wstream2.WriteLine("#1->1000X");
             _wstream2.WriteLine("M1=0");
            for (int i = 3; i <list1.Count-1; i++)//从第4行开始读取，第三行为正式加工Gcode，读取到倒数第二行,为M99上一行
            {
                _wstream2.WriteLine(list1[i]);
            }
            _wstream2.WriteLine("M1=1");
            list1.Reverse();
           
            _wstream3.WriteLine("CLOSE");
            _wstream3.WriteLine("OPEN PROG 7 CLEAR");
            _wstream3.WriteLine("&1");
            _wstream3.WriteLine("#1->1000X");
            _wstream3.WriteLine("M1=0");
            for (int i = 1; i < list1.Count - 3; i++)//转换后从第2行开始读取，第一行为M99，第二行为上一次读取时最后一行，
            {
                _wstream3.WriteLine(list1[i]);
            }
            _wstream3.WriteLine("M1=1");
            list1.Reverse();
            /******************存在问题 在转换后的最后一行不是加工初始点 且未包含M99*************************/
             _wstream2.Close();
             _wstream3.Close();
             fs3.Close();
             fs2.Close();
             fs1.Close();

       }
        /* public void mview(object n)         
         {
            string command;
            string response;
            int status;
            int  m =(object)  n;
            command  = "M1";
            while (m == 0)
            {
                Pmac.GetResponseEx(m_nDevice, command, false, out response, out status);
                m = status;
            }
            return;
           


         }*/
         public static bool Delay(int delayTime)
         {
             DateTime now = DateTime.Now;
             int s;
             do
             {
                 TimeSpan spand = DateTime.Now - now;
                 s = spand.Seconds;
                 Application.DoEvents();
             }
             while (s < delayTime);
             return true;
         }
       

    }
}
