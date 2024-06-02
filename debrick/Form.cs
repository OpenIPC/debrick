using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;


namespace debrick
{
    public partial class Form : System.Windows.Forms.Form
    {
        readonly ushort[] crctable = {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
            0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
            0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6,
            0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
            0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485,
            0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
            0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4,
            0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
            0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823,
            0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
            0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12,
            0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
            0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41,
            0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
            0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70,
            0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
            0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f,
            0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e,
            0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d,
            0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c,
            0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
            0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab,
            0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
            0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a,
            0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
            0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9,
            0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
            0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8,
            0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0,
        };
        readonly byte[] ddrstep = {
            0x04,0xe0,0x2d,0xe5,0x24,0x00,0x9f,0xe5,
            0x24,0x10,0x9f,0xe5,0x00,0x10,0x80,0xe5,
            0x20,0x00,0x9f,0xe5,0x20,0x10,0x9f,0xe5,
            0x04,0x10,0x80,0xe4,0x00,0xe0,0x80,0xe5,
            0x04,0xf0,0x9d,0xe4,0xef,0xbe,0xad,0xde,
            0xef,0xbe,0xad,0xde,0xef,0xbe,0xad,0xde,
            0x3c,0x01,0x02,0x12,0x78,0x56,0x34,0x12,
            0x40,0x01,0x02,0x12,0x75,0x6a,0x69,0x7a
        };

        enum OPCODE : byte
        {
            OC_RRQ = 1,
            OC_WRQ,
            OC_DATA,
            OC_ACK,
            OC_ERROR,
            OC_OPTION

        };

        struct FLASH_FILE
        {
            public string name;
            public int loadA;
            public int sizeE;
            public byte[] data;
        }

        readonly List<FLASH_FILE> files = new();
        byte[] tBuf = new byte[1];
        int tBuf_len = 0;
        const int TFTP_PAYLOAD_SIZE = 1468;
        Socket? socket = null;
        EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
        string cmd = "";
        SerialPort serialPort = new();
        bool run;
        string flashInfo = "";
        int flashSize = 0;
        int fi = 0;
        int stage = 0;
        Thread? threadBootLoad = null;
        Thread? threadServerTFTP = null;
        Thread? threadSendCmd = null;
        const int SP_MAX_DATA_LEN = 0x400;
        const int SP_BOOT_SPL_LEN = SP_MAX_DATA_LEN * 0x10;
        const int TIMEOUT = 3;
        readonly string dirApp;
        byte[]? ubootFile = null;
        string serverIP = "192.168.1.2";
        string cameraIP = "192.168.1.123";

        public Form()
        {
            InitializeComponent();
            dirApp = Path.GetDirectoryName(Application.ExecutablePath) ?? "";
            button0.Text = "Yes";
            button0.Left = 300;
            button0.Visible = true;
            Refresh();
        }

        private void ThreadServerTFTP()
        {
            if (socket == null) return;
            int n;
            byte[] dRx = new byte[4096];
            while (run)
            {
                n = 0;
                try
                {
                    n = socket.ReceiveFrom(dRx, ref clientEP);
                }
                catch { }
                if (n < 4)
                {
                    continue;
                }
                if (dRx[1] == (byte)OPCODE.OC_RRQ || dRx[1] == (byte)OPCODE.OC_WRQ)
                {
                    bool isSend = dRx[1] == (byte)OPCODE.OC_RRQ;
                    int i = 2;
                    while (i < n && dRx[i] != 0) i++;
                    i++;
                    while (i < n && dRx[i] != 0) i++;
                    i++;
                    int l = n - i;
                    byte[] dTx = new byte[2 + l];
                    dTx[0] = dRx[0];
                    dTx[1] = (byte)OPCODE.OC_OPTION;
                    System.Array.Copy(dRx, i, dTx, 2, l);
                    n = 0;
                    try
                    {
                        n = socket.SendTo(dTx, clientEP);
                    }
                    catch { }
                    if (n != 0)
                    {
                        if (isSend)
                        {
                            SendBuf();
                        }
                        else
                        {
                            ReceiveBuf();
                        }
                    }
                }
            }
        }

