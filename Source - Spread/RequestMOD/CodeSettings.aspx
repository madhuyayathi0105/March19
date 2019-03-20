<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CodeSettings.aspx.cs" Inherits="Gatepass_CodeSettings" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .container
            {
                width: 100%;
            }
            .col1
            {
                float: left;
                width: 50%;
            }
            .col2
            {
                float: right;
                width: 50%;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <script type="text/javascript">
            function checkvalue() {
                var fl = 0; var f2 = 0;
                var id = document.getElementById("<%=ug_grid.ClientID %>");
                var gridViewControls = id.getElementsByTagName("input");
                var len = id.rows.length;

                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_acronym") > 1) {

                        var m = gridViewControls[i].value;
                        document.getElementById("<%=hid.ClientID %>").value = m;

                        var ms = document.getElementById("<%=hid.ClientID %>").value;
                        if (gridViewControls[i].value == "") {
                            fl = 1;
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_startno") > 1) {
                        var s = gridViewControls[i].value;
                        document.getElementById("<%=hid1.ClientID %>").value = s;
                        var ms1 = document.getElementById("<%=hid1.ClientID %>").value
                        if (gridViewControls[i].value == "") {
                            fl = 1;
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_size") > 1) {
                        var d = gridViewControls[i].value;
                        document.getElementById("<%=hid2.ClientID %>").value = d;
                        var ms2 = document.getElementById("<%=hid2.ClientID %>").value
                        if (gridViewControls[i].value == "") {
                            fl = 1;
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_size") > 6) {
                        var n = gridViewControls[i].value;
                        document.getElementById("<%=hid3.ClientID %>").value = n;
                        var ms3 = document.getElementById("<%=hid3.ClientID %>").value
                        if (gridViewControls[i].value > 6) {
                            f2 = 1;
                        }
                    }
                }
                if (f2 == 1) {
                    alert('Please Enter Size Less than equal to 6');

                    return false;
                } else if (fl == 1) {
                    alert('Please Fill All Values');

                    return false;
                }
                else {

                    return true;
                }
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";

            }
        </script>
        <script type="text/javascript">
            function checkvalue1() {
                var f3 = 0; var f4 = 0;
                var id1 = document.getElementById1("<%=grid_prev.ClientID %>");
                var gridViewControls1 = id.getElementsByTagName("input1");
                var len1 = id1.rows.length;
                for (var u = 0; u < gridViewControls1.length; u++) {
                    if (gridViewControls1[u].name.indexOf("txt_acronym1") > 1) {

                        var a = gridViewControls1[u].Text;
                        document.getElementById1("<%=hi.ClientID %>").value = a;
                        var m1 = document.getElementById("<%=hi.ClientID %>").value;
                        if (gridViewControls[u].value == "") {
                            f3 = 1;
                        }
                    }
                    if (gridViewControls1[u].name.indexOf("txt_startno1") > 1) {
                        var b = gridViewControls1[u].value;
                        document.getElementById1("<%=hi1.ClientID %>").value = b;
                        var m2 = document.getElementById("<%=hi1.ClientID %>").value
                        if (gridViewControls1[u].value == "") {
                            f3 = 1;
                        }
                    }
                    if (gridViewControls1[u].name.indexOf("txt_size1") > 1) {
                        var c = gridViewControls1[u].value;
                        document.getElementById1("<%=hi2.ClientID %>").value = c;
                        var m3 = document.getElementById("<%=hi2.ClientID %>").value
                        if (gridViewControls1[u].value == "") {
                            f3 = 1;
                        }
                    }

                }

            }
            function myFunction(x1) {
                x1.style.borderColor = "#c4c4c4";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Code Master</span></div>
        </center>
        <br />
        <div>
            <div class="maindivstyle" style="height: 275px; width: 1000px;">
                <center>
                    <br />
                    <table class="maintablestyle" style="width: 980px">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="College Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collegename" runat="server" Width="243px" CssClass="textbox1 ddlstyle ddlheight6"
                                    OnSelectedIndexChanged="ddl_collegeSelectedindexchange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_frmdate" Text="From Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_frmdate" runat="server"  AutoPostBack="true" CssClass="textbox textbox1" Width="80px" OnTextChanged="txt_frmdate_OnTextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calfrmdate" TargetControlID="txt_frmdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_prvdate" Text="Previous Date" runat="server"></asp:Label>
                            </td>
                            <td rowspan="2">
                                <asp:TextBox ID="txt_prvdate" runat="server" CssClass="textbox textbox1" Width="100px"
                                    Enabled="false" Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_prvdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddl_PrevDate" runat="server" CssClass=" textbox1  ddlheight2"
                                    OnSelectedIndexChanged="ddl_PrevDate_OnSelectedIndexChange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="txtdateerr" runat="server" Style="color: Red;" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <div>
                    <center>
                        <table class="table">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_count1" runat="server" Text="Gatepass No:" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </td>
                                <td>
                                    <fieldset style="border-radius: 10px;">
                                        <legend>New Setting</legend>
                                        <div>
                                            <asp:GridView ID="ug_grid" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_acronym" runat="server"  onfocus="return myFunction2(this)" 
                                                                MaxLength="35" ></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextenderacr" runat="server" TargetControlID="txt_acronym"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_startno" runat="server"  onfocus="return myFunction2(this)"
                                                                MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_startno"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_size" runat="server" Width="30px"
                                                                MaxLength="1" onfocus="return myFunction2(this)" onchange="myFunction(this)"></asp:TextBox>
                                                            <asp:RangeValidator ID="Range1" ControlToValidate="txt_size" MinimumValue="1" MaximumValue="6"
                                                                Type="Integer" runat="server" ToolTip="Enter Only 1 - 6" SetFocusOnError="False" />
                                                            <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_size"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="border-radius: 10px;">
                                        <legend>Current Setting</legend>
                                        <div>
                                            <asp:GridView ID="grid_prev" runat="server" AutoGenerateColumns="false" GridLines="None"
                                                OnRowDataBound="grid_prev_Bound0" OnDataBound="OnDataBound0">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Acronym" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_acronym1" runat="server"  Enabled="false"
                                                                onfocus="return myFunction2(this)" MaxLength="35" ></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextenderacr" runat="server" TargetControlID="txt_acronym1"
                                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start No" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_startno1" runat="server"  Enabled="false"
                                                                onfocus="return myFunction2(this)" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_startno1"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Size" HeaderStyle-BackColor="#0CA6CA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_size1" runat="server"  Enabled="false"
                                                                Width="30px" MaxLength="1" onfocus="return myFunction2(this)" onchange="myFunction(this)"></asp:TextBox>
                                                            <asp:RangeValidator ID="Range1" ControlToValidate="txt_size1" MinimumValue="1" MaximumValue="6"
                                                                Type="Integer" runat="server" ToolTip="Enter Only 1 - 6" SetFocusOnError="False" />
                                                            <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_size1"
                                                                FilterType="Numbers" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <input type="hidden" runat="server" id="hid" />
                <input type="hidden" runat="server" id="hid1" />
                <input type="hidden" runat="server" id="hid2" />
                <input type="hidden" runat="server" id="hid3" />
                <input type="hidden" runat="server" id="hi" />
                <input type="hidden" runat="server" id="hi1" />
                <input type="hidden" runat="server" id="hi2" />
                <center>
                    <div class="container">
                        <div class="col1" style="margin-left: 350px;">
                            <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btn_save_Click"
                                OnClientClick="return checkvalue()" />
                        </div>
                        <%--OnClick="btn_save_Click"--%>
                    </div>
                </center>
                <br />
                <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
                    <center>
                        <div id="panel_erralert" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
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
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
