using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake.App.Module.Views
{
    /// <summary>
    /// JsonParserView.xaml 的交互逻辑
    /// </summary>
    public partial class JsonParserView : UserControl
    {
        public JsonParserView()
        {
            InitializeComponent();
        }


        #region functions

        /// <summary>
        /// 绑定树形控件
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="strJson"></param>
        public void BindTreeView(TreeView treeView, string strJson)
        {
            treeView.Items.Clear();


            if (IsJOjbect(strJson))
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);

                foreach (var item in jo)
                {
                    TreeViewItem tree;
                    if (item.Value.GetType() == typeof(JObject))
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        treeView.Items.Add(tree);
                    }
                    else if (item.Value.GetType() == typeof(JArray))
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key;
                        tree.IsExpanded = true;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        treeView.Items.Add(tree);
                    }
                    else
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key + ":" + item.Value.ToString();
                        treeView.Items.Add(tree);
                    }
                }
            }
            if (IsJArray(strJson))
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(strJson);
                int i = 0;
                foreach (JObject item in ja)
                {
                    TreeViewItem tree = new TreeViewItem();
                    tree.Header = string.Format("({0})", i);
                    tree.IsExpanded = true;
                    foreach (var itemOb in item)
                    {
                        TreeViewItem treeOb;
                        if (itemOb.Value.GetType() == typeof(JObject))
                        {
                            treeOb = new TreeViewItem();
                            treeOb.Header = itemOb.Key;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Items.Add(treeOb);

                        }
                        else if (itemOb.Value.GetType() == typeof(JArray))
                        {
                            treeOb = new TreeViewItem();
                            treeOb.Header = itemOb.Key;
                            treeOb.IsExpanded = true;
                            AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                            tree.Items.Add(treeOb);
                        }
                        else
                        {
                            treeOb = new TreeViewItem();
                            treeOb.Header = itemOb.Key + ":" + itemOb.Value.ToString();
                            tree.Items.Add(treeOb);
                        }
                    }
                    treeView.Items.Add(tree);
                    i++;
                }
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="parantNode"></param>
        /// <param name="value"></param>
        public void AddTreeChildNode(ref TreeViewItem parantNode, string value)
        {
            if (IsJOjbect(value))
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(value);
                foreach (var item in jo)
                {
                    TreeViewItem tree;
                    if (item.Value.GetType() == typeof(JObject))
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        parantNode.Items.Add(tree);
                    }
                    else if (item.Value.GetType() == typeof(JArray))
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key;
                        tree.IsExpanded = true;
                        AddTreeChildNode(ref tree, item.Value.ToString());
                        parantNode.Items.Add(tree);
                    }
                    else
                    {
                        tree = new TreeViewItem();
                        tree.Header = item.Key + ":" + item.Value.ToString();
                        parantNode.Items.Add(tree);
                    }
                }
            }
            if (IsJArray(value))
            {
                JArray ja = (JArray)JsonConvert.DeserializeObject(value);
                parantNode.Tag = ja;
                int i = 0;
                foreach (var item in ja)
                {
                    if (item is JValue)
                    {
                        TreeViewItem tree = new TreeViewItem();
                        tree.Header = (item as JValue).Value.ToString();
                        parantNode.Items.Add(tree);
                    }
                    else
                    {
                        TreeViewItem tree = new TreeViewItem();
                        tree.Header = string.Format("({0})", i);
                        foreach (var itemOb in (item as JObject))
                        {
                            TreeViewItem treeOb;
                            if (itemOb.Value.GetType() == typeof(JObject))
                            {
                                treeOb = new TreeViewItem();
                                treeOb.Header = itemOb.Key;
                                AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                                tree.Items.Add(treeOb);

                            }
                            else if (itemOb.Value.GetType() == typeof(JArray))
                            {
                                treeOb = new TreeViewItem();
                                treeOb.Header = itemOb.Key;
                                AddTreeChildNode(ref treeOb, itemOb.Value.ToString());
                                tree.Items.Add(treeOb);
                            }
                            else
                            {
                                treeOb = new TreeViewItem();
                                treeOb.Header = itemOb.Key + ":" + itemOb.Value.ToString();
                                tree.Items.Add(treeOb);
                            }
                        }
                        parantNode.Items.Add(tree);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        ///  判断是否JOjbect类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsJOjbect(string value)
        {
            try
            {
                JObject ja = JObject.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 判断是否JArray类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsJArray(string value)
        {
            try
            {
                JArray ja = JArray.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
        

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            string json = "[{\"Groupid\": \"1\",\"groupnum\": \"9005\",\"groupname\": \"调度中心\",\"type\": \"1\",\"dnsprefix\": \"\",\"islocal\": \"1\",\"canshowall\": \"0\",\"user\": [],\"group\": [{\"Groupid\": \"54\",\"groupnum\": \"66000\",\"groupname\": \"大唐移动\",\"type\": \"0\",\"dnsprefix\": \"\",\"islocal\": \"1\",\"canshowall\": \"1\",\"user\": [],\"group\": [{\"Groupid\": \"55\",\"groupnum\": \"67000\",\"groupname\": \"大唐移动1\",\"type\": \"1\",\"dnsprefix\": \"\",\"islocal\": \"1\",\"canshowall\": \"1\",\"user\": [],\"group\": []}]		},{\"Groupid\": \"66\",\"groupnum\": \"66000\",\"groupname\": \"大唐联通\",\"type\": \"0\",\"dnsprefix\": \"\",\"islocal\": \"1\",\"canshowall\": \"1\",\"user\": [],\"group\": [{\"Groupid\": \"67\",\"groupnum\": \"67000\",\"groupname\": \"大唐联通1\",\"type\": \"1\",\"dnsprefix\": \"\",\"islocal\": \"1\",\"canshowall\": \"1\",\"user\": [],\"group\": []}]		}]}]";

            BindTreeView(treeView, richTextBox.LineNumText);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.LineNumText = string.Empty;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem)
            {
                TreeViewItem node = treeView.SelectedItem as TreeViewItem;
                if (node.Tag is null)
                {
                    dynamicDataGrid.ItemsSource = null;
                }
                else
                {
                    var ja = node.Tag as JArray;
                    DataTable dt = new DataTable();
                    DataColumn dc;
                    bool isCreatedColumn = false;
                    foreach (var item in ja)
                    {
                        if (item is JObject)
                        {
                            //created columns
                            if (!isCreatedColumn)
                            {
                                foreach (var itemOb in (item as JObject))
                                {
                                    if (itemOb.Value.GetType() == typeof(JValue))
                                        dt.Columns.Add(new DataColumn(itemOb.Key, typeof(String)));
                                }
                                isCreatedColumn = true;
                            }

                            //set datas
                            DataRow dr = dt.NewRow();
                            foreach (var itemOb in (item as JObject))
                            {
                                if (itemOb.Value.GetType() == typeof(JValue) && dt.Columns.Contains(itemOb.Key))
                                {
                                    dr[itemOb.Key] = itemOb.Value.ToString();
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dynamicDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }
    }
}
