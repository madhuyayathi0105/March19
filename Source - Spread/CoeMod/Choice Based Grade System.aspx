<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Choice Based Grade System.aspx.cs" Inherits="Choice_Based_Grade_System" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
  <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <asp:Label ID="Label4" runat="server" Text="Choice Based Grade System" Font-Bold="True"
                    ForeColor="Green" Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
            </center>
            <br />
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="85px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Width="90px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <%-- <td>
                                <asp:Label ID="lbledu" runat="server" Text="Edu Level" Font-Bold="true" Font-Names="Book Antiqua"
                                     Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddledu" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddledu_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>--%>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                            <asp:ListItem Text="Regular"></asp:ListItem>
                            <asp:ListItem Text="Arrear"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="150px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                            </asp:DropDownList>
                            <td>
                                <asp:Label ID="lblsubtye" runat="server" Text="Subject Type" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubtype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="150px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkviewgrade" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="View Grade" AutoPostBack="true" OnCheckedChanged="ddlsubject_SelectedIndexChanged"
                                    Width="125px" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="Btngo_Click" />
                            </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="250px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnresult" runat="server" Text="View Result" OnClick="btnresult_Click" />
                    </td>
                    <td>
                         <asp:DropDownList ID="ddlResultType" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="150px" AutoPostBack="false" CssClass="arrow" OnSelectedIndexChanged="ddlResultType_SelectedIndexChanged">
                            
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblerror" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
            <asp:Button ID="btncalculate" runat="server" CssClass="fontbold" Text="Calculate"
                OnClick="btncalculate_Click" />
            <asp:Button ID="btngenerate" runat="server" CssClass="fontbold" Text="Generate" OnClick="btngenerate_Click" />
            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="1200px" ActiveSheetViewIndex="0"
                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnmasterprint_Click" />
            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnmasterprint" />
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btngo" />
            <asp:PostBackTrigger ControlID="btncalculate" />
            <asp:PostBackTrigger ControlID="btngenerate" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
