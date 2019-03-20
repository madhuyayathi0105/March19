<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ModurationApply.aspx.cs" Inherits="ModurationApply" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script type="text/javascript">
      function PrintDiv() {
          var panel = document.getElementById("<%=contentDiv.ClientID %>");
          var printWindow = window.open('', '', 'height=auto,width=685');
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Moderation Apply</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
          <asp:Panel ID="pnl_filter" runat="server" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <table class="maintablestyle" style="height: auto; width: auto;">
            <tr>
             <td  colspan="3">
                     <asp:CheckBox ID="chkindividual" runat="server" Text="Individual" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"  AutoPostBack="true" OnCheckedChanged="chkindividual_CheckedChanged"  />
                    
                        <asp:CheckBox ID="chkCommon" runat="server" Text="Common" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkCommon_CheckedChanged"  />
                             <asp:CheckBox ID="chkMultiple" runat="server" Text="Multiple Subject" Font-Bold="True" 
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkMultiple_CheckedChanged"  />
                    </td>
            </tr>
                <tr>
                    <td colspan="10">
                    <table>
                    <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px" ></asp:Label>
                          </td>
                          <td> 
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                          </td>
                          <td> 
                        <asp:Label ID="lblyrmon" runat="server" Text="Year and Month" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="130px" Style="" ></asp:Label>
                              </td>
                          <td> 
                        <asp:DropDownList ID="ddlyear" runat="server"  Width="57px" Font-Names="Book Antiqua"
                            Style="" Font-Bold="true"
                            Font-Size="Medium" Height="25px">
                        </asp:DropDownList>
                          </td>
                          <td> 
                        <asp:DropDownList ID="ddlmonth" runat="server"  Width="58px" Style="" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Height="25px">
                        </asp:DropDownList>
                       </td>
                          <td> 
                          <div ID="UpdatePanel24" runat="server" visible="false">
                            <asp:CheckBox ID="ChkBundlewise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="BundleNo" AutoPostBack="true" OnCheckedChanged="chkBundleNo_CheckedChanged" />
                      <%--   <asp:Label ID="lblBundleNo" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Bundle No"></asp:Label>--%>

                            <asp:TextBox ID="txtBundleNo" runat="server" placeholder="Bundle No" CssClass="textbox  txtheight2"></asp:TextBox>  <%--OnTextChanged="txtroll_staff_Changed" AutoPostBack="true"--%>

                         <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getbundleno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtBundleNo"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </div>
                   </td>
                          <td> 
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                              </td>
                          <td> 
                     <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"
                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                          </td>
                          <td> 
                          <div id="div2" style="position: relative;" runat="server" visible="false">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                         </td>
                          <td> 
                        <asp:Label ID="lblDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Degree"></asp:Label>
                              </td>
                          <td> 
                        <asp:DropDownList ID="ddldegree1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"
                            OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                          </td>
                          <td> 
                         <div id="div3" style="position: relative;" runat="server" visible="false">
                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                         </td>
                          <td> 
                        <asp:Label ID="lblBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Branch"></asp:Label>
                              </td>
                          <td> 
                      <asp:DropDownList ID="ddlbranch1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="160px"
                            OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                          </td>
                          <td> 
                         <div id="div4" style="position: relative;" runat="server" visible="false">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                          </td>
                        </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                     <td colspan="10">
                     <table>
                     <tr>
                     <td>
                     <asp:Label ID="lbl_org_sem" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                     <asp:DropDownList ID="ddlsem1" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px"
                            OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged" AutoPostBack="True">
                     </asp:DropDownList>
                     </td>
                     <td>
                       <div id="Div1" style="position: relative;" runat="server" visible="false">
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" Width="120px" CssClass="textbox textbox1 txtheight1" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                     </asp:UpdatePanel>
                     </div>
                     </td>
                     <td>
                     <asp:Label ID="lblSubject" Text="Subjects" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                     <asp:DropDownList ID="ddlSubject" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                            Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="320px">
                     </asp:DropDownList>
                   </td>
                     <td>
                     <div id="Div5" style="position: relative;" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="chksubject" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chksubject_checkedchange" />
                                                <asp:CheckBoxList ID="cblsubject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblsubject_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSubject"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                     </td>
                    <td>
                        <asp:DropDownList ID="ddlreptype" runat="server" AutoPostBack="true" Style="" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" OnSelectedIndexChanged="lblrepttype_OnSelectedIndexChanged">
                            <asp:ListItem>Round Off Moderation</asp:ListItem>
                            <asp:ListItem>Special Moderation</asp:ListItem>
                            <asp:ListItem>Genral Moderation</asp:ListItem>
                            <asp:ListItem>Degree Moderation</asp:ListItem>
                        </asp:DropDownList>
                         </td>
                    <td>
                      <asp:Label ID="lblMod" Text="Moderation" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                            </td>
                    <td>
                      <asp:TextBox ID="txtMod" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="40px"></asp:TextBox>
                            </td>
                    <td>
                              <asp:TextBox ID="txtfrom" runat="server" placeholder="From" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="40px" Visible="false"></asp:TextBox>
                            </td>
                    <td>
                            <asp:Label ID="lbldum1" Text="-" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Larger" Visible="false"></asp:Label>
                            </td>
                    <td>
                              <asp:TextBox ID="txtTo" runat="server"  placeholder="To" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="40px" Visible="false"></asp:TextBox>
                      </td>
                      <td>
                        <asp:Button ID="btnGo" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto;" />
                              <asp:Button ID="btnView" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"
                            OnClick="btnView_Click" Text="View" Style="width: auto; height: auto;" />
                        <asp:Button ID="btnHelp" Font-Bold="True"  Font-Names="Book Antiqua"
                            Font-Size="Large" runat="server"
                            OnClick="btnHelp_Click" Text="?" Style="width: 40px; height: auto;" />
                  </td>
                 </tr>
                 </table>
                    </td>
                    <%-- <td align="right">
                    <asp:Button ID="btnFoilCard" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="true" OnClick="btnFoilCard_click" Text="Foil Card" />
                </td>--%>
                    </tr>
            </table>
            </asp:Panel>
        </div>
    </center>
    <br />
    <center>
    <table>
      <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnsave1" runat="server" OnClick="btnsavel1_click" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Save" />
                        <asp:Button ID="btnprintt" runat="server" OnClick="btnprintt_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Print" />
                    </td>
                </tr>
    </table>
    </center>
    <center>
      <br />
            <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false"></asp:Label>
       </center>
        <center>
        <table>
        <tr><td> <asp:Label ID="lblBeMod" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Maroon">           
       </asp:Label></td>
       <td></td>
        <td> <asp:Label ID="lblAfMod" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Blue"></asp:Label></td></tr>
        </table>
        </center>
      <center>
    <table>
      <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_click" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Save" />
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Print" />
                              <asp:Button ID="Button3" runat="server" OnClick="Button3_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Print" />
                    </td>
                </tr>
    </table>
    </center>
     <center>
       <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
                HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                        SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            </center>
             <center>
       <FarPoint:FpSpread ID="fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
                HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                        SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            </center>
            <br />
             <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                 <center>
                 <center>
                  <FarPoint:FpSpread ID="fpspread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="600" Visible="true">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                 </center>
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
      <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 900px;" visible="false">
        </div>
    </div>
</asp:Content>

