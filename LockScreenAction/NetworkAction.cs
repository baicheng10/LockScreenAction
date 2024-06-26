using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management;
using System.Windows.Forms;

namespace LockScreenAction
{
    class NetworkAction
    {
        //禁用所有物理网络适配器，需要管理员权限运行
        public static void DoALL_DisableNetworkAdapter()
        {
            try
            {
                // 查找所有物理网络适配器（非虚拟）  
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter = True AND NetEnabled = True");

                foreach (ManagementObject adapter in searcher.Get())
                {
                    // 禁用网络适配器  
                    adapter.InvokeMethod("Disable", null);
                    //Console.WriteLine("Disabled: " + adapter["Name"]);
                    //MessageBox.Show("Disabled: " + adapter["Name"]);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Error: " + e.Message);
                MessageBox.Show("Error: " + e.Message);
            }  
        }

        //恢复所有被禁用的网络适配器
        public static void DoAll_EnableNetworkAdapter()
        {
            try
            {
                // 查找所有禁用的物理网络适配器  
                var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter = True AND NetEnabled = False");

                foreach (ManagementObject adapter in searcher.Get())
                {
                    // 启用网络适配器  
                    adapter.InvokeMethod("Enable", null);
                    //Console.WriteLine("Enabled: " + adapter["Name"]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }  
        }


        //禁用所有配置文件中写的网络适配器
        public static void Do_DisableNetworkAdapter()
        {
            string network_names = INIhelper.GetValue("NETWORK_NAME");
            string[] network_names_list = network_names.Split(',');
            foreach (var anetwork_name in network_names_list)
            {
                DisableNetworkAdapter(anetwork_name);
            }  
        }

        //恢复所有配置文件中写的网络适配器
        public static void Do_EnableNetworkAdapter()
        {
            string network_names = INIhelper.GetValue("NETWORK_NAME");
            string[] network_names_list = network_names.Split(',');
            foreach (var anetwork_name in network_names_list)
            {
                EnableNetworkAdapter(anetwork_name);
            }   
        }

        //禁用网络适配器，需要管理员权限运行
        public static void DisableNetworkAdapter(string adapterName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID = '" + adapterName + "'"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        // 确保适配器是物理的，并且不是禁用的  
                        if ((bool)queryObj["PhysicalAdapter"] && (bool)queryObj["NetEnabled"])
                        {
                            // 禁用网络适配器  
                            queryObj.InvokeMethod("Disable", null, null);
                            //Console.WriteLine("Disabled: " + adapterName);
                            //MessageBox.Show("Disabled: " + adapterName);
                            return;
                        }
                    }
                    //Console.WriteLine("Adapter not found or already disabled: " + adapterName);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error disabling adapter: " + ex.Message);
                MessageBox.Show("Error disabling adapter: " + ex.Message);
            }
        }

        //恢复被禁用的网络适配器
        public static void EnableNetworkAdapter(string adapterName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID = '" + adapterName + "'"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        // 确保适配器是物理的，并且是禁用的  
                        if ((bool)queryObj["PhysicalAdapter"] && !(bool)queryObj["NetEnabled"])
                        {
                            // 启用网络适配器  
                            queryObj.InvokeMethod("Enable", null, null);
                            //Console.WriteLine("Enabled: " + adapterName);
                            return;
                        }
                    }
                    //Console.WriteLine("Adapter not found or already enabled: " + adapterName);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error enabling adapter: " + ex.Message);
                MessageBox.Show("Error enabling adapter: " + ex.Message);
            }
        }
  

    }
}
