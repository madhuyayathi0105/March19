<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="reportcard_activitysettings.aspx.cs" Inherits="reportcard_activitysettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="24px"
                Style="  width: 100%">
                <center>
                    <asp:Label ID="Label1" runat="server" Text="Report Card - Activity Settings" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Large" ForeColor="White"></asp:Label>
                </center>
              <%--  <div style="margin-top: -20px;  right: 20px;">
                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White" PostBackUrl="~/Default2.aspx">Back</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White">Logout</asp:LinkButton>
                </div>--%>
                
            </asp:Panel>
            
            
            
            
            
            
            
            
            
            
            
            <div style="height: 60px; background-color: LightBlue; border-color: Black; border-style: solid;
                border-width: 1px; width: 938px;">
                <table style="">
                    <tr>
                        <%-- <td>
                            <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" ForeColor="Black"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="182px">
                            </asp:DropDownList>
                        </td>--%>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="20px" Width="59px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="100px" ></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="74px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" 
                                Width="276px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 25px; margin-top:-20px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSemYr" runat="server" Text="Term" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 20px; width: 33px; "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Visible="true" Font-Size="Medium"
                                Style="height: 21px; width: 44px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 25px;
                                width: 40px" />
                            <%-- <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>--%>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSec" runat="server" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Visible="false" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="height: 21px; width: 47px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbClass" runat="server" OnCheckedChanged="cbClass_CheckedChanged"
                                Text="Class (For Anglo Indian)" Font-Names="Book Antiqua" AutoPostBack="true"  width="200px"/>
                        </td>
                    </tr>
                </table>
                <div style="width: 950px;">
                    <table id="parttable" runat="server" style="">
                        <tr>
                            <td>
                                <asp:Label ID="lbltitlepart" runat="server" CssClass="fontcomman"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltolparts" runat="server" AutoPostBack="true" Width="90px"
                                    OnSelectedIndexChanged="ddltolparts_SelectedIndexChanged" Style=""
                                    CssClass="fontcomman">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubtt" Style="" Text="Sub Title" runat="server"
                                    CssClass="fontcomman"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubtt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsubtt_SelectedIndexChanged"
                                    Style=" width: 52px;" CssClass="fontcomman">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlformate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlformate_SelectedIndexChanged">
                                    <asp:ListItem>Activity Settings</asp:ListItem>
                                    <asp:ListItem>Activity Grade Settings</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlactivity" runat="server" Width="250px" Style=""
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged"
                                    CssClass="fontcomman">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btngofp" runat="server" Text="Go" OnClick="btngofp_Click" Font-Names="Book Antiqua"
                                    ForeColor="Black" Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="21px"
                Style=" width: 100%;">
            </asp:Panel>
            <asp:Label ID="lblparterr" Style="  color: Red;
                " runat="server" Text="" CssClass="fontcomman"></asp:Label>
            <asp:Label ID="lblErrorMsg" runat="server" Text="" Style="font-family: Book Antiqua;
                color: Red; font-size: medium; font-weight: bold; height: 20px; 
                 width: 221px;"></asp:Label>
            
            
            <div id="dfp1">
                <table style="text-align: center; width: 750px;">
                    <tr>
                        <td>
                            <%--<span style="color: Teal" class="fontcomman">Student Name :</span>--%>
                            <asp:Label ID="lblselectedstudentname" Text="" Visible="false" Style="color: Teal"
                                runat="server" CssClass="fontcomman"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--  <span style="color: Teal" class="fontcomman">Roll No :</span>--%>
                            <asp:Label ID="lblselectedstudentrollno" Text="" Visible="false" Style="color: Teal"
                                runat="server" CssClass="fontcomman"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style=" ">
                    <tr>
                        <td style="width: 120px">
                            <asp:Image ID="image1" Style="" Visible="false" runat="server" Width="75px" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblpartname" runat="server" Text="" CssClass="fontcomman"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblsubtitle" runat="server" Text="" Style="" CssClass="fontcomman"></asp:Label>
                        </td>
                    </tr>
                </table>
                <center>
                    <div id="divfpdata" style="height: auto; border: 0px solid teal; width: 720px;">
                        <FarPoint:FpSpread ID="fp1" ShowHeaderSelection="false" runat="server" BorderColor="Black"
                            BorderStyle="Solid" CssClass="fontcomman" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
                            HorizontalScrollBarPolicy="Never">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        
                        <div>
                            <asp:Button ID="btnrowadd" runat="server" Text="Add" OnClick="btnrowadd_Click" CssClass="fontcomman" />
                            <asp:Button ID="btnsaveparts" runat="server" Text="Save" OnClick="btnsaveparts_Click"
                                CssClass="fontcomman" />
                            <asp:Button ID="btnremove" runat="server" Text="Remove" OnClick="btnremove_Click"
                                CssClass="fontcomman" />
                        </div>
                        <div>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                CssClass="fontcomman" Height="320" Width="400" BorderWidth="1px" Visible="true"
                                VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            
                            <asp:Button ID="Button1" runat="server" Text="Add Row" CssClass="fontcomman" OnClick="Button1_Click1" />
                            <asp:Button ID="btnfpspread1save" runat="server" Text="Save" CssClass="fontcomman"
                                OnClick="btnfpspread1save_Click1" />
                            <asp:Button ID="btnfpspread1delete" runat="server" CssClass="fontcomman" Text="Delete All"
                                OnClick="btnfpspread1delete_Click1" />
                            
                            <asp:Label ID="lblerrvel" runat="server" CssClass="fontcomman" Text="Please Enter From & To Range and Description"
                                ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                </center>
                
            </div>
            <style>
                .fontcomman
                {
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                }
            </style>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

