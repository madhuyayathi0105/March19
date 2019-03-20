<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ITGroupMapping.aspx.cs" Inherits="HRMOD_ITGroupMapping" %>

<%@ Register Src="~/UserControls/PrintMaster.ascx" TagName="printmaster" TagPrefix="InsproPlus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <body>
        <script type="text/javascript">
            function getgrp(txt) {
                $.ajax({
                    type: "POST",
                    url: "GroupMaster.aspx/checkGroupName",
                    data: '{grpname: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function Success(response) {
                var mesg1 = $("#lbladd_err")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "Not Exist";
                        break;
                    case "1":
                        mesg1.style.color = "red";
                        document.getElementById('<%=txtgroup.ClientID %>').value = "";
                        mesg1.innerHTML = "Group Name Already Exist!";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter Group Name";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error occurred";
                        break;
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Group Master</span>
                </div>
                <center>
                    <div class="maindivstyle" style="width: 1000px; height: 550px;">
                        <br />
                        <div>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcol" runat="server" Style="font-family: 'Book Antiqua'" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcolload" runat="server" CssClass="textbox textbox1 ddlheight5"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlcolload_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="bttngo" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                            OnClick="bttngo_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="bttnadd" runat="server" CssClass="textbox textbox1 btn2" Width="120px"
                                            Text="Add New Group" OnClick="BtnNewTree_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGroupMapping" runat="server" CssClass="textbox textbox1 btn2"
                                            Width="120px" Text="Group Mapping" OnClick="btnGroupMapping_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGroupPrioriry" runat="server" CssClass="textbox textbox1 btn2"
                                            Width="120px" Text="Group Priority" OnClick="btnGroupPrioriry_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                        <br />
                        <div id="CreateGroup" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                                                BorderWidth="2px" Style="width: 350px; float: left; margin-left: 75px; height: 360px;
                                                overflow: auto;">
                                                <div>
                                                    <asp:Label ID="lblsubtree" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                                                    height: 240px; font-size: Small; font-weight: bold">
                                                    <asp:TreeView ID="TreeView1" runat="server" ForeColor="Green" HoverNodeStyle-ForeColor="Red"
                                                        ExpandDepth="0" LeafNodeStyle-ForeColor="Black" ShowLines="true" ShowExpandCollapse="true"
                                                        OnTreeNodeDataBound="TreeView1_DataBound" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                        AutoPostBack="true">
                                                    </asp:TreeView>
                                                </div>
                                            </asp:Panel>
                                            <br />
                                            <div style="height: 30px;">
                                                <%-- <asp:Button ID="BtnNewTree" runat="server" Text="New Group"  CssClass="textbox textbox1 btn2"  OnClick="BtnNewTree_Click"  Visible="false" />--%>
                                                <asp:Button ID="BtnAddChild" runat="server" Text="Sub Group " CssClass="textbox textbox1 btn2"
                                                    OnClick="BtnAddChild_Click" />
                                                <asp:Button ID="BtnUpdateTree" runat="server" Text="Update" CssClass="textbox textbox1 btn2"
                                                    OnClick="BtnUpdateTree_Click" />
                                                <%--<asp:Button ID="BtnExit" float="left" runat="server" Visible="false" Text="Exit"
                                                CssClass="textbox textbox1 btn2" OnClick="BtnExitTree_Click" />--%>
                                            </div>
                                        </center>
                                    </td>
                                    <td style="padding-left: 25px;">
                                        <div id="popupwindow" runat="server" visible="false" style="float: right; margin-right: 75px;
                                            border: 1px solid; border-radius: 10px; font-size: medium; width: 400px; height: 360px;
                                            background-color: AliceBlue;">
                                            <br />
                                            <center>
                                                <asp:Label ID="lblhead" runat="server" Visible="false" Style="font-size: large; color: Green;"></asp:Label>
                                            </center>
                                            <br />
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 20px; padding-bottom: 20px;">
                                                        <asp:Label ID="lblgroup" runat="server" Font-Names="Book Antiqua" Text="Group Name"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 20px; padding-bottom: 20px;">
                                                        <asp:TextBox ID="txtgroup" runat="server" TextMode="MultiLine" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" onblur="return getgrp(this.value)" Style="text-transform: none;"
                                                            Width="180px" Height="50px" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txtgroup"
                                                            FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars="/().,&-:;[] ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 20px; padding-bottom: 20px;">
                                                        <asp:Label ID="lbldesc" runat="server" Font-Names="Book Antiqua" Text="Description"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 20px; padding-bottom: 20px;">
                                                        <asp:TextBox ID="txtdesc" runat="server" TextMode="MultiLine" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Width="180px" Height="50px" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdesc"
                                                            FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars="/().,&  ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Max Limit Amount
                                                    </td>
                                                    <td style="padding-left: 20px; padding-bottom: 20px;">
                                                        <asp:TextBox ID="txtMaxLimtAmount" runat="server" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                            Width="120px" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMaxLimtAmount"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <center>
                                                <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                                <asp:Button ID="btnadd" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                                    Text="Save" OnClick="btnAddnew_click" />
                                                <asp:Button ID="btnAddnew" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                                    Text="Add" OnClick="btnadd_click" />
                                                <asp:Button ID="btnupdate" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                                    Text="Update" OnClick="btnupdate_Click" />
                                                <asp:Button ID="btndelete" runat="server" Visible="false" CssClass="textbox textbox1 btn2"
                                                    Text="Delete" OnClick="btndelete_Click" />
                                                <asp:Button ID="btnpopexit" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                                    OnClick="btnpopexit_Click" />
                                            </center>
                                            <br />
                                            <center>
                                                <asp:Label ID="lbladd_err" runat="server" Style="color: Red;"></asp:Label>
                                            </center>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="CreatePriority" runat="server" visible="false">
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" class="spreadborder" ShowHeaderSelection="false"
                                OnButtonCommand="FpSpread1_ButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <center>
                                <asp:Button ID="btnPrioritySave" CssClass="textbox textbox1 btn1" OnClick="btnPrioritySave_Click"
                                    Text="Save" runat="server" Width="60px" />
                                <asp:Button ID="btnPriorityReset" CssClass="textbox textbox1 btn1" OnClick="btnPriorityReset_Click"
                                    Text="Reset" runat="server" Width="60px" />
                            </center>
                        </div>
                        <div id="CreateMapping" runat="server" visible="false">
                            <div id="SubDiv" style="width: 800px; height: 356px;">
                                <asp:Panel ID="MappingPanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                    BorderWidth="2px" Style="width: 350px; height: 360px; float: left; overflow: auto;">
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="PopupHeaderrstud2" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                        height: 240px; font-size: Small; font-weight: bold">
                                        <asp:TreeView ID="TreeView2" runat="server" ForeColor="Green" HoverNodeStyle-ForeColor="Red"
                                            ExpandDepth="0" LeafNodeStyle-ForeColor="Black" ShowLines="true" ShowExpandCollapse="true"
                                            OnSelectedNodeChanged="TreeView2_SelectedNodeChanged" AutoPostBack="true">
                                        </asp:TreeView>
                                    </div>
                                </asp:Panel>
                                <br />
                                <div style="height: 30px; display: none;">
                                    <%-- <asp:Button ID="BtnNewTree" runat="server" Text="New Group"  CssClass="textbox textbox1 btn2"  OnClick="BtnNewTree_Click"  Visible="false" />--%>
                                    <asp:Button ID="Button1" runat="server" Text="Sub Group " CssClass="textbox textbox1 btn2"
                                        OnClick="BtnAddChild_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Update" CssClass="textbox textbox1 btn2"
                                        OnClick="BtnUpdateTree_Click" />
                                    <%--<asp:Button ID="BtnExit" float="left" runat="server" Visible="false" Text="Exit"
                                                CssClass="textbox textbox1 btn2" OnClick="BtnExitTree_Click" />--%>
                                </div>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BackColor="AliceBlue" BorderWidth="2px"
                                    Style="width: 420px; height: 360px; float: left; margin-left: 20px; margin-top: -21px;">
                                    <div>
                                        <asp:Label ID="Label2" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlHeadtype" runat="server" OnSelectedIndexChanged="ddlHeadtype_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Value="1">Income Head</asp:ListItem>
                                                        <asp:ListItem Value="2">Deduction Head</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="TitleSpan" runat="server">Income Head</span>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtIncomeHead" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                                Style="font-weight: bold; width: 330px; font-family: book antiqua; font-size: medium;">Select</asp:TextBox>
                                                            <asp:Panel ID="P3" runat="server" CssClass="multxtpanel" Height="200px" Width="330px">
                                                                <asp:CheckBox ID="cbIncome" runat="server" Text="Select All" OnCheckedChanged="cbIncome_CheckedChange"
                                                                    AutoPostBack="true" />
                                                                <asp:CheckBoxList ID="cblincome" runat="server" OnSelectedIndexChanged="cblincome_SelectedIndexChange"
                                                                    AutoPostBack="true">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtIncomeHead"
                                                                PopupControlID="P3" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <div style="height: 30px;">
                                        <%-- <asp:Button ID="BtnNewTree" runat="server" Text="New Group"  CssClass="textbox textbox1 btn2"  OnClick="BtnNewTree_Click"  Visible="false" />--%>
                                        <asp:Label ID="lblNode" runat="server" Visible="false"></asp:Label>
                                        <asp:Button ID="btnMappingChild" runat="server" Text="Add Group Mapping " CssClass="textbox textbox1 btn2"
                                            Width="130px" OnClick="btnMappingChild_Click" />
                                        <%--<asp:Button ID="BtnExit" float="left" runat="server" Visible="false" Text="Exit"
                                                CssClass="textbox textbox1 btn2" OnClick="BtnExitTree_Click" />--%>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <br />
                    </div>
                </center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <center>
                    <div id="imgdiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div4" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 150px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnyes" CssClass="textbox textbox1 btn1" OnClick="btnyes_Click" Text="Yes"
                                                        runat="server" />
                                                    <asp:Button ID="btnno" CssClass="textbox textbox1 btn1" OnClick="btnno_Click" Text="No"
                                                        runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
