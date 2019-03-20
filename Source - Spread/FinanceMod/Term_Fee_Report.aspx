<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Term_Fee_Report.aspx.cs" Inherits="Term_Fee_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--MasterPageFile="~/Site.master"--%>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <%--<asp:content id="Content1" contentplaceholderid="HeadContent" runat="Server">--%>
        <script type="text/javascript">
            function validation() {
                var error = "";
                var semester = document.getElementById('<%=txtsem.ClientID %>').value;

                if (semester == "---Select---" || semester == "--Select--") {
                    error += "Please Select Fee Category \n";
                }
                if (error.trim() == "") {
                    return true;
                }
                else {
                    alert(error);
                    return false;
                }
            }
            function hide() {
                document.getElementById('<%=lblmsg.ClientID %>').style.display = 'none';
            }

            function columnOrderCbl() {
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                if (cball.checked == true) {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = false;
                    }
                }
            }
            function columnOrderCb() {
                var count = 0;
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                for (var i = 0; i < tagname.length; i++) {
                    if (tagname[i].checked == true) {
                        count += 1;
                    }
                }
                if (tagname.length == count) {
                    cball.checked = true;
                }
                else {
                    cball.checked = false;
                }
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=btnAdd.ClientID %>').click(function () {
                    $('#<%=divaddtype.ClientID %>').show();
                    $('#<%=txtdesc.ClientID %>').val('');
                    return false;
                });
                $('#<%=btnDel.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                    if (rptText.trim() != null && rptText != "Select") {
                        var msg = confirm("Are you sure you want to delete this report type?");
                        if (msg)
                            return true;
                        else
                            return false;
                    }
                    else {
                        alert("Please select any one report type!");
                        return false;
                    }
                });

                $('#<%=btnexittype.ClientID %>').click(function () {
                    $('#<%=divaddtype.ClientID %>').hide();
                    return false;
                });

                $('#<%=btnaddtype.ClientID %>').click(function () {
                    var txtval = $('#<%=txtdesc.ClientID %>').val();
                    if (txtval == null || txtval == "") {
                        alert("Please enter the report type!");
                        return false;
                    }
                });
                $('#<%=btnclear.ClientID %>').click(function () {
                    $("[id*=cb_column]").removeAttr('checked');
                    $("[id*=cblcolumnorder]").removeAttr('checked');
                    return false;
                });

                $('#<%=imgcolumn.ClientID %>').click(function () {
                    $('#<%=divcolorder.ClientID %>').hide();
                    return false;
                });
                $('#<%=btngo.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlMainreport.ClientID %>').find('option:selected').text();
                    if (rptText.trim() == null || rptText == "Select") {
                        alert("Please select any one report type!");
                        return false;
                    }
                });

                $('#<%=btncolorderOK.ClientID %>').click(function () {
                    var count = 0;
                    var cball = document.getElementById('<%=cb_column.ClientID%>');
                    var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                    var tagname = cblall.getElementsByTagName("input");
                    for (var i = 0; i < tagname.length; i++) {
                        if (tagname[i].checked == true) {
                            count += 1;
                        }
                    }
                    if (count == 0) {
                        alert("Please Select column order Type!");
                        return false;
                    }
                });
            });
        </script>
        <%--</asp:content>--%>
        <%--<asp:content id="Content2" contentplaceholderid="MainContent" runat="Server">--%>
        <div>
            <asp:ScriptManager ID="Script1" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Term Fee Report</span></div>
                    </center>
                </div>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <div>
                        <center>
                            <table id="Table1" class="maintablestyle" runat="server">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" CssClass="textbox1 ddlheight3"
                                            OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstr" runat="server" Text="stream"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                            CssClass="textbox  ddlheight" Style="width: 90px;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" CssClass="textbox1 ddlheight1"
                                            Style="width: 80px;" OnSelectedIndexChanged="ddlbatch_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" CssClass="textbox1 ddlheight1"
                                            Style="width: 90px;" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddldept_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtsem" runat="server" CssClass="textbox textbox1" Width="123px"
                                                    ReadOnly="true" Text="--Select--"></asp:TextBox>
                                                <asp:Panel ID="pnlsem" runat="server" CssClass="multxtpanel" Style="height: auto;
                                                    width: 150px;">
                                                    <asp:CheckBox ID="cbsem" runat="server" Text="SelectAll" AutoPostBack="True" OnCheckedChanged="cbsem_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblsem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <br />
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsem"
                                                    PopupControlID="pnlsem" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="12">
                                        <table>
                                            <tr>
                                                <td>
                                                    Finance Year
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfyear" CssClass="textbox textbox1" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel">
                                                        <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                            AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                            AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtfyear"
                                                        PopupControlID="Pfyear" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </td>
                                                <td>
                                                    Header
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtheader" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                                Text="--Select--"></asp:TextBox>
                                                            <asp:Panel ID="Pheader" runat="server" CssClass="multxtpanel" Style="height: auto;">
                                                                <asp:CheckBox ID="chkheader" runat="server" Text="SelectAll" AutoPostBack="True"
                                                                    OnCheckedChanged="chkheader_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chklsheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklsheader_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <br />
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtheader"
                                                                PopupControlID="Pheader" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_detre" runat="server" Text="Deduction Reason" CssClass="textbox"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtdeduct" Font-Size="Medium" Font-Names="Book Antiqua" Style="height: 20px;
                                                                width: 164px;" CssClass="Dropdown_Txt_Box" runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="height: auto;"
                                                                Width="270px">
                                                                <asp:CheckBox ID="cbdeduct" runat="server" Text="Select All" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    OnCheckedChanged="cbdeduct_OnCheckedChanged" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cbldeduct" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    OnSelectedIndexChanged="cbldeduct_OnSelectedIndexChanged" AutoPostBack="True">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeduct"
                                                                PopupControlID="Panel1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Community" runat="server" Text="Community"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_community" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panelcomm" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_community" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_community_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_community" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_community"
                                                                PopupControlID="panelcomm" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="checkdicon" runat="server" Text="Include Student Catagory" AutoPostBack="true"
                                                        OnCheckedChanged="checkdicon_Changed" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Style="width: 200px;" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtinclude" Enabled="false" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                                ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Width="172px" Style="height: auto;">
                                                                <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                    OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                                PopupControlID="pnlinclude" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="3">
                                                    <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>
                                                    <%-- </td>
                        <td>--%>
                                                    <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                        Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                                        OnClick="btngo_OnClick" OnClientClick=" return validation()" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbAcdYear" runat="server" Text="" AutoPostBack="true" />
                                        <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="102px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblTypeNew" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Academic Year" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Odd"></asp:ListItem>
                                            <asp:ListItem Text="Even"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <%--  <div id="div1" runat="server" visible="false" style="width: 850px; overflow: auto;">--%>
                        <asp:GridView ID="grdTermFeeReport" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="grdTermFeeReport_RowDataBound">
                            <%----%>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                        <%--  </div>--%>
                    </center>
                    <br />
                    <asp:Label ID="lblmsg" runat="server" Visible="false" Text="No Records Found" ForeColor="Red"
                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblreptname" runat="server" Text="Report Name" font-name="Book Antiqua"
                                        Visible="false" Font-Size="Medium" Font-Bold="true" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtreptname" runat="server" Font-Bold="True" Visible="false" CssClass="textbox textbox1"
                                        Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="return hide(this)" Width="130px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="Excel" runat="server" Text="Export Excel" CssClass="textbox textbox1 btn3"
                                        Visible="false" Font-Size="Medium" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="Excel_OnClick" />
                                </td>
                                <td>
                                    <asp:Button ID="Print" runat="server" Text="Print" CssClass="textbox textbox1 btn3"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="false"
                                        OnClick="btnprintmaster_Click" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <%--column order--%>
        <center>
            <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 304px;" />
                <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 300px;
                        width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                        border-radius: 10px;">
                        <center>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                            font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrptype" Text="Report Type" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="textbox textbox1 btn1" /><%--OnClick="btnAdd_OnClick"--%>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                            CssClass="textbox textbox1 ddlheight4">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDel" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnDel_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 600px;"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="600px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                            RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <center>
                                            <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                            <%--   </center>
                                </td>
                                <td>
                                    <center>--%>
                                            <asp:Button ID="btnclear" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="Clear" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
                <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            </div>
        </center>
        <%--report type name enter text box--%>
        <div id="divaddtype" runat="server" style="height: 100%; z-index: 10000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
            display: none;">
            <center>
                <div id="panel_description11" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddtype_Click" />
                                <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" /><%--OnClick="btnexittype_Click"--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <%--</asp:content> --%>
</asp:Content>
