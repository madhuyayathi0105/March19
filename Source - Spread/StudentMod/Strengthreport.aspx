<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Strengthreport.aspx.cs" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    Inherits="Strengthreport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: auto;
            width: 970px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 98%;
        }
        .col1
        {
            float: left;
            width: 49%;
        }
        .col2
        {
            float: right;
            width: 49%;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">

            function ClearPrint1() {
                var id = document.getElementById('<%=lbl_norec.ClientID%>');
                id.innerHTML = "";
                id.visible = false;
            }
        </script>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Student Strength
                                Report</span>
                        </div>
                    </center>
                    <div class="maindivstyle maindivstylesize">
                        <br />
                        <center>
                            <table class="maintablestyle" width="800px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox1" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Stream" Width="94px" runat="server" Text="Stream"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_stream" CssClass="ddlheight3  textbox1" runat="server"
                                            AutoPostBack="true" Width="146px" OnSelectedIndexChanged="ddl_stream_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_graduation" CssClass="ddlheight2  textbox1" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_graduation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_batch" Width="80px" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_batch" CssClass="ddlheight textbox1" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                ReadOnly="true" Width="150px">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" BackColor="White" CssClass="multxtpanel" Height="250px"
                                                Width="120px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                        <asp:DropDownList ID="ddl_degree" runat="server" CssClass="ddlheight4 textbox1" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_degree_Selectedindexchange">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p4" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_branch_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                                    PopupControlID="p4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sem" runat="server" Width="111px" CssClass="textbox textbox1 txtheight1"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel11" runat="server" CssClass="multxtpanel" Height="250px" Width="100px"
                                                    Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                    PopupControlID="Panel11" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sec" runat="server" Width="70px" CssClass="textbox textbox1 txtheight"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel8" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                                    PopupControlID="Panel8" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_colord" runat="server" Text="Report Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_colord" runat="server" CssClass="ddlheight4 textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <%--<td>
                                    Search
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" CssClass="textbox textbox1 txtheight3 " runat="server"
                                        placeholder="Search Student Roll No"> </asp:TextBox>
                                </td>--%>
                                    <td>
                                        Order by
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderby" Width="146px" Height="30px" runat="server" CssClass="textbox1 ddlheight"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlOrderby_OnIndexChange">
                                            <asp:ListItem Text="Order by Settings" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Alphabet"></asp:ListItem>
                                            <asp:ListItem Text="Admission Date"></asp:ListItem>
                                            <asp:ListItem Text="Gender"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--  </td>
                        <td>
                            <span id="spanGen" runat="server" visible="false">Gender</span>--%>
                                    </td>
                                    <td colspan="2">
                                        <fieldset style="height: 14px; width: 250px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbvehicleType" runat="server"  ForeColor="Black"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Vehicle Type" />
                                                        <asp:DropDownList ID="ddlvehType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="25px" Width="120px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Button ID="btndetailgo" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Text="Go" CssClass="textbox btn1 textbox1" OnClick="btndetailgo_Click" />
                                    </td>
                                    <td >
                                        <asp:RadioButtonList ID="rblGen" Style="float: left;" runat="server" RepeatDirection="Horizontal"
                                            Visible="false" OnSelectedIndexChanged="rblGen_Indexchange" AutoPostBack="true">
                                            <asp:ListItem Text="Male" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Female" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkinclude" runat="server" AutoPostBack="true" Width="158px" Text="Student Category"
                                            OnCheckedChanged="chkinclude_OnCheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtinclude" Style="height: 20px; width: 155px;" CssClass="Dropdown_Txt_Box"
                                                    runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel" Width="147px">
                                                    <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" OnCheckedChanged="cbinclude_OnCheckedChanged"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cblinclude" runat="server" OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                    PopupControlID="pnlinclude" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="cb_onlydis" runat="server" Width="200px" Enabled="false" Text="Only Show Discontinue" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div style="border-radius: 7px; width: 400px; margin-left: 722px;">
                                <asp:ImageButton ID="imgbtn_columsetting" Visible="false" runat="server" Width="30px"
                                    Height="30px" Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                            </div>
                        </center>
                        <br />
                        <center>
                            <center>
                                <asp:Label ID="lbl_headernamespd2" runat="server" ForeColor="Green" Visible="false"
                                    Font-Size="X-Large"></asp:Label>
                                <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Size="X-Large"></asp:Label>
                            </center>
                            <%-- <br />--%>
                            <%-- <div id="divcolor" runat="server" visible="false">
                            <asp:Label ID="lblappl" runat="server" Text="Applied" Style="color: #b287f2; font-weight: bold;
                                font-family: Book Antiqua;"></asp:Label>
                            <asp:Label ID="lbladmit" runat="server" Text="Admitted" Style="color: #f2c77d; font-weight: bold;
                                font-family: Book Antiqua;"></asp:Label>
                            <asp:Label ID="lbl_discnt" runat="server" Text="DisContinue/Left" Style="color: #F77474;
                                font-weight: bold; font-family: Book Antiqua;"></asp:Label>
                        </div>--%>
                            <div id="divcolor" runat="server" visible="false">
                                <asp:Label ID="lbl_dis" runat="server" Text="Discontinue" Height="20px" Width="100px"
                                    BackColor="Bisque" ForeColor="IndianRed"></asp:Label>
                                <asp:Label ID="lbl_debar" runat="server" Text="Debar" BackColor="Bisque" Height="20px"
                                    Width="100px" ForeColor="Blue"></asp:Label>
                                <asp:Label ID="lbl_coursecomplete" runat="server" Text="Course Complete" Height="20px"
                                    Width="150px" BackColor="Bisque" ForeColor="Green"></asp:Label>
                            </div>
                            <br />
                            <asp:UpdatePanel ID="up_spd1" runat="server">
                                <ContentTemplate>
                                    <center>
                                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ActiveSheetViewIndex="0"
                                            Style="margin-left: -5px; height: auto;" ShowHeaderSelection="false">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                        <br />
                        <center>
                            <div id="poppernew" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                left: 0;">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                    width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="imagebtnpopclose1_Click" />
                                <br />
                                <center>
                                    <div class="popsty" style="background-color: White; height: auto; width: 974px; border: 5px solid #0CA6CA;
                                        border-top: 5px solid #0CA6CA; border-radius: 10px; margin-top: -8px">
                                        <br />
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                                            OnClick="btn_addtype_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                                            CssClass="textbox textbox1 ddlheight4">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                            OnClick="btn_deltype_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                        <br />
                                        <fieldset style="border-radius: 10px; width: 900px; height: auto;">
                                            <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                                            <div>
                                                <asp:TextBox ID="txtcolumn" runat="server" AutoPostBack="true" TextMode="MultiLine"
                                                    Width="900px" Height="100px" ReadOnly="true"></asp:TextBox>
                                                <br />
                                                <asp:LinkButton ID="lnk_selectall" runat="server" Font-Size="Small" AutoPostBack="true"
                                                    Height="20px" Visible="true" Style="margin-left: 0px; margin-top: 5px;" OnClick="LinkButtonselectall_Click">Select All</asp:LinkButton>
                                                <asp:LinkButton ID="lnk_columnordr" runat="server" Font-Size="Small" AutoPostBack="true"
                                                    Height="20px" Visible="true" Style="margin-left: 450px; margin-top: 5px;" OnClick="LinkButtonsremove_Click">Remove All</asp:LinkButton>
                                                <br />
                                                <asp:CheckBoxList ID="lb_selectcolumn" runat="server" AutoPostBack="true" RepeatColumns="6"
                                                    Font-Size="Small" OnSelectedIndexChanged="lb_selectcolumn_Selectedindexchange">
                                                </asp:CheckBoxList>
                                            </div>
                                            <br />
                                            <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                            <br />
                                            <center>
                                                <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                                                <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />
                                            </center>
                                        </fieldset>
                                    </div>
                                </center>
                            </div>
                        </center>
                        <center>
                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                            </asp:Label></center>
                        <div id="div_report" runat="server" visible="false">
                            <center>
                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                    CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                    AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                                <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                                    CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                                    Font-Bold="true" />
                                </br>
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </center>
                        </div>
                        </br>
                        <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    margin-top: 200px; border-radius: 10px;">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                                <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                        <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
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
                                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </div>
                </div>
            </center>
        </div>
    </body>
</asp:Content>
