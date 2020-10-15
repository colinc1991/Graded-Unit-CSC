namespace Tu_Stock_System
{
    partial class StockFind
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockFind));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.stockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStockToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.auditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSearchChoice = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.MaskedTextBox();
            this.radioButtonBarcode = new System.Windows.Forms.RadioButton();
            this.radioButtonStyleNum = new System.Windows.Forms.RadioButton();
            this.radioButtonSKU = new System.Windows.Forms.RadioButton();
            this.lblSelection = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblTitle.Location = new System.Drawing.Point(12, 78);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(163, 25);
            this.lblTitle.TabIndex = 63;
            this.lblTitle.Text = "Find stock location";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblHeader.Location = new System.Drawing.Point(7, 36);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(323, 37);
            this.lblHeader.TabIndex = 62;
            this.lblHeader.Text = "Tu Clothing stock system";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(132)))), ((int)(((byte)(14)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockToolStripMenuItem,
            this.locationToolStripMenuItem,
            this.addUserToolStripMenuItem,
            this.auditToolStripMenuItem,
            this.logOutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(393, 25);
            this.menuStrip1.TabIndex = 66;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // stockToolStripMenuItem
            // 
            this.stockToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewStockToolStripMenuItem,
            this.addStockToolStripMenuItem,
            this.findToolStripMenuItem,
            this.editToolStripMenuItem});
            this.stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            this.stockToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.stockToolStripMenuItem.Text = "Stock";
            // 
            // viewStockToolStripMenuItem
            // 
            this.viewStockToolStripMenuItem.Name = "viewStockToolStripMenuItem";
            this.viewStockToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.viewStockToolStripMenuItem.Text = "View";
            this.viewStockToolStripMenuItem.Click += new System.EventHandler(this.viewStockToolStripMenuItem_Click);
            // 
            // addStockToolStripMenuItem
            // 
            this.addStockToolStripMenuItem.Name = "addStockToolStripMenuItem";
            this.addStockToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.addStockToolStripMenuItem.Text = "Add";
            this.addStockToolStripMenuItem.Click += new System.EventHandler(this.addStockToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // locationToolStripMenuItem
            // 
            this.locationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.addToolStripMenuItem,
            this.addStockToolStripMenuItem1});
            this.locationToolStripMenuItem.Name = "locationToolStripMenuItem";
            this.locationToolStripMenuItem.Size = new System.Drawing.Size(69, 21);
            this.locationToolStripMenuItem.Text = "Location";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // addStockToolStripMenuItem1
            // 
            this.addStockToolStripMenuItem1.Name = "addStockToolStripMenuItem1";
            this.addStockToolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.addStockToolStripMenuItem1.Text = "Add stock";
            this.addStockToolStripMenuItem1.Click += new System.EventHandler(this.addStockToolStripMenuItem1_Click);
            // 
            // addUserToolStripMenuItem
            // 
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
            this.addUserToolStripMenuItem.Text = "Add user";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
            // 
            // auditToolStripMenuItem
            // 
            this.auditToolStripMenuItem.Name = "auditToolStripMenuItem";
            this.auditToolStripMenuItem.Size = new System.Drawing.Size(50, 21);
            this.auditToolStripMenuItem.Text = "Audit";
            this.auditToolStripMenuItem.Click += new System.EventHandler(this.auditToolStripMenuItem_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
            this.logOutToolStripMenuItem.Text = "Log out";
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(40, 21);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // lblSearchChoice
            // 
            this.lblSearchChoice.AutoSize = true;
            this.lblSearchChoice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchChoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(223)))), ((int)(((byte)(221)))));
            this.lblSearchChoice.Location = new System.Drawing.Point(9, 210);
            this.lblSearchChoice.Name = "lblSearchChoice";
            this.lblSearchChoice.Size = new System.Drawing.Size(152, 21);
            this.lblSearchChoice.TabIndex = 69;
            this.lblSearchChoice.Text = "Scan barcode below:";
            // 
            // btnFind
            // 
            this.btnFind.AutoEllipsis = true;
            this.btnFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.Color.Black;
            this.btnFind.Location = new System.Drawing.Point(12, 291);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(163, 37);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(12, 234);
            this.txtInput.Mask = "0000000000000";
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(163, 35);
            this.txtInput.TabIndex = 1;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // radioButtonBarcode
            // 
            this.radioButtonBarcode.AutoSize = true;
            this.radioButtonBarcode.Checked = true;
            this.radioButtonBarcode.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonBarcode.ForeColor = System.Drawing.Color.White;
            this.radioButtonBarcode.Location = new System.Drawing.Point(14, 156);
            this.radioButtonBarcode.Name = "radioButtonBarcode";
            this.radioButtonBarcode.Size = new System.Drawing.Size(82, 24);
            this.radioButtonBarcode.TabIndex = 70;
            this.radioButtonBarcode.TabStop = true;
            this.radioButtonBarcode.Text = "Barcode";
            this.radioButtonBarcode.UseVisualStyleBackColor = true;
            this.radioButtonBarcode.CheckedChanged += new System.EventHandler(this.radioButtonBarcode_CheckedChanged);
            // 
            // radioButtonStyleNum
            // 
            this.radioButtonStyleNum.AutoSize = true;
            this.radioButtonStyleNum.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonStyleNum.ForeColor = System.Drawing.Color.White;
            this.radioButtonStyleNum.Location = new System.Drawing.Point(104, 156);
            this.radioButtonStyleNum.Name = "radioButtonStyleNum";
            this.radioButtonStyleNum.Size = new System.Drawing.Size(117, 24);
            this.radioButtonStyleNum.TabIndex = 71;
            this.radioButtonStyleNum.Text = "Style Number";
            this.radioButtonStyleNum.UseVisualStyleBackColor = true;
            this.radioButtonStyleNum.CheckedChanged += new System.EventHandler(this.radioButtonStyleNum_CheckedChanged);
            // 
            // radioButtonSKU
            // 
            this.radioButtonSKU.AutoSize = true;
            this.radioButtonSKU.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSKU.ForeColor = System.Drawing.Color.White;
            this.radioButtonSKU.Location = new System.Drawing.Point(233, 156);
            this.radioButtonSKU.Name = "radioButtonSKU";
            this.radioButtonSKU.Size = new System.Drawing.Size(54, 24);
            this.radioButtonSKU.TabIndex = 72;
            this.radioButtonSKU.Text = "SKU";
            this.radioButtonSKU.UseVisualStyleBackColor = true;
            this.radioButtonSKU.CheckedChanged += new System.EventHandler(this.radioButtonSKU_CheckedChanged);
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(223)))), ((int)(((byte)(221)))));
            this.lblSelection.Location = new System.Drawing.Point(12, 123);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(257, 21);
            this.lblSelection.TabIndex = 73;
            this.lblSelection.Text = "Please choose how to search below:";
            // 
            // StockFind
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(393, 339);
            this.Controls.Add(this.lblSelection);
            this.Controls.Add(this.radioButtonSKU);
            this.Controls.Add(this.radioButtonStyleNum);
            this.Controls.Add(this.radioButtonBarcode);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblSearchChoice);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StockFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tu Stock System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StockFind_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem stockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addStockToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lblSearchChoice;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.MaskedTextBox txtInput;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem auditToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioButtonBarcode;
        private System.Windows.Forms.RadioButton radioButtonStyleNum;
        private System.Windows.Forms.RadioButton radioButtonSKU;
        private System.Windows.Forms.Label lblSelection;
    }
}

