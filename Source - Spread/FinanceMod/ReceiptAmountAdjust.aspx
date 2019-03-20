<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ReceiptAmountAdjust.aspx.cs" Inherits="ReceiptAmountAdjust" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Receipt Amount Adjust</span></div>
        </center>
        <center>
            <div id="maindiv" runat="server" style="width: 950px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbrcptmode" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rbrcptmode_Selected">
                                    <asp:ListItem Text="Receipt Paymode" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Receipt Amount" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <fieldset>
                                    <asp:RadioButtonList ID="rbladmission" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="After Admission" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Before Admission" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--receipt paymode adjust--%>
                <div id="divpaymode" runat="server" visible="false">
                    <div>
                        <table style="float: left;">
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="Label2" runat="server" Text="Receipt No" Style="font-family: Book Antiqua;
                                        font-size: medium;"></asp:Label>
                                    <asp:TextBox ID="txtfrcptno" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txtfrcptno_Changed"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="tddet" runat="server" visible="false">
                                    <fieldset>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Admission No:" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblroll" runat="server" Text="" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text=" Name:" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblstudname" runat="server" Text="" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Batch:" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblbatch" runat="server" Text="" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Department:" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbldept" runat="server" Text="" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" Text="School:" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblclg" runat="server" Text="" Style="font-family: Book Antiqua; font-size: medium;"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <table style="float: left;">
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <asp:GridView ID="gdfrcpt" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                        CssClass="grid-view" HeaderStyle-BackColor="#0CA6CA" BackColor="WhiteSmoke" Style="width: 600px;
                                        height: 250px; overflow: auto;" OnRowDataBound=" gdfrcpt_OnRowDataBound">
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="left: 50%; top: 25%; position: absolute;">
                        <table>
                            <tr>
                                <td>
                                    <fieldset id="tblpayfld" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    PaymentMode
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upd_paid" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Style="width: 126px; height: 160px;">
                                                                <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                                                PopupControlID="pnl_paid" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlpaymode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlpaymode_OnSelected"
                                                        CssClass="textbox  ddlheight" Style="width: 108px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset id="tblpaymode" runat="server" visible="false">
                                        <table>
                                            <%--style="left: 50%; top: 26%;   position: absolute;"--%>
                                            <tr id="div_cheque" runat="server" visible="false">
                                                <td>
                                                    <span class="challanLabel">
                                                        <p>
                                                            Bank</p>
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_bkname" runat="server" CssClass="textbox ddlheight2" onchange="return otherBank(this);"
                                                        onfocus="myFunction(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_other" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction(this)"
                                                        Placeholder="Other Bank" Style="display: none;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                                        ValidChars=" " TargetControlID="txt_other">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_branch" runat="server" Placeholder="Branch" CssClass="textbox txtheight2"
                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_chqno" runat="server" Placeholder="Cheque No" CssClass="textbox txtheight2"
                                                        onfocus="myFunction(this)" Visible="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_ddno" runat="server" Placeholder="DD No" CssClass="textbox txtheight2"
                                                        onfocus="myFunction(this)" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="div_sndcheque" runat="server" visible="false">
                                                <td>
                                                    <span class="challanLabel">
                                                        <p>
                                                            Date</p>
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_date1" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_date1" runat="server"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txt_ddnar" runat="server" Placeholder="Narration" Visible="false"
                                                        CssClass="textbox txtheight2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="div_card" runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="lblCardName" runat="server" Text="Card Type" Style="float: left; padding: 2px;
                                                        padding-top: 5px;"></asp:Label>
                                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="textbox ddlheight2" onchange="return otherCardType(this);"
                                                        Style="float: left;" onfocus="myFunction(this)">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtCardType" runat="server" MaxLength="40" CssClass="textbox txtheight2"
                                                        onfocus="return myFunction(this)" Placeholder="Other Cards" Style="display: none;
                                                        float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                                        ValidChars=" " TargetControlID="txtCardType">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Label ID="lblLast4No" runat="server" Text="Card's Last Four Digits" Style="float: left;
                                                        padding: 2px; padding-top: 5px;"></asp:Label>
                                                    <asp:TextBox ID="txtLast4No" runat="server" Placeholder="XXXX" CssClass="textbox txtheight"
                                                        MaxLength="4" onblur="if(this.value.length!=4)this.value='';" Width="35px" Style="float: left;"
                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fceCardTxt" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtLast4No">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnpaysave" CssClass=" textbox btn1 comm" Visible="false" Style="height: 28px;
                                                            width: 65px;" OnClick="btnpaysave_Click" Text="Save" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%--receipt amount adjust--%>
                <div style="float: left;" id="divamount" runat="server" visible="false">
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblrcpt" runat="server" Text="Receipt No" Style="font-family: Book Antiqua;
                                    font-size: medium;"></asp:Label>
                                <asp:TextBox ID="txtrcpt" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txtrcpt_Changed"
                                    AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="fldrcpt" visible="false" runat="server" style="height: 310px; width: auto;
                                    overflow: auto; float: left;">
                                    <table>
                                        <tr>
                                            <td colspan="5">
                                                <asp:GridView ID="gdrcpt" runat="server" Visible="false" AutoGenerateColumns="false"
                                                    GridLines="Both" CssClass="grid-view" BackColor="WhiteSmoke" OnRowDataBound="gdrcpt_OnRowDataBound"
                                                    Style="width: 400px; height: 250px; overflow: auto;">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'>
                                                                    </asp:Label>
                                                                    <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%#Eval("appno") %>'>
                                                                    </asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:CheckBox ID="cbsel" runat="server" />
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="140px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblledgerfk" Visible="false" runat="server" Text='<%#Eval("ledgerfk") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblroll" runat="server" Text='<%#Eval("Ledger Name") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblfeecat" Visible="false" runat="server" Text='<%#Eval("feecat") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Paid amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpaidamt" runat="server" Text='<%#Eval("PaidAmount") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblfinyr" Visible="false" runat="server" Text='<%#Eval("finyearfk") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbltransdate" Visible="false" runat="server" Text='<%#Eval("transdate") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lblpaymode" Visible="false" runat="server" Text='<%#Eval("paymode") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblledg" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Ledger"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtledger" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlledger" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Style="width: 300px; height: 180px;">
                                                        <asp:CheckBox ID="cbledger" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cbledger_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledger_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtledger"
                                                        PopupControlID="pnlledger" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsem" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_Selcted">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnadjust" runat="server" Text="Adjust" CssClass="textbox btn2" Width="56px"
                                                OnClick="btnadjust_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divledger" runat="server" visible="false">
                    <table>
                        <tr>
                            <td>
                                <fieldset style="height: 310px; width: auto; overflow: auto; float: left;">
                                    <table>
                                        <tr>
                                            <td colspan="5">
                                                <asp:GridView ID="gdledger" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                    CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lbsno" runat="server" Text='<%#Eval("Sno") %>'>
                                                                    </asp:Label>
                                                                    <asp:Label ID="lbappno" Visible="false" runat="server" Text='<%#Eval("appno") %>'>
                                                                    </asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbheaderfk" Visible="false" runat="server" Text='<%#Eval("headerfk") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbledgerfk" Visible="false" runat="server" Text='<%#Eval("ledgerfk") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbledgername" runat="server" Text='<%#Eval("Ledger Name") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbfeecat" Visible="false" runat="server" Text='<%#Eval("feecat") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Allot" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbtotamt" runat="server" Text='<%#Eval("TotalAmount") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtpaid" runat="server" CssClass="txtheight3 txtcaps" Text='<%#Eval("PaidAmount") %>'
                                                                    Width="94px" ReadOnly="true"></asp:TextBox><%--OnTextChanged="txtpaid_Changed" AutoPostBack="true"--%>
                                                                <asp:Label ID="lbloldamt" runat="server" Visible="false" Text='<%#Eval("PaidAmount") %>'></asp:Label>                                                              
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Be Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txttobepaid" runat="server" CssClass="txtheight3 txtcaps" Text='<%#Eval("ToBePaid") %>'
                                                                    OnTextChanged="txtpaid_Changed" AutoPostBack="true" Width="94px"></asp:TextBox>                                                              
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbbalamt" runat="server" Text='<%#Eval("BalAmount") %>'>
                                                                </asp:Label>
                                                                <asp:Label ID="lbfinyearfk" Visible="false" runat="server" Text='<%#Eval("finyearfk") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Total Select Amt:</span>
                                <asp:Label ID="lbltotselamt" runat="server" Text="">
                                </asp:Label>&nbsp;&nbsp; <span>Adjust Amount</span>
                                <asp:Label ID="lbladjamt" runat="server" Text="">
                                </asp:Label>
                                <asp:Label ID="lbloldAmt" runat="server" Visible="false" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnsave" runat="server" Text="Save" Visible="false" CssClass="textbox btn2"
                                    Width="56px" OnClick="btnsave_Click" />
                            </td>
                        </tr>
                    </table>
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
    </div>
</asp:Content>
