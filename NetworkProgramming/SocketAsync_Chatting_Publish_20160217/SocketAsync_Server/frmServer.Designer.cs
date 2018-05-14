namespace SocketAsync_Server
{
	partial class frmServer
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
			this.pbDownImage = new System.Windows.Forms.PictureBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.listUser = new System.Windows.Forms.ListBox();
			this.listLog = new System.Windows.Forms.ListBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnImageSend = new System.Windows.Forms.Button();
			this.txtDir = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).BeginInit();
			this.SuspendLayout();
			// 
			// pbDownImage
			// 
			this.pbDownImage.Location = new System.Drawing.Point(320, 259);
			this.pbDownImage.Name = "pbDownImage";
			this.pbDownImage.Size = new System.Drawing.Size(165, 111);
			this.pbDownImage.TabIndex = 11;
			this.pbDownImage.TabStop = false;
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(200, 347);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 10;
			this.btnStop.Text = "서버 중지";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(320, 12);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 328);
			this.listUser.TabIndex = 9;
			// 
			// listLog
			// 
			this.listLog.FormattingEnabled = true;
			this.listLog.ItemHeight = 12;
			this.listLog.Location = new System.Drawing.Point(12, 12);
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(302, 328);
			this.listLog.TabIndex = 8;
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(12, 349);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(100, 21);
			this.txtPort.TabIndex = 7;
			this.txtPort.Text = "9000";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(118, 347);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "서버 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnImageSend
			// 
			this.btnImageSend.Location = new System.Drawing.Point(395, 376);
			this.btnImageSend.Name = "btnImageSend";
			this.btnImageSend.Size = new System.Drawing.Size(87, 23);
			this.btnImageSend.TabIndex = 19;
			this.btnImageSend.Text = "이미지 전송";
			this.btnImageSend.UseVisualStyleBackColor = true;
			this.btnImageSend.Click += new System.EventHandler(this.btnImageSend_Click);
			// 
			// txtDir
			// 
			this.txtDir.Location = new System.Drawing.Point(12, 376);
			this.txtDir.Name = "txtDir";
			this.txtDir.Size = new System.Drawing.Size(377, 21);
			this.txtDir.TabIndex = 18;
			this.txtDir.Text = "C:\\Users\\Persephone\\Pictures\\짤빵\\제니퍼로렌스_001.jpg";
			// 
			// frmServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 408);
			this.Controls.Add(this.btnImageSend);
			this.Controls.Add(this.txtDir);
			this.Controls.Add(this.pbDownImage);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listLog);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Name = "frmServer";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pbDownImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbDownImage;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.ListBox listLog;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnImageSend;
		private System.Windows.Forms.TextBox txtDir;
	}
}

