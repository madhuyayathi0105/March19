<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Student_HT_Report.aspx.cs" Inherits="Student_HT_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        .style1
        {
            width: 80px;
        }
        .style2
        {
            width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 
    
    
    
    
    
    
    
    
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        
           <br />
         <center>
        <asp:Label ID="Label4" runat="server" Text="Student HT-Report" Font-Bold="True" ForeColor="Green"
            Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
        
        </center>
        <br />
        
    <center>
     <table style="width:700px; height:70px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                         Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <%-- <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>--%>
                <%-- <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Width="90px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>--%>
                <%--<td>
                        <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="230Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>--%>
                <td class="style1">
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td class="style2">
                    <div style=" Position:relative;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td class="style1">
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style2">
                    <div style=" position:relative;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                    Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" ForeColor="Black" Font-Size="Medium"
                                        AutoPostBack="True" Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td class="style1">
                    <asp:Label ID="lblbranch" runat="server" Text="Department" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style2">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                          <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                <asp:CheckBox ID="chkbranch" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstbranch" runat="server" ForeColor="Black" Font-Size="Medium"
                                    AutoPostBack="True" Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                </td>
            </tr>
        </table>
    </center>
    
    
    <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" ForeColor="Red"></asp:Label>
    <center>
        <asp:Label ID="lblnorec" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        <asp:Label ID="errmsg" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
    </center>
    
    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Height="250px" Width="400px"
        OnPreRender="FpSpread1_OnPreRender" OnCellClick="Cell1_Click"
        CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <center>
        <div id="div_Add" runat="server" visible="false" style="height: 500em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2);  absolute; top: 0;
            left: 0px;">
            
            <center>
                <div id="Div41" runat="server" class="sty2" style="background-color: White; height: auto;
                    width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                     absolute; top: 55px; left: 60px;">
                    <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; margin-top: -32px; margin-left: 430px;  " OnClick="ImageButton6_Click" />
                    
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Height="340px" Width="680"
                        OnButtonCommand="FpSpread2_ButtonCommand" CssClass="cursorptr" BorderColor="Black"
                        BorderWidth="0.5">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    
                    
                    <asp:TextBox ID="txtmsg" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" MaxLength="200" TextMode="MultiLine" Width="585px" Height="100px"
                        placeholder="Roll No=$ROLLNO$, Regno=$REGNO$, Name=$NAME$"></asp:TextBox>
                    
                    
                    <asp:Button ID="btn_sms" runat="server" Text="Send SMS" Visible="false" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_sms_Click" />
                    
                </div>
            </center>
        </div>
    </center>
    <%-- <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Save" />--%>
    <div id="exceldiv" runat="server" visible="false">
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
            InvalidChars="/\">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
    </div>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2);  absolute; top: 0;
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
                                        <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

