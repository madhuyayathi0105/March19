<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewAlterBatchallocation.aspx.cs" Inherits="NewAlterBatchallocation" %>

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
            width: 165px;
        }
        .style4
        {
            width: 322px;
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
    </style>
    <%--</head>--%>
    <body oncontextmenu="return false">
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up_spreadbatch" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="up_spreadbatch">
                    <ProgressTemplate>
                        <div class="CenterPB" style="height: 40px; width: 40px;">
                            <img src="../images/progress2.gif" height="180px" width="180px" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                    PopupControlID="UpdateProgress1">
                </asp:ModalPopupExtender>
                <div style="height: 54px; width: 987px;">
                    <center>
                        <asp:Label ID="lblhead" runat="server" Text="Alternate Batch Allocation For Laboratory"
                            CssClass="fontstyleheader" ForeColor="Green"></asp:Label>
                    </center>
                    <div>
                        <br />
                        <table cellpadding="2px" cellspacing="4px" style="height: 100%; margin-left: 0px;
                            width: 103%;" class="maintablestyle ">
                            <tr>
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
                                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="100px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td class="style4">
                                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
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
                                    <asp:CalendarExtender ID="CalExtFromDate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="bcntlbl" runat="server" Text="No of Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="btctxt" runat="server" AutoPostBack="True" Height="16px" OnTextChanged="btctxt_TextChanged"
                                        Width="41px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="btctxt"
                                        FilterType="Numbers" />
                                    <asp:Label ID="bcntddllbl" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="bcntddl" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="bcntddl_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <br />
                                    <asp:Label ID="fmlbl" runat="server" Text="Enter Todate" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" Style="height: 26px; top: 195px;
                                        left: 440px; position: absolute; font-weight: 700" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" />
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
                                                    <asp:Panel ID="panel_sp1" runat="server" Height="356px" Width="570px">
                                                        <%--<FarPoint:FpSpread ID="batch_spread" runat="server" 
          Height="250px" Width="400px" 
          ActiveSheetViewIndex="0" currentPageIndex="0" 
          DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;" 
          EnableClientScript="False"  CssClass="cursordflt"
         >
          <commandbar backcolor="Control">
              <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
          </commandbar>
          <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" 
              Font-Strikeout="False" Font-Underline="False" />
          <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" 
              Font-Strikeout="False" Font-Underline="False" />
          <sheets>
              <FarPoint:SheetView SheetName="Sheet1" 
                  
                  EditTemplateColumnCount="2" GridLineColor="#DEDFDE" 
                  GroupBarText="Drag a column to group by that column." 
                  SelectionBackColor="#CE5D5A" 
                  SelectionForeColor="White">
              </FarPoint:SheetView>
          </sheets>
          
      </FarPoint:FpSpread>--%>
                                                        <FarPoint:FpSpread ID="batch_spread" runat="server" Height="250" Width="500px" ActiveSheetViewIndex="0"
                                                            currentPageIndex="0" CssClass="cursordflt" OnUpdateCommand="batch_spread_UpdateCommand"
                                                            DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark">
                                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                                            </CommandBar>
                                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" />
                                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" />
                                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False"></Pager>
                                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False"></HierBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="#DEDFDE" SelectionForeColor="White">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                                Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                                Font-Strikeout="False" Font-Underline="False">
                                                            </TitleInfo>
                                                        </FarPoint:FpSpread>
                                                        <%--
      <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" 
              HorizontalAlign="Center" VerticalAlign="NotSet" Font-Bold="False" 
              Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
              Font-Underline="False">
          </TitleInfo>--%>
                                                        <br />
                                                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                                        <asp:Label ID="sfrlbl" runat="server" Text="Check From" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="90px"></asp:Label>
                                                        <asp:TextBox ID="sfmtxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="35"></asp:TextBox>
                                                        <asp:Label ID="stolbl" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="30"></asp:Label>
                                                        <asp:TextBox ID="stotxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="35" AutoPostBack="true" OnTextChanged="stotxt_TextChanged"></asp:TextBox>
                                                        <asp:Button ID="selbtn" runat="server" Text="Select" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="28px" Width="55px" OnClick="selbtn_Click" />
                                                        <asp:Button ID="btnsave" runat="server" Text="Save" Height="28px" Width="55px" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnsave_Click" />
                                                        <asp:Button ID="delbtn" runat="server" Text="Delete" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="28px" Width="60px" OnClick="delbtn_Click" />
                                                    </asp:Panel>
                                                </td>
                                                <%-- ------------------------lab----------------------%>
                                                <td class="style7" align="center">
                                                    <%--align added by Manikandan 20/08/2013--%>
                                                    <asp:Panel ID="Panel_sp2" runat="server" BorderColor="ActiveCaptionText" BorderStyle="Ridge"
                                                        Style="position: absolute; top: 342px; left: 626px;" Height="220px" Width="360px">
                                                        <FarPoint:FpSpread ID="sml_spread" runat="server" Height="170px" Width="250px" ActiveSheetViewIndex="0"
                                                            currentPageIndex="0" CssClass="cursordflt" Style="margin-top: 3px;" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark">
                                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                                            </CommandBar>
                                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" />
                                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False" />
                                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False"></Pager>
                                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                Font-Underline="False"></HierBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="#DEDFDE" SelectionForeColor="White">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                                Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                                Font-Strikeout="False" Font-Underline="False">
                                                            </TitleInfo>
                                                        </FarPoint:FpSpread>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btn2sv" runat="server" Text="save" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" OnClick="btn2sv_Click" Style="position: absolute; left: 155px;" />
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue" Style="left: 5px;
                                                            position: absolute; width: 150px; top: 200px;" OnClick="LinkButton1_Click">To Add Multiple Batch</asp:LinkButton>
                                                        <fieldset id="Fieldset5" runat="server" style="position: absolute; left: 5px; width: 116px;
                                                            background-color: white; top: 203px; height: 84px;">
                                                            <asp:CheckBoxList ID="Checkboxlistbatch" runat="server" Style="width: 92px; position: absolute;
                                                                top: 1px; left: 2px; border-style: double;" OnSelectedIndexChanged="Checkboxlistbatch_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                            <asp:Button ID="Button3" runat="server" Text="Ok" Style="position: absolute; left: 98px;
                                                                top: 68px;" OnClick="Button3_Click" />
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
        </asp:UpdatePanel>
        <%-- </form>--%>
    </body>
    </html>
</asp:Content>
