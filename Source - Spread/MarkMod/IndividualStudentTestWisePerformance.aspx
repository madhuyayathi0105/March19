<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="IndividualStudentTestWisePerformance.aspx.cs"
    Inherits="MarkMod_IndividualStudentTestWisePerformance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <center>
        <div>
            <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Individual
                Student Academic Performance</span>
        </div>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
            position: relative;">
            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlCollege"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlBatch"></asp:Label>
                   
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                            AssociatedControlID="ddlDegree"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                            AssociatedControlID="ddlBranch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem" AssociatedControlID="ddlSem"></asp:Label>
                         <asp:DropDownList ID="ddlSem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                            AutoPostBack="True" Width="40px">
                        </asp:DropDownList>
                    </td>
                    
                    
                </tr>
               
               <tr> 
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Section" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlSec"></asp:Label>
                    </td>
                     <td>
                     <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlSec" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
                   
                         
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto; margin-left: 40px" />
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlformat" runat="server" CssClass="commonHeaderFont" Width="80px" style="margin-left: 38px" OnSelectedIndexChanged="ddlformat_OnSelectedIndexedChanged" AutoPostBack="true">
                        <asp:ListItem>Format-I</asp:ListItem>
                         <asp:ListItem>Report Card</asp:ListItem>
                        </asp:DropDownList>
                    </td><td>
                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="ddlTest" style="margin-left: 5px;" ></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTest" Visible="false" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlTest" Visible="false" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkTest" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkTest_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblTest" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblTest_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtTest" runat="server" TargetControlID="txtTest"
                                        PopupControlID="pnlTest" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlTest" runat="server"  CssClass="commonHeaderFont" 
                                        Width="80px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    
                    <td>
                    <asp:UpdatePanel ID="btnprintUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btnPrint" CssClass="textbox textbox1 commonHeaderFont" Visible="false"
                            runat="server" OnClick="btnPrint_Click" Text="Print" Style="width: auto; height: auto;" />

                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                     
                    
                    <td>
                        <asp:Label ID="lblconvertions" runat="server" Text="Convert" CssClass="commonHeaderFont" ></asp:Label>
                    
                        <asp:TextBox ID="txt_Convertion" Width="76px" runat="server" MaxLength="3" CssClass="textbox  txtheight2 commonHeaderFont" ></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filterConvert" runat="server" TargetControlID="txt_Convertion"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                      <fieldset id="fve" runat="server" style="height: 14px; width:200px;">
                     <asp:CheckBox ID="chkMail" runat="server" Text="Send Mail" CssClass="commonHeaderFont" AutoPostBack="True"
                            OnCheckedChanged="rdbtnMailSend_CheckedChanged" />
                             <asp:CheckBox ID="chkSMS" runat="server" Text="Send SMS" CssClass="commonHeaderFont" AutoPostBack="True"
                            OnCheckedChanged="rdbtnSMSSend_CheckedChanged" />
                            </fieldset>
                    </td>
                     <td>
                      <fieldset id="Fieldset1" runat="server" style="height: 14px; width:200px;">
                       <asp:CheckBox ID="chkFatherSms" runat="server" Text="Father" CssClass="commonHeaderFont" AutoPostBack="True"
                            OnCheckedChanged="rdbtnsmsSendtoF_CheckedChanged" />
                             <asp:CheckBox ID="chkMotherSms" runat="server" Text="Mother" CssClass="commonHeaderFont" AutoPostBack="True"
                            OnCheckedChanged="rdbtnsmsSendtoM_CheckedChanged" />
                               
                               </fieldset>
                        <%--<asp:Button ID="btnSend" CssClass="textbox textbox1 commonHeaderFont" Visible="false"
                            runat="server" OnClick="btnSend_Click" Text="Send" Style="width: auto; height: auto;" />--%>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2px" >
                   <%-- <asp:Button ID="btnsettings" runat="server" CssClass="textbox textbox1 commonHeaderFont" OnClick="btnsettings_OnClick" Text="Signature Settings" Style="width: auto; height: auto;" />--%>
                   <asp:LinkButton ID="btnsettings" runat="server" CssClass="textbox textbox1 commonHeaderFont" OnClick="btnsettings_OnClick" Text="Signature Settings" Style="width: auto; height: auto;"></asp:LinkButton>
                    </td>
                    <td>
                       <asp:CheckBox ID="chkRoundOffMarks" runat="server" Checked="false" CssClass="commonHeaderFont"
                            Text="Round off Highest Mark" />
                    </td>
                    </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px;"></asp:Label>
            <div id="ShowReport" runat="server" visible="false">
                
                    
                

               
                              <center>
           
                            
                     <center>
                     <div id="divgrd" runat="server" style ="height:700px; width:600px; overflow:auto; border: solid 1px black;">
                            <asp:GridView ID="grdover" runat="server" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                            ShowFooter="false" ShowHeader="true">
                            <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                     <asp:CheckBox ID="chkselectall" runat="server" Width="30px" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged"></asp:CheckBox>
                                    <asp:CheckBox ID="lbl_cb" runat="server" Width="30px" ></asp:CheckBox>
                                </center>
                                    <asp:Label ID="lblappno" runat="server" Visible="false" Width="30px"></asp:Label>
                                    
                                     
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Control" />
                            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>
                        </div>
                        </center>   
                    
                </center>
            </div>
        </div>
    </center>
    <%-- Confirmation --%>
    <center>
        <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divConfirm" runat="server" class="table" style="background-color: White;
                    height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%; padding: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnYes_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnNo_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Alert Box --%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
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
        <div id="divsettings" runat="server" visible="false" style="height: 550em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divsignsettings" runat="server" class="table" style="background-color: White;
                    height: auto; width: 34%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 32%; right: 30%; top: 30%; position: fixed; border-radius: 10px;">
                     <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Signature Settings</span>
                    </center>
                    <center>
                        <table style="height: auto; width: 100%; padding: 5px;">
                            <tr>
                                <td > 
                                <asp:Label ID="lblfooter1" runat="server" Text="Left Footer" style="margin-left:24px"></asp:Label>
                                <asp:TextBox ID="txtfooter1" runat="server"  CssClass="textbox  txtheight2 commonHeaderFont"
                                        ></asp:TextBox>
                                </td>
                                <td > 
                                <asp:Label ID="lblfooter2" runat="server" Text="Middle Footer" style="margin-left:17px"></asp:Label>
                                <asp:TextBox ID="txtfooter2" runat="server"   CssClass="textbox  txtheight2 commonHeaderFont"
                                       ></asp:TextBox>
                                </td>
                                <td > 
                                <asp:Label ID="lblfooter3" runat="server" Text="Right Footer" style="margin-left:20px"></asp:Label>
                                <asp:TextBox ID="txtfooter3" runat="server"   CssClass="textbox  txtheight2 commonHeaderFont"
                                       ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3px">
                                    <center>
                                        <asp:Button ID="btnsavefooter" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px; margin-top:15px"
                                            OnClick="btnsavefooter_OnClick" Text="Save" runat="server" />
                                        <asp:Button ID="btnClosefooter" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px; margin-top:15px"
                                            OnClick="btnClosefooter_OnClick" Text="Close" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 944px;" visible="false">
        </div>
    </div>

       </ContentTemplate>
        
    </asp:UpdatePanel>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="btnprintUpdatePanel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    
</asp:Content>
