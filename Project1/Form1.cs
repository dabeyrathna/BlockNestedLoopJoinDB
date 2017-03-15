﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbCurrServer.SelectedIndex = 0;
        }

        List<Account> accountTable;
        List<Branch> branchTable;
        List<Depositor> depositorTable;
        List<Customer> customerTable;

        List<RelationInfo> tableSet = new List<RelationInfo>();

    
        public string[] tables = {"branch", "branch", "account","depositor", "customer"};
        public string[] outBuffer = new string[5];


        public void readTable(string tabel, string site)
        {
            
            if (tabel.ToLower().Equals("customer"))
            {
                try
                {
                    //MessageBox.Show("customer_" + site.ToUpper() + ".txt");
                    using (StreamReader sr = new StreamReader("customer_HOU.txt"))
                    {
                        string line;
                        customerTable = new List<Customer>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            customerTable.Add(new Customer(s[0].ToString(), s[1].ToString(), s[2].ToString())); 
                        }
                    }                    

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found customer  \n\n"+eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("branch"))
            {
                string fileName = "";
                try
                {
                    fileName = "branch_" + site.ToUpper() + ".txt";

                    //MessageBox.Show(fileName);
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string line;
                        branchTable = new List<Branch>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            branchTable.Add(new Branch(s[0].ToString(), s[1].ToString(), s[2].ToString()));
                        }
                    }
                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found branch  \n\n" + eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("account"))
            {
                try
                {
                    //MessageBox.Show("account_" + site.ToUpper() + ".txt");
                    using (StreamReader sr = new StreamReader("account_" + site.ToUpper() + ".txt"))
                    {
                        string line;
                        accountTable = new List<Account>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            accountTable.Add(new Account(s[0].ToString(), s[1].ToString(), s[2].ToString()));
                        }
                    }

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found account  \n\n" + eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("depositor") && site.ToUpper().Equals(cmbCurrServer.Text))
            {
                try
                {
                    //MessageBox.Show("depositor_" + site.ToUpper() + ".txt");
                    using (StreamReader sr = new StreamReader("depositor_" + site.ToUpper() + ".txt"))
                    {
                        string line;
                        depositorTable = new List<Depositor>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            depositorTable.Add(new Depositor(s[0].ToString(), s[1].ToString()));
                        }
                    }

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found depositor  \n\n" + eee.ToString());
                }
            }

            tableSet.Add(new RelationInfo(tabel, site.ToUpper()));

/*
            if (tabel.Equals("customer")) {
                foreach (Customer c in customerTable) {
                    lstInner.Items.Add(c.getCustName());
                }
            }
            else if (tabel.Equals("account")) {
                foreach (Account c in accountTable)
                {
                    lstOuter.Items.Add(c.getAccountName());
                }
            }
            else if (tabel.Equals("branch"))
            {
                foreach (Branch c in branchTable)
                {
                    lstOuter.Items.Add(c.getBranchName());
                }
            }
            else if (tabel.Equals("depositor"))
            {
                foreach (Depositor c in depositorTable)
                {
                    lstOuter.Items.Add(c.getKey1()+","+c.getKey2());
                }
            }

    */
        }

        public List<string> loadTables(string[] arr, List<string> conditions)
        {

            List<string> fileArray = new List<string>();

            bool cust = true;
            bool branch = true;
            bool account = true;
            bool deposite = true;

            foreach (string i in conditions)
            {
                if (i.Contains("customer") && cust)
                {
                    cust = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Customer table in HOU site";
                    fileArray.Add("customer_hou");
                }
                if (i.Contains("OMA") && i.Contains("depositor") && deposite)
                {
                    deposite = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Depositor table in Omaha site";
                    fileArray.Add("depositor_oma");
                }
                else if (!i.Contains("OMA") && i.Contains("depositor") && deposite)
                {
                    deposite = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Depositor table in NYC";
                    fileArray.Add("depositor_nyc");
                }
                if (cmbCurrServer.SelectedIndex == 0 && i.Contains("branch") && branch)
                {
                    branch = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine + "Branch table in NYC site";
                    fileArray.Add("branch_nyc");
                }
                else if (cmbCurrServer.SelectedIndex == 3 && i.Contains("branch") && branch)
                {
                    branch = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Branch table in SFO site";
                    fileArray.Add("branch_sfo");
                }
                if (i.Contains("SFO") && i.Contains("account") && account)
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Account table in SFO site";
                    fileArray.Add("account_sfo");
                }
                else if (i.Contains("OMA") && i.Contains("account") && account)
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Account table in OMA site";
                    fileArray.Add("account_oma");
                }
                else if (i.Contains("account") && account)
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+ "Account table in NYC site";
                    fileArray.Add("account_nyc");
                }
            }

            return fileArray;
        }


        public void twoWaySemijoin(RelationInfo table1, RelationInfo table2, string currentSite) {
            txtLog.Text = txtLog.Text + Environment.NewLine;
            txtLog.Text = txtLog.Text + "Ship table 1 key to merge, get table 2 key to min of not or ....... and return to"+currentSite;
        }

        public void semiJoin(RelationInfo table1, RelationInfo table2) {

            string shippingFileName = "";
            string returndFileName = "";
            if (table1.site.Equals(cmbCurrServer.Text))
            {
                shippingFileName = table1.table + "_" + table1.site + "_" + table2.site + "_copy.txt";
                returndFileName = table2.table + "_" + table2.site + "_" + table1.site + "_copy.txt";

                try
                {
                    System.IO.File.Delete(shippingFileName);
                    System.IO.File.Delete(returndFileName);
                }
                catch (System.IO.IOException eee)
                {}


                if (table1.table.Equals("customer"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Customer i in customerTable) {
                            file.WriteLine(i.getCustName());
                            lstSite1.Items.Add(i.getCustName());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            List<string> tempSite1 = new List<string>();

                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Depositor i in depositorTable)
                                {
                                    if (line.Equals(i.getKey1()))
                                    {
                                        tempSite1.Add(line);
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite1.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempsiteL1)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;

                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }
                }
                if (table1.table.Equals("depositor"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            file.WriteLine(i.getKey1());
                            lstSite1.Items.Add(i.getKey1());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            HashSet<string> tempSite2 = new HashSet<string>();

                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Customer i in customerTable)
                                {
                                    if (line.Equals(i.getCustName()))
                                    {
                                        tempSite2.Add(line);
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (Customer i in customerTable)
                            {
                                file.WriteLine(i.getCustName());
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;

                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }
                    
                }
                if (table2.table.Equals("branch"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                }
                if (table2.table.Equals("account"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                }
            }

////////////////////////////////////////////////////////////////

            else if (table2.site.Equals(cmbCurrServer.Text))
            {
                shippingFileName = table2.table + "_" + table2.site + "_" + table1.site + "_copy.txt";
                returndFileName = table1.table + "_" + table1.site + "_" + table2.site + "_copy.txt";
                try
                {
                    System.IO.File.Delete(shippingFileName);
                    System.IO.File.Delete(returndFileName);
                }
                catch (System.IO.IOException eee)
                {
                    MessageBox.Show("File not found depositor  \n\n" + eee.ToString());
                }

                if (table2.table.Equals("customer"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Customer i in customerTable)
                        {
                            file.WriteLine(i.getCustName());
                            lstSite1.Items.Add(i.getCustName());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            HashSet<string> tempSite1 = new HashSet<string>();


                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Depositor i in depositorTable)
                                {
                                    if (line.Equals(i.getKey1()))
                                    {
                                        tempSite1.Add(line);
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite1.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempsiteL1)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;

                    }

                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table2.table.Equals("depositor"))
                {
                    MessageBox.Show("starting depos site 2");
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table2.table + " key to site " + table1.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            file.WriteLine(i.getKey1());
                            lstSite1.Items.Add(i.getKey1());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            HashSet<string> tempSite2 = new HashSet<string>();
                            
                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Customer i in customerTable)
                                {
                                    if (line.Equals(i.getCustName())) {
                                        tempSite2.Add(line);
                                        break;                                       
                                    }
                                }
                            }
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;
                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : "+ shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempsiteL1)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table1.table.Equals("branch"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table1.table + " key to site" + table2.site;
                }
                if (table1.table.Equals("account"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table1.table + " key to site" + table2.site;
                }
            }
            else
            {
                twoWaySemijoin(table1,table2,cmbCurrServer.Text);
            }
        }

        public void joinProcess(List<string> fileList) {

            string[] f_info;
            foreach (string files in fileList)
            {
                f_info = files.Split('_');
                readTable(f_info[0], f_info[1]);
            }            
        }

        public List<string> conditionCheck(string condition) {

            string[] condList;
            List<string> list = new List<string>();

            Regex rgx = new Regex("AND");
            string result = rgx.Replace(condition, "~");                
            condList = result.Split('~');

            foreach (string i in condList)
            {
                txtLog.Text = txtLog.Text + Environment.NewLine + i +Environment.NewLine;
                list.Add(i);
            }
            return list;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] fromList = txtxFrom.Text.ToString().Split(',');
                bool fileFlag = true;

                foreach (string i in fromList) {
                    if (!tables.Contains(i) || txtWhere.Text == "") {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Query error. No such a table found = " + i+"\n\nPlease try again  ";
                        txtSelect.Text = "";
                        fileFlag = false;
                    }
                }                

                if (fileFlag) {
                    txtLog.Text = txtLog.Text + Environment.NewLine + "query ok........" + Environment.NewLine;
                    joinProcess(loadTables(fromList, conditionCheck(txtWhere.Text)));

                    foreach (RelationInfo i in tableSet)
                    {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Site is = " + i.site + "  and Table is = " + i.table;
                    }

                    semiJoin(tableSet[0], tableSet[1]);
                    tableSet.Clear();

                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                }              
            }
            catch (Exception err) {
                txtSelect.Text = err.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try { 
                lstInner.Items.Clear();               
            }
            catch (Exception ee1) {
                lstInner.DataSource = null;                
            }
            try {
                lstOuter.Items.Clear();
            }
            catch (Exception ee2) {
                lstOuter.DataSource = null;
            }            
            tableSet.Clear();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }
    }
}
