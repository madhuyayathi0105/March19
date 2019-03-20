<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="SupplementaryTerm.aspx.cs" Inherits="SupplementaryTerm" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Supplementary Form</title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script>
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }
    </script>
</head>
<body>
   <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager><br />
   <center>
        <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Supplementary Exam Result Analysis"></asp:Label></center>
      <br />   
              
            <center>
                <table style="width:800px; height:70px; background-color:#0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_college" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="College"></asp:Label>
                        </td>
                              <td>
                            <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Batch"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_batch_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                           
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Degree"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_degree_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_dept" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Department"></asp:Label>
                        </td>
                             <td>
                           <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_dept_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_monyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Exam Month and Year"></asp:Label>
                        </td>
                            <td>
                            <asp:UpdatePanel ID="UP_monyear" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_monyear" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_monyear" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_monyear" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_monyear_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:CheckBoxList ID="cbl_monyear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_monyear_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_monyear" runat="server" TargetControlID="txt_monyear"
                                        PopupControlID="panel_monyear" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                </center>
                <br />
                <div>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
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
                <br />
                <div>
                    <asp:Chart ID="Chart1" runat="server" Width="950px" Height="500px" Visible="false"
                        Font-Names="Book Antiqua" EnableViewState="true" Font-Size="Medium">
                        <Series>
                        </Series>
                        <Legends>
                            <asp:Legend Title="Subject Type" ShadowOffset="2" Font="Book Antiqua">
                            </asp:Legend>
                        </Legends>
                        <Titles>
                            <asp:Title Docking="Bottom" Text="Department">
                            </asp:Title>
                            <asp:Title Docking="Left" Text="Pass Percentage">
                            </asp:Title>
                        </Titles>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                <AxisY LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisY>
                                <AxisX LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false" onkeypress="display()"></asp:Label>
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1"></asp:TextBox>
                    <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                        OnClick="btn_excel_Click" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                        OnClick="btn_printmaster_Click" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
               
            </center>
       
</body>
</html>
</asp:Content>

