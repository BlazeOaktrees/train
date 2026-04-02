using Modbus.Device;
using System.IO.Ports;
using System.Net.Sockets;
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


namespace WpfModbusTestApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient tcpClient;
        ModbusMaster master;
        string sIp = "127.0.0.1";
        bool bConnect = false;
        private SerialPort serialPort;
        private IModbusSerialMaster masterSerial;  

        private string portName = "COM19";     
        private int baudRate = 9600;
        bool bConnectSerial = false;
        public MainWindow()
        {
            InitializeComponent();
            int[] iWrite = new int[10];
            for (int i = 0; i < iWrite.Length; i++)
            {
                iWrite[i] = i + 3;
            }
            tbWrite.Text = string.Join(',',iWrite);
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!bConnect)
                {
                    tcpClient = new TcpClient(sIp, 502);
                    master = ModbusIpMaster.CreateIp(tcpClient);
                    master.Transport.ReadTimeout = 1000;
                    master.Transport.WriteTimeout = 1000;
                    master.Transport.Retries = 3;
                    bConnect = true;
                    MessageBox.Show("Modbus TCP 连接成功！");
                }
                else
                {
                    bConnect = false;
                    master.Dispose();
                    master = null;
                    tcpClient.Close();
                    tcpClient.Dispose();
                    tcpClient = null;
                }
                btConnect.Content = bConnect ? "Disconnect" : "Connect";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败：{ex.Message}");
            }
        }

        private void btWrite_Click(object sender, RoutedEventArgs e)
        {
            string[] s = tbWrite.Text.Split(',');
            int iLen = s.Length;
            ushort[] iWrite = new ushort[iLen];
            for (int i = 0; i < iLen; i++)
            {
                iWrite[i] = ushort.Parse(s[i]);
            }

            //master.WriteSingleRegister(1, 5, 9999);
            master.WriteMultipleRegisters(1, 0, iWrite);
        }

        private void btRead_Click(object sender, RoutedEventArgs e)
        {
            ushort[] registers = master.ReadHoldingRegisters(1, 0, 10);  // Slave ID=1, 起始地址=0, 数量=10
            lReadData.Content = string.Join(",", registers);
        }

        private void btConnectSerial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!bConnectSerial)
                {
                    serialPort = new SerialPort(portName, baudRate);
                    serialPort.DataBits = 8;
                    serialPort.Parity = Parity.None;      // 根据设备改：None / Even / Odd
                    serialPort.StopBits = StopBits.One;   // 通常是 One
                    serialPort.ReadTimeout = 2000;
                    serialPort.WriteTimeout = 2000;
                    serialPort.Open();

                    // 创建 Modbus RTU Master
                    masterSerial = ModbusSerialMaster.CreateRtu(serialPort);

                    // 可选设置
                    masterSerial.Transport.ReadTimeout = 2000;
                    masterSerial.Transport.WriteTimeout = 2000;
                    masterSerial.Transport.Retries = 3;
                    bConnectSerial = true;
                    MessageBox.Show($"串口 {portName} 连接成功！波特率 {baudRate}");
                }
                else
                {
                    if (masterSerial != null)
                    {
                        masterSerial.Dispose();
                        masterSerial = null;
                    }
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
                    }
                    bConnectSerial = false;
                }
                btConnectSerial.Content = bConnectSerial? "Disconnect" : "Connect";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"串口连接失败：{ex.Message}");
            }
        }

        private void btReadSerial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (masterSerial == null || !serialPort.IsOpen)
                {
                    MessageBox.Show("请先连接串口！");
                    return;
                }

                byte slaveId = 1;                    // Slave ID，通常是 1
                ushort startAddress = 0;             // 起始地址（根据你的 Slave 修改）
                ushort numberOfPoints = 10;          // 读取数量

                ushort[] registers = masterSerial.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);

                string result = "";
                for (int i = 0; i < registers.Length; i++)
                {
                    result += $"地址 {startAddress + i} = {registers[i]},";
                }
                //MessageBox.Show(result);
                lReadData.Content = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取失败：{ex.Message}");
            }
        }

        private void btWriteSerial_Click(object sender, RoutedEventArgs e)
        {
            byte slaveId = 1;
            ushort address = 5;      // 要写入的地址
            ushort value = 1234;     // 要写入的值

            //masterSerial.WriteSingleRegister(slaveId, address, value);
            string[] s = tbWrite.Text.Split(',');
            int iLen = s.Length;
            ushort[] iWrite = new ushort[iLen];
            for (int i = 0; i < iLen; i++)
            {
                iWrite[i] = ushort.Parse(s[i]);
            }

            //master.WriteSingleRegister(1, 5, 9999);
            masterSerial.WriteMultipleRegisters(1, 0, iWrite);
        }
    }
}