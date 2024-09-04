<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridViewData.aspx.cs" Inherits="FileUploadWeb.GridViewData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .imageDiv1
        {
          width:250px;
          height:300px;
        }
        .ImgFrame {
            /*overflow:auto;*/
            height: 330px;
            object-fit:contain;
            margin-left: auto;
            margin-right: auto;
            display: block;
            border: solid;
        }
        .FileFrame {
            /*overflow:auto;*/
            height:300px;
            width:600px;
            object-fit:cover;
            margin-left: auto;
            margin-right: auto;
            display: block;
            border: solid;
        }
        .auto-style10 {
            align-content: center;
            height:330px;
            object-fit:cover;
        }
        </style>
    <script language="javascript" type="text/javascript">
        function Go_Submit() {
            var oSel = document.getElementById("pgRowCount");
            document.forms[0].action = "GridViewData.aspx?PAGECOUNT=" + oSel.value;
            document.forms[0].method = "post";
            document.forms[0].target = "_self";
            document.forms[0].submit();
        }
        function Link_Click(PgID) {
            document.forms[0].action = "/GridViewData.aspx?PG=" + PgID;
            document.forms[0].method = "post";
            document.forms[0].target = "_self";
            document.forms[0].submit();
        }
        function getChb(oChb)
        {
            alert(oChb.id);
        }
        function getChbAll()
        {
            var oSel = document.getElementById("pgRowCount");
            var iSelectCount = oSel.value;

            for (var i = 1; i <= iSelectCount; i++)
            {
                var ochbx = document.getElementById("chb_" + i);

                if (ochbx.checked == true) { ochbx.checked = false; }
                else { ochbx.checked = true; }   
            }
        }
        function Delete_Click()
        {
            var sCtrl = '';
            var oSel = document.getElementById("pgRowCount");
            var iSelectCount = oSel.value;

            for (var i = 1; i <= iSelectCount; i++)
            {
                var ochbx = document.getElementById("chb_" + i);
                if (ochbx.checked == true)
                {
                    sCtrl += ochbx.title.substr(4, 5) + "|";
                    
                    document.forms[0].action = "GridViewData.aspx?ACTION=DEL&PG=" + sCtrl;
                    document.forms[0].method = "post";
                    document.forms[0].target = "_self";
                    document.forms[0].submit();
                }
            }
        }
        function Display_Image(argImgID, argFileType)
        {
            var sUrl = null;
            var divWorkArea = null;
            var ifrm = null;
            var imgCtrl = null;
            try
            {
                divWorkArea = document.getElementById('DivWorkArea');
                if (divWorkArea != null)
                {
                    divWorkArea.innerHTML = '';
                    switch (argFileType)
                    {
                        case ".JPG":
                            imgCtrl = document.createElement("IMG");
                            imgCtrl.setAttribute('ID', 'IMGID');
                            imgCtrl.setAttribute('NAME', 'IMGNAME')
                            imgCtrl.setAttribute('CLASS', 'ImgFrame');
                            sUrl = 'FileRetrieve.aspx?ID=' + argImgID;
                            imgCtrl.setAttribute('SRC', sUrl);
                            divWorkArea.appendChild(imgCtrl);
                            break;

                        default:
                            ifrm = document.createElement("IFRAME");
                            ifrm.setAttribute('ID', 'IMGID');
                            ifrm.setAttribute('NAME', 'IMGNAME');
                            ifrm.setAttribute('CLASS', 'FileFrame');

                            sUrl = 'FileRetrieve.aspx?ID=' + argImgID;
                            ifrm.setAttribute('SRC', sUrl);
                            divWorkArea.appendChild(ifrm);
                            break;
                    }
                }
            }
            catch (err) { alert('Display_Image() - ' + err); }
            finally { sUrl = null; divWorkArea = null; ifrm = null; imgCtrl = null; }
        }
    </script>
    <!--<script language="javascript" src="~/Scripts/MyJavaScript.js"></script>	-->
</head>
<body>
    <form id="form1" method="post">
        <div>
            <%=strGridView%>
        </div>
        <BR>
        <BR>
        <div id="DivWorkArea" class="auto-style10">
        </div>
    </form>
</body>
</html>
