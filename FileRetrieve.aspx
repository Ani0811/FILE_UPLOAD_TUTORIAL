<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileRetrieve.aspx.cs" Inherits="FileUploadWeb.FileRetrieve" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type =" text/css">
        .imageDiv
        {
          border:double;
          width:750px;
          max-height: 233px;
          object-fit:contain;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ImgDV">
            <asp:Panel ID="Img_Panel" runat="server" HorizontalAlign="Center" class="imageDiv">
            </asp:Panel>
        </div>
    </form>
</body>
</html>
