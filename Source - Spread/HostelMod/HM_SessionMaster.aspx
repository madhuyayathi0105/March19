<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_SessionMaster.aspx.cs" Inherits="HM_SessionMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
        <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    </head>
    <script type="text/javascript">

        function valid() {
            var id = "";
            var value1 = "";
            var idval = "";
            var empty = "";
            id = document.getElementById("<%=txt_hostelname1.ClientID %>").value;
            if (id.trim() == "--Select--") {
                id = document.getElementById("<%=txt_hostelname1.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_sessionname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_sessionname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function blurFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }

        function get(txt1) {
            $.ajax({
                type: "POST",
                url: "HM_SessionMaster.aspx/CheckUserName",
                data: '{Session_Name: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccess(response) {
            var mesg = $("#msg1")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Session Name Not Exist";
                    break;
                case "1":
                    mesg.style.color = "green";

                    document.getElementById('<%=txt_sessionname.ClientID %>').value = "";
                    mesg.innerHTML = "Session Available";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter Session Name";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error Occurred";
                    break;
            }
        }
    </script>
    <body>
        <form id="form1">
        <div>
            <center>
                <br />
                <div>
                    <span style="color: Green;" class="fontstyleheader">Session Master</span>
                    <br />
                    <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 520px; width: 1000px;">
                <br />
                <table style="margin-left: 147px; position: absolute; width: 650px; height: 50px;"
                    class="maintablestyle">
                    <tr>
                        <td>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:Label ID="lbl_hostelname" Text="Mess Name" Style="top: 15px; left: 10px; position: absolute;
                                font-family: 'Book Antiqua'" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="120px" Height="18px" Style="top: 10px; left: 104px; position: absolute;">--Select--</asp:TextBox>
                                    <asp:Panel ID="phostel" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 180px;">
                                        <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_hostel_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_hostelname"
                                        PopupControlID="phostel" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sessionname1" Text="Session Name" Style="top: 15px; left: 244px;
                                position: absolute; font-family: 'Book Antiqua'" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sessionname1" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="120px" Height="18px" Style="top: 10px; left: 353px; position: absolute;">--Select--</asp:TextBox>
                                    <asp:Panel ID="Psession" runat="server" CssClass="multxtpanel" Style="height: 150px;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_sessionname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_session_CheckedChange" />
                                        <asp:CheckBoxList ID="cbl_session" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_session_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sessionname1"
                                        PopupControlID="Psession" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" runat="server" Style="top: 10px; left: 505px; position: absolute;
                                font-family: 'Book Antiqua'" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" Text="Add New" runat="server" Style="top: 10px; left: 557px;
                                position: absolute; font-family: 'Book Antiqua'" CssClass="textbox btn2" OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <div style="width: 328px;">
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
                <div id="div1" runat="server" visible="false" style="width: 850px; height: 350px;"
                    class="reportdivstyle spreadborder">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="750px" Height="350px" ShowHeaderSelection="false"
                        OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" CssClass="textbox textbox1"
                        onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                        CssClass="textbox textbox1 btn1" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox textbox1 btn2" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 144px; margin-left: 309px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; width: 640px; height: 390px;">
                        <br />
                        <div>
                            <asp:Label ID="lbl_sessionheader" Font-Bold="true" Style="font-size: large; color: Green;"
                                Text="Session Name" runat="server"></asp:Label>
                        </div>
                        <br />
                        <center>
                            <table style="line-height: 35px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hostel1" Text="Mess Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upp1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_hostelname1" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    onfocus="return myFunction(this)" Width="164px" Height="20px">--Select--</asp:TextBox>
                                                <span style="color: Red;">*</span>
                                                <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                                    height: 200px; line-height: 20px;">
                                                    <asp:CheckBox ID="cb_hostelname1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_hostelname1_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_hostelname1" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        AutoPostBack="true" OnSelectedIndexChanged="cb_hostelname1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hostelname1"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sessionname" Text="Session Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sessionname" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                            onblur="return get(this.value)" runat="server" Style="width: 200px;"></asp:TextBox>
                                        <span style="color: Red;">*</span> <span id="msg1"></span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_sessionname"
                                            FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <%-- <asp:DropDownList ID="ddlpopsessionname" runat="server" Width="200px" Height="30px"
                                    CssClass="textbox textbox1">
                                </asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_starttime1" Text="Start Time" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hour" Width="50px" Height="25px" runat="server" AutoPostBack="true"
                                            CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_hour_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_minits" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_seconds" Width="50px" Height="25px" Visible="false" runat="server"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_timeformate" Width="50px" Height="25px" runat="server"
                                            CssClass="textbox textbox1">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_endtime1" Text="End Time" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_endhour" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endminit" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endsecnonds" Width="50px" Height="25px" Visible="false"
                                            runat="server" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_endformate" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_Extentionallow1" Text="Extension Allowed" AutoPostBack="true"
                                            OnCheckedChanged="cb_Extentionallow1_CheckedChanged" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div id="subdiv" runat="server" visible="false">
                                            <asp:Label ID="lbl_Extentiontimeup" Text="Extension Time Upto" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddl_exhour" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_exminitus" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_exseconds" Width="50px" Height="25px" Visible="false" runat="server"
                                                CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_exformate" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                                <asp:ListItem>AM</asp:ListItem>
                                                <asp:ListItem>PM</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red;">*</span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%-- 16.10.15--%>
                                        <asp:Label ID="time" ForeColor="Red" runat="server"></asp:Label>
                                        <%--barath--%>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                OnClientClick="return valid()" OnClick="btn_update_Click" Visible="false" />
                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                OnClientClick="return valid()" OnClick="btn_delete_Click" Visible="false" />
                            <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2"
                                OnClientClick="return valid()" Visible="false" />
                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
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
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
