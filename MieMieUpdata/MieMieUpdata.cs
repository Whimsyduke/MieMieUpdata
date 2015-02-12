//#define Debug

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace MieMieUpdata
{
    public partial class MieMieUpdata : Form
    {
        //咩咩服务器数据

        private FTPClass MieMieFtp;
        public XElement RootXEL;

        public MieMieUpdata()
        {
            InitializeComponent();
            MieMieFtp = new FTPClass();
            FormInit();
        }

        //常量数据
        public string applicationPath = Application.StartupPath;
        public string minecraft = Application.StartupPath + @"\.minecraft\";
        
        //设置数据
        public string optionXmlPath = Application.StartupPath + "\\Config.xml";
        public MieMieSetting optionConfig = new MieMieSetting();
        public XElement optionConfigXmlFile;

        //服务器设置
        public static string ftpServerIPA;
        public static string ftpServerIPB;

        public static string ftpUserID;
        public static string ftpPassword;


        #region 窗口初始化

        //窗口初始化
        public void FormInit()
        {
            GetServersInformation();
            OptionConfigInit();
            //线路设置
            this.comboBoxLine.SelectedIndex = optionConfig.ftpServerIndex;

            //X64模式设置
            this.checkBoxX64.Checked = optionConfig.gameX64;

            //游戏相关检测
            FormGameCheck();

        }

        //游戏版本检测
        public void FormGameCheck()
        {
            //游戏存在性检测
            if (!(Directory.Exists(minecraft) && Directory.Exists(minecraft + @"libraries\") && Directory.Exists(minecraft + @"versions\")
                && Directory.Exists(minecraft + @"assets\")))
            {
                MessageBox.Show("当期路径下不存在一个有效的Minecraft游戏文件夹。一个有效的Minecraft游戏文件夹应该包含assets、libraries和versions三个子文件夹。请从群共享下载最新版本客户端并进行部分更新，或者选择全部更新。（为减轻服务器负担，请尽可能选择前者）", "找不到有效的游戏", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.buttonStart.Enabled = false;
                return;
            }

            //游戏类型设置
            List<string> simpleFileName = new List<string>();
            string simpleName;
            string[] files = Directory.GetFileSystemEntries(minecraft + "versions");
            if (files.Length == 0)
            {
                MessageBox.Show("当前不存在一个有效的游戏版本。请从群共享下载最新版本客户端并进行部分更新，或者选择全部修复。\n（为减轻服务器负担，请尽可能选择前者）", "找不到有效的游戏", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.buttonStart.Enabled = false;
                return;
            }
            foreach (string fullFileName in files)
            {
                simpleName = fullFileName.Substring(fullFileName.LastIndexOf("\\") + 1);
                if (File.Exists(fullFileName + "\\" + simpleName + ".jar") && File.Exists(fullFileName + "\\" + simpleName + ".json"))
                {
                    simpleFileName.Add(simpleName);
                    this.comboBoxGame.Items.Add(simpleName);
                }
            }

            if (simpleFileName.Count != 0)
            {
                if (simpleFileName.Contains(optionConfig.gameVersion))
                {
                    this.comboBoxGame.SelectedItem = optionConfig.gameVersion;
                }
                else
                {
                    this.comboBoxGame.SelectedIndex = 0;
                    optionConfig.gameVersion = this.comboBoxGame.Text;
                    OptionConfigSave();
                }
            }
        }

        #endregion

        #region 设置处理

        //设置初始化
        public void OptionConfigInit()
        {
            if (File.Exists(optionXmlPath))
            {
                OptionConfigLoad();
            }
            else
            {
                optionConfig.OptionConfigMieMieSetting();
                OptionConfigSave();
            }
            OptionConfigUseSet(optionConfig);
        }

        //设置保存
        public void OptionConfigSave()
        {
            optionConfigXmlFile =
                new XElement("Option",
                    new XElement("Setting",
                        new XAttribute ("x",true),
                        new XElement("GameName", optionConfig.gameName),
                        new XElement("GameJavaPath", optionConfig.gameJavaPath),
                        new XElement("GameMemory", optionConfig.gameMemory),
                        new XElement("GameVersion", optionConfig.gameVersion),
                        new XElement("GameX64", optionConfig.gameX64),
                        new XElement("FtpServerIPIndex", optionConfig.ftpServerIndex)
                    )
                );
            optionConfigXmlFile.Save(optionXmlPath, SaveOptions.None);
        }

        //读取数据
        public void OptionConfigLoad()
        {
            optionConfigXmlFile = XElement.Load(optionXmlPath);
            if (optionConfigXmlFile != null)
            {
                XElement Setting = optionConfigXmlFile.Element("Setting");
                XElement Value;
                if (Setting != null)
                {
                    Value = Setting.Element("GameName");
                    if (Value != null)
                    {
                        optionConfig.gameName = Value.Value;
                    }
                    else
                    {
                        optionConfig.gameName = "MieMiePlayer";
                    }

                    Value = Setting.Element("GameJavaPath");
                    if (Value != null)
                    {
                        optionConfig.gameJavaPath = Value.Value;
                    }
                    else
                    {
                        String[] preDefJavaPath ={"C:\\Program Files\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Program Files\\Java\\jre6\\bin\\javaw.exe",
                                    "C:\\Program Files (x86)\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Program Files (x86)\\Java\\jre6\\bin\\javaw.exe",
                                    "C:\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Java\\jre6\\bin\\javaw.exe"};
                        foreach (string javaPath in preDefJavaPath)
                        {
                            if (File.Exists(javaPath))
                            {
                                optionConfig.gameJavaPath = javaPath;
                                break;
                            }
                        }
                        if (optionConfig.gameJavaPath == null)
                        {
                            MessageBox.Show("自动匹配Java失败，请手动选择", "Java路径错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            optionConfig.gameJavaPath = "";
                        }
                    }

                    Value = Setting.Element("GameMemory");
                    if (Value != null)
                    {
                        optionConfig.gameMemory = Value.Value;

                        if (!Regex.IsMatch(optionConfig.gameMemory, @"^\d+$"))
                        {
                            MessageBox.Show("内存大小设置错误，重置为1024MB。", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            optionConfig.gameMemory = "1024";
                        }
                    }
                    else
                    {
                        optionConfig.gameMemory = "1024";
                    }

                    Value = Setting.Element("GameVersion");
                    if (Value != null)
                    {
                        optionConfig.gameVersion = Value.Value;
                    }
                    else
                    {
                        optionConfig.gameVersion = "";
                    }

                    Value = Setting.Element("GameX64");
                    if (Value != null)
                    {
                        if (Value.Value == "false")
                            optionConfig.gameX64 = false;
                        else
                            optionConfig.gameX64 = true;
                    }
                    else
                    {
                        optionConfig.gameX64 = false;
                    }

                    Value = Setting.Element("FtpServerIPIndex");
                    if (Value != null)
                    {
                        optionConfig.ftpServerIndex = int.Parse(Value.Value);
                    }
                    else
                    {
                        optionConfig.ftpServerIndex = 0;
                    }
                    if (optionConfig.ftpServerIndex == 0)
                        optionConfig.ftpServerIP = ftpServerIPA;
                    else
                        optionConfig.ftpServerIP = ftpServerIPB;
                }
            }
        }

        //应用设置
        public void OptionConfigUseSet(MieMieSetting Config)
        {
            this.textBoxPlayerName.Text = Config.gameName;
            this.textBoxJavaPath.Text = Config.gameJavaPath;
            this.textBoxMemory.Text = Config.gameMemory;
        }
        #endregion

        #region 启动参数生成

        String[] nativeDll = { "jinput-dx8.dll", 
                               "jinput-dx8_64.dll",
                               "jinput-raw.dll",
                               "jinput-raw_64.dll",
                               "jinput-wintab.dll",
                               "lwjgl.dll",
                               "lwjgl64.dll",
                               "OpenAL32.dll",
                               "OpenAL64.dll" };

        //启动参数生成
        public string GameStartArguments()
        {
            string arguments = "";
            bool froge = Directory.Exists(minecraft + @"libraries\net\minecraftforge");
            string mainClass;
            string tweakClass;
            string skipCheck;

            //预设置
            if (froge)
            {
                mainClass = " net.minecraft.launchwrapper.Launch";
                tweakClass = " --tweakClass cpw.mods.fml.common.launcher.FMLTweaker";
                skipCheck = " -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true";
            }
            else
            {
                mainClass = " net.minecraft.client.main.Main";
                tweakClass = "";
                skipCheck = "";
            }

            //X64模式
            if (optionConfig.gameX64) arguments += " -d64";

            //启动内存大小
            arguments += " -Xmx" + optionConfig.gameMemory + "M";

            //参数加载
            arguments += " -Djava.library.path=\"" + minecraft + "assets\\natives\"" + skipCheck;
            
            if (!Directory.Exists(minecraft + "assets\\natives\"")) Directory.CreateDirectory(minecraft + "assets\\natives");
            foreach (string dll in nativeDll) 
                if (!File.Exists(minecraft + "assets\\natives\\" + dll))
                    if (!MieMieFtp.DownloadFile(minecraft + "assets\\natives" + dll, optionConfig.ftpServerIP + "Client/natives/" + dll, "下载本地运行库失败，请切换线路重试。"))
                    {
                        return "";
                    }
            
            //库文件加载
            arguments += " -cp \"";
            foreach (string jar in Directory.GetFiles(minecraft + "libraries", "*.jar", SearchOption.AllDirectories))
            {
                if (!Regex.IsMatch(jar, @".*[nN][aA][tT][iI][vV][eE][Ss].*"))
                {
                    arguments += jar + ";";
                }
            }

            //游戏及参数加载
            arguments += minecraft + @"versions\" + optionConfig.gameVersion + "\\" + optionConfig.gameVersion + ".jar\"";
            arguments += mainClass;
            arguments += " --username " + optionConfig.gameName + " --session no";
            arguments += " --version " + optionConfig.gameVersion;
            arguments += " --gameDir \"" + minecraft.Substring(0, minecraft.Length - 1) + "\" -assetsDir \"" + minecraft + "assets\"";
            arguments += tweakClass;

            //保存启动参数
            optionConfigXmlFile = XElement.Load(optionXmlPath);
            optionConfigXmlFile.Add(new XElement("StartOrder", arguments));
            optionConfigXmlFile.Save(optionXmlPath, SaveOptions.None);

            return arguments;
        }

        #endregion

        #region 操作指令

        //线路选择切换
        private void comboBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            optionConfig.ftpServerIndex = this.comboBoxLine.SelectedIndex;
            if (optionConfig.ftpServerIndex == 0)
                optionConfig.ftpServerIP = ftpServerIPA;
            else
                optionConfig.ftpServerIP = ftpServerIPB;
            OptionConfigSave();

#if Debug
#else
            //获取更新日志
            this.toolStripStatusLabel.Text = "日志下载中……";
            this.comboBoxLine.Enabled = false;
            this.buttonFullUpdata.Enabled = false;
            this.buttonSimpleUpdata.Enabled = false;
            this.buttonUpdataSkin.Enabled = false;
            this.buttonUpdataCloak.Enabled = false;
            this.backgroundWorkerLog.RunWorkerAsync();
#endif
        }

        //游戏选择切换
        private void comboBoxGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            optionConfig.gameVersion = this.comboBoxGame.Text;
            OptionConfigSave();
        }

        //删除选择游戏
        private void buttonDeleteGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除游戏版本" + this.comboBoxGame.Text + "？注意此过程无法还原！", "删除游戏版本", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(minecraft + @"versions\" + this.comboBoxGame.Text, true);
                    if (Directory.Exists(minecraft + @"libraries\" + this.comboBoxGame.Text))
                    {
                        Directory.Delete(minecraft + @"libraries\" + this.comboBoxGame.Text, true);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("删除失败，请检查该客户端是否正处于运行状态。", "无法删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show("删除失败，请检查该客户端是否正处于运行状态。", "无法删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.comboBoxGame.Items.RemoveAt(this.comboBoxGame.SelectedIndex);
            }
            if (this.comboBoxGame.Items.Count != 0)
            {
                this.comboBoxGame.SelectedIndex = 0;
                optionConfig.gameVersion = this.comboBoxGame.Text;
                OptionConfigSave();
            }
        }

        //启动游戏
        private void buttonStart_Click(object sender, EventArgs e)
        {
            //备份设置
            optionConfig.gameName = this.textBoxPlayerName.Text;
            if (!File.Exists(optionConfig.gameJavaPath))
            {
                MessageBox.Show("无效的Java路径，请重新设置。", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            optionConfig.gameJavaPath = this.textBoxJavaPath.Text;
            optionConfig.gameX64 = this.checkBoxX64.Checked;

            if (!Regex.IsMatch(optionConfig.gameMemory, @"^\d+$"))
            {
                MessageBox.Show("内存大小设置错误，请重新设置(已重置为1024MB)。", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.textBoxMemory.Text = "1024";
                optionConfig.gameMemory = this.textBoxMemory.Text;
                return;
            }
            optionConfig.gameMemory = this.textBoxMemory.Text;

            //启动设置
            System.Diagnostics.ProcessStartInfo startJava = new System.Diagnostics.ProcessStartInfo();
            startJava.FileName = this.textBoxJavaPath.Text;
            startJava.Arguments = GameStartArguments();
            if (startJava.Arguments == "")
            {
                return;
            }
            System.Diagnostics.Process.Start(startJava);
            Application.Exit();
        }

        //退出程序
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //获取Java路径
        private void buttonJavaPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog javaPath = new System.Windows.Forms.OpenFileDialog();
            javaPath.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            javaPath.Filter = "Java|javaw.exe";
            if (javaPath.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("无效的Java路径，请重新设置。", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.textBoxJavaPath.Text = "";
                return;
            }
            optionConfig.gameJavaPath = javaPath.FileName;
            this.textBoxJavaPath.Text = javaPath.FileName;
        }

        //获取更新日志
        private void backgroundWorkerLog_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MieMieFtp.DownloadFile(applicationPath + "\\ChangeLog.rtf", optionConfig.ftpServerIP + "Client/ChangeLog.rtf", "下载更新日志失败,请切换到其他线路尝试。"))
            {
                this.backgroundWorkerLog.ReportProgress(0, null);
                if (MieMieFtp.DownloadFile(applicationPath + "\\Updata.xml", optionConfig.ftpServerIP + "Client/Updata.xml", "下载更新列表失败,请切换到其他线路尝试。"))
                    this.backgroundWorkerLog.ReportProgress(2, null);
                else
                    this.backgroundWorkerLog.ReportProgress(3, null);
            }
            else
                this.backgroundWorkerLog.ReportProgress(1, null);
        }

        //日志进程报告
        private void backgroundWorkerLog_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                if (File.Exists(applicationPath + "\\ChangeLog.rtf")) this.richTextBox1.LoadFile(applicationPath + "\\ChangeLog.rtf");
                this.toolStripStatusLabel.Text = "更新日志下载成功。";
                return;
            }
            else if (e.ProgressPercentage == 1)
            {
                this.richTextBox1.Text = "";
                this.toolStripStatusLabel.Text = "更新日志下载失败。";
                this.buttonFullUpdata.Enabled = false;
                this.buttonSimpleUpdata.Enabled = false;
                this.buttonUpdataSkin.Enabled = false;
                this.buttonUpdataCloak.Enabled = false;
            }
            else if (e.ProgressPercentage == 2)
            {
                this.toolStripStatusLabel.Text = "更新列表下载成功。";
                RootXEL = XElement.Load(applicationPath + "\\Updata.xml");
                File.Delete(applicationPath + "\\Updata.xml");
                this.buttonFullUpdata.Enabled = true;
                this.buttonSimpleUpdata.Enabled = true;
                this.buttonUpdataSkin.Enabled = true;
                this.buttonUpdataCloak.Enabled = true;
            }
            else
            {
                this.toolStripStatusLabel.Text = "更新列表下载失败。";
                this.buttonFullUpdata.Enabled = false;
                this.buttonSimpleUpdata.Enabled = false;
                this.buttonUpdataSkin.Enabled = false;
                this.buttonUpdataCloak.Enabled = false;
            }
            this.comboBoxLine.Enabled = true;
        }

        //文件目录类型
        public class FileFolderClass
        {
            public string Name;
            public string Ftp;
            public string Pc;
            public string Size;
            public string Type;

            public FileFolderClass(string name, string ftp, string pc, string size, string type)
            {
                Name = name;
                Ftp = ftp;
                Pc = pc;
                Size = size;
                Type = type;
            }
            public override string ToString()
            {
                if (Type == "File")
                    return String.Format("文件：{0} (服务器路径：{1} 电脑路径：{2} 大小：{3})", Path.GetFileName(Pc), Ftp, Pc, Size);
                else
                    return String.Format("目录：{0} (服务器路径：{1} 电脑路径：{2})", Path.GetFileName(Pc), Ftp, Pc);
            }
        }

        //比较方法类
        public class CompareClass : IEqualityComparer<FileFolderClass>
        {
            public bool Equals (FileFolderClass FFCA, FileFolderClass FFCB)
            {
                return (FFCA.Name == FFCB.Name) && (FFCA.Size == FFCB.Size);
            }
            public int GetHashCode(FileFolderClass FFC)
            {
                int s = System.Int32.Parse(FFC.Size);
                return System.Int32.Parse(FFC.Size);
            }
        }

        //递归处理文件
        public void UpadataFile(XElement parentXEL, DirectoryInfo parentDir, ref List<FileFolderClass> downloadFile)
        {
            List<FileFolderClass> ftpXEL = new List<FileFolderClass>();
            List<FileFolderClass> pcXEL = new List<FileFolderClass>();

            //生成基础文件列表
            foreach (XElement childXEL in parentXEL.Elements())
            {
                ftpXEL.Add(new FileFolderClass(childXEL.Attribute("PCPath").Value, childXEL.Attribute("FtpPath").Value, childXEL.Attribute("PCPath").Value, childXEL.Attribute("Size").Value, childXEL.Name.LocalName));
                if (childXEL.Name.LocalName == "Folder")
                    UpadataFile(childXEL, new DirectoryInfo(applicationPath + childXEL.Attribute("PCPath").Value), ref downloadFile);
            }
            if (parentDir.Exists)
            {
                foreach (FileInfo file in parentDir.GetFiles())
                {
                    pcXEL.Add(new FileFolderClass(file.FullName.Replace(applicationPath, ""), "", file.FullName.Replace(applicationPath, ""), file.Length.ToString(), "File"));
                }
                foreach (DirectoryInfo folder in parentDir.GetDirectories())
                {
                    pcXEL.Add(new FileFolderClass(folder.FullName.Replace(applicationPath, ""), "", folder.FullName.Replace(applicationPath, ""), "0", "Folder"));
                }
            }

            downloadFile.AddRange(ftpXEL.Except(pcXEL, new CompareClass()).Where(CFC => CFC.Type == "File"));

            if (parentXEL.Attribute("Synchronous") != null && parentXEL.Attribute("Synchronous").Value == "true")
            {
                foreach (FileFolderClass FFC in pcXEL.Except(ftpXEL, new CompareClass()))
                {
                    if (FFC.Type == "Folder")
                    {
                        Directory.Delete(applicationPath + FFC.Pc, true);
                    }
                    else
                    {
                        File.Delete(applicationPath + FFC.Pc);
                    }
                }
            }
        }

        //完整修复
        bool fullRepair = false;
        private void buttonFullUpdata_Click(object sender, EventArgs e)
        {
            
#if Debug
            RootXEL = XElement.Load(applicationPath + "\\Updata.xml");
#endif
            if (fullRepair)
            {
                this.buttonFullUpdata.Enabled = false;
                this.backgroundWorkerFullDownLoad.CancelAsync();
                this.toolStripStatusLabel.Text = "修复取消中……";
                fullRepair = false;
            }
            else
            {
                this.comboBoxLine.Enabled = false;
                this.buttonFullUpdata.Enabled = false;
                this.buttonSimpleUpdata.Enabled = false;
                this.buttonUpdataSkin.Enabled = false;
                this.buttonUpdataCloak.Enabled = false;
                if (MessageBox.Show("基本客户端可以在群共享内下载。\n完整修复主要目的是修复客户端，将会清除当前目录及子目录中全部与服务器内不同的文件。\n某些情况下相当于重新下载，这样容易出错且下载速度较慢，是否确认进行完整更新？", "修复游戏版本", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    this.comboBoxLine.Enabled = true;
                    this.buttonFullUpdata.Enabled = true;
                    this.buttonSimpleUpdata.Enabled = true;
                    this.buttonUpdataSkin.Enabled = true;
                    this.buttonUpdataCloak.Enabled = true;
                    return;
                }
                this.buttonFullUpdata.Enabled = true;
                this.buttonFullUpdata.Text = "取消修复";
                this.toolStripStatusLabel.Text = "检索修复内容……";
                this.backgroundWorkerFullDownLoad.RunWorkerAsync();
                fullRepair = true;
            }
        }

        //修复异步调用进程启动
        private void backgroundWorkerFullDownLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            XElement fullCheckXEL = RootXEL.Element("FullCheck");
            List<FileFolderClass> downloadFile = new List<FileFolderClass>();
            UpadataFile(fullCheckXEL, new DirectoryInfo(applicationPath), ref downloadFile);

            StreamWriter logWriter = new StreamWriter(applicationPath + "\\Updata.log", true);
            logWriter.Write("完整修复启动时间：" + DateTime.Now.ToLocalTime() + "\r\n");

            this.backgroundWorkerFullDownLoad.ReportProgress(-1, downloadFile.Count);
            int progress = 0;
            foreach (FileFolderClass ftpPath in downloadFile)
            {
                logWriter.Write(ftpPath.ToString() + "\r\n");
                this.backgroundWorkerFullDownLoad.ReportProgress(progress++, ftpPath.Pc);
                if (!MieMieFtp.DownloadFile(applicationPath + ftpPath.Pc, optionConfig.ftpServerIP + "Client/Download" + ftpPath.Ftp, "文件" + ftpPath.Pc + "下载失败，请（切换线路）重试。"))
                {
                    this.backgroundWorkerFullDownLoad.ReportProgress(-2, ftpPath.Pc);
                    return;
                }
                if (this.backgroundWorkerFullDownLoad.CancellationPending)
                {
                    this.backgroundWorkerFullDownLoad.ReportProgress(-3, ftpPath.Pc);
                    return;
                }
            }
            this.backgroundWorkerFullDownLoad.ReportProgress(-4);
            logWriter.Close();
        }

        //修复异步调用进程报告
        private void backgroundWorkerFullDownLoad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
            {
                this.progressBarMain.Maximum = (int)e.UserState;
            }
            else if (e.ProgressPercentage == -2)
            {
                this.toolStripStatusLabel.Text = "完整修复失败。";
                MessageBox.Show("文件" + e.UserState.ToString() + "下载失败，请（切换线路）重试。", "修复失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.buttonStart.Enabled = false;
            }
            else if (e.ProgressPercentage == -3)
            {
                this.toolStripStatusLabel.Text = "完整修复取消。";
                MessageBox.Show("完整修复成功取消。", "修复取消", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.buttonStart.Enabled = false;
            }
            else if (e.ProgressPercentage == -4)
            {
                this.toolStripStatusLabel.Text = "完整修复成功。";
                MessageBox.Show("完整修复成功，点击启动游戏开始咩咩之旅。", "修复完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.buttonStart.Enabled = true;
            }
            else
            {
                this.progressBarMain.Value = e.ProgressPercentage;
                this.toolStripStatusLabel.Text = "正在下载：" + e.UserState.ToString();
            }
        }

        //修复异步进程结束
        private void backgroundWorkerFullDownLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.comboBoxLine.Enabled = true;
            this.buttonFullUpdata.Enabled = true;
            this.buttonSimpleUpdata.Enabled = true;
            this.buttonUpdataSkin.Enabled = true;
            this.buttonUpdataCloak.Enabled = true;
            this.progressBarMain.Value = 0;
            this.buttonFullUpdata.Text = "完整修复";
            FormGameCheck();
            fullRepair = false;
        }
        
        //部分更新
        bool partUpdata = false;
        private void buttonSimpleUpdata_Click(object sender, EventArgs e)
        {
#if Debug
            RootXEL = XElement.Load(applicationPath + "\\Updata.xml");
#endif
            if (partUpdata)
            {
                this.buttonSimpleUpdata.Enabled = false;
                this.backgroundWorkerSimpleDownLoad.CancelAsync();
                this.toolStripStatusLabel.Text = "更新取消中……";
                partUpdata = false;
            }
            else
            {
                this.comboBoxLine.Enabled = false;
                this.buttonFullUpdata.Enabled = false;
                this.buttonSimpleUpdata.Enabled = false;
                this.buttonUpdataSkin.Enabled = false;
                this.buttonUpdataCloak.Enabled = false;
                if (MessageBox.Show("将要进行部分更新，如果更新后仍然无法使用，请点击完整修复来修复客户端，或者去群共享下载完整客户端后再次部分更新。", "部分更新", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    this.comboBoxLine.Enabled = true;
                    this.buttonFullUpdata.Enabled = true;
                    this.buttonSimpleUpdata.Enabled = true;
                    this.buttonUpdataSkin.Enabled = true;
                    this.buttonUpdataCloak.Enabled = true;
                    return;
                }
                this.buttonSimpleUpdata.Enabled = true;
                this.buttonSimpleUpdata.Text = "取消更新";
                this.toolStripStatusLabel.Text = "检索更新内容……";
                this.backgroundWorkerSimpleDownLoad.RunWorkerAsync();
                partUpdata = true;
            }
        }

        //部分更新异步进程开始
        private void backgroundWorkerSimpleDownLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            XElement SimpleUpdataXEL = RootXEL.Element("SimpleUpdata");
            List<FileFolderClass> downloadFile = new List<FileFolderClass>();
            UpadataFile(SimpleUpdataXEL, new DirectoryInfo(applicationPath), ref downloadFile);

            StreamWriter logWriter = new StreamWriter(applicationPath + "\\Updata.log", true);
            logWriter.Write("部分更新启动时间：" + DateTime.Now.ToLocalTime() + "\r\n");

            this.backgroundWorkerSimpleDownLoad.ReportProgress(-1, downloadFile.Count);
            int progress = 0;
            foreach (FileFolderClass ftpPath in downloadFile)
            {
                logWriter.Write(ftpPath.ToString() + "\r\n");
                this.backgroundWorkerSimpleDownLoad.ReportProgress(progress++, ftpPath.Pc);
                if (!MieMieFtp.DownloadFile(applicationPath + ftpPath.Pc, optionConfig.ftpServerIP + "Client/Download" + ftpPath.Ftp, "文件" + ftpPath.Pc + "下载失败，请（切换线路）重试。"))
                {
                    this.backgroundWorkerSimpleDownLoad.ReportProgress(-2, ftpPath.Pc);
                    return;
                }
                if (this.backgroundWorkerSimpleDownLoad.CancellationPending)
                {
                    this.backgroundWorkerSimpleDownLoad.ReportProgress(-3, ftpPath.Pc);
                    return;
                }
            }
            this.backgroundWorkerSimpleDownLoad.ReportProgress(-4);
            logWriter.Close();
        }

        //部分更新异步进程报告
        private void backgroundWorkerSimpleDownLoad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
            {
                this.progressBarMain.Maximum = (int)e.UserState;
            }
            else if (e.ProgressPercentage == -2)
            {
                this.toolStripStatusLabel.Text = "部分更新失败。";
                MessageBox.Show("文件" + e.UserState.ToString() + "下载失败，请（切换线路）重试。", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.buttonStart.Enabled = false;
            }
            else if (e.ProgressPercentage == -3)
            {
                this.toolStripStatusLabel.Text = "部分更新取消。";
                MessageBox.Show("部分更新成功取消。", "更新取消", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.buttonStart.Enabled = false;
            }
            else if (e.ProgressPercentage == -4)
            {
                this.toolStripStatusLabel.Text = "部分更新成功。";
                MessageBox.Show("部分更新成功，点击启动游戏开始咩咩之旅。", "修复完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.buttonStart.Enabled = true;
            }
            else
            {
                this.progressBarMain.Value = e.ProgressPercentage;
                this.toolStripStatusLabel.Text = "正在下载：" + e.UserState.ToString();
            }
        }

        //部分更新异步完成
        private void backgroundWorkerSimpleDownLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.comboBoxLine.Enabled = true;
            this.buttonFullUpdata.Enabled = true;
            this.buttonSimpleUpdata.Enabled = true;
            this.buttonUpdataSkin.Enabled = true;
            this.buttonUpdataCloak.Enabled = true;
            this.progressBarMain.Value = 0;
            this.buttonSimpleUpdata.Text = "部分更新";
            FormGameCheck();
            partUpdata = false;
        }

        //上传皮肤参数类 Type == true表示处理皮肤。
        private class UpdataSkinOrCape
        {
            public bool Type;
            public string PlayerName;
            public string PngPath;
            public UpdataSkinOrCape(bool type, string playerName, string pngPath)
            {
                Type = type;
                PlayerName = playerName;
                PngPath = pngPath;
            }
        }

        //上传皮肤
        private void buttonUpdataSkin_Click(object sender, EventArgs e)
        {
            ManageSkinAndCape(true);
        }

        //上传披风
        private void buttonUpdataCloak_Click(object sender, EventArgs e)
        {
            ManageSkinAndCape(false);
        }
        
        //皮肤与披风功能
        public void ManageSkinAndCape(bool skinOrCape)
        {
            string skinOrCapeString;
            if (skinOrCape)
            {
                skinOrCapeString = "皮肤";
            }
            else
            {
                skinOrCapeString = "披风";
            }
            DialogResult manageType = MessageBox.Show("选择修改或删除" + skinOrCapeString + "(是：选择" + skinOrCapeString + "上传。否：删除" + skinOrCapeString + "。取消：取消管理)。", "管理" + skinOrCapeString, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (manageType == DialogResult.Cancel)
            {
                return;
            }
            this.comboBoxLine.Enabled = false;
            this.buttonFullUpdata.Enabled = false;
            this.buttonSimpleUpdata.Enabled = false;
            this.buttonUpdataSkin.Enabled = false;
            this.buttonUpdataCloak.Enabled = false;

            if (manageType == DialogResult.No)
            {
                this.toolStripStatusLabel.Text = "正在删除" + skinOrCapeString + "……";
                this.backgroundWorkerDeleteSkinOrCape.RunWorkerAsync(new UpdataSkinOrCape(skinOrCape, this.textBoxPlayerName.Text, ""));
                return;
            }

            this.toolStripStatusLabel.Text = "正在上传" + skinOrCapeString + "……";
            System.Windows.Forms.OpenFileDialog pngPath = new System.Windows.Forms.OpenFileDialog();
            pngPath.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            pngPath.Filter = skinOrCapeString + "图片|*.png";
            if (pngPath.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            FileInfo pngFile = new FileInfo(pngPath.FileName);
            if (pngFile.Length > 297152)
            {
                MessageBox.Show("所选" + skinOrCapeString + "图片过大（要求小于2M），请选择其他图片。", "上传" + skinOrCapeString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.backgroundWorkerSkinOrCape.RunWorkerAsync(new UpdataSkinOrCape(skinOrCape, this.textBoxPlayerName.Text, pngPath.FileName));
        }

        //上传皮肤或披风异步开始
        private void backgroundWorkerSkinOrCape_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdataSkinOrCape argument = (UpdataSkinOrCape)e.Argument;
            string skinOrCapeString;
            if (argument.Type)
            {
                skinOrCapeString = "皮肤";
            }
            else
            {
                skinOrCapeString = "披风";
            }
            Image pngImage = Image.FromFile(argument.PngPath);
            if (pngImage.Width != pngImage.Height * 2)
            {
                MessageBox.Show("所选" + skinOrCapeString + "图片分辨率比例错误，要求长宽比为1:2，请选择正确图片。", "上传" + skinOrCapeString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Result = "上传" + skinOrCapeString + "失败。";
                return;
            }
            if (!MieMieFtp.DownloadFile(applicationPath + "\\white-list.txt", optionConfig.ftpServerIP + "white-list.txt", "白名单列表读取失败，请（切换线路）重试。"))
            {
                e.Result = "获取白名单失败。";
                return;
            }
            StreamReader whiteList = new StreamReader(applicationPath + "\\white-list.txt", Encoding.Default);
            string idName = whiteList.ReadLine();
            while (true)
            {
                if (idName == (argument.PlayerName))
                {
                    break;
                } 
                if (idName == null)
                {
                    MessageBox.Show("您的用户ID不在白名单中，无法使用上传" + skinOrCapeString + "功能", "上传" + skinOrCapeString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    whiteList.Close();
                    File.Delete(applicationPath + "\\white-list.txt");
                    e.Result = "上传" + skinOrCapeString + "失败。";
                    return;
                }
                idName = whiteList.ReadLine();
            }
            whiteList.Close();
            File.Delete(applicationPath + "\\white-list.txt");
            string ftpDownload;
            if (argument.Type)
            {
                ftpDownload = optionConfig.ftpServerIP + "Client/Skins/" + argument.PlayerName + ".png";
            }
            else                
            {
                ftpDownload = optionConfig.ftpServerIP + "Client/Capes/" + argument.PlayerName + ".png";
            }
            if (MieMieFtp.UploadFile(argument.PngPath, ftpDownload, "上传" + skinOrCapeString + "失败，请（切换线路）重试。"))
            {
                e.Result = "上传" + skinOrCapeString + "成功。";
            }
            else
            {
                e.Result = "上传" + skinOrCapeString + "失败。";
            }
        }


        //上传皮肤或披风异步结束
        private void backgroundWorkerSkinOrCape_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.comboBoxLine.Enabled = true;
            this.buttonFullUpdata.Enabled = true;
            this.buttonSimpleUpdata.Enabled = true;
            this.buttonUpdataSkin.Enabled = true;
            this.buttonUpdataCloak.Enabled = true;
            this.toolStripStatusLabel.Text = e.Result.ToString();
        }

        //删除皮肤或披风异步开始
        private void backgroundWorkerDeleteSkinOrCape_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdataSkinOrCape argument = (UpdataSkinOrCape)e.Argument;
            string skinOrCapeString;
            if (argument.Type)
            {
                skinOrCapeString = "皮肤";
            }
            else
            {
                skinOrCapeString = "披风";
            }
            if (!MieMieFtp.DownloadFile(applicationPath + "\\white-list.txt", optionConfig.ftpServerIP + "white-list.txt", "白名单列表读取失败，请（切换线路）重试。"))
            {
                e.Result = "获取白名单失败。";
                return;
            }
            StreamReader whiteList = new StreamReader(applicationPath + "\\white-list.txt", Encoding.Default);
            string idName = whiteList.ReadLine();
            while (true)
            {
                if (idName == (argument.PlayerName))
                {
                    break;
                }
                if (idName == null)
                {
                    MessageBox.Show("您的用户ID不在白名单中，无法使用删除" + skinOrCapeString + "功能", "删除" + skinOrCapeString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    whiteList.Close();
                    File.Delete(applicationPath + "\\white-list.txt");
                    e.Result = "删除" + skinOrCapeString + "失败。";
                    return;
                }
                idName = whiteList.ReadLine();
            }
            whiteList.Close();
            File.Delete(applicationPath + "\\white-list.txt");
            string ftpDownload;
            if (argument.Type)
            {
                ftpDownload = optionConfig.ftpServerIP + "Client/Skins/" + argument.PlayerName + ".png";
            }
            else
            {
                ftpDownload = optionConfig.ftpServerIP + "Client/Capes/" + argument.PlayerName + ".png";
            }
            if (MieMieFtp.DeleteFile(ftpDownload, "删除" + skinOrCapeString + "失败，请（切换线路）重试。"))
            {
                e.Result = "删除" + skinOrCapeString + "成功。";
            }
            else
            {
                e.Result = "删除" + skinOrCapeString + "失败。";
            }
        }

        //删除皮肤或披风异步结束
        private void backgroundWorkerDeleteSkinOrCape_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.comboBoxLine.Enabled = true;
            this.buttonFullUpdata.Enabled = true;
            this.buttonSimpleUpdata.Enabled = true;
            this.buttonUpdataSkin.Enabled = true;
            this.buttonUpdataCloak.Enabled = true;
            this.toolStripStatusLabel.Text = e.Result.ToString();
        }

        //退出窗口
        private void MieMieUpdata_FormClosing(object sender, FormClosingEventArgs e)
        {
            optionConfig.gameJavaPath = this.textBoxJavaPath.Text;
            optionConfig.gameMemory = this.textBoxMemory.Text;
            optionConfig.gameName = this.textBoxPlayerName.Text;
            optionConfig.gameJavaPath = this.textBoxJavaPath.Text;

            OptionConfigSave();
        }

        #endregion

        #region 设置数据类

        //设置数据类
        public class MieMieSetting
        {
            public string gameName;//玩家名
            public string gameJavaPath = null;//Java路径
            public string gameMemory;//使用内存
            public string gameVersion;//游戏
            public bool gameX64;//64位模式运行
            public int ftpServerIndex;
            public string ftpServerIP;//连接IP

            public void OptionConfigMieMieSetting()
            {
                String[] preDefJavaPath ={"C:\\Program Files\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Program Files\\Java\\jre6\\bin\\javaw.exe",
                                    "C:\\Program Files (x86)\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Program Files (x86)\\Java\\jre6\\bin\\javaw.exe",
                                    "C:\\Java\\jre7\\bin\\javaw.exe",
                                    "C:\\Java\\jre6\\bin\\javaw.exe"};
                gameName = "MieMiePlayer";
                foreach (string javaPath in preDefJavaPath)
                {
                    if (File.Exists(javaPath))
                    {
                        gameJavaPath = javaPath;
                        break;
                    }
                }
                if (gameJavaPath == null)
                {
                    MessageBox.Show("自动匹配Java失败，请手动选择", "设置错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gameJavaPath = "";
                }
                gameMemory = "1024";
                gameVersion = "";
                gameX64 = false;
                ftpServerIndex = 0;
                ftpServerIP = ftpServerIPA;
            }
        }

        #endregion

        #region FTP主类
        //FTP主类
        public class FTPClass
        {

            FtpWebRequest reqFTP;
            private void Connect(string path)//连接Ftp
            {
                //根据url创建FtpWebRequest对象
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));

                //指定数据传输类型
                reqFTP.UseBinary = true;

                //ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                //自动关闭连接
                reqFTP.KeepAlive = false;
            }

            //从ftp服务器下载文件的功能
            public bool DownloadFile(string pcPath, string ftpPath, string errorInfo)
            {
                try
                {
                    if (Directory.Exists(Path.GetDirectoryName(pcPath)))
                    {
                        if (File.Exists(pcPath)) File.Delete(pcPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(pcPath));
                    }
                    Connect(ftpPath);//连接 
                    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream ftpStream = response.GetResponseStream();
                    long cl = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];
                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                    FileStream outputStream = new FileStream(pcPath, FileMode.Create);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }
                    ftpStream.Close();
                    outputStream.Close();
                    response.Close();
                    return true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(errorInfo + ex.Message, "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            //上传文件
            public bool UploadFile(string pcPath, string ftpPath, string errorInfo)
            {
                try
                {
                    //检测文件存在
                    bool fileExist = false;
                    string filePath = ftpPath.Substring(0, ftpPath.LastIndexOf("/"));
                    string fileName = ftpPath.Substring(ftpPath.LastIndexOf("/") + 1);
                    Connect(filePath);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line == fileName)
                        {
                            fileExist = true;
                            break;
                        }
                        line = reader.ReadLine();
                    }
                    reader.Close();
                    response.Close();

                    //删除文件
                    if (fileExist)
                    {
                        Connect(ftpPath);
                        reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                        response = (FtpWebResponse)reqFTP.GetResponse();
                        response.Close();
                    }

                    //上传文件
                    FileInfo pcFile = new FileInfo(pcPath);
                    Connect(ftpPath);
                    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    reqFTP.ContentLength = pcFile.Length;
                    int bufferSize = 2048;
                    byte[] buff = new byte[bufferSize];
                    int contentLen;
                    FileStream fileStream = pcFile.OpenRead();

                    Stream strm = reqFTP.GetRequestStream();
                    contentLen = fileStream.Read(buff, 0, bufferSize);

                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = fileStream.Read(buff, 0, bufferSize);
                    }

                    strm.Close();
                    fileStream.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(errorInfo + ex.Message, "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            //删除文件
            public bool DeleteFile(string ftpPath, string errorInfo)
            {
                try
                {
                    Connect(ftpPath);
                    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    response.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(errorInfo + ex.Message, "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        #endregion

        #region 解密FTP设置
        //解密
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
        string[] keys = {"4126c36b0e581dfe373604c24827955f",
                         "2b755840b80f859ddf8caed5759aa78f",
                         "8c9ddcc5ebaec870665f55779172a5a9",
                         "b304827a877bca31a0268c81aa47899d",
                         "8e7536aa4bd0737dace7d958ba0e0b6d",
                         "e7042170182d0710d3ab24c52146c61f",
                         "d5b8e81c3cfb5de39004cd9fa2717692",
                         "1a1e9aa5261885616d25d7a0acae5b66",
                         "21ba830225ee31eca73de01360686d61",
                         "3491aa03ab20f8bd3287af1623bd37e2",
                         "a9667ee8ee746d2bedaefee838710004",
                         "b7766c1a0358ba00c1c5cf9d3f1ac8ea",
                         "06fe6af9ea8572bf247f15fd221084c1",
                         "87aa5fad511dde51ac7d2728a5cfecd3",
                         "e614e23a07061a3203acf68442bcdeb0",
                         "410d1694bb1a98e91f194d6d5d4a1a54",
                         "9241170d2d4aba80902caaa4265a0eef",
                         "9e76e797b95718cb7bf91aeeb9add5b9",
                         "91a2f100a5d182f65396048e70f4c2c5",
                         "0a5ecee662839b3a294c91342fcbc7e3",
                         "57f01384680d49cdf3f1432253895a87",
                         "3755e35adeee570e06b6d7593f4ec687",
                         "cf21377fe03d5cfc1400f2714078e7a0",
                         "f10afdd9e86c046999057db136f08ea5",
                         "2abb533252daa6f9f9e646defbe5cd8e",
                         "cf37673451ec9f621dcee6dbc0aac624",
                         "724d468425b44d166304e00125266db3",
                         "37efe2ff8859543d1bf01583f4cb384e",
                         "20226bae9e48681ae951d14c0b9e3cf7",
                         "6032a3d0c8c79b18f90969349fca2298",
                         "cb92e103babf15ee86c716378807317d",
                         "b31e23c1276885588653b495df347b1f",
                         "2e388179bc383ca11db54900b1648ff1",
                         "d8ec8081a66070630c887d660a7f0f52",
                         "4cf7085085b8c07ae182a4721e84a721",
                         "c3c2bcf3b3f978cdc71e6c34bbd5969d",
                         "a6f7b4ba5b3f6c202b069f24bbfe4488",
                         "e15ca347c21b9f527e3a88674b8cbf0b",
                         "eb07cb27d0edc584569cc7ac5a5311c9",
                         "d9e4525f324fda8a57613b20b704d49f",
                         "469439b4321db7f3961ef02654f2f82b",
                         "28be8e673960d89c2d41cf6fb2a5cd52",
                         "c82866c61a5d96bb2c5ec8145eb43665",
                         "680ff7f8756d65e6bb9c87f14e4841f1",
                         "519e5e2b8c3211cb28d33ada97c43e79",
                         "0a6a06a2add4dc5ac27f0a7dd6a46ce4",
                         "1dd7c2c0560ad835fe2ccfa2328b4ef6",
                         "5e0ee6e4419f447cc103690c2794f583",
                         "6656bdc0027d230091d0a48005392836",
                         "31141ec8be88a27e9345fe52a1cc0d6a",
                         "374b3cdcce8ef3e1eae65270fbc37787",
                         "da32db12762cc5ddb43e2cbc7ed05ec5",
                         "c33949b9fd4c68a2213cddd7353183cf",
                         "ce77776ef0faf40a865f278f6137621a",
                         "878dadc17abe8a8ec938f1394feea783",
                         "bb5cee0758f0d4c8453e329d24d941e9",
                         "dac24fa883c25510ece9a4b809bbbb97",
                         "b7a4d304495ac297b9e57b3b8bec6af0",
                         "f7075e8c9ffffff907dd1a6f4b1cbf8a",
                         "839be57f053ca07f00b7ef9d471a84ff",
                         "7eb3c0015c2ff8b8a5dafd6b40534c9c",
                         "26f0b8f007eaebba67de8ce71062b2ec",
                         "581bf0975bbe2e54a22dda2b3f353fad",
                         "6b5276566194f894c806997da311a842",
                         "2710122463b44168c2644dda0fbf7b32",
                         "9fde7cbdeb5b8bc3225cce5424b0b091",
                         "8497032e221a49b6100e94fd471e8409",
                         "8b67dec494ea57cb854e0207e6ed4411",
                         "b3bc5578bb4c0f8494c5addcc78e7c04",
                         "d7c5c9bcd1ecfc993f37a9d31842d023",
                         "c982498ac80c4d3e585a2fcf3c7b04f7",
                         "d1fe2f20238740c6bfbc3883a33b6b51",
                         "f58a124444f23bcb788b6477bd2d5cc4",
                         "33b2011a13afff414282ca0cd3c99b1c",
                         "eeddd9b268c0b2dfc263a89823c80892",
                         "5f7350f039513e458ad101c1272ef7ae",
                         "ef4788e053aa5c72ff2257b007671e7a",
                         "7938ad6078aae29c9f942797bff0b893",
                         "6bf9a8dc40283eb516def117fa8d3d23",
                         "b28747ef8197364564eb41bf82c5b20f",
                         "6ac3440ebf423951f9f8499ab9cba211",
                         "5604e438a9c812d3c2627754e77f3251",
                         "2be2ad36492f63704df6f5894d9c8c99",
                         "4930687de17a65a1b0704b5ccae56a79",
                         "c5f86ed71e6bc727a073ebb2d3898526",
                         "d6dd0ab617b56fb07c0b85d8e307a667",
                         "8b7bc3552db47bdd6840dc6e8fb3cd27",
                         "ea2586ecdbb3621089bd5b12e5ef4499",
                         "cd832657c530f821622c4ed6d80121c8",
                         "3e5eedbd81b98cd818b3cc6f5968f778",
                         "fb42a00b8c7661ba2c648aae658c3787",
                         "9c92d48701871814f74e3e126489c8f9",
                         "34d5da9560163cc7fc33901c2edd728c",
                         "7b44d3c7947076742a8c133e374e56a3",
                         "9c907990b1a22786560c8acef6fbe51a",
                         "dbb2971fb60165c45e656bd402fe90ad",
                         "dded809e64e2dad91d4c7088e5a87e2e",
                         "a7f96284b736ed9b2bb4314c36538358",
                         "7bddf0087a47b5f9d8be016990dcdfb5",
                         "e63c2d099b0cf6a6ffe572afb08ea2ca"
};
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public void GetServersInformation()
        {
            XElement xElData = XElement.Load("ServersSetting.xml");
            int randomKey = Convert.ToInt32(xElData.Attribute("KEY").Value);
            int randomVI = Convert.ToInt32(xElData.Attribute("VI").Value);
            ftpServerIPA = "ftp://" + DecryptStringFromBytes_Aes(StringToByteArray(xElData.Element("LTIP").Value), StringToByteArray(keys[randomKey]), StringToByteArray(keys[randomVI])) + "/";
            ftpServerIPB = "ftp://" + DecryptStringFromBytes_Aes(StringToByteArray(xElData.Element("DXIP").Value), StringToByteArray(keys[randomKey]), StringToByteArray(keys[randomVI])) + "/";
            ftpUserID = DecryptStringFromBytes_Aes(StringToByteArray(xElData.Element("FTPID").Value), StringToByteArray(keys[randomKey]), StringToByteArray(keys[randomVI]));
            ftpPassword = DecryptStringFromBytes_Aes(StringToByteArray(xElData.Element("FTPPW").Value), StringToByteArray(keys[randomKey]), StringToByteArray(keys[randomVI]));
        }
        #endregion
    }
}
