namespace SocketAsync_Client
{
	partial class frmClient
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnAutoMsg = new System.Windows.Forms.Button();
			this.labID = new System.Windows.Forms.Label();
			this.btnSend = new System.Windows.Forms.Button();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.listMsg = new System.Windows.Forms.ListBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnImageSend = new System.Windows.Forms.Button();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtAutoMsg = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.pbDownImage = new System.Windows.Forms.PictureBox();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnLogin = new System.Windows.Forms.Button();
			this.txtID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnDir = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).BeginInit();
			this.SuspendLayout();
			// 
			// btnAutoMsg
			// 
			this.btnAutoMsg.Location = new System.Drawing.Point(443, 302);
			this.btnAutoMsg.Name = "btnAutoMsg";
			this.btnAutoMsg.Size = new System.Drawing.Size(87, 23);
			this.btnAutoMsg.TabIndex = 15;
			this.btnAutoMsg.Text = "자동 메시지";
			this.btnAutoMsg.UseVisualStyleBackColor = true;
			this.btnAutoMsg.Click += new System.EventHandler(this.btnAutoMsg_Click);
			// 
			// labID
			// 
			this.labID.Location = new System.Drawing.Point(12, 335);
			this.labID.Name = "labID";
			this.labID.Size = new System.Drawing.Size(100, 23);
			this.labID.TabIndex = 13;
			this.labID.Text = "ID 출력";
			this.labID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(443, 335);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(87, 23);
			this.btnSend.TabIndex = 12;
			this.btnSend.Text = "보내기";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// txtMsg
			// 
			this.txtMsg.Location = new System.Drawing.Point(118, 335);
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(319, 21);
			this.txtMsg.TabIndex = 9;
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(368, 12);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 148);
			this.listUser.TabIndex = 7;
			// 
			// listMsg
			// 
			this.listMsg.FormattingEnabled = true;
			this.listMsg.ItemHeight = 12;
			this.listMsg.Location = new System.Drawing.Point(12, 12);
			this.listMsg.Name = "listMsg";
			this.listMsg.Size = new System.Drawing.Size(350, 316);
			this.listMsg.TabIndex = 8;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			// 
			// btnImageSend
			// 
			this.btnImageSend.Location = new System.Drawing.Point(443, 360);
			this.btnImageSend.Name = "btnImageSend";
			this.btnImageSend.Size = new System.Drawing.Size(87, 23);
			this.btnImageSend.TabIndex = 17;
			this.btnImageSend.Text = "이미지 전송";
			this.btnImageSend.UseVisualStyleBackColor = true;
			this.btnImageSend.Click += new System.EventHandler(this.btnImageSend_Click);
			// 
			// txtDir
			// 
			this.txtDir.Location = new System.Drawing.Point(12, 363);
			this.txtDir.Name = "txtDir";
			this.txtDir.Size = new System.Drawing.Size(386, 21);
			this.txtDir.TabIndex = 16;
			this.txtDir.Text = "C:\\Users\\Persephone\\Pictures\\짤빵\\제니퍼로렌스_001.jpg";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(188, 389);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 23);
			this.label1.TabIndex = 14;
			this.label1.Text = "Port";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtAutoMsg
			// 
			this.txtAutoMsg.Location = new System.Drawing.Point(368, 304);
			this.txtAutoMsg.Name = "txtAutoMsg";
			this.txtAutoMsg.Size = new System.Drawing.Size(75, 21);
			this.txtAutoMsg.TabIndex = 10;
			this.txtAutoMsg.Text = "자동 메시지";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(224, 389);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 11;
			this.txtPort.Text = "9000";
			// 
			// pbDownImage
			// 
			this.pbDownImage.Location = new System.Drawing.Point(368, 160);
			this.pbDownImage.Name = "pbDownImage";
			this.pbDownImage.Size = new System.Drawing.Size(165, 138);
			this.pbDownImage.TabIndex = 18;
			this.pbDownImage.TabStop = false;
			// 
			// txtIP
			// 
			this.txtIP.Location = new System.Drawing.Point(49, 389);
			this.txtIP.Name = "txtIP";
			this.txtIP.Size = new System.Drawing.Size(133, 21);
			this.txtIP.TabIndex = 19;
			this.txtIP.Text = "127.0.0.1";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 389);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 23);
			this.label2.TabIndex = 14;
			this.label2.Text = "IP";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(443, 387);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(87, 23);
			this.btnLogin.TabIndex = 20;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(341, 389);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(96, 21);
			this.txtID.TabIndex = 11;
			this.txtID.Text = "test";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(305, 387);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 23);
			this.label3.TabIndex = 14;
			this.label3.Text = "ID";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnDir
			// 
			this.btnDir.Location = new System.Drawing.Point(404, 360);
			this.btnDir.Name = "btnDir";
			this.btnDir.Size = new System.Drawing.Size(33, 23);
			this.btnDir.TabIndex = 21;
			this.btnDir.Text = "...";
			this.btnDir.UseVisualStyleBackColor = true;
			// 
			// frmClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(541, 416);
			this.Controls.Add(this.btnDir);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtIP);
			this.Controls.Add(this.pbDownImage);
			this.Controls.Add(this.btnAutoMsg);
			this.Controls.Add(this.labID);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.txtMsg);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listMsg);
			this.Controls.Add(this.btnImageSend);
			this.Controls.Add(this.txtDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtAutoMsg);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.txtPort);
			this.Name = "frmClient";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAutoMsg;
		private System.Windows.Forms.Label labID;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.ListBox listMsg;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnImageSend;
		private System.Windows.Forms.TextBox txtDir;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAutoMsg;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.PictureBox pbDownImage;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnDir;
	}
}

