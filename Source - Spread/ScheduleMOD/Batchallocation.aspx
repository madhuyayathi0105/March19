<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Batchallocation.aspx.cs" Inherits="Batchallocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <link href="Styles/AttendanceStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 90%;
        }
        
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .cursorptr
        {
            cursor: pointer;
        }
        .cursordflt
        {
            cursor: default;
        }
        
        #clsbtn
        {
            height: 26px;
            width: 72px;
        }
        
        .style2
        {
            width: 111px;
        }
        .style3
        {
            width: 145px;
        }
        .style4
        {
            width: 252px;
        }
        .style5
        {
            width: 160px;
        }
        
        .style6
        {
            width: 68%;
        }
        .style7
        {
            width: 336px;
        }
        .style8
        {
            width: 330px;
        }
    </style>
    <%--</head>--%>
    <body oncontextmenu="return false">
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up_spreadbatch" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up_spreadbatch">
                    <ProgressTemplate>
                        <center>
                            <div style="height: 40px; width: 150px;">
                                <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                                <br />
                                <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                    Processing Please Wait...</span>
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
                    PopupControlID="UpdateProgress1">
                </asp:ModalPopupExtender>
                <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="up_spreadbatch">
                    <ProgressTemplate>
                        <div class="CenterPB" style="height: 40px; width: 40px;">
                            <img src="../images/progress2.gif" height="180px" width="180px" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                    PopupControlID="UpdateProgress1">
                </asp:ModalPopupExtender>--%>
                <div style="height: 54px; width: 1207px;">
                    <center>
                        <asp:Label ID="lblhead" runat="server" Text="Alternate Batch Allocation For Laboratory"
                            CssClass="fontstyleheader" ForeColor="Green"></asp:Label>
                    </center>
                    <div>
                        <br />
                        <table cellpadding="2px" cellspacing="4px" style="height: 100%; margin-left: 0px;
                            width: 103%;" class="maintablestyle ">
                            <tr>
                                <td class="style8">
                                    <asp:Label runat="server" ID="lblCollege" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged" Height="25px"
                                        Width="260px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="style2">
                                    <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                        Width="60px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="style3">
                                    <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="80px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td class="style4">
                                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="180px"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="style5">
                                    <asp:Label runat="server" ID="lblduration" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="True" Height="25px" Width="80px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblsec" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="25px" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;<asp:Label ID="lblFromdate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="16px" Width="87px"
                                        OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFromDate"
                                        FilterType="Custom,Numbers" ValidChars="/" />
                                    <asp:CalendarExtender ID="CalExtFromDate" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="bcntlbl" runat="server" Text="No of Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="btctxt" runat="server" AutoPostBack="True" Height="16px" OnTextChanged="btctxt_TextChanged"
                                        Width="41px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="btctxt"
                                        FilterType="Numbers" />
                                    <%--  <asp:Label ID="bcntddllbl" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>--%>
                                    <%--   <asp:DropDownList ID="bcntddl" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="bcntddl_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" Style="height: 26px;
                                        left: 305px; position: absolute; font-weight: 700" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <br />
                                    <asp:Label ID="fmlbl" runat="server" Text="Enter Todate" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="deglbl" runat="server" Text="Select degree" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="branlbl" runat="server" Text="Select branch" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="semlbl" runat="server" Text="Select semester" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="seclbl" runat="server" Text="Select section" ForeColor="Red" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="style1">
                                    <%-- <asp:Panel ID="Panel5" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 271px;
                                        left: 0px; position: absolute; width: 1030px; height: 18px; margin-bottom: 0px;
                                        background-image: url('Menu/Top%20Band-2.jpg');">
                                        <br />
                                        <br />
                                        <br />
                                    </asp:Panel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <%----------------------bind------------------------%>
                                <td class="style1">
                                    <center>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                            Style="margin-left: -103px; position: absolute;" Font-Size="Medium"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </center>
                                    <asp:Panel ID="batchpanel" runat="server">
                                        <table id="batchtable" style="border-style: double;">
                                            <tr>
                                                <td class="style6">
                                                    <asp:Panel ID="panel_sp1" runat="server" Height="400px" Width="570px">
                                                        <div style="float: left; width: 520px; height: 300px; overflow: auto;">
                                                            <asp:GridView ID="gview" runat="server" Style="height: 100px; width: 500px; overflow: auto;"
                                                                AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label runat="server" ID="lblsno" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Select" Visible="false">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:CheckBox runat="server" ID="chck" /></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Roll No">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblroll" Text='<%#Eval("Roll No") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Reg No">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblreg" Text='<%#Eval("Reg No") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Student Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblstunme" Text='<%#Eval("Student Name") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Batch" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlbatch" Width="50" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" ID="lblbatch" Visible="false" Text='<%#Eval("Batch") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" />
                                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                                <RowStyle ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <div style="margin-left: 20px; margin-top: -110px;">
                                                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                                        <asp:Label ID="sfrlbl" runat="server" Text="Check From" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="90px"></asp:Label>
                                                        <asp:TextBox ID="sfmtxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="35"></asp:TextBox>
                                                        <asp:Label ID="stolbl" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="30"></asp:Label>
                                                        <asp:TextBox ID="stotxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="35" AutoPostBack="true" OnTextChanged="stotxt_TextChanged"></asp:TextBox>
                                                        <asp:DropDownList ID="bcntddl" runat="server" AutoPostBack="True" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="bcntddl_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="Button1" runat="server" Text="Select" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="28px" Width="58px" OnClick="selbtn_Click" />
                                                        <asp:Button ID="selbtn" runat="server" Text="Select" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="28px" Width="55px" OnClick="selbtn_Click" />
                                                        <asp:Button ID="btnsave" runat="server" Text="Save" Height="28px" Width="55px" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnsave_Click" />
                                                        <asp:Button ID="delbtn" runat="server" Text="Delete" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="28px" Width="60px" OnClick="delbtn_Click" />
                                                    </div>
                                                </td>
                                                <%-- ------------------------lab----------------------%>
                                                <td class="style7" align="center">
                                                    <%--align added by Manikandan 20/08/2013--%>
                                                    <asp:Panel ID="Panel_sp2" runat="server" BorderColor="ActiveCaptionText" BorderStyle="Ridge"
                                                        Style="position: absolute; top: 322px; left: 626px;" Height="250px" Width="360px">
                                                        <asp:Label ID="lblDayOrder" runat="server" Text="Select Day" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="90px"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlDayOrder" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="25px" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddlDayOrder_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <br />
                                                        <center>
                                                            <div style="float: left;  width: 360px; height: 250px; overflow: auto;">
                                                                <asp:GridView ID="gview1" runat="server" AutoGenerateColumns="false" Width="350"
                                                                    OnRowDataBound="gview1_OnRowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Day">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbldate" runat="server" Text='<%#Eval("Day") %>' /></center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Hour">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lblhour" runat="server" Text='<%#Eval("Hour") %>' /></center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab1" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div1" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod1" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab1" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab1" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab1" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab1" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab1" runat="server" TargetControlID="txt_lab1"
                                                                                                PopupControlID="plab1" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab2" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div2" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod2" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab2" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab2" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab2" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab2" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab2" runat="server" TargetControlID="txt_lab2"
                                                                                                PopupControlID="plab2" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab3" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div3" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod3" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab3" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab3" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab3" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab3" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab3" runat="server" TargetControlID="txt_lab3"
                                                                                                PopupControlID="plab3" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab4" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div4" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod4" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab4" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab4" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab4" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab4" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab4" runat="server" TargetControlID="txt_lab4"
                                                                                                PopupControlID="plab4" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab5" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div5" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod5" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab5" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab5" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab5" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab5" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab5" runat="server" TargetControlID="txt_lab5"
                                                                                                PopupControlID="plab5" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab6" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div6" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod6" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab6" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab6" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab6" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab6" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab6" runat="server" TargetControlID="txt_lab6"
                                                                                                PopupControlID="plab6" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab7" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div7" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod7" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab7" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab7" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab7" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab7" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab7" runat="server" TargetControlID="txt_lab7"
                                                                                                PopupControlID="plab7" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab8" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div8" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod8" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab8" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab8" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab8" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab8" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab8" runat="server" TargetControlID="txt_lab8"
                                                                                                PopupControlID="plab8" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab9" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div9" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod9" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab9" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab9" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab9" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab9" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab9" runat="server" TargetControlID="txt_lab9"
                                                                                                PopupControlID="plab9" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Lab10" ItemStyle-Width="100">
                                                                            <ItemTemplate>
                                                                                <div id="div10" style="position: relative;" runat="server">
                                                                                    <asp:UpdatePanel ID="upnlPeriod10" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:TextBox ID="txt_lab10" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                                                            <asp:Panel ID="plab10" runat="server" CssClass="multxtpanel" Width="100px" Height="100px"
                                                                                                Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                                                                <asp:CheckBox ID="chk_lab10" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                                <asp:CheckBoxList ID="Chklst_lab10" Font-Bold="true" Font-Size="Medium" runat="server"
                                                                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                                                </asp:CheckBoxList>
                                                                                            </asp:Panel>
                                                                                            <asp:PopupControlExtender ID="popuplab10" runat="server" TargetControlID="txt_lab10"
                                                                                                PopupControlID="plab10" Position="Bottom">
                                                                                            </asp:PopupControlExtender>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                                        Font-Names="Book Antiqua" />
                                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                                    <RowStyle ForeColor="#333333" />
                                                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                                </asp:GridView>
                                                            </div>
                                                        </center>
                                                        <asp:Button ID="btn2sv" runat="server" Text="save" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" OnClick="btn2sv_Click" Style="position: absolute; left: 155px;
                                                            margin-top: 274px;" />
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue" Style="left: 5px;
                                                            position: absolute; width: 150px; top: 200px;" OnClick="LinkButton1_Click" Visible="false">To Add Multiple Batch</asp:LinkButton>
                                                        <fieldset id="Fieldset5" visible="false" runat="server" style="position: absolute;
                                                            left: 5px; width: 116px; background-color: white; top: 203px; height: 84px;">
                                                            <asp:CheckBoxList ID="Checkboxlistbatch" runat="server" Style="width: 92px; position: absolute;
                                                                top: 1px; left: 2px; border-style: double;" OnSelectedIndexChanged="Checkboxlistbatch_SelectedIndexChanged"
                                                                Visible="false">
                                                            </asp:CheckBoxList>
                                                            <asp:Button ID="Button3" runat="server" Text="Ok" Style="position: absolute; left: 98px;
                                                                top: 68px;" OnClick="Button3_Click" Visible="false" />
                                                        </fieldset>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
        <%-- </form>--%>
    </body>
    </html>
</asp:Content>
