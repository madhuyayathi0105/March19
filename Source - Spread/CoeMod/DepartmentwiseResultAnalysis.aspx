<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="DepartmentwiseResultAnalysis.aspx.cs" Inherits="DepartmentwiseResultAnalysis" %>

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

            var value = document.getElementById("<%=dropreporttype.ClientID %>").value;

            if (value == "0") {
                var deg = document.getElementById('<%=txtdegree.ClientID %>').value;
                var bran = document.getElementById('<%=txtbranch.ClientID %>').value;
                var tstname = document.getElementById('<%=txttestname.ClientID %>').value;

                if (deg == "---Select---") {
                    alert("Please Select Degree");
                    return false;
                }
                else if (bran == "--Select--") {
                    alert("Please Select Branch");
                    return false;
                }
                else if (tstname == "--Select--") {
                    alert("Please Select Test Name");
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (value == "1") {
                var deg = document.getElementById('<%=txtdegree.ClientID %>').value;
                var bran = document.getElementById('<%=txtbranch.ClientID %>').value;
                if (deg == "---Select---") {
                    alert("Please Select Degree");
                    return false;
                }
                else if (bran == "--Select--") {
                    alert("Please Select Branch");
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    </script>
    
     <br />
        <div style="width: 960px; height: 26px;  
            padding-right: auto; text-align: right;">

            <center>
                
                <asp:Label ID="lbl" runat="server" Text="Departmentwise Result Analysis" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
               
               </center>
         
        </div>
         <br />
    
    <%--<div style="width: 940px; height: 64px; background-color: -webkit-border-radius: 10px;
        -moz-border-radius: 10px; padding: 10px;  padding-right: auto;
        background-color: #219DA5;">
        <center>--%>
        <center>
           <table style="width:700px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" 
                            Font-Bold="True" Font-Names="Book Antiqua" Style=" " Font-Size="Medium" Text="College" ></asp:Label>
                            </td>
                            <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddcollege" runat="server" Width="167px" Height="25px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style=" "
                                    Font-Size="Medium" OnSelectedIndexChanged="ddcollegeselect" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server"  Font-Bold="True"
                            Font-Names="Book Antiqua" Style=" "
                            Font-Size="Medium" Text="Batch"></asp:Label>
                             </td>
                            <td>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">

                            <ContentTemplate>
                                <asp:DropDownList ID="dropbatch" runat="server" Width="59px" Height="25px" Font-Bold="True"
                                    OnSelectedIndexChanged="dropbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                    Style=" " Font-Size="Medium" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Style=" "
                            Font-Size="Medium" Text="Degree" ></asp:Label>
                             </td>
                            <td>
                            <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="110px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="
                                   " Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="degreepanel" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" 

BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                    <asp:CheckBox ID="chckdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="checkDegree_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="chcklistdegree" runat="server" Font-Size="Medium" Font-Bold="True"
                                        OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged" Font-Names="Book Antiqua"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="degreepanel" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Style=""
                            Font-Size="Medium" Text="Branch" ></asp:Label>
                             </td>
                            <td>
                            <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="106px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="
                                    " Font-Size="Medium">---Select---</asp:TextBox>
                               <asp:Panel ID="branchpanel" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" 

BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                    <asp:CheckBox ID="chckbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="checkBranch_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="chcklistbranch" runat="server" Font-Size="Medium" Font-Bold="True"
                                        OnSelectedIndexChanged="cheklistBranch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="branchpanel" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Font-Color="white" 
                            Font-Bold="True" Font-Names="Book Antiqua" Style=" " Font-Size="Medium" Text="Semester" ></asp:Label>
                             </td>
                            <td>
                        <asp:DropDownList ID="dropsem" runat="server" Width="60px" Height="25px" Font-Bold="True"
                            Font-Names="Book Antiqua" Style=" "
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropsem_selectedIndex">
                        </asp:DropDownList>
                    </td>
                </tr>
            <%--</table>
            <div style="  margin-bottom: 0px;
                line-height: 27px;">
                <asp:Panel ID="Panel21" runat="server" Visible="true" Style="
                    ">
                    <table style=" height: 50px;
                        width: 600px; margin-bottom: 0px; line-height: 27px;">--%>
                        <tr>
                            <td>
                                <asp:Label ID="lblreporttype" runat="server" Width="100px"  Font-Bold="True"
                                    Font-Names="Book Antiqua" Style=" "
                                    Font-Size="Medium" Text="Report Type" ></asp:Label>
                                     </td>
                            <td>
                                <asp:DropDownList ID="dropreporttype" runat="server" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged"
                                    Width="88px" Height="25px" Font-Bold="True" Font-Names="Book Antiqua" Style="
                                    " Font-Size="Medium" AutoPostBack="true">
                                    <asp:ListItem Value="0" Selected="True">Internal</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="False">External</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                       <%-- </tr>
                    </table>
                </asp:Panel>--%>
                <asp:Panel ID="Panel22" runat="server" Visible="true" Style="">
                    <%--<table style="  height: 50px;
                        width: 600px; margin-bottom: 0px; line-height: 27px;">
                        <tr>--%>
                            <td>
                                <asp:Label ID="lbltestname" runat="server" Width="80px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style=" "
                                    Font-Size="Medium" Text="Test Name" ></asp:Label>
                                    </td>
                            <td>

                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txttestname" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="116px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="
                                            " Font-Size="Medium">---Select---</asp:TextBox>
                                     <asp:Panel ID="testnamepanel" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" 

BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                            <asp:CheckBox ID="chcktestname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="checktestname_CheckedChanged" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="chcklisttestname" runat="server" Font-Size="Medium" Font-Bold="True"
                                                OnSelectedIndexChanged="cheklisttestname_SelectedIndexChanged" Font-Names="Book Antiqua"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttestname"
                                            PopupControlID="testnamepanel" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            
                            <td>
                             <asp:Button ID="btngo" runat="server" Width="72px" Height="27px" Font-Bold="True"
                    Font-Names="Book Antiqua" Style=" "
                    Font-Size="Medium" OnClick="btngo_Click" 
                    OnClientClick="return validation()" Text="Go" />
                    </td>
                    </tr>
                        <%--</tr>
                    </table>--%>
                </asp:Panel>
            </div>
            <%--     <div id="suddiv1" style="margin-left: 0px; margin-top: 70px;  margin-bottom: 0px;
                line-height: 27px;">
                <asp:Panel ID="Panel2" runat="server" Visible="false" Style="margin-left: -6px; margin-top: -194px;
                    ">
                    <table style="margin-left: -1px; margin-top: -96px;  height: 50px;
                width: 600px; margin-bottom: 0px; line-height: 27px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblexammonth" runat="server"  Width="100px" Height="20px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style=" left: 2px; top: 261px;"
                                    Font-Size="Medium" Text="Exam Month" ></asp:Label>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="dropexammonth" runat="server" Width="149px" Height="25px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style=" left: 102px; top: 261px;"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblyear" runat="server" Font-Color="white" Width="100px" Height="20px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style=" left: 260px;
                                    top: 261px;" Font-Size="Medium" Text="Year" ></asp:Label>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style=" left: 334px; top: 261px;"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>--%>
            
               
            
        </center>
    
    <asp:Label ID="lblerror" runat="server" Style=" "
        Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Visible="true"
        Text="errorr" ForeColor="Red"></asp:Label>
    <asp:GridView ID="deptwiseresultanalysisgrid" Visible="false" runat="server" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
        HeaderStyle-BackColor="Teal" OnRowDataBound="GridView2_RowDataBound" OnDataBound="bound1"
        AlternatingRowStyle-CssClass="gvAltRow" HeaderStyle-CssClass="gvHeader" Style=" width: 817px;">
    </asp:GridView>
    <table>
        <tr>
            <td>
                <asp:Button ID="g2btnexcel" runat="server" OnClick="g2btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                    Style="" />
            </td>
            <td>
                <asp:Button ID="g2btnprint" runat="server" OnClick="g2btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="" />
            </td>
        </tr>
    </table>
    
    
    
    
    
    
    
    
    
    
    
    <div>
        <asp:GridView ID="deptwiseresultanalysisexternalgrid" Visible="false" runat="server"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" HorizontalAlign="Center"
            HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Teal" OnRowDataBound="GridView1_RowDataBound"
            OnDataBound="bound" AlternatingRowStyle-CssClass="gvAltRow" HeaderStyle-CssClass="gvHeader"
            Style=" width: 818px; ">
        </asp:GridView>
    </div>
    <table>
        <tr>
            <td>
                          
                <asp:Button ID="g1btnexcel" runat="server" OnClick="g1btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
        </tr>
    </table>
</asp:Content>

