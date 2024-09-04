<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="FileUploadWeb.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 66px;
            width: 246px;
        }
        .auto-style6 {
            width: 241px;
        }
        .auto-style8 {
            background-color: forestgreen;
            border-color: forestgreen;
            border-style: solid;
        }
        .auto-style9 {
            height: 56px;
            width: 327px;
        }
        .imageDiv
        {
          max-width:250px;
          max-height:300px;
          object-fit: contain;
        }
        </style>
    <script language="javascript" src="~/Scripts/MyJavaScript.js"></script>	
</head>
<body>
    <form id="FileUpload" name="FileUpload" runat="server">
        <table id="T1" align="left" border="1">
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="false" Accept="image/jpg,image/jpeg,text/plain,application/pdf,image/gif,image/jpeg,audio/mpeg,video/mp4" />
                </td>
                <td>
                    <asp:Button ID="btn_Upload" runat="server" OnClick="btn_Upload_Click" Text="Upload File" />
                </td>
            </tr>
        </table>
        <table id="T2" align="left" border="1">
            <tr>
                <td class="auto-style9">
                    <table id="T3" align="left" border="1">
                        <thead id="Header">
                        <tr>
                            <th>
                                <asp:GridView ID="GD_View" runat="server" Width="317px" GridLines="Both" PageSize="6" AllowPaging="True" OnPageIndexChanging="GD_View_PageIndexChanging" ShowFooter="True" HorizontalAlign="Center" OnRowCommand="GD_View_RowCommand" >
                                </asp:GridView>
                            </th>
                        </tr>
                        </thead>
                    </table>
                </td>
            </tr>
        </table>
        <table id="T4" border="1">
            <tr>
                <td class="auto-style6">
                    <div class="imageDiv">
                        <asp:Panel ID="Img_Panel" runat="server" HorizontalAlign="Center" class="imageDiv">
                        </asp:Panel>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
