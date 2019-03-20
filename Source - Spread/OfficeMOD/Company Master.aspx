<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="Company Master.aspx.cs" Inherits="Company_Master" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .popupheight3
            {
                height: 55em;
            }
            .email
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
            .watermark
            {
                color: #999999;
            }
        </style>
        <script type="text/javascript">
            function valid1() {
                var idval = "";
                var empty = "";

                idval = document.getElementById("<%=txt_connam.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_connam.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_designation.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_designation.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_conmob.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_conmob.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txt_vendorname1.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_vendorname1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_street.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_street.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_city.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_city.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
               
                
               


                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function checkEmail(id) {
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }
             function QuantityChange() {
             
              var txtyear = 0;
              var oldyear = 0;
              var d = new Date();
             
              var year2 = d.getFullYear();
              
              var lblAmt = document.getElementById('<%=txt_startyear.ClientID %>').value;
              txtyear = lblAmt;
           
              if (txtyear != "") {
                 
                  
                 if (year2 >= txtyear) {
                    
            }
             else {
                


                 document.getElementById("<%=txt_startyear.ClientID %>").value = "";
                 alert("Please Enter Valid Year");
            }
               }
             }
             
        </script>
    </head>
 <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span style="color: Green;" class="fontstyleheader">Company Master</span>
                <br />
                <br />
            </div>
        </center>
        
           <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                    <center>
            <div class="maindivstyle" style="height: 800px; width: 1000px;">
                <br />
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_companyName" runat="server" Text="Company Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_companyname" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pvendorname" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_companynamee" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_companyname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_companyname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_companyname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_companyname"
                                            PopupControlID="pvendorname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                             <td>
                                <asp:Label ID="lbl_search" runat="server" Text="Search By Company Name"></asp:Label>

                               
                                <asp:TextBox ID="txt_companyname2" Visible="true" runat="server" placeholder="Search Company Name"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_companyname2"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" runat="server" CssClass="textbox btn2" Text="Add New" OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <center>
                        <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red;"></asp:Label></center>
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                    </div>
                    <br />
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="890px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="true" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                            Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tborder" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                            <%--<asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>--%>
                                       
                                            <asp:ListItem Value="CompName" Selected="True">Company Name</asp:ListItem>
                                            <asp:ListItem Value="CompanyAddress" Selected="True">Street</asp:ListItem>
                                            <asp:ListItem Value="CompanyCity">City</asp:ListItem>
                                            <asp:ListItem Value="CompanyPin">Pincode</asp:ListItem>
                                            <asp:ListItem Value="CompanyPhoneNo">Phone No</asp:ListItem>
                                            <asp:ListItem Value="CompanyFaxNo">Fax No</asp:ListItem>
                                            <asp:ListItem Value="CompanyEmailID">Mail Id</asp:ListItem>
                                            <asp:ListItem Value="CompanyWebsite">Website</asp:ListItem>
                                            <asp:ListItem Value="CompanyDist">District</asp:ListItem>
                                            <asp:ListItem Value="CompanyState">State</asp:ListItem>
                                            <asp:ListItem Value="CompanyMobileNo">Mobile No</asp:ListItem>
                                            <asp:ListItem Value="CompanyPANNo">PAN</asp:ListItem>
                                            <asp:ListItem Value="CompanyStartYear">Business Start Year</asp:ListItem>
                                          
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                        ExpandedImage="~/images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                </div>
                 <div id="div1" runat="server" visible="false" style="width: 950px; height: 350px;
                    box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                    <br />
                      <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="gview" runat="server" ShowHeader="false" style="Width:800px;" OnSelectedIndexChanged="gview_onselectedindexchanged" OnRowCreated="OnRowCreated">
                            <Columns>
                            
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="White" Font-Bold="True" />
                         
                        </asp:GridView>
                </div>
                 <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please enter the report name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" CssClass="textbox textbox1 txtheight5"
                        onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=". ">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn2"
                        Text="Export To Excel" Width="127px" Height="30px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" Height="30px" CssClass="textbox btn2" />
                  
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
           <center>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight3">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 9px; margin-left: 437px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; height: 700px; width: 900px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green; font-size: large;">Company Details</span>
                            </center>
                        </div>
                        <br />
                        <div style="float: left; width: 900px; height: 266px;">
                            <center>
                                <table >
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lbl_Code" runat="server" Text="Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_code" CssClass="textbox textbox1 txtheight1" Width="100px" Enabled="false"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_vendor" runat="server" Text="Supplier" GroupName="same" />
                                            <asp:RadioButton ID="rdb_customer" runat="server" Visible="false" Text="Customer"
                                                GroupName="same" />
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_vendorname1" runat="server" Text="Company Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vendorname1" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="224px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_vendorname1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .&/#">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>

                                             <td>
                                            <asp:Label ID="lbl_Phone" runat="server" Text="Phone No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_phn" MaxLength="13" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_phn"
                                                FilterType="Numbers,custom" ValidChars="- ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                     
                                    </tr>
                                    <tr>
                                       <td>
                                            <asp:Label ID="lbl_street" runat="server" Text="Street"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_street" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="224px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_street"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_mobileno" runat="server" Text="Mobile No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mainmobileno" MaxLength="10" CssClass="textbox textbox1 txtheight1"
                                                Width="200px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_mainmobileno"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                      
                                     
                                    </tr>
                                    <tr>
                                      <td>
                                            <asp:Label ID="lbl_City" runat="server" Text="City"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_city" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="224px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_city"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>

                                           <td>
                                            <asp:Label ID="lbl_email" runat="server" Text="Email Id"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_email" CssClass="email textbox1 txtheight1" Width="200px" runat="server"
                                                onfocus="return myFunction(this)" onblur="return checkEmail(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_email"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    
                                     
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_state" runat="server" Text="State"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_State" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_State_Selectindexchange" Width="234px">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txt_state" CssClass="textbox textbox1 txtheight1" Width="75px" Visible="false"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txt_state"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                     <td>
                                            <asp:Label ID="lbl_web" runat="server" Text="Website"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_web" CssClass="textbox textbox1 txtheight1" Width="200px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_web"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=".@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                   
                                        
                                    </tr>
                                    <tr>
                                       <td>
                                            <asp:Label ID="lbl_district" runat="server" Text="District"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_district" CssClass="textbox textbox1 txtheight1" Width="75px"
                                                runat="server" Visible="false"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_district"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="ddl_district" runat="server" CssClass="textbox textbox1 ddlheight5" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                  
                                  
                                <td>
                                            <asp:Label ID="lbl_pan" runat="server" Text="PAN No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pan" MaxLength="8" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_pan"
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                      
                                    </tr>
                                    <tr>
                                         <td>
                                            <asp:Label ID="lbl_pin" runat="server" Text="PinCode"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pin" CssClass="textbox textbox1 txtheight1" MaxLength="6" Width="224px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_pin"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                                <td>
                                            <asp:Label ID="Label10" runat="server" Text="Business Start Year"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_startyear" runat="server" CssClass="textbox textbox1 txtheight"
                                                MaxLength="4" AutoPostBack="true" onchange="return QuantityChange()" ></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txt_startyear"
                                                FilterType="Numbers"><%--OnTextChanged="txtyear_Onchange"--%>
                                            </asp:FilteredTextBoxExtender>
                                            <asp:TextBox ID="oldyeartxt" Visible="false" Text="1900" CssClass="textbox textbox1 txtheight"
                                                Width="75px" runat="server"></asp:TextBox>
                                            <%-- <asp:DropDownList ID="ddlbis" runat="server" CssClass="textbox textbox1" Width="100px">
                                        </asp:DropDownList>--%>
                                        </td>
      
                                        </tr>
                                    <tr id ="com" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="lbl_cst" runat="server" Text="CST No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cst" MaxLength="13" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_cst"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_tin" runat="server" Text="TIN No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_tin" MaxLength="20" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_tin"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fax" runat="server" Text="Fax No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfax" MaxLength="20" CssClass="textbox textbox1 txtheight1" Width="224px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtfax"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td> 
                                      
                                       
                                      
                                    </tr>
                                   
                                </table>
                            </center>
                        </div>
                           <div style="float: left; width: 450px; height: 220px;">
                            <fieldset id="fildset" runat="server">
                                <legend>Department Select
                                    <asp:Button ID="btncontant" runat="server" Text="?" OnClick="btnitm_click" CssClass="textbox btn" />
                                </legend>
                                <asp:Panel ID="Panelbind" runat="server" ScrollBars="Auto" Style="height: 150px;
                                    width: 408px;">
                                    <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White" OnDataBound="OnDataBound" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Course">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_course"  runat="server"  Text='<%# Eval("course name") %>'></asp:Label>
                                                      <asp:Label ID="lbl_coursecode" runat="server" Visible="false" Text='<%# Eval("course Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_bran" runat="server"  Text='<%# Eval("degree Name") %>'></asp:Label>
                                                    <asp:Label ID="lbl_branchcode" runat="server" Visible="false" Text='<%# Eval("degree Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DeptName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_deptname" runat="server" Text='<%# Eval("Dept Name") %>'></asp:Label>
                                                    <asp:Label ID="lbl_deptcode" runat="server" Visible="false" Text='<%# Eval("Dept Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                        </div>
                  
                  <div id="feil" style="float: left; width: 450px; height: 220px;">
                       
                            <fieldset id="dgg" runat="server">
                                <legend>Contact Details
                                    <asp:Button ID="btnitm" runat="server" Text="?" OnClick="btncontact_click" CssClass="textbox btn" />
                                </legend>
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 150px; width: 408px;">
                                    <asp:GridView ID="ContactGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound" OnRowCommand="ContactGrid_RowCommand" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_designation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_phoneno" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_mobileno" runat="server" Text='<%# Eval("Mobile No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fax No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_faxno" runat="server" Text='<%# Eval("Fax No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                     
                     </div>
                          
                   <br />
                 
                        <center>
                       
                       
                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                OnClientClick="return valid2()" OnClick="btn_update_Click" Visible="false" BackColor="#c288d8" />
                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                OnClientClick="return valid2()" OnClick="btn_delete_Click" Visible="false" BackColor="#8ae02d" />
                            <asp:Button ID="btn_save" runat="server" Text="Save" Visible="false" OnClick="btn_save_Click"
                                CssClass="textbox btn2" OnClientClick="return valid2()" BackColor="#0ce3f3" />

                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click"  BackColor="#0cf337"/>
                           </center>
                        

                    </div>
                </center>
               
            </div>
        </center>
        <center>
          <div id="popitm" runat="server" visible="false" style="height: 48em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 47px; margin-left: 457px;"
                            OnClick="imagebtnpopclose3_Click" />
                        <br />
                        <br />
                        <br />
         <div style="background-color: White; height: 500px; width: 943px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <br />
                            <center>
                                <span style="color: Green; font-size: large;">Department</span>
                            </center>
                            <br />
                            <br />
                            <br />
                            <table>
                                <tr>
                                   
                                    <td>
                                        <asp:Label ID="lblcourse" runat="server" Text="Course"></asp:Label>
                                        <span style="color: Red;">*</span>
                                        <fieldset>
                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="height: 107px; width: 198px;">
                                                <asp:CheckBoxList ID="cblcourse" runat="server" OnSelectedIndexChanged="cblcourse_ChekedChange" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                                                        <td>
                                        <asp:Label ID="lbl_branch" runat="server" Text="degree"></asp:Label>
                                        <span style="color: Red;">*</span>
                                        <fieldset>
                                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 107px; width: 198px;">
                                                <asp:CheckBoxList ID="cbldegree" runat="server" OnSelectedIndexChanged="cbldegree_ChekedChange" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department" runat="server" Text="Branch"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px; width: 210px;">
                                                <asp:CheckBoxList ID="cbldepartment" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                   
                                     <td>
                                        <asp:CheckBox ID="cb_course" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_course_ChekedChange" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_degree" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_degree_ChekedChange" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_departemt" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cbdepartment_Change" />
                                    </td>
                                </tr>
                              
                            </table>
                            <br />
                            <br />
                            <br />
                            <br />
                            <center>
                                <asp:Button ID="btn_save1" runat="server" Text="Save" OnClick="btn_save1_Click" CssClass="textbox btn2" />
                                <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                            </center>
                        </div>
                        </div>
            <div id="popcon" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 93px; margin-left: 343px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; width: 700px; height: 400px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <br />
                    <center>
                        <span style="color: Green; font-size: large;">Contact Details</span>
                    </center>
                    <br />
                    <br />
                    <center>
                        <table>
                            <tr style="display: none;">
                                <td>
                                    <asp:Label ID="lbl_contyp" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_contyp" runat="server" AutoPostBack="true" 
                                        CssClass="textbox cont">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">Others</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_contyp" CssClass="textbox textbox1" Style="color: #000066;"
                                        Width="75px" Visible="false" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txt_contyp"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conname" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_connam" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                        Width="250px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_connam"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_conph" runat="server" Text="Phone No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conpn" MaxLength="13" CssClass="textbox textbox1 txtheight1"
                                        Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_conpn"
                                        FilterType="Numbers,custom" ValidChars="- ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_designation" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_designation" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                        Width="250px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_designation"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conmob" runat="server" Text="Mobile No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conmob" MaxLength="10" CssClass="textbox textbox1 txtheight1"
                                        onfocus="return myFunction(this)" Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_conmob"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_confax" runat="server" Text="Fax No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_confax" MaxLength="20" CssClass="textbox textbox1 txtheight1"
                                        Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_confax"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conemail" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conmail" CssClass="email textbox1 txtheight1" Width="150px"
                                        runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_conmail"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".@ ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Button ID="btn_congo" runat="server" Text="Save" OnClick="btn_congo_Click" CssClass="textbox btn2"
                                OnClientClick="return valid1()" />
                            <asp:Button ID="btn_conexist" runat="server" Text="Exit" CssClass="textbox btn2"
                                OnClick="btn_conexit_Click" />
                        </center>
                    </center>
                </div>
            </div>
        </center>
          <center>
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
        </center>
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
        </center></ContentTemplate>
         <Triggers>
        <asp:PostBackTrigger ControlID="btnExcel" />
         </Triggers>
        </asp:UpdatePanel>
  </form>
    </body>
    </html>
</asp:Content>

