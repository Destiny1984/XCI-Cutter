Imports System.ComponentModel
Imports System.IO

Public Class frmXCIcutter
    Dim CurrentFile As XCIFile

    'Source-file dialog: Create XCIFile instance by dialog / call DisplaySizes()
    Private Sub btnSourceDialog_Click(sender As Object, e As EventArgs) Handles btnSourceDialog.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Try
                CurrentFile = New XCIFile(OpenFileDialog1.FileName)
                txtSourcePath.Text = OpenFileDialog1.FileName
                If rdbCut.Checked = True Then
                    SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName) & "-cut"
                Else
                    SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName) & "-uncut"
                End If
                DisplaySizes(CurrentFile)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    'Source-file textbox: Check for valid path / create XCIFile instance / call DisplaySizes()
    Private Sub txtSourcePath_Leave(sender As Object, e As EventArgs) Handles txtSourcePath.Leave
        If File.Exists(txtSourcePath.Text) Then
            Try
                CurrentFile = New XCIFile(txtSourcePath.Text)
                If rdbCut.Checked = True Then
                    SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtSourcePath.Text) & "-cut"
                Else
                    SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtSourcePath.Text) & "-uncut"
                End If
                DisplaySizes(CurrentFile)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            DisplaySizes()
        End If
    End Sub

    'Destination-file dialog: Write filestring to textbox. Don't create yet
    Private Sub btnDestinationDialog_Click(sender As Object, e As EventArgs) Handles btnDestinationDialog.Click
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            txtDestinationPath.Text = SaveFileDialog1.FileName
        End If
    End Sub

    'DisplaySizes(): Display sizes of XCIFile or clear display
    Private Sub DisplaySizes(Optional File As XCIFile = Nothing)
        If File Is Nothing Then
            txtCartSize.Text = ""
            txtDataSize.Text = ""
            txtXCIsize.Text = ""
            chkSplit.Enabled = False
        Else
            txtCartSize.Text = Format(File.CartSize / 1048576, "n") & " MB"
            txtDataSize.Text = Format(File.DataSize / 1048576, "n") & " MB"
            txtXCIsize.Text = Format(File.RealFileSize / 1048576, "n") & " MB"
            If File.ChunkCount > 1 AndAlso rdbCut.Checked Then chkSplit.Enabled = True Else chkSplit.Enabled = False
        End If
    End Sub

    'btnCut_Click(): Check wether current size and target size differ / start backgroundworker & progressbar
    Private Sub btnCut_Click(sender As Object, e As EventArgs) Handles btnCut.Click
        If IsNothing(CurrentFile) Then
            MessageBox.Show("Please specify an Inputfile!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else
            If rdbCut.Checked Then
                If txtXCIsize.Text = txtDataSize.Text Then
                    If chkSplit.Checked Then
                        If MessageBox.Show("The XCI-file size already corresponds" & vbLf &
                                           "to the actual data size." & vbCrLf & vbCrLf & "Do you just want to split?",
                                           "No need to Cut!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.No Then Exit Sub
                    Else
                        MessageBox.Show("The XCI-file size already corresponds" & vbLf & "to the actual data size.", "No need to Cut!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                End If
            Else
                If txtXCIsize.Text = txtCartSize.Text Then
                    MessageBox.Show("The XCI-file size already corresponds" & vbLf & "to the full cartridge size.", "No need to Uncut!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If

            If txtSourcePath.Text <> txtDestinationPath.Text Then
                Try
                    If Directory.Exists(Path.GetDirectoryName(txtDestinationPath.Text)) Then
                        CurrentFile.OutPath = txtDestinationPath.Text
                    End If
                Catch ex As Exception
                    MessageBox.Show("Please enter a valid path, or use the ""Destination"" Button to choose.", "Destination Error!")
                    Exit Sub
                End Try
                progressTimer.Start()
                BackgroundWorker.RunWorkerAsync()
            Else
                MessageBox.Show("Make sure to use two different files" & vbLf & "for source and destination.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            ToggleControls(False)
        End If
    End Sub

    Friend Sub ToggleControls(Trigger As Boolean)
        rdbCut.Enabled = Trigger
        rdbUncut.Enabled = Trigger
        btnCut.Enabled = Trigger
        chkSplit.Enabled = Trigger
        btnSourceDialog.Enabled = Trigger
        btnDestinationDialog.Enabled = Trigger
        txtSourcePath.Enabled = Trigger
        txtDestinationPath.Enabled = Trigger
        If Trigger Then
            If rdbCut.Checked Then
                btnCut.Text = "Cut!"
            Else
                btnCut.Text = "Uncut!"
            End If
        Else
            btnCut.Text = "Please wait ..."
        End If

    End Sub

    Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker.DoWork
        CurrentFile.OpenReaders()
        BackgroundWorker.WorkerSupportsCancellation = True

        'Cut / Split
        If rdbCut.Checked = True Then
            'jump to DataSíze
            CurrentFile.InPos = CurrentFile.DataSize
            'check sectors after DataSize
            BackgroundWorker.ReportProgress(1)
            For i As UInt64 = CurrentFile.DataSize To CurrentFile.RealFileSize - 1
                If BackgroundWorker.CancellationPending = False Then
                    If CurrentFile.br.ReadByte <> 255 Then
                        MessageBox.Show("Found used space after gamedata! Aborting!", "Warning!")
                        Exit Sub
                    End If
                Else
                    e.Cancel = True
                    Exit Sub
                End If
            Next
            CurrentFile.InPos = 0
            Dim NextOffset, LastOffset As UInt64
            For n As Byte = 0 To CurrentFile.ChunkCount - 1
                LastOffset = NextOffset + 1
                'Set Offset/filename/status whether we want splitting or not
                If chkSplit.Checked Then
                    If CurrentFile.DataSize > NextOffset + CurrentFile.ChunkSize Then
                        NextOffset = (n + 1) * CurrentFile.ChunkSize
                    Else
                        NextOffset = CurrentFile.DataSize
                    End If
                    BackgroundWorker.ReportProgress(n + 11)
                    'set filename for next chunk
                    If n > 0 Then CurrentFile.OutPath = CurrentFile.OutPath.TrimEnd((n - 1).ToString) & n.ToString
                Else
                    NextOffset = CurrentFile.DataSize
                    BackgroundWorker.ReportProgress(2)
                End If
                For i As UInt64 = LastOffset To NextOffset
                    If BackgroundWorker.CancellationPending = False Then
                        CurrentFile.bw.Write(CurrentFile.br.ReadByte)
                    Else
                        e.Cancel = True
                        Exit Sub
                    End If
                Next
                If CurrentFile.InPos = CurrentFile.DataSize Then Exit For
            Next
        End If

        'Uncut / Join
        If rdbUncut.Checked = True Then
            BackgroundWorker.ReportProgress(2)
            If CurrentFile.InPath.EndsWith(".xci", StringComparison.CurrentCultureIgnoreCase) Then
                For i As UInt64 = 1 To CurrentFile.DataSize
                    CurrentFile.bw.Write(CurrentFile.br.ReadByte)
                Next
            ElseIf CurrentFile.InPath.EndsWith(".xc0", StringComparison.CurrentCultureIgnoreCase) Then
                Dim NextOffset As UInt64 = CurrentFile.ChunkSize
                Dim LastOffset As UInt64 = 0
                For n As Byte = 1 To CurrentFile.ChunkCount
                    For i As UInt64 = LastOffset + 1 To NextOffset
                        If BackgroundWorker.CancellationPending = False Then
                            CurrentFile.bw.Write(CurrentFile.br.ReadByte)
                        Else
                            e.Cancel = True
                            Exit Sub
                        End If
                    Next
                    LastOffset = NextOffset
                    If CurrentFile.DataSize > NextOffset + CurrentFile.ChunkSize Then
                        NextOffset = (n + 1) * CurrentFile.ChunkSize
                    Else
                        NextOffset = CurrentFile.DataSize
                    End If
                    If n < CurrentFile.ChunkCount Then CurrentFile.InPath = CurrentFile.InPath.TrimEnd((n - 1).ToString) & n.ToString
                Next
            Else
                'this message shouldn't be triggered, since we checked input files before
                MessageBox.Show("Please specify a .xci or .xc0 file as source", "Error!")
                Exit Sub
            End If
            BackgroundWorker.ReportProgress(3)
            For i As UInt64 = CurrentFile.DataSize + 1 To CurrentFile.CartSize
                If BackgroundWorker.CancellationPending = False Then
                    CurrentFile.bw.Write(CByte(255))
                Else
                    e.Cancel = True
                    Exit Sub
                End If
            Next
        End If
    End Sub

    'Update progressbar using a seperate timer
    Private Sub progressTimer_Tick(sender As Object, e As EventArgs) Handles progressTimer.Tick
        If CurrentFile.InPos <= CurrentFile.DataSize Then
            If rdbCut.Checked = True Then
                ProgressBar.Value = CInt(100 / CurrentFile.DataSize * CurrentFile.InPos)
            Else
                ProgressBar.Value = CInt(100 / CurrentFile.CartSize * CurrentFile.OutPos)
            End If
        End If
    End Sub

    'Change status text
    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                lblStatus.Text = "Checking space after gamedata ..."
            Case 2
                lblStatus.Text = "Copying gamedata to new file ..."
            Case 3
                lblStatus.Text = "Appending unused sectors after gamedata ..."
            Case > 10
                lblStatus.Text = "Copying gamedata to new files ..." & " Part " & e.ProgressPercentage - 10 & " of " & CurrentFile.ChunkCount
        End Select
    End Sub

    'File creation complete show message and reset form
    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted
        If e.Cancelled = False Then
            ProgressBar.Value = 100
            progressTimer.Stop()
            lblStatus.Text = ""
            MessageBox.Show("Your XCI-file was saved successfully!", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ProgressBar.Value = 0
            CurrentFile.CloseReaders()
            ToggleControls(True)
        End If
    End Sub

    'Exit button
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Close()
    End Sub

    'Set labels and buttons depending checked radiobutton
    Private Sub ModeChanged(sender As Object, e As EventArgs) Handles rdbCut.CheckedChanged, rdbUncut.CheckedChanged
        If rdbUncut.Checked Then
            btnCut.Text = "Uncut!"
            chkSplit.Checked = False
            chkSplit.Enabled = False
            If File.Exists(txtSourcePath.Text) Then SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtSourcePath.Text) & "-uncut"
        Else
            btnCut.Text = "Cut!"
            If Not IsNothing(CurrentFile) AndAlso CurrentFile.ChunkCount > 1 Then chkSplit.Enabled = True
            If File.Exists(txtSourcePath.Text) Then SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(txtSourcePath.Text) & "-cut"
        End If
    End Sub

    'FormClosing: Warn if operations in progress / delete incomplete files
    Private Sub frmXCIcutter_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If lblStatus.Text <> "" Then
            If MessageBox.Show("Conversion in progress!" & vbCrLf & "Really close?", "Warning!",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button2) = DialogResult.No Then
                e.Cancel = True
            Else
                BackgroundWorker.CancelAsync()
                CurrentFile.CloseReaders()
                'remove incomplete file
                If File.Exists(txtDestinationPath.Text) Then File.Delete(txtDestinationPath.Text)
            End If
        End If
    End Sub

    Private Sub chkSplit_CheckedChanged(sender As Object, e As EventArgs) Handles chkSplit.CheckedChanged
        If chkSplit.Checked = True Then
            SaveFileDialog1.Filter = "Split-XCI-File|*.xc0|all Files|*.*"
            If txtDestinationPath.Text.EndsWith("xci") Then
                txtDestinationPath.Text = txtDestinationPath.Text.TrimEnd("i") & "0"
            End If
        Else
            SaveFileDialog1.Filter = "XCI-File|*.xci|all Files|*.*"
            If txtDestinationPath.Text.EndsWith("xc0") Then
                txtDestinationPath.Text = txtDestinationPath.Text.TrimEnd("0") & "i"
            End If
        End If
    End Sub
End Class

'Seperate Class for XCIFile Objects, making it more convenient to handle from different forms/procedures
Friend Class XCIFile
    Private InfileStream As FileStream
    Private OutfileStream As FileStream
    Private pDataSize, pCartSize, pRealfileSize As UInt64
    Private pInPath, pOutPath As String
    Friend ReadOnly ChunkSize As UInt32 = 4 * 1024 ^ 3 - 1
    Private pChunkCount As Byte
    Friend br As BinaryReader
    Friend bw As BinaryWriter

    Public Property OutPath As String
        Get
            Return pOutPath
        End Get
        Set(value As String)
            pOutPath = value
            OutfileStream = New FileStream(pOutPath, FileMode.Create, FileAccess.Write, FileShare.Write)
            bw = New BinaryWriter(OutfileStream)
        End Set
    End Property
    Public Property InPath As String
        Get
            Return pInPath
        End Get
        Set(value As String)
            pInPath = value
            InfileStream = New FileStream(pInPath, FileMode.Open, FileAccess.Read, FileShare.Read)
            br = New BinaryReader(InfileStream)
        End Set
    End Property
    Public Property InPos As UInt64
        Get
            Return InfileStream.Position
        End Get
        Set(value As UInt64)
            InfileStream.Position = value
        End Set
    End Property
    Public Property OutPos As UInt64
        Get
            Return OutfileStream.Position
        End Get
        Set(value As UInt64)
            OutfileStream.Position = value
        End Set
    End Property
    Public ReadOnly Property DataSize As UInt64
        Get
            Return pDataSize
        End Get
    End Property
    Public ReadOnly Property CartSize As UInt64
        Get
            Return pCartSize * 1048576 'Return CartSize in Bytes
        End Get
    End Property
    Public ReadOnly Property RealFileSize As UInt64
        Get
            Return pRealfileSize
        End Get
    End Property
    Public ReadOnly Property ChunkCount As Byte
        Get
            Return pChunkCount
        End Get
    End Property

    Friend Sub OpenReaders()
        'True if reader wasn't initiated at all, or if it was closed
        If IsNothing(br) OrElse IsNothing(br.BaseStream) Then
            InfileStream = New FileStream(pInPath, FileMode.Open, FileAccess.Read, FileShare.Read)
            br = New BinaryReader(InfileStream)
        End If
        'only open writer if a destination-file was set
        If pOutPath IsNot Nothing Then
            If IsNothing(bw) OrElse IsNothing(bw.BaseStream) Then
                OutfileStream = New FileStream(pOutPath, FileMode.Create, FileAccess.Write, FileShare.Write)
                bw = New BinaryWriter(OutfileStream)
            End If
        End If
    End Sub

    Friend Sub CloseReaders()
        If Not IsNothing(br) AndAlso Not IsNothing(br.BaseStream) Then br.Close()
        If Not IsNothing(bw) AndAlso Not IsNothing(bw.BaseStream) Then bw.Close()
    End Sub

    Sub New(InPath As String, Optional OutPath As String = Nothing)
        pInPath = InPath
        pOutPath = OutPath
        ReadSizes()
    End Sub

    Private Sub ReadSizes()
        OpenReaders()
        InfileStream.Position = 269
        Select Case br.ReadByte
            Case 248
                pCartSize = 1904
            Case 240
                pCartSize = 3808
            Case 224
                pCartSize = 7616
            Case 225
                pCartSize = 15232
            Case 226
                pCartSize = 30464
            Case Else
                MessageBox.Show("The source file doesn't look like an XCI file", "Can't determine cartridge size!")
                Exit Sub
        End Select
        InfileStream.Position = 280
        pDataSize = 512 + (BitConverter.ToUInt32(br.ReadBytes(4), 0) * 512)
        pChunkCount = pDataSize \ ChunkSize + 1
        pRealfileSize = InfileStream.Length
        If InPath.EndsWith(".xc0") Then
            For n As Byte = 1 To ChunkCount - 1
                Try
                    pRealfileSize = pRealfileSize + FileLen(InPath.TrimEnd(0.ToString) & n.ToString)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    MessageBox.Show("Make sure that all parts of the dump are accessible.", "Dump incomplete!")
                End Try
            Next
        Else
        End If
        If pRealfileSize < 32 * 1024 Then
            MessageBox.Show("The source file doesn't look like an XCI file", "File to small!")
            Exit Sub
        End If
        CloseReaders()
    End Sub
End Class