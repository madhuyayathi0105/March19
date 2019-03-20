<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="CumulativeReport.aspx.cs" Inherits="CumulativeReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Cumulative Report</title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function semesterTest() {
            var empty = "";
            var dep;
            var dep1;
            var semfrom = document.getElementById("<%=txt_semfrom.ClientID %>").value;
            var semto = document.getElementById("<%=txt_semto.ClientID %>").value;

            if (semfrom.trim() == "") {
                dep = document.getElementById("<%=txt_semfrom.ClientID %>");
                dep.style.borderColor = 'Red';
                empty = "E";

            }
            if (semto.trim() == "") {
                dep1 = document.getElementById("<%=txt_semto.ClientID %>");
                dep1.style.borderColor = "red";
                empty = "E";
            }

            if (semfrom.trim() != "" && semto.trim() != "") {

                if (parseInt(semfrom) > parseInt(semto)) {
                    alert("From semester shoud be less than To semester");
                    empty = "E";
                }
                if (parseInt(semfrom) > 10) {
                    alert("From semester shoud not exceed 10");
                    empty = "E";
                }
                if (parseInt(semto) > 10) {
                    alert("To semester shoud not exceed 10");
                    empty = "E";
                }
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function returnColor(y) {
            y.style.borderColor = "#c4c4c4";
        }
    </script>
</head>
<body>
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> <br />
     <center>
          <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Cumulative Report"></asp:Label></center>
<br />
        <center>
            <table style="width:700px; height:70px; background-color:#0CA6CA;">
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
                    <td>
                        <asp:Label ID="lbl_dept" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_dept_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:DropDownList>
                       
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_subtype" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Subject Type" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_subtype" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_subtype" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_subtype" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_subtype" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb__subtype_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_subtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subtype_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_subtype" runat="server" TargetControlID="txt_subtype"
                                    PopupControlID="panel_subtype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="display: none;">
                        <asp:Label ID="lbl_subname" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Subject Name" Visible="false" ></asp:Label>
                    </td>
                    <td style="display: none;">
                        <asp:UpdatePanel ID="UP_subname" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_subname" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua" Visible="false">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_subname" runat="server" CssClass="multxtpanel" Visible="false">
                                    <asp:CheckBox ID="cb_subname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_subname_OnCheckedChanged"  Font-Bold="True" Font-Names="Book Antiqua"/>
                                    <asp:CheckBoxList ID="cbl_subname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subname_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_subname" runat="server" TargetControlID="txt_subname"
                                    PopupControlID="panel_subname" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_semfrom" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Semester From" Width="125px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_semfrom" runat="server" CssClass="textbox txtheight" MaxLength="2"
                            onfocus="return returnColor(this)" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filt_extenderfrom" runat="server" TargetControlID="txt_semfrom"
                            FilterType="custom,Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbl_semto" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="To"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_semto" runat="server" CssClass="textbox txtheight" MaxLength="2"
                            onfocus="return returnColor(this)" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filt_extenderto" runat="server" TargetControlID="txt_semto"
                            FilterType="custom,Numbers">
                        </asp:FilteredTextBoxExtender>
                        </td>
                    <td>
                        <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1"  Font-Bold="True" Font-Names="Book Antiqua" Text="Go" OnClientClick="return semesterTest()"
                            OnClick="btn_go_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div>
                <asp:Chart ID="Chart1" runat="server" Width="970px" Visible="false" Font-Names="Book Antiqua"
                    EnableViewState="true" Font-Size="Medium">
                    <Series>
                    </Series>
                    <Legends>
                        <asp:Legend Title="Subject Type" ShadowOffset="2" Font="Book Antiqua">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Docking="Bottom" Text="Semester">
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
        </center>
    </div>
    </form>
</body>
</html>
</asp:Content>

