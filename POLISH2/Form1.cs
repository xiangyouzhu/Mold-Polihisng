using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using test;
using PCOMMSERVERLib;
using polishing;
using System.Threading;
using ZedGraph;
using System.IO.Ports;
using Client;
using POLISH2.Properties;
using System.Data.OleDb;
using System.Diagnostics;
using System.Security.Cryptography;//加密




namespace POLISH2
{
    public partial class Form1 : Form
    {
        

        // public int symbol;//ao凹凸



       public static PCOMMSERVERLib.PmacDeviceClass PMAC;
     //   public static PCOMMSERVERLib.IPmacDevice PMAC;
        public String speed;
        public static bool selectPmacSuccess = false;
        public static bool openPmacSuccess = false;
        public static int pmacNumber = 0;
        public static double[] parameter_shape;
        public static double[] parameter_process;
        public static double[] x, zd;//面型补正数据
        public static double[,] NC_Data;//生成代码数据
        public static double[] F1;//补偿后的F
        public static double[] rate_dist;//进给速度变化比例
        public static double x_devia_value, b_devia_value,z_devia_value;//定义轴的偏移量,单位脉冲
        public static double x_devia_ADDvalue, b_devia_ADDvalue, z_devia_ADDvalue;//定义轴的偏移量改变值,单位脉冲
        public static bool draw_flag;//画过一次后标志
        public static bool set_flag=false,set_proces_flag=false;//设置过一次参数后标志
        public static bool own_code_flag, compensate_code_flag, other_code_flag;//判断下载了什么代码
        public static double process_time;//加工时间
        public static bool download_code_flag;//下载代码进控制器标志
        int hour, min, Sec, h, m, s;
        double compens_cycle_time;//补偿加工次数
        public static bool process_fininsh_flag, process_begin_flag = false, Z_motor_status, C_motor_status, X_motor_status, B_motor_status;
        public static  bool Vc_flag,F_flag;//确定是否是恒转速和恒进给速度
        public static bool connect_flag;//连接控制器标志
        public static bool Pcode_return_flag;//进入生成代码界面标志
        public static bool handle_model_flag=false;//进入手动界面标志
        public static bool read_data_flag=false;//读取面型数据标志
        public static bool compensate_flag = false;//进行面型补正后的
        public static bool process_flag = false;//进入加工模式一次标志
        public static bool multi_process_flag = false;//进入多段程序设定界面一次标志
        public static bool multi_process_flag2 = false;//从自己代码设定进入多段程序设定界面一次标志
        public static bool X_ALARM_flag = false;//X轴警报标志
        public static bool B_ALARM_flag = false;//B轴警报标志
        public static bool Z_ALARM_flag = false;//Z轴警报标志
        public static bool C_ALARM_flag = false;//C轴警报标志
        public static bool Alarm_clear_flag = false;//异常清除按钮清除标志
        static public Form1 frmForm1;
        private PmacApi Pmac = new PmacApi(pmacNumber);
        private static SerialPort comm = new SerialPort();//建立串口通讯，模拟量输出板控制抛光轴转动
        public static bool buzzer_work = false;//蜂鸣器响
        public static int buzzer_worktime = 0;//蜂鸣器响的次数
        public static bool resetC_work = false;//C轴清楚警报
        public static int resetC_worktime = 0;//发生警报清楚电平次数
        public static double Lworkpiece_h;//为L型件底面到中心孔高度
        public static double other_h;//L型件底面到夹具底面高度
        public static double C_motor_scale_factor;//C轴电压转速比例因子
        public static double C_motor_offset;//C轴电压转速比例一次变换偏差      
         public static double U_motor_scale_factor;//抛光轴电压转速比例因子
        public static double U_motor_offset;//抛光轴电压转速比例一次变换偏差
        public static double zfeed;//多段抛光Z轴上升和下降速度
        public static double xBfeed;//多段抛光XB轴速度
        public static double xlimit;//X轴极限位置，左右对称
        public static double blimit;//B轴极限位置，左右对称
        public static double zlimit_up;//Z轴上极限位置
        public static double zlimit_down;//Z轴上极限位置
        public static double min_z;//数控代码Z的最小值
        public static double polish_axi_maxfeed;//抛光轴最大速度
        public static double polish_axi_minfeed;//抛光轴最小速度
        public static double x_axi_hand_maxfeed;//X轴手动最大速度
        public static double b_axi_hand_maxfeed;//b轴手动最大速度
        public static double z_axi_hand_maxfeed;//z轴手动最大速度
        public static double C_axi_maxfeed;//C轴最大速度
        public static double C_axi_minspeed;//C轴最小速度
        public static double C_Button_speed;//C轴最小速度
        public static double caculate_time_offset;//计算时间补正系数，单位/min
        public static double first_position_feed;//单段抛X B 到初始位进给速度（mm/min）
        public static double first_polish_zfeed;//抛光z到初始位速度
        public static double x_software_limit_left;//x左软限位
        public static double x_software_limit_right;//x右软限位
        public static double b_software_limit_left;//b左软限位
        public static double b_software_limit_right;//b右软限位
        public static double z_software_limit_up;//z上软限位
        public static double z_software_limit_down;//z下软限位
        public static double z_axi_first_jogdownfeed;////抛光Z轴下降手动速度；
        public static double B_axi_Center_Polish_head_distance;//在零位时，B轴回转中心到抛光头触碰到第二个限位的距离；
        public static double[] intefere_check_paramenter;//干涉检查传入模具参数参数
        public static double[] intere_A;///干涉检查传入模具参数参数
        public static List<double> Compesent_rate;//速度倍率补偿文件
        public static bool input_compesentfile_flag;//导入倍率件标志
       // public static double 
       //  public System.Threading.Thread dectect;

        Form2 form2;
        Form3 form3;


        public Form1()
        {
            InitializeComponent();
            form2 = new Form2();
            form2.Show();            
            PMAC = new PmacDeviceClass();
            this.pictureBox4.Image = System.Drawing.Bitmap.FromFile(@".\公式.png");
            timer1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
               {
                    if (process.MainModule.FileName
                    == current.MainModule.FileName)
                    {
                        MessageBox.Show("程序已经运行！", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        System.Environment.Exit(0); 
                    }
                }
            }

            try
            {
                string file = "C:\\Windows\\setupad.txt";
                StreamReader reader = new StreamReader(file, System.Text.Encoding.UTF8);//null;//new StreamReader();
                // myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                string permit = reader.ReadLine();
                reader.Close();
                if (permit != "SL7762DELTA538")
                {
                   // MessageBox.Show("软件未激活！","提示");
                    System.Environment.Exit(0);
                }
            }
            catch
            {
               // MessageBox.Show("软件未激活！","提示");
                System.Environment.Exit(0);
            }
            /******通讯******/

            //根据屏幕大小设置窗体初始大小
            //Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            Rectangle rect = SystemInformation.WorkingArea;
            this.Height = rect.Height;
            this.Width = rect.Width;
            //根据屏幕大小设置窗体最大化大小
            this.MaximizedBounds = new Rectangle(rect.X, rect.Y, rect.Width , rect.Height );
            this.panel1.Visible = true;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
           // string status;
           //  PMAC.SelectDevice(0, out pmacNumber, out selectPmacSuccess);
            pmacNumber = 0;
           // if (selectPmacSuccess == true)
           //   {
           // PMAC.SelectDevice(0, out pmacNumber, out selectPmacSuccess);
         //   if (selectPmacSuccess)
         // {

              PMAC.Open(pmacNumber, out openPmacSuccess);
               
          //  }   
              #region
              /**********************************读取EXCLE系统参数**************************************************/

             //   #region
               //    string file2 = System.Environment.CurrentDirectory + "\\machine_parameter.txt";
               //  StreamReader read = new StreamReader(file2, System.Text.Encoding.UTF8);
               //   Lworkpiece_h = Convert.ToDouble(read.ReadLine().Trim());
               //  other_h = Convert.ToDouble(read.ReadLine().Trim());

               //C_motor_scale_factor = Convert.ToDouble(read.ReadLine().Trim());//C轴电压转速比例因子
               //C_motor_offset = Convert.ToDouble(read.ReadLine().Trim());//C轴电压转速比例一次变换偏差      
               //U_motor_scale_factor = Convert.ToDouble(read.ReadLine().Trim());//抛光轴电压转速比例因子
               //U_motor_offset = Convert.ToDouble(read.ReadLine().Trim());//抛光轴电压转速比例一次变换偏差
               //zfeed = Convert.ToDouble(read.ReadLine().Trim());//多段抛光Z轴上升和下降速度
               //xBfeed = Convert.ToDouble(read.ReadLine().Trim());//多段抛光XB轴到初始位速度

            string strConn;
            string file_name;
            file_name = "system_value.xlsx";
            // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + System.Environment.CurrentDirectory + "\\data.xlsx" + ";Extended Properties=Excel 8.0;";
            strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + System.Environment.CurrentDirectory + "\\" + file_name + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
            if (Path.GetExtension(System.Environment.CurrentDirectory + "\\" + file_name).Trim().ToUpper() == ".XLSX")
            {
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.Environment.CurrentDirectory + "\\" + file_name + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
            }
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            DataSet myDataSet = new DataSet();
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            try
            {
                myCommand.Fill(myDataSet);



                Lworkpiece_h = Convert.ToDouble(myDataSet.Tables[0].Rows[0][1]); //为L型件底面到中心孔高度，单位mm/
                other_h = Convert.ToDouble(myDataSet.Tables[0].Rows[1][1]);//L型件底面到夹具底面高度，单位mm/
                C_motor_scale_factor = Convert.ToDouble(myDataSet.Tables[0].Rows[2][1]);//C轴电压转速比例因子
                C_motor_offset = Convert.ToDouble(myDataSet.Tables[0].Rows[3][1]);//C轴电压转速比例一次变换偏差      
                U_motor_scale_factor = Convert.ToDouble(myDataSet.Tables[0].Rows[4][1]);//抛光轴电压转速比例因子
                U_motor_offset = Convert.ToDouble(myDataSet.Tables[0].Rows[5][1]);//抛光轴电压转速比例一次变换偏差
                zfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[6][1]);//多段抛光Z轴上升和下降速度，单位mm/分钟
                xBfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[7][1]);//多段抛光XB轴到初始位速度，单位mm/分钟
                xlimit = Convert.ToDouble(myDataSet.Tables[0].Rows[8][1]);//X轴极限位置，左右对称，单位mm
                blimit = Convert.ToDouble(myDataSet.Tables[0].Rows[9][1]);//B轴极限位置，左右对称，单位度

                zlimit_up = Convert.ToDouble(myDataSet.Tables[0].Rows[10][1]);//Z轴上极限位置，单位mm/
                zlimit_down = Convert.ToDouble(myDataSet.Tables[0].Rows[11][1]);//Z轴下极限位置，单位mm/
                min_z = Convert.ToDouble(myDataSet.Tables[0].Rows[12][1]);//数控代码Z的最小值，单位mm/
                polish_axi_maxfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[13][1]);//抛光轴最大速度,r/min
                polish_axi_minfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[14][1]);//抛光轴最小速度,r/min

                x_axi_hand_maxfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[15][1]);//X轴手动最大速度，单位mm/分钟
                b_axi_hand_maxfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[16][1]);//b轴手动最大速度，单位mm/分钟
                z_axi_hand_maxfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[17][1]);//z轴手动最大速度，单位mm/分钟
                C_axi_maxfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[18][1]);//C轴最大速度,r/min

                C_axi_minspeed = Convert.ToDouble(myDataSet.Tables[0].Rows[19][1]);//C轴最小速度,r/min
                C_Button_speed = Convert.ToDouble(myDataSet.Tables[0].Rows[20][1]);//C轴按钮速度,r/min

                caculate_time_offset = Convert.ToDouble(myDataSet.Tables[0].Rows[21][1]);//时间计算补正系数，单位分钟
                first_position_feed = Convert.ToDouble(myDataSet.Tables[0].Rows[22][1]);//X_b_Z初始位速度，单位mm/分钟

                x_devia_value = Convert.ToDouble(myDataSet.Tables[0].Rows[23][1]);//X轴回零偏移量，mm
                b_devia_value = Convert.ToDouble(myDataSet.Tables[0].Rows[24][1]);//B轴回零偏移量,度

                z_devia_value = Convert.ToDouble(myDataSet.Tables[0].Rows[25][1]);//Z轴回零偏移量，mm
                z_axi_first_jogdownfeed = Convert.ToDouble(myDataSet.Tables[0].Rows[26][1]);//抛光Z轴下降手动速度；
                B_axi_Center_Polish_head_distance = Convert.ToDouble(myDataSet.Tables[0].Rows[28][1]);//在零位时，B轴回转中心到抛光头触碰到第二个限位的距离；

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
              //  return;
            }
            // myDataSet.Tables[0].
           

            /************************************************************************************/

            #endregion


           
            if (openPmacSuccess == true)

            {

                //PMAC.GetResponse(pmacNumber, "j/", out status);//设置闭环
                //设置灰键
                connect_flag = true;
                openPmacSuccess = false;
                MessageBox.Show("连接控制器成功", "提示！");
                form2.Dispose(); //关闭启动页
                this.WindowState = FormWindowState.Normal; //打开主界面

            }
            else
            {
                connect_flag = false;
                MessageBox.Show("连接控制器失败", "提示！");
                form2.Dispose(); //关闭启动页
                this.WindowState = FormWindowState.Normal; //打开主界面

            }

            // }
            download_code_flag = false;//还没下进代码
           // this.panel1.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+"\\main.jpg");
            timer5.Enabled = true;//开启故障监测
         //   dectect.Start();
            /*****************h回零偏置读取****************/
            string status;
            string myfile = System.Environment.CurrentDirectory + "\\home_value.txt";
            StreamReader myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);//null;//new StreamReader();
           // myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
            string x_pos = myreader.ReadLine();
            string b_pos=myreader.ReadLine();
            string z_pos = (Convert.ToDouble(myDataSet.Tables[0].Rows[27][1]) * 16 * 10000).ToString();//Z轴回零偏移改变量(mm)
            myreader.Close();

         
            PMAC.GetResponse(pmacNumber, "P30=" + x_pos, out status);//X回零偏置
            PMAC.GetResponse(pmacNumber, "P31=" + b_pos, out status);//B回零偏置
            PMAC.GetResponse(pmacNumber, "P29=" + z_pos, out status);//Z回零偏置
             PMAC.GetResponse(pmacNumber, "P28="+(C_Button_speed*C_motor_scale_factor+C_motor_offset).ToString().Trim(), out status);//连接成功标志


             x_software_limit_left = Pmac.GetI(113);
             x_software_limit_right = Pmac.GetI(114);

             b_software_limit_left = Pmac.GetI(213);
             b_software_limit_right = Pmac.GetI(214);

             z_software_limit_up = Pmac.GetI(414);
             z_software_limit_down = Pmac.GetI(413);

          

            if(Pmac.GetP(33)==0)
            {

                x_software_limit_left = Pmac.GetI(113) - Convert.ToDouble(x_pos) / 16;
                x_software_limit_right = Pmac.GetI(114) - Convert.ToDouble(x_pos) / 16;
                b_software_limit_left = Pmac.GetI(213) - Convert.ToDouble(b_pos) / 16;
                b_software_limit_right = Pmac.GetI(214) - Convert.ToDouble(b_pos) / 16;
                z_software_limit_up = Pmac.GetI(414) - Convert.ToDouble(z_pos) / 16;
                z_software_limit_down = Pmac.GetI(413) - Convert.ToDouble(z_pos) / 16;

                PMAC.GetResponse(pmacNumber, "I113=" + x_software_limit_left.ToString().Trim(), out status);//X左软限位
                PMAC.GetResponse(pmacNumber, "I114=" + x_software_limit_right.ToString().Trim(), out status);//X右软限位
                PMAC.GetResponse(pmacNumber, "I213=" + b_software_limit_left.ToString().Trim(), out status);//B左软限位
                PMAC.GetResponse(pmacNumber, "I214=" + b_software_limit_right.ToString().Trim(), out status);//B右软限位
                PMAC.GetResponse(pmacNumber, "I413=" + z_software_limit_down.ToString().Trim(), out status);//Z下软限位
                PMAC.GetResponse(pmacNumber, "I414=" + z_software_limit_up.ToString().Trim(), out status);//Z上软限位

            }

         
        
            /********************************工件尺寸参数*************************************/

            x_devia_ADDvalue = Convert.ToDouble(x_pos) / 16 / 10000;
            b_devia_ADDvalue = Convert.ToDouble(b_pos) / 16 / 5000;
            z_devia_ADDvalue = Convert.ToDouble(z_pos) / 16 / 10000; 
            /*********************************************************************/
            PMAC.GetResponse(pmacNumber, "P33=1", out status);//连接成功标志
            //x_devia_value = 513431;
            //b_devia_value = 428014;

           // PMAC.GetResponse(pmacNumber, "#1->10000X-" + Convert.ToInt64((Convert.ToDouble(x_pos) + x_devia_value)).ToString(), out status);
          //  PMAC.GetResponse(pmacNumber, "#1->10000X+", out status);
            
           // PMAC.GetResponse(pmacNumber, "#2->5000B+" + Convert.ToInt64((Convert.ToDouble(b_pos)/16 + b_devia_value)).ToString(), out status);

           // x_devia_value = Convert.ToDouble(x_pos) / 1 + x_devia_value;//调过零点后的
           // b_devia_value = Convert.ToDouble(b_pos) / 16 + b_devia_value;


            PMAC.GetResponse(pmacNumber, "ENABLE PLC 1", out status);
            if (Pmac.GetP(34) == 0&&connect_flag==true)
            {
                if (MessageBox.Show("开始复位？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                   
                     PMAC.GetResponse(pmacNumber, "P30=" + x_pos, out status);
                    PMAC.GetResponse(pmacNumber, "P31=" + b_pos, out status);
                    PMAC.GetResponse(pmacNumber, "P29=" + z_pos, out status);
                    //PMAC.GetResponse(pmacNumber, "#1J/", out status);
                    PMAC.GetResponse(pmacNumber, "ENABLE PLC 5", out status);
                    Delay(2);

                   // timer7.Enabled = true;
                    Pmac.SetP(34, 1);//置开机复位标志

                }
                else
                    return;

            }

            GraphPane myPane;
            // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
            myPane = zedGraphControl1.GraphPane;
            myPane.XAxis.Title.Text = "半径(mm)";
            myPane.YAxis.Title.Text = "误差(um)";
            myPane.Title.Text = " ";
            myPane.XAxis.CrossAuto = true;
          //  myPane.XAxis.Type = ZedGraph.AxisType.LinearAsOrdinal;

            GraphPane myPane1;


            // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
            myPane1 = zedGraphControl2.GraphPane;
            myPane1.XAxis.Title.Text = "";
            myPane1.YAxis.Title.Text = "";
            myPane1.Title.Text = "";

//账号密码信息读取


            //string accountfile = System.Environment.CurrentDirectory + "\\account.txt";
            //StreamReader accounreader = new StreamReader(accountfile, System.Text.Encoding.UTF8);//null;//new StreamReader();
            //// myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
            //string account = accounreader.ReadLine();
            //string passward = accounreader.ReadLine();
            //accounreader.Close();
            //this.textBox36.Text = account;
            //this.textBox38.Text = passward;

            /* string[] ports = SerialPort.GetPortNames();
              Array.Sort(ports);

              cbxPortNum.Items.AddRange(ports);


              Settings.Default.Reload();
              cbxPortNum.SelectedItem = Settings.Default["PortNum"].ToString();
              cbxBaudRate.SelectedItem = Settings.Default["BaudRate"].ToString();
              txtaddr.Text = Settings.Default["addr"].ToString();

              */





        }







        private void button1_Click(object sender, EventArgs e)//读取TXT中已有参数
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string InitialDire = System.Environment.CurrentDirectory;
            int ao_tu_symbol;
            dialog.Title = "模仁形状参数";
            dialog.InitialDirectory = InitialDire+"\\模仁形状参数";
            dialog.Filter = @"形状参数文件(.txt1)|*.txt1";//"文件|*.txt|txt1文件(*.txt1)|*.txt1";
            dialog.ShowDialog();
            string myfile = dialog.FileName;
            
            if (myfile.Trim() == "")
                return;
            textBox1.Text = Path.GetFileNameWithoutExtension(myfile);
           // if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)//非球面时
          //  {
                StreamReader myreader = null;//new StreamReader();
                myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
            try
            { 
                yuanr_text.Text = myreader.ReadLine();
                gongD_textBox3.Text = myreader.ReadLine();
                SAG_text.Text = myreader.ReadLine();
                TextBox35.Text = myreader.ReadLine();
                R_text.Text = myreader.ReadLine();
                K_text.Text = myreader.ReadLine();
                A1.Text = myreader.ReadLine();
                A2.Text = myreader.ReadLine();
                A3.Text = myreader.ReadLine();
                A4.Text = myreader.ReadLine();
                A5.Text = myreader.ReadLine();
                A6.Text = myreader.ReadLine();
                A7.Text = myreader.ReadLine();
                A8.Text = myreader.ReadLine();
                A9.Text = myreader.ReadLine();
                A10.Text = myreader.ReadLine();
                A11.Text = myreader.ReadLine();
                A12.Text = myreader.ReadLine();
                A13.Text = myreader.ReadLine();
                A14.Text = myreader.ReadLine();
                A15.Text = myreader.ReadLine();
                A16.Text = myreader.ReadLine();
                A17.Text = myreader.ReadLine();
                A18.Text = myreader.ReadLine();
                A19.Text = myreader.ReadLine();
                A20.Text = myreader.ReadLine();
                ao_tu_symbol = Convert.ToInt16(myreader.ReadLine());
               textBox46.Text = myreader.ReadLine().Trim();//工件直径
                //if (textbox46 =="")
                //    textBox46.Text = Convert.ToString(Convert.ToInt16((Convert.ToDouble(gongD_textBox3.Text)+5)));
                //else
                //    textBox46.Text = textbox46;
            }
            catch (Exception )
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("数据文件格式错误！请检查文件数据格式是否正确。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(ex.Message);
                return;
            }
                if (ao_tu_symbol == 1)
                    ao_non_sphere.Checked =true;
                else if (ao_tu_symbol == 2)
                    tu_non_sphere.Checked =true;
                else if (ao_tu_symbol == 3)
                    ao_sphere.Checked = true;
                else if (ao_tu_symbol == 4)
                    tu_sphere.Checked = true;
                else if (ao_tu_symbol == 5)
                    plane.Checked = true;


                myreader.Close();
                this.draw();
          /*  }
            if (ao_sphere.Checked == true || tu_sphere.Checked == true)//球面时
            {
                StreamReader myreader = null;//new StreamReader();
                myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                yuanr_text.Text = myreader.ReadLine();
                gongD_textBox3.Text = myreader.ReadLine();
                SAG_text.Text = myreader.ReadLine();
                D_text.Text = myreader.ReadLine();
                R_text.Text = myreader.ReadLine();
                ao_tu_symbol = Convert.ToInt16(myreader.ReadLine());
                if (ao_tu_symbol == 1)
                    ao_sphere.Checked = true;
                else
                    tu_sphere.Checked = true;
                myreader.Close();
                this.draw();

            }

            if (plane.Checked == true)//平面时
            {
                StreamReader myreader = null;//new StreamReader();
                myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                yuanr_text.Text = myreader.ReadLine();
                gongD_textBox3.Text = myreader.ReadLine();
                SAG_text.Text = myreader.ReadLine();
                D_text.Text = myreader.ReadLine();
                ao_tu_symbol = Convert.ToInt16(myreader.ReadLine());
                if (ao_tu_symbol == 2)
                    ao_sphere.Checked = true;
                myreader.Close();
                this.draw();

            }*/

            this.button19.Enabled = true;//应用按钮正常

        }

        private void button2_Click(object sender, EventArgs e)//保存参数到TXT
        {

            this.saveParameter();
            this.draw();
            this.button19.Enabled = true;//应用按钮正常


        }
        public void saveParameter()//保存面形参数
        {
            double[] Parameter = new double[26];

            try
            {
                Parameter = new double[26] { Convert.ToDouble(yuanr_text.Text), Convert.ToDouble(gongD_textBox3.Text), Convert.ToDouble(SAG_text.Text), Convert.ToDouble(TextBox35.Text), Convert.ToDouble(R_text.Text), Convert.ToDouble(K_text.Text), Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
            }
            catch (Exception )
            {
                MessageBox.Show("请输入全部参数！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // MessageBox.Show(Err.Message);
                return;
            }


            SaveFileDialog sfd = new SaveFileDialog();
            string InitialDire = System.Environment.CurrentDirectory;
            sfd.Title = "模仁形状参数保存";
            sfd.InitialDirectory = InitialDire+"\\模仁形状参数";
            sfd.Filter = "txt1文件(*.txt1)|*.txt1";//@"文本文件| *.txt";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }

           // if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
          //  {
                
                    /***************写入TXT********/

                //  path = sfd.FileName;//结果保存

                /*先清空parameter.txt文件内容*/
                FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream2.Seek(0, SeekOrigin.Begin);
                stream2.SetLength(0); //清空txt文件
                stream2.Close();
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                for (int i = 0; i < 26; i++)
                {
                    wr.WriteLine(Convert.ToString(Parameter[i]));
                }
                if (ao_non_sphere.Checked == true)
                    wr.WriteLine("1");
                else if (tu_non_sphere.Checked == true)
                    wr.WriteLine("2");
                else if (ao_sphere.Checked == true)
                    wr.WriteLine("3");
                else if (tu_sphere.Checked == true)
                    wr.WriteLine("4");
                else if (plane.Checked == true)
                    wr.WriteLine("5");
                wr.WriteLine(textBox46.Text.Trim());//工件直径
                wr.Close();
          //  }
           // if (ao_sphere.Checked == true || tu_sphere.Checked == true)
           // {
            //    double[] Parameter = new double[5];
             //   Parameter = new double[5] { Convert.ToDouble(yuanr_text.Text), Convert.ToDouble(gongD_textBox3.Text), Convert.ToDouble(SAG_text.Text), Convert.ToDouble(D_text.Text), Convert.ToDouble(R_text.Text) };
                /***************写入TXT********/

                //string result1 = @".\" + textBox1.Text + @".txt";//结果保存到.\parameter.txt

                /*先清空parameter.txt文件内容*/
            /*    FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream2.Seek(0, SeekOrigin.Begin);
                stream2.SetLength(0); //清空txt文件
                stream2.Close();
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                for (int i = 0; i < 5; i++)
                {
                    wr.WriteLine(Convert.ToString(Parameter[i]));
                }
                if (ao_sphere.Checked == true)
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                wr.Close();

            }
            if (plane.Checked == true)
            {
                double[] Parameter = new double[4];
                Parameter = new double[4] { Convert.ToDouble(yuanr_text.Text), Convert.ToDouble(gongD_textBox3.Text), Convert.ToDouble(SAG_text.Text), Convert.ToDouble(D_text.Text) };
                /***************写入TXT********/

                //  string result1 = @".\" + textBox1.Text + @".txt";//结果保存到.\parameter.txt

                /*先清空parameter.txt文件内容*/
              /*  FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream2.Seek(0, SeekOrigin.Begin);
                stream2.SetLength(0); //清空txt文件
                stream2.Close();
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                for (int i = 0; i < 4; i++)
                {
                    wr.WriteLine(Convert.ToString(Parameter[i]));
                }               
                    wr.WriteLine("2");
                
                wr.Close();

            }*/

        }

        private void drawClear()//清空绘图区
        {
            Bitmap bitmap;
            Graphics graphics;
            Brush brushPoint = new SolidBrush(Color.Red);
           // float Tension = 0.05f;
            //创建位图
            bitmap = new Bitmap(400, 400);
            //创建Graphics类对象
            graphics = Graphics.FromImage(bitmap);

            Pen mypenBlue = new Pen(Color.Blue, 1);//线条
            Pen mypenRed = new Pen(Color.Black, 1);
            Pen mypenYellow = new Pen(Color.Black, 1);//点颜色

            //double[] x = new double[101];
            //double[] y = new double[101];
            /*for (int i = 0; i < 101; i++)
            {
                x[i] = -50 + i;
            }
            for (int i = 0; i < 101; i++)
            {
                y[i] = 0.05 * Math.Pow(x[i], 2);
            }*/
           

          

          //  graphics.DrawCurve(mypenYellow, CurvePointF, Tension);//画曲线
            graphics.DrawLine(mypenYellow, 0, 120, 340, 120);//X坐标轴
            graphics.DrawLine(mypenYellow, 0, 120, 10, 110);//X坐标轴箭头
            graphics.DrawLine(mypenYellow, 0, 120, 10, 130);
            graphics.DrawLine(mypenYellow, 170, 0, 170, 240);//Z坐标轴
            graphics.DrawLine(mypenYellow, 170, 240, 180, 230);//Z坐标轴箭头
            graphics.DrawLine(mypenYellow, 170, 240, 160, 230);//Z坐标轴箭头
            SolidBrush MyBrush = new SolidBrush(Color.Black);
            graphics.DrawString("X+", this.Font, MyBrush, 20, 130);
            graphics.DrawString("Z+", this.Font, MyBrush, 140, 220);
            //graphics.DrawRectangle(mypenYellow, pictureBox3.ClientRectangle.X, pictureBox3.ClientRectangle.Y, pictureBox3.ClientRectangle.X + pictureBox3.ClientRectangle.Width, pictureBox3.ClientRectangle.Y + pictureBox3.ClientRectangle.Height);


            graphics.Dispose();
            this.pictureBox3.Image = bitmap;
            draw_flag = true;
            /********画在zed*********/
            GraphPane myPane1;


            // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
            myPane1 = zedGraphControl2.GraphPane;
            myPane1.XAxis.Title.Text = "";
            myPane1.YAxis.Title.Text = "";
            myPane1.Title.Text = "";
            //myPane1.XAxis.
            // myPane1.YAxis.Scale.MajorStep = 1;
            // myPane1.YAxis.Scale.Max = 100;
            myPane1.CurveList.Clear();
            myPane1.GraphObjList.Clear();


            myPane1.AxisChange();
            zedGraphControl2.Refresh();
        }
        private void draw()//绘图
        {

            /******绘图数据***/

            try
            {

                double D = Convert.ToDouble(gongD_textBox3.Text);//加工口径  

                if (tu_sphere.Checked == true || ao_sphere.Checked == true)
                {
                    if (D > 2 * Math.Abs(Convert.ToDouble(R_text.Text)))
                        D = 2 * Math.Abs(Convert.ToDouble(R_text.Text));
                }

                int a = Convert.ToInt16(D / 0.01);
                double[] X = new double[a + 1];
                double[] Y = new double[a + 1];//函数曲线Y值

                double symbol = 1;

                if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
                {
                    double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
                    double R = Convert.ToDouble(R_text.Text);  //面型参数R
                    double K = Convert.ToDouble(K_text.Text);//面型参数K
                    double C = 1 / R;
                    if (ao_non_sphere.Checked == true)
                    {
                        if (R > 0)
                            symbol = 1;
                        else
                            symbol = -1;

                    }
                    if (tu_non_sphere.Checked == true)
                    {
                        if (R > 0)
                            symbol = -1;
                        else
                            symbol = 1;

                    }


                    for (int i = 0; i < a + 1; i++)//x坐标
                    {
                        X[i] = -D / 2 + i * 0.01;
                    }
                    for (int i = 0; i < a + 1; i++)//Y坐标，
                    {

                        Y[i] = symbol * (Math.Pow(X[i], 2) * C / (1 + Math.Sqrt(1 - (K + 1) * Math.Pow(X[i], 2) * Math.Pow(C, 2))) + A[0] * Math.Abs(X[i]) + A[1] * Math.Pow((X[i]), 2) + A[2] * Math.Pow(Math.Abs(X[i]), 3) + A[3] * Math.Pow((X[i]), 4) + A[4] * Math.Pow(Math.Abs(X[i]), 5) + A[5] * Math.Pow((X[i]), 6) + A[6] * Math.Pow(Math.Abs(X[i]), 7) + A[7] * Math.Pow((X[i]), 8) + A[8] * Math.Pow(Math.Abs(X[i]), 9) + A[9] * Math.Pow((X[i]), 10) + A[10] * Math.Pow(Math.Abs(X[i]), 11) + A[11] * Math.Pow((X[i]), 12) + A[12] * Math.Pow(Math.Abs(X[i]), 13) + A[13] * Math.Pow((X[i]), 14) + A[14] * Math.Pow(Math.Abs(X[i]), 15) + A[15] * Math.Pow((X[i]), 16) + A[16] * Math.Pow(Math.Abs(X[i]), 17) + A[17] * Math.Pow((X[i]), 18) + A[18] * Math.Pow(Math.Abs(X[i]), 19) + A[19] * Math.Pow((X[i]), 20));
               
                    }


                }
                else if (ao_sphere.Checked == true || tu_sphere.Checked == true)
                {
                    double R = Convert.ToDouble(R_text.Text);  //面型参数R
                    if (ao_sphere.Checked == true)
                        symbol = 1;
                    if (tu_sphere.Checked == true)
                        symbol = -1;
                    for (int i = 0; i < a + 1; i++)//x坐标
                    {
                        X[i] = -D / 2 + i * 0.01;
                    }
                    for (int i = 0; i < a + 1; i++)//Y坐标，
                    {
                        if (tu_sphere.Checked == true)
                            Y[i] = -symbol * (Math.Sqrt(Math.Pow(R, 2) - Math.Pow(X[i], 2)) + symbol * R);
                        if (ao_sphere.Checked == true)
                            Y[i] = -symbol * (Math.Sqrt(Math.Pow(R, 2) - Math.Pow(X[i], 2)) - symbol * R);

                    }

                }
          /*****绘图*************************/

                Bitmap bitmap;
                Graphics graphics;
                Brush brushPoint = new SolidBrush(Color.Red);
                float Tension = 0.05f;
                //创建位图
                bitmap = new Bitmap(400, 400);
                //创建Graphics类对象
                graphics = Graphics.FromImage(bitmap);

                Pen mypenBlue = new Pen(Color.Blue, 1);//线条
                Pen mypenRed = new Pen(Color.Black, 1);
                Pen mypenYellow = new Pen(Color.Black, 1);//点颜色

                //double[] x = new double[101];
                //double[] y = new double[101];
                /*for (int i = 0; i < 101; i++)
                {
                    x[i] = -50 + i;
                }
                for (int i = 0; i < 101; i++)
                {
                    y[i] = 0.05 * Math.Pow(x[i], 2);
                }*/
                PointF[] CurvePointF = new PointF[a + 1];//坐标点
                float pointX = 0;
                float pointY = 0;
                for (int i = 0; i < a + 1; i++)
                {
                    string a1 = Convert.ToString(X[i]);
                    string b = Convert.ToString(Y[i]);
                    pointX = 10 * float.Parse(a1) + 170;
                    pointY = -10 * float.Parse(b) + 120;
                    //pointX = i * times * 4 + 80;    //坐标系（0，0）实际位置为（70,310）实际工程坐标左上角为原点。
                    //pointY = 310 - 60 * (buf[i] / 10);
                    CurvePointF[i] = new PointF(pointX, pointY);

                    // graphics.FillEllipse(brushPoint, pointX , pointY , 8, 8);//画点，是以圆心为切点画的圆，所以减去半径

                }
                graphics.DrawCurve(mypenYellow, CurvePointF, Tension);//画曲线
                graphics.DrawLine(mypenYellow, 0, 120, 340, 120);//X坐标轴
                graphics.DrawLine(mypenYellow, 0, 120, 10, 110);//X坐标轴箭头
                graphics.DrawLine(mypenYellow, 0, 120, 10, 130);
                graphics.DrawLine(mypenYellow, 170, 0, 170, 240);//Z坐标轴
                graphics.DrawLine(mypenYellow, 170, 240, 180, 230);//Z坐标轴箭头
                graphics.DrawLine(mypenYellow, 170, 240, 160, 230);//Z坐标轴箭头
                SolidBrush MyBrush = new SolidBrush(Color.Black);
                graphics.DrawString("X+", this.Font, MyBrush, 20, 130);
                graphics.DrawString("Z+", this.Font, MyBrush, 140, 220);
                //graphics.DrawRectangle(mypenYellow, pictureBox3.ClientRectangle.X, pictureBox3.ClientRectangle.Y, pictureBox3.ClientRectangle.X + pictureBox3.ClientRectangle.Width, pictureBox3.ClientRectangle.Y + pictureBox3.ClientRectangle.Height);


                graphics.Dispose();
                this.pictureBox3.Image = bitmap;
                draw_flag = true;
                /********画在zed*********/
                GraphPane myPane1;


                // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
                myPane1 = zedGraphControl2.GraphPane;
                myPane1.XAxis.Title.Text = "";
                myPane1.YAxis.Title.Text = "";
                myPane1.Title.Text = "";
                //myPane1.XAxis.
                // myPane1.YAxis.Scale.MajorStep = 1;
                // myPane1.YAxis.Scale.Max = 100;
                myPane1.CurveList.Clear();
                myPane1.GraphObjList.Clear();
                //   myPane.XAxis.Type = ZedGraph.AxisType.LinearAsOrdinal;
                // 设置初始数据
                // double xx, y11, y22;
                PointPairList list1 = new PointPairList();

                /*  for (int i = 0; i < 36; i++)
                  {
                      xx = (double)i + 5;
                      y11 = 1.5 + Math.Sin((double)i * 0.2);
                      y22 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                      list1.Add(xx, y11);
                      list2.Add(xx, y22);
                  }*/
                //list1.Clear();
                //  list2.Clear();
                //  list3.Clear();
                for (int i = 0; i < a + 1; i++)
                {


                    list1.Add(X[i], Y[i]);
                    // graphics.FillEllipse(brushPoint, pointX , pointY , 8, 8);//画点，是以圆心为切点画的圆，所以减去半径

                }

                /* for (int i = NC_Data.Length / 10 - 1; i > 0; i--)
                 {
                     xx = x2[i];
                     y11 = y2[i];
                     y22 = ave_zd2[y2_temp[i]] - y2[i];
                     list2.Add(xx, y11 * 1000);
                     list3.Add(xx, y22 * 1000);
                     //   list5.Add(-xx, y11);
                     //    list6.Add(-xx, y22);
                 }
                 for (int i = 0; i < NC_Data.Length / 10; i++)
                 {
                     xx = x2[i];
                     y11 = y2[i];
                     y22 = ave_zd2[y2_temp[i]] - y2[i];
                     list2.Add(-xx, y11 * 1000);
                     list3.Add(-xx, y22 * 1000);
                     //   list5.Add(-xx, y11);
                     //    list6.Add(-xx, y22);
                 }

                 // 创建红色的菱形曲线
                 // 标记, 图中的 "Porsche" */
                LineItem myCurve = myPane1.AddCurve("", list1, Color.Red, SymbolType.None);
                // LineItem myCurve_2 = myPane.AddCurve("测量面型", list4, Color.Red, SymbolType.None);
                // 创建蓝色的圆形曲线
                // 标记, 图中的 "Piper"    
                //  LineItem myCurve2 = myPane.AddCurve("拟合面型", list2, Color.Black, SymbolType.None);
                // LineItem myCurve2_2 = myPane.AddCurve("拟合面型", list5, Color.Black, SymbolType.None);

                //   LineItem myCurve3 = myPane.AddCurve("补正面型", list3, Color.Blue, SymbolType.None);
                //  LineItem myCurve3_2 = myPane.AddCurve("补正面型", list6, Color.Blue, SymbolType.None);

                // 在数据变化时绘制图形
                //  myPane.AxisChange(this.CreateGraphics());

                myPane1.AxisChange();
                zedGraphControl2.Refresh();
                //  myPane.Draw(e.Graphics);
            }
            catch(Exception Err)
            {
                MessageBox.Show(Err.Message);
                return;
            }


        }

        private void button20_Click(object sender, EventArgs e)//转到手动模式
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = true;
            this.panel7.Visible = false;
            timer2.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //this.tabControl1.SelectedIndex = 1;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            timer2.Enabled = false;//关闭手动模式界面计时器
            if (Alarm_clear_flag == true)
            {
                Pmac.SendCommand("I113=" + Convert.ToString(x_software_limit_left));//X轴左软限位恢复
                Pmac.SendCommand("I114=" + Convert.ToString(x_software_limit_right));//X轴右软限位恢复
             //   Pmac.SendCommand("I213=" + Convert.ToString(Math.Abs(blimit * 10000)));//X轴左软限位恢复
                Pmac.SendCommand("I214=" + Convert.ToString(b_software_limit_right));//b轴右软限位恢复
                Pmac.SendCommand("I413=" + Convert.ToString(z_software_limit_down));//Z轴下软限位恢复
                Pmac.SendCommand("I414="+Convert.ToString(z_software_limit_up));//Z轴上软限位恢复
                Alarm_clear_flag = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)//显示模仁参数示意图片
        {
            //Thread MyThreadOne = new Thread(new ThreadStart(pshow));
            //MyThreadOne.Start();

            timer1.Interval = 500;
            GC.Collect();
            if (tu_sphere.Checked == true)
            {
                this.pictureBox1.Image = System.Drawing.Bitmap.FromFile(@".\picture_tu.png");
                R_text.Enabled = true;
                K_text.Enabled = false;
                SAG_text.Enabled = true;
                this.disable_A();
                this.set_Ak_zero();
                //textBox36.Visible = true;
                //textBox6.Visible = false;
                //textBox18.Visible = false;
                //label56.Visible = false;

            }
            else if (ao_sphere.Checked == true)
            {
                this.pictureBox1.Image = System.Drawing.Bitmap.FromFile(@".\picture_ao.png");
                R_text.Enabled = true;
                K_text.Enabled = false;
                SAG_text.Enabled = true;
                this.disable_A();
                this.set_Ak_zero();
                //textBox36.Visible = true;
                //textBox6.Visible = false;
                //textBox18.Visible = false;
                //label56.Visible = false;
            }
            else if (tu_non_sphere.Checked == true)
            {
                this.pictureBox1.Image = System.Drawing.Bitmap.FromFile(@".\picture_tu.png");
                R_text.Enabled = true;
                K_text.Enabled = true;
                SAG_text.Enabled = true;
                this.enable_A();
                //textBox36.Visible = true;
                //textBox6.Visible = false;
                //textBox18.Visible = false;
                //label56.Visible = false;
            }
            else if (ao_non_sphere.Checked == true)
            {
                this.pictureBox1.Image = System.Drawing.Bitmap.FromFile(@".\picture_ao.png");
                R_text.Enabled = true;
                K_text.Enabled = true;
                SAG_text.Enabled = true;
                this.enable_A();
                //textBox36.Visible = true;
                //textBox6.Visible = false;
                //textBox18.Visible = false;
                //label56.Visible = false;
            }
            if (plane.Checked == true)
            {

                // this.pictureBox1.Image = System.Drawing.Bitmap.FromFile(@".\picture3.png");
                this.pictureBox1.Image = Image.FromFile(@".\picture_plane.png");
                R_text.Enabled = false;
                K_text.Enabled = false;
                SAG_text.Enabled = false;
                this.disable_A();
                this.set_Akr_zero();
                SAG_text.Text = "0";//非球面矢高为零
                //textBox36.Visible = false;
                //textBox6.Visible = true;
                //textBox18.Visible = true;
                //label56.Visible = true;
                drawClear();


            }
            //if (Alarm_clear_flag == true)
            //{
 
            //}

           /* if (draw_flag == true)
            {
                if (Convert.ToDouble(K_text.Text) != 0)
                    this.draw();

            }*/


        }
        public void set_Akr_zero()
        {
            R_text.Text = "0";
            K_text.Text = "0";
            A1.Text = "0";
            A2.Text = "0";
            A3.Text = "0";
            A4.Text = "0";
            A5.Text = "0";
            A6.Text = "0";
            A7.Text = "0";
            A8.Text = "0";
            A9.Text = "0";
            A10.Text = "0";
            A11.Text = "0";
            A12.Text = "0";
            A13.Text = "0";
            A14.Text = "0";
            A15.Text = "0";
            A16.Text = "0";
            A17.Text = "0";
            A18.Text = "0";
            A19.Text = "0";
            A20.Text = "0";


        }
        public void set_Ak_zero()
        {
            K_text.Text = "0";
            A1.Text = "0";
            A2.Text = "0";
            A3.Text = "0";
            A4.Text = "0";
            A5.Text = "0";
            A6.Text = "0";
            A7.Text = "0";
            A8.Text = "0";
            A9.Text = "0";
            A10.Text = "0";
            A11.Text = "0";
            A12.Text = "0";
            A13.Text = "0";
            A14.Text = "0";
            A15.Text = "0";
            A16.Text = "0";
            A17.Text = "0";
            A18.Text = "0";
            A19.Text = "0";
            A20.Text = "0";

        }
        public void disable_A()
        {
            A1.Enabled = false;
            A2.Enabled = false;
            A3.Enabled = false;
            A4.Enabled = false;
            A5.Enabled = false;
            A6.Enabled = false;
            A7.Enabled = false;
            A8.Enabled = false;
            A9.Enabled = false;
            A10.Enabled = false;
            A11.Enabled = false;
            A12.Enabled = false;
            A13.Enabled = false;
            A14.Enabled = false;
            A15.Enabled = false;
            A16.Enabled = false;
            A17.Enabled = false;
            A18.Enabled = false;
            A19.Enabled = false;
            A20.Enabled = false;

        }
        public void enable_A()
        {

            A1.Enabled = true;
            A2.Enabled = true;
            A3.Enabled = true;
            A4.Enabled = true;
            A5.Enabled = true;
            A6.Enabled = true;
            A7.Enabled = true;
            A8.Enabled = true;
            A9.Enabled = true;
            A10.Enabled = true;
            A11.Enabled = true;
            A12.Enabled = true;
            A13.Enabled = true;
            A14.Enabled = true;
            A15.Enabled = true;
            A16.Enabled = true;
            A17.Enabled = true;
            A18.Enabled = true;
            A19.Enabled = true;
            A20.Enabled = true;

        }
        public void enable_SAG()
        {
            SAG_text.Enabled = true;
            //SAG_text.Text = "0";
        }
        public void disable_SAG()
        {
            SAG_text.Enabled = false;
            SAG_text.Text = "0";
        }

        private void button3_Click(object sender, EventArgs e)//退出界面
        {
           // this.tabControl1.SelectedIndex = 1;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            this.button19.Enabled = true; //应用按钮变正常
            this.timer1.Enabled = false;//关闭模具参数设定界面的timer
            //this.timer1.Enabled = false;
        }

        private void button19_Click(object sender, EventArgs e)//跳转到加工参数界面
        {
            //this.tabControl1.SelectedIndex = 3;//转到加工参数界面
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = true;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            this.textBox34.Text = textBox1.Text;//显示模仁名称
            textBox34.Enabled = false;
            textBox35_1.Text = SAG_text.Text;//显示模仁平台高度
            // textBox33.Enabled = false;
            //textBox36.Text = TextBox35.Text;//显示模仁口径
            // textBox32.Enabled = false;
            timer4.Enabled = true;//开启加工参数界面计时器
            radioButton8.Checked = true;
            radioButton2.Checked = true;
            radioButton3.Checked = true;
            textBox3.Text = "0";
            textBox4.Text = "0";
            //textBox40.Text = "0.1";//数据间隔
            textBox39.Text = "1";

        }

      

        private void button13_Click(object sender, EventArgs e)//生成NC代码
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = true;
            this.panel9.Visible = false;
            Pcode_return_flag = true;
            textBox16.Text = textBox39.Text;

        }

        private void button14_Click(object sender, EventArgs e)//形状参数设定选择界面
        {
            // this.tabControl1.SelectedIndex = 2;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            timer4.Enabled = false;//关闭加工参数界面计时器
           
        }

        private void button18_Click(object sender, EventArgs e)//返回加工参数界面
        {
            //this.tabControl1.SelectedIndex = 1;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
        }

        private void button21_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)//手动模式界面的timer
        {
            double pos1, pos2,pos3;
            Pmac.GetPos(1, out pos1);
            Pmac.GetPos(2, out pos2);
            Pmac.GetPos(4, out pos3);
            textBox27.Text = Convert.ToString(((pos1) / 10000 + x_devia_value + x_devia_ADDvalue).ToString("f4"));//x轴机械坐标
            textBox28.Text = Convert.ToString(((pos2) / 5000 + b_devia_value+b_devia_ADDvalue).ToString("f4"));//B轴机械坐标
            textBox24.Text = Convert.ToString(((pos3) / 10000 + z_devia_value+z_devia_ADDvalue).ToString("f4"));//z轴机械坐标

            textBox26.Text = Convert.ToString(((pos1) / 10000).ToString("f4"));//x轴相对坐标
            textBox29.Text = Convert.ToString(((pos2) / 5000).ToString("f4"));//B轴相对坐标
            textBox23.Text = Convert.ToString(((pos3) / 10000).ToString("f4"));//z轴相对坐标
            if (comboBox2.SelectedIndex == 0)
            {
                textBox30.Enabled = false;
                textBox31.Enabled = false;
                textBox22.Enabled = false;
                

            }
            if (comboBox2.SelectedIndex == 1)
            {
                textBox30.Enabled = true;
                textBox31.Enabled = true;
                textBox22.Enabled = true;
       

            }
           // string status;
          //  PMAC.GetResponse(pmacNumber, "I122=", out status);
            timer2.Interval = 200;

        }

        private void button4_Click(object sender, EventArgs e)//回原点
        {
            string status;

            if (MessageBox.Show("是否复位？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string myfile = System.Environment.CurrentDirectory + "\\home_value.txt";
                StreamReader myreader = null;//new StreamReader();
                myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                string x_pos = myreader.ReadLine(), b_pos = myreader.ReadLine();// z_pos = myreader.ReadLine();
                myreader.Close();
                PMAC.GetResponse(pmacNumber, "P30=" + x_pos, out status);
                PMAC.GetResponse(pmacNumber, "P31=" + b_pos, out status);
              //  PMAC.GetResponse(pmacNumber, "P29=" + z_pos, out status);
                //PMAC.GetResponse(pmacNumber, "#1J/", out status);
                PMAC.GetResponse(pmacNumber, "ENABLE PLC 1", out status);
                PMAC.GetResponse(pmacNumber, "ENABLE PLC 5", out status);
               // Delay(2);
                //timer7.Enabled = true;
                //PMAC.GetResponse(pmacNumber, "#2hm", out status);
                x_devia_ADDvalue = Convert.ToDouble(x_pos) / 16 / 10000;
                b_devia_ADDvalue = Convert.ToDouble(b_pos) / 16 / 5000;
              //  z_devia_ADDvalue = Convert.ToDouble(z_pos) / 16 / 10000; 
            }
            else
                return;
        }

        private void button5_Click(object sender, EventArgs e)//再原点
        {
            string status;
           // PMAC.GetResponse(pmacNumber, "#1HMZ", out status);
           // PMAC.GetResponse(pmacNumber, "#2HMZ", out status);
            double x_pos,b_pos,z_pos;
            Pmac.GetPos(1,out x_pos);
            Pmac.GetPos(2,out b_pos);
            Pmac.GetPos(4, out z_pos);
          //  Pmac.SendCommand("I51=0");
            if (MessageBox.Show("是否设置当前位为原点？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                //  StreamWriter wr = new StreamWriter();
                string path = System.Environment.CurrentDirectory + "\\home_value.txt";

                StreamReader Readfile = new StreamReader(path, System.Text.Encoding.UTF8);
                //double pos_x = Convert.ToDouble(Readfile.ReadLine());//读取原来的
                double p30 = Convert.ToDouble(Readfile.ReadLine());
                double p31 = Convert.ToDouble(Readfile.ReadLine());
               // double p29 = Convert.ToDouble(Readfile.ReadLine());
                Readfile.Close();
                //*先清空result1.txt文件内容
                FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream2.Seek(0, SeekOrigin.Begin);
                stream2.SetLength(0); //清空txt文件
                stream2.Close();


                FileStream fs = new FileStream(path, FileMode.Append);
                // fs.Seek(0, SeekOrigin.Begin);
                // fs.SetLength(0);
                StreamWriter wr = null;
                //   double n = Convert.ToDouble(textBox39.Text);//加工次数
                wr = new StreamWriter(fs);
               // wr.WriteLine(Convert.ToInt64((x_pos - x_devia_value + pos_x)).ToString());
                wr.WriteLine(Convert.ToInt64((x_pos * 16 + p30)).ToString());
                wr.WriteLine(Convert.ToInt64((b_pos * 16 + p31)).ToString());
               // wr.WriteLine(Convert.ToInt64((z_pos * 16 + p29)).ToString());
                wr.Close();

                PMAC.GetResponse(pmacNumber, "P30=" + ((x_pos * 16 + p30)).ToString(), out status);
                PMAC.GetResponse(pmacNumber, "P31=" + ((b_pos * 16 + p31)).ToString(), out status);
             //   PMAC.GetResponse(pmacNumber, "P29=" + ((z_pos * 16 + p29)).ToString(), out status);
                MessageBox.Show("原点设置成功！");
              

                 x_devia_ADDvalue = (Convert.ToDouble(x_pos)+p30/16) /  10000;
                 b_devia_ADDvalue = (Convert.ToDouble(b_pos) + p31 / 16) / 5000;
              //   z_devia_ADDvalue = (Convert.ToDouble(z_pos) + p29 / 16) / 10000;

                 x_software_limit_left = x_software_limit_left - Convert.ToDouble(x_pos);
                 x_software_limit_right = x_software_limit_right - Convert.ToDouble(x_pos);

                 b_software_limit_left = b_software_limit_left - Convert.ToDouble(b_pos);
                 b_software_limit_right = b_software_limit_right - Convert.ToDouble(b_pos);

              //   z_software_limit_up = z_software_limit_up - Convert.ToDouble(z_pos);
              //   z_software_limit_down = z_software_limit_down - Convert.ToDouble(z_pos);


                 PMAC.GetResponse(pmacNumber, "I113=" + x_software_limit_left.ToString().Trim(), out status);//X左软限位
                 PMAC.GetResponse(pmacNumber, "I114=" + x_software_limit_right.ToString().Trim(), out status);//X右软限位
                 PMAC.GetResponse(pmacNumber, "I213=" + b_software_limit_left.ToString().Trim(), out status);//B左软限位
                 PMAC.GetResponse(pmacNumber, "I214=" + b_software_limit_right.ToString().Trim(), out status);//B右软限位
               //  PMAC.GetResponse(pmacNumber, "I413=" + z_software_limit_down.ToString().Trim(), out status);//Z下软限位
              //   PMAC.GetResponse(pmacNumber, "I414=" + z_software_limit_up.ToString().Trim(), out status);//Z上软限位




                 Pmac.SendCommand("#1HMZ");
                 Pmac.SendCommand("#2HMZ");
             //    Pmac.SendCommand("#4HMZ");
                // Pmac.SendCommand("#4HMZ");
               // PMAC.GetResponse(pmacNumber, "#1->10000X-" + Convert.ToInt64((x_pos)).ToString(), out status);
               // PMAC.GetResponse(pmacNumber, "#2->5000B+" + Convert.ToInt64((b_pos + b_devia_value)).ToString(), out status);
               // x_devia_value = x_pos;
                //b_devia_value = Convert.ToDouble(b_pos) / 16 + b_devia_value; ;
               // Pmac.SendCommand("&1b4r");
                //Delay(5);
               // Pmac.SendCommand("I51=1");
            }
            else
                return;
        }



        private void timer3_Tick(object sender, EventArgs e)//进入加工模式界面开，读取坐标值
        {
            timer5.Interval = 500;
            double dist;
            if (comboBox4.SelectedIndex == 0)
                dist = 0.1;
            else if (comboBox4.SelectedIndex == 1)
                dist = 0.01;
            else
                dist = 0.1;

            if (radioButton12.Checked == true)
            {
                button57.Visible = true;
                button21.Visible = true;
                button58.Visible = true;
                button43.Visible = true;
                radioButton21.Visible = true;
                radioButton22.Visible = true;
               // button40.Visible = false;
            }
            else
            {
               // button40.Visible = true;
                button21.Visible = false;
                button57.Visible = false;
                button58.Visible = false;
                button43.Visible = false;
                radioButton21.Visible = false;
                radioButton22.Visible = false;
            }

            double pos1, pos2,pos3;
            int X_GONG,Z_GONG;
            X_GONG=Pmac.GetM(113);
            Z_GONG = Pmac.GetM(213);
            Pmac.GetPos(1, out pos1);
            Pmac.GetPos(2, out pos2);
            Pmac.GetPos(4, out pos3);
           
            textBox41.Text = Convert.ToString((Math.Truncate(((pos1) / 10000) * 1000) / 1000).ToString("0.000"));//x轴机械坐标
            textBox42.Text = Convert.ToString((Math.Truncate(((pos2) / 5000) * 1000) / 1000).ToString("0.000"));//B轴机械坐标
            textBox32.Text = Convert.ToString((Math.Truncate(((pos3) / 10000) * 1000) / 1000).ToString("0.000"));//z轴机械坐标

            if (dist == 0.01)
            {
                textBox44.Text = Convert.ToString((Convert.ToDouble(X_GONG)*dist).ToString("0.00"));//x轴工件坐标
                textBox43.Text = Convert.ToString(((pos2) / 5000).ToString("0.00"));//B轴工件坐标
                textBox25.Text = Convert.ToString((Convert.ToDouble(Z_GONG) / 1000).ToString("0.00"));//z轴工件坐标
            }
            else
            {
                textBox44.Text = Convert.ToString((Convert.ToDouble(X_GONG) *dist).ToString("0.0"));//x轴工件坐标
                textBox43.Text = Convert.ToString(((pos2) / 5000).ToString("0.0"));//B轴工件坐标
                textBox25.Text = Convert.ToString((Convert.ToDouble(Z_GONG) / 1000).ToString("0.0"));//z轴工件坐标
            }
          
            
            if (radioButton13.Checked == true)
            {
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                textBox19.Enabled = false;
                textBox20.Enabled = false;
                textBox50.Enabled = false;
                comboBox10.Enabled = false;
                comboBox11.Enabled = false;
            }
            if (radioButton13.Checked == false)
            {
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                textBox19.Enabled = true;
                textBox20.Enabled = true;
                textBox50.Enabled = true;
                comboBox10.Enabled = true;
                comboBox11.Enabled = true;
            }

        }

        private void button9_Click(object sender, EventArgs e)//读取加工参数
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string InitialDire = System.Environment.CurrentDirectory;                     
            dialog.Title = "模仁加工参数读取";
            dialog.InitialDirectory = InitialDire + "\\模仁加工参数";
            dialog.Filter = "加工参数文件|*.txt2";
            dialog.ShowDialog();
            string myfile = dialog.FileName;
            //textBox1.Text = Path.GetFileNameWithoutExtension(myfile);
            if (myfile.Trim() == "")
                return;
            StreamReader myreader = null;//new StreamReader();

            myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);

            try
            {
                textBox2.Text = myreader.ReadLine();//恒转速
                textBox3.Text = myreader.ReadLine();//变最大转速

                textBox5.Text = myreader.ReadLine();//进给速度
                textBox4.Text = myreader.ReadLine();//速度倍率
                textBox33.Text = myreader.ReadLine();//抛光头转速
                // textBox32.Text = myreader.ReadLine();//磨头直径
                comboBox12.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//研磨布厚度
                textBox35_1.Text = myreader.ReadLine();//平台高度
                textBox6.Text = myreader.ReadLine();//加工范围左位置
                textBox18.Text = myreader.ReadLine();//加工范围右位置
                textBox37.Text = myreader.ReadLine();//荷重
                //textBox40.Text = myreader.ReadLine();//数据间隔
                textBox39.Text = myreader.ReadLine();//往复回数
               // textBox18.Text = myreader.ReadLine();//加工内径   
                comboBox4.SelectedIndex = Convert.ToInt16(myreader.ReadLine());///数据间隔
                comboBox5.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//磨头直径
                comboBox8.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//抛光轴方向
                comboBox9.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//工件方向
                comboBox1.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//模仁材质
                comboBox3.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//研磨颗粒规格

                if (myreader.ReadLine() == "1")//工件转速
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
                if (myreader.ReadLine() == "1")//进给速度
                    radioButton4.Checked = true;
                else
                    radioButton3.Checked = true;
                if (myreader.ReadLine() == "1")//研磨角度
                    radioButton7.Checked = true;
                else
                    radioButton8.Checked = true;

                if (myreader.ReadLine() == "1")//单段加工
                    radioButton23.Checked = true;
                else
                    radioButton24.Checked = true;

                myreader.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("数据文件格式错误！请检查文件数据格式是否正确。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        //    MessageBox.Show("数据文件格式错误！请检查文件数据格式是否正确。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
            this.button10.Enabled = true;//应用按钮正常
        }

        private void button11_Click(object sender, EventArgs e)//转到加工模式
        {
           // this.tabControl1.SelectedIndex = 6;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = true;
        }

        private void button15_Click(object sender, EventArgs e)//读取面型补正数据
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "面型补正数据读取";
            dialog.InitialDirectory = System.Environment.CurrentDirectory+"\\面型补正数据";
            dialog.Filter = @"面型数据文件(.csv/.MOD)|*.csv;*.MOD";
         //   dialog.Filter = @".csv|*.csv|.MOD|*.MOD";
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (fileName.Trim() == "")
                return;

          

            bool file_property_flag=false;
            string TEX = fileName.Split(char.Parse("."))[1];
           // MessageBox.Show(TEX);
            if (TEX == "MOD" || TEX == "mod")
                file_property_flag=true;
            if (TEX == "csv")
                file_property_flag = false;


           // textBox1.Text = Path.GetFileNameWithoutExtension(fileName);
            
            textBox49.Text = Path.GetFileNameWithoutExtension(fileName);
           // textBox48.Text = Path.GetFileNameWithoutExtension(fileName);
            //文件打开
            //string fileName = @"C:\test.csv";
            StreamReader reader = new StreamReader(fileName);
            //文件内容保存用变量
            string line, line1, line2;
            double[] xx = new double[1000000];
            double[] zzd = new double[1000000];
            double[] num = new double[10000000];
            //读取一行数据到数组
            if (file_property_flag == false)
            {
                int i = 0, j = 0;
                int start = 1;
                bool x_value_flag=true;//判断是读哪行为横坐标
                while ((line = reader.ReadLine()) != null)
                {
                    

                    if (i > 1)
                    {


                        int fist = line.IndexOf(',', start);
                        int second = line.IndexOf(',', fist + 1);
                        int third = line.IndexOf(',', second + 1);
                        // textBox1.Text = Convert.ToString(fist);
                        // textBox2.Text = Convert.ToString(second);
                        
                        if (i == 2)
                        {
                          //  double a = Convert.ToDouble(line.Substring(fist+1, second-fist-1));
                          //  double b = Math.Abs(Convert.ToDouble(line.Substring(0, fist)));

                            if (Math.Abs(Convert.ToDouble(line.Substring(0, fist))) < Math.Abs(Convert.ToDouble(line.Substring(fist + 1, second - fist - 1))))
                                x_value_flag = false;
                        }

                        if(x_value_flag==true)
                            line1 = line.Substring(0, fist);
                        else
                            line1 = line.Substring(fist + 1, second - fist - 1);


                        line2 = line.Substring(third + 1);
                        
                        xx[j] = Convert.ToDouble(line1);
                        zzd[j] = Convert.ToDouble(line2);

                        j++;

                        //  string[] sArray = line.Split(line, ",", RegexOptions.IgnoreCase);

                        // this.listBox1.Items.Add(line1);
                        //this.listBox2.Items.Add(line2);
                    }
                    i++;

                }
                x = new double[j];
                zd = new double[j];
                for (int ii = 0; ii < j; ii++)
                {
                    x[ii] = xx[ii];
                    zd[ii] = zzd[ii];
                    //this.listBox1.Items.Add(Convert.ToString(xx[ii]));
                    // this.listBox2.Items.Add(Convert.ToString(zzd[ii]));


                }
            }
            if(file_property_flag==true)
            {
                int i = 0, j = 0;
               // int start = 1;
                int EOR_TIME=0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "EOR")
                    {
                        EOR_TIME = EOR_TIME + 1;
                        line = reader.ReadLine();
                    }
                        
                    if (EOR_TIME==2&&i>1)
                    {


                      //  int fist = line.IndexOf(',', start);
                       // int second = line.IndexOf(',', fist + 1);
                       // int third = line.IndexOf(',', second + 1);
                        // textBox1.Text = Convert.ToString(fist);
                        // textBox2.Text = Convert.ToString(second);
                       // line1 = line.Substring(0, fist);
                      //  line2 = line.Substring(third + 1);
                      //  xx[j] = Convert.ToDouble(line1);
                     //   zzd[j] = Convert.ToDouble(line2);

                        
                        num[j] = Convert.ToDouble(line);
                        j++;
                        //  string[] sArray = line.Split(line, ",", RegexOptions.IgnoreCase);

                        // this.listBox1.Items.Add(line1);
                        //this.listBox2.Items.Add(line2);
                    }
                    i++;

                }

                x = new double[j/2];
                zd = new double[j/2];

                for (int ii = 0; ii < j; ii++)
                {
                    if(ii<j/2)
                    x[ii] = num[ii];
                    if(ii>=j/2&&ii<j)
                     zd[ii-j/2] = num[ii];
                    //this.listBox1.Items.Add(Convert.ToString(xx[ii]));
                    // this.listBox2.Items.Add(Convert.ToString(zzd[ii]));

                }


            }

            if ((line = reader.ReadLine()) == null)
            {
                MessageBox.Show("读取成功!", "信息提示!");

            }
            read_data_flag = true;
           // num_nh.Text = Convert.ToString(8);
/****************************************运算*************************/






        }

        private void button16_Click(object sender, EventArgs e)//补正运算
        {
            /***************************************************滤波*******************************************************/

            button16.Text = "运算中...";

            Produce_x_b_F_C NC = new Produce_x_b_F_C();

            if (read_data_flag==false)
            {
                MessageBox.Show("运算失败！请先读取面型误差数据!", "信息提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button16.Text = "补正运算";
                return;
           
            }

                int count ;             
                    count = x.Length;
                
                

                int m_b = 0, m_yu = count - m_b, m_ave_zd;//x面型补正数据的x,

                double[] zd_b, zd_yu, ave_zd, ave_zd1, x1, ave_zd2, ave_zd3, zd_d, x_d, zd_d2;//zd_b开始的一段面型误差值，zd_yu余下的一段面型误差值，zd_d取0.0105为间隔的面型误差值（存储加工范围），zd_d2取滤波后0.0105为间隔的面型误差值（存储加工范围），x_d0.0105间隔横坐标，ave_zd面型误差左右对称平均值，x1横坐标，ave_zd1滤波后的平均误差
                double max_ave_zd1 = 0, min_ave_zd1 = 0, PV, PV_rate = 0.3, std_revel,removal;// removal 为           
                double curvature_compensate,n, R, K, dist, t = 0, vc, H, R_left, SAG, yuan_r, ao_tu, R_right, constan_vc, constan_F, symbol = 1, tool_r, D_workpiece, Dp;

               try
               {
                   D_workpiece = Math.Round(Convert.ToDouble(textBox46.Text.Trim()),1);//模仁柱面直径
                   if (D_workpiece * 10 % 2 == 1)
                       D_workpiece = D_workpiece - 0.1;

                    for (int i = 0; i < count; i++)//找出X正值所在位置
                    {
                        if (x[i] > 0)
                            m_b = m_b + 1;//m_b是靠近零第一个X正值所在位置
                    }
                    m_yu = count - m_b;
                    if (m_b <= m_yu)
                    {

                        zd_b = new double[m_b];
                        zd_yu = new double[m_b];
                        ave_zd = new double[m_b];
                        x1 = new double[m_b];

                        for (int i = 0; i < m_b; i++)//提取正并倒转
                        {
                            zd_b[i] = zd[m_b - i - 1];//
                        }
                        for (int i = 0; i < m_b; i++)//提取余下的
                        {
                            zd_yu[i] = zd[m_b + i - 1];//
                        }
                        for (int i = 0; i < m_b; i++)//求zd的均值
                        {

                            ave_zd[i] = (zd_b[i] + zd_yu[i]) / 2;
                            x1[i] = x[m_b - i - 1];


                        }



                    }
                    else
                    {
                        zd_b = new double[m_yu];
                        zd_yu = new double[m_yu];
                        ave_zd = new double[m_yu];
                        x1 = new double[m_yu];

                        for (int i = 0; i < m_yu; i++)//提取正并倒转
                        {
                            zd_b[i] = zd[m_b - i - 1];
                        }
                        for (int i = 0; i < m_yu; i++)//提取余下的
                        {
                            zd_yu[i] = zd[m_b + i - 1];
                        }
                        for (int i = 0; i < m_yu; i++)//求zd的均值
                        {

                            ave_zd[i] = (zd_b[i] + zd_yu[i]) / 2;
                            x1[i] = -x[m_b + i];


                        }

                    }
/******************************提取0.0105为间隔的X与zd*********************************************/
                    int count_num=0;//0.015的个数
                    for(int i=0;i<x1.Length;i++)
                    {
                        if(Math.Abs(x1[i])>=count_num*0.0105)
                            count_num++;
                    }
                  if (radioButton23.Checked == true)//单段加工
                {

                     //zd_d = new double[Convert.ToInt16(Convert.ToDouble(gongD_textBox3.Text) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值
                     //   zd_d2 = new double[Convert.ToInt16(Convert.ToDouble(gongD_textBox3.Text) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用
                     //   x_d = new double[Convert.ToInt16(Convert.ToDouble(gongD_textBox3.Text) / 2 * 10000 / 105) + 1];

                        //zd_d = new double[Convert.ToInt16(-Convert.ToDouble(textBox6.Text)  * 10000 / 105) + 1];//存放0.015为间隔的误差值
                        //zd_d2 = new double[Convert.ToInt16(-Convert.ToDouble(textBox6.Text) * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用
                        //x_d = new double[Convert.ToInt16(-Convert.ToDouble(textBox6.Text) * 10000 / 105) + 1];

                    zd_d = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值
                    zd_d2 = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用
                    x_d = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];
                   

                }
                  else
                    {
                       

                        double[] arry = new double[16];

                        if (textBox40.Text != "")
                            arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                        else
                            arry[0] = 0;

                        if (textBox51.Text != "")
                            arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                        else
                            arry[1] = 0;
                        if (textBox55.Text != "")
                            arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                        else
                            arry[2] = 0;
                        if (textBox54.Text != "")
                            arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                        else
                            arry[3] = 0;
                        if (textBox58.Text != "")
                            arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                        else
                            arry[4] = 0;
                        if (textBox57.Text != "")
                            arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                        else
                            arry[5] = 0;
                        if (textBox61.Text != "")
                            arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                        else
                            arry[6] = 0;
                        if (textBox60.Text != "")
                            arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                        else
                            arry[7] = 0;
                        if (textBox64.Text != "")
                            arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                        else
                            arry[8] = 0;
                        if (textBox63.Text != "")
                            arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                        else
                            arry[9] = 0;
                        if (textBox67.Text != "")
                            arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                        else
                            arry[10] = 0;
                        if (textBox66.Text != "")
                            arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                        else
                            arry[11] = 0;
                        if (textBox73.Text != "")
                            arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                        else
                            arry[12] = 0;
                        if (textBox72.Text != "")
                            arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                        else
                            arry[13] = 0;
                        if (textBox70.Text != "")
                            arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                        else
                            arry[14] = 0;
                        if (textBox69.Text != "")
                            arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                        else
                            arry[15] = 0;

                        zd_d = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值
                        zd_d2 = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用

                        Array.Sort(arry);
                     //   if (Math.Abs(Convert.ToDouble(gongD_textBox3.Text) / 2) > arry[15])
                     //   { 
                          //  zd_d = new double[Convert.ToInt16(Convert.ToDouble(textBox46.Text) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值
                           // zd_d2 = new double[Convert.ToInt16(Convert.ToDouble(textBox46.Text) / 2 * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用
                        x_d = new double[Convert.ToInt16(Convert.ToDouble(D_workpiece) / 2 * 10000 / 105) + 1];
                   
                     //   }
                    //      else
                    //    {
                    //        zd_d = new double[Convert.ToInt16(arry[15] * 10000 / 105) + 1];//存放0.015为间隔的误差值
                    //        zd_d2 = new double[Convert.ToInt16(arry[15] * 10000 / 105) + 1];//存放0.015为间隔的误差值,l滤波用
                     
                  //          x_d = new double[Convert.ToInt16(arry[15] * 10000 / 105) + 1];
                       
                   //     }

                                       


 
                    }
                      count_num=0;
                      double process_radius;//加工口径
                      if (radioButton23.Checked == true)//单段加工
                      {
                         // process_radius = Math.Abs(Convert.ToDouble(textBox6.Text));
                          double[] arry = new double[16];

                          if (textBox40.Text != "")
                              arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                          else
                              arry[0] = 0;

                          if (textBox51.Text != "")
                              arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                          else
                              arry[1] = 0;
                          if (textBox55.Text != "")
                              arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                          else
                              arry[2] = 0;
                          if (textBox54.Text != "")
                              arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                          else
                              arry[3] = 0;
                          if (textBox58.Text != "")
                              arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                          else
                              arry[4] = 0;
                          if (textBox57.Text != "")
                              arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                          else
                              arry[5] = 0;
                          if (textBox61.Text != "")
                              arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                          else
                              arry[6] = 0;
                          if (textBox60.Text != "")
                              arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                          else
                              arry[7] = 0;
                          if (textBox64.Text != "")
                              arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                          else
                              arry[8] = 0;
                          if (textBox63.Text != "")
                              arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                          else
                              arry[9] = 0;
                          if (textBox67.Text != "")
                              arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                          else
                              arry[10] = 0;
                          if (textBox66.Text != "")
                              arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                          else
                              arry[11] = 0;
                          if (textBox73.Text != "")
                              arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                          else
                              arry[12] = 0;
                          if (textBox72.Text != "")
                              arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                          else
                              arry[13] = 0;
                          if (textBox70.Text != "")
                              arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                          else
                              arry[14] = 0;
                          if (textBox69.Text != "")
                              arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                          else
                              arry[15] = 0;


                          Array.Sort(arry);
                          if (Math.Abs(Convert.ToDouble(D_workpiece) / 2) > arry[15])
                              process_radius = Math.Abs(Convert.ToDouble(D_workpiece) / 2);
                          else
                              process_radius = arry[15];
                      }
                      else//多段加工
                      {
                          double[] arry = new double[16];

                          if (textBox40.Text != "")
                              arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                          else
                              arry[0] = 0;

                          if (textBox51.Text != "")
                              arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                          else
                              arry[1] = 0;
                          if (textBox55.Text != "")
                              arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                          else
                              arry[2] = 0;
                          if (textBox54.Text != "")
                              arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                          else
                              arry[3] = 0;
                          if (textBox58.Text != "")
                              arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                          else
                              arry[4] = 0;
                          if (textBox57.Text != "")
                              arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                          else
                              arry[5] = 0;
                          if (textBox61.Text != "")
                              arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                          else
                              arry[6] = 0;
                          if (textBox60.Text != "")
                              arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                          else
                              arry[7] = 0;
                          if (textBox64.Text != "")
                              arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                          else
                              arry[8] = 0;
                          if (textBox63.Text != "")
                              arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                          else
                              arry[9] = 0;
                          if (textBox67.Text != "")
                              arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                          else
                              arry[10] = 0;
                          if (textBox66.Text != "")
                              arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                          else
                              arry[11] = 0;
                          if (textBox73.Text != "")
                              arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                          else
                              arry[12] = 0;
                          if (textBox72.Text != "")
                              arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                          else
                              arry[13] = 0;
                          if (textBox70.Text != "")
                              arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                          else
                              arry[14] = 0;
                          if (textBox69.Text != "")
                              arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                          else
                              arry[15] = 0;


                          Array.Sort(arry);
                          if (Math.Abs(Convert.ToDouble(D_workpiece) / 2) > arry[15])
                              process_radius = Math.Abs(Convert.ToDouble(D_workpiece) / 2);
                          else
                              process_radius = arry[15];


 
                      }
                    if (Math.Abs(x1[x1.Length-1]) > process_radius)//判断数据范围与补偿范围
                    {
                        for (int i = 0; i < x1.Length; i++)
                        {
                            if (Math.Abs(x1[i]) >= count_num * 0.0105&&count_num<x_d.Length)
                            {                               
                                x_d[count_num] = count_num * 0.0105;
                                zd_d[count_num] = ave_zd[i];
                                zd_d2[count_num] = ave_zd[i];                               
                                count_num++;
                            }

                        }

                    }
                    else //数据范围小于补偿加工范围
                    {
                        for (int i = 0; i < x1.Length; i++)
                        {
                            if (Math.Abs(x1[i]) >= count_num * 0.0105)
                            {
                                x_d[count_num] = count_num * 0.0105;
                                zd_d[count_num] = ave_zd[i];
                                zd_d2[count_num] = ave_zd[i];
                                count_num++;
                            }

                        }
                        for (int i = count_num; i < x_d.Length; i++)
                        {
                            x_d[count_num] = count_num * 0.0105;
                            zd_d[count_num] = ave_zd[ave_zd.Length-1];
                            zd_d2[count_num] = ave_zd[ave_zd.Length - 1];
                            count_num++;
                        }
 
                    }

                    for (int i = 0; i < 10; i++)//滤波
                    {
                        zd_d[i] = (zd_d2[i] + zd_d2[i + 1] + zd_d2[i + 2] + zd_d2[i + 2]) / 4;
                    }
                    for (int i = 10; i < 50; i++)//滤波
                    {
                        zd_d[i] = (zd_d2[i - 1] + zd_d2[i - 2] + zd_d2[i - 3] + zd_d2[i] + zd_d2[i + 1] + zd_d2[i + 2] + zd_d2[i + 3]) / 7;

                    }
                   for (int i = 50; i < zd_d.Length-5; i++)//滤波
                    {
                        zd_d[i] = (zd_d2[i - 1] + zd_d2[i - 2] + zd_d2[i - 3] + zd_d2[i - 4] + zd_d2[i - 5] +zd_d2[i] + zd_d2[i + 1] + zd_d2[i + 2] + zd_d2[i + 3] + zd_d2[i +4] + zd_d2[i + 5]) / 11;
                       // zd_d[i]= (zd_d2[i]+zd_d2[i+1])/2;
                    }


/***************************************************************************************/


                  // m_ave_zd = ave_zd.Length;
                    m_ave_zd = zd_d.Length;
                    ave_zd1 = new double[m_ave_zd];//滤波后的平均误差
                    ave_zd2 = new double[m_ave_zd];//整体下移动后的平均误差
                    ave_zd3 = new double[m_ave_zd];

                    for (int i = 0; i < m_ave_zd ; i++)//滤波
                    {
                        ave_zd1[i] = zd_d[i];
                    }
               /*  for (int i = 0; i < 10; i++)//滤波
                    {
                        ave_zd1[i] = (ave_zd[i + 1] + ave_zd[i + 2] + ave_zd[i + 3]) / 3;
                    }
                    for (int i = 10; i < 50; i++)//滤波
                    {
                        ave_zd1[i] = (ave_zd[i - 1] + ave_zd[i - 2] + ave_zd[i - 3] + ave_zd[i + 1] + ave_zd[i + 2] + ave_zd[i + 3]) / 6;

                    }
                    for (int i = 50; i < m_ave_zd - 5; i++)//滤波
                    {
                        ave_zd1[i] = (ave_zd[i - 1] + ave_zd[i - 2] + ave_zd[i - 3] + +ave_zd[i - 4] + ave_zd[i - 5] + ave_zd[i + 1] + ave_zd[i + 2] + ave_zd[i + 3] + +ave_zd[i + 4] + ave_zd[i + 5]) / 10;

                    }
                    //m_ave_zd = ave_zd.Length;
                 /*    m_ave_zd = zd_d.Length;
                    ave_zd1 = new double[m_ave_zd];//滤波后的平均误差
                    ave_zd2 = new double[m_ave_zd];//整体下移动后的平均误差
                    ave_zd3 = new double[m_ave_zd];

                    for (int i = 0; i < 10; i++)//滤波
                    {
                        ave_zd1[i] = (zd_d[i] + zd_d[i + 1] + zd_d[i + 2]) / 3;
                    }
                    for (int i = 10; i < zd_d.Length - 3; i++)//滤波
                    {
                        ave_zd1[i] = (zd_d[i - 1] + zd_d[i - 2] + zd_d[i - 3] + zd_d[i + 1] + zd_d[i + 2] + zd_d[i + 3]) / 6;

                    }
                    /*
        /***********拟合*****************************************/
                    double temp1 = ave_zd1[0], temp2 = ave_zd1[0];
                    //int max_index=0,min_index=0;

                 //   int test = Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox6.Text)) * 10000 / 105) + 1;
                    int c = Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox6.Text)) * 10000 / 105) + 1;
                    int d = Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox18.Text)) * 10000 / 105) + 1;
                    int compare_range = c;
                    if (c > d)
                    {
                        compare_range = c;

                    }
                    else
                    {
                        compare_range = d;
                    }


                    for (int i = 0; i < compare_range + 1 - 5; i++)//取zd1中最大最小值
                    {

                        // temp1 = ave_zd1[0];
                        //temp2 = ave_zd1[0];
                        if (temp1 < ave_zd1[i])
                        {
                            temp1 = ave_zd1[i];
                            // max_index = i;
                        //    c = i;
                        }

                        if (temp2 > ave_zd1[i])
                        {
                            temp2 = ave_zd1[i];
                            //  min_index = i;
                        }

                    }

                    for (int i = compare_range + 1 - 5; i < compare_range + 1; i++)//取zd1中最大最小值
                    {

                        // temp1 = ave_zd1[0];
                        //temp2 = ave_zd1[0];
                        if (temp1 < zd_d2[i])
                        {
                            temp1 = zd_d2[i];
                            // max_index = i;
                          //  c = i;
                        }

                        if (temp2 > zd_d2[i])
                        {
                            temp2 = zd_d2[i];
                            //  min_index = i;
                        }

                    }
                    max_ave_zd1 = temp1;
                    min_ave_zd1 = temp2;
                  //  max_ave_zd1 = -0.0000107559;
                    PV = max_ave_zd1 - min_ave_zd1;

                    std_revel = min_ave_zd1 - (PV * PV_rate);

                    for (int i = 0; i < m_ave_zd; i++)
                    {
                        ave_zd2[i] = std_revel - ave_zd1[i];
                    }
                    double[] a = new double[21];
                    double[] b;
                    b = Produce_x_b_F_C.MultiLine(x_d, ave_zd2, ave_zd2.Length, Convert.ToInt16(8));//拟合
                    for (int i = 0; i < Convert.ToInt16(8) + 1; i++)
                    {
                        // if (b[i] != 0)
                        a[i] = b[i];

                    }
                    //   Lagrange lageran= new Lagrange(x1,ave_zd2);
                    //  this.textBox16.Text = a[0].ToString() + a[1].ToString() + a[2].ToString() + a[3].ToString() + a[4].ToString() + a[5].ToString() + a[6].ToString() + a[7].ToString() + a[8].ToString() + a[9].ToString() + a[10].ToString() + a[11].ToString() + a[12].ToString() + a[13].ToString() + a[14].ToString() + a[15].ToString();
                    for (int i = 0; i < m_ave_zd; i++)
                    {
                        //ave_zd3[i] = a[0] + a[1] * x1[i] + a[2] * Math.Pow(x1[i], 2) + a[3] * Math.Pow(x1[i], 3) + a[4] * Math.Pow(x1[i], 4) + a[5] * Math.Pow(x1[i], 5) + a[6] * Math.Pow(x1[i], 6) + a[7] * Math.Pow(x1[i], 7) + a[8] * Math.Pow(x1[i], 8) + a[9] * Math.Pow(x1[i], 9) + a[10] * Math.Pow(x1[i], 10) + a[11] * Math.Pow(x1[i], 11) + a[12] * Math.Pow(x1[i], 7) + a[12] * Math.Pow(x1[i], 7) + a[13] * Math.Pow(x1[i], 13) + a[14] * Math.Pow(x1[i], 14) + a[15] * Math.Pow(x1[i], 15) + a[16] * Math.Pow(x1[i], 16) + a[17] * Math.Pow(x1[i], 17) + a[18] * Math.Pow(x1[i], 18) + a[19] * Math.Pow(x1[i], 19) + a[20] * Math.Pow(x1[i], 20);
                        //  ave_zd3[i] = lagelangri(x1, ave_zd2, x1[i]+0.1, m_ave_zd);
                        // ave_zd3[i] = lageran.GetValue(x1[i]);

                    }
               }
               catch (Exception)
               {
                   MessageBox.Show("数据文件格式错误！请检查文件数据格式是否正确！", "信息提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   button16.Text = "补正运算";
                   return;
               }

                /************************************画出拟合后的曲线图**************************************************/





                /***********************计算未补正时的原始代码*************************************/
               double D_end=0;

                try
                {
                    double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };


                    n = Convert.ToDouble(textBox39.Text);//加工次数
                    R = Convert.ToDouble(R_text.Text);  //面型参数R
                    K = Convert.ToDouble(K_text.Text);//面型参数K
                    vc = Convert.ToDouble(textBox3.Text);//C轴最大转速
                    H = Convert.ToDouble(TextBox35.Text);//模仁高度
                    Dp = Convert.ToDouble(gongD_textBox3.Text.Trim());//加工口径
                    D_workpiece = Math.Round(Convert.ToDouble(textBox46.Text.Trim()),1);//模仁柱面直径

                    if (D_workpiece * 10 % 2 == 1)
                        D_workpiece = D_workpiece - 0.1;

                    if (radioButton23.Checked == true)//单段程序加工
                    {


                       // D = Convert.ToDouble(textBox6.Text);//加工口径
                      //  D_end = 0;// Convert.ToDouble(textBox18.Text);//加工口径另一值
                        if (textBox6.Text == "")
                            R_left = 0;
                        else
                            R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

                        if (textBox18.Text == "")
                            R_right = 0;
                        else
                            R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值

                 
                        // textBox18.text
                       // if (Math.Abs(R_left) < arry[15])
                       //     R_left = arry[15];

                        if (Math.Abs(R_left) > D_workpiece / 2)
                        {
                            MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                            button16.Text = "补正运算";
                            return;
                        }
                      

                        if (Math.Abs(R_right) > D_workpiece / 2)
                        { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); button16.Text = "补正运算"; return; }
                        if (R_left >= R_right)
                        { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); button16.Text = "补正运算"; return; }

                        R_left = -D_workpiece / 2;//加工范围半径


                        R_right = D_workpiece / 2;//加工口径另一值

                    }
                    else
                    {
                        double[] arry = new double[16];

                        if (textBox40.Text != "")
                            arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                        else
                            arry[0] = 0;

                        if (textBox51.Text != "")
                            arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                        else
                            arry[1] = 0;
                        if (textBox55.Text != "")
                            arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                        else
                            arry[2] = 0;
                        if (textBox54.Text != "")
                            arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                        else
                            arry[3] = 0;
                        if (textBox58.Text != "")
                            arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                        else
                            arry[4] = 0;
                        if (textBox57.Text != "")
                            arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                        else
                            arry[5] = 0;
                        if (textBox61.Text != "")
                            arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                        else
                            arry[6] = 0;
                        if (textBox60.Text != "")
                            arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                        else
                            arry[7] = 0;
                        if (textBox64.Text != "")
                            arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                        else
                            arry[8] = 0;
                        if (textBox63.Text != "")
                            arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                        else
                            arry[9] = 0;
                        if (textBox67.Text != "")
                            arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                        else
                            arry[10] = 0;
                        if (textBox66.Text != "")
                            arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                        else
                            arry[11] = 0;
                        if (textBox73.Text != "")
                            arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                        else
                            arry[12] = 0;
                        if (textBox72.Text != "")
                            arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                        else
                            arry[13] = 0;
                        if (textBox70.Text != "")
                            arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                        else
                            arry[14] = 0;
                        if (textBox69.Text != "")
                            arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                        else
                            arry[15] = 0;


                        Array.Sort(arry);

                        R_left = -D_workpiece / 2;//加工范围半径


                        R_right = D_workpiece / 2;//加工口径另一值
                        // textBox18.text
                        //if (Math.Abs(R_left) < arry[15])
                        //    R_left = arry[15];

                        if (Math.Abs(arry[15]) > D_workpiece / 2)
                        {
                            MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                            button16.Text = "补正运算";
                            return;
                        }
                        //R_left = -D_workpiece / 2;//加工范围半径


                        //R_right = D_workpiece / 2;//加工口径另一值
                        //if (Math.Abs(R_left) > D_workpiece / 2)
                        //{
                        //    MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                        //    return;
                        //}

                        //if (Math.Abs(R_right) > D_workpiece / 2)
                        //{ MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }
                        //if (R_left >= R_right)
                        //{ MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }
                        ////if (Math.Abs(Convert.ToDouble(gongD_textBox3.Text)) > arry[15] * 2)
                        //    D = Math.Abs(Convert.ToDouble(gongD_textBox3.Text));//加工口径（形状参数）
                        //else
                        //    D = arry[15] * 2;

                         D_end = 0;//加工口径另一值
                    }
                  

                    SAG = Convert.ToDouble(SAG_text.Text);//非球面中心到平面距离
                    yuan_r = Convert.ToDouble(yuanr_text.Text);//夹具高度，从移动调心夹具底面到模仁底面距离
                    if (yuan_r < 0.1)
                        yuan_r = 0.1;
                    constan_vc = Convert.ToDouble(textBox2.Text);//恒定C转速
                    constan_F = Convert.ToDouble(textBox5.Text);//恒定进给
                    if (ao_non_sphere.Checked == true)//凹凸判断
                        ao_tu = -1;
                    else
                        ao_tu = 1;

                    /* 
                     if (comboBox4.SelectedIndex == 0)
                         dist = 0.1;
                     else
                         dist = 0.05;
                     if (ao_non_sphere.Checked == true)
                     {
                         if (R > 0)
                             symbol = 1;
                         else
                             symbol = -1;

                     }
                     if (tu_non_sphere.Checked == true)
                     {
                         if (R > 0)
                             symbol = -1;
                         else
                             symbol = 1;

                     }

                     NC_Data = NC.asphere(dist,symbol, n, vc, H, D, R, K, A, out t);//生成代码
         */
                    if (comboBox5.SelectedIndex == 0)

                    { 
                        tool_r = 1;
                        removal = 0.006667;
                    }
                    else if (comboBox5.SelectedIndex == 1)
                    {
                        tool_r = 3;
                        removal = 0.0125;
                    }
                    else
                    {
                        tool_r = 5;
                        removal = 0.0185;
                    }
                    if (radioButton7.Checked == true)//垂直抛
                    {
                        tool_r = 7; 
                    }

                    if (comboBox4.SelectedIndex == 0)
                        dist = 0.1;
                    else if (comboBox4.SelectedIndex == 1)
                        dist = 0.01;
                    else
                        dist = 0.01;

                    if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
                    {
                        if (ao_non_sphere.Checked == true)
                        {
                            if (R > 0)
                                symbol = 1;
                            else
                                symbol = -1;

                        }
                        if (tu_non_sphere.Checked == true)
                        {
                            if (R > 0)
                                symbol = -1;
                            else
                                symbol = 1;
                        }
                        if (comboBox13.SelectedIndex == 1)
                            curvature_compensate = 1;
                        else if (comboBox13.SelectedIndex == 2)
                            curvature_compensate = 2;
                        else
                            curvature_compensate = 0;

                        //textBox47.Text = Convert.ToString(symbol);//看看symbol对不对
                        // NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
                        //if (radioButton14.Checked == true)
                        //    NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
                      
                        //      //else if (radioButton16.Checked == true)
                        //      //NC_Data = NC.asphere_heitian2(SAG,fixture_h,ao_tu,D_end,tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

                        //else
                        NC_Data = NC.asphere_heitian(curvature_compensate,first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
                     

                        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
                    }
                    if (ao_sphere.Checked == true || tu_sphere.Checked == true)
                    {
                        if (ao_sphere.Checked == true)
                            symbol = 1;
                        if (tu_sphere.Checked == true)
                            symbol = -1;
                        NC_Data = NC.sphere(first_position_feed,D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
                     
                       // textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间


                    }
                    if (plane.Checked == true)
                    {
                      //  NC_Data = NC.plane(C_motor_scale_factor, C_motor_offset, fixture_h, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
                        NC_Data = NC.plane_heitian(first_position_feed, D_workpiece, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
               
                        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间

                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("参数设定发生错误！请检查加工参数和形状参数是否设定。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button16.Text = "补正运算";
                    return;
                }
                /*******************************计算进给F***************************************************************/
            
                // min_ave_zd1 = min_ave_zd1 * 1000;
              //  double D_end = 1;
                #region  
                if (D_end == 0) 
                {

                    GraphPane myPane;

                    
                    // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
                    myPane = zedGraphControl1.GraphPane;
                    myPane.CurveList.Clear();
                        // CurveItem myCurve3 = myPane.AddCurve("补正误差", list3, Color.Blue, SymbolType.None);

                  //  myPane.AxisChange();
                    zedGraphControl1.Refresh();
/******************************************************迭代补偿*****************************************/

                     std_revel = std_revel * 1000;
                    int num_cycle, num_cycle1;//初次抛光次数
                    double first_num;
                    double P_or_N = 1;//x2与x1正负要相同；
                    double[] sim = new double[NC_Data.Length / 14];
                    double[] x2 = new double[NC_Data.Length / 14];
                    double[] y2 = new double[NC_Data.Length / 14];
                    int[] y2_temp = new int[NC_Data.Length / 14];
                    double[] rate = new double[NC_Data.Length / 14];
                    double[] rate1 = new double[NC_Data.Length / 7];
                    double SIM = 1, zd_temp_x_1 = 1;
                    double pressure_cycle_effect = 1, C_speed_cycle_effect = 1, P_speed_cycle_effect = 1,Cloth_diaman_effect=1;//压力，抛光轴速度和C轴速度,黄布与颗粒对抛光次数影响。
                    double multiple_rate = 1;//速度倍率
                    // num_cycle = Convert.ToInt16(-min_ave_zd1 / 0.0125);
                    if (textBox4.Text != "")
                        multiple_rate = Convert.ToDouble(textBox4.Text.Trim())*0.01;
                    if (textBox37.Text != "")//抛压力影响抛光次数
                        pressure_cycle_effect = Math.Pow(Math.Abs(150/Convert.ToDouble(textBox37.Text.Trim())), 0.65);
                    if (textBox2.Text != "")//C轴转速影响抛光次数
                        C_speed_cycle_effect = Math.Pow(Math.Abs(150 / Convert.ToDouble(textBox2.Text.Trim())), 0.5);
                    if (textBox33.Text != "")//抛光轴转速影响抛光次数
                        P_speed_cycle_effect = Math.Pow(Math.Abs(180 / Convert.ToDouble(textBox33.Text.Trim())), 0.5);
                    if (this.comboBox12.SelectedIndex == 1)//黄布的规格选择
                        Cloth_diaman_effect = 0.555;

                    //  this.textBox17.Text = a[0].ToString() + a[1].ToString() + a[2].ToString() + a[3].ToString() + a[4].ToString() + a[5].ToString() + a[6].ToString() + a[7].ToString() + a[8].ToString() + a[9].ToString() + a[10].ToString() + a[11].ToString() + a[12].ToString() + a[13].ToString() + a[14].ToString() + a[15].ToString();

                    if (x1[2] * NC_Data[2, 4] > 0)
                        P_or_N = 1;
                    else
                        P_or_N = -1;
                    num_cycle = Convert.ToInt16(zd_temp_x_1 / removal);
                    // this.textBox17.Text = P_or_N.ToString();
                    for (int i = 0; i < NC_Data.Length / 14; i++)//计算倍率
                    {
                        //x2[i] = P_or_N*NC_Data[NC_Data.Length / 10 + i, 4];
                        x2[i] = NC_Data[NC_Data.Length / 14 + i, 4];
                        // x2[i] = 0.1 * i;
                        //    y2[i] = a[0] + a[1] * x2[i] + a[2] * Math.Pow(x2[i], 2) + a[3] * Math.Pow(x2[i], 3) + a[4] * Math.Pow(x2[i], 4) + a[5] * Math.Pow(x2[i], 5) + a[6] * Math.Pow(x2[i], 6) + a[7] * Math.Pow(x2[i], 7) + a[8] * Math.Pow(x2[i], 8) + a[9] * Math.Pow(x2[i], 9) + a[10] * Math.Pow(x2[i], 10) + a[11] * Math.Pow(x2[i], 11) + a[12] * Math.Pow(x2[i], 7) + a[12] * Math.Pow(x2[i], 7) + a[13] * Math.Pow(x2[i], 13) + a[14] * Math.Pow(x2[i], 14) + a[15] * Math.Pow(x2[i], 15) + a[16] * Math.Pow(x2[i], 16) + a[17] * Math.Pow(x2[i], 17) + a[18] * Math.Pow(x2[i], 18) + a[19] * Math.Pow(x2[i], 19) + a[20] * Math.Pow(x2[i], 20);
                        int index = 0;//s
                        double temp = Math.Abs(x2[i] + P_or_N * x_d[0]);
                        //listBox1.Items.Add("y"+i.ToString());
                        for (int ii = 0; ii < x_d.Length; ii++)
                        {
                            double tep = Math.Abs(x2[i] + P_or_N * x_d[ii]);
                            //  listBox1.Items.Add(tep.ToString());

                            if (temp > tep)
                            {
                                index = ii;
                                temp = tep;
                                // MessageBox.Show("1");
                            }
                           // if (Math.Abs(1 + x_d[ii]) < 0.1)//1附件的点为补偿基准点
                            ///{
                           //     zd_temp_x_1 = Math.Abs(ave_zd2[ii] * 1000);
                            //}
                            if(ave_zd1.Length<97)
                                zd_temp_x_1 = Math.Abs(ave_zd2[ave_zd1.Length-1] * 1000);
                            else
                                zd_temp_x_1 = Math.Abs(ave_zd2[96] * 1000);//1附件的点为补偿基准点
                                
                        }
                        //if (radioButton26.Checked == true)//补正模式2选中
                        //    zd_temp_x_1 = Math.Abs(PV / 2) * 1000;

                        index = i * 10*100/105;
                        // SIM = zd_temp_x_1 / 0.05;
                        num_cycle = Convert.ToInt16(zd_temp_x_1 / removal);
                        num_cycle1 = num_cycle;
                        compens_cycle_time = Convert.ToDouble(num_cycle);
                     
                         first_num = Convert.ToDouble(num_cycle1) * removal;
                        SIM = -num_cycle * removal;
                       // SIM = 0.1;
                                y2[i] = ave_zd2[index];
                            y2_temp[i] = index;
                      
                                   rate[i] = y2[i] * 1000 / SIM;
                    }
                       if (compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate > 1)
                        {
                            num_nh.Text = Convert.ToString(Convert.ToInt16(compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate));//补正界面补正次数
                            textBox17.Text = Convert.ToString(Convert.ToInt16(compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate));//代码界面补正次数

                        }
                        else
                        {
                            num_nh.Text = "1";
                            textBox17.Text="1";
 
                        }
                    if (this.comboBox3.SelectedIndex == 0 || this.comboBox12.SelectedIndex==0)//研磨膏规格与布的规格选择
                    {
                        if (num_cycle <= 1)
                        {
                            compens_cycle_time = Convert.ToInt16(num_cycle);
                            num_nh.Text = Convert.ToString(compens_cycle_time);
                            textBox17.Text = Convert.ToString(compens_cycle_time);
                        }
                        else
                        {
                            double cloth_flag = 1, diamon_compd = 1;
                            if (this.comboBox3.SelectedIndex == 0)//蓝膏
                                diamon_compd = 0.714;
                            if (this.comboBox12.SelectedIndex == 0)//白布
                                cloth_flag = 0.5;

                            compens_cycle_time = Convert.ToInt16(Math.Floor(num_cycle * diamon_compd * cloth_flag * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * multiple_rate));
                            if (compens_cycle_time < 1)
                                compens_cycle_time = 1;
                            num_nh.Text = Convert.ToString(compens_cycle_time);
                            textBox17.Text = Convert.ToString(compens_cycle_time);

                        }
                    }

/*****************************************读取excel数据*********************************************************************/
                   
                    string strConn;
                    string spot_data;
                    if (comboBox5.SelectedIndex == 0)
                        spot_data = "data1.xlsx";
                    else if (comboBox5.SelectedIndex == 1)
                        spot_data = "data2.xlsx";
                    else
                        spot_data = "data3.xlsx";
                    string datapath="C:\\Program Files\\Microsoft Office\\Office12";
                   // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + System.Environment.CurrentDirectory + "\\data.xlsx" + ";Extended Properties=Excel 8.0;";
                    strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + datapath + "\\" + spot_data + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
                    if (Path.GetExtension(System.Environment.CurrentDirectory + "\\" + spot_data).Trim().ToUpper() == ".XLSX")            
                    {
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + datapath + "\\" + spot_data + ";Extended Properties=\"Excel 12.0;HDR=YES\"";       
                    }
                    double[,] data = new double[x_d.Length+5, Convert.ToInt16(x_d[x_d.Length - 1] * 10)+1];//存放原始表格数据
                    double[,] data2 = new double[x_d.Length+5, Convert.ToInt16(x_d[x_d.Length - 1] * 10)+1];//存放迭代数据
                    double[] data3 = new double[x_d.Length];//存放理论去除量
                    OleDbConnection conn = new OleDbConnection(strConn);
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                   
                        myCommand.Fill(myDataSet);
                        try
                        {
                        for (int i = 0; i < x_d.Length; i++)//读取数据
                        {
                            if (i < 2098)
                            {
                                for (int j = 0; j < x_d[x_d.Length - 1] * 10; j++)
                                {
                                    if (j < 255)
                                    {
                                        data[i, j] = Convert.ToDouble(myDataSet.Tables[0].Rows[i][j]);
                                    }
                                    else
                                    {
                                        //if (i == 0)
                                        //   data[i, j] = -0.375;
                                        //  else
                                        //    data[i, j] = Convert.ToDouble(myDataSet.Tables[0].Rows[i][j-100]);                                  
                                    }

                                }
                            }
                            else
                            {
 
                            }

                        }

                        //string path = System.Environment.CurrentDirectory + "//rate.txt";
                        //StreamReader myfile = new StreamReader(path);                    
                        //for (int i = 0; i < rate.Length; i++)
                        //{
                        //    rate[i] = Convert.ToDouble(myfile.ReadLine());
                        //}

                        for (int i = 0; i < x_d.Length; i++)//乘上倍率
                        {

                            if (i < 2098)
                            {
                                for (int j = 0; j < rate.Length; j++)
                                {
                                    data2[i, j] = data[i, j] * rate[j];
                                    data3[i] = data3[i] + data2[i, j];
                                }
                                data3[i] = data3[i] / -0.37315 * SIM;
                            }
                            else
                            {
                                data3[i] = data3[2020];
                                
                            }

                        }

                        



                        /**********************************************debug数据输出txt*********************************************************/
                        //string result1 = @"D:\data3数据.txt";//结果保存到D:\data3数据.txt
                        ////先清空data3数据.txt文件内容
                        //FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
                        //stream2.Seek(0, SeekOrigin.Begin);
                        //stream2.SetLength(0); //清空txt文件
                        //stream2.Close();


                        //FileStream fs = new FileStream(result1, FileMode.Append);
                        //// fs.Seek(0, SeekOrigin.Begin);
                        //// fs.SetLength(0);
                        //StreamWriter wr = null;

                        //wr = new StreamWriter(fs);
                        //wr.WriteLine("data3:");
                        //for (int i = 0; i < data3.Length; i++)
                        //{
                        //    wr.WriteLine((std_revel - data3[i]).ToString());

                        //}
                        //wr.Close();

                    
                       // /**************************************************************************************************************/



   //*************************收敛运算*********************  //

                        double er1 = 0.03;
                        double er2 = 0.02;
                        double er3 = 0.01;
                        double sc1 = 0.05;
                        double sc2 = 0.03;
                        double sc3 = 0.01;
                        double sc4 = 0;
                        double sc = 0;
                        double max_er =  er3*1.1;
                        int k = 0;
                        int offset = 0;
                        while (max_er > er3 && k < 20)
                        {


                            max_er = er3 * 1.1;//最大误差赋初始值
                            for (int j = 0; j < Convert.ToInt16(x_d[x_d.Length - 1] * 10); j++)//倍率
                            {
                                //if (j <= 8)
                                //    offset = 0;
                                //else
                                //    offset = -8;
                                if (j < 220)
                                {

                                    if (Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) >= er1)//根据补正误差判断增减倍率量
                                        sc = sc1;
                                    else if (Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) >= er2 && Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) < er1)




                                        sc = sc2;
                                    else if (Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) >= er3 && Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) < er2)
                                        sc = sc3;
                                    else if (Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) <= er3)
                                        sc = sc4;
                                    if (Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000) >= max_er)
                                        max_er = Math.Abs(data3[j * 10 * 100 / 105 + offset] - ave_zd2[j * 10 * 100 / 105 + offset] * 1000);

                                    if (data3[j * 10 * 100 / 105 + offset] <= ave_zd2[j * 10 * 100 / 105 + offset] * 1000)//补正误差为正时
                                    {
                                        if (rate[j] > 0.3)
                                        {
                                            if (j == 0 || j == 1)
                                                rate[j] = rate[j] - sc;
                                            else if (j == 2)
                                            {
                                                rate[j] = rate[j] - sc / 2;
                                                rate[j - 1] = rate[j - 1] - sc / 2;
                                            }
                                            else
                                                rate[j - 1] = rate[j - 1] - sc;
                                        }

                                    }
                                    else if (data3[j * 10 * 100 / 105 + offset] > ave_zd2[j * 10 * 100 / 105 + offset] * 1000)   //补正误差为负时                          
                                    {
                                        if (rate[j] < 5)
                                        {
                                            if (j == 0 || j == 1)
                                                rate[j] = rate[j] + sc;
                                            else if (j == 2)
                                            {
                                                rate[j] = rate[j] + sc / 2;
                                                rate[j - 1] = rate[j - 1] + sc / 2;
                                            }
                                            else
                                                rate[j - 1] = rate[j - 1] + sc;
                                        }
                                    }
                                  }

                                }

                                for (int i = 0; i < x_d.Length; i++)//乘上倍率,i行，j列
                                {

                                    if (i < 2098)
                                    {

                                        data3[i] = 0;
                                        for (int j = 0; j < rate.Length; j++)
                                        {
                                            data2[i, j] = data[i, j] * rate[j];
                                            data3[i] = data3[i] + data2[i, j];
                                        }
                                        data3[i] = data3[i] / -0.37315 * SIM;
                                    }
                                        
                                   else
                                   {
                                        data3[i] = data3[2020];
                                
                                   }


                                }

                                k++;
                            

                        }
                       // double c = Convert.ToDouble(myDataSet.Tables[0].Rows[0][0]);
                       // MessageBox.Show(c.ToString());
                        }
                        catch (Exception ex)
                        {
                            // throw new InvalidFormatException("该Excel文件的工作表的名字不正确," + ex.Message);
                          //  MessageBox.Show(ex.Message);
                            button16.Text = "补正运算";
                        }
    /**********************************************debug数据输出txt*********************************************************/
                      
                        /**************************************************************************************************************/

    
                   
               /**************************************************************************************************************/


                    rate1[0] = 1;
                    for (int i = 0; i < NC_Data.Length / 14; i++)
                    {
                        rate1[i + 1] = rate[NC_Data.Length / 14 -1 - i];

                    }
                    for (int i = NC_Data.Length / 14; i < NC_Data.Length / 7 - 1; i++)
                    {
                        rate1[i + 1] = rate[i - NC_Data.Length / 14];
                    }
                    //  rate1[NC_Data.Length / 5 - 1] = rate[ NC_Data.Length / 10-1];
                    F1 = new double[NC_Data.Length];
                    rate_dist = new double[NC_Data.Length];
                    for (int i = 0; i < NC_Data.Length / 7; i++)
                    {
                        F1[i] = NC_Data[i, 2]/ (rate1[i]);
                        rate_dist[i] = rate1[i];//
                    }
                    

   /**********************************************绘图*****************************************************/



                    /********************************画在zedgraph上********************************************************/
                    // zedGraphControl1.Dispose();
                    myPane.XAxis.Title.Text = "半径(mm)";
                    myPane.YAxis.Title.Text = "误差(um)";
                    myPane.Title.Text = " ";

                    myPane.CurveList.Clear();
                    myPane.GraphObjList.Clear();
                   
                    // 设置初始数据
                    double xx, y11, y22;
                    PointPairList list1 = new PointPairList();
                    PointPairList list2 = new PointPairList();
                    PointPairList list3 = new PointPairList();
                    PointPairList list4 = new PointPairList();
                    PointPairList list5 = new PointPairList();
                    PointPairList list6 = new PointPairList();
                    int list1_dataRange = 0;
                    int list2_dataRange=0;
                    double tempp = 0;
                  try
      {                  
                     
                    if(Math.Abs(Convert.ToDouble(textBox6.Text))>Math.Abs(Convert.ToDouble(textBox18.Text)))
                        tempp=Math.Abs(Convert.ToDouble(textBox6.Text));
                    else
                         tempp=Math.Abs(Convert.ToDouble(textBox18.Text));

                     list1_dataRange = Convert.ToInt16(Convert.ToDouble(gongD_textBox3.Text.Trim())/2 * 10000 / 105) + 1;
                     list2_dataRange = Convert.ToInt16(tempp * 10);
                     list1_dataRange = Convert.ToInt16(tempp * 10000 / 105)+1;
                  }

                    catch(Exception err)
                    {
                        MessageBox.Show(err.Message);
                        button16.Text = "补正运算";
                        return;
                    }

                    
                    for (int i = list1_dataRange - 1; i > 0; i--)
                    {
                        //xx = x1[i];
                        xx = x_d[i];
                        y11 = ave_zd2[i];
                        //  y22 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                        list1.Add(xx, y11 * 1000);
                       

                    }
                    for (int i = 0; i < list1_dataRange; i++)
                    {
                        //xx = x1[i];
                        xx = x_d[i];
                        y11 = ave_zd2[i];
                       
                        list1.Add(-xx, y11 * 1000);
                       

                    }
                    for (int i =  list2_dataRange - 1; i > 0; i--)
                    {
                        xx = x2[i];
                       // y11 = y2[i];
                        y11 = data3[i*10*100/105]/1000;
                        y22 = ave_zd2[y2_temp[i]] - data3[i * 10 * 100 / 105] / 1000;
                        list2.Add(xx, y11 * 1000);
                        list3.Add(xx, y22 * 1000);
                       
                    }
                    for (int i = 0; i < list2_dataRange; i++)
                    {
                        xx = x2[i];
                        //y11 = y2[i];
                        y11 = data3[i *10 *100 / 105] / 1000;
                        y22 = ave_zd2[y2_temp[i]] - data3[i * 10 * 100 / 105] / 1000;
                        list2.Add(-xx, y11 * 1000);
                        list3.Add(-xx, y22 * 1000);
                      
                    }

                   
                    LineItem myCurve = myPane.AddCurve("实际应去除量", list1, Color.Red, SymbolType.None);
                     
                    LineItem myCurve2 = myPane.AddCurve("拟合应去除量", list2, Color.Black, SymbolType.None);
                  
                  LineItem myCurve3 = myPane.AddCurve("补正误差", list3, Color.Blue, SymbolType.None);
                   // CurveItem myCurve3 = myPane.AddCurve("补正误差", list3, Color.Blue, SymbolType.None);

                    myPane.AxisChange();
                    zedGraphControl1.Refresh();
                    /******************test*********************/

                   /* string result1 = @"D:\测试.txt";//结果保存到F:\result1.txt

                    //先清空result1.txt文件内容
                    FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
                    stream2.Seek(0, SeekOrigin.Begin);
                    stream2.SetLength(0); //清空txt文件
                    stream2.Close();


                    FileStream fs = new FileStream(result1, FileMode.Append);
                    // fs.Seek(0, SeekOrigin.Begin);
                    // fs.SetLength(0);
                    StreamWriter wr = null;

                    wr = new StreamWriter(fs);
                    //wr.WriteLine("OPEN PROG 2");
                    //wr.WriteLine("CLEAR");
                    //wr.WriteLine("G90 G01");
                    //wr.WriteLine("WHILE(P25<=" + textBox39.Text + ")");
                    //for (int i = 0; i < NC_Data.Length / 12; i++)
                    //{
                    //    wr.WriteLine(Convert.ToString("X2" + "  " + x2[i].ToString("f4") + "  " + "y2" + "  " + y2[i] + "  " + "F1" + F1[i].ToString("f4") + "  " + "sim" + "  " + sim[i].ToString("f4") + "  " + "rate" + "  " + rate[i].ToString("f4") + "  " + "rate1" + "  " + rate1[i].ToString("f4") + "ave_zd1" + "  " + ave_zd1[i].ToString() + "ave_zd2" + "  " + ave_zd2[i].ToString() + "\n"));
                    //}
                    wr.WriteLine("F1:" + "\n");
                    for (int i = NC_Data.Length / 14+1; i < NC_Data.Length / 7; i++)
                    {
                        wr.WriteLine(F1[i].ToString("f4"));
                    }
                    wr.WriteLine("rate1" + "\n");
                    for (int i = 0; i < NC_Data.Length / 7; i++)
                    {
                        wr.WriteLine(rate1[i].ToString("f4"));

                    }
                    for (int i = 0; i < NC_Data.Length / 14; i++)
                    {
                        wr.WriteLine("X2" + "  " + x2[i].ToString("f4") + "\n");

                    }
                   for (int i = 0; i < m_ave_zd; i++)
                    {
                        wr.WriteLine("data3:" + data3[i].ToString() + "  " + "ave_zd2:" + ave_zd2[i].ToString() + "  " + "sub:" + (ave_zd2[i]*1000-data3[i]).ToString() + "  " + "x_d:" + x_d[i].ToString() + "  " + "stdl" + std_revel.ToString() + "    ave_zd1/stdl" + (ave_zd1[i] / std_revel * 1000).ToString() + "\n");
                    }
                   wr.WriteLine("ave_zd2:" + "\n");
                     for (int i = 0; i < m_ave_zd; i++)
                    {
                        wr.WriteLine(ave_zd2[i].ToString() + "\n");
                    }
                    wr.WriteLine(max_ave_zd1.ToString() + "   " + Convert.ToString(m_b) + "  " + std_revel.ToString());
                    wr.WriteLine(min_ave_zd1.ToString());
                    wr.WriteLine(SIM.ToString());
                    wr.WriteLine(zd_temp_x_1.ToString());
                    wr.WriteLine(std_revel.ToString());
                    wr.Close();*/
                   
         /*******************************************************************************************/

                }
                #endregion

                #region
                else
                {

               //     std_revel = std_revel * 1000;
               //     int num_cycle, num_cycle1;
               //     double first_num;
               //     double P_or_N = 1;//x2与x1正负要相同；
               //     double[] sim = new double[NC_Data.Length / 6];
               //     double[] x2 = new double[NC_Data.Length / 6];
               //     double[] y2 = new double[NC_Data.Length / 6];
               //     int[] y2_temp = new int[NC_Data.Length / 6];
               //     double[] rate = new double[NC_Data.Length / 6];
               //     double[] rate1 = new double[NC_Data.Length / 6];
               //     double SIM = 1, zd_temp_x_1 = 1;
               //     // num_cycle = Convert.ToInt16(-min_ave_zd1 / 0.0125);

               //     //  this.textBox17.Text = a[0].ToString() + a[1].ToString() + a[2].ToString() + a[3].ToString() + a[4].ToString() + a[5].ToString() + a[6].ToString() + a[7].ToString() + a[8].ToString() + a[9].ToString() + a[10].ToString() + a[11].ToString() + a[12].ToString() + a[13].ToString() + a[14].ToString() + a[15].ToString();

               //     if (x1[0] * NC_Data[0, 4] > 0)
               //         P_or_N = -1;
               //     else
               //         P_or_N = 1;
               //     // this.textBox17.Text = P_or_N.ToString();
               //     for (int i = 0; i < NC_Data.Length / 6; i++)
               //     {
               //         //x2[i] = P_or_N*NC_Data[NC_Data.Length / 10 + i, 4];
               //         x2[i] = NC_Data[NC_Data.Length / 6 - i-1, 4];
               //         // x2[i] = 0.1 * i;
               //         //    y2[i] = a[0] + a[1] * x2[i] + a[2] * Math.Pow(x2[i], 2) + a[3] * Math.Pow(x2[i], 3) + a[4] * Math.Pow(x2[i], 4) + a[5] * Math.Pow(x2[i], 5) + a[6] * Math.Pow(x2[i], 6) + a[7] * Math.Pow(x2[i], 7) + a[8] * Math.Pow(x2[i], 8) + a[9] * Math.Pow(x2[i], 9) + a[10] * Math.Pow(x2[i], 10) + a[11] * Math.Pow(x2[i], 11) + a[12] * Math.Pow(x2[i], 7) + a[12] * Math.Pow(x2[i], 7) + a[13] * Math.Pow(x2[i], 13) + a[14] * Math.Pow(x2[i], 14) + a[15] * Math.Pow(x2[i], 15) + a[16] * Math.Pow(x2[i], 16) + a[17] * Math.Pow(x2[i], 17) + a[18] * Math.Pow(x2[i], 18) + a[19] * Math.Pow(x2[i], 19) + a[20] * Math.Pow(x2[i], 20);
               //         int index = 0;
               //         double temp = Math.Abs(x2[i] + P_or_N * x1[0]);
               //         //listBox1.Items.Add("y"+i.ToString());
               //         for (int ii = 0; ii < x1.Length; ii++)//找最接近点
               //         {
               //             double tep = Math.Abs(x2[i] + P_or_N * x1[ii]);
               //             //  listBox1.Items.Add(tep.ToString());

               //             if (temp > tep)
               //             {
               //                 index = ii;
               //                 temp = tep;
               //                 // MessageBox.Show("1");
               //             }
               //             if (Math.Abs(1 + x1[ii]) < 0.1)
               //             {
               //                 zd_temp_x_1 = Math.Abs(ave_zd2[ii] * 1000);


               //             }
               //         }

               //         // SIM = zd_temp_x_1 / 0.05;
               //         num_cycle = Convert.ToInt16(zd_temp_x_1 / removal);
               //         num_cycle1 = num_cycle;
               //         compens_cycle_time = Convert.ToDouble(num_cycle);
               //         num_nh.Text = Convert.ToString(compens_cycle_time);
               //         textBox17.Text = Convert.ToString(compens_cycle_time);
               //         first_num = Convert.ToDouble(num_cycle1) * removal;
               //         SIM = -num_cycle * removal;

               //         if (index >= ave_zd2.Length - 20)
               //         {
               //             y2[i] = ave_zd2[index];
               //             y2_temp[i] = index;
               //         }
               //         else
               //         {
               //             y2[i] = (ave_zd2[index] + ave_zd2[index + 3] + ave_zd2[index + 5] + ave_zd2[index + 7] + ave_zd2[index + 9] + ave_zd2[index + 11] + ave_zd2[index + 13] + ave_zd2[index + 15] + ave_zd2[index + 17] + ave_zd2[index + 19]) / 10;
               //             y2_temp[i] = index;
               //         }
               //         rate[i] = y2[i] * 1000 / SIM;
               //     }
               //     rate1[0] = 1;
               //     for (int i = 0; i < NC_Data.Length / 6-1; i++)
               //     {
               //         rate1[i + 1] = rate[NC_Data.Length / 6 -2 -i];

               //     }
               ///*     for (int i = NC_Data.Length / 10; i < NC_Data.Length / 5 - 1; i++)
               //     {
               //         rate1[i + 1] = rate[i - NC_Data.Length / 10];
               //     }*/
               //     //  rate1[NC_Data.Length / 5 - 1] = rate[ NC_Data.Length / 10-1];
               //     F1 = new double[NC_Data.Length];
               //     for (int i = 0; i < NC_Data.Length / 6; i++)
               //     {
               //         F1[i] = NC_Data[i, 2] / (rate1[i] - 0.05);

               //     }

/****************************************抛环带时************************************************************/
                    std_revel = std_revel * 1000;
                    int num_cycle, num_cycle1;
                    double first_num;
                    double P_or_N = 1;//x2与x1正负要相同；
                    double[] sim = new double[((NC_Data.Length/7+Convert.ToInt16(D_end * 10) / 2)*2+1) / 2];
                    double[] x2 = new double[((NC_Data.Length/7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2];
                    double[] y2 = new double[((NC_Data.Length/7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2];
                    int[] y2_temp = new int[((NC_Data.Length/7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2];
                    double[] rate = new double[((NC_Data.Length/7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2];
                    double[] rate1 = new double[NC_Data.Length / 7];
                    double SIM = 1, zd_temp_x_1 = 1;
                       double pressure_cycle_effect = 1, C_speed_cycle_effect = 1, P_speed_cycle_effect = 1,Cloth_diaman_effect=1;//压力，抛光轴速度和C轴速度,黄布与颗粒对抛光次数影响。
                    double multiple_rate = 1;//速度倍率
                    // num_cycle = Convert.ToInt16(-min_ave_zd1 / 0.0125);
                    if (textBox4.Text != "")
                        multiple_rate = Convert.ToDouble(textBox4.Text.Trim())*0.01;
                    if (textBox37.Text != "")//抛压力影响抛光次数
                        pressure_cycle_effect = Math.Pow(Math.Abs(150/Convert.ToDouble(textBox37.Text.Trim())), 0.65);
                    if (textBox2.Text != "")//C轴转速影响抛光次数
                        C_speed_cycle_effect = Math.Pow(Math.Abs(150 / Convert.ToDouble(textBox2.Text.Trim())), 0.5);
                    if (textBox33.Text != "")//抛光轴转速影响抛光次数
                        P_speed_cycle_effect = Math.Pow(Math.Abs(180 / Convert.ToDouble(textBox33.Text.Trim())), 0.5);
                    if (this.comboBox3.SelectedIndex == 1 && this.comboBox12.SelectedIndex == 1)//黄布与白膏的规格选择
                        Cloth_diaman_effect = 0.555;

                    // num_cycle = Convert.ToInt16(-min_ave_zd1 / 0.0125);

                    //  this.textBox17.Text = a[0].ToString() + a[1].ToString() + a[2].ToString() + a[3].ToString() + a[4].ToString() + a[5].ToString() + a[6].ToString() + a[7].ToString() + a[8].ToString() + a[9].ToString() + a[10].ToString() + a[11].ToString() + a[12].ToString() + a[13].ToString() + a[14].ToString() + a[15].ToString();

                    if (x1[2] * NC_Data[2, 4] > 0)
                        P_or_N = 1;
                    else
                        P_or_N = -1;
                    num_cycle = Convert.ToInt16(zd_temp_x_1 / removal);
                    // this.textBox17.Text = P_or_N.ToString();
                    for (int i = 0; i < ((NC_Data.Length / 7+ Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2; i++)//计算倍率
                    {
                        //x2[i] = P_or_N*NC_Data[NC_Data.Length / 10 + i, 4];
                        x2[i] = i * 0.1;
                        // x2[i] = 0.1 * i;
                        //    y2[i] = a[0] + a[1] * x2[i] + a[2] * Math.Pow(x2[i], 2) + a[3] * Math.Pow(x2[i], 3) + a[4] * Math.Pow(x2[i], 4) + a[5] * Math.Pow(x2[i], 5) + a[6] * Math.Pow(x2[i], 6) + a[7] * Math.Pow(x2[i], 7) + a[8] * Math.Pow(x2[i], 8) + a[9] * Math.Pow(x2[i], 9) + a[10] * Math.Pow(x2[i], 10) + a[11] * Math.Pow(x2[i], 11) + a[12] * Math.Pow(x2[i], 7) + a[12] * Math.Pow(x2[i], 7) + a[13] * Math.Pow(x2[i], 13) + a[14] * Math.Pow(x2[i], 14) + a[15] * Math.Pow(x2[i], 15) + a[16] * Math.Pow(x2[i], 16) + a[17] * Math.Pow(x2[i], 17) + a[18] * Math.Pow(x2[i], 18) + a[19] * Math.Pow(x2[i], 19) + a[20] * Math.Pow(x2[i], 20);
                        int index = 0;//s
                        double temp = Math.Abs(x2[i] + P_or_N * x_d[0]);
                        //listBox1.Items.Add("y"+i.ToString());
                        for (int ii = 0; ii < x_d.Length; ii++)
                        {
                            double tep = Math.Abs(x2[i] + P_or_N * x_d[ii]);
                            //  listBox1.Items.Add(tep.ToString());

                            if (temp > tep)
                            {
                                index = ii;
                                temp = tep;
                                // MessageBox.Show("1");
                            }
                            // if (Math.Abs(1 + x_d[ii]) < 0.1)//1附件的点为补偿基准点
                            ///{
                            //     zd_temp_x_1 = Math.Abs(ave_zd2[ii] * 1000);
                            //}
                            zd_temp_x_1 = Math.Abs(ave_zd2[96] * 1000);//1附件的点为补偿基准点
                        }
                        index = i * 10 * 100 / 105;
                        // SIM = zd_temp_x_1 / 0.05;
                        num_cycle = Convert.ToInt16(zd_temp_x_1 / removal);
                        num_cycle1 = num_cycle;
                        compens_cycle_time = Convert.ToDouble(num_cycle);
                       // num_nh.Text = Convert.ToString(compens_cycle_time);
                        //textBox17.Text = Convert.ToString(compens_cycle_time);
                      
                        first_num = Convert.ToDouble(num_cycle1) * removal;
                        SIM = -num_cycle * removal;

                        y2[i] = ave_zd2[index];
                        y2_temp[i] = index;

                        rate[i] = y2[i] * 1000 / SIM;
                    }

                    //if (this.comboBox3.SelectedIndex == 0 || this.comboBox12.SelectedIndex == 0) //研磨布和抛光膏选择
                    //{
                    //    if (num_cycle <= 1)
                    //    {
                    //        compens_cycle_time = Convert.ToInt16(num_cycle);
                    //        num_nh.Text = Convert.ToString(compens_cycle_time);
                    //        textBox17.Text = Convert.ToString(compens_cycle_time);
                    //    }
                    //    else
                    //    {
                    //        double cloth_flag = 1, diamon_compd = 1;
                    //        if (this.comboBox3.SelectedIndex == 0)//蓝膏
                    //            diamon_compd = 0.714;
                    //        if (this.comboBox12.SelectedIndex == 0)//白布
                    //            cloth_flag = 0.5;

                    //        compens_cycle_time = Convert.ToInt16(Math.Floor(num_cycle * diamon_compd * cloth_flag));
                    //        if (compens_cycle_time < 1)
                    //            compens_cycle_time = 1;
                    //        num_nh.Text = Convert.ToString(compens_cycle_time);
                    //        textBox17.Text = Convert.ToString(compens_cycle_time);

                    //    }
                    //}
                    if (compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate > 1)
                    {
                        num_nh.Text = Convert.ToString(Convert.ToInt16(compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate));//补正界面补正次数
                        textBox17.Text = Convert.ToString(Convert.ToInt16(compens_cycle_time * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * Cloth_diaman_effect * multiple_rate));//代码界面补正次数

                    }
                    else
                    {
                        num_nh.Text = "1";
                        textBox17.Text = "1";

                    }
                     if (this.comboBox3.SelectedIndex == 0 || this.comboBox12.SelectedIndex==0)//研磨膏规格与布的规格选择
                    {
                        if (num_cycle <= 1)
                        {
                            compens_cycle_time = Convert.ToInt16(num_cycle);
                            num_nh.Text = Convert.ToString(compens_cycle_time);
                            textBox17.Text = Convert.ToString(compens_cycle_time);
                        }
                        else
                        {
                            double cloth_flag = 1, diamon_compd = 1;
                            if (this.comboBox3.SelectedIndex == 0)//蓝膏
                                diamon_compd = 0.714;
                            if (this.comboBox12.SelectedIndex == 0)//白布
                                cloth_flag = 0.5;

                            compens_cycle_time = Convert.ToInt16(Math.Floor(num_cycle * diamon_compd * cloth_flag * pressure_cycle_effect * P_speed_cycle_effect * C_speed_cycle_effect * multiple_rate));
                            if (compens_cycle_time < 1)
                                compens_cycle_time = 1;
                            num_nh.Text = Convert.ToString(compens_cycle_time);
                            textBox17.Text = Convert.ToString(compens_cycle_time);

                        }
                    }
                  
                    /*****************************************读取excel数据*********************************************************************/
                  
                    string strConn;
                    // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + System.Environment.CurrentDirectory + "\\data.xlsx" + ";Extended Properties=Excel 8.0;";
                    string spot_data;
                    if (comboBox5.SelectedIndex == 0)
                        spot_data = "data1.xlsx";
                    else if (comboBox5.SelectedIndex == 1)
                        spot_data = "data2.xlsx";
                    else
                        spot_data = "data3.xlsx";
                    string datapath = "C:\\Program Files\\Microsoft Office\\Office12";
                    strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + datapath + "\\" + spot_data + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
                    if (Path.GetExtension(System.Environment.CurrentDirectory + "\\" + spot_data).Trim().ToUpper() == ".XLSX")
                    {
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + datapath + "\\" + spot_data + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                    }
                     OleDbConnection conn = new OleDbConnection(strConn);
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                   
                        myCommand.Fill(myDataSet);



                        double[,] data = new double[x_d.Length + 5, Convert.ToInt16(x_d[x_d.Length - 1] * 10) + 1];//存放原始表格数据
                        double[,] data2 = new double[x_d.Length + 5, Convert.ToInt16(x_d[x_d.Length - 1] * 10) + 1];//存放迭代数据
                        double[] data3 = new double[x_d.Length];//存放理论去除量
                 
                        try
                        {
                        for (int i = 0; i < x_d.Length; i++)//读取数据
                        {
                            for (int j = 0; j < x_d[x_d.Length - 1] * 10; j++)
                            {
                                data[i, j] = Convert.ToDouble(myDataSet.Tables[0].Rows[i][j]);
                            }

                        }

                        for (int i = 0; i < x_d.Length; i++)//乘上倍率
                        {
                            for (int j = 0; j < rate.Length; j++)
                            {
                                data2[i, j] = data[i, j] * rate[j];
                                data3[i] = data3[i] + data2[i, j];
                            }
                            data3[i] = data3[i] / -0.37315 * SIM;
                        }

                        //收敛运算

                        double er1 = 0.03;
                        double er2 = 0.02;
                        double er3 = 0.01;
                        double sc1 = 0.05;
                        double sc2 = 0.03;
                        double sc3 = 0.01;
                        double sc4 = 0;
                        double sc = 0;
                        double max_er = 2 * er3;
                        int k = 0;

                        while (max_er > er3 && k < 20)
                        {
                            for (int j = 0; j < Convert.ToInt16(x_d[x_d.Length - 1] * 10); j++)//倍率
                            {
                                if (Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) >= er1)//根据补正误差判断增减倍率量
                                    sc = sc1;
                                else if (Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) >= er2 && Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) < er1)




                                    sc = sc2;
                                else if (Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) >= er3 && Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) < er2)
                                    sc = sc3;
                                else if (Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) <= er3)
                                    sc = sc4;
                                if (Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000) >= max_er)
                                    max_er = Math.Abs(data3[j * 10 * 100 / 105] - ave_zd2[j * 10 * 100 / 105] * 1000);

                                if (data3[j * 10 * 100 / 105] <= ave_zd2[j * 10 * 100 / 105] * 1000)//补正误差为正时
                                {
                                    if (rate[j] > 0.3)
                                    {
                                        if (j == 0 || j == 1)
                                            rate[j] = rate[j] - sc;
                                        else if (j == 2)
                                        {
                                            rate[j] = rate[j] - sc / 2;
                                            rate[j - 1] = rate[j - 1] - sc / 2;
                                        }
                                        else
                                            rate[j-1] = rate[j-1] - sc;
                                    }

                                }
                                else if (data3[j * 10 * 100 / 105] > ave_zd2[j * 10 * 100 / 105] * 1000)   //补正误差为负时                          
                                {
                                    if (rate[j] < 5)
                                    {
                                        if (j == 0 || j == 1)
                                            rate[j] = rate[j] + sc;
                                        else if (j == 2)
                                        {
                                            rate[j] = rate[j] + sc / 2;
                                            rate[j - 1] = rate[j - 1] + sc / 2;
                                        }
                                        else
                                            rate[j-1] = rate[j-1] + sc;
                                    }
                                }


                            }

                            for (int i = 0; i < x_d.Length; i++)//乘上倍率
                            {
                                data3[i] = 0;
                                for (int j = 0; j < rate.Length; j++)
                                {
                                    data2[i, j] = data[i, j] * rate[j];
                                    data3[i] = data3[i] + data2[i, j];
                                }
                                data3[i] = data3[i] / -0.37315 * SIM;
                            }
                            k++;
                        }
                        // double c = Convert.ToDouble(myDataSet.Tables[0].Rows[0][0]);
                        // MessageBox.Show(c.ToString());
                    }
                    catch (Exception ex)
                    {
                        //   // throw new InvalidFormatException("该Excel文件的工作表的名字不正确," + ex.Message);
                        MessageBox.Show(ex.Message);
                    }



                    /**************************************************************************************************************/


                        rate1[0] = 1;
                    for (int i = 0; i < NC_Data.Length / 7-1; i++)
                    {
                        rate1[i+1] = rate[rate.Length - 1 - i];

                    }
                    //for (int i = NC_Data.Length / 12; i < NC_Data.Length / 6 - 1; i++)
                    //{
                    //    rate1[i + 1] = rate[i - NC_Data.Length / 12];
                    //}
                    //  rate1[NC_Data.Length / 5 - 1] = rate[ NC_Data.Length / 10-1];
                    F1 = new double[NC_Data.Length];
                    rate_dist = new double[NC_Data.Length];
                 
                    for (int i = 0; i < NC_Data.Length / 7; i++)
                    {
                        F1[i] = NC_Data[i, 2] / (rate1[i]);
                        rate_dist[i] = rate1[i];//
                    }


                    /**********************************************绘图*****************************************************/



                    /********************************画在zedgraph上********************************************************/
                    // zedGraphControl1.Dispose();
                    GraphPane myPane;


                    // myPane = new GraphPane(new Rectangle(40, 40, 600, 400),"My Test Graph\n(For CodeProject Sample)", "My X Axis", "My Y Axis");
                    myPane = zedGraphControl1.GraphPane;
                    myPane.XAxis.Title.Text = "半径(mm)";
                    myPane.YAxis.Title.Text = "误差(um)";
                    myPane.Title.Text = " ";

                    myPane.CurveList.Clear();
                    myPane.GraphObjList.Clear();

                    // 设置初始数据
                    double xx, y11, y22;
                    PointPairList list1 = new PointPairList();
                    PointPairList list2 = new PointPairList();
                    PointPairList list3 = new PointPairList();
                    PointPairList list4 = new PointPairList();
                    PointPairList list5 = new PointPairList();
                    PointPairList list6 = new PointPairList();

                    for (int i = m_ave_zd - 1; i > 0; i--)
                    {
                        //xx = x1[i];
                        xx = x_d[i];
                        y11 = ave_zd2[i];
                        //  y22 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                        list1.Add(xx, y11 * 1000);


                    }
                    for (int i = 0; i < m_ave_zd; i++)
                    {
                        //xx = x1[i];
                        xx = x_d[i];
                        y11 = ave_zd2[i];

                        list1.Add(-xx, y11 * 1000);


                    }

                    for (int i = ((NC_Data.Length / 7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2; i > ((NC_Data.Length / 7 + Convert.ToInt16(D_end * 10) / 2) * 2 + 1) / 2 - NC_Data.Length / 7 - 1; i--)
                    {
                        //xx = x2[i];
                        // y11 = y2[i];
                        xx = i * 0.1;
                        y11 = data3[i * 10 * 100 / 105] / 1000;
                        y22 = ave_zd2[i * 10 * 100 / 105] - data3[i * 10 * 100 / 105] / 1000;
                        list2.Add(-xx, y11 * 1000);
                        list3.Add(-xx, y22 * 1000);

                    }
                  


                    LineItem myCurve = myPane.AddCurve("实际应去除量", list1, Color.Red, SymbolType.None);

                    LineItem myCurve2 = myPane.AddCurve("拟合应去除量", list2, Color.Black, SymbolType.None);

                    LineItem myCurve3 = myPane.AddCurve("补正误差", list3, Color.Blue, SymbolType.None);
                    // CurveItem myCurve3 = myPane.AddCurve("补正误差", list3, Color.Blue, SymbolType.None);

                    myPane.AxisChange();
                    zedGraphControl1.Refresh();



                }
                #endregion
                button16.Text = "补正运算";
                compensate_flag = true;
            /**************************************test****************************************************/
            #region//再生成非补正代码
                try
                {
                    double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };


                    n = Convert.ToDouble(textBox39.Text);//加工次数
                    R = Convert.ToDouble(R_text.Text);  //面型参数R
                    K = Convert.ToDouble(K_text.Text);//面型参数K
                    vc = Convert.ToDouble(textBox3.Text);//C轴最大转速
                    H = Convert.ToDouble(TextBox35.Text);//模仁高度
                    Dp = Convert.ToDouble(gongD_textBox3.Text.Trim());//加工口径
                    D_workpiece = Math.Round(Convert.ToDouble(textBox46.Text.Trim()),1);//模仁柱面直径

                    if (D_workpiece * 10 % 2 == 1)
                        D_workpiece = D_workpiece - 0.1;
                    if (radioButton23.Checked == true)//单段程序加工
                    {


                        // D = Convert.ToDouble(textBox6.Text);//加工口径
                        //  D_end = 0;// Convert.ToDouble(textBox18.Text);//加工口径另一值
                        if (textBox6.Text == "")
                            R_left = 0;
                        else
                            R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

                        if (textBox18.Text == "")
                            R_right = 0;
                        else
                            R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值


                        // textBox18.text
                        // if (Math.Abs(R_left) < arry[15])
                        //     R_left = arry[15];

                        if (Math.Abs(R_left) > D_workpiece / 2)
                        {
                            MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                            button16.Text = "补正运算";
                            return;
                        }


                        if (Math.Abs(R_right) > D_workpiece / 2)
                        { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); button16.Text = "补正运算"; return; }
                        if (R_left >= R_right)
                        { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); button16.Text = "补正运算"; return; }

                        R_left = -D_workpiece / 2;//加工范围半径


                        R_right = D_workpiece / 2;//加工口径另一值

                    }
                    else
                    {
                        double[] arry = new double[16];

                        if (textBox40.Text != "")
                            arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                        else
                            arry[0] = 0;

                        if (textBox51.Text != "")
                            arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                        else
                            arry[1] = 0;
                        if (textBox55.Text != "")
                            arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                        else
                            arry[2] = 0;
                        if (textBox54.Text != "")
                            arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                        else
                            arry[3] = 0;
                        if (textBox58.Text != "")
                            arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                        else
                            arry[4] = 0;
                        if (textBox57.Text != "")
                            arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                        else
                            arry[5] = 0;
                        if (textBox61.Text != "")
                            arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                        else
                            arry[6] = 0;
                        if (textBox60.Text != "")
                            arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                        else
                            arry[7] = 0;
                        if (textBox64.Text != "")
                            arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                        else
                            arry[8] = 0;
                        if (textBox63.Text != "")
                            arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                        else
                            arry[9] = 0;
                        if (textBox67.Text != "")
                            arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                        else
                            arry[10] = 0;
                        if (textBox66.Text != "")
                            arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                        else
                            arry[11] = 0;
                        if (textBox73.Text != "")
                            arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                        else
                            arry[12] = 0;
                        if (textBox72.Text != "")
                            arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                        else
                            arry[13] = 0;
                        if (textBox70.Text != "")
                            arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                        else
                            arry[14] = 0;
                        if (textBox69.Text != "")
                            arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                        else
                            arry[15] = 0;


                        Array.Sort(arry);

                        R_left = -D_workpiece / 2;//加工范围半径


                        R_right = D_workpiece / 2;//加工口径另一值
                        // textBox18.text
                        //if (Math.Abs(R_left) < arry[15])
                        //    R_left = arry[15];

                        if (Math.Abs(arry[15]) > D_workpiece / 2)
                        {
                            MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                            button16.Text = "补正运算";
                            return;
                        }
                        //R_left = -D_workpiece / 2;//加工范围半径


                        //R_right = D_workpiece / 2;//加工口径另一值
                        //if (Math.Abs(R_left) > D_workpiece / 2)
                        //{
                        //    MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                        //    return;
                        //}

                        //if (Math.Abs(R_right) > D_workpiece / 2)
                        //{ MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }
                        //if (R_left >= R_right)
                        //{ MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }
                        ////if (Math.Abs(Convert.ToDouble(gongD_textBox3.Text)) > arry[15] * 2)
                        //    D = Math.Abs(Convert.ToDouble(gongD_textBox3.Text));//加工口径（形状参数）
                        //else
                        //    D = arry[15] * 2;

                        D_end = 0;//加工口径另一值
                    }


                    SAG = Convert.ToDouble(SAG_text.Text);//非球面中心到平面距离
                    yuan_r = Convert.ToDouble(yuanr_text.Text);//夹具高度，从移动调心夹具底面到模仁底面距离
                    if (yuan_r < 0.1)
                        yuan_r = 0.1;
                    constan_vc = Convert.ToDouble(textBox2.Text);//恒定C转速
                    constan_F = Convert.ToDouble(textBox5.Text);//恒定进给
                    if (ao_non_sphere.Checked == true)//凹凸判断
                        ao_tu = -1;
                    else
                        ao_tu = 1;

                    /* 
                     if (comboBox4.SelectedIndex == 0)
                         dist = 0.1;
                     else
                         dist = 0.05;
                     if (ao_non_sphere.Checked == true)
                     {
                         if (R > 0)
                             symbol = 1;
                         else
                             symbol = -1;

                     }
                     if (tu_non_sphere.Checked == true)
                     {
                         if (R > 0)
                             symbol = -1;
                         else
                             symbol = 1;

                     }

                     NC_Data = NC.asphere(dist,symbol, n, vc, H, D, R, K, A, out t);//生成代码
         */
                    if (comboBox5.SelectedIndex == 0)
                    {
                        tool_r = 1;
                        removal = 0.006667;
                    }
                    else if (comboBox5.SelectedIndex == 1)
                    {
                        tool_r = 3;
                        removal = 0.0125;
                    }
                    else
                    {
                        tool_r = 5;
                        removal = 0.0185;
                    }
                    if (radioButton7.Checked == true)//垂直抛
                    {
                        tool_r = 7;
                    }

                    if (comboBox4.SelectedIndex == 0)
                        dist = 0.1;
                    else if (comboBox4.SelectedIndex == 1)
                        dist = 0.01;
                    else
                        dist = 0.1;

                    if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
                    {
                        if (ao_non_sphere.Checked == true)
                        {
                            if (R > 0)
                                symbol = 1;
                            else
                                symbol = -1;

                        }
                        if (tu_non_sphere.Checked == true)
                        {
                            if (R > 0)
                                symbol = -1;
                            else
                                symbol = 1;
                        }
                        if (comboBox13.SelectedIndex == 1)
                            curvature_compensate = 1;
                        else if (comboBox13.SelectedIndex == 2)
                            curvature_compensate = 2;
                        else
                            curvature_compensate = 0;

                        //textBox47.Text = Convert.ToString(symbol);//看看symbol对不对
                        // NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
                        //if (radioButton14.Checked == true)
                        //    NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

                        //      //else if (radioButton16.Checked == true)
                        //      //NC_Data = NC.asphere_heitian2(SAG,fixture_h,ao_tu,D_end,tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

                        //else

                 if (comboBox4.SelectedIndex == 0)
                     NC_Data = NC.asphere_heitian(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码               
                 else
                     NC_Data = NC.asphere_heitian_dist(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码

                        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
                    }
                    if (ao_sphere.Checked == true || tu_sphere.Checked == true)
                    {
                        if (ao_sphere.Checked == true)
                            symbol = 1;
                        if (tu_sphere.Checked == true)
                            symbol = -1;
                        NC_Data = NC.sphere(first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码

                        // textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间


                    }
                    if (plane.Checked == true)
                    {
                        //  NC_Data = NC.plane(C_motor_scale_factor, C_motor_offset, fixture_h, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
                        NC_Data = NC.plane_heitian(first_position_feed, D_workpiece, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码

                        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间

                    }

                    if (comboBox4.SelectedIndex == 1)
                    {
                        F1 = new double[NC_Data.Length];

                        F1[0] = NC_Data[0, 2];
                        for (int i = 1; i < NC_Data.Length / 7; i++)//(rate_dist[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10) + 1)]); //
                        {
                            F1[i] = NC_Data[i, 2] / (rate_dist[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10) + 1)]);

                            if (i % 10 == 0)
                                F1[i] =  NC_Data[i, 2] / (rate_dist[Convert.ToInt16(Math.Truncate(Convert.ToDouble(i) / 10))]);
                        }

 
                    }
                  

                }
                catch (Exception)
                {
                    MessageBox.Show("参数设定发生错误！请检查加工参数和形状参数是否设定。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button16.Text = "补正运算";
                    return;
                }
            #endregion



        }

        private void button17_Click(object sender, EventArgs e)//生成补正代码
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = true;
            this.panel9.Visible = false;
            Pcode_return_flag = false;
            /***************写入TXT********/

          /*  SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "磨仁补正加工代码保存";
            sfd.InitialDirectory = System.Environment.CurrentDirectory+"\\模仁补正加工代码";
            sfd.Filter = "文本文件| *.txt";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }

            //string result1 = @".\Gcode_buzheng.txt";//结果保存到F:\result1.txt

            /*先清空result1.txt文件内容*/
          /*  FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
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
            wr.WriteLine("WHILE(P25<" + textBox39.Text + ")");
            for (int i = 0; i < NC_Data.Length / 5; i++)
            {
                wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + Math.Abs(F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
            }
            // for (int i = a-1; i >= 0; i--)
            //{
            //    wr.WriteLine(Convert.ToString("X" + X2[i].ToString("f4") + "  " + "B" + B[i].ToString("f4") + "  " + "F" + F[i].ToString("f4") + "  " + "M302==" + Nc[i].ToString("f4") + "  " + "\n"));
            // 
            // } 
            wr.WriteLine("P25=P25-1");
            wr.WriteLine("ENDWHILE");
            if (Convert.ToDouble(textBox39.Text) % 2 == 1)
            {
                for (int i = 0; i < NC_Data.Length / 5; i++)
                {
                    wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + Math.Abs(F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                }

            }
            wr.WriteLine("P14=1");
            wr.WriteLine("CLOSE");
            wr.Close();*/

        }

        private void button23_Click(object sender, EventArgs e)//进入
        {

            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;

            //if (CheckLicence())
            //{
            //    this.panel1.Visible = false;
            //    this.panel2.Visible = true;
            //    this.panel3.Visible = false;
            //    this.panel4.Visible = false;
            //    this.panel5.Visible = false;
            //    this.panel6.Visible = false;
            //    this.panel7.Visible = false;
            //    this.panel8.Visible = false;
            //    this.panel9.Visible = false;
            //}
            //else
            //{
            //    MessageBox.Show("软件密钥失效，请联系厂商!");
            //    return;

            //}

           // CheckLicence();//检测秘钥

            //String account = this.textBox36.Text.Trim();
            //String password = this.textBox38.Text.Trim();




            //this.tabControl1.SelectedIndex = 1;


            //if (account == "JMPG-100" && password == "12345")
            //{
            //    this.panel1.Visible = false;
            //    this.panel2.Visible = true;
            //    this.panel3.Visible = false;
            //    this.panel4.Visible = false;
            //    this.panel5.Visible = false;
            //    this.panel6.Visible = false;
            //    this.panel7.Visible = false;
            //    this.panel8.Visible = false;
            //    this.panel9.Visible = false;
            //}
            //else
            //{
            //    if (CheckLicence())
            //    {
            //        this.panel1.Visible = false;
            //        this.panel2.Visible = true;
            //        this.panel3.Visible = false;
            //        this.panel4.Visible = false;
            //        this.panel5.Visible = false;
            //        this.panel6.Visible = false;
            //        this.panel7.Visible = false;
            //        this.panel8.Visible = false;
            //        this.panel9.Visible = false;

            //    }
            //    else
            //    {
            //        MessageBox.Show("软件使用时间过期，请联系厂商!");
            //        return;
            //    }
            //}

            //String path = System.Environment.CurrentDirectory + "\\account.txt";
            //FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            //stream2.Seek(0, SeekOrigin.Begin);
            //stream2.SetLength(0); //清空txt文件
            //stream2.Close();
            //FileStream fs = new FileStream(path, FileMode.Append);
            //StreamWriter wr = null;
            //wr = new StreamWriter(fs);
            //wr.WriteLine(account);
            //wr.WriteLine(password);
            //wr.Close();
            //string status;
            //PMAC.GetResponse(pmacNumber, "j/", out status);

            //dectect = new System.Threading.Thread(new System.Threading.ThreadStart(this.dective));
            //dectect.Start();

           




        }


        public bool CheckLicence()
        {

           try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                EncrypHelper Myencry = new EncrypHelper();



                String TimeFile = System.Environment.CurrentDirectory + "\\license.txt";

                System.IO.StreamReader sr = new System.IO.StreamReader(TimeFile);
                int encryp_stauts = Convert.ToInt16(sr.ReadLine());
                string year = sr.ReadLine();
                string month = sr.ReadLine();
                string day = sr.ReadLine();
                string remain_day = sr.ReadLine();
                sr.Close();
/*
        
              格式：
 * 1
 * 2022
 * 3
 * 1
 * 60            
  */
                if (encryp_stauts == 1)
                {
                    year = Myencry.Decrypt(year, "Jingdian");
                    month = Myencry.Decrypt(month, "Jingdian");
                    day = Myencry.Decrypt(day, "Jingdian");
                    remain_day = Myencry.Decrypt(remain_day, "Jingdian");

                }



                //string time= ReadTxt(TimeFile);


                DateTime Now = DateTime.Now;
            
                //   time = Myencry.Decrypt(time, "Jingdian");
                //Thread.Sleep(2000);
                //DateTime time = DateTime.Now;
                // long Now_time = Now.ToFileTime();
                //string Now_time = Now.ToShortDateString();
                //int first = Now_time.IndexOf("-");
                //int second = Now_time.IndexOf("-", first + 1);

                //string nowtime_year = Now_time.Substring(0, first);
                //string nowtime_month = Now_time.Substring(first + 1, second - first - 1);
                //string nowtime_day = Now_time.Substring(second + 1);

                //int now_year = Convert.ToInt16(nowtime_year);
                //int now_month = Convert.ToInt16(nowtime_month);
                //int now_day = Convert.ToInt16(nowtime_day);

                int now_year = Now.Year;
                int now_month = Now.Month;
                int now_day = Now.Day;


                //   int date = Convert.ToInt16(Now_time);
                // DateTime dt = DateTime.ParseExact(time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                //  string encryt_time=Myencry.Encrypt(time, "Jingdian");//加密时间

                // string decrep_time = Myencry.Decrypt(encryt_time, "Jingdian");//解密时间

                int remain_day3 = Convert.ToInt16(remain_day);//开始读取时间

                int remain_day2 = (Convert.ToInt16(year) * 356 + 30 * Convert.ToInt16(month) + Convert.ToInt16(day)) - (now_year * 356 + 30 * now_month + now_day);


                if (remain_day2 > remain_day3)
                {
                    
                        remain_day = Convert.ToString(remain_day3);
                    
                        remain_day2 = remain_day3;
                    
                }
                else 
                {
                    if (remain_day2 > 0)
                        remain_day = Convert.ToString(remain_day2);
                    else
                    {
                        remain_day = "0";

                        remain_day2 = 0;
                    }

                }


                year = Myencry.Encrypt(year, "Jingdian");
                month = Myencry.Encrypt(month, "Jingdian");
                day = Myencry.Encrypt(day, "Jingdian");
                remain_day = Myencry.Encrypt(remain_day, "Jingdian");


                System.IO.StreamWriter wr = new System.IO.StreamWriter(TimeFile);
                wr.WriteLine("1");//加密存贮
                wr.WriteLine(year);
                wr.WriteLine(month);
                wr.WriteLine(day);
                wr.WriteLine(remain_day);
                wr.Close();


                //    WriteTxt(TimeFile, encryt_time);


                if (remain_day2 < 1)
                {
                    // MessageBox.Show("软件使用时间过期，请联系厂商");

                    return false;
                }
                else
                {
                    // MessageBox.Show("未过期");

                    return true;
                }
            }
           catch
           {
               return false;
           }
            
        }
        public static string ReadTxt(String filePath)
        {
            string strData = "";
            try
            {
                string line;
                //    // 创建一个 StreamReader 的实例来读取文件 ,using 语句也能关闭 StreamReader
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath))
                {
                    //        // 从文件读取并显示行，直到文件的末尾
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        strData = line;
                    }
                }
            }
            //}
            catch (Exception e)
            {
                //    // 向用户显示出错消息
                //    //Console.WriteLine("The file could not be read:");
                //    //Console.WriteLine(e.Message);
            }
            return strData;
        }



        public static void WriteTxt(String filePath,string writeString)
        {
            string strData = writeString;
            try
            {
               // string line;
                //    // 创建一个 StreamReader 的实例来读取文件 ,using 语句也能关闭 StreamReader
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(filePath))
                {
                    //        // 写数据
                    sr.WriteLine(strData);
                  
                }
            }
            //}
            catch (Exception e)
            {
                //    // 向用户显示出错消息
                //    //Console.WriteLine("The file could not be read:");
                //    //Console.WriteLine(e.Message);
            }
           // return strData;
        }


        private void button24_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }

        private void button25_Click(object sender, EventArgs e)//模具参数设定
        {
           // this.tabControl1.SelectedIndex = 2;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = true;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            string myfile = System.Environment.CurrentDirectory + "\\last_shape_parameter.txt";
            int ao_tu_symbol;
            if(set_flag==false)
            {
                tu_non_sphere.Checked = true;
                /*****初始化数据****/
                textBox1.Text = null;
                yuanr_text.Text = "0";
                gongD_textBox3.Text = "0";
                SAG_text.Text = "0";
                TextBox35.Text = "0";
                R_text.Text = "0";
                K_text.Text = "0";
                A1.Text = "0";
                A2.Text = "0";
                A3.Text = "0";
                A4.Text = "0";
                A5.Text = "0";
                A6.Text = "0";
                A7.Text = "0";
                A8.Text = "0";
                A9.Text = "0";
                A10.Text = "0";
                A11.Text = "0";
                A12.Text = "0";
                A13.Text = "0";
                A14.Text = "0";
                A15.Text = "0";
                A16.Text = "0";
                A17.Text = "0";
                A18.Text = "0";
                A19.Text = "0";
                A20.Text = "0";
                set_flag = true;


                StreamReader myreader = null;//new StreamReader();
                myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                yuanr_text.Text = myreader.ReadLine();
                gongD_textBox3.Text = myreader.ReadLine();
                SAG_text.Text = myreader.ReadLine();
                TextBox35.Text = myreader.ReadLine();
                R_text.Text = myreader.ReadLine();
                K_text.Text = myreader.ReadLine();
                A1.Text = myreader.ReadLine();
                A2.Text = myreader.ReadLine();
                A3.Text = myreader.ReadLine();
                A4.Text = myreader.ReadLine();
                A5.Text = myreader.ReadLine();
                A6.Text = myreader.ReadLine();
                A7.Text = myreader.ReadLine();
                A8.Text = myreader.ReadLine();
                A9.Text = myreader.ReadLine();
                A10.Text = myreader.ReadLine();
                A11.Text = myreader.ReadLine();
                A12.Text = myreader.ReadLine();
                A13.Text = myreader.ReadLine();
                A14.Text = myreader.ReadLine();
                A15.Text = myreader.ReadLine();
                A16.Text = myreader.ReadLine();
                A17.Text = myreader.ReadLine();
                A18.Text = myreader.ReadLine();
                A19.Text = myreader.ReadLine();
                A20.Text = myreader.ReadLine();
                ao_tu_symbol = Convert.ToInt16(myreader.ReadLine());
                if (ao_tu_symbol == 1)
                    ao_non_sphere.Checked = true;
                else if (ao_tu_symbol == 2)
                    tu_non_sphere.Checked = true;
                else if (ao_tu_symbol == 3)
                    ao_sphere.Checked = true;
                else if (ao_tu_symbol == 4)
                    tu_sphere.Checked = true;
                else if (ao_tu_symbol == 5)
                    plane.Checked = true;
                textBox1.Text = myreader.ReadLine();
                textBox46.Text = myreader.ReadLine();
                myreader.Close();
                this.draw();
            }
            
            this.timer1.Enabled = true;//开启模具参数设定界面的timer
        }

        private void button27_Click(object sender, EventArgs e)//加工参数设定
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = true;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            // this.timer1.Enabled = false;
            this.button10.Enabled = true;
            this.textBox34.Text = textBox1.Text;//显示模仁名称
            // textBox34.Enabled = false;
            // textBox35.Text = SAG_text.Text;//显示模仁平台高度
            // textBox33.Enabled = false;
           
            // textBox32.Enabled = false;
            timer4.Enabled = true;//开启加工参数界面计时器
            if (set_proces_flag == false)
            {
             //   radioButton8.Checked = true;//45度角
             //   radioButton1.Checked = true;//恒转速
             //   radioButton3.Checked = true;//速率可变
             //   radioButton23.Checked = true;//单端加工
             //  // textBox6.Text = gongD_textBox3.Text;//设置模仁加工口径
             ////   textBox6.Text = gongD_textBox3.Text;//设置平面加工口径
             //    //gongD_textBox3.Text
             //   textBox3.Text = "150";//最大转速
             //   textBox4.Text = "100";//
             //   //textBox18.Text = "0";//加工半径
             //   textBox39.Text = "1";//加工次数
             //   comboBox1.SelectedIndex = 0;//模仁材质
             //   comboBox3.SelectedIndex = 0;//研磨颗粒规格
             //   comboBox4.SelectedIndex = 0;//数据间隔
             //   comboBox5.SelectedIndex = 1;//磨头直径
             //   comboBox8.SelectedIndex = 0;//工件转动方向
             //   comboBox9.SelectedIndex = 1;//抛光轴旋转方向
             //   textBox33.Text = "180";//抛光轴转速
             //  // textBox32.Text = "3";//抛光头直径
             //   comboBox12.SelectedIndex = 0;//研磨布规格
             //   textBox37.Text = "150";//荷重
             //   textBox2.Text = "150";//恒转速
             //   textBox5.Text = "60";//恒进给
             //   //textBox18.Text = "0";//加工范围
             //   textBox35_1.Text = "15";//加工范围
              

                try
                {
                    string myfile = System.Environment.CurrentDirectory + "\\last_process_paramenters.txt";//new StreamReader();

                    StreamReader myreader = null;
                    myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
                    textBox2.Text = myreader.ReadLine();//恒转速
                    textBox3.Text = myreader.ReadLine();//变最大转速

                    textBox5.Text = myreader.ReadLine();//进给速度
                    textBox4.Text = myreader.ReadLine();//速度倍率
                    textBox33.Text = myreader.ReadLine();//抛光头转速
                    // textBox32.Text = myreader.ReadLine();//磨头直径
                    comboBox12.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//研磨布厚度
                    textBox35_1.Text = myreader.ReadLine();//平台高度
                    textBox6.Text = myreader.ReadLine();//加工范围左位置
                    textBox18.Text = myreader.ReadLine();//加工范围右位置
                    textBox37.Text = myreader.ReadLine();//荷重
                    //textBox40.Text = myreader.ReadLine();//数据间隔
                    textBox39.Text = myreader.ReadLine();//往复回数
                    // textBox18.Text = myreader.ReadLine();//加工内径   
                    comboBox4.SelectedIndex = Convert.ToInt16(myreader.ReadLine());///数据间隔
                    comboBox5.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//磨头直径
                    comboBox8.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//抛光轴方向
                    comboBox9.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//工件方向
                    comboBox1.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//模仁材质
                    comboBox3.SelectedIndex = Convert.ToInt16(myreader.ReadLine());//研磨颗粒规格
                    comboBox13.SelectedIndex = 0;//曲率补正

                    if (myreader.ReadLine() == "1")//工件转速
                        radioButton1.Checked = true;
                    else
                        radioButton2.Checked = true;
                    if (myreader.ReadLine() == "1")//进给速度
                        radioButton4.Checked = true;
                    else
                        radioButton3.Checked = true;
                    if (myreader.ReadLine() == "1")//研磨角度
                        radioButton7.Checked = true;
                    else
                        radioButton8.Checked = true;

                    if (myreader.ReadLine() == "1")//单段加工
                        radioButton23.Checked = true;
                    else
                        radioButton24.Checked = true;

                    myreader.Close();
                }
                catch (Exception err)
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show(err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                set_proces_flag = true;
            }

        }

        private void button26_Click(object sender, EventArgs e)//面型误差修正
        {
            //this.tabControl1.SelectedIndex = 4;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = true;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            this.textBox48.Text = textBox34.Text;//补正模仁名称

        }

        private void button28_Click(object sender, EventArgs e)//手动模式
        {
           // this.tabControl1.SelectedIndex = 5;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = true;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            if(handle_model_flag==false)
            {
                textBox10.Text = "4";
                textBox9.Text = "5";
                textBox21.Text = "4";
                textBox7.Text = "100";
                textBox8.Text = "100";
                textBox30.Text = "0";
                textBox31.Text = "0";
                textBox22.Text = "0";
                comboBox2.SelectedIndex = 0;//点动
                comboBox6.SelectedIndex = 0;//工作台方向
                comboBox7.SelectedIndex = 0;//抛光轴方向
                radioButton5.Checked = true;
                handle_model_flag = true;

            }
            
            timer2.Enabled = true;//开启手动模式界面计时器
        }

        private void button29_Click(object sender, EventArgs e)//加工模式
        {
            //this.tabControl1.SelectedIndex = 6;

            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = true;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            timer3.Enabled = true;


           process_flag = true;
            if (process_flag == false)
            {
            
               
                radioButton12.Checked = true;

               // comboBox8.SelectedIndex = 0;//工件转动方向
              //  comboBox9.SelectedIndex = 1;//抛光轴旋转方向
                string path = System.Environment.CurrentDirectory + "\\last_process_parameter.txt"; ;

                StreamReader w = null;//new StreamReader();
                w = new StreamReader(path, System.Text.Encoding.UTF8);
                textBox11.Text = w.ReadLine();//模仁名称
                textBox12.Text = w.ReadLine();//次数
                textBox14.Text = w.ReadLine();//抛光轴转速
                textBox13.Text = w.ReadLine();//工件高度
                textBox15.Text = w.ReadLine();//工件转速
                textBox20.Text = w.ReadLine();//夹具高度转速
                textBox19.Text = w.ReadLine();//速度倍率
               
                if (Convert.ToDouble(w.ReadLine()) == 0)
                    comboBox10.SelectedIndex = 0;//抛光顺逆
                else
                    comboBox10.SelectedIndex = 1;//抛光顺逆
                if (Convert.ToDouble(w.ReadLine()) == 0)
                    comboBox11.SelectedIndex = 0;//工件顺逆
                else
                    comboBox11.SelectedIndex = 1;//工件顺逆
                // w.WriteLine(comboBox10.SelectedIndex);//抛光顺逆
                textBox50.Text = w.ReadLine();//矢高
                w.Close();
                radioButton21.Checked = true;

                process_flag = true;
            }

            radioButton13.Checked = true;

            textBox12.Text = "0";//次数
            textBox14.Text = "0";//抛光轴转速
            textBox13.Text = "0";//工件高度
            textBox15.Text = "0";//工件转速
            textBox20.Text = "0";//夹具高度转速
            textBox19.Text = "100";//速度倍率


            comboBox10.SelectedIndex = 1;//抛光顺逆


            comboBox11.SelectedIndex = 0;//工件顺逆

            // w.WriteLine(comboBox10.SelectedIndex);//抛光顺逆
            textBox50.Text = "0";//矢高

            double time = process_time;//Convert.ToDouble(textBox47.Text);
           
            if ((own_code_flag == true || compensate_code_flag == true)&&process_begin_flag==false)
            {
                radioButton13.Checked = true;
               
                textBox12.Text = "0";//次数
                textBox14.Text = "0";//抛光轴转速
                textBox13.Text = "0";//工件高度
                textBox15.Text = "0";//工件转速
                textBox20.Text = "0";//夹具高度转速
                textBox19.Text = "100";//速度倍率

                
                    comboBox10.SelectedIndex = 1;//抛光顺逆
               
                
                    comboBox11.SelectedIndex = 0;//工件顺逆
               
                // w.WriteLine(comboBox10.SelectedIndex);//抛光顺逆
                    textBox50.Text = "0";//矢高

                textBox11.Text = textBox34.Text;
                hour = Convert.ToInt32((time / 60).ToString().Split(char.Parse("."))[0]); //time % 60;
                min = Convert.ToInt32((time % 60).ToString().Split(char.Parse("."))[0]);
                double sec = (time % 1) * 60;

                Sec = Convert.ToInt16(sec);
                //Sec = Convert.ToInt32((time.ToString().Split(char.Parse("."))[1])) * 6;
                textBox47.Text = Convert.ToString(hour.ToString() + "时" + min.ToString() + "分" + Sec.ToString() + "秒");
           
               // textBox47.Text = textBox38.Text;
            }
           timer5.Enabled = true;

        }

        private void button30_Click(object sender, EventArgs e)//加工模式
        {
           // this.tabControl1.SelectedIndex = 6;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = true;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
        }

        private void button22_Click(object sender, EventArgs e)//保存加工参数
        {
            //this.button10.Enabled = false;

            double[] Parameter = new double[17];
            try
            {
                
                Parameter = new double[17] {
                    Convert.ToDouble(textBox2.Text),//转速
                    Convert.ToDouble(textBox3.Text), //最大转速
                    Convert.ToDouble(textBox5.Text), //进给速度
                    Convert.ToDouble(textBox4.Text),//速度倍率
                    Convert.ToDouble(textBox33.Text), //抛光轴转速
                    
                    Convert.ToDouble(comboBox12.SelectedIndex), //研磨布规格
                    Convert.ToDouble(textBox35_1.Text), //工件高度
                    Convert.ToDouble(textBox6.Text), //加工范围左位置
                    Convert.ToDouble(textBox18.Text), //加工范围右位置
                    Convert.ToDouble(textBox37.Text), //荷重
                    
                    Convert.ToDouble(textBox39.Text),//往复回数
                  //  Convert.ToDouble(textBox18.Text),//加工内径
                Convert.ToDouble(comboBox4.SelectedIndex),//数据间隔
                Convert.ToDouble(comboBox5.SelectedIndex),//磨头直径
                Convert.ToDouble(comboBox8.SelectedIndex),//抛光轴方向
                Convert.ToDouble(comboBox9.SelectedIndex),//工件方向
                 Convert.ToDouble(comboBox1.SelectedIndex),//模仁材质
                Convert.ToDouble(comboBox3.SelectedIndex)//研磨颗粒规格
                };
                
            }
            catch (Exception)
            {
                MessageBox.Show("请输入所有参数再保存", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            SaveFileDialog sfd = new SaveFileDialog();  
            string InitialDire = System.Environment.CurrentDirectory; 
            sfd.Title = "模仁加工参数保存";
            sfd.InitialDirectory = InitialDire + "\\模仁加工参数";
            sfd.Filter = @"加工参数文件|*.txt2";//.csv| *.csv|加工参数文件|.*txt2";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }


           
            /***************写入TXT********/

            try
            {
                /*先清空parameter.txt文件内容*/
                FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream2.Seek(0, SeekOrigin.Begin);
                stream2.SetLength(0); //清空txt文件
                stream2.Close();
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                for (int i = 0; i < 17; i++)
                {
                    wr.WriteLine(Convert.ToString(Parameter[i]));
                }
                if (radioButton1.Checked == true)//工件转速
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton4.Checked == true)//进给速度
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton7.Checked == true)//研磨角度
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton23.Checked == true)//单段加工
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                wr.Close();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,"提示！");
                return;
            }
            //  path = sfd.FileName;//结果保存

           

            this.button10.Enabled = true;//应用按钮正常


        }

        private void timer4_Tick(object sender, EventArgs e)//加工参数设定界面的timer
        {
            timer4.Interval = 200;
            if (radioButton1.Checked == true)
            {
                textBox2.Enabled = true;
                Vc_flag = true;
            }
            else
            { 
                textBox2.Enabled = false;
                Vc_flag = false;
            }
            if (radioButton2.Checked == true)
            {
                textBox3.Enabled = true;
            }
            else
                textBox3.Enabled = false;
            if (radioButton4.Checked == true)
            {
                textBox5.Enabled = true;
                F_flag = true;
            }
            else
            {
                textBox5.Enabled = false;
                 F_flag = false;
            }

            if (radioButton3.Checked == true)
            {
                textBox4.Enabled = true;
            }
            else
                textBox4.Enabled = false;

        }

        private void button12_Click(object sender, EventArgs e)//转到模具几何参数设定
        {
           // this.tabControl1.SelectedIndex = 2;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = true;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
        }

        private void button44_Click(object sender, EventArgs e)//转到加工参数设定
        {
           // this.tabControl1.SelectedIndex = 3;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = true;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
        }

        private void button45_Click(object sender, EventArgs e)//转到手动模式
        {
           // this.tabControl1.SelectedIndex = 5;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = true;
            this.panel7.Visible = false;
        }

        private void button43_Click(object sender, EventArgs e)//转到面型误差修正
        {
            //this.tabControl1.SelectedIndex = 4;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = true;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            //textBox11.Text = textBox11.Text;
        }

        private void button31_Click(object sender, EventArgs e)//返回到模式选择页
        {
            //this.tabControl1.SelectedIndex = 1;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            timer3.Enabled = false;
            
        }

        private void button40_Click(object sender, EventArgs e)//读取并下载加工代码
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择加工代码";
            dialog.InitialDirectory = @".\";

            dialog.Filter = @".txt|*.txt";
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (fileName.Trim() == "")
                return;
            textBox11.Text = Path.GetFileNameWithoutExtension(fileName);
   /***************************提取第一个位置,生成代码*******************************************************/
            string line=null,result,X,B;
            StreamReader read_file = new StreamReader(fileName, System.Text.Encoding.Default);
            for (int i = 0; i < 5; i++)
            {
                line=read_file.ReadLine();
               
            }
            result = line;
            int x = result.IndexOf("X");
            int b = result.IndexOf("B");
            int f = result.IndexOf("F");
            // int f = result.IndexOf("F");
            string str1 = result.Substring(x + 1, b - x - 1);
            string str2 = result.Substring(b + 1, f - b - 1);
            string str3 = result.Substring(f + 1);
            //this.label2.Text = "X坐标：" + str;
            //this.label1.Text = "B坐标" + str2;
            // this.listBox2.Items.Add(str1 + "," + str2 + "," + str3);
           
            X = str1.Trim();
            B = str2.Trim();

            string result2 = @"C:\test3.txt";//结果保存到F:\result1.txt
            //先清空result1.txt文件内容
            FileStream stream3 = File.Open(result2, FileMode.OpenOrCreate, FileAccess.Write);
            stream3.Seek(0, SeekOrigin.Begin);
            stream3.SetLength(0); //清空txt文件
            stream3.Close();


            FileStream fs2 = new FileStream(result2, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
            StreamWriter wr = null;

            wr = new StreamWriter(fs2);
            wr.WriteLine("OPEN PROG 1");
            wr.WriteLine("CLEAR");
            // wr.WriteLine("S150");

            wr.WriteLine(" FRAX(X,Z)");
            wr.WriteLine("G90 G01");
            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");

            wr.WriteLine(Convert.ToString("X" + X + "  " + "B" + B + "  " + "F200"  + "  " + "\n"));
            wr.WriteLine("WHILE((" + (10000 * Convert.ToDouble(X)).ToString("f4") + "-M162/32/96)>100" + ")");
            wr.WriteLine("P10=0");
            wr.WriteLine("DWELL 10");
            wr.WriteLine("ENDWHILE");
            wr.WriteLine("P10=1");
            wr.WriteLine("CLOSE");

            //    wr.WriteLine("ENDWHILE");
            /*   if (n % 2 == 1)
               {
                   for (int i = 0; i < NC_Data.Length / 5; i++)
                   {
                       wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                   }

               }*/

            wr.Close();

            
           

    /********************************下载******************************************/
            // PMAC.GetResponse(pmacNumber,"enable plc 2",out status);//
            timer3.Enabled = false;
            bool pbSuccess;
            bool bMacro = true;
            bool bMap = true;
            bool bLog = true;
            bool bDnld = true;
            PMAC.Open(pmacNumber, out pbSuccess);
            PMAC.Download(pmacNumber, result2, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载
            if (pbSuccess)
            {
                Delay(3);
                PMAC.Download(pmacNumber, fileName, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载
                button40.Text = "读取成功";

            }
            else
                button40.Text = "读取失败";

            timer3.Enabled = true;
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)//X+
        {
            string status;
            if (textBox10.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
          //  MessageBox.Show("请设定运动速度！");
            if (textBox30.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
           // MessageBox.Show("请设定目标位置！");   
            if(Math.Abs(Convert.ToDouble(textBox10.Text))>x_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox30.Text)) > xlimit)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I122="+Convert.ToString(Math.Abs(Convert.ToDouble(textBox10.Text)*10)), out status);
            if (radioButton5.Checked == true && comboBox2.SelectedIndex == 0)//x点动
            {
                
                PMAC.GetResponse(pmacNumber, "#1J+", out status);

            }

            else if (radioButton5.Checked == true && comboBox2.SelectedIndex == 1)//X点位
            {
                PMAC.GetResponse(pmacNumber, "#1J=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox30.Text)) * 10000), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择X！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button7_MouseUp(object sender, MouseEventArgs e)//X+
        {
            string status;
            if (radioButton5.Checked == true && comboBox2.SelectedIndex == 0)//x点动停
            {
                PMAC.GetResponse(pmacNumber, "#1J/", out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
            }
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)//X-
        {
            string status;
            if (textBox10.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
            if (textBox30.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox10.Text)) > x_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox30.Text)) > xlimit)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I122=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox10.Text)) * 10), out status);
           
            if (radioButton5.Checked == true && comboBox2.SelectedIndex == 0)//X点动
            {
                PMAC.GetResponse(pmacNumber, "#1J-", out status);
            }

            else if (radioButton5.Checked == true && comboBox2.SelectedIndex == 1)//X点位
            {
                PMAC.GetResponse(pmacNumber, "#1J=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox30.Text)) * (-10000)), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择X！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void button8_MouseUp(object sender, MouseEventArgs e)//X-
        {
            string status;
            if (radioButton5.Checked == true && comboBox2.SelectedIndex == 0)//X点动停
            {
                PMAC.GetResponse(pmacNumber, "#1J/", out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
            }

        }

        private void button33_MouseDown(object sender, MouseEventArgs e)//B顺时针
        {
            string status;
            if (textBox9.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
            if (textBox31.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox9.Text)) > b_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox31.Text)) > 90)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I222=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox9.Text)) * 5), out status);
           
            if (radioButton6.Checked == true && comboBox2.SelectedIndex == 0)//B点动
            {
                PMAC.GetResponse(pmacNumber, "#2J+", out status);
            }
            else if (radioButton6.Checked == true && comboBox2.SelectedIndex == 1)//B点位
            {
                PMAC.GetResponse(pmacNumber, "#2J=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox31.Text)) * 5000), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择B！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void button33_MouseUp(object sender, MouseEventArgs e)//B顺时针
        {
            string status;
            if (radioButton6.Checked == true && comboBox2.SelectedIndex == 0)//B点动停
            {
                PMAC.GetResponse(pmacNumber, "#2J/", out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
            }

        }

        private void button32_MouseDown(object sender, MouseEventArgs e)//B逆时针
        {
            string status;
            if (textBox9.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
            if (textBox31.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox9.Text)) > b_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox31.Text)) > 90)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I222=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox9.Text) * 5)), out status);
           
            if (radioButton6.Checked == true && comboBox2.SelectedIndex == 0)//B点动
            {
                PMAC.GetResponse(pmacNumber, "#2J-", out status);
            }
            else if (radioButton6.Checked == true && comboBox2.SelectedIndex == 1)//B点位
            {
                PMAC.GetResponse(pmacNumber, "#2J=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox31.Text)) * (-5000)), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择B！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button32_MouseUp(object sender, MouseEventArgs e)//B逆时针
        {
            string status;
            if (radioButton6.Checked == true && comboBox2.SelectedIndex == 0)//B点动停
            {
                PMAC.GetResponse(pmacNumber, "#2J/", out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
            }

        }

        private void button35_MouseDown(object sender, MouseEventArgs e)//Z上升
        {
            /*string status;
            if (radioButton9.Checked == true)
                PMAC.GetResponse(0, "M7=1", out status);
            else
                MessageBox.Show("请先选择Z！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */
            string status;
            if (textBox21.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
            if (textBox22.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox21.Text)) > z_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox22.Text)) > zlimit_up)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I422=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox21.Text) * 10)), out status);
            if (radioButton9.Checked == true && comboBox2.SelectedIndex == 0)//Z点动
            {

                PMAC.GetResponse(pmacNumber, "#4J-", out status);

            }

            else if (radioButton9.Checked == true && comboBox2.SelectedIndex == 1)//Z点位
            {
                PMAC.GetResponse(pmacNumber, "#4J=-" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox22.Text)) * 10000), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择Z！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button35_MouseUp(object sender, MouseEventArgs e)//Z上升停
        {
           /* string status;
            PMAC.GetResponse(0, "M7=0", out status);
            */
            string status;

            if (radioButton9.Checked == true && comboBox2.SelectedIndex == 0)//Z点动停
            {
                PMAC.GetResponse(pmacNumber, "#4J/", out status);
            }
        }

        private void button34_MouseDown(object sender, MouseEventArgs e)//Z下降
        {
            /*string status;
            if (radioButton9.Checked == true)
                PMAC.GetResponse(0, "M6=1", out status);
            else
                MessageBox.Show("请先选择Z！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */
            string status;
            if (textBox21.Text == "")
            {
                MessageBox.Show("请设定运动速度！");
                return;
            }
            if (textBox22.Text == "")
            {
                MessageBox.Show("请设定目标位置！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox21.Text)) > z_axi_hand_maxfeed)
            {
                MessageBox.Show("运动速度过大！");
                return;
            }
            if (Math.Abs(Convert.ToDouble(textBox22.Text)) > zlimit_down)
            {
                MessageBox.Show("目标位置超程！");
                return;
            }
            PMAC.GetResponse(pmacNumber, "I422=" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox21.Text)) * 10), out status);

            if (radioButton9.Checked == true && comboBox2.SelectedIndex == 0)//Z点动
            {
                PMAC.GetResponse(pmacNumber, "#4J+", out status);
            }
            else if (radioButton9.Checked == true && comboBox2.SelectedIndex == 1)//Z点位
            {
                PMAC.GetResponse(pmacNumber, "#4J=+" + Convert.ToString(Math.Abs(Convert.ToDouble(textBox22.Text)) * 10000), out status);
            }
            else
            {
                //MessageBox.Show("请选择X或B");
                MessageBox.Show("请先选择Z！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button34_MouseUp(object sender, MouseEventArgs e)//Z下降停止
        {
            string status;
            if (radioButton9.Checked == true && comboBox2.SelectedIndex == 0)//Z停
            {
                PMAC.GetResponse(pmacNumber, "#4J/", out status);
            }

        }

        private void button36_MouseDown(object sender, MouseEventArgs e)//工作台开始
        {
            string status;
            int turn_dir = 0;
            if(comboBox6.SelectedIndex==0)
            {
                turn_dir = 0;
            }
            else
            {
                 turn_dir=1;
            }

         
            if (radioButton10.Checked == true)
            {

                if (textBox7.Text == "")
                {
                 
                    PMAC.GetResponse(0, "M302=1000", out status);
                    if (turn_dir == 0)
                    {
                        PMAC.GetResponse(0, "M3=1", out status);
                        PMAC.GetResponse(0, "M4=0", out status);
                    }
                    else
                    {
                        PMAC.GetResponse(0, "M3=0", out status);
                        PMAC.GetResponse(0, "M4=1", out status);

                    }
                   
                }
                else if (textBox7.Text != "")
                {
                    if (turn_dir == 0)
                    {
                        PMAC.GetResponse(0, "M3=1", out status);
                        PMAC.GetResponse(0, "M4=0", out status);
                    }
                    else
                    {
                        PMAC.GetResponse(0, "M3=0", out status);
                        PMAC.GetResponse(0, "M4=1", out status);

                    }
                    if (Math.Abs(Convert.ToDouble(textBox7.Text)) > C_axi_maxfeed)
                    {
                        MessageBox.Show("转动速度过大！");
                        return;
                    }
                    if (Math.Abs(Convert.ToDouble(textBox7.Text)) < C_axi_minspeed)
                    {
                        MessageBox.Show("转动速度过小！");
                        return;
                    }
                    PMAC.GetResponse(0, "M302=" + Convert.ToString(Convert.ToDouble(textBox7.Text) * C_motor_scale_factor + C_motor_offset), out status);
               
                }
                     else
                {

                }
            }

            else
                MessageBox.Show("请先选择工作台！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

          
        }

        private void button36_MouseUp(object sender, MouseEventArgs e)//工作台开始停
        {
            string status;
            if (radioButton10.Checked == true && comboBox2.SelectedIndex == 0)//工作台点动           
            {
                PMAC.GetResponse(0, "M302=0", out status);
                PMAC.GetResponse(0, "M3=0", out status);
                PMAC.GetResponse(0, "M4=0", out status);
            }

        }

        private void button37_MouseDown(object sender, MouseEventArgs e)//工作台停止
        {
            string status;

            PMAC.GetResponse(0, "M302=0", out status);
            PMAC.GetResponse(0, "M3=0", out status);

        }

        private void button38_MouseDown(object sender, MouseEventArgs e)//抛光头开始
        {
            string status;
             //string status;

            int turn_dir = 0;
          //  timer10.Enabled = false;
            if (comboBox7.SelectedIndex == 0)
            {
                turn_dir = 0;
            }
            else
            {
                turn_dir = 1;
            }
          //  tip11 = true;
            if (tip11 == true )//手轮急停
            {
                MessageBox.Show("手轮急停按钮按下，不能转动！");
                return;
            }
            if (tip12 == true)//按钮急停
            {
                MessageBox.Show("急停按钮按下，不能转动！");
                return;
            }
                

             if (radioButton11.Checked == true)
             {
                 if (textBox7.Text == null)
                 {
                     PMAC.GetResponse(0, "M0=0", out status);
                     PMAC.GetResponse(0, "M502=260", out status);
                     
                     if (turn_dir == 0)
                     {
                         PMAC.GetResponse(0, "M1=0", out status);
                     }
                     else
                     {
                         PMAC.GetResponse(0, "M1=1", out status);
                     }
                 }
                 else if (textBox7.Text != null)
                 {
                     if (turn_dir == 0)
                     {
                         PMAC.GetResponse(0, "M1=0", out status);
                     }
                     else
                     {
                         PMAC.GetResponse(0, "M1=1", out status);
                     }
                     PMAC.GetResponse(0, "M0=0", out status);
                     if (Math.Abs(Convert.ToDouble(textBox8.Text)) > polish_axi_maxfeed)
                     {
                         MessageBox.Show("转动速度过大！");
                         return;
                     }
                     if (Math.Abs(Convert.ToDouble(textBox8.Text)) < polish_axi_minfeed)
                     {
                         MessageBox.Show("转动速度过小！");
                         return;
                     }
                     PMAC.GetResponse(0, "M502=" + Convert.ToString(Convert.ToDouble(textBox8.Text) * U_motor_scale_factor + U_motor_offset), out status);
                 }
                 else
                 {

                 }
             }
             else
                 MessageBox.Show("请先选择抛光轴！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button38_MouseUp(object sender, MouseEventArgs e)//抛光头开始
        {
            string status;
           
            if (radioButton11.Checked == true && comboBox2.SelectedIndex == 0)//工作台点动           
            {
              //  timer10.Enabled = true;
                PMAC.GetResponse(0, "M0=1", out status);
                PMAC.GetResponse(0, "M502=1", out status);
              //  PMAC.GetResponse(0, "M0=1", out status);
            }

        }

        private void button39_MouseDown(object sender, MouseEventArgs e)//抛光头停止
        {
            string status;
          //  PMAC.GetResponse(0, "M0=1", out status);
          //  timer10.Enabled = true;
            PMAC.GetResponse(0, "M0=1", out status);
            PMAC.GetResponse(0, "M502=1", out status);
        }

        private void button39_MouseUp(object sender, MouseEventArgs e)//抛光头停止
        {

        }

        private void button42_Click(object sender, EventArgs e)//返回初始页
        {
            //this.tabControl1.SelectedIndex = 0;
            this.panel1.Visible = true;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
        }


        bool tip1 = false, tip2 = false, tip3 = false, tip4 = false, tip5 = false, tip6 = false, tip7 = false, tip8 = false, tip9 = false, tip10 = false, tip11 = false, tip12 = false;
        private void timer5_Tick(object sender, EventArgs e)//获取电机状态
        {
           // string status1,status2,status3,status4,status5,status6;

            //PMAC.GetResponse(pmacNumber, "#1J/", out status);
            timer5.Interval = 200;
            int X_ALARM, B_ALARM, C_ALARM,Z_ALARM, X_P_LIMIT, B_P_LIMIT, X_N_LIMIT, B_N_LIMIT, Z_P_LIMIT, Z_N_LIMIT,LUN_ALARM,STOP_ALARM;
            double finish_flag,pmac_connect_status,polish_uplimit_contaceflag;
           // bool status;
           
            X_ALARM=Pmac.GetM(123);//X轴警报
            B_ALARM=Pmac.GetM(223);//B轴警报
            C_ALARM = Pmac.GetM(343);//C轴警报
            LUN_ALARM = Pmac.GetM(50);//手轮急停警报
            STOP_ALARM = Pmac.GetM(9);//
            X_P_LIMIT=Pmac.GetM(121);//X正限位
            B_P_LIMIT=Pmac.GetM(221);//B正限位
            X_N_LIMIT=Pmac.GetM(122);//X负限位
            B_N_LIMIT=Pmac.GetM(222);//B负限位
            finish_flag = Pmac.GetP(32);//加工完成
            polish_uplimit_contaceflag = Pmac.GetP(48);//触碰抛光轴上限位
            pmac_connect_status = Pmac.GetP(33);//询问是否连接上
           // MessageBox.Show(finish_flag.ToString());
            Z_P_LIMIT = Pmac.GetM(421);//Z上限位
            Z_N_LIMIT = Pmac.GetM(422);//Z下限位
            Z_ALARM = Pmac.GetM(423);//Z轴警报
           // PAO_P_LIMIT = Pmac.GetM(321);//抛光轴上限位
          //  PMAC.GetResponseEx()
            if (pmac_connect_status == 1)
            {
                pmac_connect_status = 0;
            }
            else
            {
              //  PMAC.Open(pmacNumber, out status);
            }
            if (finish_flag == 1)
            {
                process_fininsh_flag = true;              
                button46.Enabled = true;//开始加工按钮
                button28.Enabled = true;//手动模式按钮
                button21.Enabled = true;//读取其它NC按钮
                button12.Enabled = true;//下载标准代码
                button47.Enabled = true;//下载补偿代码
                Pmac.SetP(32,0);
               // timer9.Enabled = false;//加工监测时钟停
                timer11.Enabled = true;//蜂鸣器响
                buzzer_work = true;
                MessageBox.Show("加工完成！","提示！");
                Pmac.SendCommand("M46=0");//绿灯灭
                Pmac.SetM(7, 0);//黄灯灭
                timer6.Enabled = false;
              //  timer10.Enabled = true;
                textBox47.Text = Convert.ToString("0时" + "0分" + "0秒");
                textBox45.Text = null;
                this.progressBar1.Value=0;
            }
            if (polish_uplimit_contaceflag==1)
            {
                button46.Enabled = true;//开始加工按钮
                button28.Enabled = true;//手动模式按钮
                button21.Enabled = true;//读取其它NC按钮
                button12.Enabled = true;//下载标准代码
                button47.Enabled = true;//下载补偿代码
                timer6.Enabled = false;
               // timer10.Enabled = true;
              //  timer9.Enabled = false;//加工监测时钟停
                Pmac.SetP(48, 0);
                textBox47.Text = Convert.ToString("0时" + "0分" + "0秒");
                textBox45.Text = null;
                this.progressBar1.Value = 0;
            }

          


            if (X_ALARM == 1 && tip1 == false&&connect_flag==true)
            {
                tip1 = true;
                X_ALARM_flag = true;
                MessageBox.Show("X轴驱动器警报");
                
            }
            if (X_ALARM == 0 && tip1 == true)
            {
                tip1 = false;
                X_ALARM_flag = false;

            }


        


            if (X_P_LIMIT == 1 && tip3 == false)
            {
                tip3 = true;
                MessageBox.Show("X轴左限位触发,请手动回原点后复位！" );
                
            }
            if (X_P_LIMIT == 0 && tip3 == true && connect_flag == true)
            {
                tip3 = false;
                //MessageBox.Show("Z轴上限位触发");

            }


            if (B_ALARM == 1 && tip2 == false && connect_flag == true)
            {
                tip2 = true;
                B_ALARM_flag = true;
                MessageBox.Show("B轴驱动器警报");

            }
            if (B_ALARM == 0 && tip2 == true)
            {
                tip2 = false;
                B_ALARM_flag = false;

            }

            if (B_P_LIMIT == 1 && tip4 == false)
            {
                tip4 = true;
                MessageBox.Show("B轴左限位触发,请手动回原点后复位！");
                
            }
            if (B_P_LIMIT == 0 && tip4 == true && connect_flag == true)
            {
                tip4 = false;             

            }

            if (X_N_LIMIT == 1 && tip5 == false)
            {
                tip5 = true;
                MessageBox.Show("X轴右限位触发,请手动回原点后复位！");
                
            }
            if (X_N_LIMIT == 0 && tip5 == true && connect_flag == true)
            {
                tip5 = false;               

            }


            if (B_N_LIMIT == 1&&tip6==false)
            {
                tip6 = true;
                MessageBox.Show("B轴右限位触发,请手动回原点后复位！");
                
            }
            if (B_N_LIMIT == 0 && tip6 == true && connect_flag == true)
            {
                tip6 = false;              

            }


            if (Z_N_LIMIT == 1 && tip7 == false&&connect_flag==true)
            {
                tip7 = true;
                MessageBox.Show("Z轴上限位触发,请手动回原点后复位！");
               
            }
            if (Z_N_LIMIT == 0 && tip7 == true && connect_flag == true)
            {
                tip7 = false;              

            }

            if (Z_P_LIMIT == 1 && tip8 == false && connect_flag == true)
            {
                tip8 = true;
                MessageBox.Show("Z轴下限位触发,请手动回原点后复位！");
                
            }
            if (Z_P_LIMIT == 0 && tip8 == true && connect_flag == true)
            {
                tip8 = false;              

            }

            if (Z_ALARM == 1 && tip9 == false&&connect_flag==true)
            {
                tip9 = true;
                Z_ALARM_flag = true;
                MessageBox.Show("Z轴驱动器警报");
               
            }
            if (Z_ALARM == 0 && tip9 == true)
            {
                tip9 = false;
                Z_ALARM_flag = false;           

            }

            if (C_ALARM == 1 && tip10 == false && connect_flag == true)

            {
                tip10 = true;
                C_ALARM_flag = true;
                MessageBox.Show("C轴警报");
                
            }

            if (C_ALARM == 0 && tip10 == true)
            {
                tip10 = false;
                C_ALARM_flag = false;

            }

            if (LUN_ALARM == 0 && tip11 == false && connect_flag == true)
            {
                tip11 = true;
              
                Pmac.SetM(47, 1);//亮红灯
                Pmac.SendCommand("&1A");
                Pmac.SendCommand("#1j/");
                Pmac.SendCommand("#2j/");
                Pmac.SendCommand("m502=0");
                Pmac.SendCommand("m302=0");
                Pmac.SendCommand("m6=0");
                Pmac.SendCommand("m7=0");
                Pmac.SendCommand("m0=1");
                MessageBox.Show("手轮急停警报");

            }
            if (LUN_ALARM == 1 && tip11 == true)
            {
                tip11 = false;
               
                Pmac.SetM(47, 0);//灭红灯
                Pmac.SendCommand("m0=0");
               // MessageBox.Show("急停警报");

            }
 
            if (STOP_ALARM == 1 && tip12 == false)
            {
                tip12 = true;
               
                Pmac.SetM(47, 1);//亮红灯
                Pmac.SendCommand("&1A");
                Pmac.SendCommand("#1j/");
                Pmac.SendCommand("#2j/");
                Pmac.SendCommand("m502=0");
                Pmac.SendCommand("m302=0");
                Pmac.SendCommand("m6=0");
                Pmac.SendCommand("m7=0");
                Pmac.SendCommand("m0=1");
                Pmac.SendCommand("m3=0");
                Pmac.SendCommand("m4=0");
                MessageBox.Show("急停警报");

            }
            if (STOP_ALARM == 0 && tip12 == true)
            {
                tip12 = false;
                Pmac.SendCommand("m0=0");
                Pmac.SetM(47, 0);//灭红灯
              //  MessageBox.Show("急停警报");

            }
        }

        private void button21_Click_1(object sender, EventArgs e)//读取其它软件生成的代码，进行转换
        {
            
            string result = "";
            string line;
         //   string status;
            string myfile;
            OpenFileDialog dialog = new OpenFileDialog();
            StreamReader MyReader = null;
            StreamReader _rstream = null;//为Gcode所创建的流
            StreamReader _rstream1 = null;//为Gcode所创建的流
            dialog.Title = "代码读取";
            dialog.InitialDirectory = System.Environment.CurrentDirectory + "\\其它软件转换代码";//@".\面型补正数据\";

            dialog.Filter = @".NCS|*.NCS";
            dialog.ShowDialog();

            myfile = dialog.FileName;

            if (myfile.Trim() == "")
                return;

            button21.Text = "读取中";

             try
              {
         /******************提取数据**********************************/
            int i = 0, j = 0, k = 0;
            double H,n,speed,speed_multiple,clamp_h,SAG;
            H = Convert.ToDouble(textBox13.Text);//工件高度 
            clamp_h = Convert.ToDouble(textBox20.Text);//夹具高度 
            SAG= Convert.ToDouble(textBox50.Text);//矢高
            n = Convert.ToDouble(textBox12.Text); 
            speed = Convert.ToDouble(textBox15.Text); //H工件底面到面型最极值点距离，n加工次数,speed抛光轴转速,speed_multiple速度倍率
            speed_multiple = Convert.ToDouble(textBox19.Text)/100;
            double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0;//各个程序段的运行时间
            double xend1 = 0, xend2 = 0, xend3 = 0, xend4 = 0, xend5 = 0, xend6 = 0, xend7 = 0;//各个程序段结束加工X
            double xstart2 = 0, xstart3 = 0, xstart4 = 0, xstart5 = 0, xstart6 = 0, xstart7 = 0, xstart8 = 0;//各个程序段开始加工X
            double zend1 = 0, zend2 = 0, zend3 = 0, zend4 = 0,zend5 = 0, zend6 = 0, zend7 = 0;//各个程序段结束加工Z
            double zstart2 = 0, zstart3 = 0, zstart4 = 0, zstart5 = 0, zstart6 = 0, zstart7 = 0, zstart8 = 0;//各个程序段开始加工Z
            
            double t = 0;//加工时间
            MyReader = new StreamReader(myfile, System.Text.Encoding.Default);
            _rstream = new StreamReader(myfile, System.Text.Encoding.UTF8);
            _rstream1 = new StreamReader(myfile, System.Text.Encoding.UTF8);


            while ((line = _rstream1.ReadLine()) != null)
                j = j + 1;
           // this.label2.Text = j.ToString();

            double[,] data = new double[j - 9, 5];//存放提取数据

            while ((line = _rstream.ReadLine()) != null)
            {

               // this.listBox1.Items.Add(line);
                if (i == 3)
                {

                    result = line;
                    int s = result.IndexOf("S");
                    string str1 = result.Substring(s + 1);
                    //this.label2.Text = "X坐标：" + str;
                    //this.label1.Text = "B坐标" + str2;
                    //this.listBox2.Items.Add(str1);
                    k = k + 1;
                    data[0, 0] = Convert.ToDouble(str1.Trim());
                    // break;
                }
                if (i == 4)
                {

                    result = line;
                    int x = result.IndexOf("X");
                    int y = result.IndexOf("Y");
                    int b = result.IndexOf("B");
                    // int f = result.IndexOf("F");
                    string str1 = result.Substring(x + 1, y - x - 1);
                    string str2 = result.Substring(y + 1, b - y - 1);
                    string str3 = result.Substring(b + 1);
                    //this.label2.Text = "X坐标：" + str;
                    //this.label1.Text = "B坐标" + str2;
                   // this.listBox2.Items.Add(str1 + "," + str2 + "," + str3);
                    k = k + 1;
                    data[1, 0] = Convert.ToDouble(str1.Trim());
                    data[1, 1] = Convert.ToDouble(str2.Trim());
                    data[1, 2] = Convert.ToDouble(str3.Trim());
                    // break;
                }
                if (i == 7)
                {

                    result = line;
                    int x = result.IndexOf("X");
                    int y = result.IndexOf("Y");
                    int b = result.IndexOf("B");
                    int f = result.IndexOf("F");
                    string str1 = result.Substring(x + 1, y - x - 1);
                    string str2 = result.Substring(y + 1, b - y - 1);
                    string str3 = result.Substring(b + 1, f - b - 1);
                    string str4 = result.Substring(f + 1);
                    //this.label2.Text = "X坐标：" + str;
                    //this.label1.Text = "B坐标" + str2;
                  //  this.listBox2.Items.Add(str1 + "," + str2 + "," + str3 + "," + str4);
                    k = k + 1;
                    data[2, 0] = Convert.ToDouble(str1.Trim());
                    data[2, 1] = Convert.ToDouble(str2.Trim());
                    data[2, 2] = Convert.ToDouble(str3.Trim());
                    data[2, 3] = Convert.ToDouble(str4.Trim());
                    // break;
                }
                if (i >= 9 & i < j - 4)
                {

                    result = line;
                    int x = result.IndexOf("X");
                    int y = result.IndexOf("Y");
                    int b = result.IndexOf("B");
                    int f = result.IndexOf("F");
                    string str1 = result.Substring(x + 1, y - x - 1);
                    string str2 = result.Substring(y + 1, b - y - 1);
                    string str3 = result.Substring(b + 1, f - b - 1);
                    string str4 = result.Substring(f + 1);
                    //this.label2.Text = "X坐标：" + str;
                    //this.label1.Text = "B坐标" + str2;
                    //this.listBox2.Items.Add(str1 + "," + str2 + "," + str3 + "," + str4);
                    k = k + 1;
                    data[i - 6, 0] = Convert.ToDouble(str1.Trim());
                    data[i - 6, 1] = Convert.ToDouble(str2.Trim());
                    data[i - 6, 2] = Convert.ToDouble(str3.Trim());
                    data[i - 6, 3] = Convert.ToDouble(str4.Trim());

                    // break;
                }
                if (i == j - 2)
                {
                    result = line;
                    int x = result.IndexOf("X");
                    int y = result.IndexOf("Y");
                    int b = result.IndexOf("B");
                    int f = result.IndexOf("F");
                    string str1 = result.Substring(x + 1, y - x - 1);
                    string str2 = result.Substring(y + 1, b - y - 1);
                    string str3 = result.Substring(b + 1, f - b - 1);
                    string str4 = result.Substring(f + 1);
                    //this.label2.Text = "X坐标：" + str;
                    //this.label1.Text = "B坐标" + str2;
                  //  this.listBox2.Items.Add(str1 + "," + str2 + "," + str3 + "," + str4);
                    k = k + 1;

                    data[j - 10, 0] = Convert.ToDouble(str1.Trim());
                    data[j - 10, 1] = Convert.ToDouble(str2.Trim());
                    data[j - 10, 2] = Convert.ToDouble(str3.Trim());
                    data[j - 10, 3] = Convert.ToDouble(str4.Trim());
                }
                i++;

            }
            //this.label1.Text = k.ToString();



            /*********计算转换data数组里的数据***********/
            double r0,max_x=0,max_z=0;//max_x最大X位移
            double[,] data2 = new double[j - 9, 5];//存放代码数据，

            if (data[2, 1] > 0)//判断凹凸
                SAG = -SAG;

            for (int l = 2; l < data.Length / 5; l++)//x移动
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                // X[i] = -D / 2 + i * 0.001;
              //  r0[i] = Math.Sqrt(Math.Pow(X[i], 2) + Math.Pow(Z[i], 2));//转动中心半径

             //   ZZ[i] = r0[i] * Math.Abs(Math.Cos(B[i] * Math.PI / 180 + Math.Asin(X[i] / r0[i])));//Z轴坐标
              

                r0 = Math.Sqrt(Math.Pow(data[l, 0], 2) + Math.Pow(data[l, 1] + H + clamp_h+SAG-Lworkpiece_h+other_h, 2));//转动中心半径  33.78
                
                //data[l, 4] = r0 * Math.Abs(Math.Cos(data[l, 2] * Math.PI / 180 + Math.Asin(data[l, 0] / r0)));//Z轴转动中心
              //  MessageBox.Show(data[l, 0].ToString());
                //X2[i]=r0[i]*Math.Cos(Math.Acos(X[i]/r0[i])-Math.Atan(Z2[i]));//x移动坐标  
                data2[l, 0] = -r0 * Math.Cos(Math.PI / 2 -Math.Asin(data[l, 0] / r0) + data[l, 2] * Math.PI / 180);//x移动坐标
                //dangle=(beta1-alfa1); //切线与转动中心连线的夹角
                // dX=(r0*cos(dangle));
                data[l, 4] = Math.Sqrt(Math.Pow(r0, 2) - Math.Pow(data2[l, 0], 2));//Z轴坐标
               
                if (Math.Abs(data2[l, 0])> max_x)
                    max_x = Math.Abs(data2[l, 0]);

                if (Math.Abs(data[l, 4])> max_z)
                    max_z = Math.Abs(data[l, 4]);
                
                
            }
            if (Math.Abs(max_x) > xlimit)
            {
                MessageBox.Show("加工代码超出X轴行程");
                button21.Text = "读取其它NC";
                return;
            }


            //string result5 = System.Environment.CurrentDirectory + "\\其它软件转换代码\\test4.txt";//@"D:\test.txt";//结果保存到F:\result1.txt
            ////*先清空result1.txt文件内容
            //FileStream stream5 = File.Open(result5, FileMode.OpenOrCreate, FileAccess.Write);
            //stream5.Seek(0, SeekOrigin.Begin);
            //stream5.SetLength(0); //清空txt文件
            //stream5.Close();


            //FileStream fs5 = new FileStream(result5, FileMode.Append);
            //// fs.Seek(0, SeekOrigin.Begin);
            //// fs.SetLength(0);
            //StreamWriter wr5 = null;

          //  wr5 = new StreamWriter(fs5);

            for (int l = 3; l < data.Length / 5; l++)//F1进给速度
            {
                // ArrayList_X.Add(-D/2+i*0.001);
                //X[i] = -D / 2 + i * 0.001;
                // F1=abs(dist.*Vc./(2.*pi.*(2.*x+dist)));%dist距离走一圈的进给速度 
                //  T1=dist./F1;%工件的
                //  TT=sum(T1);
                //F1[i] = Math.Abs(dist * Vc[i] / (0.35 * 2 * Math.PI * (2 * X[i] + dist)));
                double T;
               // T = 0.1 / data[l, 3];
                if(radioButton17.Checked==true)//F是X联动速度
                {
                    T = 0.1 / data[l, 3];
                    data2[l, 3] = Math.Abs((data2[l, 0] - data2[l - 1, 0]) / T * speed_multiple);
                   
                    t = t + T;
                    
                }
                else if (radioButton18.Checked == true)//F是B联动速度
                {
                  /*  T = Math.Sqrt(Math.Pow(data[l, 2] - data[l - 1, 2], 2)) / data[l, 3];
                    data2[l, 3] = Math.Abs((data2[l, 1] - data2[l - 1, 1]) / T * speed_multiple);
                    t = t + T;*/
                    T = Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(data[l, 2] - data[l - 1, 2], 2)) / data[l, 3];
                    data2[l, 3] = Math.Abs((data2[l, 0] - data2[l - 1, 0]) / T * speed_multiple);
                    t = t + T;

                }
                else//F是X B联动速度
                {
                    /* T = Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(data[l, 2] - data[l - 1, 2], 2)) / data[l, 3];
                      data2[l, 3] = Math.Abs((data2[l, 0] - data2[l - 1, 0]) / T * speed_multiple);
                      t = t + T;

                     */ //F是XY联动速度
                                         T = Math.Sqrt(Math.Pow(0.1, 2) + Math.Pow(data[l, 1] - data[l - 1, 1], 2)) / data[l, 3];
                                         data2[l, 3] = Math.Abs(Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]),2)+Math.Pow((data[l, 4] - data[l - 1, 4]),2)) / T * speed_multiple); //Math.Abs((data2[l, 0] - data2[l - 1, 0]) / T * speed_multiple);
                                         if (l < data.Length / 5 - 1)                   
                                         t = t + T;
                                 //        data2[l, 2] = T;
                                //         wr5.WriteLine("X  " + data2[l, 0].ToString() + "  F" + data2[l, 3].ToString());
                }
            }
         //   wr5.Close();
            t   = t / speed_multiple;//加了倍率后算算时间   
            data2[2, 3] = first_position_feed;
            t = (t * n + Math.Abs(data2[2, 0] / data2[2, 3])) + (B_axi_Center_Polish_head_distance- Math.Abs(data[2, 4])) / z_axi_first_jogdownfeed + 0.05;//单段加工时间




        

            /*********************************生成代码******************************************************************/

           /* SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "模仁加工代码保存";
            sfd.InitialDirectory = @".\";
            sfd.Filter = "文本文件| *.txt";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }
*/
            // string result1 = @".\GCode.txt";//结果保存到F:\result1.txt
            //string result1 = System.Environment.CurrentDirectory + "\\其它软件转换代码\\test1.txt";//@"D:\test.txt";//结果保存到F:\result1.txt
            ////*先清空result1.txt文件内容
            //FileStream stream2 = File.Open(result1, FileMode.OpenOrCreate, FileAccess.Write);
            //stream2.Seek(0, SeekOrigin.Begin);
            //stream2.SetLength(0); //清空txt文件
            //stream2.Close();
           

            //FileStream fs = new FileStream(result1, FileMode.Append);
            //// fs.Seek(0, SeekOrigin.Begin);
            //// fs.SetLength(0);
            //StreamWriter wr = null;

            //wr = new StreamWriter(fs);
            //wr.WriteLine("OPEN PROG 2");
            //wr.WriteLine("CLEAR");
            //wr.WriteLine("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
            //wr.WriteLine("M502=" + Convert.ToString(speed * U_motor_scale_factor+U_motor_offset));
            //wr.WriteLine("G90 G01 P25=0 P32=0");

            //if (comboBox11.SelectedIndex == 0)//工件转动方向
            //{
            //    wr.WriteLine("M3=1");
            //    wr.WriteLine("M4=0");
            //}
            //else
            //{
            //    wr.WriteLine("M3=0");
            //    wr.WriteLine("M4=1");
            //}
            //if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
            //{
            //    wr.WriteLine("M1=0");

            //}
            //else
            //{
            //    wr.WriteLine("M1=1");
            //}
            //if (n == 1)
            //{
            //    // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
            //    for (int l = 2; l < data2.Length / 5 - 1; l++)
            //    {
            //        wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "\n"));
            //    }
            //}
            //else
            //{
            //    if (n % 2 == 1)
            //        wr.WriteLine("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
            //    else
            //        wr.WriteLine("WHILE(P25<" + Convert.ToString(n / 2) + ")");

            //    for (int l = 2; l < data2.Length / 5 - 1; l++)
            //    {
            //        wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "\n"));
            //    }
            //    for (int l = data2.Length / 5- 2; l > 2; l--)//代码倒转
            //    {
            //        wr.WriteLine(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "\n"));
            //    }
            //    wr.WriteLine("P25=P25+1");
            //    wr.WriteLine("ENDWHILE");
            //    if(n%2==1)
            //    {
            //        for (int l = 2; l < data2.Length / 5 - 1; l++)
            //        {
            //            wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "\n"));
            //        }

            //    }
            //}

            ////    wr.WriteLine("ENDWHILE");
            ///*   if (n % 2 == 1)
            //   {
            //       for (int i = 0; i < NC_Data.Length / 5; i++)
            //       {
            //           wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
            //       }

            //   }*/
            //wr.WriteLine("DWELL 0");
                 
            //wr.WriteLine("P14=1");
            //wr.WriteLine("P32=1");
            //wr.WriteLine("CLOSE");
            //wr.Close();

                 /**************生成到初始加工位置的代码*********/


         //   string result2 = System.Environment.CurrentDirectory + "\\其它软件转换代码\\test2.txt"; //@"D:\test2.txt";//结果保存到F:\result1.txt
         //   //*先清空result1.txt文件内容
         //   FileStream stream3 = File.Open(result2, FileMode.OpenOrCreate, FileAccess.Write);
         //   stream3.Seek(0, SeekOrigin.Begin);
         //   stream3.SetLength(0); //清空txt文件
         //   stream3.Close();


         //   FileStream fs2 = new FileStream(result2, FileMode.Append);
         //   // fs.Seek(0, SeekOrigin.Begin);
         //   // fs.SetLength(0);
         ////   StreamWriter wr2 = null;

         //   wr = new StreamWriter(fs2);
         //   wr.WriteLine("OPEN PROG 1");
         //   wr.WriteLine("CLEAR");
         //  // wr.WriteLine("S150");
         //   Pmac.SendCommand("FRAX(X,Z)");
         //   wr.WriteLine("G90 G01");
         //    wr.WriteLine(("P50=" + Convert.ToString(data[2, 4])));
            
         //       wr.WriteLine(Convert.ToString("X" + data2[2, 0].ToString("f4") + "  " + "B" + data[2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "\n"));
                
         //       wr.WriteLine("WHILE(ABS(" + (10000*data2[2, 0]).ToString("f4")+"-M162/32/96)>100"+")");
         //       wr.WriteLine("P10=0");
         //       wr.WriteLine("DWELL 10");
         //       wr.WriteLine("ENDWHILE");
         //       wr.WriteLine("P10=1");
         //       wr.WriteLine("CLOSE");

         //   //    wr.WriteLine("ENDWHILE");
         //   /*   if (n % 2 == 1)
         //      {
         //          for (int i = 0; i < NC_Data.Length / 5; i++)
         //          {
         //              wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
         //          }

         //      }*/
                    
         //   wr.Close();



            /********************************多段加工代码测试***************/
    //         string result4 = System.Environment.CurrentDirectory + "\\其它软件转换代码\\test3.txt"; //@"D:\test2.txt";//结果保存到F:\result1.txt
    //   //*先清空result1.txt文件内容
    //   FileStream stream4 = File.Open(result4, FileMode.OpenOrCreate, FileAccess.Write);
    //   stream4.Seek(0, SeekOrigin.Begin);
    //   stream4.SetLength(0); //清空txt文件
    //   stream4.Close();


    //   FileStream fs4 = new FileStream(result4, FileMode.Append);
    //   // fs.Seek(0, SeekOrigin.Begin);
    //   // fs.SetLength(0);
    ////   StreamWriter wr2 = null;

    //   wr = new StreamWriter(fs4);
                  
/********************************下载到pmac*****************************************************************/


            string fileName = myfile;
            if (fileName.Trim() == "")
                return;
            textBox11.Text = Path.GetFileNameWithoutExtension(fileName);

            // PMAC.GetResponse(pmacNumber,"enable plc 2",out status);//
            bool pbSuccess;
       //     bool bMacro = true;
      //      bool bMap = true;
      //      bool bLog = true;
       //     bool bDnld = true;
            timer5.Enabled = false;
           // dectect.Abort();
            timer3.Enabled = false;
            PMAC.Open(pmacNumber, out pbSuccess);
           
           // PMAC.Download(pmacNumber, result2, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载

            File.Delete(System.Environment.CurrentDirectory + "\\其它软件转换代码\\test2.txt");

            File.Delete(System.Environment.CurrentDirectory + "\\其它软件转换代码\\test1.txt");

/*************在线下载测试**********************/

            try
            {
                if (radioButton21.Checked == true)//单段程序加工
                {
                    if (data[2, 4] < min_z)
                    {
                        MessageBox.Show("Z轴行程不足，无法加工！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;


                    }
                    if (speed_multiple > 10)
                    {
                        MessageBox.Show("速度倍率设置过大！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;


                    }
                    if (speed_multiple < 0.05)
                    {
                        MessageBox.Show("速度倍率设置过小！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;


                    }

                    Pmac.SendCommand("OPEN PROG 1");//初始位置
                    Pmac.SendCommand("CLEAR");
                    // wr.WriteLine("S150");
                    Pmac.SendCommand("DISABLE PLC6");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");
                    Pmac.SendCommand(("P50=" + Convert.ToString(data[2, 4])));
                    Pmac.SendCommand("P26=1");
                    Pmac.SendCommand("P27=1");
                    Pmac.SendCommand(Convert.ToString("X" + data2[2, 0].ToString("f4") + "  " + "B" + data[2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 , 0] * 10).ToString("f4") + "M213==" + (-data[2, 1] * 1000).ToString("f4") + "  " + "\n"));

                    Pmac.SendCommand("WHILE(ABS(" + (10000 * data2[2, 0]).ToString("f4") + "-M162/32/96)>100" + ")");
                    Pmac.SendCommand("P10=0");
                    Pmac.SendCommand("DWELL 10");
                    Pmac.SendCommand("ENDWHILE");
                    Pmac.SendCommand("P10=1");
                    Pmac.SendCommand("CLOSE");
                    // 加工代码       

                    Pmac.SendCommand("OPEN PROG 7");//运动到初始位置
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");
                     Pmac.SendCommand(Convert.ToString("X" + data2[2, 0].ToString("f4") + "  " + "B" + data[2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2, 0] * 10).ToString("f4") + "M213==" + (-data[2, 1] * 1000).ToString("f4") + "  " + "\n"));
                     Pmac.SendCommand("CLOSE");

                     Pmac.SendCommand("OPEN PROG 8");//运动到初始位置
                     Pmac.SendCommand("CLEAR");
                     Pmac.SendCommand("FRAX(X,Z)");
                     Pmac.SendCommand("G90 G01");
                     Pmac.SendCommand(Convert.ToString("X" + (-data2[2, 0]).ToString("f4") + "  " + "B" + (-data[2, 2]).ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2, 0] * 10).ToString("f4") + "M213==" + (-data[2, 1] * 1000).ToString("f4") + "  " + "\n"));
                     Pmac.SendCommand("CLOSE");



                    Pmac.SendCommand("OPEN PROG 2");//加工程序
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("PSET Z(P50)");//加工程序
                    Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor+C_motor_offset));
                    Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                    // Pmac.SendCommand("FRAX(X)");
                    Pmac.SendCommand("G90 G01 P25=0 P32=0");
                    //wr.WriteLine("G90 G01 P25=0 P32=0");
                    // Pmac.SendCommand("M0=0");

                    if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SendCommand("M1=0");

                    }
                    else
                    {
                        Pmac.SendCommand("M1=1");
                    }
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3=1");
                        Pmac.SendCommand("M4=0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3=0");
                        Pmac.SendCommand("M4=1");
                    }
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3==1");
                        Pmac.SendCommand("M4==0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3==0");
                        Pmac.SendCommand("M4==1");
                    }
                    Pmac.SendCommand("P26=1");
                    Pmac.SendCommand("P27=0");

                    if (n == 1)
                    {
                        Pmac.SendCommand("P27=P27+1 DWELL0");
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        for (int l = 2; l < data2.Length / 5 - 1; l++)
                        {

                            Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                        
                        }
                    }
                    else
                    {
                        if (n % 2 == 1)
                            Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                        else
                            Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                        Pmac.SendCommand("P27=P27+1 DWELL0");
                        for (int l = 2; l < data2.Length / 5 - 1; l++)
                        {
                            Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                        }
                        Pmac.SendCommand("P27=P27+1 DWELL0");
                        for (int l = data2.Length / 5 - 2; l > 2; l--)//代码倒转
                        {
                            Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                        }
                        Pmac.SendCommand("P25=P25+1");
                        Pmac.SendCommand("ENDWHILE");
                        if (n % 2 == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL0");
                            for (int l = 2; l < data2.Length / 5 - 1; l++)
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }

                        }
                    }


                    Pmac.SendCommand("DWELL 0");
                    Pmac.SendCommand("P14=1");
                    Pmac.SendCommand("P32=1");
                    Pmac.SendCommand("M7=1");
                    Pmac.SendCommand("M113=0 M213=0");
                    Pmac.SendCommand("ENABLE PLC6");
                    Pmac.SendCommand("CLOSE");
                }
                else //多段程序加工*******************************//
                {
                    if (data[2, 4] < min_z)
                    {
                        MessageBox.Show("Z轴行程不足，无法加工！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;
                    }
                    if (speed_multiple > 10)
                    {
                        MessageBox.Show("速度倍率设置过大！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;
                    }
                    if (speed_multiple < 0.05)
                    {
                        MessageBox.Show("速度倍率设置过小！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button21.Text = "读取其它NC";
                        return;
                    }

                    if (String.IsNullOrWhiteSpace(textBox40.Text))
                    {
                        MessageBox.Show("请先设定多段程序加工范围！！", "提示！");
                        button21.Text = "读取其它NC";
                        Pmac.SendCommand("CLOSE");
                        return;
                    }
                    Pmac.SendCommand("OPEN PROG 1");//初始位置
                    Pmac.SendCommand("CLEAR");
                    // wr.WriteLine("S150");
                    Pmac.SendCommand("DISABLE PLC6");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");
                    Pmac.SendCommand("P26=1");
                    Pmac.SendCommand("P27=1");

                    Pmac.SendCommand(("P50=" + Convert.ToString(data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 4])));

                    Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));
                   t1 = t1 + Math.Abs(data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0] / data2[2, 3]);//缓存区1到初始位置时间
                    Pmac.SendCommand("WHILE(ABS(" + (10000 * data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0]).ToString("f4") + "-M162/32/96)>100" + ")");
                    Pmac.SendCommand("P10=0");
                    Pmac.SendCommand("DWELL 10");
                    Pmac.SendCommand("ENDWHILE");
                    Pmac.SendCommand("P10=1");
                    Pmac.SendCommand("CLOSE");
                    // 加工代码       

                    Pmac.SendCommand("OPEN PROG 7");//初始位置左
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");
                
                    Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));
                    Pmac.SendCommand("CLOSE");

                    Pmac.SendCommand("OPEN PROG 8");//初始位置右
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");

                    Pmac.SendCommand(Convert.ToString("X" + (-data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0]).ToString("f4") + "  " + "B" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 2]).ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 20)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));
                    Pmac.SendCommand("CLOSE");


                    Pmac.SendCommand("OPEN PROG 2");//加工程序
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("PSET Z(P50)");//加工程序
                    Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                    Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                    // Pmac.SendCommand("FRAX(X)");
                    Pmac.SendCommand("G90 G01 P25=0 P32=0");
                    //wr.WriteLine("G90 G01 P25=0 P32=0");
                    // Pmac.SendCommand("M0=0");

                    if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SendCommand("M1=0");

                    }
                    else
                    {
                        Pmac.SendCommand("M1=1");
                    }
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3=1");
                        Pmac.SendCommand("M4=0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3=0");
                        Pmac.SendCommand("M4=1");
                    }
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3==1");
                        Pmac.SendCommand("M4==0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3==0");
                        Pmac.SendCommand("M4==1");
                    }


 /*********************************************************************************************************************/
                    string z_feed = "F"+zfeed.ToString();//Z轴上升和下降速度
                    // zfeed = 120;//Z轴上升和下降速度
                    // xBfeed = 240;//X B到程序段位置速度
                    string X_B_feed = "F"+xBfeed.ToString();//X B到程序段位置速度
                  
                    if (Math.Abs(Convert.ToDouble(textBox40.Text)) == 0 && Math.Abs(Convert.ToDouble(textBox40.Text)) == 0 && Math.Abs(Convert.ToDouble(textBox40.Text)) == 0)
                    {
                        MessageBox.Show("请先设定多段程序加工范围！！", "提示！");
                        button21.Text = "读取其它NC";
                        Pmac.SendCommand("CLOSE");
                        return;
                    }
                  
                    //程序段1
                    if (checkBox1.Checked == true)//程序段1
                    {
                        n = Convert.ToDouble(textBox52.Text);
                        if(n==0)
                         n = 1;
                        double begin_point = Convert.ToDouble(textBox40.Text), end_point = Convert.ToDouble(textBox51.Text);//程序加工初始位置和程序加工结束位置
                       
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序1加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=0");
                       // double t0 = 0;
                        if (n <= 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                            
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                //   t1 = t1 + Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                {
                                    t1 = t1 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];//程序1抛光时间
                              //      data2[l, 1] = Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];//程序1抛光时间
                             //       wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                          
                                }
                               //   if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                            //        t0 = t0 + Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                            
                            }

                         //  MessageBox.Show(Convert.ToString(t0));

                            xend1 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend1 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t1 = t1 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];//程序1抛光时间
                            //    wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                              //  wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");

                            xend1 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];//程序1 X结束位置
                            zend1 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];//程序1 Z结束位置

                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {



                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                //    wr.WriteLine(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            
   

                                }

                                xend1 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend1 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];

                            }
                        }
                        t1 = t1 * n + (B_axi_Center_Polish_head_distance - Math.Abs(data[2 + (data2.Length / 5 - 3 - 
                            Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4])) / z_axi_first_jogdownfeed 
                            + 0.05;
                        /*程序1加工时间：  抛光时间*抛光次数（t*n）+Z轴上升时间（100
                        是Z在初始位置时，抛光头在第二个限位位置时到B回转中心距离，100减去Z轴初始位
                         置点位移，再除以速度时6mm/s，除以60转化为分钟,）
                        +估计补正气缸下落时间 （上面代码加了xb到初始位置时间，这里没显示） */
                        Pmac.SendCommand("DWELL 0");

                    }
  /*********************************************************************************************************************/
              //      wr.Close();

                    if (checkBox2.Checked == true)//程序段2
                    {
                        n = Convert.ToDouble(textBox53.Text);
                     
                        if (n <1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox55.Text), end_point = Convert.ToDouble(textBox54.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序2加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }

                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                 
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4")+z_feed+" "+"DWELL 10");                       
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed +" " +"DWELL 10" + "  " + "\n"));
                       //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=2 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart2 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart2 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];//Z轴初始点位置
                   
               
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t2 = t2 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; ////Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                             xend2 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                             zend2 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];

                                     }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t2 = t2 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "\n"));
                            }

                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");

                            xend2 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend2 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend2 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend2 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                            }
                        }
                        t2 = t2 * n + Math.Abs(max_z + 30 - zend1) / zfeed + 
                            Math.Abs(xstart2 - xend1) / xBfeed
                            + Math.Abs(zstart2 - max_z - 30) / zfeed;
                        //程序段2抛光时间  抛光时间*n+Z下来时间+XB到程序段2初始位时间
                        //+Z轴上去到程序段2初始Z位置时间，其中zfeed==240mm/min,xBfeed默认240mm/min
                        Pmac.SendCommand("DWELL 0");
                    }

    /*********************************************************************************************************************/


                    if (checkBox3.Checked == true)//程序段3
                    {
                        n = Convert.ToDouble(textBox56.Text);
                       
                        if (n <1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox58.Text), end_point = Convert.ToDouble(textBox57.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序3加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }

                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=3 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");

                        xstart3 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart3 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
                   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t3 = t3 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            xend3 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend3 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t3 = t3 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend3 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend3 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend3 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend3 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                            }
                        }
                        t3 = t3 * n + Math.Abs(max_z + 30 - zend2) / zfeed + Math.Abs(xstart3 - xend2) / xBfeed + Math.Abs(zstart3 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

  /*********************************************************************************************************************/

                    if (checkBox4.Checked == true)//程序段4
                    {
                        n = Convert.ToDouble(textBox59.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox61.Text), end_point = Convert.ToDouble(textBox60.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序4加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=4 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");

                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart4 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart4 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t4 = t4 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            xend4 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend4 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t4 = t4 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend4 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend4 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend4 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend4 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                            }
                        }
                        t4 = t4 * n + Math.Abs(max_z + 30 - zend4) / zfeed + Math.Abs(xstart4 - xend3) / xBfeed + Math.Abs(zstart4 - max_z - 30) / zfeed;
                        
                        Pmac.SendCommand("DWELL 0");
                    }


                    /*********************************************************************************************************************/


                    if (checkBox5.Checked == true)//程序段5
                    {
                        n = Convert.ToDouble(textBox62.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox64.Text), end_point = Convert.ToDouble(textBox63.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序5加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=5 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart5 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart5 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t5 = t5 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            xend5 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend5 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t5 = t5 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend5 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend5 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend5 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend5 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                            }
                        }
                        t5 = t5 * n + Math.Abs(max_z + 30 - zend4) / zfeed + Math.Abs(xstart5 - xend4) / xBfeed + Math.Abs(zstart5 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }


                    /*********************************************************************************************************************/


                    if (checkBox6.Checked == true)//程序段6
                    {
                        n = Convert.ToDouble(textBox65.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox67.Text), end_point = Convert.ToDouble(textBox66.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序6加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=6 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart6 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart6 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t6 = t6 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            } 
                            xend6 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend6 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t6 = t6 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend6 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend6 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend6 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend6 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                            }
                        }
                        t6 = t6 * n + Math.Abs(max_z + 30 - zend6) / zfeed + Math.Abs(xstart6 - xend5) / xBfeed + Math.Abs(zstart6 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

   /*********************************************************************************************************************/


                    if (checkBox7.Checked == true)//程序段7
                    {
                        n = Convert.ToDouble(textBox71.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox73.Text), end_point = Convert.ToDouble(textBox72.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序7加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=7 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart7 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart7 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t7 = t7 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            xend7 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                            zend7 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t7 = t7 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend7 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                            zend7 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }
                                xend7 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 0];
                                zend7 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10), 4];
                   
                            }
                        }
                        t7 = t7 * n + Math.Abs(max_z + 30 - zend6) / zfeed + Math.Abs(xstart7 - xend6) / xBfeed + Math.Abs(zstart7 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

                    /*********************************************************************************************************************/


                    if (checkBox8.Checked == true)//程序段8
                    {
                        n = Convert.ToDouble(textBox68.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox70.Text), end_point = Convert.ToDouble(textBox69.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序8加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=8 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox14.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart8 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 0];
                        zstart8 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 10); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t8 = t8 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; // Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2)
                                    t8 = t8 + Math.Sqrt(Math.Pow((data2[l, 0] - data2[l - 1, 0]), 2) + Math.Pow((data[l, 4] - data[l - 1, 4]), 2)) / data2[l, 3]; //Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 20)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 20)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }

                            }
                        }
                        t8 = t8 * n + Math.Abs(max_z + 30 - zend7) / zfeed + Math.Abs(xstart8 - xend7) / xBfeed + Math.Abs(zstart8 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }



                    Pmac.SendCommand("DWELL 0");
                    Pmac.SendCommand("P14=1");
                    Pmac.SendCommand("P32=1");
                    Pmac.SendCommand("M7=1");
                    Pmac.SendCommand("M113=0 M213=0");
                    Pmac.SendCommand("ENABLE PLC6");
                    Pmac.SendCommand("CLOSE");

                    t = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;//加工时间和
                }

 

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                button21.Text = "读取其它NC";
            }

  /*************************/
           // process_time = (t * n + Math.Abs(data2[2, 0] / data2[2, 3])) + (100 - Math.Abs(data[2, 4])) / 6 / 60 + 0.05;

            //if (radioButton21.Checked == true)//单段加工
            //    process_time = (t * n + Math.Abs(data2[2, 0] / data2[2, 3])) + (100 - Math.Abs(data[2, 4])) / 6 / 60 + 0.05;
            //else//多段加工
            //    process_time = t;
            //process_time = (t * n + Math.Abs(data2[2, 0] / data2[2, 3])) + (100 - Math.Abs(data[2, 4])) / 6 / 60 + 0.05;
           // process_time = t;
                 if (pbSuccess)
            {
               // Delay(3);

               // PMAC.Download(pmacNumber, result1, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载
                button21.Text = "读取成功";
                own_code_flag = false;
                compensate_code_flag = false;
                other_code_flag = true;
                //if (radioButton21.Checked == true)//单段加工
                //    process_time = (t * n + Math.Abs(data2[2, 0] / data2[2, 3])) + (100 - Math.Abs(data[2, 4])) / 6 / 60 + 0.05+caculate_time_offset;
                //else//多段加工
                    process_time = t+caculate_time_offset;
               // MessageBox.Show(((100 - Math.Abs(data[2, 4])) / 6 / 60).ToString());
                double time = process_time;
               // MessageBox.Show(time.ToString());
                hour = Convert.ToInt32((time / 60).ToString().Split(char.Parse("."))[0]); //time % 60;
                min = Convert.ToInt32((time % 60).ToString().Split(char.Parse("."))[0]);
                double sec = (time % 1) * 60;

                Sec = Convert.ToInt16(sec);
                //Sec = Convert.ToInt32((time.ToString().Split(char.Parse("."))[1])) * 6;
                textBox47.Text = Convert.ToString(hour.ToString() + "时" + min.ToString() + "分" + Sec.ToString() + "秒");
           
                //textBox47.Text = t.ToString("f1");
                download_code_flag = true;
                timer5.Enabled = true;
               // dectect.Start();
                timer3.Enabled = true;
            }
            else
            {
                download_code_flag = false;
                button21.Text = "读取失败";
                timer5.Enabled = true;
               // dectect.Start();
                timer3.Enabled = true;
            }
/************存参数到文件***************/

            string path = System.Environment.CurrentDirectory + "\\last_process_parameter.txt"; ;
            FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0); //清空txt文件
            stream.Close();
            FileStream file = new FileStream(path, FileMode.Append);
            StreamWriter w = null;
            w = new StreamWriter(file);
                w.WriteLine(textBox11.Text);//模仁名称
                w.WriteLine(textBox12.Text);//次数
                w.WriteLine(textBox14.Text);//抛光轴转速
                w.WriteLine(textBox13.Text);//工件高度
                w.WriteLine(textBox15.Text);//工件转速
                w.WriteLine(textBox20.Text);//速度倍率
                w.WriteLine(textBox19.Text);//速度倍率
                
            if(comboBox10.SelectedIndex==0)
                     w.WriteLine("0");//抛光顺逆
                 else
                     w.WriteLine("1");
                 if (comboBox11.SelectedIndex == 0)
                     w.WriteLine("0");//工件顺逆
                 else
                     w.WriteLine("1");
               // w.WriteLine(comboBox10.SelectedIndex);//抛光顺逆
                 w.WriteLine(textBox50.Text);//矢高
                w.Close();
/******************************************************************************************************************/



                    
              }
                

           
            catch (Exception )
            {
                button21.Text = "读取其它NC";
                MessageBox.Show("读文本文件发生错误！请检查源文件是否是要读入的文本文件或是加工次数和工件高度值或抛光轴速度没有输入？", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                return;
            }
            finally
            {


                if (MyReader != null)
                {
                    MyReader.Close();
                }
            }
            
        
           


        }

        private void button46_Click(object sender, EventArgs e)//执行，给P变量赋值。
        {
            if (CheckLicence())//判断秘钥是否失效
            {
              
            }
            else
            {
                MessageBox.Show("软件密钥失效，请联系厂商!");
                return;

            }


            if (MessageBox.Show("是否开始加工？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            
           {
               int door_status = Pmac.GetM(11);//门开关状态

               if (checkBox9.Checked == true&&door_status==0)
               {
                   MessageBox.Show("安全门功能有效，请先关闭门！", "提示！");
                   return;
                   
               }
               int LUN_ALARM = Pmac.GetM(50);//手轮急停警报
               if (LUN_ALARM == 0  && connect_flag == true)
               {
                   MessageBox.Show("请先松开手轮急停按钮！", "提示！");
                       return;
               }
                if (download_code_flag == true)
                {
                    button46.Enabled = false;//加工
                    button28.Enabled = false;//手动
                    button21.Enabled = false;//读取其它NC
                    button12.Enabled = false;//下载标准代码
                    button47.Enabled = false;//下载补偿代码
                    button21.Text = "读取其它NC";
                    string status;
                    PMAC.GetResponse(pmacNumber, "ENABLE PLC 2", out status);
                    Delay(1);

                    PMAC.GetResponse(pmacNumber, "DISABLE PLC 11", out status);
                    PMAC.GetResponse(pmacNumber, "DISABLE PLC 12", out status);
                    PMAC.GetResponse(pmacNumber, "DISABLE PLC 13", out status);
         
                    PMAC.GetResponse(pmacNumber, "P21=1", out status);
                    Pmac.reset_P();//重置P变量确保P处于初始值。
                    process_begin_flag = true;
                    //int hour, min, Sec, h, m, s;
                    double time = process_time;//Convert.ToDouble(textBox47.Text);
                    hour = Convert.ToInt32((time / 60).ToString().Split(char.Parse("."))[0]);
                    min = Convert.ToInt32((time % 60).ToString().Split(char.Parse("."))[0]);
                    double sec = (time % 1) * 60;
                    this.progressBar1.Maximum =Convert.ToInt32(process_time*60);
                    process_fininsh_flag = false;
                    Sec = Convert.ToInt16(sec);
                    //Sec = Convert.ToInt32((time.ToString().Split(char.Parse("."))[1])) * 6;
                    //TimeSpan dtTo = new TimeSpan(hour,min,Sec); //设置开始时间
                   // timer10.Enabled = false;//加工监控
                    timer6.Enabled = true; //加工计时
                   // timer9.Enabled = true;//不加工时监控
                    Pmac.SetM(46, 1);//绿灯亮

                    if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SetM(1,0);

                    }
                    else
                    {
                        Pmac.SetM(1, 1);
                    }
                   // timer7.Enabled = true;
                    //process_time = Convert.ToDouble(textBox47.Text);
                }
                else
                {
                    MessageBox.Show("请先设定好参数");
                }
            }
            else
                return;
           
            
        }
        

        private void button41_Click(object sender, EventArgs e)//中止运动程序，给P变量赋值，并发在线指令“A”
        {
            string status;
            if(download_code_flag==true)
            PMAC.GetResponse(pmacNumber, "&1A", out status);
            Delay(1);
           // if(process_fininsh_flag==true)
            if (process_begin_flag == true)
            {
                PMAC.GetResponse(pmacNumber, "P15=1", out status);
                process_begin_flag = false;
            }
            Delay(1);

  
            PMAC.GetResponse(pmacNumber, "P15=0", out status);
            PMAC.GetResponse(pmacNumber, "M113=0", out status);
            PMAC.GetResponse(pmacNumber, "M213=0", out status);
            button46.Enabled = true;//加工
            button28.Enabled = true;//手动
            button21.Enabled = true;//读取
            button12.Enabled = true;//下载标准代码
            button47.Enabled = true;//下载补偿代码
          
            timer6.Enabled = false;//加工倒计时
          //  timer9.Enabled = false;//加工监测时钟停
            Pmac.SetM(46, 0);//绿灯灭
           // timer10.Enabled = true;
            textBox47.Text = Convert.ToString("0时" + "0分" + "0秒");
            textBox45.Text = null;
            this.progressBar1.Value = 0;
        }

        private void timer6_Tick(object sender, EventArgs e)//加工时间倒计时用
        {
          /*  timer6.Interval = 600;
            double pos1, pos2;
            Pmac.GetPos(1, out pos1);
            Pmac.GetPos(2, out pos2);
            textBox41.Text = Convert.ToString((pos1 / 10000).ToString("f4"));//x轴机械坐标
            textBox42.Text = Convert.ToString((pos2 / 5000).ToString("f4"));//B轴机械坐标
            textBox44.Text = Convert.ToString((pos1 / 10000).ToString("f4"));//x轴相对坐标
            textBox43.Text = Convert.ToString((pos2 / 5000).ToString("f4"));//B轴相对坐标
            if (radioButton13.Checked == true)
            {
                textBox12.En
           * abled = false;
                textBox13.Enabled = false;
            }
            if (radioButton13.Checked == false)
            {
                textBox12.Enabled = true;
                textBox13.Enabled = true;
            }*/
            timer6.Interval = 993;


          
          //  int hour, min, Sec, h, m, s;
            
            h = hour;
            m = min;
            s = Sec;


            if (h == 0 && m == 0 && s == 1)
            {
                timer6.Enabled = false;
                textBox47.Text = Convert.ToString("0时" + "0分" + "0秒");
               // textBox45.Text = Convert.ToString("0时" + "0分" + "0秒");
                button21.Text = "读取其它NC";
                //  this.progressBar1.Value = 0;
            }

             if(hour>=0)
            {
               
                if (min >= 0)
                {
                    if (Sec >= 0)
                    {

                        Sec--;
                    
                        if (Sec == -1)
                        {
                            
                            if(min>0)
                            min--;
                            m = min;
                            if (min == 0)
                            {
                                if (hour > 0)
                                {
                                    hour--;
                                    h = hour;
                                    min = 59;
                                    m = min;
                                }
                            }

                            // label1.Text = mint.ToString() + "分";

                            Sec = 59;

                        }
                        
                        s = Sec;
                    }

                    //  label2.Text = scss.ToString() + "秒";

                }
            }
             this.progressBar1.Value = progressBar1.Value + 1;
            double P26=Pmac.GetP(26),P27=Pmac.GetP(27);//加工次数和程序段


            textBox47.Text = Convert.ToString(h.ToString()+"时"+m.ToString()+"分"+s.ToString()+"秒");
            textBox45.Text = Convert.ToString( "程序" + P26.ToString() + "第" + P27.ToString() + "回");

//安安全门
            int door_status = Pmac.GetM(11);
            if (checkBox9.Checked == true && door_status == 0 && process_begin_flag==true)
            {
                string status;
                if (download_code_flag == true)
                    PMAC.GetResponse(pmacNumber, "&1A", out status);
                Pmac.SetM(302, 0);//C轴停
                Pmac.SetM(502, 0);//抛光轴停
                
                // if(process_fininsh_flag==true)

                process_begin_flag = false;
                if (MessageBox.Show("门在加工过程打开，加工中止\n 是否运动回零点？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PMAC.GetResponse(pmacNumber, "P15=1", out status);
                    process_begin_flag = false;
                }
                else
                {
                    PMAC.GetResponse(pmacNumber, "DISABLE PLC3", out status);
 
                }
                
                //if (process_begin_flag == true)
                //{
                //    PMAC.GetResponse(pmacNumber, "P15=1", out status);
                //    process_begin_flag = false;
                //}
                Delay(1);


                PMAC.GetResponse(pmacNumber, "P15=0", out status);
                PMAC.GetResponse(pmacNumber, "M113=0", out status);
                PMAC.GetResponse(pmacNumber, "M213=0", out status);
                button46.Enabled = true;//加工
                button28.Enabled = true;//手动
                button21.Enabled = true;//读取
                button12.Enabled = true;//下载标准代码
                button47.Enabled = true;//下载补偿代码

                timer6.Enabled = false;//加工倒计时
                //  timer9.Enabled = false;//加工监测时钟停
                Pmac.SetM(46, 0);//绿灯灭
                // timer10.Enabled = true;
                textBox47.Text = Convert.ToString("0时" + "0分" + "0秒");
                textBox45.Text = null;
                this.progressBar1.Value = 0;
            }
            
         

        }

        private void button19_Click_1(object sender, EventArgs e)//形状参数设置界面，应用按钮
        {
            try
            {
                this.draw();

                parameter_shape = new double[26] { Convert.ToDouble(yuanr_text.Text), Convert.ToDouble(gongD_textBox3.Text), Convert.ToDouble(SAG_text.Text), Convert.ToDouble(TextBox35.Text), Convert.ToDouble(R_text.Text), Convert.ToDouble(K_text.Text), Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };

            }
            catch (Exception )
            {
                MessageBox.Show("参数设定发生错误！请检查参数是否设定。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                string path = System.Environment.CurrentDirectory + "\\last_shape_parameter.txt"; ;
            FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            for (int i = 0; i < 26; i++)
            {
                wr.WriteLine(Convert.ToString(parameter_shape[i]));
            }
            if (ao_non_sphere.Checked == true)
                wr.WriteLine("1");
            else if (tu_non_sphere.Checked == true)
                wr.WriteLine("2");
            else if (ao_sphere.Checked == true)
                wr.WriteLine("3");
            else if(tu_sphere.Checked == true)
                wr.WriteLine("4");
            else if (plane.Checked == true)
                wr.WriteLine("5");
            wr.WriteLine(textBox1.Text);
            //textBox46.Text = myreader.ReadLine().Trim();//工件直径
            wr.WriteLine(textBox46.Text);
            wr.Close();
            //this.draw();


            this.button19.Enabled = false; //应用按钮变灰
            
        }

        private void button10_Click(object sender, EventArgs e)//加工参数设置界面，应用按钮
        {
            double[] Parameter = new double[17];
            try
            {

                Parameter = new double[17] {
                    Convert.ToDouble(textBox2.Text),//转速
                    Convert.ToDouble(textBox3.Text), //最大转速
                    Convert.ToDouble(textBox5.Text), //进给速度
                    Convert.ToDouble(textBox4.Text),//速度倍率
                    Convert.ToDouble(textBox33.Text), //抛光轴转速
                    
                    Convert.ToDouble(comboBox12.SelectedIndex), //研磨布规格
                    Convert.ToDouble(textBox35_1.Text), //工件高度
                    Convert.ToDouble(textBox6.Text), //加工范围左位置
                    Convert.ToDouble(textBox18.Text), //加工范围右位置
                    Convert.ToDouble(textBox37.Text), //荷重
                    
                    Convert.ToDouble(textBox39.Text),//往复回数
                  //  Convert.ToDouble(textBox18.Text),//加工内径
                Convert.ToDouble(comboBox4.SelectedIndex),//数据间隔
                Convert.ToDouble(comboBox5.SelectedIndex),//磨头直径
                Convert.ToDouble(comboBox8.SelectedIndex),//抛光轴方向
                Convert.ToDouble(comboBox9.SelectedIndex),//工件方向
                 Convert.ToDouble(comboBox1.SelectedIndex),//模仁材质
                Convert.ToDouble(comboBox3.SelectedIndex)//研磨颗粒规格
                };

            }
            catch (Exception)
            {
                MessageBox.Show("参数设定发生错误！请检查加工参数是否设定!", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string filePath = System.Environment.CurrentDirectory + "\\last_process_paramenters.txt"; ;
                FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0); //清空txt文件
                stream.Close();

                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                for (int i = 0; i < 17; i++)
                {
                    wr.WriteLine(Convert.ToString(Parameter[i]));
                }
                if (radioButton1.Checked == true)//工件转速
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton4.Checked == true)//进给速度
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton7.Checked == true)//研磨角度
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                if (radioButton23.Checked == true)//单段加工
                    wr.WriteLine("1");
                else
                    wr.WriteLine("0");
                wr.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }

            //  this.button36.Size = new System.Drawing.Size(50, 34);
            this.button10.Enabled = false; //应用按钮变灰
        }


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

        private void textBox35_TextChanged(object sender, EventArgs e)//更改加工口径时，应用按钮正常。
        {
            button19.Enabled = true;
        }

        private void button11_Click_1(object sender, EventArgs e)//生成代码
        {
            Produce_x_b_F_C NC = new Produce_x_b_F_C();
            double curvature_compensate, n, R, K, dist, t = 0, vc, H, R_left, SAG, yuan_r, ao_tu, R_right, constan_vc, constan_F, symbol = 1, tool_r, Dp, D_workpiece;
            //double symbol;
           // double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };

            richTextBox1.Clear();

            //A = new double[24] { Convert.ToDouble(H_text.Text), Convert.ToDouble(D_text.Text), Convert.ToDouble(R_text.Text), Convert.ToDouble(K_text.Text), Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
            // if (textBox35.Text != null || textBox39 == null)
            //  {
            //     MessageBox.Show("请输入工件转速");
            //  }
            //  else if (textBox39 != null || textBox35.Text == null)
            //  {
            //      MessageBox.Show("请输入加工次数");
            //  }
            /// if (textBox35.Text != null) //|| textBox39 == null)
            //{
            //    MessageBox.Show("请输入工件转速和加工次数");
            // }
            // else
            //{
            double[] arry= new double [16];

            if (textBox40.Text != "")
                arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
            else
                arry[0] = 0;

            if (textBox51.Text != "")
                arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
            else
                arry[1] = 0;
            if (textBox55.Text != "")
                arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
            else
                arry[2] = 0;
            if (textBox54.Text != "")
                arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
            else
                arry[3] = 0;
            if (textBox58.Text != "")
                arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
            else
                arry[4] = 0;
            if (textBox57.Text != "")
                arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
            else
                arry[5] = 0;
            if (textBox61.Text != "")
                arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
            else
                arry[6] = 0;
            if (textBox60.Text != "")
                arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
            else
                arry[7] = 0;
            if (textBox64.Text != "")
                arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
            else
                arry[8] = 0;
            if (textBox63.Text != "")
                arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
            else
                arry[9] = 0;
            if (textBox67.Text != "")
                arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
            else
                arry[10] = 0;
            if (textBox66.Text != "")
                arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
            else
                arry[11] = 0;
            if (textBox73.Text != "")
                arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
            else
                arry[12] = 0;
            if (textBox72.Text != "")
                arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
            else
                arry[13] = 0;
            if (textBox70.Text != "")
                arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
            else
                arry[14] = 0;
            if (textBox69.Text != "")
                arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
            else
                arry[15] = 0;
           

            Array.Sort(arry);

           try 
            {

            double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };

            n = Convert.ToDouble(textBox16.Text);//加工次数
            R = Convert.ToDouble(R_text.Text);  //面型参数R
            K = Convert.ToDouble(K_text.Text);//面型参数K
            vc = Convert.ToDouble(textBox3.Text);//C轴最大转速
            H = Convert.ToDouble(TextBox35.Text);//模仁高度
           
            SAG = Convert.ToDouble(SAG_text.Text);//非球面中心到平面距离
          //  fixture_h = Convert.ToDouble(yuanr_text.Text.Trim());//夹具高度，从移动调心夹具底面到模仁底面距离
            yuan_r = Convert.ToDouble(yuanr_text.Text.Trim());//圆角半径
            if (yuan_r < 0.1)
                yuan_r = 0.1;
             constan_vc = Convert.ToDouble(textBox2.Text);//恒定C转速
            constan_F = Convert.ToDouble(textBox5.Text);//恒定进给

            Dp = Convert.ToDouble(gongD_textBox3.Text.Trim());//工件加工口径
            D_workpiece = Convert.ToDouble(textBox46.Text.Trim());

            if (comboBox4.SelectedIndex == 0)

                dist = 0.1;
            else if (comboBox4.SelectedIndex == 1)
                dist = 0.01;
            else
                dist = 0.1;

            if (D_workpiece * 100 % 2 == 1)
                D_workpiece = D_workpiece - dist;

            if (radioButton23.Checked == true)//单段加工
            {
                if (textBox6.Text == "")
                    R_left = 0;
                else
                    R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

                if (textBox18.Text == "")
                    R_right = 0;
                else
                    R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值
                // textBox18.text
                if (R_left >= R_right)
                { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }
         


            }
            else
            {
                R_left = -D_workpiece/2;
                R_right = D_workpiece/2;
                if(arry[15]>R_right)
                { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }   
 
            }
           


                if(Math.Abs(R_left)>D_workpiece/2)
                {
                      MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                        return;
                }

                if (Math.Abs(R_right) > D_workpiece / 2)
                { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }
               
               
               //  if()
            if (ao_non_sphere.Checked == true)//凹凸判断
                ao_tu = -1;
            else
                ao_tu = 1;
          

            if (comboBox5.SelectedIndex == 0)
                tool_r = 1;
            else if (comboBox5.SelectedIndex == 1)
                tool_r = 3;
            else
                tool_r = 5;

            if (radioButton7.Checked == true)//垂直抛
            {
                tool_r = 7;
            }


            if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
            {
                if (ao_non_sphere.Checked == true)
                {
                    if (R > 0)
                        symbol = 1;
                    else
                        symbol = -1;

                }
                if (tu_non_sphere.Checked == true)
                {
                    if (R > 0)
                        symbol = -1;
                    else
                        symbol = 1;
                }
                if (comboBox13.SelectedIndex == 1)
                    curvature_compensate = 1;
                else if (comboBox13.SelectedIndex == 2)
                    curvature_compensate = 2;
                else
                    curvature_compensate = 0;

                //textBox47.Text = Convert.ToString(symbol);//看看symbol对不对
                //if(radioButton14.Checked==true)
                //     NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

                //else if(radioButton16.Checked==true)
                //    NC_Data = NC.asphere_heitian2(SAG, yuan_r, ao_tu, D_end, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码
           
             //   else
              //NC_Data = NC.asphere_heitian(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
                 if (comboBox4.SelectedIndex == 0)
                    NC_Data = NC.asphere_heitian(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码               
                else
                     NC_Data = NC.asphere_heitian_dist(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
              

               // textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
            }
            if (ao_sphere.Checked == true || tu_sphere.Checked == true)
            {
                if (ao_sphere.Checked == true)
                    symbol = 1;
                if (tu_sphere.Checked == true)
                    symbol = -1;
                NC_Data = NC.sphere(first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
                //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
          
               
            }
            if (plane.Checked == true)
            {
                if (radioButton23.Checked == true)
                {
                    if (textBox6.Text != "")
                        R_left = Convert.ToDouble(textBox6.Text);//加工口径
                    else
                    {
                        MessageBox.Show("请设定加工范围！");
                        return;
                    }

                    if (textBox18.Text != "")
                        R_right = Convert.ToDouble(textBox18.Text);//加工口径
                    else
                    {
                        MessageBox.Show("请设定加工范围！");
                        return;
                    }
                }

                if (Math.Abs(R_left) < arry[15])
                    R_left = arry[15] ;
                //if (radioButton14.Checked == true)
                //    NC_Data = NC.plane(C_motor_scale_factor, C_motor_offset, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码              
                //     else
                NC_Data = NC.plane_heitian(first_position_feed, D_workpiece, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
               
                //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
          
            }
            }
           catch (Exception)
           {
               MessageBox.Show("参数设定发生错误！请检查加工参数和形状参数是否设定。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
           }
            /***********************************写入xtbox************************************************/
          

            
            bool x_limit_flag = false;
            double time = 0;
            int index = 0;
            if (radioButton23.Checked == true) //单段加工
            {
                for (int i = 0; i < NC_Data.Length / 7; i++)
                {
                    if (i == 0)
                        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4")+ "  " + "\n"));// + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2]).ToString("f4") + "  " + "\n"));
                  
                    else
                 
                        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4")+ "  " + "\n"));// + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
                  
                    
                    
                    if (NC_Data[i, 0] > xlimit)
                        x_limit_flag = true;
                    // wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));

                    if (checkBox10.Checked == true)
                    {
                        if (input_compesentfile_flag == true)
                        {
                            if (i == 0)
                                NC_Data[i, 2] = NC_Data[i, 2];
                            else
                            // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                            {

                                if(NC_Data[i, 4]<0.01)
                                index = Convert.ToInt32(Math.Abs(NC_Data[i, 4])*10);
                                else
                                    index = Convert.ToInt32(Math.Abs(NC_Data[i, 4]) * 10)-1;

                                NC_Data[i, 2] = Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) * NC_Data[i, 2] / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0]) * Convert.ToDouble(textBox4.Text) * 0.01 * Compesent_rate[index];//进给速度X Z联动
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先导入补偿文件！");
                            return;
                        }
                    }
                    else
                    {

                        if (i == 0)
                            NC_Data[i, 2] = NC_Data[i, 2];
                        else
                            // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动

                            NC_Data[i, 2] = Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) * NC_Data[i, 2] / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0]) * Convert.ToDouble(textBox4.Text) * 0.01;//进给速度X Z联动
                    }

                 //   if (i > 0)
                  //      time = time + Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2)) / NC_Data[i, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                     


                } 


            }

            else
            {

                for (int i = (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2 / dist)) / 2 + 1; i <= (NC_Data.Length / 7 - 3) / 2 + Convert.ToInt16(arry[15]  / dist) + 1; i++)
                {

                  //  Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));

                    if (i == (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2 + 1)
                        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4")+ "  " + "\n"));// + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[0, 2]).ToString("f4") + "  " + "\n"));
                    else
                        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "\n"));//+ "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
                    if (NC_Data[i, 0] > xlimit)
                        x_limit_flag = true;
                    //  NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动

                    if (checkBox10.Checked == true)
                    {
                        if (input_compesentfile_flag == true)
                        {
                            if (i == 0)
                                NC_Data[i, 2] = NC_Data[i, 2];
                            else
                            // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                            {
                                if (NC_Data[i, 4] < 0.01)
                                    index = Convert.ToInt32(Math.Abs(NC_Data[i, 4]) * 10);
                                else
                                    index = Convert.ToInt32(Math.Abs(NC_Data[i, 4]) * 10) - 1;

                                NC_Data[i, 2] = Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) * NC_Data[i, 2] / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0]) * Convert.ToDouble(textBox4.Text) * 0.01 * Compesent_rate[index];//进给速度X Z联动
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先导入补偿文件！");
                            return;
                        }
                    }
                    else
                    {
                        if (i == 0)
                            NC_Data[i, 2] = NC_Data[i, 2];
                        else
                            // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动

                            NC_Data[i, 2] = Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) * NC_Data[i, 2] / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0]) * Convert.ToDouble(textBox4.Text) * 0.01;//进给速度X Z联动
                    }
                }
                //for (int i = 0; i < NC_Data.Length / 7; i++)
                //{
                //    if (i == 0)
                //        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2]).ToString("f4") + "  " + "\n"));
                //    else
                //        richTextBox1.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
                //    if (NC_Data[i, 0] > xlimit)
                //        x_limit_flag = true;
                //    // wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                //}
            }
          
           
            
            


            if (x_limit_flag == true)
                MessageBox.Show("加工代码超出X轴行程");

            this.button10.Enabled = true;//应用按钮正常
        }

        private void button12_Click_1(object sender, EventArgs e)//导入代码到控制器
        {
            #region
            double dist;//间距
            if (comboBox4.SelectedIndex == 0)

                dist = 0.1;
            else if (comboBox4.SelectedIndex == 1)
                dist = 0.01;
            else
                dist = 0.1;

            if (radioButton23.Checked == true)//单段加工
            {
                button12.Text = "导入中...";
                //string pathh = System.Environment.CurrentDirectory + "\\模仁加工代码\\richtexbox.txt";

                //FileStream stream = File.Open(pathh, FileMode.OpenOrCreate, FileAccess.Write);
                //stream.Seek(0, SeekOrigin.Begin);
                //stream.SetLength(0); //清空txt文件
                //stream.Close();

                //richTextBox1.SaveFile(pathh, RichTextBoxStreamType.PlainText);  //richtexbox
                //Delay(1);

                /*****richtextboxd读取数据****/
                //string[]  word1 =new string [2000];
                //int ii=0;
                //foreach (string word in richTextBox1.Lines)
                //{
                //    word1[ii] = word;
                //    ii++;

                //}
                /***************************把richbox修改过的替换掉******最初身生成的**************/
                // StreamReader _rstream = null;//为richtext所创建的流
                //// string line;
                string result = "";
                int j = 0;
                // _rstream = new StreamReader(pathh, System.Text.Encoding.UTF8);

                try
                {

                    // while ((line = _rstream.ReadLine()) != null)
                    foreach (string line in richTextBox1.Lines)
                    {


                        if (j >= 0 & j < NC_Data.Length / 7)
                        {

                            result = line;
                            int x = result.IndexOf("X");

                            int b = result.IndexOf("B");
                            int z = result.IndexOf("Z");
                         //      int f = result.IndexOf("F");
                            string str1 = result.Substring(x + 1, b - x - 1);
                            string str3 = result.Substring(b + 1, z - b - 1);
                            string str33 = result.Substring(z + 1);
                           //  string str33 = result.Substring(z + 1, f - z - 1);
                        //     string str4 = result.Substring(f + 1);

                            //NC_Data[j, 0] = Convert.ToDouble(str1.Trim());
                            NC_Data[j, 1] = Convert.ToDouble(str3.Trim());
        //                    if (j == 0)
        //                        NC_Data[j, 2] = NC_Data[j, 2];
        //                    else
        //                       // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                  
        //                        NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) *NC_Data[j,2]/Math.Abs(NC_Data[j, 0] - NC_Data[j - 1, 0])*Convert.ToDouble(textBox4.Text) * 0.01;//进给速度X Z联动
        ////+ "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2]).ToString("f4") + "  " + "\n"));
                            //"  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * NC_Data[i, 2] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
            
                            NC_Data[j, 5] = Convert.ToDouble(str33.Trim());

                            // break;
                        }

                        j++;


                    }
                    //   _rstream.Close();
                }
                catch
                {
                    MessageBox.Show("导入失败！请勿删除代码", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button12.Text = "导入非补正代码";
                    //  _rstream.Close();
                    return;
                }


                double t = 0;//加工一遍的时间
                /*************************************生成代码**********************************************/

                /*    string path = System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.txt";

           
                    //*先清空result1.txt文件内容
                    FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                    stream2.Seek(0, SeekOrigin.Begin);
                    stream2.SetLength(0); //清空txt文件
                    stream2.Close();


                    FileStream fs = new FileStream(path, FileMode.Append);
                    // fs.Seek(0, SeekOrigin.Begin);
                    // fs.SetLength(0);
                    StreamWriter wr = null;
                    double n;
                    try
                    {
                         n = Convert.ToDouble(textBox16.Text);//加工次数
                    }
                    catch (Exception Err)
                    {
                        MessageBox.Show("导入失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button12.Text = "导入标准代码";
                        return;
                    }
                    wr = new StreamWriter(fs);
                    if (radioButton20.Checked == true)
                    {
                        string status;
                        PMAC.GetResponse(pmacNumber,"P25="+Convert.ToString(n),out  status);
                        wr.WriteLine("OPEN PROG 2");
                        wr.WriteLine("CLEAR");
                        wr.WriteLine("G90 G01 P32=0 M402==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * 68.1 + 2574.7));
                        if (comboBox8.SelectedIndex == 0)//工件转动方向
                        {
                            wr.WriteLine("M3==1");
                            wr.WriteLine("M4==0");
                        }
                        else
                        {
                            wr.WriteLine("M3==0");
                            wr.WriteLine("M4==1");
                        }
                        if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                        {
                            wr.WriteLine("M1==0");
                    
                        }
                        else
                        {
                            wr.WriteLine("M1==1");                   
                        }
                        //wr.WriteLine("P25=0");
                       // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        for (int i = 0; i < NC_Data.Length / 5; i++)
                        {
                            wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                            if (i > 0)
                                t = t + (Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                        }
                        wr.WriteLine("P25=P25-1");
                       // wr.WriteLine("ENDWHILE");
                        wr.WriteLine("DWELL 0");
                        wr.WriteLine("IF(P25=0)");
                        wr.WriteLine("P32=1");
                        wr.WriteLine("P14=1");
                        wr.WriteLine("ELSE");
                        wr.WriteLine("COMMAND \"&1B1R\"");
                        wr.WriteLine("M5=0"); 
                        wr.WriteLine("ENDIF");
                
                        wr.WriteLine("CLOSE");
                        wr.Close();



                    }
                    else
                    {
                        if (n == 1)
                        {
                            wr.WriteLine("OPEN PROG 2");
                            wr.WriteLine("CLEAR");
                            wr.WriteLine("G90 G01 P25=0 P32=0 M402==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * 68.1 + 2574.7));
                            //wr.WriteLine("P25=0");
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                wr.WriteLine("M3==1");
                                wr.WriteLine("M4==0");
                            }
                            else
                            {
                                wr.WriteLine("M3==0");
                                wr.WriteLine("M4==1");
                            }
                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                wr.WriteLine("M1==0");

                            }
                            else
                            {
                                wr.WriteLine("M1==1");
                            }
                            wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int i = 0; i < NC_Data.Length / 5; i++)
                            {
                                wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + (Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                            }
                            wr.WriteLine("P25=P25+1");
                            wr.WriteLine("ENDWHILE");
                            wr.WriteLine("DWELL 0");
                            wr.WriteLine("P14=1");
                            wr.WriteLine("P32=1");
                            wr.WriteLine("CLOSE");
                            wr.Close();

                        }
                        else
                        {
                            wr.WriteLine("OPEN PROG 2");
                            wr.WriteLine("CLEAR");
                            wr.WriteLine("G90 G01 P25=0 P32=0 M402==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * 68.1 + 2574.7));
                            //wr.WriteLine("P25=0");
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                wr.WriteLine("M3==1");
                                wr.WriteLine("M4==0");
                            }
                            else
                            {
                                wr.WriteLine("M3==0");
                                wr.WriteLine("M4==1");
                            }
                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                wr.WriteLine("M1==0");

                            }
                            else
                            {
                                wr.WriteLine("M1==1");
                            }
                            if (n % 2 == 1)
                                wr.WriteLine("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                wr.WriteLine("WHILE(P25<" + Convert.ToString(n / 2) + ")");

                            for (int i = 0; i < NC_Data.Length / 5; i++)
                            {

                                wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + (Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                            }
                            for (int i = NC_Data.Length / 5 - 2; i >= 0; i--)//代码倒转
                            {
                                wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i + 1, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                            }
                            wr.WriteLine("P25=P25+1");
                            wr.WriteLine("ENDWHILE");
                            if (n % 2 == 1)
                            {
                                for (int i = 0; i < NC_Data.Length / 5; i++)
                                {
                                    wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                                }

                            }
                            wr.WriteLine("DWELL 0");
                            wr.WriteLine("P14=1");
                            wr.WriteLine("P32=1");
                            wr.WriteLine("CLOSE");
                            wr.Close();

                       }
            
                
                    }
            
                /**********************生成第一个位置代码**********************************************/
                /*   string path2 = System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.txt";


                   //*先清空result1.txt文件内容
                   FileStream stream3 = File.Open(path2, FileMode.OpenOrCreate, FileAccess.Write);
                   stream3.Seek(0, SeekOrigin.Begin);
                   stream3.SetLength(0); //清空txt文件
                   stream3.Close();


                   FileStream fss = new FileStream(path2, FileMode.Append);
                   // fs.Seek(0, SeekOrigin.Begin);
                   // fs.SetLength(0);
                   StreamWriter wrr = null;
                  // double n = Convert.ToDouble(textBox39.Text);//加工次数
                   wrr = new StreamWriter(fss);
                   if (radioButton20.Checked==true)
                   {
                       wrr.WriteLine("OPEN PROG 1");
                       wrr.WriteLine("CLEAR");
                       // wr.WriteLine("S150");
                       wrr.WriteLine("G90 G01");
                       // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");

                       wrr.WriteLine(Convert.ToString("X" + NC_Data[0, 0] + "  " + "B" + NC_Data[0, 1] + "  " + "F100" + "  " + "\n"));
                       //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                       if (NC_Data[0, 0] > 0)
                           wrr.WriteLine("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0])).ToString("f4") + "-M162/32/96)>100" + ")");
                       else
                           wrr.WriteLine("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0])).ToString("f4") + "-M162/32/96)<-100" + ")");
                       wrr.WriteLine("P10=0");
                       wrr.WriteLine("DWELL 10");
                       wrr.WriteLine("ENDWHILE");

                     //  wrr.WriteLine("WHILE(M11==0)" );              
                   //    wrr.WriteLine("ENDWHILE");
                      // wrr.WriteLine("M5=1");

                       wrr.WriteLine("IF(P25="+Convert.ToString(n)+")");
                       wrr.WriteLine("P10=1");               
                       wrr.WriteLine("ELSE");
                       wrr.WriteLine("M5=1");
                       wrr.WriteLine("DWELL 3000");
                       wrr.WriteLine("COMMAND \"&1B2R\"");                              
                       wrr.WriteLine("ENDIF");
                       wrr.WriteLine("CLOSE");

                   }
                   else
                   {
                       wrr.WriteLine("OPEN PROG 1");
                       wrr.WriteLine("CLEAR");
                       // wr.WriteLine("S150");
                       wrr.WriteLine("G90 G01");
                       // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");

                       wrr.WriteLine(Convert.ToString("X" + NC_Data[0, 0] + "  " + "B" + NC_Data[0, 1] + "  " + "F100" + "  " + "\n"));
                       //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                       if (NC_Data[0, 0] > 0)
                           wrr.WriteLine("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0])).ToString("f4") + "-M162/32/96)>100" + ")");
                       else
                           wrr.WriteLine("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0])).ToString("f4") + "-M162/32/96)<-100" + ")");
                       wrr.WriteLine("P10=0");
                       wrr.WriteLine("DWELL 10");
                       wrr.WriteLine("ENDWHILE");
                       wrr.WriteLine("P10=1");
                       wrr.WriteLine("CLOSE");

                   }

                   wrr.Close();

                   process_time = (t * n+ Math.Abs(NC_Data[0, 0]) / 100)/2+0.15;//加工总时间
       /*****************************下载到控制器**************/
                /*    bool pbSuccess;
                    bool bMacro = true;
                    bool bMap = true;
                    bool bLog = true;
                    bool bDnld = true;
                   timer5.Enabled = false;
                  //  dectect.Abort();

                   try 
                   { 

                    PMAC.Open(pmacNumber, out pbSuccess);
                    PMAC.Download(pmacNumber, path, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载

                   }
                   catch (Exception Err)
                   {
                       MessageBox.Show("导入失败！线程安全冲突，请重启软件", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       return;
                   }

                    if (pbSuccess)
                    {
                        Delay(2);
                        PMAC.Download(pmacNumber, path2, bMacro, bMap, bLog, bDnld, out pbSuccess);//下载
                        button12.Text = "导入成功";
                        own_code_flag = true;
                        compensate_code_flag = false;
                        other_code_flag = false;
                        download_code_flag = true;
                       // Delay(1);
                       timer5.Enabled = true;
                       process_begin_flag = false;
                       MessageBox.Show("加工时长：" + process_time.ToString("f2") + "分钟");
                       // dectect.Start();
                    }
                    else
                    {
                        download_code_flag = false;
                        button12.Text = "导入失败";
                        timer5.Enabled = true;
                        //dectect.Start();
                    }

                    File.Delete(pathh);
                    File.Delete(path);
                    File.Delete(path2);
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.PMA");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.PMA");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.TBL");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.TBL");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.56K");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.56K");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.MAP");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.MAP");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download1.LOG");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\download2.LOG");
                    File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\Gcode_primary.txt");

                    /*******************************在线下载测试*******************************************/

                // File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\Gcode_primary.txt");
                //   File.Delete(System.Environment.CurrentDirectory + "\\模仁加工代码\\richtexbox.txt");
                /***************第一个位置代码**************/

                double n;

                try
                {
                    n = Convert.ToDouble(textBox16.Text);//加工次数
                }
                catch (Exception err)
                {
                    //MessageBox.Show("导入失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(err.Message);
                    button12.Text = "导入非补正代码";
                    return;
                }

                try
                {
                    if (NC_Data[0, 5] < min_z)
                    {
                        MessageBox.Show("Z轴行程不足，无法加工！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button12.Text = "导入非补正代码";
                        return;
                    }
                    if (radioButton20.Checked == true)
                    {
                        Pmac.SendCommand("OPEN PROG 1");
                        Pmac.SendCommand("CLEAR");
                        // wr.WriteLine("S150");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=1");
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P50=" + Convert.ToString(NC_Data[0, 5]));
                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0, 0].ToString("f4") + "  " + "B" + NC_Data[0, 1].ToString("f4") + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0, 4] * 1 / dist).ToString("f4") + "  " + "M213==" + (NC_Data[0, 6] * 1000).ToString("f4") + "  " + "\n"));
                        //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                        if (NC_Data[0, 0] > 0)
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0]) ).ToString("f4") + "-M162/32/96)>100" + ")");
                        else
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0]) ).ToString("f4") + "-M162/32/96)<-100" + ")");
                        Pmac.SendCommand("P10=0");
                        Pmac.SendCommand("DWELL 10");
                        Pmac.SendCommand("ENDWHILE");

                        //  wrr.WriteLine("WHILE(M11==0)" );              
                        //    wrr.WriteLine("ENDWHILE");
                        // wrr.WriteLine("M5=1");

                        Pmac.SendCommand("IF(P25=" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P10=1");
                        Pmac.SendCommand("ELSE");
                        Pmac.SendCommand("M5=1");
                        Pmac.SendCommand("DWELL 3000");
                        Pmac.SendCommand("COMMAND \"&1B2R\"");
                        Pmac.SendCommand("ENDIF");
                        Pmac.SendCommand("CLOSE");

                    }
                    else
                    {
                        Pmac.SendCommand("OPEN PROG 1");
                        Pmac.SendCommand("CLEAR");
                        // wr.WriteLine("S150");
                        Pmac.SendCommand("DISABLE PLC6");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=1");
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P50=" + Convert.ToString(NC_Data[0, 5]));

                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0, 0].ToString("f4") + "  " + "B" + NC_Data[0, 1].ToString("f4") + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0, 4] * 1 / dist).ToString("f4") + "  " + "M213==" + (NC_Data[0, 6] * 1000).ToString("f4") + "  " + "\n"));
                        //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                        if (NC_Data[0, 0] > 0)
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0])).ToString("f4") + "-M162/32/96)>100" + ")");
                        else
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0]) ).ToString("f4") + "-M162/32/96)<-100" + ")");
                        Pmac.SendCommand("P10=0");
                        Pmac.SendCommand("DWELL 10");
                        Pmac.SendCommand("ENDWHILE");
                        Pmac.SendCommand("P10=1");
                        Pmac.SendCommand("CLOSE");

                        //运动到初始加工位置
                        Pmac.SendCommand("OPEN PROG 7");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");

                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0, 0] + "  " + "B" + NC_Data[0, 1] + "  " + "F" +NC_Data[0,2].ToString().Trim()+ "  " + "M113==" + (NC_Data[0, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[0, 6] * 1000).ToString("f4") + "  " + "\n"));
                        Pmac.SendCommand("CLOSE");

                        Pmac.SendCommand("OPEN PROG 8");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");

                        Pmac.SendCommand(Convert.ToString("X" + (-NC_Data[0, 0]) + "  " + "B" + (-NC_Data[0, 1]) + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[0, 6] * 1000).ToString("f4") + "  " + "\n"));
                        Pmac.SendCommand("CLOSE");

                    }

                    //**********************************运动程序代码***********************/


                    // wr = new StreamWriter(fs);
                    if (radioButton20.Checked == true)//radiobutton 单方向加工
                    {
                        string status;
                        PMAC.GetResponse(pmacNumber, "P25=" + Convert.ToString(n), out  status);
                        Pmac.SendCommand("OPEN PROG 2");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("PSET Z(P50)");//加工程序
                        Pmac.SendCommand("G90 G01 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                        if (comboBox8.SelectedIndex == 0)//工件转动方向
                        {
                            Pmac.SendCommand("M3==1");
                            Pmac.SendCommand("M4==0");
                        }
                        else
                        {
                            Pmac.SendCommand("M3==0");
                            Pmac.SendCommand("M4==1");
                        }
                        if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                        {
                            Pmac.SendCommand("M1==0");

                        }
                        else
                        {
                            Pmac.SendCommand("M1==1");
                        }
                        //wr.WriteLine("P25=0");
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=1");
                        for (int i = 0; i < NC_Data.Length / 7; i++)
                        {
                            Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                            if (i > 0)
                                t = t + Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) / NC_Data[i, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                        }
                        Pmac.SendCommand("P25=P25-1");
                        // wr.WriteLine("ENDWHILE");
                        Pmac.SendCommand("DWELL 0");
                        Pmac.SendCommand("IF(P25=0)");
                        Pmac.SendCommand("P32=1");
                        Pmac.SendCommand("P14=1");
                        Pmac.SendCommand("ELSE");
                        Pmac.SendCommand("COMMAND \"&1B1R\"");
                        Pmac.SendCommand("M5=0");
                        Pmac.SendCommand("ENDIF");

                        Pmac.SendCommand("CLOSE");
                        // wr.Close();



                    }
                    else//双方向加工
                    {
                        if (n == 1)
                        {
                            Pmac.SendCommand("OPEN PROG 2");
                            Pmac.SendCommand("CLEAR");
                            Pmac.SendCommand("PSET Z(P50)");//加工程序
                            Pmac.SendCommand("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                            //wr.WriteLine("P25=0");
                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                Pmac.SendCommand("M1==0");

                            }
                            else
                            {
                                Pmac.SendCommand("M1==1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3=1");
                                Pmac.SendCommand("M4=0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3=0");
                                Pmac.SendCommand("M4=1");
                            }

                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3==1");
                                Pmac.SendCommand("M4==0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3==0");
                                Pmac.SendCommand("M4==1");
                            }


                            Pmac.SendCommand("P26=1");
                            Pmac.SendCommand("P27=1");
                            Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int i = 0; i < NC_Data.Length / 7; i++)
                            {
                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) / NC_Data[i, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                            }
                            Pmac.SendCommand("P25=P25+1");
                            Pmac.SendCommand("ENDWHILE");
                            Pmac.SendCommand("DWELL 0");
                            Pmac.SendCommand("P14=1");
                            Pmac.SendCommand("P32=1");
                            Pmac.SendCommand("M7=1");
                            Pmac.SendCommand("M113=0 M213=0");
                            Pmac.SendCommand("ENABLE PLC6");
                            Pmac.SendCommand("CLOSE");
                            // wr.Close();

                        }
                        else
                        {
                            Pmac.SendCommand("OPEN PROG 2");
                            Pmac.SendCommand("CLEAR");
                            Pmac.SendCommand("PSET Z(P50)");//加工程序
                            Pmac.SendCommand("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                            //wr.WriteLine("P25=0");

                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                Pmac.SendCommand("M1==0");

                            }
                            else
                            {
                                Pmac.SendCommand("M1==1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3=1");
                                Pmac.SendCommand("M4=0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3=0");
                                Pmac.SendCommand("M4=1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3==1");
                                Pmac.SendCommand("M4==0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3==0");
                                Pmac.SendCommand("M4==1");
                            }

                            Pmac.SendCommand("P26=1");
                            Pmac.SendCommand("P27=0");
                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1");
                            for (int i = 0; i < NC_Data.Length / 7; i++)
                            {

                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 5] - NC_Data[i - 1, 5]), 2)) / NC_Data[i, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                            }
                            Pmac.SendCommand("P27=P27+1");
                            for (int i = NC_Data.Length / 7 - 2; i >= 0; i--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i + 1, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 10).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1");
                            Pmac.SendCommand("ENDWHILE");
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1");
                                for (int i = 0; i < NC_Data.Length / 7; i++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                                }

                            }
                            Pmac.SendCommand("DWELL 0");
                            Pmac.SendCommand("P14=1");
                            Pmac.SendCommand("P32=1");
                            Pmac.SendCommand("M7=1");
                            Pmac.SendCommand("M113=0 M213=0");
                            Pmac.SendCommand("ENABLE PLC6");
                            Pmac.SendCommand("CLOSE");
                            //wr.Close();

                        }


                    }

                    process_time = (t * n + Math.Abs(NC_Data[0, 0]) / first_position_feed) + (B_axi_Center_Polish_head_distance - Math.Abs(NC_Data[0, 5]))/z_axi_first_jogdownfeed+ 0.05 + caculate_time_offset; //加工总时间
                    timer5.Enabled = false;
                }


                catch (Exception)
                {
                    MessageBox.Show("导入失败！线程安全冲突，请重启软件", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    download_code_flag = false;
                    button12.Text = "导入失败";
                    timer5.Enabled = true;
                    //dectect.Start();
                    return;
                }
               




                    button12.Text = "导入成功";
                    own_code_flag = true;
                    compensate_code_flag = false;
                    other_code_flag = false;
                    download_code_flag = true;
                    // Delay(1);
                    timer5.Enabled = true;
                    process_begin_flag = false;
                    MessageBox.Show("加工时长：" + process_time.ToString("f2") + "分钟");
                    // dectect.Start();





            }
            #endregion
            /**************************************多段加工*****************************************/
            #region
            else//多段加工
            {


                button12.Text = "导入中...";
                //string pathh = System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt";

                //FileStream stream = File.Open(pathh, FileMode.OpenOrCreate, FileAccess.Write);
                //stream.Seek(0, SeekOrigin.Begin);
                //stream.SetLength(0); //清空txt文件
                //stream.Close();

                //richTextBox2.SaveFile(pathh, RichTextBoxStreamType.PlainText);  //richtexbox
                //Delay(1);
                ///***************************把richbox修改过的替换掉******最初身生成的**************/
                //StreamReader _rstream = null;//为richtext所创建的流
                //  string line;
                string result = "";
                //   _rstream = new StreamReader(pathh, System.Text.Encoding.UTF8);
                try
                {

                    //  while ((line = _rstream.ReadLine()) != null)
                    //{

                     double[] arry= new double [16];

            if (textBox40.Text != "")
                arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
            else
                arry[0] = 0;

            if (textBox51.Text != "")
                arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
            else
                arry[1] = 0;
            if (textBox55.Text != "")
                arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
            else
                arry[2] = 0;
            if (textBox54.Text != "")
                arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
            else
                arry[3] = 0;
            if (textBox58.Text != "")
                arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
            else
                arry[4] = 0;
            if (textBox57.Text != "")
                arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
            else
                arry[5] = 0;
            if (textBox61.Text != "")
                arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
            else
                arry[6] = 0;
            if (textBox60.Text != "")
                arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
            else
                arry[7] = 0;
            if (textBox64.Text != "")
                arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
            else
                arry[8] = 0;
            if (textBox63.Text != "")
                arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
            else
                arry[9] = 0;
            if (textBox67.Text != "")
                arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
            else
                arry[10] = 0;
            if (textBox66.Text != "")
                arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
            else
                arry[11] = 0;
            if (textBox73.Text != "")
                arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
            else
                arry[12] = 0;
            if (textBox72.Text != "")
                arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
            else
                arry[13] = 0;
            if (textBox70.Text != "")
                arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
            else
                arry[14] = 0;
            if (textBox69.Text != "")
                arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
            else
                arry[15] = 0;
           

            Array.Sort(arry);

            int j = (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2 + 1;
              
                    foreach (string line in richTextBox1.Lines)
                    {



                      //  if (j >= 0 & j < NC_Data.Length / 7)
                        if( j >= (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2+1 & j < (NC_Data.Length / 7 - 3) / 2 + Convert.ToInt16(arry[15] * 1/dist)+1)
                        {

                            result = line;
                            int x = result.IndexOf("X");

                            int b = result.IndexOf("B");
                            int z = result.IndexOf("Z");
                         //   int f = result.IndexOf("F");
                            string str1 = result.Substring(x + 1, b - x - 1);
                            string str3 = result.Substring(b + 1, z - b - 1);
                            string str33 = result.Substring(z + 1);
                          // string str33 = result.Substring(z + 1, f - z - 1);
                        //   string str4 = result.Substring(f + 1);

                            // NC_Data[j, 0] = Convert.ToDouble(str1.Trim());
                            NC_Data[j, 1] = Convert.ToDouble(str3.Trim());

                            //if (j == 0)
                            //    NC_Data[j, 2] = Convert.ToDouble(str4.Trim());
                            //else
                            //    NC_Data[j, 2] = Math.Abs(NC_Data[j, 0] - NC_Data[j - 1, 0]) * Convert.ToDouble(str4.Trim()) * 10;

                            //if (j == 0)
                            //    NC_Data[j, 2] = Convert.ToDouble(str4.Trim());
                            //else
                            //    NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                            //if (j == 0)
                            //    NC_Data[j, 2] = NC_Data[j, 2];
                            //else
                            //  NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * NC_Data[j, 2] / Math.Abs(NC_Data[j, 0] - NC_Data[j - 1, 0])*Convert.ToDouble(textBox4.Text) * 0.01;//进给速度X Z联动
                            //   // NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                  

                            NC_Data[j, 5] = Convert.ToDouble(str33.Trim());

                            // break;
                        }
                        j++;


                    }
                    //_rstream.Close();

                    //    File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt");
                }

                catch(Exception err)
                {
                    //MessageBox.Show("导入失败！请勿删除代码", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(err.Message);
                    button12.Text = "导入非补正代码";
                    //_rstream.Close();
                    return;
                }

                double t = 0;//加工一次时间
                /*************************************生成代码**********************************************/


                /***************第一个位置代码**************/

                double n;

                try
                {
                    n = Convert.ToDouble(textBox16.Text);//加工次数
                }
                catch (Exception)
                {
                    MessageBox.Show("导入失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button12.Text = "导入非补正代码";
                    return;
                }

               try
               {


                    t = download_Gcode();

                    process_time = t + caculate_time_offset;
                    timer5.Enabled = false;
               }



              catch (Exception err)
                {
                    MessageBox.Show("导入失败！"+err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    download_code_flag = false;
                    button12.Text = "导入失败";
                    timer5.Enabled = true;
                    //dectect.Start();
                    return;
               }
              finally
              {

                    button12.Text = "导入成功";
                    own_code_flag = true;
                    compensate_code_flag = false;
                    other_code_flag = false;
                    download_code_flag = true;
                    // Delay(1);
                    timer5.Enabled = true;
                    process_begin_flag = false;
                    MessageBox.Show("加工时长：" + process_time.ToString("f2") + "分钟");

              }


            }
            #endregion

            /******************************************************************/







        }


        private void button20_Click_1(object sender, EventArgs e)//由生成代码界面到模式选择界面
        {
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            this.button12.Text = "导入非补正代码";
            this.button47.Text = "导入补正代码";
        }

        private void button30_Click_1(object sender, EventArgs e)//跳转到加工参数设定
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = true;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
           // this.timer1.Enabled = false;
            this.button19.Enabled = true;
            this.textBox34.Text = textBox1.Text;//显示模仁名称
           // textBox34.Enabled = false;
           // textBox35.Text = SAG_text.Text;//显示模仁平台高度
            // textBox33.Enabled = false;
          //  textBox36.Text = TextBox35.Text;//显示模仁口径
            // textBox32.Enabled = false;
            timer4.Enabled = true;//开启加工参数界面计时器
            if (set_proces_flag == false)
            {
                radioButton8.Checked = true;//45度角
                radioButton1.Checked = true;//恒转速
                radioButton3.Checked = true;//速率可变

                textBox3.Text = "150";//最大转速
                textBox4.Text = "100";//
                //textBox40.Text = "0.1";//数据间隔
                textBox39.Text = "1";//加工次数
                comboBox1.SelectedIndex = 0;//模仁材质
                comboBox3.SelectedIndex = 0;//研磨颗粒规格
                comboBox4.SelectedIndex = 0;//数据间隔
                comboBox5.SelectedIndex = 1;//磨头直径
                textBox33.Text = "180";//抛光轴转速
               // textBox32.Text = "3";//抛光头直径
                comboBox12.SelectedIndex = 1;//研磨布厚度
                textBox37.Text = "50";//荷重
                textBox2.Text = "150";//恒转速
                textBox5.Text = "60";//恒进给
                set_proces_flag = true;
            }
        }

        private void button43_Click_1(object sender, EventArgs e)//面型误差修正
        {
            //this.tabControl1.SelectedIndex = 4;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = true;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
            this.textBox48.Text = textBox34.Text;//补正模仁名称
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("确定要关闭?", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {


                e.Cancel = true;
            }
            else
            {
                Pmac.SendCommand("DISABLE PLC6");
               // Pmac.SendCommand("K");
            }
                //this.dectect.Abort();
        }

        private void button44_Click_1(object sender, EventArgs e)
        {
            if (Pcode_return_flag == true)
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = false;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = false;
            }
            else
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = false;
                this.panel5.Visible = true;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = false;
 
            }

        }

        private void button45_Click_1(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = false;
        }

        private void button48_Click(object sender, EventArgs e)//生成补正代码
        {
            //#region //生成非补正代码
            //Produce_x_b_F_C NC = new Produce_x_b_F_C();
            double curvature_compensate, n, R, K, dist, t = 0, vc, H, R_left, SAG, yuan_r, ao_tu, R_right, constan_vc, constan_F, symbol = 1, tool_r, Dp, D_workpiece;
            ////double symbol;
            //// double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };

            //richTextBox2.Clear();

            ////A = new double[24] { Convert.ToDouble(H_text.Text), Convert.ToDouble(D_text.Text), Convert.ToDouble(R_text.Text), Convert.ToDouble(K_text.Text), Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
            //// if (textBox35.Text != null || textBox39 == null)
            ////  {
            ////     MessageBox.Show("请输入工件转速");
            ////  }
            ////  else if (textBox39 != null || textBox35.Text == null)
            ////  {
            ////      MessageBox.Show("请输入加工次数");
            ////  }
            ///// if (textBox35.Text != null) //|| textBox39 == null)
            ////{
            ////    MessageBox.Show("请输入工件转速和加工次数");
            //// }
            //// else
            ////{
           double[] arry = new double[16];

            //if (textBox40.Text != "")
            //    arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
            //else
            //    arry[0] = 0;

            //if (textBox51.Text != "")
            //    arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
            //else
            //    arry[1] = 0;
            //if (textBox55.Text != "")
            //    arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
            //else
            //    arry[2] = 0;
            //if (textBox54.Text != "")
            //    arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
            //else
            //    arry[3] = 0;
            //if (textBox58.Text != "")
            //    arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
            //else
            //    arry[4] = 0;
            //if (textBox57.Text != "")
            //    arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
            //else
            //    arry[5] = 0;
            //if (textBox61.Text != "")
            //    arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
            //else
            //    arry[6] = 0;
            //if (textBox60.Text != "")
            //    arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
            //else
            //    arry[7] = 0;
            //if (textBox64.Text != "")
            //    arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
            //else
            //    arry[8] = 0;
            //if (textBox63.Text != "")
            //    arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
            //else
            //    arry[9] = 0;
            //if (textBox67.Text != "")
            //    arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
            //else
            //    arry[10] = 0;
            //if (textBox66.Text != "")
            //    arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
            //else
            //    arry[11] = 0;
            //if (textBox73.Text != "")
            //    arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
            //else
            //    arry[12] = 0;
            //if (textBox72.Text != "")
            //    arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
            //else
            //    arry[13] = 0;
            //if (textBox70.Text != "")
            //    arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
            //else
            //    arry[14] = 0;
            //if (textBox69.Text != "")
            //    arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
            //else
            //    arry[15] = 0;


            //Array.Sort(arry);

            //try
            //{

            //    double[] A = new double[20] { Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };

            //    n = Convert.ToDouble(textBox16.Text);//加工次数
            //    R = Convert.ToDouble(R_text.Text);  //面型参数R
            //    K = Convert.ToDouble(K_text.Text);//面型参数K
            //    vc = Convert.ToDouble(textBox3.Text);//C轴最大转速
            //    H = Convert.ToDouble(TextBox35.Text);//模仁高度

            //    SAG = Convert.ToDouble(SAG_text.Text);//非球面中心到平面距离
            //    //  fixture_h = Convert.ToDouble(yuanr_text.Text.Trim());//夹具高度，从移动调心夹具底面到模仁底面距离
            //    yuan_r = Convert.ToDouble(yuanr_text.Text.Trim());//圆角半径
            //    constan_vc = Convert.ToDouble(textBox2.Text);//恒定C转速
            //    constan_F = Convert.ToDouble(textBox5.Text);//恒定进给

            //    Dp = Convert.ToDouble(gongD_textBox3.Text.Trim());//工件加工口径
            //    D_workpiece = Convert.ToDouble(textBox46.Text.Trim());

            //    if (D_workpiece * 10 % 2 == 1)
            //        D_workpiece = D_workpiece - 0.1;

            //    if (radioButton23.Checked == true)//单段加工
            //    {

            //        if (textBox6.Text == "")
            //            R_left = 0;
            //        else
            //            R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

            //        if (textBox18.Text == "")
            //            R_right = 0;
            //        else
            //            R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值
            //        // textBox18.text
            //        if (R_left >= R_right)
            //        { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }



            //    }
            //    else
            //    {
            //        R_left = -D_workpiece / 2;
            //        R_right = D_workpiece / 2;
            //        if (arry[15] > R_right)
            //        { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }

            //    }



            //    if (Math.Abs(R_left) > D_workpiece / 2)
            //    {
            //        MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
            //        return;
            //    }

            //    if (Math.Abs(R_right) > D_workpiece / 2)
            //    { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }


            //    //  if()
            //    if (ao_non_sphere.Checked == true)//凹凸判断
            //        ao_tu = -1;
            //    else
            //        ao_tu = 1;
            //    if (comboBox4.SelectedIndex == 0)

            //        dist = 0.1;
            //    else if (comboBox4.SelectedIndex == 1)
            //        dist = 0.01;
            //    else
            //        dist = 0.01;

            //    if (comboBox5.SelectedIndex == 0)
            //        tool_r = 1;
            //    else if (comboBox5.SelectedIndex == 1)
            //        tool_r = 3;
            //    else
            //        tool_r = 5;

            //    if (radioButton7.Checked == true)//垂直抛
            //    {
            //        tool_r = 7;
            //    }


            //    if (ao_non_sphere.Checked == true || tu_non_sphere.Checked == true)
            //    {
            //        if (ao_non_sphere.Checked == true)
            //        {
            //            if (R > 0)
            //                symbol = 1;
            //            else
            //                symbol = -1;

            //        }
            //        if (tu_non_sphere.Checked == true)
            //        {
            //            if (R > 0)
            //                symbol = -1;
            //            else
            //                symbol = 1;
            //        }
            //        if (comboBox13.SelectedIndex == 1)
            //            curvature_compensate = 1;
            //        else if (comboBox13.SelectedIndex == 2)
            //            curvature_compensate = 2;
            //        else
            //            curvature_compensate = 0;

            //        //textBox47.Text = Convert.ToString(symbol);//看看symbol对不对
            //        //if(radioButton14.Checked==true)
            //        //     NC_Data = NC.asphere(constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

            //        //else if(radioButton16.Checked==true)
            //        //    NC_Data = NC.asphere_heitian2(SAG, yuan_r, ao_tu, D_end, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码

            //        //   else
            //        //NC_Data = NC.asphere_heitian(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
            //        if (comboBox4.SelectedIndex == 0)
            //            NC_Data = NC.asphere_heitian(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码               
            //        else
            //            NC_Data = NC.asphere_heitian_dist(curvature_compensate, first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码


            //        // textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间
            //    }
            //    if (ao_sphere.Checked == true || tu_sphere.Checked == true)
            //    {
            //        if (ao_sphere.Checked == true)
            //            symbol = 1;
            //        if (tu_sphere.Checked == true)
            //            symbol = -1;
            //        NC_Data = NC.sphere(first_position_feed, D_workpiece, Dp, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, SAG, yuan_r, ao_tu, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码
            //        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间


            //    }
            //    if (plane.Checked == true)
            //    {
            //        if (radioButton23.Checked == true)
            //        {
            //            if (textBox6.Text != "")
            //                R_left = Convert.ToDouble(textBox6.Text);//加工口径
            //            else
            //            {
            //                MessageBox.Show("请设定加工范围！");
            //                return;
            //            }

            //            if (textBox18.Text != "")
            //                R_right = Convert.ToDouble(textBox18.Text);//加工口径
            //            else
            //            {
            //                MessageBox.Show("请设定加工范围！");
            //                return;
            //            }
            //        }

            //        if (Math.Abs(R_left) < arry[15])
            //            R_left = arry[15];
            //        //if (radioButton14.Checked == true)
            //        //    NC_Data = NC.plane(C_motor_scale_factor, C_motor_offset, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, D, R, K, A, out t);//生成代码              
            //        //     else
            //        NC_Data = NC.plane_heitian(first_position_feed, D_workpiece, C_motor_scale_factor, C_motor_offset, Lworkpiece_h, other_h, R_right, tool_r, constan_vc, constan_F, Vc_flag, F_flag, dist, symbol, n, vc, H, R_left, R, K, A, out t);//生成代码

            //        //textBox38.Text = Convert.ToString((t * Convert.ToDouble(textBox39.Text) / (Convert.ToDouble(textBox4.Text) * 0.01)).ToString("f4"));//显示时间

            //    }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("参数设定发生错误！请检查加工参数和形状参数是否设定。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //#endregion





            
            try{
          
                n= Convert.ToDouble(textBox17.Text);//Convert.ToDouble(textBox39.Text);//加工次数
       
               }
            catch (Exception )
            {
                MessageBox.Show("生成失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           // string path = System.Environment.CurrentDirectory + "\\模仁补正加工代码\\Gcode_primary_compensate.txt";
            richTextBox2.Clear();
            //  if (path == "")
            // {
            //      return;
            //  }

            // string result1 = @".\GCode.txt";//结果保存到F:\result1.txt

            //*先清空result1.txt文件内容
            //FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            //stream2.Seek(0, SeekOrigin.Begin);
            //stream2.SetLength(0); //清空txt文件
            //stream2.Close();

            try
            {
                

           // FileStream fs = new FileStream(path, FileMode.Append);
            // fs.Seek(0, SeekOrigin.Begin);
            // fs.SetLength(0);
           // StreamWriter wr = null;
            bool x_limit_flag = false;

           // double  R_left, R_right, D_workpiece,dist;
            if (comboBox4.SelectedIndex == 0)

                dist = 0.1;
            else if (comboBox4.SelectedIndex == 1)
                dist = 0.01;
            else
                dist = 0.01;

            D_workpiece = Convert.ToDouble(textBox46.Text.Trim());

            if (D_workpiece * 10 % 2 == 1)
                D_workpiece = D_workpiece - 0.1;

            if (textBox6.Text == "")
                R_left = 0;
            else
                R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

            if (textBox18.Text == "")
                R_right = 0;
            else
                R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值
            // textBox18.text
           

            if (Math.Abs(R_left) > D_workpiece / 2)
            {
                MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                return;
            }

            if (Math.Abs(R_right) > D_workpiece / 2)
            { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }
            if (R_left >= R_right)
            { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }
            int c = Convert.ToInt16((R_right - R_left) / dist);//输出代码行数-1
            int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
            int d = Convert.ToInt16(R_left / dist);
          //  int e = Convert.ToInt16(R_right / dist);
            //double[,] b = new double[c + 1, 7];
            //for (int i = 0; i < c + 1; i++)
            //{

            //    b[i, 0] = NC_Data[i + d + a / 2];//x移动坐标
            //    b[i, 1] = B[i + d + a / 2];//B轴角度

            //    b[i, 2] = F[i + d + a / 2];//进给速度
            //    b[i, 3] = Nc[i + d + a / 2];//C轴转速

            //    b[i, 4] = X[i + d + a / 2];//X工件坐标
            //    b[i, 5] = ZZ[i + d + a / 2];//Z轴移动坐标
            //    b[i, 6] = Z1[i + d + a / 2];//函数曲线Z值
            //}
         //   b[0, 2] = 200;
           // wr = new StreamWriter(fs);
          //  double[] arry = new double[16];

            if (textBox40.Text != "")
                arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
            else
                arry[0] = 0;

            if (textBox51.Text != "")
                arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
            else
                arry[1] = 0;
            if (textBox55.Text != "")
                arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
            else
                arry[2] = 0;
            if (textBox54.Text != "")
                arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
            else
                arry[3] = 0;
            if (textBox58.Text != "")
                arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
            else
                arry[4] = 0;
            if (textBox57.Text != "")
                arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
            else
                arry[5] = 0;
            if (textBox61.Text != "")
                arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
            else
                arry[6] = 0;
            if (textBox60.Text != "")
                arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
            else
                arry[7] = 0;
            if (textBox64.Text != "")
                arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
            else
                arry[8] = 0;
            if (textBox63.Text != "")
                arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
            else
                arry[9] = 0;
            if (textBox67.Text != "")
                arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
            else
                arry[10] = 0;
            if (textBox66.Text != "")
                arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
            else
                arry[11] = 0;
            if (textBox73.Text != "")
                arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
            else
                arry[12] = 0;
            if (textBox72.Text != "")
                arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
            else
                arry[13] = 0;
            if (textBox70.Text != "")
                arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
            else
                arry[14] = 0;
            if (textBox69.Text != "")
                arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
            else
                arry[15] = 0;

            int index = 0;
            Array.Sort(arry);
            if (compensate_flag == true)
            {
                if(radioButton23.Checked==true)//单段加工
                {

                    for (int i = 0; i < c + 1; i++)
                    {

                        // wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));


                        if (checkBox10.Checked == true)
                        {
                            if (input_compesentfile_flag == true)
                            {

                                if (i == 0)
                                    richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i + d + a / 2, 4].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "\n"));// ).ToString("f4") + "  " + "\n"));//
                                else
                                {
                                    if (NC_Data[i + d + a / 2, 4] < 0.01)
                                        index = Convert.ToInt32(Math.Abs(NC_Data[i + d + a / 2, 4]) * 10);
                                    else
                                        index = Convert.ToInt32(Math.Abs(NC_Data[i + d + a / 2, 4]) * 10) - 1;

                                    richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i + d + a / 2, 4].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i + d + a / 2] * dist / Math.Abs(NC_Data[i + d + a / 2, 0] - NC_Data[i + d + a / 2 - 1, 0])*Compesent_rate[index]).ToString("f4") + "  " + "\n"));
                                
                                }

                            }
                            else
                            {
                                MessageBox.Show("请先导入补偿文件！");
                                return;
                            }
                        }
                        else
                        {
                           
                            if (i == 0)
                                richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i + d + a / 2, 4].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "\n"));// ).ToString("f4") + "  " + "\n"));//
                            else
                                richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i + d + a / 2, 4].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i + d + a / 2] * dist / Math.Abs(NC_Data[i + d + a / 2, 0] - NC_Data[i + d + a / 2 - 1, 0])).ToString("f4") + "  " + "\n"));
                    
 
                        }
                      
                        
                        
                        if (NC_Data[i + d + a / 2, 0] > xlimit)
                            x_limit_flag = true;
                   


                    
                    
                    }


                }
                else //多段加工
                {

                    for (int i = (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2 + 1; i <= (NC_Data.Length / 7 - 3) / 2 + Convert.ToInt16(arry[15] /dist) + 1; i++)
                    {

                        // wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                        if (checkBox10.Checked == true)
                        {
                            if (input_compesentfile_flag == true)
                            {

                                if (i == (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2 / dist)) / 2 + 1)
                                    richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[0]).ToString("f4") + "  " + "\n"));
                                else
                                {

                                    if (NC_Data[i , 4] < 0.01)
                                        index = Convert.ToInt32(Math.Abs(NC_Data[i , 4]) * 10);
                                    else
                                        index = Convert.ToInt32(Math.Abs(NC_Data[i , 4]) * 10) - 1;

                                    richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] * dist / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])*Compesent_rate[index]).ToString("f4") + "  " + "\n"));
                               
                                }
                            }
                            else 
                            {

                                MessageBox.Show("请先导入补偿文件！");
                                return;
                            }
                        }
                        else
                        {
                            if (i == (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2 / dist)) / 2 + 1)
                                richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[0]).ToString("f4") + "  " + "\n"));
                            else
                                richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] * dist / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));

                        }

                        if (NC_Data[i, 0] > xlimit)
                            x_limit_flag = true;

                    }
                    //for (int i = 0; i < NC_Data.Length / 7; i++)
                    //{

                    //    // wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                    //    if (i == 0)
                    //        richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "\n"));
                    //    else
                    //        richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
                    //    if (NC_Data[i, 0] > xlimit)
                    //        x_limit_flag = true;

                    //}
               
                }
                
                //if (n == 1)
                //{
                //    wr.WriteLine("OPEN PROG 2");
                //    wr.WriteLine("CLEAR");
                //    wr.WriteLine("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                //    //wr.WriteLine("P25=0");
                //    wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                //    for (int i = 0; i < NC_Data.Length / 7; i++)
                //    {

                //        wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                //        if (i == 0)
                //            richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "\n"));
                //        else
                //            richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])).ToString("f4") + "  " + "\n"));
                //        if (NC_Data[i, 0] > xlimit)
                //            x_limit_flag = true;
                //    }
                //    wr.WriteLine("P25=P25+1");
                //    wr.WriteLine("ENDWHILE");
                //    wr.WriteLine("DWELL 0");
                //    wr.WriteLine("P14=1");
                //    wr.WriteLine("P32=1");
                //    wr.WriteLine("CLOSE");

            
                //    wr.Close();

                //}
                //else
                //{
                //    wr.WriteLine("OPEN PROG 2");
                //    wr.WriteLine("CLEAR");
                //    wr.WriteLine("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                //    //wr.WriteLine("P25=0");
                //    if (n % 2 == 1)
                //        wr.WriteLine("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                //    else
                //        wr.WriteLine("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                //    for (int i = 0; i < NC_Data.Length / 7; i++)
                //    {
                //        if(i==0)
                //            richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] ).ToString("f4") + "  " + "\n"));                      
                //        else
                //        richTextBox2.AppendText(Convert.ToString("X" + NC_Data[i, 4].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i] * 0.1 / Math.Abs(NC_Data[i, 0] - NC_Data[i-1, 0])).ToString("f4") + "  " + "\n"));
                //        wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                //        if (NC_Data[i, 0] > xlimit)
                //            x_limit_flag = true;
                //    }
                //    for (int i = NC_Data.Length / 7 - 1; i >= 0; i--)//代码倒转
                //    {
                //        wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                //    }
                //    wr.WriteLine("P25=P25+1");
                //    wr.WriteLine("ENDWHILE");
                //    if (n % 2 == 1)
                //    {
                //        for (int i = 0; i < NC_Data.Length / 7; i++)
                //        {
                //            wr.WriteLine(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + (Convert.ToDouble(textBox4.Text) * 0.01 * F1[i]).ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "\n"));
                //        }

                //    }
                //    wr.WriteLine("DWELL 0");
                //    wr.WriteLine("P14=1");
                //    wr.WriteLine("P32=1");
                //    wr.WriteLine("CLOSE");
                //    wr.Close();
                //}
                if (x_limit_flag == true)
                    MessageBox.Show("加工代码超出X轴行程");
            //    File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\Gcode_primary_compensate.txt");
                

            }
            else
            {
                MessageBox.Show("生成失败！请先进行面型误差参数补正数据导入与运算", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

               }
            catch (Exception )
            {
                MessageBox.Show("生成失败！请先进行面型误差参数补正数据导入与运算", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void button47_Click(object sender, EventArgs e)//导入补正代码
        {
            #region
            if (radioButton23.Checked == true)//单段加工
            {
                button47.Text = "导入中...";
                //string pathh = System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt";

                //FileStream stream = File.Open(pathh, FileMode.OpenOrCreate, FileAccess.Write);
                //stream.Seek(0, SeekOrigin.Begin);
                //stream.SetLength(0); //清空txt文件
                //stream.Close();

                //richTextBox2.SaveFile(pathh, RichTextBoxStreamType.PlainText);  //richtexbox
                //Delay(1);
                ///***************************把richbox修改过的替换掉******最初身生成的**************/
                //StreamReader _rstream = null;//为richtext所创建的流
                //string line;
                double R_left, R_right, D_workpiece, dist;
                if (comboBox4.SelectedIndex == 0)

                    dist = 0.1;
                else if (comboBox4.SelectedIndex == 1)
                    dist = 0.01;
                else
                    dist = 0.1;

                D_workpiece = Convert.ToDouble(textBox46.Text.Trim());

                if (D_workpiece * 10 % 2 == 1)
                    D_workpiece = D_workpiece - 0.1;

                if (textBox6.Text == "")
                    R_left = 0;
                else
                    R_left = Convert.ToDouble(textBox6.Text);//加工范围半径

                if (textBox18.Text == "")
                    R_right = 0;
                else
                    R_right = Convert.ToDouble(textBox18.Text);//加工口径另一值
                // textBox18.text


                if (Math.Abs(R_left) > D_workpiece / 2)
                {
                    MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！");
                    return;
                }

                if (Math.Abs(R_right) > D_workpiece / 2)
                { MessageBox.Show("加工范围设定超出工件口径，请重新设定加工范围！", "提示！"); return; }
                if (R_left >= R_right)
                { MessageBox.Show("加工范围设定有误，左边框值不可大于右边框值，请重新设定加工范围！", "提示！"); return; }
                int c = Convert.ToInt16((R_right - R_left) / dist);//输出代码行数-1
                int a = Convert.ToInt16(D_workpiece / dist);//代码数组行数-1
                int d = Convert.ToInt16(R_left / dist);

                string result = "";
                int j = 0;
               // _rstream = new StreamReader(pathh, System.Text.Encoding.UTF8);
                try
                {

                   // while ((line = _rstream.ReadLine()) != null)
                    foreach (string line in richTextBox2.Lines)
                    {


                        if (j >= 0 & j < c + 1)
                        {

                            result = line;
                            int x = result.IndexOf("X");

                            int b = result.IndexOf("B");
                            int z = result.IndexOf("Z");
                            int f = result.IndexOf("F");
                            string str1 = result.Substring(x + 1, b - x - 1);
                            string str3 = result.Substring(b + 1, z - b - 1);
                            string str33 = result.Substring(z + 1, f - z - 1);
                            string str4 = result.Substring(f + 1);

                            // NC_Data[j, 0] = Convert.ToDouble(str1.Trim());
                            NC_Data[j + d + a / 2, 1] = Convert.ToDouble(str3.Trim());

                            if (j == 0)
                                NC_Data[j + d + a / 2, 2] = Convert.ToDouble(str4.Trim());
                            else
                                NC_Data[j + d + a / 2, 2] = Math.Sqrt(Math.Pow((NC_Data[j + d + a / 2, 0] - NC_Data[j + d + a / 2 - 1, 0]), 2) + Math.Pow((NC_Data[j + d + a / 2, 5] - NC_Data[j + d + a / 2 - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) / dist;//进给速度X Z联动


                            NC_Data[j + d + a / 2, 5] = Convert.ToDouble(str33.Trim());


                            // break;
                        }
                        j++;


                    }
             //       _rstream.Close();

                 //   File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt");
                }

                catch
                {
                    MessageBox.Show("导入失败！请勿删除代码", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button47.Text = "导入补正代码";
                  //  _rstream.Close();
                    return;
                }

                double t = 0;//加工一次时间
                /*************************************生成代码**********************************************/

              
/*
                      File.Delete(pathh);
                      File.Delete(path);
                      File.Delete(path2);
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download1.PMA");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download2.PMA");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download1.TBL");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download2.TBL");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download1.56K");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download2.56K");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download1.MAP");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download2.MAP");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download1.LOG");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\download2.LOG");
                      File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\Gcode_primary_compensate.txt");

                      /*******************************在线下载测试*******************************************/


                /***************第一个位置代码**************/

                double n;

                try
                {
                    n = Convert.ToDouble(textBox17.Text);//加工次数
                }
                catch (Exception )
                {
                    MessageBox.Show("导入失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button47.Text = "导入补正代码";
                    return;
                }

                try
                {
                    if (NC_Data[0 + d + a / 2, 5] < min_z)
                    {
                        MessageBox.Show("Z轴行程不足，无法加工！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button47.Text = "导入补正代码";
                        return;
                    }
                    if (radioButton20.Checked == true)
                    {
                        Pmac.SendCommand("OPEN PROG 1");
                        Pmac.SendCommand("CLEAR");
                        // wr.WriteLine("S150");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand("P50=" + Convert.ToString(NC_Data[0 + d + a / 2, 5]));
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");

                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0, 0].ToString("f4") + "  " + "B" + NC_Data[0, 1].ToString("f4") + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0, 4] * 1 / dist).ToString("f4") + "  " + "M213==" + (NC_Data[0, 6] * 1000).ToString("f4") + "  " + "\n"));
                        //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                        if (NC_Data[0, 0] > 0)
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0]) ).ToString("f4") + "-M162/32/96)>100" + ")");
                        else
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0, 0]) ).ToString("f4") + "-M162/32/96)<-100" + ")");
                        Pmac.SendCommand("P10=0");
                        Pmac.SendCommand("DWELL 10");
                        Pmac.SendCommand("ENDWHILE");

                        //  wrr.WriteLine("WHILE(M11==0)" );              
                        //    wrr.WriteLine("ENDWHILE");
                        // wrr.WriteLine("M5=1");

                        Pmac.SendCommand("IF(P25=" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P10=1");
                        Pmac.SendCommand("ELSE");
                        Pmac.SendCommand("M5=1");
                        Pmac.SendCommand("DWELL 3000");
                        Pmac.SendCommand("COMMAND \"&1B2R\"");
                        Pmac.SendCommand("ENDIF");
                        Pmac.SendCommand("CLOSE");

                    }
                    else
                    {
                        Pmac.SendCommand("OPEN PROG 1");
                        Pmac.SendCommand("CLEAR");
                        // wr.WriteLine("S150");
                        Pmac.SendCommand("DISABLE PLC6");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand("P50=" + Convert.ToString(NC_Data[0 + d + a / 2, 5]));
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=1");
                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0 + d + a / 2, 0].ToString("f4") + "  " + "B" + NC_Data[0 + d + a / 2, 1].ToString("f4") + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0 + d + a / 2, 4] * 1 / dist).ToString("f4") + "  " + "M213==" + (NC_Data[0 + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                        //t = t ;+ Math.Abs(NC_Data[0, 0]) / 100
                        if (NC_Data[0 + d + a / 2, 0] > 0)
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0 + d + a / 2, 0]) ).ToString("f4") + "-M162/32/96)>100" + ")");
                        else
                            Pmac.SendCommand("WHILE((" + (10000 * Convert.ToDouble(NC_Data[0 + d + a / 2, 0]) ).ToString("f4") + "-M162/32/96)<-100" + ")");
                        Pmac.SendCommand("P10=0");
                        Pmac.SendCommand("DWELL 10");
                        Pmac.SendCommand("ENDWHILE");
                        Pmac.SendCommand("P10=1");
                        Pmac.SendCommand("CLOSE");
//运动到初始加工点
                        Pmac.SendCommand("OPEN PROG 7");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand(Convert.ToString("X" + NC_Data[0 + d + a / 2, 0] + "  " + "B" + NC_Data[0 + d + a / 2, 1] + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0 + d + a / 2, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[0 + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                        Pmac.SendCommand("CLOSE");

                        Pmac.SendCommand("OPEN PROG 8");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("FRAX(X,Z)");
                        Pmac.SendCommand("G90 G01");
                        Pmac.SendCommand(Convert.ToString("X" + (-NC_Data[0 + d + a / 2, 0]) + "  " + "B" + (-NC_Data[0 + d + a / 2, 1]) + "  " + "F" + NC_Data[0, 2].ToString().Trim() + "  " + "M113==" + (NC_Data[0 + d + a / 2, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[0 + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                        Pmac.SendCommand("CLOSE");
                    }

                    //**********************************运动程序代码***********************


                    // wr = new StreamWriter(fs);
                    if (radioButton20.Checked == true)
                    {
                        string status;
                        PMAC.GetResponse(pmacNumber, "P25=" + Convert.ToString(n), out  status);
                        Pmac.SendCommand("OPEN PROG 2");
                        Pmac.SendCommand("CLEAR");
                        Pmac.SendCommand("PSET Z(P50)");//加工程序
                        Pmac.SendCommand("G90 G01 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                        if (comboBox8.SelectedIndex == 0)//工件转动方向
                        {
                            Pmac.SendCommand("M3==1");
                            Pmac.SendCommand("M4==0");
                        }
                        else
                        {
                            Pmac.SendCommand("M3==0");
                            Pmac.SendCommand("M4==1");
                        }
                        if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                        {
                            Pmac.SendCommand("M1==0");

                        }
                        else
                        {
                            Pmac.SendCommand("M1==1");
                        }
                        //wr.WriteLine("P25=0");
                        // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                        for (int i = 0; i < NC_Data.Length / 7; i++)
                        {
                            Pmac.SendCommand(Convert.ToString("X" + NC_Data[i, 0].ToString("f4") + "  " + "B" + NC_Data[i, 1].ToString("f4") + "  " + "Z" + NC_Data[i, 5].ToString("f4") + "  " + "F" + NC_Data[i, 2].ToString("f4") + "  " + "M302==" + NC_Data[i, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i, 4] * 10).ToString("f4") + "  " + "M213==" + (NC_Data[i, 6] * 1000).ToString("f4") + "  " + "\n"));
                            if (i > 0)
                                t = t + Math.Sqrt(Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2) + Math.Pow((NC_Data[i, 0] - NC_Data[i - 1, 0]), 2)) / NC_Data[i, 2];// (Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                        }
                        Pmac.SendCommand("P25=P25-1");
                        // wr.WriteLine("ENDWHILE");
                        Pmac.SendCommand("DWELL 0");
                        Pmac.SendCommand("IF(P25=0)");
                        Pmac.SendCommand("P32=1");
                        Pmac.SendCommand("P14=1");
                        Pmac.SendCommand("ELSE");
                        Pmac.SendCommand("COMMAND \"&1B1R\"");
                        Pmac.SendCommand("M5=0");
                        Pmac.SendCommand("ENDIF");

                        Pmac.SendCommand("CLOSE");
                        // wr.Close();



                    }
                    else
                    {
                        if (n == 1)
                        {
                            Pmac.SendCommand("OPEN PROG 2");
                            Pmac.SendCommand("CLEAR");
                            Pmac.SendCommand("PSET Z(P50)");//加工程序
                            Pmac.SendCommand("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                            //wr.WriteLine("P25=0");

                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                Pmac.SendCommand("M1==0");

                            }
                            else
                            {
                                Pmac.SendCommand("M1==1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3=1");
                                Pmac.SendCommand("M4=0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3=0");
                                Pmac.SendCommand("M4=1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3==1");
                                Pmac.SendCommand("M4==0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3==0");
                                Pmac.SendCommand("M4==1");
                            }
                            Pmac.SendCommand("P26=1");
                            Pmac.SendCommand("P27=1");
                            Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n) + ")");
                             for (int i = 0; i < c+1; i++)
                            {
                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i + d + a / 2, 0].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + NC_Data[i + d + a / 2, 2].ToString("f4") + "  " + "M302==" + NC_Data[i + d + a / 2, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i + d + a / 2, 4]/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + Math.Sqrt(Math.Pow((NC_Data[i + d + a / 2, 0] - NC_Data[i + d + a / 2 - 1, 0]), 2) + Math.Pow((NC_Data[i + d + a / 2, 5] - NC_Data[i + d + a / 2 - 1, 5]), 2)) / NC_Data[i + d + a / 2, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
//此处n=1
                            }
                           // MessageBox.Show()
                            Pmac.SendCommand("P25=P25+1");
                            Pmac.SendCommand("ENDWHILE");
                            Pmac.SendCommand("DWELL 0");
                            Pmac.SendCommand("P14=1");
                            Pmac.SendCommand("P32=1");
                            Pmac.SendCommand("M7=1");
                            Pmac.SendCommand("M113=0 M213=0");
                            Pmac.SendCommand("ENABLE PLC6");
                            Pmac.SendCommand("CLOSE");
                            // wr.Close();

                        }
                        else
                        {
                            Pmac.SendCommand("OPEN PROG 2");
                            Pmac.SendCommand("CLEAR");
                            Pmac.SendCommand("PSET Z(P50)");//加工程序
                            Pmac.SendCommand("G90 G01 P25=0 P32=0 M502==" + Convert.ToString(Convert.ToDouble(textBox33.Text) * U_motor_scale_factor + U_motor_offset));
                            //wr.WriteLine("P25=0");

                            if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                            {
                                Pmac.SendCommand("M1==0");

                            }
                            else
                            {
                                Pmac.SendCommand("M1==1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3=1");
                                Pmac.SendCommand("M4=0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3=0");
                                Pmac.SendCommand("M4=1");
                            }
                            if (comboBox8.SelectedIndex == 0)//工件转动方向
                            {
                                Pmac.SendCommand("M3==1");
                                Pmac.SendCommand("M4==0");
                            }
                            else
                            {
                                Pmac.SendCommand("M3==0");
                                Pmac.SendCommand("M4==1");
                            }

                            Pmac.SendCommand("P26=1");
                            Pmac.SendCommand("P27=0");
                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int i = 0; i < c+1; i++)
                            {

                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i + d + a / 2, 0].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + NC_Data[i + d + a / 2, 2].ToString("f4") + "  " + "M302==" + NC_Data[i + d + a / 2, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i + d + a / 2, 4] /dist).ToString("f4") + "  " + "M213==" + (NC_Data[i + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                                if (i > 0)
                                    t = t + Math.Sqrt(Math.Pow((NC_Data[i + d + a / 2, 0] - NC_Data[i + d + a / 2 - 1, 0]), 2) + Math.Pow((NC_Data[i + d + a / 2, 5] - NC_Data[i + d + a / 2 - 1, 5]), 2)) / NC_Data[i + d + a / 2, 2];//(Math.Abs(NC_Data[i, 0] - NC_Data[i - 1, 0])) / NC_Data[i, 2];
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int i = c+1 - 2; i >= 0; i--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + NC_Data[i + d + a / 2, 0].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + NC_Data[i + d + a / 2 + 1, 2].ToString("f4") + "  " + "M302==" + NC_Data[i + d + a / 2, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i + d + a / 2, 4] * 10).ToString("f4") + "  " + "M213==" + (NC_Data[i + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1");
                            Pmac.SendCommand("ENDWHILE");
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int i = 0; i < c+1; i++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + NC_Data[i + d + a / 2, 0].ToString("f4") + "  " + "B" + NC_Data[i + d + a / 2, 1].ToString("f4") + "  " + "Z" + NC_Data[i + d + a / 2, 5].ToString("f4") + "  " + "F" + NC_Data[i + d + a / 2, 2].ToString("f4") + "  " + "M302==" + NC_Data[i + d + a / 2, 3].ToString("f4") + "  " + "M113==" + (NC_Data[i + d + a / 2, 4] * 1/dist).ToString("f4") + "  " + "M213==" + (NC_Data[i + d + a / 2, 6] * 1000).ToString("f4") + "  " + "\n"));
                                }

                            }
                            Pmac.SendCommand("DWELL 0");
                            Pmac.SendCommand("P14=1");
                            Pmac.SendCommand("P32=1");
                            Pmac.SendCommand("M7=1");
                            Pmac.SendCommand("M113=0 M213=0");
                            Pmac.SendCommand("ENABLE PLC6");
                            Pmac.SendCommand("CLOSE");
                            //wr.Close();

                        }


                    }

                    process_time = (t * n + Math.Abs(NC_Data[d + a / 2, 0]) / first_position_feed) + (B_axi_Center_Polish_head_distance - Math.Abs(NC_Data[d + a / 2, 5])) / z_axi_first_jogdownfeed + 0.05 + caculate_time_offset;//加工总时间

                    timer5.Enabled = false;
                }


                catch (Exception )
                {
                    MessageBox.Show("导入失败！线程安全冲突，请重启软件", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    download_code_flag = false;
                    button47.Text = "导入失败";
                    timer5.Enabled = true;
                    //dectect.Start();
                    return;
                }
              


                    button47.Text = "导入成功";
                    own_code_flag = true;
                    compensate_code_flag = false;
                    other_code_flag = false;
                    download_code_flag = true;
                    // Delay(1);
                    timer5.Enabled = true;
                    process_begin_flag = false;
                    MessageBox.Show("加工时长：" + process_time.ToString("f2") + "分钟");

            }
            #endregion
            #region
            /**************************************多段加工*****************************************/
            else//多段加工
            {

                button47.Text = "导入中...";
                //string pathh = System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt";

                //FileStream stream = File.Open(pathh, FileMode.OpenOrCreate, FileAccess.Write);
                //stream.Seek(0, SeekOrigin.Begin);
                //stream.SetLength(0); //清空txt文件
                //stream.Close();

                //richTextBox2.SaveFile(pathh, RichTextBoxStreamType.PlainText);  //richtexbox
                //Delay(1);
                ///***************************把richbox修改过的替换掉******最初身生成的**************/
                //StreamReader _rstream = null;//为richtext所创建的流
              //  string line;
                string result = "";
               
             //   _rstream = new StreamReader(pathh, System.Text.Encoding.UTF8);
                try
                {
                    double dist;
                    if (comboBox4.SelectedIndex == 0)

                        dist = 0.1;
                    else if (comboBox4.SelectedIndex == 1)
                        dist = 0.01;
                    else
                        dist = 0.1;
                  //  while ((line = _rstream.ReadLine()) != null)
                    //{
                    double[] arry = new double[16];

                    if (textBox40.Text != "")
                        arry[0] = Math.Abs(Convert.ToDouble(textBox40.Text));
                    else
                        arry[0] = 0;

                    if (textBox51.Text != "")
                        arry[1] = Math.Abs(Convert.ToDouble(textBox51.Text));
                    else
                        arry[1] = 0;
                    if (textBox55.Text != "")
                        arry[2] = Math.Abs(Convert.ToDouble(textBox55.Text));
                    else
                        arry[2] = 0;
                    if (textBox54.Text != "")
                        arry[3] = Math.Abs(Convert.ToDouble(textBox54.Text));
                    else
                        arry[3] = 0;
                    if (textBox58.Text != "")
                        arry[4] = Math.Abs(Convert.ToDouble(textBox58.Text));
                    else
                        arry[4] = 0;
                    if (textBox57.Text != "")
                        arry[5] = Math.Abs(Convert.ToDouble(textBox57.Text));
                    else
                        arry[5] = 0;
                    if (textBox61.Text != "")
                        arry[6] = Math.Abs(Convert.ToDouble(textBox61.Text));
                    else
                        arry[6] = 0;
                    if (textBox60.Text != "")
                        arry[7] = Math.Abs(Convert.ToDouble(textBox60.Text));
                    else
                        arry[7] = 0;
                    if (textBox64.Text != "")
                        arry[8] = Math.Abs(Convert.ToDouble(textBox64.Text));
                    else
                        arry[8] = 0;
                    if (textBox63.Text != "")
                        arry[9] = Math.Abs(Convert.ToDouble(textBox63.Text));
                    else
                        arry[9] = 0;
                    if (textBox67.Text != "")
                        arry[10] = Math.Abs(Convert.ToDouble(textBox67.Text));
                    else
                        arry[10] = 0;
                    if (textBox66.Text != "")
                        arry[11] = Math.Abs(Convert.ToDouble(textBox66.Text));
                    else
                        arry[11] = 0;
                    if (textBox73.Text != "")
                        arry[12] = Math.Abs(Convert.ToDouble(textBox73.Text));
                    else
                        arry[12] = 0;
                    if (textBox72.Text != "")
                        arry[13] = Math.Abs(Convert.ToDouble(textBox72.Text));
                    else
                        arry[13] = 0;
                    if (textBox70.Text != "")
                        arry[14] = Math.Abs(Convert.ToDouble(textBox70.Text));
                    else
                        arry[14] = 0;
                    if (textBox69.Text != "")
                        arry[15] = Math.Abs(Convert.ToDouble(textBox69.Text));
                    else
                        arry[15] = 0;
                    Array.Sort(arry);
                    int j = (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2 + 1;
                  //  j = Convert.ToInt16(arry[15] * 20);
                    
                    foreach (string line in richTextBox2.Lines)
                    {



                       // if (j >= 0 & j < NC_Data.Length / 7)
                        if (j >= (NC_Data.Length / 7 - 3 - Convert.ToInt16(arry[15] * 2/dist)) / 2 + 1 & j <= (NC_Data.Length / 7 - 3) / 2 + Convert.ToInt16(arry[15] * 1/dist) + 1)
                     
                        {

                            result = line;
                            int x = result.IndexOf("X");

                            int b = result.IndexOf("B");
                            int z = result.IndexOf("Z");
                            int f = result.IndexOf("F");
                            string str1 = result.Substring(x + 1, b - x - 1);
                            string str3 = result.Substring(b + 1, z - b - 1);
                            string str33 = result.Substring(z + 1, f - z - 1);
                            string str4 = result.Substring(f + 1);

                            // NC_Data[j, 0] = Convert.ToDouble(str1.Trim());
                            NC_Data[j, 1] = Convert.ToDouble(str3.Trim());

                            //if (j == 0)
                            //    NC_Data[j, 2] = Convert.ToDouble(str4.Trim());
                            //else
                            //    NC_Data[j, 2] = Math.Abs(NC_Data[j, 0] - NC_Data[j - 1, 0]) * Convert.ToDouble(str4.Trim()) * 10;

                            if (j == 0)
                                NC_Data[j, 2] = Convert.ToDouble(str4.Trim());
                            else
                                NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 5] - NC_Data[j - 1, 5]), 2)) * Convert.ToDouble(str4.Trim()) * 1/dist;//进给速度X Z联动
                      

                            NC_Data[j, 5] = Convert.ToDouble(str33.Trim());

                            // break;
                        }
                        j++;


                    }
                    //_rstream.Close();

                //    File.Delete(System.Environment.CurrentDirectory + "\\模仁补正加工代码\\richtexbox.txt");
                }

                catch
                {
                    MessageBox.Show("导入失败！请勿删除代码", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button47.Text = "导入补正代码";
                    //_rstream.Close();
                    return;
                }

                double t = 0;//加工一次时间
                /*************************************生成代码**********************************************/


                /***************第一个位置代码**************/

                double n;

                try
                {
                    n = Convert.ToDouble(textBox17.Text);//加工次数
                }
                catch (Exception )
                {
                    MessageBox.Show("导入失败！请先设定加工次数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button47.Text = "导入补正代码";
                    return;
                }

              try
               {




                    t = download_Gcode();

                    process_time = t+caculate_time_offset;
                    timer5.Enabled = false;
               }



               catch (Exception )
               {
                    MessageBox.Show("导入失败！线程安全冲突，请重启软件", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    download_code_flag = false;
                    button47.Text = "导入失败";
                    timer5.Enabled = true;
                    //dectect.Start();
                    return;
                }
             finally
              {


                    button47.Text = "导入成功";
                    own_code_flag = true;
                    compensate_code_flag = false;
                    other_code_flag = false;
                    download_code_flag = true;
                    // Delay(1);
                    timer5.Enabled = true;
                    process_begin_flag = false;
                    MessageBox.Show("加工时长：" + process_time.ToString("f2") + "分钟");

             }




            }

            #endregion


            /******************************************************************/








        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            string status;
            PMAC.GetResponse(pmacNumber, "#1k", out status);
            PMAC.GetResponse(pmacNumber, "#1J/", out status);
           
        }

        private void button49_Click(object sender, EventArgs e)
        {
            string status;
            PMAC.GetResponse(pmacNumber, "#2k", out status);
            PMAC.GetResponse(pmacNumber, "#2J/", out status);
        }

        private void label105_Click(object sender, EventArgs e)
        {
           
        }




        double lagelangri(double[] x, double[] y, double xx, int n)
        {
            int i, j;
            double yy = 0;
            double[] a = new double[n];
            for (i = 0; i <= n - 1; i++)
            {
                a[i] = y[i];
                for (j = 0; j <= n - 1; j++)
                    if (j != i) a[i] *= (xx - x[j]) / (x[i] - x[j]);

                yy += a[i];
            }
            // delete a;
            // delete a;
            return yy;
        }

        private void button51_Click(object sender, EventArgs e)
        {
            string status;
 
                PMAC.GetResponse(0, "M5=1", out status);
                //  PMAC.GetResponse(0, "M0=1", out status);
            
        }

        private void button50_Click(object sender, EventArgs e)
        {
            string status;
            PMAC.GetResponse(0, "M5=0", out status);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            PMAC.Close(pmacNumber);
            //PMAC.SelectDevice(0, out pmacNumber, out selectPmacSuccess);
            PMAC.Open(pmacNumber, out openPmacSuccess);

            // }

            if (openPmacSuccess == true)
            {

                //PMAC.GetResponse(pmacNumber, "j/", out status);//设置闭环
                //设置灰键
                connect_flag = true;
                openPmacSuccess = false;
                MessageBox.Show("连接控制器成功", "提示");

                x_software_limit_left = Pmac.GetI(113);
                x_software_limit_right = Pmac.GetI(114);

                b_software_limit_left = Pmac.GetI(213);
                b_software_limit_right = Pmac.GetI(214);

                z_software_limit_up = Pmac.GetI(414);
                z_software_limit_down = Pmac.GetI(413);

           

            }
            else
            {
                connect_flag = false;
                MessageBox.Show("连接控制器失败", "警报");

            }
        }

        private void button53_Click(object sender, EventArgs e)//生成NC代码界面
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = true;
            this.panel9.Visible = false;
            Pcode_return_flag = true;
            textBox16.Text = textBox39.Text;
        }

        private void SAG_text_TextChanged(object sender, EventArgs e)//矢高数值改变
        {
            button19.Enabled = true;
        }

        private void yuanr_text_TextChanged(object sender, EventArgs e)//圆角半径数值改变触发
        {
            button19.Enabled = true;
        }

        private void gongD_textBox3_TextChanged(object sender, EventArgs e)//工件口径数值改变触发
        {
            button19.Enabled = true;
        }

        private void R_text_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void K_text_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A2_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A4_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A6_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A8_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A10_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A12_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A14_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A16_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void A18_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
          //  MessageBox.Show("hello");
        }

        private void D_text_TextChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void textBox37_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;

            if(comboBox4.SelectedIndex==1&(plane.Checked==true|tu_sphere.Checked==true|ao_sphere.Checked==true))
            {
                MessageBox.Show("平面与球面无法进行0.01数据间隔的加工！","提示！");
                comboBox4.SelectedIndex = 0;

            }
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void button54_Click(object sender, EventArgs e)//警报解除
        {
            //int M_VALUE;
            Pmac.SendCommand("ENABLE PLC1");
            Pmac.SendCommand("I113=0");//X轴左软限位禁用
            Pmac.SendCommand("I114=0");//X轴右软限位禁用
            //Pmac.SendCommand("I213=0");//X轴左软限位禁用
            Pmac.SendCommand("I214=0");//b轴右软限位禁用
            Pmac.SendCommand("I413=0");//Z轴下软限位禁用
            Pmac.SendCommand("I414=0");//Z轴上软限位禁用
            timer12.Enabled = true;//C轴警报清除
            MessageBox.Show("警报清楚成功！","提示！");
            Alarm_clear_flag = true;
            /*if (Z_ALARM_flag == true)
            {
                Pmac.SendCommand("M6=0");
                Pmac.SendCommand("M7=0");
                Pmac.SendCommand("M45=1");
                Delay(1);
                Pmac.SendCommand("M45=0");
                // Pmac.SendCommand("ENABLE PLC 9");               
                Delay(1);
                M_VALUE = Pmac.GetM(8);
                if (M_VALUE == 1)
                {
                    MessageBox.Show("警报解除成功");
                }
                if (M_VALUE == 0)
                {
                    MessageBox.Show("警报解除失败");
                }
            }
            else
            {
                MessageBox.Show("无警报");
 
            }*/
         
            

        }

        private void plane_CheckedChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void tu_sphere_CheckedChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void ao_sphere_CheckedChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void tu_non_sphere_CheckedChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void ao_non_sphere_CheckedChanged(object sender, EventArgs e)
        {
            button19.Enabled = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
            button48.Enabled = false;//生成补正代码按钮
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
            button48.Enabled = true;//生成补正代码按钮
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            button10.Enabled = true;
        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)//标准加工次数
        {
            button12.Text = "导入非补正代码";
        }

        private void textBox17_TextChanged(object sender, EventArgs e)//补正加工次数
        {
            button47.Text = "导入补正代码";
        }

        private void timer7_Tick(object sender, EventArgs e)//加工开始停止时钟，控制抛光轴
        {

            timer7.Interval = 200;
           
            //string myfile = System.Environment.CurrentDirectory + "\\home_value.txt";
            //StreamReader myreader = null;//new StreamReader();
            //myreader = new StreamReader(myfile, System.Text.Encoding.UTF8);
           // string x_pos = myreader.ReadLine();
           // myreader.Close();
            if (Pmac.GetM(302) >0)
            {
               // Pmac.SendCommand("#1j="+x_devia_value.ToString());
                //Delay(2);
                if (textBox33.Text!="")
                SET_POLISH_SPEED(Convert.ToDouble(textBox33.Text));
                else
                    SET_POLISH_SPEED(Convert.ToDouble(textBox14.Text));
                timer7.Enabled = false;
                timer8.Enabled = true;
                //set_zero();
               // MessageBox.Show(Convert.ToString(x_devia_value));
            }

        }

        private void timer8_Tick(object sender, EventArgs e)//模拟量板控制抛光轴速度
        {
            timer8.Interval = 200;
            if (Pmac.GetM(302) == 0)
            {
               // Pmac.SendCommand("#1j=" + x_devia_value.ToString());
                //Delay(2);
                
               // timer7.Enabled = false;
                SET_POLISH_SPEED(0);
                //MessageBox.Show("set_zero");
                //Pmac.SendCommand("&1b4r");
                timer8.Enabled = false;
               // set_zero();                        
               // MessageBox.Show(Convert.ToString(x_devia_value));
            }
        }



void set_zero()
        {
            Delay(5);
            //MessageBox.Show("复位完成");
            Pmac.SendCommand("&1b4r");
                   
        }


        /********************模拟量输板出控制抛光轴****************/

          
public bool OpenSerialPort()
{

    //关闭时点击，则设置好端口，波特率后打开
    try
    {
       // comm.PortName = cbxPortNum.SelectedItem.ToString(); //串口名 COM1
        string[] ports = SerialPort.GetPortNames();
        comm.PortName = ports[0];
        comm.BaudRate = int.Parse("9600"); //波特率  9600
        comm.DataBits = 8; // 数据位 8
        comm.ReadBufferSize = 4096;
        comm.StopBits = StopBits.One;
        comm.Parity = Parity.None;
        comm.Open();
    }
    catch (Exception ex)
    {

        //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
        comm = new SerialPort();
        //现实异常信息给客户。
        MessageBox.Show(ex.Message);
       
        return false;
    }
    return true;
}

        private void DebugInfo(string infotxt, byte[] info, int len = 0)
        {
            string debuginfo;
            StringBuilder builder = new StringBuilder();
            if (info != null)
            {
                if (len == 0) len = info.Length;
                //判断是否是显示为16禁止
                //依次的拼接出16进制字符串
                for (int i = 0; i < len; i++)
                {
                    builder.Append(info[i].ToString("X2") + " ");
                }
            }
            debuginfo = string.Format("{0}:{1}\r\n", infotxt, builder.ToString());
            builder.Clear();
            //因为要访问ui资源，所以需要使用invoke方式同步ui。
            this.Invoke((EventHandler)(delegate
            {
                //追加的形式添加到文本框末端，并滚动到最后。
                //txtBoxRcv.AppendText(debuginfo);
                // if (txtBoxRcv.TextLength > 1000) txtBoxRcv.Clear();
            }));
        }

        //int errrcvcnt = 0;
        private byte[] sendinfo(byte[] info)
        {
            if (comm == null)
            {
                comm = new SerialPort();
                return null;
            }

            if (comm.IsOpen == false)
            {
                OpenSerialPort();
                return null;
            }
            try
            {
                byte[] data = new byte[2048];
                int len = 0;

                comm.Write(info, 0, info.Length);
                DebugInfo("发送", info, info.Length);

                try
                {
                    Thread.Sleep(50);
                    Stream ns = comm.BaseStream;
                    ns.ReadTimeout = 50;
                    len = ns.Read(data, 0, 2048);

                    DebugInfo("接收", data, len);
                }
                catch (Exception)
                {
                    return null;
                }
                //errrcvcnt = 0;
                return analysisRcv(data, len);
            }
            catch (Exception)
            {

            }
            return null;
        }
        private byte[] analysisRcv(byte[] src, int len)
        {
            if (len < 6) return null;
            if (src[0] != Convert.ToInt16("254")) return null;

            switch (src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x02:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x04:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x05:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[4];
                        return dst;
                    }
                    break;
                case 0x0f:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = 1;
                        return dst;
                    }
                    break;
                case 0x06:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;
                case 0x10:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;
            }
            return null;
        }

        private void SET_POLISH_SPEED(double speed)
        {

           
                //打开时点击，则关闭串口
                comm.Close();          
                OpenSerialPort();

            double voltage = speed * 1.1781 + 38.0398;
            if (speed>=0&&speed<1)
                voltage = 0;
            byte[] info = CModbusDll.WriteAOInfo(Convert.ToInt16(254), 0, Convert.ToInt16(voltage));
            byte[] rst = sendinfo(info);
        }

        private void button56_Click_1(object sender, EventArgs e)
        {
            if (Pmac.GetM(5) == 1)
            {
                if (MessageBox.Show("是否进行压力调节操作？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Pmac.SendCommand("ENABLE PLC17");
                }
                else
                    return;
            }
            else
            {
                MessageBox.Show("请先点击“抛光笔下降”按钮令抛光笔下降", "提示！");
            }
           
        }

        private void button57_Click(object sender, EventArgs e)//读取参数
        {


            OpenFileDialog dialog = new OpenFileDialog();
            string InitialDire = System.Environment.CurrentDirectory;
            dialog.Title = "模仁加工参数读取";
            dialog.InitialDirectory = InitialDire + "\\模仁加工参数";
            dialog.Filter = "加工参数文件|*.txt3";
            dialog.ShowDialog();
            string myfile = dialog.FileName;
            //textBox1.Text = Path.GetFileNameWithoutExtension(myfile);
            if (myfile.Trim() == "")
                return;
            StreamReader w = null;//new StreamReader();

           w = new StreamReader(myfile, System.Text.Encoding.UTF8);

            try
            {

                textBox11.Text = w.ReadLine();//次数
                textBox12.Text = w.ReadLine();//次数
                textBox14.Text = w.ReadLine();//抛光轴转速
                textBox13.Text = w.ReadLine();//工件高度
                textBox15.Text = w.ReadLine();//工件转速
                textBox20.Text = w.ReadLine();//夹具高度
                textBox19.Text = w.ReadLine();//速度倍率
                textBox50.Text = w.ReadLine();//矢高
                if (Convert.ToDouble(w.ReadLine()) == 0)
                    comboBox10.SelectedIndex = 0;//抛光顺逆
                else
                    comboBox10.SelectedIndex = 1;//抛光顺逆
                if (Convert.ToDouble(w.ReadLine()) == 0)
                    comboBox11.SelectedIndex = 0;//工件顺逆
                else
                    comboBox11.SelectedIndex = 1;//工件顺逆
                // w.WriteLine(comboBox10.SelectedIndex);//抛光顺逆

                w.Close();

               
            }
            catch (Exception )
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("数据文件格式错误！请检查文件数据格式是否正确。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



         
        }

        private void button58_Click(object sender, EventArgs e)//保存参数
        {
            double[] Parameter = new double[7];
            try
            {
                Parameter = new double[7] {
                    Convert.ToDouble(textBox12.Text),//次数
                    Convert.ToDouble(textBox14.Text), //抛光轴转速
                    Convert.ToDouble(textBox13.Text), //工件高度
                    Convert.ToDouble(textBox15.Text),//工件转速
                    Convert.ToDouble(textBox20.Text), //夹具高度
                    
                    Convert.ToDouble(textBox19.Text), //速度倍率
                     Convert.ToDouble(textBox50.Text), //矢高
                    
               
                };
         

            SaveFileDialog sfd = new SaveFileDialog();
            string InitialDire = System.Environment.CurrentDirectory;
            sfd.Title = "模仁加工参数保存";
            sfd.InitialDirectory = InitialDire + "\\模仁加工参数";
            sfd.Filter = @"加工参数文件|*.txt3";//.csv| *.csv|加工参数文件|.*txt2";
            sfd.ShowDialog();
            //sfd.FileName(textBox1.Text) ;
            string path = sfd.FileName;
            if (path == "")
            {
                return;
            }



            /***************写入TXT********/

            //  path = sfd.FileName;//结果保存

            /*先清空parameter.txt文件内容*/
            FileStream stream2 = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream2.Seek(0, SeekOrigin.Begin);
            stream2.SetLength(0); //清空txt文件
            stream2.Close();
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(textBox11.Text);
            for (int i = 0; i < 7; i++)
            {
                wr.WriteLine(Convert.ToString(Parameter[i]));
            }
            if (comboBox10.SelectedIndex == 0)
                wr.WriteLine("0");//抛光顺逆
            else
                wr.WriteLine("1");//抛光顺逆
            if (comboBox11.SelectedIndex == 0)
                wr.WriteLine("0");//工件顺逆
            else
                wr.WriteLine("1");;//工件顺逆
            wr.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("请输入所有参数再保存", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void button55_Click(object sender, EventArgs e)
        {
            string status;
            PMAC.GetResponse(pmacNumber, "#4k", out status);
            PMAC.GetResponse(pmacNumber, "#4J/", out status);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//加工程序1复选框
        {
            this.textBox40.Enabled = false;
            this.textBox51.Enabled = false;
            this.textBox52.Enabled = false;
            if (checkBox1.Checked == true)
                this.textBox40.Enabled = true;
            if (checkBox1.Checked == true)
                this.textBox51.Enabled = true;
            if(checkBox1.Checked==true)
                this.textBox52.Enabled = true;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//加工程序2复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox55.Enabled = false;
                this.textBox54.Enabled = false;
                this.textBox53.Enabled = false;
                if (checkBox2.Checked == true)
                    this.textBox55.Enabled = true;
                if (checkBox2.Checked == true)
                    this.textBox54.Enabled = true;
                if (checkBox2.Checked == true)
                    this.textBox53.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)//加工程序3复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox58.Enabled = false;
                this.textBox57.Enabled = false;
                this.textBox56.Enabled = false;
                if (checkBox3.Checked == true)
                    this.textBox58.Enabled = true;
                if (checkBox3.Checked == true)
                    this.textBox57.Enabled = true;
                if (checkBox3.Checked == true)
                    this.textBox56.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)//加工程序4复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox61.Enabled = false;
                this.textBox60.Enabled = false;
                this.textBox59.Enabled = false;
                if (checkBox4.Checked == true)
                    this.textBox61.Enabled = true;
                if (checkBox4.Checked == true)
                    this.textBox60.Enabled = true;
                if (checkBox4.Checked == true)
                    this.textBox59.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)//加工程序5复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox64.Enabled = false;
                this.textBox63.Enabled = false;
                this.textBox62.Enabled = false;
                if (checkBox5.Checked == true)
                    this.textBox64.Enabled = true;
                if (checkBox5.Checked == true)
                    this.textBox63.Enabled = true;
                if (checkBox5.Checked == true)
                    this.textBox62.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)//加工程序6复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox67.Enabled = false;
                this.textBox66.Enabled = false;
                this.textBox65.Enabled = false;
                if (checkBox6.Checked == true)
                    this.textBox67.Enabled = true;
                if (checkBox6.Checked == true)
                    this.textBox66.Enabled = true;
                if (checkBox6.Checked == true)
                    this.textBox65.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)//加工程序7复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox73.Enabled = false;
                this.textBox72.Enabled = false;
                this.textBox71.Enabled = false;
                if (checkBox7.Checked == true)
                    this.textBox73.Enabled = true;
                if (checkBox7.Checked == true)
                    this.textBox72.Enabled = true;
                if (checkBox7.Checked == true)
                    this.textBox71.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)//加工程序8复选框
        {
            if (checkBox1.Checked == true)
            {
                this.textBox70.Enabled = false;
                this.textBox69.Enabled = false;
                this.textBox68.Enabled = false;
                if (checkBox8.Checked == true)
                    this.textBox70.Enabled = true;
                if (checkBox8.Checked == true)
                    this.textBox69.Enabled = true;
                if (checkBox8.Checked == true)
                    this.textBox68.Enabled = true;
            }
            else
                MessageBox.Show("请先勾选加工程序1选框！", "提示！");

        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)//多段加工
        {
            //this.button43.Enabled = false;
            if (radioButton22.Checked == true)
                this.button43.Enabled = true;

        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)//d单段程序加工
        {
            //this.button43.Enabled = false;

            if (radioButton21.Checked == true)
                this.button43.Enabled = false;
        }

        private void button43_Click_2(object sender, EventArgs e)//多程序设定
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = true;

            if (multi_process_flag == false)
            {
                textBox40.Enabled = false;
                textBox51.Enabled = false;
                textBox52.Enabled = false;
                textBox55.Enabled = false;
                textBox54.Enabled = false;
                textBox53.Enabled = false;
                textBox58.Enabled = false;
                textBox57.Enabled = false;
                textBox56.Enabled = false;
                textBox61.Enabled = false;
                textBox60.Enabled = false;
                textBox59.Enabled = false;
                textBox64.Enabled = false;
                textBox63.Enabled = false;
                textBox62.Enabled = false;
                textBox67.Enabled = false;
                textBox66.Enabled = false;
                textBox65.Enabled = false;
                textBox73.Enabled = false;
                textBox72.Enabled = false;
                textBox71.Enabled = false;
                textBox70.Enabled = false;
                textBox69.Enabled = false;
                textBox68.Enabled = false;
                textBox40.Text = "0";
                textBox51.Text = "0";
                textBox52.Text = "0";
                textBox55.Text = "0";
                textBox54.Text = "0";
                textBox53.Text = "0";
                textBox58.Text = "0";
                textBox57.Text = "0";
                textBox56.Text = "0";
                textBox61.Text = "0";
                textBox60.Text = "0";
                textBox59.Text = "0";
                textBox64.Text = "0";
                textBox63.Text = "0";
                textBox62.Text = "0";
                textBox67.Text = "0";
                textBox66.Text = "0";
                textBox65.Text = "0";
                textBox73.Text = "0";
                textBox72.Text = "0";
                textBox71.Text = "0";
                textBox70.Text = "0";
                textBox69.Text = "0";
                textBox68.Text = "0";
                multi_process_flag = true;
                
            }
            multi_process_flag2 = true;
        }

        private void button13_Click_1(object sender, EventArgs e)//多段程序设定完成
        {
            if (multi_process_flag2 == true)
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = false;
                this.panel5.Visible = false;
                this.panel6.Visible = false;
                this.panel7.Visible = true;
                this.panel8.Visible = false;
                this.panel9.Visible = false;
                multi_process_flag2 = false;
            }
            else
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                this.panel5.Visible = false;
                this.panel6.Visible = false;
                this.panel7.Visible = false;
                this.panel8.Visible = false;
                this.panel9.Visible = false;
 
            }
           
        }

        private void timer9_Tick(object sender, EventArgs e)//抛光加工执行时开始监控
        {
            timer9.Interval = 100;
            if (Pmac.GetM(302) > 100)
            {
                if (Pmac.GetM(3) == 0 && Pmac.GetM(4) == 0)
                {
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3=1");
                        Pmac.SendCommand("M4=0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3=0");
                        Pmac.SendCommand("M4=1");
                    }

                }
                if (Pmac.GetM(3) == 1 && Pmac.GetM(4) == 1)
                {
                    if (comboBox11.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3=1");
                        Pmac.SendCommand("M4=0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3=0");
                        Pmac.SendCommand("M4=1");
                    }

                }
            }
            if (Pmac.GetM(502) > 10)
            {
                if (Pmac.GetM(1) == 0 && comboBox10.SelectedIndex == 1)
                {
                    if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SendCommand("M1=0");

                    }
                    else
                    {
                        Pmac.SendCommand("M1=1");
                    }

                }
                if (Pmac.GetM(1) == 1 && comboBox10.SelectedIndex == 0)
                {
                    if (comboBox10.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SendCommand("M1=0");

                    }
                    else
                    {
                        Pmac.SendCommand("M1=1");
                    }

                }

                if (Pmac.GetM(5) == 0)//监控气缸
                {

                    Pmac.SendCommand("M5=1");

                }
            }

        }

        private void timer10_Tick(object sender, EventArgs e)//不加工时监测抛光轴转动状态
        {
            timer10.Interval = 200;
           int polish_stop= Pmac.GetM(0);
           if (polish_stop == 0)
               Pmac.SetM(0, 1);

        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)//多段程序加工选择按钮变化
        {
            button59.Visible = true;//多段加工设置
            //textBox36.Enabled = false;//单端加工范围文本框变灰
            textBox6.Enabled = false;//单端加工范围文本框变灰
            textBox18.Enabled = false;//单端加工范围文本框变灰
            //textBox18.Enabled = false;//单端加工范围文本框变灰
            textBox17.Enabled = false;//单端加工次数文本框变灰

        }

        private void radioButton23_CheckedChanged(object sender, EventArgs e)//单段程序加工选择按钮变化
        {
            button59.Visible = false;//单段加工设置
            //textBox36.Enabled = true;//单端加工范围文本框变正常
            textBox6.Enabled = true;//单端加工范围文本框变正常
            textBox18.Enabled = true;//单端加工范围文本框变正常
           // textBox18.Enabled = true;//单端加工范围文本框变正常
            textBox17.Enabled = true;//单端加工次数文本框变正常
        }

        private void button59_Click(object sender, EventArgs e)//多程序加工设置按钮
        {
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            this.panel6.Visible = false;
            this.panel7.Visible = false;
            this.panel8.Visible = false;
            this.panel9.Visible = true;
            if (multi_process_flag == false)
            {
                textBox40.Enabled = false;
                textBox51.Enabled = false;
                textBox52.Enabled = false;
                textBox55.Enabled = false;
                textBox54.Enabled = false;
                textBox53.Enabled = false;
                textBox58.Enabled = false;
                textBox57.Enabled = false;
                textBox56.Enabled = false;
                textBox61.Enabled = false;
                textBox60.Enabled = false;
                textBox59.Enabled = false;
                textBox64.Enabled = false;
                textBox63.Enabled = false;
                textBox62.Enabled = false;
                textBox67.Enabled = false;
                textBox66.Enabled = false;
                textBox65.Enabled = false;
                textBox73.Enabled = false;
                textBox72.Enabled = false;
                textBox71.Enabled = false;
                textBox70.Enabled = false;
                textBox69.Enabled = false;
                textBox68.Enabled = false;
                textBox40.Text = "0";
                textBox51.Text = "0";
                textBox52.Text = "0";
                textBox55.Text = "0";
                textBox54.Text = "0";
                textBox53.Text = "0";
                textBox58.Text = "0";
                textBox57.Text = "0";
                textBox56.Text = "0";
                textBox61.Text = "0";
                textBox60.Text = "0";
                textBox59.Text = "0";
                textBox64.Text = "0";
                textBox63.Text = "0";
                textBox62.Text = "0";
                textBox67.Text = "0";
                textBox66.Text = "0";
                textBox65.Text = "0";
                textBox73.Text = "0";
                textBox72.Text = "0";
                textBox71.Text = "0";
                textBox70.Text = "0";
                textBox69.Text = "0";
                textBox68.Text = "0";
                multi_process_flag = true;


            }
        }



        public double download_Gcode()
    {
            double n;//加工次数
             double max_z=0,t=0,t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0;//各个程序段的运行时间
            double xend1 = 0, xend2 = 0, xend3 = 0, xend4 = 0, xend5 = 0, xend6 = 0, xend7 = 0;//各个程序段结束加工X
            double xstart2 = 0, xstart3 = 0, xstart4 = 0, xstart5 = 0, xstart6 = 0, xstart7 = 0, xstart8 = 0;//各个程序段开始加工X
            double zend1 = 0, zend2 = 0, zend3 = 0, zend4 = 0,zend5 = 0, zend6 = 0, zend7 = 0;//各个程序段结束加工Z
            double zstart2 = 0, zstart3 = 0, zstart4 = 0, zstart5 = 0, zstart6 = 0, zstart7 = 0, zstart8 = 0;//各个程序段开始加工Z
            double speed=Convert.ToDouble(textBox33.Text);//抛光轴速度
             double [,] data,data2;
            data=new double[NC_Data.Length/7+2,5];
            data2=new double[NC_Data.Length/7+2,5];
            //int number=data.Length/5;
            double dist;//间距

            if (comboBox4.SelectedIndex == 0)
                dist = 0.1;
            else if (comboBox4.SelectedIndex == 1)
                dist = 0.01;
            else
                dist = 0.1;

            for(int i=0;i<NC_Data.Length/7;i++)
            {
                data[i+2,0]=-NC_Data[i,4];//X
                data[i + 2, 1] = -NC_Data[i, 6];//Y
                data[i + 2, 2] = NC_Data[i, 1];//B
                data[i + 2, 4] = NC_Data[i, 5];//Z
                data2[i + 2, 3] = NC_Data[i, 2];//F
                data2[i + 2, 0] = NC_Data[i, 0];//X加工坐标

                if (NC_Data[i, 5] > max_z)
                    max_z = NC_Data[i, 5];
            }
    
                    if (String.IsNullOrWhiteSpace(textBox40.Text))
                    {
                        MessageBox.Show("请先设定多段程序加工范围！！", "提示！");
                        button21.Text = "读取其它NC";
                        Pmac.SendCommand("CLOSE");
                        return 0;
                    }
                    Pmac.SendCommand("OPEN PROG 1");//初始位置
                    Pmac.SendCommand("CLEAR");
                    // wr.WriteLine("S150");
                    Pmac.SendCommand("DISABLE PLC6");
                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");
                    Pmac.SendCommand("P26=1");
                    Pmac.SendCommand("P27=1");
                    Pmac.SendCommand(("P50=" + Convert.ToString(data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text) * 2/dist))) / 2, 4])));

                    Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));
                    t1 = t1 + Math.Abs(data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0] / data2[2, 3]);
                    Pmac.SendCommand("WHILE(ABS(" + (10000 * data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0]).ToString("f4") + "-M162/32/96)>100" + ")");
                    Pmac.SendCommand("P10=0");
                    Pmac.SendCommand("DWELL 10");
                    Pmac.SendCommand("ENDWHILE");
                    Pmac.SendCommand("P10=1");
                    Pmac.SendCommand("CLOSE");


            //运动到加工初始位
                    Pmac.SendCommand("OPEN PROG 7");//初始位置
                    Pmac.SendCommand("CLEAR");

                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");

                    Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 2].ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));
                   
                    Pmac.SendCommand("CLOSE");

                    Pmac.SendCommand("OPEN PROG 8");//初始位置
                    Pmac.SendCommand("CLEAR");

                    Pmac.SendCommand("FRAX(X,Z)");
                    Pmac.SendCommand("G90 G01");

                    Pmac.SendCommand(Convert.ToString("X" + (-data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0]).ToString("f4") + "  " + "B" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 2]).ToString("f4") + "  " + "F" + data2[2, 3].ToString("f4") + "  " + "M113==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 0] * 10).ToString("f4") + "M213==" + (-data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(Convert.ToDouble(textBox40.Text)) * 2 / dist)) / 2, 1] * 1000).ToString("f4") + "  " + "\n"));

                    Pmac.SendCommand("CLOSE");
                    // 加工代码       

                    Pmac.SendCommand("OPEN PROG 2");//加工程序
                    Pmac.SendCommand("CLEAR");
                    Pmac.SendCommand("PSET Z(P50)");//加工程序
                    Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                    Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                    // Pmac.SendCommand("FRAX(X)");
                    Pmac.SendCommand("G90 G01 P25=0 P32=0");
                    //wr.WriteLine("G90 G01 P25=0 P32=0");
                    // Pmac.SendCommand("M0=0");

                    if (comboBox9.SelectedIndex == 0)//抛光轴转动方向
                    {
                        Pmac.SendCommand("M1=0");

                    }
                    else
                    {
                        Pmac.SendCommand("M1=1");
                    }
                    if (comboBox8.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3=1");
                        Pmac.SendCommand("M4=0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3=0");
                        Pmac.SendCommand("M4=1");
                    }
                    if (comboBox8.SelectedIndex == 0)//工件转动方向
                    {
                        Pmac.SendCommand("M3==1");
                        Pmac.SendCommand("M4==0");
                    }
                    else
                    {
                        Pmac.SendCommand("M3==0");
                        Pmac.SendCommand("M4==1");
                    }


 /*********************************************************************************************************************/
                    string z_feed = "F"+zfeed.ToString();//Z轴上升和下降速度
                    // zfeed = 120;//Z轴上升和下降速度
                    // xBfeed = 240;//X B到程序段位置速度
                    string X_B_feed = "F"+xBfeed.ToString();//X B到程序段位置速度
                  
                    if (Math.Abs(Convert.ToDouble(textBox40.Text)) == 0 && Math.Abs(Convert.ToDouble(textBox40.Text)) == 0 && Math.Abs(Convert.ToDouble(textBox40.Text)) == 0)
                    {
                        MessageBox.Show("请先设定多段程序加工范围！！", "提示！");
                        button21.Text = "读取其它NC";
                        Pmac.SendCommand("CLOSE");
                        return 0;
                    }
                  
                    //程序段1
                    if (checkBox1.Checked == true)//程序段1
                    {
                        n = Convert.ToDouble(textBox52.Text);
                        if(n==0)
                         n = 1;
                        double begin_point = Convert.ToDouble(textBox40.Text), end_point = Convert.ToDouble(textBox51.Text);//程序加工初始位置和程序加工结束位置
                       
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序1加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("P26=1");
                        Pmac.SendCommand("P27=0");
                           if (n <= 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist); l++)
                            { }
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist); l++)
                          
                            {
                            
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2)
                                    t1 = t1 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];

                               
                                  //  NC_Data[j, 2] = Math.Sqrt(Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2) + Math.Pow((NC_Data[j, 0] - NC_Data[j - 1, 0]), 2)) * Convert.ToDouble(str4.Trim()) * 10;//进给速度X Z联动
                      
                              }
                            xend1 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist), 0];
                            zend1 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist), 4];
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 2 / dist)) / 2; l++)
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t1 = t1 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                          //  int j = 0;
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 2/dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2/dist)) / 2; l--)//代码倒转
                            {
                                //j++;
                                //if(l)
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l-1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l-1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l-1, 1] * 1000).ToString("f4") + "  " + "\n"));
                        
                               // Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[ 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                               // Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l -, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] * 1000).ToString("f4") + "  " + "\n"));
                           
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");

                            xend1 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2, 0];
                            zend1 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2, 4];

                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point * 2 / dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] * 10).ToString("f4") + "  " + "M213==" + (-data[l, 1] * 1000).ToString("f4") + "  " + "\n"));
                                }

                                xend1 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist), 0];
                                zend1 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point * 1 / dist), 4];

                            }
                        }
                           t1 = t1 * n + (B_axi_Center_Polish_head_distance - Math.Abs(data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) * 2 / dist)) / 2, 4])) / z_axi_first_jogdownfeed + 0.05;//程序1加工时间,抛光时间+z轴下降时间+气缸松开时间
                        Pmac.SendCommand("DWELL 0");

                    }
  /*********************************************************************************************************************/


                    if (checkBox2.Checked == true)//程序段2
                    {
                        n = Convert.ToDouble(textBox53.Text);
                     
                        if (n <1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox55.Text), end_point = Convert.ToDouble(textBox54.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序2加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }

                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4")+z_feed+" "+"DWELL 10");                       
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed +" " +"DWELL 10" + "  " + "\n"));
                       //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=2 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart2 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart2 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
                   
               
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t2 = t2 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                             xend2 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                             zend2 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                                     }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t2 = t2 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");

                            xend2 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend2 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend2 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend2 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                            }
                        }
                        t2 = t2 * n + Math.Abs(max_z + 30 - zend1) / zfeed + Math.Abs(xstart2 - xend1) / xBfeed + Math.Abs(zstart2 - max_z - 30) / zfeed;
                        Pmac.SendCommand("DWELL 0");
                    }

    /*********************************************************************************************************************/


                    if (checkBox3.Checked == true)//程序段3
                    {
                        n = Convert.ToDouble(textBox56.Text);
                       
                        if (n <1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox58.Text), end_point = Convert.ToDouble(textBox57.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序3加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }

                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=3 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");

                        xstart3 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart3 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
                   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t3 = t3 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            xend3 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                            zend3 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t3 = t3 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend3 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend3 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend3 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend3 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                            }
                        }
                        t3 = t3 * n + Math.Abs(max_z + 30 - zend2) / zfeed + Math.Abs(xstart3 - xend2) / xBfeed + Math.Abs(zstart3 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

  /*********************************************************************************************************************/

                    if (checkBox4.Checked == true)//程序段4
                    {
                        n = Convert.ToDouble(textBox59.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox61.Text), end_point = Convert.ToDouble(textBox60.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序4加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=4 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");

                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart4 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart4 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t4 = t4 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            xend4 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                            zend4 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t4 = t4 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend4 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend4 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend4 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend4 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                            }
                        }
                        t4 = t4 * n + Math.Abs(max_z + 30 - zend4) / zfeed + Math.Abs(xstart4 - xend3) / xBfeed + Math.Abs(zstart4 - max_z - 30) / zfeed;
                        
                        Pmac.SendCommand("DWELL 0");
                    }


                    /*********************************************************************************************************************/


                    if (checkBox5.Checked == true)//程序段5
                    {
                        n = Convert.ToDouble(textBox62.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox64.Text), end_point = Convert.ToDouble(textBox63.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序5加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=5 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart5 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart5 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t5 = t5 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            xend5 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                            zend5 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t5 = t5 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend5 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend5 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend5 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend5 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                            }
                        }
                        t5 = t5 * n + Math.Abs(max_z + 30 - zend4) / zfeed + Math.Abs(xstart5 - xend4) / xBfeed + Math.Abs(zstart5 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }


                    /*********************************************************************************************************************/


                    if (checkBox6.Checked == true)//程序段6
                    {
                        n = Convert.ToDouble(textBox65.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox67.Text), end_point = Convert.ToDouble(textBox66.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序6加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=6 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart6 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart6 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t6 = t6 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            } 
                            xend6 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                            zend6 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t6 = t6 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend6 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend6 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend6 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend6 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                            }
                        }
                        t6 = t6 * n + Math.Abs(max_z + 30 - zend6) / zfeed + Math.Abs(xstart6 - xend5) / xBfeed + Math.Abs(zstart6 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

   /*********************************************************************************************************************/


                    if (checkBox7.Checked == true)//程序段7
                    {
                        n = Convert.ToDouble(textBox71.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox73.Text), end_point = Convert.ToDouble(textBox72.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序7加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=7 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart7 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart7 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t7 = t7 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            xend7 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                            zend7 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t7 = t7 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            xend7 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                            zend7 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
    
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }
                                xend7 = data2[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 0];
                                zend7 = data[2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist), 4];
                   
                            }
                        }
                        t7 = t7 * n + Math.Abs(max_z + 30 - zend6) / zfeed + Math.Abs(xstart7 - xend6) / xBfeed + Math.Abs(zstart7 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }

                    /*********************************************************************************************************************/


                    if (checkBox8.Checked == true)//程序段8
                    {
                        n = Convert.ToDouble(textBox68.Text);
                        if (n < 1)
                            n = 1;
                        double begin_point = Convert.ToDouble(textBox70.Text), end_point = Convert.ToDouble(textBox69.Text);//加工开始位置和加工结束位置
                        if (Math.Abs(begin_point) > Math.Abs(data[2, 0]))
                        {
                            MessageBox.Show("程序8加工设定范围过大！！", "提示！");
                            button21.Text = "读取其它NC";
                            Pmac.SendCommand("CLOSE");
                            return 0;
                        }
                        Pmac.SendCommand("M302=10");
                        Pmac.SendCommand("M502=1");
                        Pmac.SendCommand("M0==1");
                        //Z轴下降
                        Pmac.SendCommand("Z" + (max_z + 30).ToString("f4") + z_feed + " " + "DWELL 10");
                        //X轴和B轴摆到初始点
                        Pmac.SendCommand(Convert.ToString("X" + data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0].ToString("f4") + "  " + "B" + data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 2].ToString("f4") + " " + X_B_feed + " " + "DWELL 10" + "  " + "\n"));
                        //Z轴上升到初始加工点
                        Pmac.SendCommand("Z" + (data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4]).ToString("f4") + z_feed + " " + "DWELL 10");
                        Pmac.SendCommand("P26=8 DWELL 0");
                        Pmac.SendCommand("P27=0 DWELL 0");
                        Pmac.SendCommand("P25=0 DWELL 0");
                        Pmac.SendCommand("M302=" + Convert.ToString(Convert.ToDouble(textBox2.Text) * C_motor_scale_factor + C_motor_offset));
                        Pmac.SendCommand("M502=" + Convert.ToString(speed * U_motor_scale_factor + U_motor_offset));
                        Pmac.SendCommand("M0==0");
                        xstart8 = data2[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 0];
                        zstart8 = data[2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2, 4];
   
                        if (n == 1)
                        {
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            // wr.WriteLine("WHILE(P25<" + Convert.ToString(n) + ")");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3) / 2 + Convert.ToInt16(end_point *1 /dist); l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t8 = t8 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];//Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                        }
                        else
                        {

                            if (n % 2 == 1)
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString((n - 1) / 2) + ")");
                            else
                                Pmac.SendCommand("WHILE(P25<" + Convert.ToString(n / 2) + ")");
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                            {
                                if (l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2)
                                    t8 = t8 + Math.Sqrt(Math.Pow(data2[l, 0] - data2[l - 1, 0], 2) + Math.Pow(data[l, 4] - data[l - 1, 4], 2)) / data2[l, 3];// Math.Abs(data2[l, 0] - data2[l - 1, 0]) / data2[l, 3];
                                Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P27=P27+1 DWELL 0");
                            for (int l = 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l > 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l--)//代码倒转
                            {
                                Pmac.SendCommand(Convert.ToString("X" + data2[l - 1, 0].ToString("f4") + "  " + "B" + data[l - 1, 2].ToString("f4") + "  " + "Z" + data[l - 1, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l - 1, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l - 1, 1] *1000).ToString("f4") + "  " + "\n"));
                            }
                            Pmac.SendCommand("P25=P25+1 ");
                            Pmac.SendCommand("ENDWHILE");
                            if (n % 2 == 1)
                            {
                                Pmac.SendCommand("P27=P27+1 DWELL 0");
                                for (int l = 2 + (data2.Length / 5 - 3 - Convert.ToInt16(Math.Abs(begin_point) *2 /dist)) / 2; l <= 2 + (data2.Length / 5 - 3 + Convert.ToInt16(end_point *2 /dist)) / 2; l++)
                                {
                                    Pmac.SendCommand(Convert.ToString("X" + data2[l, 0].ToString("f4") + "  " + "B" + data[l, 2].ToString("f4") + "  " + "Z" + data[l, 4].ToString("f4") + "  " + "F" + data2[l, 3].ToString("f4") + "  " + "M113==" + (-data[l, 0] *1 /dist).ToString("f4") + "  " + "M213==" + (-data[l, 1] *1000).ToString("f4") + "  " + "\n"));
                                }

                            }
                        }
                        t8 = t8 * n + Math.Abs(max_z + 30 - zend7) / zfeed + Math.Abs(xstart8 - xend7) / xBfeed + Math.Abs(zstart8 - max_z - 30) / zfeed;
                       
                        Pmac.SendCommand("DWELL 0");
                    }



                    Pmac.SendCommand("DWELL 0");
                    Pmac.SendCommand("P14=1");
                    Pmac.SendCommand("P32=1");
                    Pmac.SendCommand("M7=1");
                    Pmac.SendCommand("M113=0 M213=0");
                    Pmac.SendCommand("ENABLE PLC6");
                    Pmac.SendCommand("CLOSE");

                    t = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;//加工时间和

                    return t;
                }

        private void button60_Click(object sender, EventArgs e)//移动到初始加工点
        {
            if (MessageBox.Show("是否移动到模仁左边初始加工点？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Pmac.SendCommand("&1b7r");
            }
            else
                return;
          
        }

        private void button61_Click(object sender, EventArgs e)//移动到初始加工点
        {
            if (MessageBox.Show("是否移动到模仁右边初始加工点？", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Pmac.SendCommand("&1b8r");
            }
            else
                return;

        }



        private void timer11_Tick(object sender, EventArgs e)//蜂鸣器响
        {


            /***新蜂鸣器，通电后滴滴响***/
            timer11.Interval = 3000;
            if (buzzer_work == true)
            {
                Pmac.SetM(6, 1);
                buzzer_work = false;

            }

            else
            {
                Pmac.SetM(6, 0);
                // buzzer_work = false;
                timer11.Enabled = false;
                // buzzer_worktime--;

            }
/********************************************/



            /********************旧蜂鸣器，电后一直响************************/
            //timer11.Interval = 800;
            //if (buzzer_work == false)
            //{
            //    Pmac.SetM(6, 1);
            //    buzzer_work = true;
            //}

            //if (buzzer_work == true)
            //{
            //    Pmac.SetM(6, 0);
            //    buzzer_work = false;
            //}
            //if (buzzer_worktime >= 3)
            //{
            //    timer11.Enabled = false;
            //    Pmac.SetM(6, 0);
            //    buzzer_worktime = 0;
            //    buzzer_work = false;
            //}
            //buzzer_worktime++;

            /********************************************/

            
         //   buzzer_work = true;
            //Pmac.SetM(6, 1);

            //if(buzzer_worktime>=1)
            //{
            //    timer11.Enabled = false;
            //    Pmac.SetM(6, 0);
            //    buzzer_worktime = 0;
            //    buzzer_work = false;
               
            //}
           // buzzer_worktime = 0;
            

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void textBox36_MouseLeave(object sender, EventArgs e)//加工范围
        {
           // MessageBox.Show("h");
        }

        private void textBox36_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  MessageBox.Show("h");
        }

        private void textBox36_ModifiedChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("h");
        }

        private void textBox36_Leave(object sender, EventArgs e)
        {
            if(textBox6.Text!="")
            {
                if (Convert.ToDouble(textBox6.Text) > 22)
                    MessageBox.Show("加工范围半径不能超过22mm !", "提示！");
            }
           
        }

        private void textBox14_MouseLeave(object sender, EventArgs e)//抛光轴转速范围
        {
          
          

        }

        private void textBox14_Leave(object sender, EventArgs e)//工件转速范围
        {
            double c_axi_speed;
            if (textBox14.Text != "")
            {
                c_axi_speed = Convert.ToDouble(textBox14.Text.Trim());
                if (c_axi_speed > C_axi_maxfeed)
                    MessageBox.Show( "工件转速设定过大！","提示！");
                if (c_axi_speed < C_axi_minspeed)
                {

                    MessageBox.Show("工件转速设定过小！","提示！");

                }
            }
        }

        private void textBox15_Leave(object sender, EventArgs e)//抛光轴转速范围
        {
            double polish_axi_speed;
            if (textBox15.Text != "")
            {
                polish_axi_speed = Convert.ToDouble(textBox15.Text.Trim());
                if (polish_axi_speed > polish_axi_maxfeed)
                    MessageBox.Show("抛光轴转速设定过大！", "提示！");
                if (polish_axi_speed < polish_axi_minfeed)
                {
                    MessageBox.Show("抛光轴转速设定过小！", "提示！");
                }
            }

        }

        private void textBox33_Leave(object sender, EventArgs e)//抛光轴转速范围
        {
            double polish_axi_speed;
            if (textBox33.Text != "")
            {
                polish_axi_speed = Convert.ToDouble(textBox33.Text.Trim());
                if (polish_axi_speed > polish_axi_maxfeed)
                    MessageBox.Show("抛光轴转速设定过大！", "提示！");
                if (polish_axi_speed < polish_axi_minfeed)
                {
                    MessageBox.Show("抛光轴转速设定过小！", "提示！");
                }
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)//工件转速范围
        {
            double c_axi_speed;
            if (textBox2.Text != "")
            {
                c_axi_speed = Convert.ToDouble(textBox2.Text.Trim());
                if (c_axi_speed > C_axi_maxfeed)
                    MessageBox.Show("工件转速设定过大！", "提示！");
                if (c_axi_speed < C_axi_minspeed)
                {
                    MessageBox.Show("工件转速设定过小！", "提示！");
                }
            }

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            double c_axi_speed;
            if (textBox3.Text != "")
            {
                c_axi_speed = Convert.ToDouble(textBox3.Text.Trim());
                if (c_axi_speed > C_axi_maxfeed)
                    MessageBox.Show("工件转速设定过大！", "提示！");
                if (c_axi_speed < C_axi_minspeed)
                {
                    MessageBox.Show("工件转速设定过小！", "提示！");
                }
            }
        }

        private void textBox40_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox40.Text != "")
                LeftPosition = Convert.ToDouble(textBox40.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!","提示！");
                                                                     
                                                                

                
        }

        private void textBox55_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox55.Text != "")
                LeftPosition = Convert.ToDouble(textBox55.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示！");
                                                                      
                                                                

        }

        private void textBox58_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox58.Text != "")
                LeftPosition = Convert.ToDouble(textBox58.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示！");
                                                                      
                                                                

        }

        private void textBox61_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox61.Text != "")
                LeftPosition = Convert.ToDouble(textBox61.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示!");
                                                                      
                                                                

        }

        private void textBox64_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox64.Text != "")
                LeftPosition = Convert.ToDouble(textBox64.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示!");
                                                                     
                                                                

        }

        private void textBox67_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox67.Text != "")
                LeftPosition = Convert.ToDouble(textBox67.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示!");
                                                                       
                                                                

        }

        private void textBox73_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox73.Text != "")
                LeftPosition = Convert.ToDouble(textBox73.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示!");
                                                                   

        }

        private void textBox70_Leave(object sender, EventArgs e)
        {
            double LeftPosition;

            if (textBox70.Text != "")
                LeftPosition = Convert.ToDouble(textBox70.Text.Trim());
            else
                return;
            if (LeftPosition > 0)
                MessageBox.Show("开始位置应设定为负值!", "提示!");
                                                                       
                                                                

        }

        private void textBox46_Leave(object sender, EventArgs e)
        {
            double D_Workpiece;

            if (textBox46.Text != "")
                D_Workpiece = Convert.ToDouble(textBox46.Text.Trim());
            else
                return;
            if (D_Workpiece > 62)
                MessageBox.Show("工件直径不得大于62mm!", "提示!");

        }

        private void gongD_textBox3_Leave(object sender, EventArgs e)
        {

            double Dp_Workpiece;
            if (gongD_textBox3.Text != "")
                Dp_Workpiece = Convert.ToDouble(gongD_textBox3.Text.Trim());
            else
                return;
            if (Dp_Workpiece > 62)
                MessageBox.Show("工件口径不得大于62mm!", "提示!");
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)//加工范围左位置
        {
            button10.Enabled = true;
        }

        private void textBox18_TextChanged_1(object sender, EventArgs e)//加工范围右位置
        {
            button10.Enabled = true;
        }

        private void timer12_Tick(object sender, EventArgs e)//C轴清楚警报
        {

            Pmac.SetM(3, 0);
            Pmac.SetM(4, 0);
            timer12.Interval = 800;
            if (resetC_work == false)
            {
                Pmac.SetM(2, 1);
                resetC_work = true;
            }

            if (resetC_work == true)
            {
                Pmac.SetM(2, 0);
                resetC_work = false;
            }
            if (resetC_worktime >= 3)
            {
                timer12.Enabled = false;
                Pmac.SetM(2, 0);
                resetC_worktime = 0;
                resetC_work = false;
            }
            resetC_worktime++;

        }

        private void button62_Click(object sender, EventArgs e)//干涉检查
        {
            double ao_tu;
            try {   
            double R = Convert.ToDouble(R_text.Text.Trim());
            if (tu_non_sphere.Checked == true || tu_sphere.Checked == true)
            {
                if (R > 0)
                     ao_tu = -1;
                else
                     ao_tu = 1;
               
            }
            else if (ao_sphere.Checked == true || ao_non_sphere.Checked == true)
            {
                if (R > 0)
                    ao_tu = 1;
                else
                    ao_tu = -1;
                
            }
            else
            {
                MessageBox.Show(this, "平面抛光不需校验");
                return;
            }
            
             
                intefere_check_paramenter = new double[27] {ao_tu,Convert.ToDouble(yuanr_text.Text), Convert.ToDouble(gongD_textBox3.Text), Convert.ToDouble(SAG_text.Text), Convert.ToDouble(TextBox35.Text), Convert.ToDouble(R_text.Text), Convert.ToDouble(K_text.Text), Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
            
                intere_A= new double[20]{ Convert.ToDouble(A1.Text), Convert.ToDouble(A2.Text), Convert.ToDouble(A3.Text), Convert.ToDouble(A4.Text), Convert.ToDouble(A5.Text), Convert.ToDouble(A6.Text), Convert.ToDouble(A7.Text), Convert.ToDouble(A8.Text), Convert.ToDouble(A9.Text), Convert.ToDouble(A10.Text), Convert.ToDouble(A11.Text), Convert.ToDouble(A12.Text), Convert.ToDouble(A13.Text), Convert.ToDouble(A14.Text), Convert.ToDouble(A15.Text), Convert.ToDouble(A16.Text), Convert.ToDouble(A17.Text), Convert.ToDouble(A18.Text), Convert.ToDouble(A19.Text), Convert.ToDouble(A20.Text) };
     
            }
            catch(Exception err)
            {
                MessageBox.Show(this,err.Message);
                return;
            }
           

            form3= new Form3();
          //  form3.Show();
            form3.ShowDialog();



        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)//导入外部速率文件
        {
            if (checkBox10.Checked == true)
                button63.Visible = true;
            else
                button63.Visible = false;

        }

        private void button63_Click(object sender, EventArgs e)//导入补偿倍率文件
        { 
            

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "倍率文件读取";
            dialog.InitialDirectory = System.Environment.CurrentDirectory+"\\补偿文件";
            dialog.Filter = @"倍率文件(.csv)|*.csv";
         //   dialog.Filter = @".csv|*.csv|.MOD|*.MOD";
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (fileName.Trim() == "")
                return;

            Compesent_rate = new List<double>();
            Compesent_rate.Clear();
            input_compesentfile_flag = false;
           //

           // textBox1.Text = Path.GetFileNameWithoutExtension(fileName);
            
          //  textBox49.Text = Path.GetFileNameWithoutExtension(fileName);
           // textBox48.Text = Path.GetFileNameWithoutExtension(fileName);
            //文件打开
            //string fileName = @"C:\test.csv";
            StreamReader reader = new StreamReader(fileName);
            //文件内容保存用变量
            string line, line1, line2;
         //   double[] xx = new double[1000000];
        //    double[] zzd = new double[1000000];
          //  double[] num = new double[10000000];
            //读取一行数据到数组
          
                int i = 0, j = 0;
                int start = 1;
                bool x_value_flag = true;//判断是读哪行为横坐标
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {



                        if (i > 0)
                        {
                            int fist = line.IndexOf(',', start);
                            int second = line.IndexOf(',', fist + 1);

                            //  int third = line.IndexOf(',', second + 1);
                            // textBox1.Text = Convert.ToString(fist);
                            // textBox2.Text = Convert.ToString(second);

                            //if (i == 2)
                            //{
                            //    //  double a = Convert.ToDouble(line.Substring(fist+1, second-fist-1));
                            //    //  double b = Math.Abs(Convert.ToDouble(line.Substring(0, fist)));

                            //    if (Math.Abs(Convert.ToDouble(line.Substring(0, fist))) < Math.Abs(Convert.ToDouble(line.Substring(fist + 1, second - fist - 1))))
                            //        x_value_flag = false;
                            //}

                            //if (x_value_flag == true)
                            //    line1 = line.Substring(0, fist);
                            //else
                            //    line1 = line.Substring(fist + 1, second - fist - 1);

                            if (second > 1)
                                line2 = line.Substring(fist + 1, second - fist - 1);
                            else
                                line2 = line.Substring(fist + 1);
                            //    xx[j] = Convert.ToDouble(line1);
                            //   zzd[j] = Convert.ToDouble(line2);
                            Compesent_rate.Add(Convert.ToDouble(line2));

                            j++;

                            //  string[] sArray = line.Split(line, ",", RegexOptions.IgnoreCase);

                            // this.listBox1.Items.Add(line1);
                            //this.listBox2.Items.Add(line2);
                        }
                        i++;
                    }
                    if ((line = reader.ReadLine()) == null)
                    {
                        MessageBox.Show("导入成功!", "信息提示!");
                        input_compesentfile_flag = true;
                    }
                }
            catch( Exception err)
            {
                MessageBox.Show(err.Message);
            }

         //   string strConn;
         ////    string file_name;
         // //  file_name = "system_value.xlsx";
         //   OpenFileDialog dialog = new OpenFileDialog();
         //   string InitialDire = System.Environment.CurrentDirectory;                     
         //   dialog.Title = "读取补偿文件";
         //   dialog.InitialDirectory = InitialDire + "\\补偿文件";
         //   dialog.Filter = "补偿文件|*.xlsx";
         //   dialog.ShowDialog();
         //   string file_name = dialog.FileName;
         //   //textBox1.Text = Path.GetFileNameWithoutExtension(myfile);
         //   if (file_name.Trim() == "")
         //       return;

         //  Compesent_rate=new List<double>();
         //   Compesent_rate.Clear();

         //   // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + System.Environment.CurrentDirectory + "\\data.xlsx" + ";Extended Properties=Excel 8.0;";
         //   strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + System.Environment.CurrentDirectory + "\\" + file_name + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";
         //   if (Path.GetExtension(System.Environment.CurrentDirectory + "\\" + file_name).Trim().ToUpper() == ".XLSX")
         //   {
         //       strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.Environment.CurrentDirectory + "\\" + file_name + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
         //   }
         //   OleDbConnection conn = new OleDbConnection(strConn);
         //   OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
         //   DataSet myDataSet = new DataSet();
         //   conn.Open();
         //   DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);


         //   try
         //   {
         //       myCommand.Fill(myDataSet);
         //       for(int i=0;i<301;i++)
         //       {
         //            Compesent_rate.Add(Convert.ToDouble(myDataSet.Tables[0].Rows[i][1]));
         //       }
               

         //       //Lworkpiece_h = Convert.ToDouble(myDataSet.Tables[0].Rows[0][1]); //为L型件底面到中心孔高度，单位mm/
         //       //other_h = Convert.ToDouble(myDataSet.Tables[0].Rows[1][1]);//L型件底面到夹具底面高度，单位mm/
         //       //C_motor_scale_factor = Convert.ToDouble(myDataSet.Tables[0].Rows[2][1]);//C轴电压转速比例因子
         //       //C_motor_offset = Convert.ToDouble(myDataSet.Tables[0].Rows[3][1]);//C轴电压转速比例一次变换偏差      
         //       //U_motor_scale_factor = Convert.ToDouble(myDataSet.Tables[0].Rows[4][1]);//抛光轴电压转速比例因子
         //       //U_motor_offset = Convert.ToDouble(myDataSet.Tables[0].Rows[5][1]);//抛光轴电压转速比例一次变换偏差
               
         //   }
         //   catch (Exception err)
         //   {
         //       MessageBox.Show(err.Message);
         //     //  return;
         //   }
         //   // myDataSet.Tables[0].
           

            /************************************************************************************/

          

        }

     

     
 

            
    






        /***************************************************************/





     

        

      /*  public void dective()
        {
            // string status1,status2,status3,status4,status5,status6;
            
            //PMAC.GetResponse(pmacNumber, "#1J/", out status);
            timer5.Interval = 200;
            int X_ALARM, B_ALARM, C_ALARM, Z_ALARM, X_P_LIMIT, B_P_LIMIT, X_N_LIMIT, B_N_LIMIT, Z_P_LIMIT, Z_N_LIMIT;
            double finish_flag, pmac_connect_status;
            bool status;
            while (true)
            {
                //Delay(1);
                X_ALARM = Pmac.GetM(143);//X轴警报
                B_ALARM = Pmac.GetM(243);//B轴警报
                C_ALARM = Pmac.GetM(343);//C轴警报
                X_P_LIMIT = Pmac.GetM(121);//X正限位
                B_P_LIMIT = Pmac.GetM(221);//B正限位
                X_N_LIMIT = Pmac.GetM(122);//X负限位
                B_N_LIMIT = Pmac.GetM(222);//B负限位
                finish_flag = Pmac.GetP(32);//加工完成
                pmac_connect_status = Pmac.GetP(33);//询问是否连接上
                // MessageBox.Show(finish_flag.ToString());
                Z_P_LIMIT = Pmac.GetM(15);//Z上限位
                Z_N_LIMIT = Pmac.GetM(13);//Z下限位
                Z_ALARM = Pmac.GetM(12);//Z轴警报
                //  PMAC.GetResponseEx()
                if (pmac_connect_status == 1)
                {
                    pmac_connect_status = 0;
                }
                else
                {
                    //  PMAC.Open(pmacNumber, out status);
                }
                if (finish_flag == 1)
                {
                    process_fininsh_flag = true;
                    button46.Enabled = true;
                    Pmac.SetP(32, 0);
                    MessageBox.Show("加工完成");
                }
                if (X_ALARM == 1 && tip1 == false)
                {
                    tip1 = true;
                    MessageBox.Show("X轴驱动器警报");

                }

                if (B_ALARM == 1 && tip2 == false)
                {
                    tip2 = true;
                    MessageBox.Show("B轴驱动器警报");

                }
                if (X_P_LIMIT == 1 && tip3 == false)
                {
                    tip3 = true;
                    MessageBox.Show("X轴左限位触发");

                }
                if (B_P_LIMIT == 1 && tip4 == false)
                {
                    tip4 = true;
                    MessageBox.Show("B轴左限位触发");

                }
                if (X_N_LIMIT == 1 && tip5 == false)
                {
                    tip5 = true;
                    MessageBox.Show("X轴右限位触发");

                }
                if (B_N_LIMIT == 1 && tip6 == false)
                {
                    tip6 = true;
                    MessageBox.Show("B轴右限位触发");

                }
                if (Z_N_LIMIT == 0 && tip7 == false && connect_flag == true)
                {
                    tip7 = true;
                    MessageBox.Show("Z轴下限位触发");

                }
                if (Z_P_LIMIT == 0 && tip8 == false && connect_flag == true)
                {
                    tip8 = true;
                    MessageBox.Show("Z轴上限位触发");

                }
                if (Z_ALARM == 1 && tip9 == false)
                {
                    tip9 = true;
                    MessageBox.Show("Z轴警报");

                }
                if (C_ALARM == 1 && tip10 == false)
                {
                    tip10 = true;
                    MessageBox.Show("C轴警报");

                }
            }
 
        }*/

       
        




/*
        class Lagrange
        {
            /// <summary>
            /// X各点坐标组成的数组
            /// </summary>
            public double[] x { get; set; }

            /// <summary>
            /// X各点对应的Y坐标值组成的数组
            /// </summary>
            public double[] y { get; set; }

            /// <summary>
            /// x数组或者y数组中元素的个数, 注意两个数组中的元素个数需要一样
            /// </summary>
            public int itemNum { get; set; }

            /// <summary>
            /// 初始化拉格朗日插值
            /// </summary>
            /// <param name="x">X各点坐标组成的数组</param>
            /// <param name="y">X各点对应的Y坐标值组成的数组</param>
            public Lagrange(double[] x, double[] y)
            {
                this.x = x; this.y = y;
                this.itemNum = x.Length;
            }

            /// <summary>
            /// 获得某个横坐标对应的Y坐标值
            /// </summary>
            /// <param name="xValue">x坐标值</param>
            /// <returns></returns>
            public double GetValue(double xValue)
            {
                //用于累乘数组始末下标
                int start, end;
                //返回值
                double value = 0.0;
                //如果初始的离散点为空, 返回0
                if (itemNum < 1) { return value; }
                //如果初始的离散点只有1个, 返回该点对应的Y值
                if (itemNum == 1) { value = y[0]; return value; }
                //如果初始的离散点只有2个, 进行线性插值并返回插值
                if (itemNum == 2)
                {
                    value = (y[0] * (xValue - x[1]) - y[1] * (xValue - x[0])) / (x[0] - x[1]);
                    return value;
                }
                //如果插值点小于第一个点X坐标, 取数组前3个点做插值
                if (xValue <= x[1]) { start = 0; end = 2; }
                //如果插值点大于等于最后一个点X坐标, 取数组最后3个点做插值
                else if (xValue >= x[itemNum - 2]) { start = itemNum - 3; end = itemNum - 1; }
                //除了上述的一些特殊情况, 通常情况如下
                else
                {
                    start = 1; end = itemNum;
                    int temp;
                    //使用二分法决定选择哪三个点做插值
                    while ((end - start) != 1)
                    {
                        temp = (start + end) / 2;
                        if (xValue < x[temp - 1])
                            end = temp;
                        else
                            start = temp;
                    }
                    start--; end--;
                    //看插值点跟哪个点比较靠近
                    if (Math.Abs(xValue - x[start]) < Math.Abs(xValue - x[end]))
                        start--;
                    else
                        end++;
                }
                //这时已经确定了取哪三个点做插值, 第一个点为x[start]
                double valueTemp;
                //注意是二次的插值公式
                for (int i = start; i <= end; i++)
                {
                    valueTemp = 1.0;
                    for (int j = start; j <= end; j++)
                        if (j != i)
                            valueTemp *= (double)(xValue - x[j]) / (double)(x[i] - x[j]);
                    value += valueTemp * y[i];
                }
                return value;
            }

        }*/

      


       

      

       

      











       

       

        

       

    }
}
