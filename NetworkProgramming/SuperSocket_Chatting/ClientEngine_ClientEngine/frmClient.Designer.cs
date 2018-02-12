namespace ClientEngine_ClientEngine
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
			this.button1 = new System.Windows.Forms.Button();
			this.labID = new System.Windows.Forms.Label();
			this.btnSend = new System.Windows.Forms.Button();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.listMsg = new System.Windows.Forms.ListBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.txtAutoMsg = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(360, 268);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(81, 23);
			this.button1.TabIndex = 13;
			this.button1.Text = "자동 메시지";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// labID
			// 
			this.labID.Location = new System.Drawing.Point(4, 326);
			this.labID.Name = "labID";
			this.labID.Size = new System.Drawing.Size(100, 23);
			this.labID.TabIndex = 11;
			this.labID.Text = "ID 출력";
			this.labID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(447, 326);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 10;
			this.btnSend.Text = "로그인";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// txtMsg
			// 
			this.txtMsg.Location = new System.Drawing.Point(110, 326);
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(331, 21);
			this.txtMsg.TabIndex = 7;
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(360, 3);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 256);
			this.listUser.TabIndex = 5;
			// 
			// listMsg
			// 
			this.listMsg.FormattingEnabled = true;
			this.listMsg.ItemHeight = 12;
			this.listMsg.Location = new System.Drawing.Point(4, 3);
			this.listMsg.Name = "listMsg";
			this.listMsg.Size = new System.Drawing.Size(350, 316);
			this.listMsg.TabIndex = 6;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(358, 294);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 23);
			this.label1.TabIndex = 12;
			this.label1.Text = "Port";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtAutoMsg
			// 
			this.txtAutoMsg.Location = new System.Drawing.Point(447, 268);
			this.txtAutoMsg.Name = "txtAutoMsg";
			this.txtAutoMsg.Size = new System.Drawing.Size(75, 21);
			this.txtAutoMsg.TabIndex = 8;
			this.txtAutoMsg.Text = "자동 메시지";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(447, 296);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(75, 21);
			this.txtPort.TabIndex = 9;
			this.txtPort.Text = "8000";
			// 
			// frmClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 355);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.labID);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.txtMsg);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listMsg);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtAutoMsg);
			this.Controls.Add(this.txtPort);
			this.Name = "frmClient";
			this.Text = "ClientEngine_ClientEngine";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label labID;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.ListBox listMsg;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAutoMsg;
		private System.Windows.Forms.TextBox txtPort;

	}
}

