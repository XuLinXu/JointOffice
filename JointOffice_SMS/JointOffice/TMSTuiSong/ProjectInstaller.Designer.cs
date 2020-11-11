namespace TMSTuiSongJointOffice
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller2 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller2 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller2
            // 
            this.serviceProcessInstaller2.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller2.Password = null;
            this.serviceProcessInstaller2.Username = null;
            this.serviceProcessInstaller2.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller2_AfterInstall);
            // 
            // serviceInstaller2
            // 
            this.serviceInstaller2.Description = "T模式印工社微信推送";
            this.serviceInstaller2.DisplayName = "TMoShiTuiSongJointOfficeService";
            this.serviceInstaller2.ServiceName = "JointOfficeRiQingService";
            this.serviceInstaller2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller2,
            this.serviceInstaller2});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller2;
        private System.ServiceProcess.ServiceInstaller serviceInstaller2;
    }
}