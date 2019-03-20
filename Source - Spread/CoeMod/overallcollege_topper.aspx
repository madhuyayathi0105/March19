<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="overallcollege_topper.aspx.cs" Inherits="overallcollege_topper" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<html>
    <head>
        <style>
            .fontmedium
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
            .style1
            {
                width: 152px;
            }
            .style2
            {
                width: 100px;
            }
            .style4
            {
                width: 25px;
            }
            .style6
            {
                width: 133px;
            }
            .ModalPopupBG
            {
                background-color: #666699;
                filter: alpha(opacity=50);
                opacity: 0.7;
            }
            .HellowWorldPopup
            {
                min-width: 600px;
                min-height: 400px;
                background: white;
            }
            
            .modalPopup
            {
                background-color: #ffffdd;
                border-width: 1px;
                -moz-border-radius: 5px;
                border-style: solid;
                border-color: Gray;
                top: 50px;
                left: 150px;
            }
        </style>
        <script type="text/javascript">

            function checkvalidate() {
                var checkvalidation = document.getElementById('<%=txtbatch.ClientID%>').value;
                var checkvalidation1 = document.getElementById('<%=txtdegree.ClientID%>').value;
                var checkvalidation2 = document.getElementById('<%=txtbranch.ClientID%>').value;
                var rangefromcgpa = document.getElementById('<%=txt_rangefrom.ClientID%>').value;
                var rangetocgpa = document.getElementById('<%=txt_to.ClientID%>').value;

                if (checkvalidation == "---Select---") {
                    alert("Please Select Batch");
                    return false;
                }
                else if (checkvalidation1 == "---Select---") {

                    alert("Please Select Degree");
                    return false;
                }
                else if (checkvalidation2 == "---Select---") {

                    alert("Please Select Branch");
                    return false;
                }

                else if (rangefromcgpa == "") {
                    alert("Please Enter CGPA Range From Value");
                    return false;
                }

                else if (rangetocgpa == "") {
                    alert("Please Enter CGPA To Value");
                    return false;
                }
                else {
                    return true;
                }
            }

            function display() {
                document.getElementById('MainContent_lbl_reptnoname').innerHTML = "";
            }
        </script>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br /><center>
            <asp:Label ID="lbl_heading" runat="server" Font-Size="Large" ForeColor="Green"
                Text="Overall College Topper List "  Font-Bold="true" font-name="Book Antiqua" ></asp:Label></center>
        <br />
        <center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;"
>
                <tr>
                    <td>
                        <asp:Label ID="lblcoll" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="true" Width="240px" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px " OnSelectedIndexChanged="ddlclg_click">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                            
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                    <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style=" height: 20px; width: 125px;">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" Width="125px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                    <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbatch_ChekedChange" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel></div>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                           
                            Width="80px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                    <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" Style=" font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" ReadOnly="true" Width="120px">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Width="150px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                    <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel></div>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                           
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                    <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="120px" Style="
                                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" Width="400px" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel></div>
                    </td>
                </tr>
           
                <tr>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Text="Exam Year" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="61px" AutoPostBack="True" OnSelectedIndexChanged="ddlyear_onselected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblmonth" runat="server" Text="Exam Month" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="61px" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_onselected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_rangefrom" runat="server" Text="CGPA Range From" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="160px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_rangefrom" Font-Bold="true" runat="server" Width="50px" Font-Names="Book Antiqua"
                            Font-Size="Medium" MaxLength="2" onkeyup="checkDec(this);"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FTX2" runat="server" TargetControlID="txt_rangefrom"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbl_rangeto" runat="server" Text=" To " Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_to" Font-Bold="true" runat="server" Width="50px" Font-Names="Book Antiqua"
                            Font-Size="Medium" MaxLength="2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftx1" runat="server" TargetControlID="txt_to" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Width="50px" Height="28px" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_go" OnClientClick="return checkvalidate()" />
                    </td>
                </tr>
            </table>
            </center>
            <br />
           
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                Visible="false" BorderWidth="1px">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <asp:Label ID="lbl_errmsg" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
                Style="top: 316px; left: 48px; position: absolute;" Font-Size="Medium" ForeColor="Red"></asp:Label>
            <asp:Label ID="lbl_reptnoname" runat="server" Font-Bold="True" Visible="false" ForeColor="Red"
                Font-Size="Medium" Width="375px" Font-Names="Book Antiqua" Text=""></asp:Label>
            <br />
            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Font-Size="Medium"
                Visible="false" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
            <asp:TextBox ID="txt_rpt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Visible="false" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
            <asp:Button ID="btn_excel" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Visible="false" Font-Size="Medium" runat="server" OnClick="btn_excelname" />
            <asp:Button ID="btn_print" Text="Print" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Visible="false" Font-Size="Medium" OnClick="btn_printcmn" />
            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </body>
    </html>
</asp:Content>

