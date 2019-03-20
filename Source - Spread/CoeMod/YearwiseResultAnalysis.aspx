<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="YearwiseResultAnalysis.aspx.cs" Inherits="YearwiseResultAnalysis" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Year wise Result Analysis</title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script>
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }
    </script>
</head>
<body>
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <br /><center>
         <asp:Label ID="Label1" runat="server" Text="Year wise Result Analysis" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Large" ForeColor="Green"></asp:Label></center>
        <br />
            <center>
                <table style="width:900px; height:70px; background-color:#0CA6CA;">
                    <tr>
                        <td>
                        <asp:Label ID="Label2" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                              <td>
                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_batch_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                           
                        </td>
                        <td>
                             <asp:Label ID="Label4" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_degree_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                           
                        </td>
                        <td>
                             <asp:Label ID="Label5" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox  ddlheight3 multxtpanleheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_dept_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Label ID="Label6" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:UpdatePanel ID="UP_sem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_sem_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_sem" runat="server" TargetControlID="txt_sem" PopupControlID="panel_sem"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                             <asp:Label ID="Label7" runat="server" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:UpdatePanel ID="UP_sec" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_sec" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sec" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_sec_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_sec" runat="server" TargetControlID="txt_sec" PopupControlID="panel_sec"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                             <asp:Label ID="Label8" runat="server" Text="Test Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:UpdatePanel ID="UP_test" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_test" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_test_Change"
                                        CssClass="textbox  ddlheight3 multxtpanleheight" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddl_test" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <%-- <asp:UpdatePanel ID="UP_test" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_test" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_test" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_test" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_test_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_test" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_test_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_test" runat="server" TargetControlID="txt_test"
                                        PopupControlID="panel_test" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Go" OnClick="btn_go_Click"/>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="text-align: left; text-indent: 50px;">
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </div>
                        <div id="divspread" runat="server" style="width: 950px; height: 390px; overflow: auto;"
                            class="table">
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                Width="950px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div id="rptprint" runat="server" visible="false" style="font-weight: bold;">
                            <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false" onkeypress="display()"></asp:Label>
                            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="font-family: Book Antiqua;
                                font-size: medium;"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                                OnClick="btn_excel_Click" Style="font-weight: bold; font-family: Book Antiqua;
                                font-size: medium;" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                                OnClick="btn_printmaster_Click" Width="60px" Style="font-weight: bold; font-family: Book Antiqua;
                                font-size: medium;" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_excel" />
                    </Triggers>
                </asp:UpdatePanel>
                </br>
            </center>
       
    </form>
</body>
</html>
</asp:Content>

