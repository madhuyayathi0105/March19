<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    AutoEventWireup="true" CodeFile="CourseInformation.aspx.cs" Inherits="MasterWizardMod_CourseInformation" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">


        function frelig5() {
            document.getElementById('<%= btn_pls_mat .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_mat.ClientID%>').style.display = 'block';

        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Course Information</span></div>
        </center>
    </div>
    <center>
        <div>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                        </asp:Label>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="commonHeaderFont">
                        </asp:Label>
                        <asp:TextBox ID="txtsearch1" runat="server" CssClass="textbox txtheight2" Width="179px"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn2" OnClick="btn_go_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                            OnClick="btnaddnew_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="divtable" runat="server" visible="false" style="width: 900px; overflow: auto;
            background-color: White; border-radius: 10px;">
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
            <asp:GridView ID="grdcourse" runat="server" ShowHeader="false" Font-Names="book antiqua" style=" margin-top:70px"
                Width="840px" OnSelectedIndexChanged="grdcourse_onselectedindexchanged"  OnRowCreated="grdcourse_OnRowCreated">
                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
            </asp:GridView>
        </div>
    </center>
    <center>
        <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
            <br />
            <div class="subdivstyle" style="background-color: White; height: 220px; width: 431px;margin-top: 96px;">
            <center>
           <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Course Creation</span>
            </center>
                <br />
                <table>
                
                    <tr>
                        <td>
                            <asp:Label ID="lbl_course" Text="Course Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_course" runat="server" CssClass="textbox textbox1 txtheight5"
                                Style="width: 171px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_edu" Text="Education Level" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddleducation" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="130px" Height="" AutoPostBack="True" OnSelectedIndexChanged="ddleducation_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtedu" runat="server" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbltype" Text="Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btn_pls_mat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                Style="height: 27px; display: none; left: 644px; position: absolute; top: 265px;
                                width: 27px;" OnClick="btn_pls_mat_Click" Text="+" />
                            <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="130px" Height="" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:Button ID="btn_min_mat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                Style="height: 27px; display: none; left: 807px; position: absolute; top: 265px;
                                width: 27px;" OnClick="btn_min_mat_Click" Text="-" />
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 10px;">
                    <tr>
                    <td colspan="2px" align="center">
                        <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btnpopsave_Click"
                            OnClientClick="return valid1()" />
                            <asp:Button ID="btnupdate" Text="Update" Visible="false" runat="server" CssClass="textbox btn2" OnClick="btnupdate_Click"
                            OnClientClick="return valid1()" />
                        <asp:Button ID="btn_delete" Text="Delete" Visible="false" OnClick="btndelete_Click"
                            CssClass="textbox btn2" OnClientClick="return gym()" runat="server" />
                        <asp:Button ID="btn_exit" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btnpopexit_Click" />
                   </td> </tr>
                </table>
            </div>
        </div>
    </center>
    <center>
        <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
            <center>
                <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                    height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table style="line-height: 30px">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                    onkeypress="display1()"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                <ContentTemplate>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                        <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                    <ContentTemplate>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
    </center>
</asp:Content>
