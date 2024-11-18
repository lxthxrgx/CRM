Imports System.Runtime.InteropServices
Imports Word = Microsoft.Office.Interop.Word
Imports System.IO

Public Class Rent_Dog_Tov
    Private Const MaxRetryAttempts As Integer = 5
    Private Const RetryDelayMilliseconds As Integer = 500

    Public Shared Function AddTextToContentControl(docPath As String,
                                              DogovirRent As String,
                                              DateTime As String,
                                              rnokpp As String,
                                                   address As String,
                                                   Director As String,
                                                   area As String,
                                                   area_text As String,
                                                    address_p As String,
                                                   StrokDii As String,
                                                   suma As String,
                                                    sum_text As String,
                                                   PIB As String,
                                                   BanckAccount As String,
                                                   PIBSDirector As String,
                                                   NaPid As String) As Boolean
        Dim wordApp As Word.Application = Nothing
        Dim doc As Word.Document = Nothing
        Console.WriteLine("Путь к файлу: " & docPath)
        Console.WriteLine("DogovirRent: " & DogovirRent)
        Console.WriteLine("DateTime: " & DateTime)
        Console.WriteLine("rnokpp: " & rnokpp)
        Console.WriteLine("address: " & address)
        Console.WriteLine("Director: " & Director)
        Console.WriteLine("area: " & area)
        Console.WriteLine("area_text: " & area_text)
        Console.WriteLine("address_p: " & address_p)
        Console.WriteLine("StrokDii: " & StrokDii)
        Console.WriteLine("suma: " & suma)
        Console.WriteLine("sum_text: " & sum_text)
        Console.WriteLine("PIB: " & PIB)
        Console.WriteLine("BanckAccount: " & BanckAccount)
        Console.WriteLine("PIBSDirector: " & PIBSDirector)
        Console.WriteLine("NaPid: " & NaPid)

        Try
            If Not File.Exists(docPath) Then
                Console.WriteLine("Ошибка: Файл не существует.")
                Return False
            End If

            wordApp = New Word.Application()
            wordApp.Visible = False

            Try
                doc = OpenDocumentWithRetry(wordApp, docPath)
            Catch ex As Exception
                Console.WriteLine("Ошибка при открытии документа: " & ex.Message)
                Return False
            End Try

            Dim contentData As New Dictionary(Of String, String) From {
                {"DateTime", DateTime},
                {"DogovirRent", DogovirRent},
                {"PIB", PIB},
                {"rnokpp", rnokpp},
                {"area", area},
                {"area_text", area_text},
                {"address_p", address_p},
                {"StrokDii", StrokDii},
                {"address", address},
                {"BanckAccount", BanckAccount},
                {"sum_text", sum_text},
                {"suma", suma},
                {"Director", Director},
                {"PIBSDirector", PIBSDirector},
                 {"NaPid", NaPid}
            }

            Try
                For Each cc As Word.ContentControl In doc.ContentControls
                    If contentData.ContainsKey(cc.Title) Then
                        cc.Range.Text = contentData(cc.Title)
                    End If
                Next
            Catch ex As Exception
                Console.WriteLine("Ошибка при заполнении содержимого: " & ex.Message)
                Return False
            End Try

            Try
                doc.Save()
            Catch ex As Exception
                Console.WriteLine("Ошибка при сохранении документа: " & ex.Message)
                Return False
            End Try

        Catch ex As Exception
            Console.WriteLine("Общая ошибка: " & ex.Message)
        Finally
            If doc IsNot Nothing Then
                Try
                    doc.Close(SaveChanges:=False)
                    Marshal.ReleaseComObject(doc)
                Catch ex As Exception
                    Console.WriteLine("Ошибка при закрытии документа: " & ex.Message)
                End Try
            End If

            If wordApp IsNot Nothing Then
                Try
                    wordApp.Quit()
                    Marshal.ReleaseComObject(wordApp)
                Catch ex As Exception
                    Console.WriteLine("Ошибка при закрытии приложения Word: " & ex.Message)
                End Try
            End If
        End Try
        Return True
    End Function

    Private Shared Function OpenDocumentWithRetry(wordApp As Word.Application, docPath As String) As Word.Document
        Dim attempt As Integer = 0
        Dim doc As Word.Document = Nothing

        While attempt < MaxRetryAttempts
            Try
                doc = wordApp.Documents.Open(docPath)
                Return doc
            Catch ex As COMException When ex.ErrorCode = &H8001010A
                attempt += 1
                If attempt >= MaxRetryAttempts Then
                    Throw
                End If
                Threading.Thread.Sleep(RetryDelayMilliseconds)
            End Try
        End While

        Return doc
    End Function
End Class