        private void ReceiveBuf()
        {
            if (socket == null) return;
            bool end = false;
            ushort bE = 1;
            int n;
            byte[] dRx = new byte[4096];
            while (!end)
            {
                n = 0;
                try
                {
                    n = socket.ReceiveFrom(dRx, ref clientEP);
                }
                catch { }
                if (n < 4 || dRx[1] != (byte)OPCODE.OC_DATA)
                {
                    return;
                }
                ushort bN = (ushort)((dRx[2] << 8) | (dRx[3] & 0xff));
                if (bE == bN)
                {
                    int payloadLen = n - 4;
                    Array.Copy(dRx, 4, tBuf, tBuf_len, payloadLen);
                    tBuf_len += payloadLen;
                    if (payloadLen < TFTP_PAYLOAD_SIZE) end = true;
                }
                else if (bE < bN)
                {
                    return;
                }
                bE = (ushort)(bN + 1);
                byte[] dTx = new byte[4];
                Array.Copy(dRx, 0, dTx, 0, 4);
                dTx[1] = (byte)OPCODE.OC_ACK;
                n = 0;
                try
                {
                    n = socket.SendTo(dTx, clientEP);
                }
                catch { }
                if (n == 0) return;
            }
        }

        private void SendBuf()
        {
            if (socket == null) return;
            bool end = false;
            int n;
            byte[] dRx = new byte[4096];
            while (!end)
            {
                try
                {
                    n = socket.ReceiveFrom(dRx, ref clientEP);
                }
                catch
                {
                    return;
                }
                if (n < 4 || dRx[1] != (byte)OPCODE.OC_ACK)
                {
                    return;
                }
                ushort bN = (ushort)((dRx[2] << 8) | (dRx[3] & 0xff));
                int i = bN * TFTP_PAYLOAD_SIZE;
                bN += 1;
                int payloadLen = 0;
                if (i < tBuf.Length)
                {
                    payloadLen = tBuf.Length - i;
                    if (payloadLen > TFTP_PAYLOAD_SIZE)
                    {
                        payloadLen = TFTP_PAYLOAD_SIZE;
                    }
                }
                byte[] dTx = new byte[4 + payloadLen];
                dTx[0] = dRx[0];
                dTx[1] = (byte)OPCODE.OC_DATA;
                dTx[2] = (byte)(bN >> 8);
                dTx[3] = (byte)(bN);
                if (payloadLen != 0)
                {
                    System.Array.Copy(tBuf, i, dTx, 4, payloadLen);
                }
                else
                {
                    end = true;
                }
                n = 0;
                try
                {
                    n = socket.SendTo(dTx, clientEP);
                }
                catch { }
                if (n == 0) return;
            }
        }

        private void FileGet()
        {
            tBuf = new byte[flashSize];
            tBuf_len = 0;
            cmd = "mw.b 0x42000000 0xff 0x" + flashSize.ToString("x") +
                   "; sf probe 0; sf read 0x42000000 0x0 0x" + flashSize.ToString("x") +
                   "; tftpput 0x42000000 0x" + flashSize.ToString("x") + " file\n";
        }

        private void FilePut()
        {
            tBuf = files[fi].data;
            cmd = "mw.b 0x42000000 0xff 0x" + flashSize.ToString("x") +
                  "; tftp 0x42000000 file \n";
        }

        private void ShowSelectAction()
        {
            label.Text = "Note! Before \"burn\" make sure\nyou have \"dump\" flash.\n\nAll files (*.bin,*.img) must be located\nin the directory with the executable file.\n\nSelect action:\n\n";
            textBoxIP.Visible = false;
            checkedListBox.Visible = false;
            progressBar.Visible = false;
            button2.Text = "Dump";
            button2.Left = 100;
            button1.Text = "Burn bin";
            button1.Left = 300;
            button0.Text = "Burn img";
            button0.Left = 500;
            button0.Enabled = true;
            button2.Visible = true;
            button1.Visible = true;
            button0.Visible = true;
            Refresh();
        }

