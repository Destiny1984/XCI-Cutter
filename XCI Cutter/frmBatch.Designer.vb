<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatch))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstFilelist = New System.Windows.Forms.ListBox()
        Me.btnAddFolder = New System.Windows.Forms.Button()
        Me.pnlSettings = New System.Windows.Forms.Panel()
        Me.lblSettings = New System.Windows.Forms.Label()
        Me.rdbUncut = New System.Windows.Forms.RadioButton()
        Me.chkDelete = New System.Windows.Forms.CheckBox()
        Me.rdbCut = New System.Windows.Forms.RadioButton()
        Me.chkSplit = New System.Windows.Forms.CheckBox()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblCurrent = New System.Windows.Forms.Label()
        Me.prgTotal = New System.Windows.Forms.ProgressBar()
        Me.prgCurrent = New System.Windows.Forms.ProgressBar()
        Me.btnAddFiles = New System.Windows.Forms.Button()
        Me.btnStartBatch = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstFilelist)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnAddFolder)
        Me.SplitContainer1.Panel2.Controls.Add(Me.pnlSettings)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblTotal)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblCurrent)
        Me.SplitContainer1.Panel2.Controls.Add(Me.prgTotal)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnStartBatch)
        Me.SplitContainer1.Panel2.Controls.Add(Me.prgCurrent)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnAddFiles)
        Me.SplitContainer1.Panel2MinSize = 100
        Me.SplitContainer1.Size = New System.Drawing.Size(496, 698)
        Me.SplitContainer1.SplitterDistance = 523
        Me.SplitContainer1.SplitterWidth = 1
        Me.SplitContainer1.TabIndex = 1
        '
        'lstFilelist
        '
        Me.lstFilelist.AllowDrop = True
        Me.lstFilelist.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFilelist.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstFilelist.FormattingEnabled = True
        Me.lstFilelist.HorizontalScrollbar = True
        Me.lstFilelist.ItemHeight = 20
        Me.lstFilelist.Location = New System.Drawing.Point(0, 0)
        Me.lstFilelist.Name = "lstFilelist"
        Me.lstFilelist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFilelist.Size = New System.Drawing.Size(496, 523)
        Me.lstFilelist.TabIndex = 0
        '
        'btnAddFolder
        '
        Me.btnAddFolder.Location = New System.Drawing.Point(3, 43)
        Me.btnAddFolder.Name = "btnAddFolder"
        Me.btnAddFolder.Size = New System.Drawing.Size(125, 32)
        Me.btnAddFolder.TabIndex = 19
        Me.btnAddFolder.Text = "Add folder"
        Me.btnAddFolder.UseVisualStyleBackColor = True
        '
        'pnlSettings
        '
        Me.pnlSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSettings.Controls.Add(Me.lblSettings)
        Me.pnlSettings.Controls.Add(Me.rdbUncut)
        Me.pnlSettings.Controls.Add(Me.chkDelete)
        Me.pnlSettings.Controls.Add(Me.rdbCut)
        Me.pnlSettings.Controls.Add(Me.chkSplit)
        Me.pnlSettings.Location = New System.Drawing.Point(220, 3)
        Me.pnlSettings.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(273, 72)
        Me.pnlSettings.TabIndex = 18
        '
        'lblSettings
        '
        Me.lblSettings.AutoSize = True
        Me.lblSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSettings.Location = New System.Drawing.Point(13, 12)
        Me.lblSettings.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSettings.Name = "lblSettings"
        Me.lblSettings.Size = New System.Drawing.Size(42, 13)
        Me.lblSettings.TabIndex = 19
        Me.lblSettings.Text = "Mode:"
        '
        'rdbUncut
        '
        Me.rdbUncut.AutoSize = True
        Me.rdbUncut.Location = New System.Drawing.Point(155, 10)
        Me.rdbUncut.Name = "rdbUncut"
        Me.rdbUncut.Size = New System.Drawing.Size(84, 17)
        Me.rdbUncut.TabIndex = 17
        Me.rdbUncut.Text = "Uncut / Join"
        Me.rdbUncut.UseVisualStyleBackColor = True
        '
        'chkDelete
        '
        Me.chkDelete.AutoSize = True
        Me.chkDelete.Location = New System.Drawing.Point(155, 42)
        Me.chkDelete.Name = "chkDelete"
        Me.chkDelete.Size = New System.Drawing.Size(113, 17)
        Me.chkDelete.TabIndex = 1
        Me.chkDelete.Text = "Delete source files"
        Me.chkDelete.UseVisualStyleBackColor = True
        '
        'rdbCut
        '
        Me.rdbCut.AutoSize = True
        Me.rdbCut.Checked = True
        Me.rdbCut.Location = New System.Drawing.Point(62, 10)
        Me.rdbCut.Name = "rdbCut"
        Me.rdbCut.Size = New System.Drawing.Size(72, 17)
        Me.rdbCut.TabIndex = 16
        Me.rdbCut.TabStop = True
        Me.rdbCut.Text = "Cut / Split"
        Me.rdbCut.UseVisualStyleBackColor = True
        '
        'chkSplit
        '
        Me.chkSplit.AutoSize = True
        Me.chkSplit.Location = New System.Drawing.Point(62, 41)
        Me.chkSplit.Margin = New System.Windows.Forms.Padding(2)
        Me.chkSplit.Name = "chkSplit"
        Me.chkSplit.Size = New System.Drawing.Size(85, 17)
        Me.chkSplit.TabIndex = 2
        Me.chkSplit.Text = "4GB splitting"
        Me.chkSplit.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(7, 127)
        Me.lblTotal.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(0, 13)
        Me.lblTotal.TabIndex = 1
        '
        'lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrent.Location = New System.Drawing.Point(7, 82)
        Me.lblCurrent.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(0, 13)
        Me.lblCurrent.TabIndex = 1
        '
        'prgTotal
        '
        Me.prgTotal.Location = New System.Drawing.Point(4, 145)
        Me.prgTotal.Name = "prgTotal"
        Me.prgTotal.Size = New System.Drawing.Size(490, 23)
        Me.prgTotal.TabIndex = 1
        '
        'prgCurrent
        '
        Me.prgCurrent.Location = New System.Drawing.Point(4, 99)
        Me.prgCurrent.Name = "prgCurrent"
        Me.prgCurrent.Size = New System.Drawing.Size(490, 23)
        Me.prgCurrent.TabIndex = 3
        '
        'btnAddFiles
        '
        Me.btnAddFiles.Location = New System.Drawing.Point(3, 3)
        Me.btnAddFiles.Name = "btnAddFiles"
        Me.btnAddFiles.Size = New System.Drawing.Size(125, 32)
        Me.btnAddFiles.TabIndex = 2
        Me.btnAddFiles.Text = "Add file(s)"
        Me.btnAddFiles.UseVisualStyleBackColor = True
        '
        'btnStartBatch
        '
        Me.btnStartBatch.Location = New System.Drawing.Point(134, 3)
        Me.btnStartBatch.Name = "btnStartBatch"
        Me.btnStartBatch.Size = New System.Drawing.Size(81, 72)
        Me.btnStartBatch.TabIndex = 1
        Me.btnStartBatch.Text = "&Start"
        Me.btnStartBatch.UseVisualStyleBackColor = True
        '
        'frmBatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(496, 698)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmBatch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XCI-Cutter - Batch processing"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.pnlSettings.ResumeLayout(False)
        Me.pnlSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents lstFilelist As ListBox
    Friend WithEvents btnStartBatch As Button
    Friend WithEvents btnAddFiles As Button
    Friend WithEvents prgCurrent As ProgressBar
    Friend WithEvents prgTotal As ProgressBar
    Friend WithEvents chkDelete As CheckBox
    Friend WithEvents rdbUncut As RadioButton
    Friend WithEvents rdbCut As RadioButton
    Friend WithEvents chkSplit As CheckBox
    Friend WithEvents lblCurrent As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents pnlSettings As Panel
    Friend WithEvents lblSettings As Label
    Friend WithEvents btnAddFolder As Button
End Class
