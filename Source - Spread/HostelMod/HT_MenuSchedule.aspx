<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HT_MenuSchedule.aspx.cs" Inherits="HT_MenuSchedule" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <script src="Styles/~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
        <style type="text/css">
        .btn
        {
            width: 40px;
            height: 30px;
        }
        .btn1
        {
            height: 30px;
        }
        .sty1
        {
            height: 500px;
            width: 900px;
            border: 1px solid Gray;
            background-color: White;
        }
        .sty2
        {
            height: 550px;
            width: 900px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .sty3
        {
            height: 550px;
            width: 900px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .backpaneldrop
        {
            position: absolute;
            background-color: White;
            border: 1px solid Gray;
        }
        .style
        {
            height: 550px;
            width: 1000px;
            border: 1px solid Gray;
            background-color: #F0F0F0;
            border-radius: 10px;
        }
        .ddlstyle
        {
            width: 160px;
            height: 30px;
            outline: none;
            border: 1px solid #7bc1f7;
            box-shadow: 0px 0px 8px #7bc1f7;
            -moz-box-shadow: 0px 0px 8px #7bc1f7;
            -webkit-box-shadow: 0px 0px 8px #7bc1f7;
        }
        .txtdate
        {
            border: 1px solid #c4c4c4;
            height: 20px;
            width: 70px;
            font-size: 13px;
            text-transform: capitalize;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .table
        {
            background-color: white;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
        }
        .multxtpanel
        {
            background: White;
            border-color: Gray;
            border-style: Solid;
            border-width: 2px;
            position: absolute;
            box-shadow: 0px 0px 4px #999999;
            border-radius: 5px;
            overflow: auto;
            width: auto;
        }
         .autocomplete_highlightedListItem
        {
            background-color: #EEEE89;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;

            border-style: solid;
            cursor: 'default';
            height: 100px;
            font-family: Book Antiqua;
            font-size: small;
            text-align: left;
            list-style-type: none;
            padding-left: 1px;
            width: 430px;
            overflow: auto;
            overflow-x: hidden;
            
            border-color: #999999;

            border-width: 1px;
            position: absolute;
            box-shadow: 0px 0px 4px #999999;
            border-radius: 2px;

            
        }
        .fontstyleheader
        {
            font-family: Book Antiqua;
            font-size: x-large;
        }
        .style
        {
             border: 1px solid #999999;
            background-color: #F0F0F0;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            -moz-box-shadow: 0px 0px 10px #999999;
            -webkit-box-shadow: 0px 0px 10px #999999;
            border: 3px solid #D9D9D9;
            border-radius: 15px;
        }
        <%-- .multxtpanel
        {
           background:  #DDEBF9;
        }--%>
    </style>
    </head>
    <body>
        <script type="text/javascript">
            function checkDecimal(el) {
                var ex = /^((\d{5})*|([1-9]\d{0,5}))(\.\d{0,2})?$/;
                if (ex.test(el.value) == false) {
                    el.value = '';
                    //alert('Enter Before Decimal six numbers & After Decimal two numbers');

                }
            }
            function check() {
                var check = true;
                var txt = document.getElementById('myInput');
                tmp = txt.value;
                if (tmp && tmp.length > 0) {

                    arg = tmp.split('.');
                    if (arg && arg.length > 0) {

                        check = arg[0].length <= 6;
                        if (arg.length > 1 && check) {
                            check = arg[1].length <= 2;
                        }
                    }


                }
                if (!check)
                    alert('check failed');

            }


            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

 
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <asp:Label ID="lblheader" runat="server" Style="color: Green;" Text="Menu Schedule"
                    CssClass="fontstyleheader"></asp:Label>
                <br />
                <br />
            </center>
            <center>
                <div class="style">
                    <br />
                    <table style="border: 1px solid #0CA6CA; border-radius: 10px; background-color: #0CA6CA;
                        font-size: medium;" class="table">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_messname" runat="server" Text="Mess Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_messname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_messname_SelectedIndexChanged"
                                    Width="154px" Height="30px" CssClass="Dropdown_Txt_Box textbox1 ">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblscheduletype" runat="server" Text="Schedule Type"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdodatewise" Text="Datewise" runat="server" GroupName="day"
                                    AutoPostBack="true" OnCheckedChanged="rdodatewise_CheckedChanged" />
                                <asp:RadioButton ID="rdodaywise" Text="Daywise" runat="server" GroupName="day" AutoPostBack="true"
                                    OnCheckedChanged="rdodaywise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" runat="server" CssClass="txtdate" AutoPostBack="true"
                                    OnTextChanged="txtfromdate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttodate" runat="server" CssClass="txtdate" AutoPostBack="true"
                                    OnTextChanged="txttodate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="caltodate" TargetControlID="txttodate" runat="server" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="rdbMenu" runat="server" Text="Menu Item Schedule" AutoPostBack="true"
                                    OnCheckedChanged="rdbMenu_Change" GroupName="Same1" />
                                <asp:RadioButton ID="rdbcleaning" runat="server" AutoPostBack="true" OnCheckedChanged="rdbCleaning_Change"
                                    Text="Cleaning & Other Item Schedule" GroupName="Same1" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkall1" runat="server" Visible="false" Enabled="false" Text="Check All" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" Text="Go" runat="server" CssClass="textbox btn" OnClick="btngo_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="errorlable" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <div id="div1" runat="server" visible="false" style="width: 850px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;
                        box-shadow: 0px 0px 8px #999999">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="750px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                            CssClass="textbox btn1" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" Width="60px" />
                        <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="textbox btn1" Width="60px"
                            OnClick="btnsave_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
                <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 440px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <div class="sty2" style="background-color: White;" align="center">
                        <br />
                        <center>
                            <span style="font-size: large; color: Green;">Select the Menu</span>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_menutype" runat="server" Text="Menu Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanelnew" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_menutype" runat="server" CssClass="textbox textbox1" Width="115px"
                                                    Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="120px" Height="100px">
                                                    <asp:CheckBox ID="cb_menutype" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_menutype_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_menutype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_menutype_SelectIndexChange">
                                                        <asp:ListItem Value="0">Veg</asp:ListItem>
                                                        <asp:ListItem Value="1">Non-Veg</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtendernew" runat="server" TargetControlID="txt_menutype"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_menutype" runat="server" Text="Go" CssClass="textbox btn" OnClick="btn_menutype_Click" />
                                    </td>
                                    <td>
                                        <span>Menu Name </span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="menusearch" runat="server" CssClass="textbox textbox1" placeholder="Search Menu Name"
                                            AutoPostBack="true" OnTextChanged="menusearch_txtchange"></asp:TextBox>
                                        <br />
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="menusearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <div style="overflow: auto; width: 750px; height: 362px; border-radius: 10px; border: 1px solid Gray;">
                            <br />
                            <asp:DataList ID="gvdatass" runat="server" Font-Size="Small" RepeatColumns="3" Width="600px"
                                ForeColor="#333333">
                                <AlternatingItemStyle BackColor="White" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkup3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMenuname" ForeColor="Red" runat="server" Text='<%# Eval("MenuName") %>'></asp:Label>
                                                <asp:Label ID="lblmenucode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("MenuCode") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblmenuid" Visible="false" ForeColor="BlueViolet" Width="156px" runat="server"
                                                    Text='<%# Eval("MenuMasterPK") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataList>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btnmenusave" Text="Save" runat="server" Width="60px" Height="30px"
                                CssClass="textbox" OnClick="btnmenusave_click" />
                            <asp:Button ID="btnmenuexit" Text="Exit" runat="server" Width="60px" Height="30px"
                                CssClass="textbox" OnClick="btnmenuexit_click" />
                        </center>
                    </div>
                </div>
                <div id="popwindow1" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 9px; margin-left: 397px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <div style="background-color: White; height: 570px; width: 817px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <asp:Label ID="lblpop2selectitem" runat="server" Style="font-size: large; color: Green;"
                                Text="Select the Item Name"></asp:Label>
                        </div>
                        <br />
                        <center>
                            <asp:UpdatePanel ID="upp4" runat="server">
                                <ContentTemplate>
                                    <table class="maintablestyle" style="margin-left: 0px; position: absolute; width: 800px;
                                        height: 49px;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpop2itemtype" runat="server" Style="top: 15px; left: 42px; position: absolute;
                                                    font-family: 'Book Antiqua'" Text="Item Header"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpop2itemheader" runat="server" Style="top: 10px; left: 138px;
                                                    position: absolute; font-family: 'Book Antiqua'" CssClass="textbox" ReadOnly="true"
                                                    Width="106px" Height="20px">--Select--</asp:TextBox>
                                                <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 180px; width: 160px;">
                                                    <asp:CheckBox ID="chk_pop2itemheader" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="chkitemheader" />
                                                    <asp:CheckBoxList ID="chklst_pop2itemheader" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemheader">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txtpop2itemheader"
                                                    PopupControlID="p5" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name" Style="top: 15px;
                                                    left: 266px; position: absolute;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                            ReadOnly="true" Width="120px" Style="top: 10px; left: 389px; position: absolute;
                                                            font-family: 'Book Antiqua'">--Select--</asp:TextBox>
                                                        <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                            height: 190px;">
                                                            <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                                Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                                            PopupControlID="Panel5" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblpop2itemname" runat="server" Style="top: 15px; left: 530px; position: absolute;
                                                    font-family: 'Book Antiqua'" Text="Item Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Upp5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtpop2itemname" runat="server" Style="top: 10px; left: 618px; position: absolute;
                                                            font-family: 'Book Antiqua'" CssClass="textbox" ReadOnly="true" Width="106px"
                                                            Height="20px">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 180px; width: 160px;">
                                                            <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" AutoPostBack="true"
                                                                OnCheckedChanged="chkitemtyp" />
                                                            <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemtyp">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txtpop2itemname"
                                                            PopupControlID="p51" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="gobtn" runat="server" Style="top: 10px; left: 747px; position: absolute;
                                                    font-family: 'Book Antiqua'" CssClass="textbox btn" Text="Go" OnClick="gobtn_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="gobtn" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </center>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblerrornew" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                        <span>Item Name </span>
                        <asp:TextBox ID="searchitem" runat="server" CssClass="textbox textbox1" placeholder="Search Item Name"
                            AutoPostBack="true" OnTextChanged="itemsearch_txtchange"></asp:TextBox>
                        <br />
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getitem" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="searchitem"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                        <br />
                        <div id="div2" runat="server" visible="false" style="width: 668px; height: 322px;
                            overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;"
                            class="table">
                            <br />
                            <asp:DataList ID="gvdatass1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                RepeatColumns="4" Width="600px" ForeColor="#333333">
                                <AlternatingItemStyle BackColor="White" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblitemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                <asp:Label ID="lblitemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                <asp:Label ID="lblitemunit" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                                <asp:Label ID="lblitempk" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Itempk") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                    Text='<%# Eval("ItemHeaderName") %>'></asp:Label>
                                                <asp:Label ID="lblitemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                    Text='<%# Eval("ItemHeaderCode") %>'></asp:Label>
                                                <asp:Label ID="lblmeasureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataList>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btnitemsave" runat="server" Text="Save" CssClass="textbox btn1" Width="60px"
                                OnClick="btnitemsave_Click" />
                            <asp:Button ID="btnconexist" runat="server" Text="Exit" CssClass="textbox btn1" Width="60px"
                                OnClick="btnconexist_Click" />
                        </center>
                    </div>
                </div>
                <div id="popwindow" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 437px;"
                        OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <div class="sty2" style="background-color: White;">
                        <br />
                        <div>
                            <asp:Label ID="lblpopmenuitemmaster" runat="server" Style="font-size: large; color: Green;"
                                Text="Cleaning Item Master"></asp:Label>
                        </div>
                        <br />
                        <div style="width: 900px; height: 500px;">
                            <div style="float: left; width: 850px; height: 300px; border: 1px solid Gray; border-radius: 10px;
                                margin-left: 10px; overflow: auto;">
                                <br />
                                <center>
                                    <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                        HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound"
                                        OnRowCommand="SelectdptGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbselect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Measure">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtquantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                        Width="80px" CssClass="textbox" onblur="checkDecimal(this);"></asp:TextBox>
                                                    <%--  <input type="text" id="myInput"  onkeyup="check()" />--%>
                                                    <%-- <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ValidationExpression="^((\d{5})*|([1-9]\d{0,5}))(\.\d{0,2})?$"
                                                    ControlToValidate="txtquantity" Text="Input must be 123456.78 format." Display="Dynamic" />--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtquantity"
                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item_pk" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitempk" runat="server" Text='<%# Eval("itempk") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </center>
                                <br />
                            </div>
                            <div style="float: left; width: 900px; height: 100px; margin-top: 20px;">
                                <center>
                                    <asp:Button ID="btnadd_item" runat="server" Text="Remove" Width="80px" CssClass="textbox btn1"
                                        OnClientClick="return valid1()" OnClick="btnadd_item_Clcik" />
                                    <asp:Button ID="btnupdate" runat="server" Text="Update" Width="80px" CssClass="textbox btn1"
                                        OnClientClick="return valid()" OnClick="btnupdate_Click" Visible="false" />
                                    <asp:Button ID="btndelete" runat="server" Text="Delete" Width="80px" CssClass="textbox btn1"
                                        OnClientClick="return valid()" OnClick="btndelete_Click" Visible="false" />
                                    <asp:Button ID="btnpopsave" runat="server" Text="Save" CssClass="textbox btn1" Width="60px"
                                        Visible="false" OnClick="btnpopsave_Clcik" OnClientClick="return valid1()" />
                                    <asp:Button ID="btnpopexit" runat="server" Text="Exit" CssClass="textbox btn1" Width="60px"
                                        OnClick="btnpopexit_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                                <%-- <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                ImageUrl="~/images/okimg.jpg" runat="server" />--%>
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
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