        private void ShowSelectPort()
        {
            progressBar.Visible = false;
            RefreshComList();
            checkedListBox.Visible = true;
            label.Text = "Select COM Port:";
            button2.Visible = false;
            button1.Text = "Refresh";
            button1.Left = 200;
            button1.Visible = true;
            button0.Text = "Listen";
            button0.Left = 400;
            button0.Visible = true;
            Refresh();
        }

        private void ShowSelectSoc()
        {
            progressBar.Visible = false;
            checkedListBox.Items.Clear();
            checkedListBox.Items.Add("gk7205v200/210");
            checkedListBox.Items.Add("gk7205v300");
            checkedListBox.SetItemChecked(0, true);
            checkedListBox.Visible = true;
            label.Text = "Select SoC:";
            button0.Text = "OK";
            button0.Left = 300;
            button0.Enabled = true;
            button0.Visible = true;
            Refresh();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "Yes")
            {
                ShowSelectSoc();
            }
            else if (btn.Text == "Refresh")
            {
                if (label.Text.Contains("Port"))
                {
                    RefreshComList();
                }
                else
                {
                    RefreshFilesList();
                }
            }
            else if (btn.Text == "Cancel")
            {
                if (label.Text.StartsWith("Listening"))
                {
                    run = false;
                    threadBootLoad?.Join();
                    serialPort?.Close();
                    ShowSelectPort();
                }
                else
                {
                    ShowSelectAction();
                }
            }
            else if (btn.Text == "Start")
            {
                cameraIP = textBoxIP.Text;
                textBoxIP.Visible = false;
                button0.Visible = false;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                try
                {
                    socket.Bind(new IPEndPoint(IPAddress.Parse(serverIP), 69));
                    socket.ReceiveTimeout = 3000;
                    threadServerTFTP = new Thread(ThreadServerTFTP);
                    threadServerTFTP.Start();
                    threadSendCmd?.Join();
                    cmd = "setenv ipaddr " + cameraIP + "; setenv serverip " + serverIP + ";\n";
                    threadSendCmd = new Thread(ThreadSendCmd);
                    threadSendCmd.Start();
                    ShowSelectAction();
                }
                catch
                {
                    ShowFinish("Fail server start!\nThe port busy. Try again");
                }
            }
            else if (btn.Text == "Dump")
            {
                button0.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                threadSendCmd?.Join();
                FileGet();
                SetStage(1);
                threadSendCmd = new Thread(ThreadSendCmd);
                threadSendCmd.Start();
            }
            else if (btn.Text == "OK")
            {
                if (label.Text.StartsWith("Dump") || label.Text.StartsWith("Burn"))
                {
                    ShowSelectAction();
                }
                else if (label.Text.StartsWith("Fail"))
                {
                    if (label.Text.Contains("server"))
                    {
                        ShowServerStart();
                    }
                    else if (label.Text.Contains("serial port"))
                    {
                        ShowSelectPort();
                    }
                    else
                    {
                        ShowSelectSoc();
                    }
                }
                else
                {
                    if (label.Text.Contains("SoC"))
                    {
                        string? soc = checkedListBox.CheckedItems[0].ToString();
                        if (soc == null || soc.Contains("v2"))
                        {
                            ubootFile = Properties.Resources.gk7205v2;
                        }
                        else
                        {
                            ubootFile = Properties.Resources.gk7205v3;
                        }
                    }
                    ShowSelectPort();
                }
            }
            else if (btn.Text == "Listen")
            {
                if (ubootFile == null) return;
                button1.Visible = false;
                checkedListBox.Visible = false;
                label.Text = "Listening on a port " + checkedListBox.CheckedItems[0].ToString();
                label.Text += "\nTurn off->on the camera power";
                button0.Text = "Cancel";
                button0.Left = 300;
                progressBar.Value = 0;
                progressBar.Maximum = SP_BOOT_SPL_LEN / SP_MAX_DATA_LEN + (ubootFile.Length + SP_MAX_DATA_LEN + 1) / SP_MAX_DATA_LEN + 1;
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.Visible = true;
                Refresh();
                try
                {
                    serialPort = new SerialPort(checkedListBox.CheckedItems[0].ToString(), 115200, Parity.None, 8, StopBits.One);
                    serialPort.Open();
                    run = true;
                    threadBootLoad = new Thread(ThreadBootLoad);
                    threadBootLoad.Start();
                }
                catch
                {
                    ShowFinish("Fail serial port! Unable to open a port.\nReset the equipment and try again");
                }
            }
            else if (btn.Text.StartsWith("Burn "))
            {
                if (btn.Text.Contains("bin"))
                {
                    label.Text = "Select file bin:";
                }
                else
                {
                    label.Text = "Select files img:";
                }
                RefreshFilesList();
                checkedListBox.Visible = true;
                button2.Text = "Cancel";
                button2.Left = 100;
                button2.Visible = true;
                button1.Text = "Refresh";
                button1.Left = 300;
                button1.Visible = true;
                button0.Text = "Burn";
                button0.Left = 500;
                button0.Enabled = false;
                button0.Visible = true;
                Refresh();
            }
            else if (btn.Text == "Burn")
            {
                files.Clear();
                List<string> checkedItems = new();
                foreach (var item in checkedListBox.CheckedItems)
                {
                    string? filename = item.ToString();
                    if (filename == null) return;
                    FLASH_FILE bf;
                    bf.name = filename;
                    byte[] data = File.ReadAllBytes(filename);
                    if (label.Text.Contains("bin"))
                    {
                        bf.data = data;
                        bf.loadA = 0;
                        bf.sizeE = flashSize;
                        files.Add(bf);
                        break;
                    }
                    else
                    {
                        bf.data = new byte[data.Length - 0x40];
                        Array.Copy(data, 0x40, bf.data, 0, bf.data.Length);
                        bf.loadA = Htonl(data, 0x10);
                        bf.sizeE = Htonl(data, 0x14) - bf.loadA;
                        files.Add(bf);
                    }
                }
                if (files.Count > 0)
                {
                    checkedListBox.Visible = false;
                    button0.Visible = false;
                    button1.Visible = false;
                    button2.Visible = false;
                    threadSendCmd?.Join();
                    fi = 0;
                    FilePut();
                    SetStage(3);
                    threadSendCmd = new Thread(ThreadSendCmd);
                    threadSendCmd.Start();
                }
            }
        }

