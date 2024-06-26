using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Management;  

namespace LockScreenAction
{
    public partial class MainForm : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;  

        public MainForm()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.Myico;
            //this.Icon = new Icon("myico2.ico");

            // 初始化托盘图标和上下文菜单  
            trayIcon = new NotifyIcon();
            //trayIcon.Icon = new Icon("myico2.ico"); // 使用你的图标文件 
            trayIcon.Icon = Properties.Resources.Myico;
            trayIcon.Text = "LockScreenAction";
            trayIcon.Visible = false; // 初始时不可见  

            trayMenu = new ContextMenuStrip();
            // 向上下文菜单添加项，例如退出选项  
            var exitToolStripMenuItem = new ToolStripMenuItem("Exit");
            exitToolStripMenuItem.Click += (sender, e) => Application.Exit();
            trayMenu.Items.Add(exitToolStripMenuItem);

            trayIcon.ContextMenuStrip = trayMenu;

            // 订阅窗体大小改变事件  
            this.Resize += MainForm_Resize;  
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // 隐藏窗体并显示托盘图标  
                this.Hide();
                trayIcon.Visible = true;
            }
        }

        // 可以添加其他事件处理程序来响应托盘图标的双击等事件  
        // 例如：trayIcon.DoubleClick += (sender, e) => this.Show(); this.WindowState = FormWindowState.Normal;  

        // 在窗体关闭事件中确保托盘图标被移除  
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (trayIcon.Visible)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }

            base.OnFormClosing(e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
             //程序加载时执行监听，并最小化程序、隐藏图标。可将程序设为开机自启动
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        } 

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock||e.Reason==Microsoft.Win32.SessionSwitchReason.SessionLogoff)
            {
                // 屏幕锁定  
                //锁屏后执行
                //MessageBox.Show("Screen Lock" + DateTime.Now);
                NetworkAction.DoALL_DisableNetworkAdapter();
            }
            else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock||e.Reason==Microsoft.Win32.SessionSwitchReason.SessionLogon)
            {
                // 屏幕解锁  
                //解屏、登录后执行

                //MessageBox.Show("Screen Unlock" + DateTime.Now);
                NetworkAction.DoAll_EnableNetworkAdapter();

            }
        }
    }
}