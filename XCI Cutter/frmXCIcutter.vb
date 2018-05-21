Imports System.ComponentModel
Imports System.IO

Public Class frmXCIcutter
    Dim infs As FileStream
    Dim outfs As FileStream
    Dim DataSize As UInt64
    Dim CartSize As UInt64
    Dim br As BinaryReader
    Dim bw As BinaryWriter

    'Source-file dialog: Create filestream + binaryreader / Call ReadSizes()
    Private Sub btnSourceDialog_Click(sender As Object, e As EventArgs) Handles btnSourceDialog.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            txtSourcePath.Text = OpenFileDialog1.FileName
            If rdbCut.Checked = True Then
                SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName) & "-cut"
            Else
                SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName) & "-uncut"
            End If
            infs = New FileStream(txtSourcePath.Text, FileMode.Open, FileAccess.Read, FileShare.Read)
            br = New BinaryReader(infs)
            ReadSizes()
        End If
    End Sub
    'Source-file textbox: Check for valid path / Create filestream + binaryreader / Call ReadSizes()
    Private Sub txtSourcePath_Leave(sender As Object, e As EventArgs) Handles txtSourcePath.Leave
        If File.Exists(txtSourcePath.Text) Then
            infs = New FileStream(txtSourcePath.Text, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
            br = New BinaryReader(infs)
            ReadSizes()
        Else
            MessageBox.Show("Please enter the path to an existing XCI-file")
            txtSourceSize.Text = ""
            txtDestSize.Text = ""
        End If
    End Sub

    'Destinaton-file dialog
    Private Sub btnDestinationDialog_Click(sender As Object, e As EventArgs) Handles btnDestinationDialog.Click
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            txtDestinationPath.Text = SaveFileDialog1.FileName
        End If
    End Sub

    'ReadSizes(): Read cartridge-Size from offset 269 / Read data-size from offset 280
    Private Sub ReadSizes()
        infs.Position = 269
        Select Case br.ReadByte
            Case 248
                CartSize = 1904
            Case 240
                CartSize = 3808
            Case 224
                CartSize = 7616
            Case 225
                CartSize = 15232
            Case 226
                CartSize = 30464
            Case Else
                MessageBox.Show("The source file doesn't look like an XCI file", "Can't determine cartridge size!")
                txtSourcePath.Text = ""
                Exit Sub
        End Select
        infs.Position = 280
        DataSize = 512 + (BitConverter.ToUInt32(br.ReadBytes(4), 0) * 512)
        If rdbCut.Checked = True Then
            txtSourceSize.Text = Format(CartSize, "n") & " MB"
            txtDestSize.Text = Format(DataSize / 1048576, "n") & " MB"
        Else
            txtSourceSize.Text = Format(infs.Length / 1048576, "n") & " MB"
            txtDestSize.Text = Format(CartSize, "n") & " MB"
        End If
    End Sub

    'btnCut_Click(): Check wether source and destination are of different size / start backgroundworker & progressbar
    Private Sub btnCut_Click(sender As Object, e As EventArgs) Handles btnCut.Click
        If txtSourceSize.Text = txtDestSize.Text Then
            If rdbCut.Checked = True Then
                MessageBox.Show("The XCI-file size already corresponds" & vbLf & "to the actual data size.", "No need to Cut!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                MessageBox.Show("The XCI-file size already corresponds" & vbLf & "to the full cartridge size.", "No need to Uncut!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            Exit Sub
        End If

        Try
            outfs = New FileStream(txtDestinationPath.Text, FileMode.Create, FileAccess.Write, FileShare.Write)
            If File.Exists(txtSourcePath.Text) And txtSourcePath.Text <> txtDestinationPath.Text Then
                bw = New BinaryWriter(outfs)
                progressTimer.Start()
                BackgroundWorker.RunWorkerAsync(DataSize)
            Else
                MessageBox.Show("Make sure to use two different files" & vbLf & "for source and destination.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        rdbCut.Enabled = False
        rdbUncut.Enabled = False
        btnCut.Enabled = False
        btnCut.Text = "Please wait ..."
        btnSourceDialog.Enabled = False
        btnDestinationDialog.Enabled = False
        txtSourcePath.Enabled = False
        txtDestinationPath.Enabled = False
    End Sub

    Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker.DoWork
        If rdbCut.Checked = True Then
            'jump to DataSíze
            infs.Position = e.Argument
            'check sectors after DataSize
            BackgroundWorker.ReportProgress(1)
            For i As UInt64 = e.Argument To infs.Length - 1
                If br.ReadByte <> 255 Then
                    MessageBox.Show("Found used space after gamedata! Aborting!", "Warning!")
                    Exit Sub
                End If
            Next
        End If

        'write 'till DataSize
        BackgroundWorker.ReportProgress(2)
        infs.Position = 0
        outfs.Position = 0
        For i As UInt64 = 1 To e.Argument
            bw.Write(br.ReadByte)
        Next

        'write from DataSize 'till CartSize
        If rdbUncut.Checked = True Then
            BackgroundWorker.ReportProgress(3)
            For i As UInt64 = e.Argument To CartSize * 1048576 - 1
                bw.Write(CByte(255))
            Next
        End If
    End Sub

    'Update progressbar using a seperate timer
    Private Sub progressTimer_Tick(sender As Object, e As EventArgs) Handles progressTimer.Tick
        If rdbCut.Checked = True Then
            ProgressBar.Value = CInt(100 * outfs.Position / DataSize)
        Else
            ProgressBar.Value = CInt(100 * outfs.Position / (CartSize * 1048576))
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
        End Select
    End Sub

    'File creation complete show message and reset form
    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted
        ProgressBar.Value = 100
        progressTimer.Stop()
        bw.Close()
        br.Close()
        lblStatus.Text = ""
        MessageBox.Show("Your XCI-file was saved successfully!", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ProgressBar.Value = 0
        rdbCut.Enabled = True
        rdbUncut.Enabled = True
        btnCut.Enabled = True
        If rdbCut.Checked = True Then btnCut.Text = "Cut!" Else btnCut.Text = "Uncut!"
        btnSourceDialog.Enabled = True
        btnDestinationDialog.Enabled = True
        txtSourcePath.Enabled = True
        txtDestinationPath.Enabled = True
    End Sub

    'Exit button
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Close()
    End Sub

    'Reset form depending checked radiobutton
    Private Sub ModeChanged(sender As Object, e As EventArgs) Handles rdbCut.CheckedChanged, rdbUncut.CheckedChanged
        If rdbUncut.Checked Then
            lblCartsize.Text = "XCI-file size"
            lblUsedspace.Text = "Cartridge size"
            btnCut.Text = "Uncut!"
        Else
            lblCartsize.Text = "Cartridge size"
            lblUsedspace.Text = "Used space"
            btnCut.Text = "Cut!"
        End If
        txtSourceSize.Text = ""
        txtDestSize.Text = ""
        txtSourcePath.Text = ""
        txtDestinationPath.Text = ""
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
                'remove incomplete file
                br.Close()
                bw.Close()
                If File.Exists(txtDestinationPath.Text) Then File.Delete(txtDestinationPath.Text)
            End If
        End If
    End Sub
End Class