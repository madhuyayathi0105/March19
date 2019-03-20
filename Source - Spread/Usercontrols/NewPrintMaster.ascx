<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewPrintMaster.ascx.cs"
    Inherits="Usercontrols_NewPrintMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
<script type="text/javascript">
    function OnCheckBoxCheckChanged(evt) {

        var src = window.event != window.undefined ? window.event.srcElement : evt.target;
        var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
        if (isChkBoxClick) {
            var parentTable = GetParentByTagName("table", src);
            var nxtSibling = parentTable.nextSibling;
            if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
            {
                if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                {
                    //check or uncheck children at all levels
                    CheckUncheckChildren(parentTable.nextSibling, src.checked);
                }
            }
            //check or uncheck parents at all levels
            CheckUncheckParents(src, src.checked);
        }
    }

    function CheckUncheckChildren(childContainer, check) {
        var childChkBoxes = childContainer.getElementsByTagName("input");
        var childChkBoxCount = childChkBoxes.length;
        for (var i = 0; i < childChkBoxCount; i++) {
            childChkBoxes[i].checked = check;
        }
    }
    function CheckUncheckParents(srcChild, check) {
        var parentDiv = GetParentByTagName("div", srcChild);
        var parentNodeTable = parentDiv.previousSibling;

        if (parentNodeTable) {
            var checkUncheckSwitch;

            if (check) //checkbox checked
            {
                var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                if (isAllSiblingsChecked)
                    checkUncheckSwitch = true;
                else
                    return; //do not need to check parent if any(one or more) child not checked
            }
            else //checkbox unchecked
            {
                checkUncheckSwitch = false;
            }

            var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
            if (inpElemsInParentTable.length > 0) {
                var parentNodeChkBox = inpElemsInParentTable[0];
                parentNodeChkBox.checked = checkUncheckSwitch;
                //do the same recursively
                CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
            }
        }
    }
    function AreAllSiblingsChecked(chkBox) {
        var parentDiv = GetParentByTagName("div", chkBox);
        var childCount = parentDiv.childNodes.length;
        for (var i = 0; i < childCount; i++) {
            if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
            {
                if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                    var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                    //if any of sibling nodes are not checked, return false
                    if (!prevChkBox.checked) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    //utility function to get the container of an element by tagname
    function GetParentByTagName(parentTagName, childElementObj) {
        var parent = childElementObj.parentNode;
        while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
            parent = parent.parentNode;
        }
        return parent;
    }
    
</script>
<%--<script type="text/javascript">
    function OnLoad() {
        var links = document.getElementById("<%=treeview_spreadfields.ClientID %>").getElementsByTagName("a");
        for (var i = 0; i < links.length; i++) {
            links[i].setAttribute("href", "javascript:NodeClick(\"" + links[i].id + "\", \"" + links[i].getAttribute("href") + "\")");
        }
    }
    window.onload = OnLoad;
    function NodeClick(id, attribute) {
        //Do Something
        var nodeLink = document.getElementById(id);
        alert(nodeLink.innerHTML + " clicked");
      
        //Execute the server side event.
        eval(attribute);
    }--%>
</script>
<style type="text/css">
    .printclass
    {
        display: none;
    }
    .marginSet
    {
        margin: 0px;
        padding: 0px;
    }
    .headerDisp
    {
        font-size: 25px;
        font-weight: :normal;
    }
    .headerDisp1
    {
        font-family: Book Antiqua;
        font-size: medium;
    }
     .datacellone
    {
        background-color: #bada55;
    }
    .datacelltwo
    {
       background-color: lightblue;
    }
    .topright {
    position: absolute;
    top: 20px;
    right: 50px;
    font-size: 18px;
    
}
.bottomright {
    position: absolute;
    bottom: 8px;
    right: 16px;
    font-size: 18px;
}
    @media print
    {
        #printdiv
        {
            display: block;
        }
        .printclass
        {
            display: block;
            font-family: Book Antiqua;
        }
        .noprint
        {
            display: none;
        }
       
      .page-break  { display:block; page-break-before:always; }
        tr:nth-of-type(10n){
    page-break-after: always;
  }

    }
    @media screen,print
    {
    
    }
    @page
    {
        size: A3;
        margin: 0;
    }
   
     .odd{background-color: white;} 
  .even{background-color: gray;} 
 
  <%--  tbody tr:nth-child(even)
    {
        background-color: #bada55;
    }
    
    tbody tr:nth-child(odd)
    {
        background-color: lightblue;
    }--%>
   
</style>
<script type="text/javascript">
    //    function PrintDiv() {
    //        var panel = document.getElementById("<%=contentDiv.ClientID %>");
    //        var printWindow = window.open('', '', 'height=auto,width=auto');
    //        printWindow.document.write('<html');
    //        printWindow.document.write('<head> <style type="text/css"> p{ font-size:small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
    //        printWindow.document.write('</head><body>');
    ////        printWindow.document.write('<form>');
    //        printWindow.document.write(panel.innerHTML);
    ////        printWindow.document.write(' </form>');
    //        printWindow.document.write('</body></html>');
    //        printWindow.document.close();
    //        setTimeout(function () {
    //            printWindow.print();
    //        }, 500);
    //        return false;
    //    }

    function PrintDiv() {
        var panel = document.getElementById("<%=contentDiv.ClientID %>");
        var printWindow = window.open('', '', 'height=auto,width=auto');
        printWindow.document.write('<html><head>');
        printWindow.document.write('<style>body, html {margin:0;padding:0;height:100%;} .classRegular { font-family:Arial; font-size:10px; } .classBold10 { font-family:Arial; font-size:12px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:14px; font-weight:bold;} .classBold { font-family:Arial; font-size:10px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(panel.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    
</script>
<asp:Image ID="Image1" runat="server" Visible="false" Width="16px" />
<asp:Image ID="Image2" runat="server" Visible="false" />
<asp:UpdatePanel ID="updprint" runat="server">
    <ContentTemplate>
        <%--    <center>
            <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="SaveSizeInfo">
                <ProgressTemplate>
                    <center>
                        <div style="height: 40px; width: 150px;">
                            <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="ModalPopupExtender14" runat="server" TargetControlID="UpdateProgress14"
                PopupControlID="UpdateProgress14">
            </asp:ModalPopupExtender>
        </center>--%>
        <asp:Image ID="printheader" runat="server" Style="top: 1px; left: 1px; position: absolute;
            height: 1px; width: 1px;" />
        <asp:Panel ID="pnlforlistbox" runat="server" Style="top: 300px; left: 151px; position: absolute;"
            Width="650px" Height="850px" BackColor="AliceBlue" BorderStyle="Solid" BorderWidth="2">
            <asp:Label ID="lblprint" Text="Print Master Settings" runat="server" Font-Bold="True"
                Font-Size="Large" Font-Names="Book Antiqua" Style="top: 6px; left: 240px; position: absolute;"></asp:Label>
            <br />
            <asp:CheckBox ID="Chk_sel" runat="server" Text="Select All" Style="top: 30px; left: 10px;
                position: absolute;" Font-Bold="True" OnCheckedChanged="Chk_sel_CheckedChanged"
                AutoPostBack="true" />
            <asp:CheckBox ID="Chk_sell" runat="server" Text="Select All" Style="top: 30px; left: 400px;
                position: absolute;" Font-Bold="True" OnCheckedChanged="Chk_sell_CheckedChanged"
                AutoPostBack="true" />
            <asp:CheckBox ID="chkcollegeheader" runat="server" Text="College Header Image" Style="top: 30px;
                left: 483px; position: absolute;" Font-Bold="True" OnCheckedChanged="chkcollegeheader_CheckedChanged"
                AutoPostBack="true" />
            <br />
            <asp:Panel ID="first_tree_panel" runat="server" BorderStyle="Solid" Direction="LeftToRight"
                ScrollBars="Auto" BackColor="White" Height="300px" Width="350px" Style="top: 50px;
                left: 10px; position: absolute;" BorderWidth="1">
                <asp:Label ID="first_tree_lbl" runat="server" Text="Fields Available For Printing"
                    Font-Bold="true" Font-Size="Medium" Style="top: 4px; left: 30px; position: absolute;
                    text-align: left"></asp:Label>
                <asp:TreeView ID="treeview_spreadfields" runat="server" SelectedNodeStyle-ForeColor="Red"
                    HoverNodeStyle-BackColor="Black" Height="258px" Width="343px" Font-Names="Book Antiqua"
                    ForeColor="Black" Style="top: 22px; left: 1px; position: absolute; text-align: left">
                </asp:TreeView>
                <br />
            </asp:Panel>
            <asp:Panel ID="clgchecklist_pnl" runat="server" BorderStyle="Solid" Direction="LeftToRight"
                ScrollBars="Auto" BackColor="White" Style="top: 50px; left: 400px; position: absolute;
                height: 300px; width: 230px; text-align: left" BorderWidth="1">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="clgChklist_lbl" runat="server" Text="College Header Fields" Font-Bold="true"
                                Font-Size="Medium" Style="top: 4px; left: 50px; position: absolute; text-align: left"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="chkcollege" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                                Width="208px" Style="top: 22px; left: 20px; position: absolute; text-align: left">
                                <asp:ListItem>College Name</asp:ListItem>
                                <asp:ListItem>University</asp:ListItem>
                                <asp:ListItem>Affliated By</asp:ListItem>
                                <asp:ListItem>Address</asp:ListItem>
                                <asp:ListItem>City</asp:ListItem>
                                <asp:ListItem>District & State & Pincode</asp:ListItem>
                                <asp:ListItem>Phone No & Fax</asp:ListItem>
                                <asp:ListItem>Email & Web Site</asp:ListItem>
                                <asp:ListItem>Right Logo</asp:ListItem>
                                <asp:ListItem>Left Logo</asp:ListItem>
                                <asp:ListItem>Signature</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div id="maindiv" runat="server" visible="true">
                <fieldset style="border: thin double Black; font-size: medium; font-style: normal;
                    top: 350px; left: 21px; height: 20px; width: 267px; position: absolute;">
                    <legend>Header </legend>
                    <asp:RadioButtonList ID="radioheader" runat="server" RepeatDirection="Horizontal"
                        Style="top: -3px; left: 0px; height: 25px; width: 266px; position: absolute;">
                        <asp:ListItem>All Pages</asp:ListItem>
                        <asp:ListItem>First Page</asp:ListItem>
                        <asp:ListItem>No Header</asp:ListItem>
                    </asp:RadioButtonList>
                </fieldset>
                <fieldset style="font-size: medium; border-color: Black; font-style: normal; top: 350px;
                    left: 350px; height: 20px; right: 28px; width: 268px; position: absolute;">
                    <legend>Footer </legend>
                    <asp:RadioButtonList ID="radiofooter" runat="server" Style="top: -3px; left: 0px;
                        height: -3px; width: 268px; position: absolute;" RepeatDirection="Horizontal">
                        <asp:ListItem>All Pages</asp:ListItem>
                        <asp:ListItem>Last Page</asp:ListItem>
                        <asp:ListItem>No Footer</asp:ListItem>
                    </asp:RadioButtonList>
                </fieldset>
                <fieldset style="font-size: medium; border-color: Black; font-style: normal; top: 420px;
                    left: 20px; height: 170px; right: 124px; width: 550px; position: absolute;">
                    <legend>Additional Header/Footer/ISO No</legend>
                    <asp:Label ID="lblrow" runat="server" Text="No of Row : " Font-Bold="True" Font-Names="Book Antoqua"
                        Style="top: 0px; left: 0px; height: 19px; width: 82px; position: absolute;"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtrow" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                        Style="top: 0px; left: 90px; height: 15px; width: 42px; position: absolute;"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:FilteredTextBoxExtender ID="ftf1" FilterType="Numbers" runat="server" TargetControlID="txtrow">
                    </asp:FilteredTextBoxExtender>
                    <asp:Label ID="lblcolumn" runat="server" Text="No of Columns : " Font-Bold="True"
                        Font-Names="Book Antoqua" Style="top: 0px; left: 150px; height: 19px; width: 110px;
                        position: absolute;"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtcolumn" runat="server" Font-Bold="True" Width="50px" Font-Names="Book Antoqua"
                        Style="top: 0px; left: 270px; height: 15px; width: 42px; position: absolute;"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:FilteredTextBoxExtender ID="ftf2" FilterType="Numbers" runat="server" TargetControlID="txtcolumn">
                    </asp:FilteredTextBoxExtender>
                    <asp:Label ID="lbladdtional" runat="server" Text="Additional : " Font-Bold="True"
                        Font-Names="Book Antoqua" Style="top: 0px; left: 320px; height: 19px; width: 82px;
                        position: absolute;"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="ddladd" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                        AutoPostBack="true" OnSelectedIndexChanged="ddladd_SelectedIndexChanged" Style="top: 0px;
                        left: 420px; height: 19px; width: 82px; position: absolute;">
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem>Header</asp:ListItem>
                        <asp:ListItem>Footer</asp:ListItem>
                        <asp:ListItem>ISO Code</asp:ListItem>
                    </asp:DropDownList>
                    <%-- <FarPoint:FpSpread ID="FpFooter" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="100" Width="600" Enabled="False" VerticalScrollBarPolicy="AsNeeded"
                HorizontalScrollBarPolicy="AsNeeded">
                <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" Visible="true">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>--%>
                    <asp:GridView ID="FpFooter" Width="750px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        Font-Names="Book Antiqua" toGenerateColumns="true">
                        <%--AllowPaging="true" PageSize="20" OnPageIndexChanging="GrdVNonBkAccNo_OnPageIndexChanged"--%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                    <asp:Button ID="btnok" runat="server" Text="Ok" Font-Bold="true" Width="50px" OnClick="btnok_Click"
                        Enabled="False" />
                </fieldset>
                <asp:Label ID="lblsection" runat="server" Text="Section" Font-Bold="true" Font-Names="Book Antoqua"
                    Style="top: 650px; left: 10px; height: 19px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlsection" AutoPostBack="true" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged"
                    runat="server" Font-Bold="True" Font-Names="Book Antoqua" Style="top: 650px;
                    left: 120px; height: 19px; width: 82px; position: absolute;">
                    <asp:ListItem>Header</asp:ListItem>
                    <asp:ListItem>Body</asp:ListItem>
                    <asp:ListItem>Footer</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblfont" runat="server" Text="Font" Font-Bold="true" Font-Names="Book Antoqua"
                    Style="top: 650px; left: 220px; height: 19px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlfont" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                    AutoPostBack="true" Style="top: 650px; left: 260px; height: 19px; width: 100px;
                    position: absolute;">
                    <asp:ListItem>Book Antoqua</asp:ListItem>
                    <asp:ListItem>Times New Roman</asp:ListItem>
                    <asp:ListItem>Arial</asp:ListItem>
                    <asp:ListItem>Arial Narrow</asp:ListItem>
                    <asp:ListItem>Arial Narrow</asp:ListItem>
                    <asp:ListItem>Arial Black</asp:ListItem>
                    <asp:ListItem>Cambria</asp:ListItem>
                    <asp:ListItem>Franklin Gothic Book</asp:ListItem>
                    <asp:ListItem>Garamond</asp:ListItem>
                    <asp:ListItem>Harrington</asp:ListItem>
                    <asp:ListItem>Lucida Bright</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblsize" runat="server" Text="Size" Font-Bold="true" Font-Names="Book Antoqua"
                    Style="top: 650px; left: 370px; height: 19px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlsize" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                    AutoPostBack="true" Style="top: 650px; left: 420px; height: 19px; width: 50px;
                    position: absolute;">
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnset" Text="Set" runat="server" Font-Bold="true" Font-Names="Book Antoqua"
                    Style="top: 650px; left: 510px; position: absolute;" OnClick="btnset_Click" />
                <asp:Button ID="btnsavesize" Text="Size" runat="server" Font-Bold="true" Font-Names="Book Antoqua"
                    Style="top: 650px; left: 571px; position: absolute;" OnClick="btnsavesize_Click" />
                <asp:Label ID="lblnofrow" runat="server" Text="No of Rows Per Page" Font-Bold="true"
                    Font-Size="Medium" Font-Names="Book Antoqua" Style="top: 720px; left: 10px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlheader" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                    Style="top: 720px; left: 170px; position: absolute;">
                    <asp:ListItem>With Header</asp:ListItem>
                    <asp:ListItem>With out Header</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtnofrow" runat="server" Font-Bold="True" Width="50px" Font-Names="Book Antoqua"
                    Style="top: 720px; left: 300px; height: 15px; width: 42px; position: absolute;"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" runat="server"
                    TargetControlID="txtnofrow">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnrow" Text="Set" runat="server" Font-Bold="true" Style="top: 720px;
                    left: 360px; position: absolute;" OnClick="btnrow_Click" />
            </div>
            <asp:Label ID="lblpagesize" runat="server" Text="Page Size" Font-Bold="true" Font-Size="Medium"
                Font-Names="Book Antoqua" Style="top: 680px; left: 10px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddlpagesize" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                Style="top: 680px; left: 120px; position: absolute;">
                <asp:ListItem>A4</asp:ListItem>
                <asp:ListItem>A3</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblorientation" runat="server" Text="Orientation" Font-Bold="true"
                Font-Size="Medium" Font-Names="Book Antoqua" Style="top: 680px; left: 220px;
                position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddlorientation" runat="server" Font-Bold="True" Font-Names="Book Antoqua"
                Style="top: 680px; left: 310px; position: absolute;">
                <asp:ListItem>Portrait</asp:ListItem>
                <asp:ListItem>Landscape</asp:ListItem>
            </asp:DropDownList>
            <asp:CheckBox ID="chkcolour" runat="server" Text="Colour" Font-Bold="true" Font-Size="Medium"
                Font-Names="Book Antoqua" Style="top: 680px; left: 410px; position: absolute;" />
            <asp:Label ID="Label1" runat="server" Text="Pading" Font-Bold="true" Font-Size="Medium"
                Font-Names="Book Antoqua" Style="top: 680px; left: 509px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtpading" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antoqua"
                Width="70px" Style="top: 680px; left: 564px; position: absolute;"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Numbers,Custom"
                ValidChars="." runat="server" TargetControlID="txtpading">
            </asp:FilteredTextBoxExtender>
            <asp:CheckBox ID="chkfitpaper" runat="server" Text="Fit The Paper" Font-Bold="true"
                Font-Size="Medium" Font-Names="Book Antoqua" Style="top: 720px; left: 509px;
                position: absolute;" />
            <asp:CheckBox ID="chksno" runat="server" Text="S.No" Font-Bold="true" Font-Size="Medium"
                Font-Names="Book Antoqua" Style="top: 720px; left: 410px; position: absolute;" />
            <asp:FileUpload runat="server" ID="Fpimage" Style="top: 760px; left: 10px; position: absolute;" />
            <asp:CheckBox ID="chktblfalse" runat="server" Text="Table Visible False" Font-Bold="true"
                Font-Size="Medium" Font-Names="Book Antoqua" Style="top: 760px; left: 360px;
                position: absolute;" />
            <asp:Button ID="btnimagesave" Text="Save" runat="server" Font-Bold="true" Style="top: 760px;
                left: 250px; position: absolute;" OnClick="btnimagesave_Click" />
            <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"
                Style="top: 795px; left: 10px; position: absolute;"></asp:Label>
            <asp:Button ID="pdfval" Text="Print Pdf" runat="server" Font-Bold="true" Style="top: 790px;
                left: 70px; position: absolute;" OnClick="btnprintpdf_Click" />
            <asp:Button ID="excelconvert" Text="Excel" runat="server" Font-Bold="true" Style="top: 790px;
                left: 169px; position: absolute;" OnClick="btnexcel_Click" />
            <asp:Button ID="btnprint" Text="Print" runat="server" Font-Bold="true" Style="top: 790px;
                left: 250px; position: absolute;" OnClick="btnprint_Click" />
            <asp:Button ID="btnreset" Text="Reset" runat="server" Font-Bold="true" Style="top: 790px;
                left: 318px; position: absolute;" OnClick="btnreset_Click" />
            <asp:Button ID="btnclose" Text="Close" runat="server" Font-Bold="true" Style="top: 790px;
                left: 400px; position: absolute;" OnClick="btnclose_Click" />
            <asp:CheckBox ID="chkSetCommPrint" Text="Set Common Print" Checked="false" runat="server"
                Font-Bold="true" Style="top: 790px; left: 470px; position: absolute;" />
            <br />
            <center>
                <div id="div_HeaderLed" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 10px; margin-left: 318px;"
                        OnClick="imagebtnpopLedgerclose_Click" />
                    <div style="background-color: White; height: 500px; width: 672px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <div>
                            <span class="fontstyleheader" style="color: Green;">SIZE SETTINGS</span></div>
                        <div id="div2" runat="server" style="width: 667px; height: 409px; overflow: auto;">
                            <asp:GridView ID="grid_Fields" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                Width="650px" OnDataBound="gdSetting_OnDataBound">
                                <%--OnDataBound="grid_HeaderLedger_OnDataBound"--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_serial1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Label" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%-- <asp:Label ID="lbl_fields" runat="server" Visible="false" Text='<%#Eval("LedgerPK") %>'></asp:Label>--%>
                                            <asp:Label ID="lbl_fields" runat="server" Text='<%#Eval("Fields") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Change Label" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Edit" runat="server" CssClass="  textbox txtheight" Style="text-align: left;"></asp:TextBox>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Size" runat="server" onblur="checkFloatValue(this);" CssClass="  textbox txtheight"
                                                    Style="text-align: right;"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_Size"
                                                    FilterType="numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header Align" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:DropDownList ID="ddlHeader" runat="server" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value Align" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:DropDownList ID="ddlValue" runat="server" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lbl_ReportName" runat="server" Text="Report Name" Font-Bold="true"
                            Font-Size="small" ForeColor="Green"></asp:Label>
                        <asp:TextBox ID="txt_ReportName" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                            margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="SaveSizeInfo" Text="SAVE" runat="server" Font-Bold="true" OnClick="btnsave_Click" />
                    </div>
                </div>
            </center>
        </asp:Panel>
        <%--<div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 10000;
    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
    left: 0px;">
    <center>
        <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
            margin-top: 200px; border-radius: 10px;">
            <table>
                <tr>
                    <td align="center">
                        <asp:Label ID="lbl_description11" runat="server" Text="Type" Font-Bold="true" Font-Size="Large"
                            ForeColor="Green"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:TextBox ID="txt_description11" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                            margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopadd_Click" />
                        <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopexit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
</div>--%>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnprint" />
        <%--  <asp:PostBackTrigger ControlID="btnclose" />--%>
        <asp:PostBackTrigger ControlID="ddladd" />
        <asp:PostBackTrigger ControlID="btnok" />
        <asp:PostBackTrigger ControlID="btnrow" />
        <%-- <asp:PostBackTrigger ControlID="btnset" />--%>
        <asp:PostBackTrigger ControlID="btnimagesave" />
        <%--      <asp:PostBackTrigger ControlID="btnreset" />--%>
        <asp:PostBackTrigger ControlID="excelconvert" />
        <asp:PostBackTrigger ControlID="pdfval" />
    </Triggers>
</asp:UpdatePanel>
<div style="height: 1px; width: 1px; overflow: auto;">
    <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
    </div>
</div>
