<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmXCIcutter
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmXCIcutter))
        Me.btnSourceDialog = New System.Windows.Forms.Button()
        Me.btnDestinationDialog = New System.Windows.Forms.Button()
        Me.txtSourcePath = New System.Windows.Forms.TextBox()
        Me.txtDestinationPath = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.btnCut = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblCartsize = New System.Windows.Forms.Label()
        Me.lblUsedspace = New System.Windows.Forms.Label()
        Me.txtSourceSize = New System.Windows.Forms.MaskedTextBox()
        Me.txtDestSize = New System.Windows.Forms.TextBox()
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.progressTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblMode = New System.Windows.Forms.Label()
        Me.rdbCut = New System.Windows.Forms.RadioButton()
        Me.rdbUncut = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'btnSourceDialog
        '
        Me.btnSourceDialog.Location = New System.Drawing.Point(12, 90)
        Me.btnSourceDialog.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSourceDialog.Name = "btnSourceDialog"
        Me.btnSourceDialog.Size = New System.Drawing.Size(196, 25)
        Me.btnSourceDialog.TabIndex = 0
        Me.btnSourceDialog.Text = "Source"
        Me.btnSourceDialog.UseVisualStyleBackColor = True
        '
        'btnDestinationDialog
        '
        Me.btnDestinationDialog.Location = New System.Drawing.Point(12, 132)
        Me.btnDestinationDialog.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnDestinationDialog.Name = "btnDestinationDialog"
        Me.btnDestinationDialog.Size = New System.Drawing.Size(196, 25)
        Me.btnDestinationDialog.TabIndex = 1
        Me.btnDestinationDialog.Text = "Destination"
        Me.btnDestinationDialog.UseVisualStyleBackColor = True
        '
        'txtSourcePath
        '
        Me.txtSourcePath.Location = New System.Drawing.Point(213, 90)
        Me.txtSourcePath.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtSourcePath.Name = "txtSourcePath"
        Me.txtSourcePath.Size = New System.Drawing.Size(376, 22)
        Me.txtSourcePath.TabIndex = 2
        '
        'txtDestinationPath
        '
        Me.txtDestinationPath.Location = New System.Drawing.Point(213, 132)
        Me.txtDestinationPath.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDestinationPath.Name = "txtDestinationPath"
        Me.txtDestinationPath.Size = New System.Drawing.Size(376, 22)
        Me.txtDestinationPath.TabIndex = 3
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "XCI-File|*.xci|All FIles|*.*"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "XCI-File|*.xci|All FIles|*.*"
        '
        'btnCut
        '
        Me.btnCut.Location = New System.Drawing.Point(239, 194)
        Me.btnCut.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCut.Name = "btnCut"
        Me.btnCut.Size = New System.Drawing.Size(169, 55)
        Me.btnCut.TabIndex = 4
        Me.btnCut.Text = "Cut!"
        Me.btnCut.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(421, 194)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(169, 55)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'lblCartsize
        '
        Me.lblCartsize.AutoSize = True
        Me.lblCartsize.Location = New System.Drawing.Point(15, 198)
        Me.lblCartsize.Name = "lblCartsize"
        Me.lblCartsize.Size = New System.Drawing.Size(95, 17)
        Me.lblCartsize.TabIndex = 6
        Me.lblCartsize.Text = "Cartridge size"
        '
        'lblUsedspace
        '
        Me.lblUsedspace.AutoSize = True
        Me.lblUsedspace.Location = New System.Drawing.Point(15, 228)
        Me.lblUsedspace.Name = "lblUsedspace"
        Me.lblUsedspace.Size = New System.Drawing.Size(83, 17)
        Me.lblUsedspace.TabIndex = 7
        Me.lblUsedspace.Text = "Used space"
        '
        'txtSourceSize
        '
        Me.txtSourceSize.Enabled = False
        Me.txtSourceSize.Location = New System.Drawing.Point(117, 194)
        Me.txtSourceSize.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtSourceSize.Name = "txtSourceSize"
        Me.txtSourceSize.Size = New System.Drawing.Size(100, 22)
        Me.txtSourceSize.TabIndex = 8
        '
        'txtDestSize
        '
        Me.txtDestSize.Enabled = False
        Me.txtDestSize.Location = New System.Drawing.Point(117, 225)
        Me.txtDestSize.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDestSize.Name = "txtDestSize"
        Me.txtDestSize.Size = New System.Drawing.Size(100, 22)
        Me.txtDestSize.TabIndex = 9
        '
        'ProgressBar
        '
        Me.ProgressBar.ForeColor = System.Drawing.Color.SteelBlue
        Me.ProgressBar.Location = New System.Drawing.Point(9, 309)
        Me.ProgressBar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(581, 23)
        Me.ProgressBar.TabIndex = 10
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
        Me.BackgroundWorker.WorkerSupportsCancellation = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.SteelBlue
        Me.lblStatus.Location = New System.Drawing.Point(16, 277)
        Me.lblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 17)
        Me.lblStatus.TabIndex = 11
        '
        'progressTimer
        '
        Me.progressTimer.Interval = 1000
        '
        'lblMode
        '
        Me.lblMode.AutoSize = True
        Me.lblMode.Location = New System.Drawing.Point(180, 34)
        Me.lblMode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMode.Name = "lblMode"
        Me.lblMode.Size = New System.Drawing.Size(47, 17)
        Me.lblMode.TabIndex = 12
        Me.lblMode.Text = "Mode:"
        '
        'rdbCut
        '
        Me.rdbCut.AutoSize = True
        Me.rdbCut.Checked = True
        Me.rdbCut.Location = New System.Drawing.Point(252, 32)
        Me.rdbCut.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdbCut.Name = "rdbCut"
        Me.rdbCut.Size = New System.Drawing.Size(50, 21)
        Me.rdbCut.TabIndex = 13
        Me.rdbCut.TabStop = True
        Me.rdbCut.Text = "Cut"
        Me.rdbCut.UseVisualStyleBackColor = True
        '
        'rdbUncut
        '
        Me.rdbUncut.AutoSize = True
        Me.rdbUncut.Location = New System.Drawing.Point(331, 32)
        Me.rdbUncut.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdbUncut.Name = "rdbUncut"
        Me.rdbUncut.Size = New System.Drawing.Size(66, 21)
        Me.rdbUncut.TabIndex = 14
        Me.rdbUncut.Text = "Uncut"
        Me.rdbUncut.UseVisualStyleBackColor = True
        '
        'frmXCIcutter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 347)
        Me.Controls.Add(Me.rdbUncut)
        Me.Controls.Add(Me.rdbCut)
        Me.Controls.Add(Me.lblMode)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.txtDestSize)
        Me.Controls.Add(Me.txtSourceSize)
        Me.Controls.Add(Me.lblUsedspace)
        Me.Controls.Add(Me.lblCartsize)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnCut)
        Me.Controls.Add(Me.txtDestinationPath)
        Me.Controls.Add(Me.txtSourcePath)
        Me.Controls.Add(Me.btnDestinationDialog)
        Me.Controls.Add(Me.btnSourceDialog)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmXCIcutter"
        Me.Text = "XCI Cutter"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSourceDialog As Button
    Friend WithEvents btnDestinationDialog As Button
    Friend WithEvents txtSourcePath As TextBox
    Friend WithEvents txtDestinationPath As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents btnCut As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents lblCartsize As Label
    Friend WithEvents lblUsedspace As Label
    Friend WithEvents txtSourceSize As MaskedTextBox
    Friend WithEvents txtDestSize As TextBox
    Friend WithEvents ProgressBar As ProgressBar
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblStatus As Label
    Friend WithEvents progressTimer As Timer
    Friend WithEvents lblMode As Label
    Friend WithEvents rdbCut As RadioButton
    Friend WithEvents rdbUncut As RadioButton
End Class
