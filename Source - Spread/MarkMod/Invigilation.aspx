<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Invigilation.aspx.cs" Inherits="MarkMod_Invigilation" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Invigilation Entry</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_cycletest" Text="Cycle Test Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_cycletest" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_cycletest" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_cycletest" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_cycletest" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_cycletest_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_cycletest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_cycletest_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_cycletest" runat="server" TargetControlID="txt_cycletest"
                                                                PopupControlID="panel_cycletest" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <center>
            <div id="showreport1" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                              
        <asp:GridView ID="GridView1" runat="server" style="margin-bottom:15px;margin-top:15px; width:auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowDataBound="gridview1_OnRowDataBound" Width="500px">
        <Columns>
        <asp:TemplateField HeaderText="S.No">
        <ItemTemplate>
        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
        <ItemTemplate>
        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("date") %>'></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Hall" >
        <ItemTemplate>
        <asp:Label ID="lblhall" runat="server" Text='<%# Eval("hall") %>'></asp:Label>     
        <asp:Label ID="lbltestno" runat="server" Text='<%# Eval("testno") %>' Visible="false"></asp:Label>
        </ItemTemplate>
           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="FN">
        <ItemTemplate>
        <asp:Label ID="lblfn" runat="server" Text= '<%# Eval("fn") %>' Visible="false"></asp:Label>
        <asp:CheckBox ID="cb_fn" runat="server"  />
        </ItemTemplate>
          <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="AN">
        <ItemTemplate>
        <asp:Label ID="lblAn" runat="server" Text='<%# Eval("an") %>'  Visible="false"></asp:Label>
        <asp:CheckBox ID="cb_an" runat="server" />
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
        </asp:TemplateField>
        </Columns>
         <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
      
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                        Visible="false" Style="text-align: center" />
                </center>
            </div>
        </center>
    
    </div>
    <center>
      <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                border-radius: 10px;">
                <center>
                    <br />
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                        OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
    </center>
     <center>

       <div id="saveMessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                border-radius: 10px;">
                <center>
                    <br />
                    <table style="height: 100px; width: 100%">
                          <tr>
                   
                    <td valign="middle" align="left">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label1" Text="Do You Want to Save" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Red;"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="Button1" runat="server" Text="Yes" OnClick="btnattOk_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="Button2" runat="server" Text="No" OnClick="btnattCancel_Click"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
    </center>
     
       <center>
      <div id="alertpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                border-radius: 10px;">
                <center>
                    <br />
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblalerterror" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btnerorrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                        OnClick="btnok_Click" Text="Ok" runat="server" />
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
