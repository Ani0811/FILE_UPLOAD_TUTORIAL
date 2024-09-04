<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuPage.aspx.cs" Inherits="FileUploadWeb.MenuPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style3 {
            width: 123px;
        }
        .Homebtn {
            width: 150px;
            background-color: #0094ff;
            padding: 5px;
            font-size:15px;
            border:thick;
        }
        .Filebtn{
            width: 150px;
            background-color: #0094ff;
            padding: 5px;
            font-size:15px;
            border:thick;
        }
        .Viewbtn{
            width: 150px;
            background-color: #0094ff;
            padding: 5px;
            font-size:15px;
            border:thick;
        }
        .auto-style6 {
            width: 112px;
        }
        .MenuBackground {
            background-color:#00ff90;
            text-align: center;
            border: solid 1px #808080;
        }
        .MenuLevel {
            background-color: #00ff90;
            text-align:left;
            border: solid 1px #808080;
        }
        .MenuHover {
            text-align: center;
            color:black;
        }
        .textarea {
	        border: 1px solid;
	        padding: 10px;
	        position: relative;
	        text-align: justify;
	        width: 80%;
	        color: gray;
        }
        .auto-style10 {
            width: 90px;
        }
        .DivTbl2_S {
            height: 52px;
            width: 50px;
            display:none;
        }
        .DivTbl3_S {
            height: 54px;
            width: 101px;
            display:none;
        }
        .DivTbl4_S {
            height: 30px;
            width: 268px;
            display:none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function Mouse_Over(sMenu)
        {
            var oMenu = document.getElementById(sMenu);
            if (oMenu.style.display == 'block') { oMenu.style.display = 'none'; }
            else { oMenu.style.display = 'block'; }
            oMenu = null;
        }
    </script>
</head>
<body>
    <form id="MenuPage" runat="server">
        <table id="TMenu">
            <tr>
                <td>
                    <div class="auto-style13">
                        <table id="Table1" align="left" style="padding-block:auto">
                            <tr>
                                <td>
                                    <span class="Homebtn" onmouseover="Mouse_Over('DivTbl2')" onmouseout="Mouse_Over('DivTbl2')">Home</span>
                                </td>
                                <td class="auto-style15">
                                    <span class="Filebtn" onmouseover="Mouse_Over('DivTbl3')" onmouseout="Mouse_Over('DivTbl3')">Manage Files</span>
                                </td>
                                <td>
                                    <span class="Viewbtn" onmouseover="Mouse_Over('DivTbl4')" onmouseout="Mouse_Over('DivTbl4')">View Data</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style14">
                                    <div id="DivTbl2" name="DivTbl2" class="DivTbl2_S">
                                        <table id="Table2" align="left">
                                            <tr>
                                                <td class="auto-style6">
                                                    <a href="#">About</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style3">
                                                    <a href="#">Help</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <div id="DivTbl3" class="DivTbl3_S">
                                        <table id="Table3" align="left">
                                            <tr>
                                                <td class="auto-style3">
                                                    <a href="#">Upload File</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style10">
                                                    <a href="#">Retrieve File</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <div id="DivTbl4" class="DivTbl4_S">
                                        <table id="Table4" align="left">
                                            <tr>
                                                <td class="auto-style6">
                                                    <a href="#">Load Data</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <a href="#">Grid View</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <!--SERVER CONTROL-->
        <table id="TMenuServer">
            <tr>
                <td>
                    <div>
                        <table id="T1" align="left" style="padding-block:auto">
                            <tr>
                                <td>
                                    <asp:Menu ID="Menu" runat="server" Orientation="Horizontal">
                                        <LevelMenuItemStyles>
                                            <asp:MenuItemStyle CssClass="MenuBackground" />
                                            <asp:MenuItemStyle CssClass="MenuLevel" />
                                        </LevelMenuItemStyles>
                                        <DynamicHoverStyle CssClass="MenuHover"/>
                                        <Items>
                                            <asp:MenuItem Text="Home">
                                                <asp:MenuItem Text="About"></asp:MenuItem>
                                                <asp:MenuItem Text="Help">
                                                    <asp:MenuItem Text="Update">
                                                    </asp:MenuItem>
                                                </asp:MenuItem>
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="Manage Files">
                                                <asp:MenuItem NavigateUrl="~/FileUpload.aspx" Text="Upload File"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/FileRetrieve.aspx" Text="Retrieve File"></asp:MenuItem>
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="View Data">
                                                <asp:MenuItem Text="Load Data"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/GridViewData.aspx" Text="Grid View"></asp:MenuItem>
                                            </asp:MenuItem>
                                        </Items>
                                    </asp:Menu>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <div id="MenuControl" runat="server">
        </div>
        <div id="TreeViewControl" runat="server"></div>
    </form>
</body>
</html>