        private static int Htonl(byte[] d, int o) => (d[o + 0] << (8 * 3)) | (d[o + 1] << (8 * 2)) | (d[o + 2] << (8 * 1)) | (d[o + 3]);

        private void RefreshComList()
        {
            checkedListBox.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                checkedListBox.Items.Add(ports[i]);
            }
            if (checkedListBox.Items.Count > 0)
            {
                checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, true);
                button0.Enabled = true;
            }
            else
            {
                button0.Enabled = false;
            }
        }

        private void RefreshFilesList()
        {
            checkedListBox.Items.Clear();
            string ex = "*.img";
            if (label.Text.Contains("bin"))
            {
                ex = "*.bin";
            }
            string[] files = Directory.GetFiles(dirApp, ex);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInf = new(files[i]);
                if (fileInf.Length <= flashSize)
                {
                    string name = fileInf.Name;
                    checkedListBox.Items.Add(name);
                }
            }
            button0.Enabled = false;
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!label.Text.Contains("files"))
            {
                for (int i = 0; i < checkedListBox.Items.Count; ++i)
                {
                    if (i != e.Index) checkedListBox.SetItemChecked(i, false);
                }
            }
            List<string> checkedItems = new();
            string? str;
            foreach (var item in checkedListBox.CheckedItems)
            {
                str = item.ToString();
                checkedItems.Add(str ?? "");
            }
            str = checkedListBox.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
                checkedItems.Add(str ?? "");
            else
                checkedItems.Remove(str ?? "");

            if (checkedItems.Count > 0)
            {
                button0.Enabled = true;
            }
            else
            {
                button0.Enabled = false;
            }
        }

        private void ThreadBootLoad()
        {
            int cnt = 0;
            byte[] b = new byte[1];
            serialPort.ReadTimeout = 500;
            while (run && cnt < 5)
            {
                if (Read(b))
                {
                    if (b[0] == 0x20) cnt++; else cnt = 0;
                }
            }
            if (!run) return;
            try
            {
                Invoke(ShowLoading);
                Invoke(ProgressStage, -1);
            }
            catch { return; }
            b[0] = 0xaa;
            if (!Write(b, 1)) goto FAIL;
            if (!Send_ddrstep()) goto FAIL;
            if (!Send_SPL()) goto FAIL;
            Thread.Sleep(100);
            if (!Send_uboot()) goto FAIL;
            b[0] = 0x03;
            for (int i = 0; i < 10; i++)
            {
                Write(b, 1);
            }
            Thread.Sleep(1000);
            b[0] = 0x0A;
            Write(b, 1);
            flashInfo = "";
            flashSize = 0;
            int n = serialPort.BytesToRead;
            if (n > 0)
            {
                b = new byte[n];
                serialPort.Read(b, 0, b.Length);
                string info = Encoding.ASCII.GetString(b);
                string find = "Flash Name: ";
                int iS = info.IndexOf(find);
                if (iS != -1)
                {
                    iS += find.Length;
                    info = info[iS..];
                    char[] chars = info.ToCharArray();
                    iS = 0;
                    int iC = 0;
                    while (chars[iC] != '{') iC++;
                    string name = new(chars, iS, iC - iS);
                    while (chars[iC] != ',') iC++;
                    while (chars[iC] != '0') iC++;
                    iS = iC;
                    while (chars[iC] != '.') iC++;
                    string size = new(chars, iS, iC - iS);
                    flashSize = Convert.ToInt32(size, 16);
                    flashInfo = "Flash:" + name + "(" + flashSize / 0x100000 + "MB)";
                }
            }
            if (flashInfo == "" || flashSize == 0)
            {
                run = false;
                serialPort?.Close();
                try
                {
                    Invoke(ShowFinish, "Fail SoC!\nMaybe the wrong choice.\nTry again");
                }
                catch { return; }
            }
            else
            {
                serverIP = GetLocalIPAddress();
                try
                {
                    Invoke(ShowServerStart);
                }
                catch { return; }
            }
            return;
        FAIL:
            run = false;
            serialPort?.Close();
            try
            {
                Invoke(ShowFinish, "Fail serial port!\nSomething went wrong.\nCheck wiring connections,\nreset the equipment and try again");
            }
            catch { return; }
            return;
        }

        private void ProgressStage(int progress)
        {
            progressBar.Style = ProgressBarStyle.Blocks;
            if (progress < 0) progress = progressBar.Value + 1;
            progressBar.Value = progress <= progressBar.Maximum ? progress : progressBar.Maximum;
            Refresh();
        }

        private void ShowFinish(string txt)
        {
            stage = 0;
            label.Text = txt;
            progressBar.Visible = false;
            button0.Text = "OK";
            button0.Left = 300;
            button0.Visible = true;
            Refresh();

        }

        private void ShowServerStart()
        {
            label.Text = flashInfo + "\nTFTP server IP:" + serverIP + "\nTFTP client(camera) IP:";
            progressBar.Visible = false;
            textBoxIP.Visible = true;
            button0.Text = "Start";
            button0.Visible = true;
            Refresh();
        }

        private static string GetLocalIPAddress()
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 0);
            return socket.LocalEndPoint is IPEndPoint endPoint ? endPoint.Address.ToString() : "";
        }

        private void ThreadSendCmd()
        {
            if (serialPort.IsOpen == false) return;
            serialPort.ReadTimeout = 8000;
            while (run && cmd.Length != 0)
            {
                Write(Encoding.ASCII.GetBytes(cmd), cmd.Length);
                cmd = "";
                byte[] b = new byte[1];
                string line = "";
                int cT = TIMEOUT;
                bool sl = false;
                bool tR = false;
                bool tS = false;
                line = "";
                while (run)
                {
                    if (Read(b))
                    {
                        line += Encoding.ASCII.GetString(b);
                        if (line.Contains("goke #"))
                        {
                            break;
                        }
                        else if (b[0] == '\r')
                        {
                            if (line.Contains("Erasing") || line.Contains("Writing"))
                            {
                                int progress = int.Parse(line.Substring(line.IndexOf("%") - 3, 3)); ;
                                if (line.StartsWith("Writing")) progress += 100;
                                try
                                {
                                    Invoke(ProgressStage, progress);
                                }
                                catch { return; }
                            }
                            line = "";
                        }
                        else if (sl)
                        {
                            if (b[0] == '#' || b[0] == 'T')
                            {
                                if (b[0] == '#')
                                {
                                    cT = TIMEOUT;
                                    try
                                    {
                                        Invoke(ProgressStage, -1);
                                    }
                                    catch { return; }
                                }
                                else if (--cT == 0)
                                {
                                    tS = true;
                                    break;
                                }
                                line = "";
                            }
                        }
                        else if (line.Contains("Saving:") || line.Contains("Loading:"))
                        {
                            sl = true;
                            line = "";
                        }
                    }
                    else
                    {
                        tR = true;
                        break;
                    }
                }
                if (run && stage != 0)
                {
                    if (tR)
                    {
                        run = false;
                        serialPort?.Close();
                        try
                        {
                            Invoke(ShowFinish, "Fail serial port!\nSomething went wrong.\nCheck wiring connections,\nreset the equipment and try again");
                        }
                        catch { return; }
                        return;
                    }
                    else if (tS)
                    {
                        string txt = "Dump. Fail!";
                        if (stage > 2)
                        {
                            txt = "Burn. Fail!";
                        }
                        txt += "\nServer response timed out.\nTry again";
                        try
                        {
                            Invoke(ShowFinish, txt);
                        }
                        catch { return; }
                        files.Clear();
                        cmd = "\x03";
                    }
                    else 
                    {
                        Thread.Sleep(1000);
                        FLASH_FILE ff;
                        switch (stage)
                        {
                            case 1:
                                ff.name = "";
                                ff.loadA = 0;
                                ff.sizeE = flashSize;
                                ff.data = new byte[tBuf_len];
                                Array.Copy(tBuf, ff.data, tBuf_len);
                                files.Add(ff);
                                FileGet();
                                try
                                {
                                    Invoke(SetStage, 2);
                                }
                                catch { return; }
                                break;
                            case 2:
                                ff.name = "";
                                ff.loadA = 0;
                                ff.sizeE = flashSize;
                                ff.data = new byte[tBuf_len];
                                Array.Copy(tBuf, ff.data, tBuf_len);
                                files.Add(ff);
                                bool success = VerifyFiles();
                                if (success)
                                {
                                    string filename = "dump_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".bin";
                                    File.WriteAllBytes(filename, files[0].data);
                                    try
                                    {
                                        Invoke(ShowFinish, "Dump. Success!\nSaved file:\n" + filename);
                                    }
                                    catch { return; }
                                }
                                else
                                {
                                    try
                                    {
                                        Invoke(ShowFinish, "Dump. Fail!\nVerification error\nTry again");
                                    }
                                    catch { return; }
                                }
                                files.Clear();
                                break;
                            case 3:
                                cmd = "sf probe 0; sf lock 0; sf erase 0x" + files[fi].loadA.ToString("x") + " 0x" + files[fi].sizeE.ToString("x") +
                                       "; sf write 0x42000000 0x" + files[fi].loadA.ToString("x") + " 0x" + files[fi].sizeE.ToString("x") + ";\n";
                                try
                                {
                                    Invoke(SetStage, 4);
                                }
                                catch { return; }
                                break;
                            case 4:
                                fi++;
                                if (fi < files.Count)
                                {
                                    FilePut();
                                    try
                                    {
                                        Invoke(SetStage, 3);
                                    }
                                    catch { return; }
                                }
                                else
                                {
                                    FileGet();
                                    try
                                    {
                                        Invoke(SetStage, 5);
                                    }
                                    catch { return; }
                                }
                                break;
                            case 5:
                                ff.name = "";
                                ff.loadA = 0;
                                ff.sizeE = flashSize;
                                ff.data = new byte[tBuf_len];
                                Array.Copy(tBuf, ff.data, tBuf_len);
                                files.Add(ff);
                                if (VerifyFiles() == true)
                                {
                                    try
                                    {
                                        Invoke(ShowFinish, "Burn. Success!");
                                    }
                                    catch { return; }
                                }
                                else
                                {
                                    try
                                    {
                                        Invoke(ShowFinish, "Burn. Fail!\nVerification error\nTry again");
                                    }
                                    catch { return; }
                                }
                                files.Clear();
                                break;
                        }
                    }
                }
            }
        }

        private bool VerifyFiles()
        {
            if (files.Count < 2) return false;
            byte[] v = files.Last().data;
            if (v.Length == 0) return false;
            for (int i = 0; i < files.Count - 1; i++)
            {
                byte[] d = files[i].data;
                int a = files[i].loadA;
                for (int j = 0; j < d.Length; j++)
                {
                    if (v[a + j] != d[j]) return false;
                }
            }
            return true;
        }

        private void SetStage(int s)
        {
            stage = s;
            string txt = "";
            int progressMax = 0;
            progressBar.Style = ProgressBarStyle.Marquee;
            switch (stage)
            {
                case 1:
                    txt = "Dump. Download.\nPlease wait...";
                    progressMax = flashSize / (TFTP_PAYLOAD_SIZE * 10);
                    break;
                case 2:
                    txt = "Dump. Verification.\nPlease wait...";
                    progressMax = flashSize / (TFTP_PAYLOAD_SIZE * 10);
                    break;
                case 3:
                    txt = "Burn file (" + (fi + 1) + "/" + files.Count + "):\n" + files[fi].name + "\nUpload...";
                    progressMax = files[fi].data.Length / (TFTP_PAYLOAD_SIZE * 10);
                    break;
                case 4:
                    txt = "Burn file (" + (fi + 1) + "/" + files.Count + "):\n" + files[fi].name + "\nWrite...";
                    progressMax = 200;
                    progressBar.Style = ProgressBarStyle.Blocks;
                    break;
                case 5:
                    txt = "Burn. Verification.\nPlease wait...";
                    progressMax = flashSize / (TFTP_PAYLOAD_SIZE * 10);
                    break;
            }
            label.Text = txt;
            progressBar.Value = 0;
            progressBar.Maximum = progressMax;
            progressBar.Visible = true;
            Refresh();
        }

        private bool Send_uboot()
        {
            if (ubootFile == null) return false;
            int length = ubootFile.Length;
            int address2 = 0x41000000;
            if (!Send_frame_head(length, address2)) return false;
            int seq = 1;
            byte[] data = new byte[SP_MAX_DATA_LEN];
            int ui = 0;
            while (ui < ubootFile.Length)
            {
                int dataLen = ubootFile.Length - ui;
                if (dataLen > SP_MAX_DATA_LEN)
                {
                    dataLen = SP_MAX_DATA_LEN;
                }
                System.Array.Copy(ubootFile, ui, data, 0, dataLen);
                ui += dataLen;
                if (!Send_frame_data(seq, data, dataLen)) return false;
                seq++;
                try
                {
                    Invoke(ProgressStage, -1);
                }
                catch { return false; }
            }
            if (!Send_frame_tail(seq)) return false;
            return true;
        }

        private bool Send_SPL()
        {
            if (ubootFile == null) return false;
            int first_length = SP_BOOT_SPL_LEN;
            int address1 = 0x04010500;
            if (!Send_frame_head(first_length, address1)) return false;
            int seq = 1;
            byte[] data = new byte[SP_MAX_DATA_LEN];
            int ui = 0;
            for (int i = 0; i < first_length / data.Length; i++)
            {
                System.Array.Copy(ubootFile, ui, data, 0, data.Length);
                ui += data.Length;
                if (!Send_frame_data(seq, data, data.Length)) return false;
                seq++;
                try
                {
                    Invoke(ProgressStage, -1);
                }
                catch { return false; }
            }
            if (!Send_frame_tail(seq)) return false;
            return true;
        }


        private bool Send_frame_data(int seq, byte[] data, int data_len)
        {
            int sizeFrame = 3 + data_len + 2;
            byte[] frame = new byte[3 + SP_MAX_DATA_LEN + 2];
            int i = 0;
            frame[i++] = 0xda;
            frame[i++] = (byte)(seq & 0xff);
            frame[i++] = (byte)(~seq & 0xff);
            System.Array.Copy(data, 0, frame, i, data_len);
            i += data_len;
            ushort crc = Calc_crc(frame, i);
            frame[i++] = (byte)((crc >> 8) & 0xff);
            frame[i++] = (byte)(crc & 0xff);
            if (!Send_frame(frame, sizeFrame, 32)) return false;
            return true;
        }

        private bool Read(byte[] b)
        {
            try
            {
                serialPort.Read(b, 0, b.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool Send_ddrstep()
        {
            int i = 0;
            int seq = 1;
            int address0 = 0x04013000;
            if (!Send_frame_head(64, address0)) return false;
            byte[] frame = new byte[3 + ddrstep.Length + 2];
            frame[i++] = 0xda;
            frame[i++] = (byte)(seq & 0xff);
            frame[i++] = (byte)(~seq & 0xff);
            System.Array.Copy(ddrstep, 0, frame, i, ddrstep.Length);
            i += ddrstep.Length;
            ushort crc = Calc_crc(frame, i);
            frame[i++] = (byte)((crc >> 8) & 0xff);
            frame[i++] = (byte)(crc & 0xff);
            if (!Send_frame(frame, frame.Length, 16)) return false;
            if (!Send_frame_tail(seq + 1)) return false;
            return true;
        }

        private bool Send_frame_tail(int seq)
        {
            int i = 0;
            byte[] frame = new byte[5];
            frame[i++] = 0xed;
            frame[i++] = (byte)(seq & 0xff);
            frame[i++] = (byte)(~seq & 0xff);
            ushort crc = Calc_crc(frame, i);
            frame[i++] = (byte)((crc >> 8) & 0xff);
            frame[i++] = (byte)(crc & 0xff);
            if (!Send_frame(frame, frame.Length, 16)) return false;
            return true;
        }

        private bool Send_frame(byte[] data, int len, int loop)
        {
            byte[] b = new byte[1];
            for (int i = 0; i < loop; i++)
            {
                if (!Write(data, len)) return false;
                if (Read(b))
                {
                    if (b[0] == 0xaa)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Send_frame_head(int length, int address)
        {
            int i = 0;
            byte[] frame = new byte[14];
            frame[i++] = 254;
            frame[i++] = 0;
            frame[i++] = 255;
            frame[i++] = 1;
            frame[i++] = (byte)((length >> 24) & 0xff);
            frame[i++] = (byte)((length >> 16) & 0xff);
            frame[i++] = (byte)((length >> 8) & 0xff);
            frame[i++] = (byte)((length) & 0xff);
            frame[i++] = (byte)((address >> 24) & 0xff);
            frame[i++] = (byte)((address >> 16) & 0xff);
            frame[i++] = (byte)((address >> 8) & 0xff);
            frame[i++] = (byte)((address) & 0xff);
            ushort crc = Calc_crc(frame, i);
            frame[i++] = (byte)((crc >> 8) & 0xff);
            frame[i++] = (byte)(crc & 0xff);
            if (!Send_frame(frame, frame.Length, 16)) return false;
            return true;
        }

        private ushort Calc_crc(byte[] data, int len)
        {
            ushort crc = 0;
            for (int i = 0; i < len; i++)
            {
                crc = (ushort)(((crc << 8) | data[i]) ^ crctable[(crc >> 8) & 0xff]);
            }
            for (int i = 0; i < 2; i++)
            {
                crc = (ushort)(((crc << 8) | 0) ^ crctable[(crc >> 8) & 0xff]);
            }
            return crc;
        }

        private bool Write(byte[] b, int len)
        {
            try
            {
                serialPort.Write(b, 0, len);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ShowLoading()
        {
            label.Text = "Loading u-boot into RAM\nPlease wait...";
            button0.Visible = false;
            Refresh();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            run = false;
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = linkLabel.Text,
                UseShellExecute = true
            });
        }
    }
}

