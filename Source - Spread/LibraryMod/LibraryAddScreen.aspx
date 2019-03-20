<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LibraryAddScreen.aspx.cs"
    Inherits="LibraryMod_LibraryAddScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
<link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .txtcaps
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .maindivstylesize
        {
            height: 1300px;
            width: 1000px;
        }
    </style>
    <script type="text/javascript">
     function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        <%--Student tab time--%>
        setInterval(function () {
            document.getElementById("<%=txt_time.ClientID %>").value = (new Date()).toLocaleTimeString();
            return false;
        }, 1000);

        function HidePop() {
        var seconds = 1;
        setTimeout(function () {
            document.getElementById("<%=divWelcome.ClientID %>").style.display = "none";
            document.getElementById("<%=LblName.ClientID %>").style.display = "none";            
            document.getElementById("<%=LblDept.ClientID %>").style.display = "none";
            document.getElementById("<%=img_stud1.ClientID %>").style.display = "none";
            
        }, seconds * 1500);
    }
   
   
        function SelectAll(id) { 
            //get reference of GridView control
            var grid = document.getElementById("<%= grdManualExit.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[1];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
        function fundate()
    {
   
    var todt = new Date().toLocaleDateString(undefined, {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
});
 var to = todt;
            var sp = to.split("/");             
            to = sp[1] + '/' + sp[0] + '/' + sp[2];             
            document.getElementById("<%=LblDate.ClientID %>").value=to; 
           
}
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <br />
            <div>
                <asp:Label ID="LblCollegeName" runat="server" Style="margin: 0px; font-size: 34px;
                    margin-top: 15px; margin-bottom: 15px; position: relative;" ForeColor="Green"
                    CssClass="fontstyleheader"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Style="margin: 0px; font-size: 34px; margin-top: 60px;
                    margin-bottom: 30px; position: relative;" Text="E-Gate Entry" ForeColor="Green"
                    CssClass="fontstyleheader"></asp:Label>
            </div>
            <br />
            <table class="maintablestyle" style="font-weight: bold; background-color: #0CA6CA;
                font-family: Book Antiqua; font-weight: bold; font-size: large; height: 40px;
                width: 1000px;">
                <tr>
                    <td>
                        <asp:Label ID="LblUser" runat="server" Text="USER ENTRY :-"></asp:Label>
                        <asp:Label ID="LblLibrarian" runat="server" Text="Librarian Name :"></asp:Label>
                        <asp:Label ID="LblLibrarianName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="font-weight: normal; font-family: Book Antiqua; background-image: url('../images/libentry.jpg');
                background-repeat: no-repeat; font-weight: bold; font-family: Book Antiqua; height: 400px;
                width: 1000px;">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="LblDt" runat="server" Text="Date: " Style="margin-left: 847px;"></asp:Label>
                        <asp:TextBox ID="LblDate" runat="server" CssClass="txtcaps txtheight" Style="width: 90px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="LblTim" runat="server" Text="Time: " Style="margin-left: 847px;"></asp:Label>
                        <asp:TextBox ID="txt_time" runat="server" CssClass="txtcaps txtheight" Style="width: 90px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged" Width="200px"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="LibraryName "></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_LibName" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            OnSelectedIndexChanged="ddlLibName_OnSelectedIndexChanged" Width="200px" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Label ID="LblLibName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lbl_RollNo" runat="server" Text="Staff Code/Roll No "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Txt_UserID" runat="server" MaxLength="20" OnTextChanged="Txt_UserID_OnTextChanged"
                            AutoPostBack="true" Style="width: 191px; height: 20px; margin-left: 1px"></asp:TextBox>
                        <asp:TextBox ID="Txt_SmartCardID" runat="server" Visible="false" OnTextChanged="Txt_SmartCardID_OnTextChanged"
                            Style="width: 150px; height: 20px;"></asp:TextBox>
                        <asp:TextBox ID="Txt_RollNo" runat="server" Visible="false" Style="width: 150px;
                            height: 20px;"></asp:TextBox>
                        <asp:Label ID="Lbl_Semester" runat="server" Visible="false" Text=""></asp:Label>
                    </td>
                    <td rowspan="3">
                        <asp:Image ID="img_stud1" runat="server" Visible="false" Style="height: 118px; width: 105px;
                            margin-left: 129px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Name "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblName" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="Txt_VisitorName" runat="server" Visible="false" Style="width: 150px;
                            height: 20px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Department "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblDept" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <asp:Image ID="img_BstUser" runat="server" ImageUrl="~/images/BestUserLibrary1.jpg"
                                        Style="height: 50px; width: 80px;" />
                                </td>
                                <td style="color: Blue; font-family: Book Antiqua">
                                    <asp:Label ID="Label4" runat="server" Text="Student:"></asp:Label>
                                    <asp:Label ID="LblStuName" runat="server" Style="margin-left: 102px;" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="Label6" runat="server" Text="Staff:"></asp:Label>
                                    <asp:Label ID="LblStaffName" runat="server" Style="margin-left: 124px;" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblHit" runat="server" Text="Hit Status "></asp:Label>
                    </td>
                    <td>
                        <%-- <asp:Label ID="LblToday" runat="server" Text="Today" Style="margin-left: 173px;"></asp:Label>
                    <asp:Label ID="LbltodayDt" runat="server" Text=""></asp:Label>--%>
                    </td>
                    <td>
                        <asp:Label ID="LblThismth" runat="server" Text="This Month" Style="margin-left: 12px;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="LblOut" runat="server" Text="In" Style="margin-left: 0px;"></asp:Label>
                        <asp:Label ID="LblTot" runat="server" Text="Out" Style="margin-left: 60px;"></asp:Label>
                        <asp:Label ID="LblInOutTotal" runat="server" Text="Total" Style="margin-left: 53px;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblMonth" runat="server" Text="Total" Style="margin-left: 38px;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblStaff" runat="server" Text="Staff" Style="margin-left: 2px;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblStaffIn" runat="server" Text="" Style="margin-left: 2px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblStaffOut" runat="server" Text="" Style="margin-left: 68px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblStaffTotal" runat="server" Text="" Style="margin-left: 75px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblStaffMthTot" runat="server" Text="" Style="margin-left: 46px; color: Blue;
                            font-family: Book Antiqua; text-align: right;"></asp:Label>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblStu" runat="server" Text="Student" Style="margin-left: 2px;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblStuIn" runat="server" Text="" Style="margin-left: 2px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblStuOut" runat="server" Text="" Style="margin-left: 68px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblStuTotal" runat="server" Text="" Style="margin-left: 75px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblStuMthTot" runat="server" Text="" Style="margin-left: 46px; color: Blue;
                            font-family: Book Antiqua; text-align: right;"></asp:Label>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblVisit" runat="server" Text="Visitor" Style="margin-left: 2px;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblVisitIn" runat="server" Text="" Style="margin-left: 2px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblVisitOut" runat="server" Text="" Style="margin-left: 68px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="LblVisitTotal" runat="server" Text="" Style="margin-left: 75px; text-align: right;
                            color: Blue; font-family: Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblVisitMthTot" runat="server" Text="" Style="margin-left: 46px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lbl_Tot" runat="server" Text="Total" Style="margin-left: 2px;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Lbl_TotIn" runat="server" Text="" Style="margin-left: 2px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="Lbl_TotOut" runat="server" Text="" Style="margin-left: 68px; color: Blue;
                            font-family: Book Antiqua"></asp:Label>
                        <asp:Label ID="Lbl_TotStrength" runat="server" Text="" Style="margin-left: 75px;
                            color: Blue; font-family: Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LblMthTot" runat="server" Text="" Style="margin-left: 46px; color: Blue;
                            font-family: Book Antiqua; text-align: right;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:UpdatePanel ID="UpExitOrManual" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btn_Exit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="btn_Exit_OnClick" />
                                <asp:ImageButton ID="btn_ManualExit" runat="server" ImageUrl="~/LibImages/Manual Exit.jpg"
                                    OnClick="btn_Manual_OnClick" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <%-- Popup for Exit--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="DivExit" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblExit" runat="server" Text="Enter Password To Exit:" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:TextBox ID="Txt_Password" runat="server" Style="width: 150px; height: 20px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_Ok_Click" />
                                            </center>
                                        </td>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="Button2" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                    OnClick="btn_exit_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Popup for Manual Exit--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="DivManualExit" runat="server" visible="false" class="popupstyle popupheight1 "
                    style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2);
                    position: absolute; top: 0; left: 0px;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-left: 454px; margin-top: 36px;"
                        OnClick="imagebtnPospopclose_Click" />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 800px; width: 1000px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Manual Entry</span>
                            </div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 850px;">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlLib_ManualExit" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddlLibName_OnSelectedIndexChanged" Width="200px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="Btn_Search" runat="server" Style="margin-left: -112px;" ImageUrl="~/LibImages/Go.jpg"
                                        OnClick="BtnSearch_Click" />
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="LblExitTime" runat="server" Text="Exit Time" Style="margin-left: 151px;"></asp:Label>
                                </td>
                                <td>
                                    <cc1:TimeSelector ID="TimeSelector1" runat="server" repeatdirection="Horizondal"
                                        Enabled="false" AllowSecondEditing="true" MinuteIncrement="1" SecondIncrement="1">
                                    </cc1:TimeSelector>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divExitSpread" runat="server" visible="false" style="width: 950px; height:600px; overflow: auto;
                            background-color: White; border-radius: 10px;">
                            <center>
                                <asp:GridView ID="grdManualExit" runat="server" Width="930px" ShowFooter="false"
                                    AutoGenerateColumns="false" Font-Names="Book Antiqua" toGenerateColumns="false"
                                    OnRowDataBound="grdManualExit_RowDataBound">
                                    <%--AllowPaging="true" PageSize="10"  OnPageIndexChanging="grdManualExit_OnPageIndexChanged"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="allchk" runat="server" Text="Select All" onchange="return SelLedgers();" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="selectchk" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="120px" DataField="Roll_No" HeaderText="Roll No" />
                                        <asp:BoundField ItemStyle-Width="200px" DataField="Stud_Name" HeaderText="Name" />
                                        <asp:BoundField ItemStyle-Width="200px" DataField="Dept_Name" HeaderText="Department" />
                                        <asp:BoundField ItemStyle-Width="200px" DataField="Entry_Date" HeaderText="Entry Date" />
                                        <asp:BoundField ItemStyle-Width="110px" DataField="Entry_Time" HeaderText="Entry Time" />
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="White" />
                                </asp:GridView>
                            </center>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpSave" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="BtnSave_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Popup for Manual Exit ok--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="DivManualExitok" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblManualOk" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="BtnManualOk" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="BtnManualOk_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Popup for Welcome Message--%>
    <center>
        <div id="divWelcome" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblWel" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for Password Message--%>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <div id="imgPassWord" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 270px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblExitPass" runat="server" Text="Enter Exit Password" Style="color: Black;
                                                    margin-left: -2px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="6" Style="height: 15px; width: 85px;
                                                    margin-left: 10px;" CssClass="textbox textbox1" TextMode="Password"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredtxtPassword" runat="server" TargetControlID="txtPassword"
                                                    FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="btn_Ok" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_ExitScreenOk_Click" />
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="btn_ExitScreen_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <%-- Popup for Error Message--%>
                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_alertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                        OnClick="btn_errorclose_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--progressBar for UpExitOrManual--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpExitOrManual">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpSave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    </form>
</body>
</html>
