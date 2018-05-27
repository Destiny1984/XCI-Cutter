Imports System.ComponentModel
Imports System.IO

Public Class frmBatch
    Friend CurrentIndex As Integer

    'opens frmXCIcutter.OpenFileDialog1 but with multiselect enabled
    Private Sub btnAddFiles_Click(sender As Object, e As EventArgs) Handles btnAddFiles.Click
        frmXCIcutter.OpenFileDialog1.Multiselect = True
        If frmXCIcutter.OpenFileDialog1.ShowDialog = DialogResult.OK Then
            AddToList(frmXCIcutter.OpenFileDialog1.FileNames())
        End If
        frmXCIcutter.OpenFileDialog1.Multiselect = False
    End Sub

    'Adds files to list if not yet listed
    Private Sub AddToList(FileBuffer() As String)
        For i As Integer = 0 To FileBuffer.GetUpperBound(0)
            For n As Integer = 0 To lstFilelist.Items.Count - 1
                If lstFilelist.Items.Item(n) = FileBuffer(i) Then
                    MessageBox.Show("Already listed:" & vbCrLf & FileBuffer(i), "Skipping file!")
                    FileBuffer(i) = ""
                End If
            Next
            If FileBuffer(i) <> "" Then lstFilelist.Items.Add(FileBuffer(i))
        Next
    End Sub

    'detect DEL-Key to remove items
    Private Sub lstFilelist_KeyDown(sender As Object, e As KeyEventArgs) Handles lstFilelist.KeyDown
        If e.KeyCode = Keys.Delete Then
            For i As Integer = lstFilelist.SelectedIndices.Count - 1 To 0 Step -1
                lstFilelist.Items.RemoveAt(lstFilelist.SelectedIndices(i))
            Next
        End If
    End Sub

    'Colorize listitems based on their status
    Private Sub DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstFilelist.DrawItem
        If lstFilelist.Items.Count > 0 Then
            e.DrawBackground()
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds)
            Else
                If lstFilelist.Items(e.Index).ToString().StartsWith("OK!") Then
                    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds)
                ElseIf lstFilelist.Items(e.Index).ToString().StartsWith("SKIP!") Then
                    e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds)
                ElseIf lstFilelist.Items(e.Index).ToString().StartsWith("ERR") Then
                    e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
                ElseIf lstFilelist.Items(e.Index).ToString().StartsWith("ACTIVE") Then
                    e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
                Else
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds)
                End If
            End If
            e.Graphics.DrawString(lstFilelist.Items(e.Index).ToString(), e.Font, Brushes.Black, New System.Drawing.PointF(e.Bounds.X, e.Bounds.Y + (e.Bounds.Height / 2 - e.Font.Height / 2)))
            e.DrawFocusRectangle()
        End If
    End Sub

    'prepare input and ouput / several conditions to skip files / call backgroundworker
    Private Sub StartConversion()
        'reset progressbar
        prgCurrent.Value = 0
        'show number of file
        lblTotal.Text = "File " & CurrentIndex + 1 & " of " & lstFilelist.Items.Count
        'check if a previous backgroundworker is still running and let it finish
        If frmXCIcutter.BackgroundWorker.IsBusy Then Application.DoEvents()
        'Skip files that have been processed before
        If lstFilelist.Items(CurrentIndex).ToString.StartsWith("ERR".ToCharArray) OrElse
            lstFilelist.Items(CurrentIndex).ToString.StartsWith("SKIP!".ToCharArray) OrElse
            lstFilelist.Items(CurrentIndex).ToString.StartsWith("OK!".ToCharArray) Then
            StartNext(False)
            Exit Sub
        End If

        frmXCIcutter.CurrentFile = New XCIFile(lstFilelist.Items(CurrentIndex))

        If rdbCut.Checked Then
            ' Skip files when file was already trimmed and ...
            ' 1. splitting wasn't checked at all
            ' 2. splitting was checked but input is already a xc0 file
            ' 3. splitting was checked but output fits in one part
            If frmXCIcutter.CurrentFile.RealFileSize <= frmXCIcutter.CurrentFile.DataSize Then
                If Not chkSplit.Checked _
                    OrElse (chkSplit.Checked AndAlso frmXCIcutter.CurrentFile.ChunkCount > 1 AndAlso lstFilelist.Items(CurrentIndex).ToString.EndsWith("0")) _
                    OrElse (chkSplit.Checked AndAlso frmXCIcutter.CurrentFile.ChunkCount = 1) Then
                    lstFilelist.Items(CurrentIndex) = "SKIP!" & vbTab & lstFilelist.Items(CurrentIndex)
                    lstFilelist.Refresh()
                    StartNext(False)
                    Exit Sub
                End If
            End If

            'set output: set file extension to xc0 if splitting was checked and we'll need more than one part
            If frmXCIcutter.CurrentFile.ChunkCount > 1 AndAlso chkSplit.Checked Then
                frmXCIcutter.CurrentFile.OutPath = lstFilelist.Items(CurrentIndex).ToString.Substring(0, lstFilelist.Items(CurrentIndex).ToString.Length - 4) & "-cut.xc0"
            Else
                frmXCIcutter.CurrentFile.OutPath = lstFilelist.Items(CurrentIndex).ToString.Substring(0, lstFilelist.Items(CurrentIndex).ToString.Length - 4) & "-cut.xci"
            End If
        Else
            'Skip if file is already of full size
            If frmXCIcutter.CurrentFile.RealFileSize >= frmXCIcutter.CurrentFile.CartSize Then
                lstFilelist.Items(CurrentIndex) = "SKIP!" & vbTab & lstFilelist.Items(CurrentIndex)
                StartNext(False)
                Exit Sub
            End If
            frmXCIcutter.CurrentFile.OutPath = lstFilelist.Items(CurrentIndex).ToString.Substring(0, lstFilelist.Items(CurrentIndex).ToString.Length - 4) & "-uncut.xci"
        End If
        'mark current file and start
        lstFilelist.Items(CurrentIndex) = "ACTIVE" & vbTab & lstFilelist.Items(CurrentIndex)
        frmXCIcutter.progressTimer.Start()
        frmXCIcutter.BackgroundWorker.RunWorkerAsync()
    End Sub

    'cleanup last file / set index to next file / show message if finished
    Friend Sub StartNext(PreviousOK As Boolean)
        prgTotal.Value = 100 / lstFilelist.Items.Count * (CurrentIndex + 1)
        If PreviousOK Then
            'Mark last item as OK!
            lstFilelist.Items(CurrentIndex) = "OK!" & lstFilelist.Items(CurrentIndex).ToString.TrimStart("ACTIVE".ToCharArray)
            'Delete source file(s)
            If chkDelete.Checked Then
                If frmXCIcutter.CurrentFile.InPath.EndsWith(".xc0") Then
                    For i As Byte = 1 To frmXCIcutter.CurrentFile.ChunkCount - 1
                        File.Delete(frmXCIcutter.CurrentFile.InPath.TrimEnd((i - 1).ToString) & i)
                    Next
                End If
                File.Delete(frmXCIcutter.CurrentFile.InPath)
            End If
        End If
        'if there are more entries on the list: start next
        If CurrentIndex < lstFilelist.Items.Count - 1 Then
            CurrentIndex = CurrentIndex + 1
            StartConversion()
        Else
            'finished last entry: show message and cleanup
            MessageBox.Show("Processed all files.", "Finished!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ToggleControls(True)
            lblCurrent.Text = ""
            prgCurrent.Value = 0
            prgTotal.Value = 0
            CurrentIndex = 0
        End If
    End Sub

    'Start- and Cancel-button
    Private Sub btnStartBatch_click() Handles btnStartBatch.Click
        'Start conversion
        If lstFilelist.Items.Count > 0 Then
            If btnStartBatch.Text = "Start" Then
                ToggleControls(False)
                StartConversion()
            Else
                'Cancel and cleanup
                If MessageBox.Show("Really Cancel?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                    frmXCIcutter.BackgroundWorker.CancelAsync()
                    ToggleControls(True)
                    lblCurrent.Text = ""
                    prgCurrent.Value = 0
                    prgTotal.Value = 0
                    lstFilelist.Items(CurrentIndex) = frmXCIcutter.CurrentFile.InPath
                    CurrentIndex = 0
                End If
            End If
        Else
            MessageBox.Show("Please add at least one file!", "Nothing to do!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    'Toggle controls
    Friend Sub ToggleControls(Trigger As Boolean)
        lstFilelist.Enabled = Trigger
        btnAddFiles.Enabled = Trigger
        rdbCut.Enabled = Trigger
        rdbUncut.Enabled = Trigger
        chkSplit.Enabled = Trigger
        chkDelete.Enabled = Trigger
        lblTotal.Text = ""
        If Trigger Then
            btnStartBatch.Text = "Start"
        Else
            btnStartBatch.Text = "Cancel"
        End If
    End Sub

    'actually the conversion is handled by frmXCIcutter
    'we need to copy the selected parameters from batchform to frmXCIcutter
    Private Sub rdbUncut_CheckedChanged(sender As Object, e As EventArgs) Handles rdbUncut.CheckedChanged, rdbCut.CheckedChanged
        frmXCIcutter.rdbCut.Checked = rdbCut.Checked
        frmXCIcutter.rdbUncut.Checked = rdbUncut.Checked
    End Sub
    Private Sub chkSplit_CheckedChanged(sender As Object, e As EventArgs) Handles chkSplit.CheckedChanged
        frmXCIcutter.chkSplit.Checked = chkSplit.Checked
    End Sub

    'hide frmXCIcutter while BatchForm is open
    Private Sub frmBatch_Load(sender As Object, e As EventArgs) Handles Me.Load
        chkSplit.Checked = frmXCIcutter.chkSplit.Checked
        frmXCIcutter.Hide()
    End Sub

    'Closing: Ask if conversion in progress
    Private Sub frmBatch_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If btnStartBatch.Text = "Cancel" Then
            If MessageBox.Show("Conversion in progress!" & vbCrLf & "Really Close?", "Warning!",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Exclamation,
                               MessageBoxDefaultButton.Button2) = DialogResult.No Then
                e.Cancel = True
            Else
                frmXCIcutter.BackgroundWorker.CancelAsync()
            End If
        End If
    End Sub

    'show frmXCIcutter
    Private Sub frmBatch_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        frmXCIcutter.Show()
    End Sub
End Class