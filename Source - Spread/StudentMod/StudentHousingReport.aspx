<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentHousingReport.aspx.cs" Inherits="StudentMod_StudentHousingReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
        function autoComplete3_OnClientPopulating(sender, args) {
            var SEARCHTYPE = document.getElementById("<%=ddl_searchtype.ClientID %>").value;
            //var SEARCHTYPE = skillsSelect.options[skillsSelect.selectedIndex].value;
            sender.set_contextKey(SEARCHTYPE);
        }
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; font-size: x-large;">Housing Report</span>
        </div>
        <br />
    </center>
    <div>
        <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_degreeT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_branchT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_semT" runat="server" Visible="false"></asp:Label>
         <asp:Label ID="lb1_housingT" runat="server" Visible="false"></asp:Label>
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbl_clgname" runat="server" Text="College"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight3 textbox1" runat="server" AutoPostBack="true"
                            Width="185px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                    </td>

                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                    Width="135px" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_batch_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                   


                    <td>
                        <asp:Label ID="lbl_degree" Text="Degree" Width="52px" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                    Width="100px" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_degree_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="p3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_branch" Text="Branch" runat="server" ></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="140px" Style="position: absolute;">
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
                </tr>
                <tr>
                    
                    


                    <td colspan="2">
                        <asp:Label ID="Lbl_housing" Text="Housing" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="Txt_housing" runat="server" CssClass="textbox textbox txtheight4" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_housing" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_housing_checkedchange" />
                                    <asp:CheckBoxList ID="cb1_housing" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_housing_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="Txt_housing"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
             
                    <td>
                        <%-- <asp:Label ID="lbl_searchappno" runat="server" Text="Admission No"></asp:Label>--%>
                        <asp:DropDownList ID="ddl_searchtype" runat="server" CssClass="textbox 1 ddlheight"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_searchtype_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>

                 

                    <td >
                        <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight1" 
                            Width="135px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=".-/ ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" Enabled="True"
                            ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno" CompletionListCssClass="autocomplete_completionListElement"
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground"
                            UseContextKey="true" OnClientPopulating="autoComplete3_OnClientPopulating" DelimiterCharacters="">
                        </asp:AutoCompleteExtender>
                    </td>

                     <td>
                        <asp:Button ID="Button1" runat="server" Text="Go" CssClass="textbox textbox1 btn" OnClick="btn_go_OnClick"/>
                    </td>
                </tr>
                   
                    
         
               
            </table>
            <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Style="height: 370px; overflow: auto; background-color: White;
                    border-radius: 10px; box-shadow: 0px 0px 8px #999999" ShowHeaderSelection="false"
                    Visible="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>

            
            <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
            <br />
</asp:Content>
