<%@ Page Title="CAM Test Comparision Settings" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Cam_Comparision_Settings_For_PerformanceMoniter.aspx.cs"
    Inherits="Cam_Comparision_Settings_For_PerformanceMoniter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body
        {
            font-family: Book Antiqua;
            height: auto;
            background-color: #ffffff;
            color: Black;
        }
        .Chartdiv
        {
            background-color: #ffffff;
            margin: 0px;
            color: #000000;
            position: relative;
            font-family: Book Antiqua;
            height: auto;
            width: 100%;
        }
        .Header
        {
            font-weight: bold;
            text-align: center;
            font-size: 22px;
            color: Green;
            margin-top: 20px;
            margin-bottom: 20px;
            line-height: 3em;
        }
        .fontCommon
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: #000000;
        }
        .defaultHeight
        {
            width: auto;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 100%; height: auto;">
            <table>
                <thead>
                    <tr>
                        <td colspan="3">
                            <center>
                                <span class="Header">CAM Test Comparision Settings</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <fieldset id="maindiv" runat="server" style="width: 960px; margin-left: 0px; height: auto;
                    border-color: silver; border-radius: 10px;">
                    <center>
                        <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;">
                            <table style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                                box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px;
                                padding: 1em; margin-left: 0px; width: 930px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="50px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlsubject_Selectchanged">
                                                    </asp:DropDownList>
                                                    <div style="position: relative; display: none;">
                                                        <asp:UpdatePanel ID="UpnlSubjects" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtSubjects" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlSubjects" runat="server" CssClass="multxtpanel" Height="200px"
                                                                    Width="250px">
                                                                    <asp:CheckBox ID="chkSubjects" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubjects_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblSubjects" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSubjects_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popubExtSubjects" runat="server" TargetControlID="txtSubjects"
                                                                    PopupControlID="pnlSubjects" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Width="59px" CssClass="textbox defaultHeight" Text="Go" OnClick="btngo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <center>
                        <div id="divMainContents" runat="server" style="display: table-row; margin: 0px;
                            margin-top: 45px; margin-bottom: 10px; height: auto; width: auto; padding: 0px;">
                            <span id="spanCompare" runat="server" style="margin: 0px; margin-top: 30px; position: relative;
                                height: auto; padding: 45px; width: auto; font-family: Book Antiqua; font-size: larger;
                                font-style: oblique; font-weight: bold; color: Green; text-indent: 20px; text-decoration: underline;">
                                Comparision Of Tests</span>
                            <div id="divSavedSettings" runat="server" style="display: table-row; margin: 0px;
                                height: auto; padding: 15px; width: auto; padding: 0px; margin-top: 100px;">
                                <FarPoint:FpSpread ID="FpSpreadSettings" AutoPostBack="false" Width="1050px" runat="server"
                                    Visible="true" BorderStyle="Solid" BorderWidth="1px" CssClass="spreadborder"
                                    ShowHeaderSelection="false" Style="width: 100%; height: auto; display: block;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <br />
                                <br />
                                <asp:Button ID="btnAdd" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    CssClass="textbox defaultHeight" Text="Add New Test" Style="width: auto;" OnClick="btnAdd_Click" />
                            </div>
                            <div id="divSettings" runat="server" style="display: table-row; margin: 0px; margin-top: 30px;
                                position: relative; height: auto; padding: 15px; width: auto;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNameCompare" runat="server" Text="Name of Comparision" Font-Bold="True"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNameCompare" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="" CssClass="textbox  txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConvertedTo" runat="server" Text="Converted To" Font-Bold="True"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConvertedTo" Width="60px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" MaxLength="3" Text="" CssClass="textbox  txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterExtTxtTypeConvertedTo" runat="server" FilterType="Numbers"
                                                TargetControlID="txtConvertedTo">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Width="59px" CssClass="textbox defaultHeight" Text="Save" OnClick="btnSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="divComparator" runat="server" style="display: table-row; margin: 0px; padding: 10px;
                                    width: 200%; height: auto; background-color: Silver;">
                                    <div id="divFrom" style="display: table-cell; width: auto; height: auto; margin: 0px;
                                        padding: 15px;">
                                        <span id="spanFromTest" runat="server">Compare Test From</span>
                                        <asp:TreeView ID="tvFromTest" runat="server">
                                        </asp:TreeView>
                                        <asp:CheckBoxList ID="cblFromTest" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" Style="display: table-cell; text-align: left;">
                                        </asp:CheckBoxList>
                                    </div>
                                    <div id="divTo" style="display: table-cell; width: auto; margin: 0px; padding: 15px;
                                        height: auto;">
                                        <span id="spanToTest" runat="server">Compare Test To</span>
                                        <asp:TreeView ID="tvToTest" runat="server">
                                        </asp:TreeView>
                                        <asp:CheckBoxList ID="cblToTest" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" Style="display: table-cell; text-align: left;">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                            <div>
                            </div>
                        </div>
                    </center>
                </fieldset>
            </center>
            <div id="popupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lblpopuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                                Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                                OnClick="btn_errorclose_Click" Text="Ok" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </div>
    </center>
</asp:Content>
