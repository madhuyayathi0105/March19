<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="StudentwiseArrearStatus.aspx.cs" Inherits="StudentwiseArrearStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
        }
        
        .gvRow td
        {
            background-color: #F0FFFF;
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
        }
        
        .gvAltRow td
        {
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
    </style>
    <script type="text/javascript">
        function validation() {
            var value = document.getElementById("<%=dd_subtype.ClientID %>").value;

            if (value == "0") {
                var subname = document.getElementById('<%=txt_subname.ClientID %>').value;
                if (subname == "---Select---") {
                    alert("Please Select Subject Name");
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (value == "1") {
                var subname = document.getElementById('<%=txt_subname.ClientID %>').value;
                if (subname == "---Select---") {
                    alert("Please Select Subject Name");
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    </script>
    <br />
    <center>
            <asp:Label ID="lbl" runat="server" Text="Subjectwise Arrear Status" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
          </center>
            <br />
   <center>
   <table style="width:750px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblexm_month" runat="server" Font-Color="white" Width="100px" Height="20px"
                            Font-Bold="True" Font-Names="Book Antiqua" Style="position" Font-Size="Medium" Text="Exam Month" ></asp:Label>
                            
 </td>
                            <td>
                        <asp:DropDownList ID="ddexm_month" runat="server" Width="107px" Height="25px" Font-Bold="True"
                            Font-Names="Book Antiqua" Style=""
                            Font-Size="Medium" OnSelectedIndexChanged="dd_exmmonth_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_year" runat="server" Width="45px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style=""
                                Font-Size="Medium" Text="Year" ></asp:Label>
                                
 </td>
                            <td>
                            <asp:DropDownList ID="dd_year" runat="server" Width="67px" Height="25px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style=""
                                Font-Size="Medium" OnSelectedIndexChanged="dd_year_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsubtype" runat="server" Width="100px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style=""
                                Font-Size="Medium" Text="Subject Type" ></asp:Label>
                                
 </td>
                            <td>
                            <asp:DropDownList ID="dd_subtype" runat="server" OnSelectedIndexChanged="chklstselc_subtype_SelectedIndexChanged"
                                Width="92px" Height="25px" Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium" AutoPostBack="true">
                                <asp:ListItem Value="0" Selected="True">Common</asp:ListItem>
                                <asp:ListItem Value="1" Selected="False">General</asp:ListItem>
                            </asp:DropDownList>
                        </td>
               
                            <td>
                                <asp:Label ID="lblsubname" runat="server" Width="116px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style=""
                                    Font-Size="Medium" Text="Subject Name" ></asp:Label>
                                    
 </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_subname" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="151px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="p_subname" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" 

BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                            <asp:CheckBox ID="chck_subname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chcksubname_CheckedChanged" />
                                            <asp:CheckBoxList ID="chcklist_subname" runat="server" Font-Size="Medium" Font-Bold="True"
                                                Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklistsubname_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_subname"
                                            PopupControlID="p_subname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        
                       </tr>
                       <tr>
   <td> 
                                <asp:Label ID="lbldegree" runat="server" Font-Color="white" Visible="false" Width="100px"
                                    Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="Degree" ></asp:Label>
                                    
 </td>
                            <td>
                                <asp:DropDownList ID="dd_degree" runat="server" Width="76px" Visible="false" Height="25px"
                                    AutoPostBack="true" OnSelectedIndexChanged="dropdegree_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Font-Color="white" Visible="false"  Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="Branch" ></asp:Label>
                                
 </td>
                                <td>
                                <asp:DropDownList ID="dd_dept" runat="server" Visible="false" Width="221px" Height="25px"
                                    AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="dd_dept_SelectedIndexChanged"
                                    Style="" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                              <asp:Button ID="btngo" runat="server" Width="67px" Height="27px" Font-Bold="True"
                    Font-Names="Book Antiqua" Style=""
                    Font-Size="Medium"  OnClick="btngo_Click"
                    OnClientClick="return validation()" Text="Go" />
                            </td>
                            </tr>
                      </table>
                    </center>
              <div>
       <br />
    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="position: absolute;
        left: 15px; top: 277px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" ForeColor="#FF3300"></asp:Label>
    <center>
        <div>
            <br />
            <asp:GridView ID="grid1common" Visible="false" runat="server" OnRowDataBound="GridView1_RowDataBound"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" HorizontalAlign="Center"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Teal" AlternatingRowStyle-CssClass="gvAltRow"
                HeaderStyle-CssClass="gvHeader" Style="border-collapse: collapse; font-family: Book Antiqua;
                font-size: medium; font-weight: bold; margin-left: 203px; margin-top: 6px; width: 530px;">
            </asp:GridView>
        </div>
    </center>
    <table>
        <tr>
            <td>
                           
                           
                      
                <asp:Button ID="g1btnexcel" runat="server" OnClick="btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
        </tr>
    </table>
    <center>
        <asp:GridView ID="grid2general" Visible="false" runat="server" OnRowDataBound="GridView2_RowDataBound"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" HorizontalAlign="Center"
            HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Teal" AlternatingRowStyle-CssClass="gvAltRow"
            HeaderStyle-CssClass="gvHeader" Style="border-collapse: collapse; font-family: Book Antiqua;
            font-size: medium; font-weight: bold; margin-left: 203px; margin-top: -15px;
            width: 530px;">
        </asp:GridView>
    </center>
    <table>
        <tr>
            <td>
                           
                           
                      
                <asp:Button ID="g2btnexcel" runat="server" OnClick="btnexcel1_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <asp:Button ID="g2btnprint" runat="server" OnClick="g1btnprint1_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
        </tr>
    </table>
</asp:Content>

