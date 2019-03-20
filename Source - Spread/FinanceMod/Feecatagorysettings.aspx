<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Feecatagorysettings.aspx.cs" Inherits="Feecatagorysettings" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Fee Catagory
                        Settings</span>
                </div>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <table>
                        <tr>
                            <td>
                                Institution Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" CssClass="ddlheight6 textbox1" runat="server">
                                </asp:DropDownList>
                            </td>
                            <%--<td>
                                Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_type" CssClass="ddlheight2 textbox1" runat="server">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Semester</asp:ListItem>
                                    <asp:ListItem Value="2">Year</asp:ListItem>
                                    <asp:ListItem Value="3">Term</asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            <td>
                                <asp:Button ID="btn_go" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Text="Go" CssClass="textbox btn1 textbox1" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Text="Add New" CssClass="textbox btn2 textbox1" OnClick="btn_addnew_Click" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkfine" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Style="margin-left: 30px;" Font-Size="Large" ForeColor="Blue" CausesValidation="False"
                                    OnClick="lnkyearMatch_click">Year Matching</asp:LinkButton><%----%>
                            </td>
                        </tr>
                    </table>
                    <div id="popfine" runat="server" visible="false" style="height: 100em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 12px; margin-left: 376px;" OnClick="imagepopclose_click" />
                        <br />
                        <br />
                        <center>
                            <div style="height: 700px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <fieldset style="height: auto;">
                                    <legend class="fontstyleheader" style="color: Green;">Fee Category -Year Matching</legend>
                                    <br />
                                    <br />
                                    <center>
                                        <br />
                                        <br />
                                        <b>Year Matching</b>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_CollegeName" runat="server" Text="College Name" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegepop1" CssClass="ddlheight6 textbox1" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_collegepop1_Selectedindex" Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Year" runat="server" Text="Year" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_Year" CssClass="ddlheight6 textbox1" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddl_Year_Selectedindex" Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Semseter" runat="server" Text="Semseter" />
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updSemester" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtsemster" runat="server" Height="15px" CssClass="textbox txtheight2"
                                                                ReadOnly="true" Width="90px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlsemester" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cbSemester" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cbSemester_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblSemester" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSemester_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtsemster"
                                                                PopupControlID="pnlSemester" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Term" runat="server" Text="Term" />
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updTerm" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtTerm" runat="server" Height="15px" CssClass="textbox txtheight2"
                                                                ReadOnly="true" Width="90px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlTerm" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cbTerm" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbTerm_CheckedChanged" />
                                                                <asp:CheckBoxList ID="cblTerm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblTerm_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtTerm"
                                                                PopupControlID="pnlTerm" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="SaveFeeCat" runat="server" Visible="True" Font-Bold="true" Font-Names="Book Antiqua"
                                                        CssClass="textbox btn2 textbox1" Text="Save" OnClick="SaveFeeCat_click" />
                                                    <%-- <asp:Label ID="lbl_status" runat="server" Visible="false" />--%>
                                                </td>
                                                <td>
                                                    <asp:Button ID="GoFeeCat" runat="server" Visible="True" Font-Bold="true" Font-Names="Book Antiqua"
                                                        CssClass="textbox btn2 textbox1" Text="Go" OnClick="GoFeeCat_click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </fieldset>
                                 <center>
                        <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gdReport" runat="server" Visible="false" AutoGenerateColumns="false"
                                    GridLines="Both" Width="730px" OnDataBound="gdattrpt_OnDataBound" OnRowDataBound="gdReport_OnRowDataBound" >
                               
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="College Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblclg" runat="server" Text='<%#Eval("collegeStr") %>'></asp:Label>
                                                <asp:Label ID="lblclgVal" runat="server" Visible="false" Text='<%#Eval("collegeVal") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblYear" runat="server" Text='<%#Eval("Year") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblSem" runat="server" Text='<%#Eval("semester") %>'></asp:Label>
                                                    <asp:Label ID="lblSemVal" runat="server" Visible="false" Text='<%#Eval("semesterVal") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Term" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblTerm" runat="server" Text='<%#Eval("TermStr") %>'></asp:Label>
                                                    <asp:Label ID="lblTermVal" runat="server" Visible="false" Text='<%#Eval("TermVal") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" /><%--OnClick="btnUpdate_Click"--%>
                                                    <asp:Label ID="lblbutton" runat="server" Visible="false" Text='<%#Eval("button") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                        </center>
                            </div>
                        </center>
                       
                    </div>
                    
                    <div>
                        <asp:Label ID="lbl_error2" runat="server" ForeColor="red"></asp:Label></div>
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Width="868px" Height="360px" CssClass="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
            </center>
            <center>
                <div id="popwindow" runat="server" visible="false" style="height: 355em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 83px; margin-left: 470px;" OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <center>
                        <br />
                        <br />
                        <br />
                        <br />
                        <div class="popsty" style="background-color: White; height: auto; width: 991px; border: 5px solid #0CA6CA;
                            border-top: 5px solid #0CA6CA; border-radius: 10px; margin-top: -8px">
                            <br />
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            Institution Name
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_collegepop" CssClass="ddlheight6 textbox1" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_collegepop_Selectedindex">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fee Catagory Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_feecatagory" CssClass="textbox textbox1 txtheight4" runat="server">
                                            </asp:TextBox>
                                            <asp:Button ID="btn_savepop" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                CssClass="textbox btn2 textbox1" Text="Save" OnClick="btn_savepop_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="btn_lnk" runat="server" Text="Fee catagory matching" OnClick="btn_lnk_OnClick"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnlnk_Month" runat="server" Text="Month Matching" OnClick="btnlnkMonth_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_error" runat="server" ForeColor="Green"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <div runat="server" visible="false" id="feecatagorymatch">
                                    <table>
                                        <tr>
                                            <td>
                                                Fee Catagory
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_feecatagory" CssClass="ddlheight3 textbox1" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_feecatagory_selectedindex">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_degree" Text="Degree Name" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_degree" runat="server" Width="150px" CssClass="textbox textbox1 txtheight3"
                                                            ReadOnly="true">-- Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel8" runat="server" CssClass="multxtpanel" Height="250px" Width="150px"
                                                            Style="position: absolute;">
                                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="cb_degree_checkedchange" />
                                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                                            PopupControlID="Panel8" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_savepop1" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn2 textbox1" Text="Save" OnClick="btn_savepop1_click" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblmonth" Visible="false" runat="server" Text="Month"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlmonth" Visible="false" Style="width: 100px;" CssClass="ddlheight6 textbox1"
                                                    runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblYear" Visible="false" runat="server" Text="Year"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlYear" Visible="false" Style="width: 80px;" CssClass="ddlheight6 textbox1"
                                                    runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblbatchYear" Visible="false" runat="server" Text="batchYear"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbatchYear" Visible="false" Style="width: 80px;" CssClass="ddlheight6 textbox1"
                                                    runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnmonth_save" Visible="false" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn2 textbox1" Text="Save" OnClick="btnmonth_save_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_error1" ForeColor="Green" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    </body>
</asp:Content>
