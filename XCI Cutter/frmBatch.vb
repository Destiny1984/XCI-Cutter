Imports System.ComponentModel
Imports System.IO

Public Class frmBatch
    Friend CurrentIndex As Integer

    'Open frmXCIcutter.OpenFileDialog1 but with multiselect enabled
    Private Sub btnAddFiles_Click(sender As Object, e As EventArgs) Handles btnAddFiles.Click
        frmXCIcutter.OpenFileDialog1.Multiselect = True
        If frmXCIcutter.OpenFileDialog1.ShowDialog = DialogResult.OK Then
            AddToList(frmXCIcutter.OpenFileDialog1.FileNames())
        End If
        frmXCIcutter.OpenFileDialog1.Multiselect = False
    End Sub

    'Add files to list if not yet listed
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

    'Detect DEL-Key to remove items
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
                e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
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

    'Prepare input and ouput / several conditions to skip files / call backgroundworker
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
        'skip if inputfile is no valid xci-file (mark as error 35)
        If frmXCIcutter.CurrentFile.FileOK = False Then
            lstFilelist.Items(CurrentIndex) = "ERR35" & vbTab & lstFilelist.Items(CurrentIndex)
            StartNext(False)
            Exit Sub
        End If

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

    'Cleanup last file / set index to next file / show message if finished
    Friend Sub StartNext(PreviousOK As Boolean)
        prgTotal.Value = 100 / lstFilelist.Items.Count * (CurrentIndex + 1)
        If PreviousOK Then
            'Delete source file(s)
            If chkDelete.Checked Then
                If frmXCIcutter.CurrentFile.InPath.EndsWith(".xci") Then
                    File.Delete(frmXCIcutter.CurrentFile.InPath)
                Else
                    For n As SByte = 0 To frmXCIcutter.CurrentFile.ChunkCount - 1
                        File.Delete(frmXCIcutter.CurrentFile.InPath.Substring(0, frmXCIcutter.CurrentFile.InPath.Length - 1) & n.ToString)
                    Next
                End If
            End If
            'Mark last item as OK!
            lstFilelist.Items(CurrentIndex) = "OK!" & lstFilelist.Items(CurrentIndex).ToString.TrimStart("ACTIVE".ToCharArray)
        End If
        'if there are more entries on the list: start next
        If CurrentIndex < lstFilelist.Items.Count - 1 Then
            CurrentIndex = CurrentIndex + 1
            StartConversion()
        Else
            'finished last entry: show message and cleanup
            ToggleControls(True)
            MessageBox.Show("Processed all files.", "Finished!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            lblTotal.Text = ""
            lblCurrent.Text = ""
            prgTotal.Value = 0
            prgCurrent.Value = 0
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
                    lblTotal.Text = ""
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
        If Trigger Then
            btnStartBatch.Text = "Start"
        Else
            btnStartBatch.Text = "Cancel"
        End If
    End Sub

    'Actually the conversion is handled by frmXCIcutter
    'We need to copy the selected parameters from batchform to frmXCIcutter
    Private Sub rdbUncut_CheckedChanged(sender As Object, e As EventArgs) Handles rdbUncut.CheckedChanged, rdbCut.CheckedChanged
        frmXCIcutter.rdbCut.Checked = rdbCut.Checked
        frmXCIcutter.rdbUncut.Checked = rdbUncut.Checked
        If rdbCut.Checked Then
            chkSplit.Enabled = True
        Else
            'The split-checkbox is always ignored in mode uncut/join
            'but nevertheless disable the control to avoid confusion
            chkSplit.Enabled = False
            chkSplit.Checked = False
        End If
    End Sub
    Private Sub chkSplit_CheckedChanged(sender As Object, e As EventArgs) Handles chkSplit.CheckedChanged
        frmXCIcutter.chkSplit.Checked = chkSplit.Checked
    End Sub

    'Hide frmXCIcutter while BatchForm is open
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

    'Show frmXCIcutter after closing frmBatch
    Private Sub frmBatch_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        frmXCIcutter.Show()
        Me.Dispose()
    End Sub

    '@rapidraid: Added an AddFolder button. Searches for all xci/xc0 files recursively'
    Private Sub btnAddFolder_Click(sender As Object, e As EventArgs) Handles btnAddFolder.Click

        Dim fb = New FolderBrowserDialog()
        fb.ShowDialog()
        If String.IsNullOrEmpty(fb.SelectedPath) = False Then


            Dim f1 As FileInfo() = New DirectoryInfo(fb.SelectedPath).GetFiles("*.xci", SearchOption.AllDirectories)
            Dim f2 As FileInfo() = New DirectoryInfo(fb.SelectedPath).GetFiles("*.xc0", SearchOption.AllDirectories)
            Dim ff As FileInfo() = f1.Union(f2).ToArray()

            Dim p As String()
            ReDim Preserve p(ff.Length)

            For i As Integer = 0 To ff.Length - 1
                p(i) = ff(i).FullName()
            Next

            AddToList(p)
        End If
    End Sub

    'DragEnter & DragDrop procedures to add files by Drag&Drop
    Private Sub lstFilelist_DragEnter(sender As Object, e As DragEventArgs) Handles lstFilelist.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub lstFilelist_DragDrop(sender As Object, e As DragEventArgs) Handles lstFilelist.DragDrop
        Dim DroppedFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim FileBuffer As New List(Of String)

        Try
            For Each Entry As String In DroppedFiles
                'If dropped item is a directory: scan recursive (copied from btnAddFolder_Click / thanks to getraid)
                If Directory.Exists(Entry) Then
                    Dim f1 As FileInfo() = New DirectoryInfo(Entry).GetFiles("*.xci", SearchOption.AllDirectories)
                    Dim f2 As FileInfo() = New DirectoryInfo(Entry).GetFiles("*.xc0", SearchOption.AllDirectories)
                    Dim ff As FileInfo() = f1.Union(f2).ToArray
                    For i As Integer = 0 To ff.Length - 1
                        FileBuffer.Add(ff(i).FullName)
                    Next
                    'if xci or xc0 file: just add
                ElseIf Entry.EndsWith(".xci") OrElse Entry.EndsWith(".xc0") Then
                    FileBuffer.Add(Entry)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        AddToList(FileBuffer.ToArray)
    End Sub

End Class