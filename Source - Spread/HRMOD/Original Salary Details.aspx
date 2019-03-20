<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Original Salary Details.aspx.cs" Inherits="Original_Salary_Details" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
.fontstyle
{
    font-family="Book Antiqua";
    font-size:medium;
    font-weight:bold;
}
 .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 165px;
            position: absolute;
            font-weight: bold;
            width: 980px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 980px;
            position: absolute;
            height: 80px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
</style>
    <script type="text/javascript">

        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 95px;
        left: -16px; position: absolute; width: 1025px; height: 21px">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="White" Text="Original Salary Details"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div class="CenterPB" style="height: 40px; width: 40px;">
                        <img src="../images/progress2.gif" height="180px" width="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <asp:Label ID="lblfyear" Text="From Year" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 125px; left: 10px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddlfyear" runat="server" ont-Names="Book Antiqua" Font-Size="Medium"
                AutoPostBack="true" OnSelectedIndexChanged="ddlfyear_SelectedIndexChanged" Width="100px"
                Style="top: 125px; left: 102px; position: absolute;">
            </asp:DropDownList>
            <asp:Label ID="lblfmonth" Text="From Month" runat="server" CssClass="fontstyle" Style="top: 125px;
                left: 211px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddlfmonth" runat="server" ont-Names="Book Antiqua" Font-Size="Medium"
                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlfmonth_SelectedIndexChanged"
                Style="top: 125px; left: 306px; position: absolute;">
            </asp:DropDownList>
            <asp:Label ID="lbltyear" Text="To Year" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 125px; left: 415px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddltyear" runat="server" ont-Names="Book Antiqua" Font-Size="Medium"
                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddltyear_SelectedIndexChanged"
                Style="top: 125px; left: 490px; position: absolute;">
            </asp:DropDownList>
            <asp:Label ID="lbltmonth" Text="To Month" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 125px; left: 595px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddltmonth" runat="server" ont-Names="Book Antiqua" Font-Size="Medium"
                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddltmonth_SelectedIndexChanged"
                Style="top: 125px; left: 680px; position: absolute;">
            </asp:DropDownList>
            <asp:RadioButton ID="rbconsolidate" runat="server" GroupName="Report" Text="Consolidate"
                CssClass="fontstyle" AutoPostBack="true" OnCheckedChanged="rbreport_SelectedIndexChanged"
                Style="top: 125px; left: 780px; position: absolute;" />
            <asp:RadioButton ID="rbreport" runat="server" OnCheckedChanged="rbreport_SelectedIndexChanged"
                CssClass="fontstyle" Text="Staff Wise" GroupName="Report" AutoPostBack="true"
                Style="top: 125px; left: 885px; position: absolute;" />

            

            <asp:Label ID="lbldept" Text="Department" runat="server" CssClass="fontstyle" Style="top: 156px;
                left: 10px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtdept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 155px; left: 102px; position: absolute;">---Select---</asp:TextBox>
            <asp:Panel ID="pdept" runat="server" CssClass="multxtpanel" Height="250px">
                <asp:CheckBox ID="chkdept" runat="server" CssClass="fontstyle" Font-Size="Medium"
                    Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdept_ChekedChange" />
                <asp:CheckBoxList ID="chklsdept" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnSelectedIndexChanged="chklsdept_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdept"
                PopupControlID="pdept" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lbldesign" Text="Designation" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 156px; left: 211px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtdesign" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 155px; left: 306px; position: absolute;">---Select---</asp:TextBox>
            <asp:Panel ID="pdesign" runat="server" CssClass="multxtpanel" Height="250px">
                <asp:CheckBox ID="chkdesign" runat="server" OnCheckedChanged="chkdesign_ChekedChange"
                    CssClass="fontstyle" Text="Select All" AutoPostBack="True" />
                <asp:CheckBoxList ID="chklsdesign" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnSelectedIndexChanged="chklsdesign_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdesign"
                PopupControlID="pdesign" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lblcategory" Text="Category" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 156px; left: 415px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtcategory" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 155px; left: 490px; position: absolute">---Select---</asp:TextBox>
            <asp:Panel ID="pcategory" runat="server" CssClass="multxtpanel" Height="250px">
                <asp:CheckBox ID="chkcategory" runat="server" OnCheckedChanged="chkcategory_ChekedChange"
                    CssClass="fontstyle" Text="Select All" AutoPostBack="True" />
                <asp:CheckBoxList ID="chklscategory" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnSelectedIndexChanged="chklscategory_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtcategory"
                PopupControlID="pcategory" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lblstaff" Text="Staff" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 156px; left: 595px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 155px; left: 680px; position: absolute">---Select---</asp:TextBox>
            <asp:Panel ID="pstaff" runat="server" CssClass="multxtpanel" Height="400px">
                <asp:CheckBox ID="chkstaff" runat="server" OnCheckedChanged="chkstaff_ChekedChange"
                    CssClass="fontstyle" Text="Select All" AutoPostBack="True" />
                <asp:CheckBoxList ID="chklsstaff" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnSelectedIndexChanged="chklsstaff_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>

            

     <asp:RadioButton ID="rdbindividual" runat="server" 
                CssClass="fontstyle" Text="Individual Head" GroupName="mapping" AutoPostBack="true"
                Style="top: 156px; left: 600px; position: absolute;" />

                <asp:RadioButton ID="rdmappingbased" runat="server" 
                CssClass="fontstyle" Text="Based On Mapping" GroupName="mapping" AutoPostBack="true"
                Style="top: 156px; left: 744px; position: absolute;" />
            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstaff"
                PopupControlID="pstaff" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lblallowance" Text="Allowance" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 186px; left: 10px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtallowance" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 185px; left: 102px; position: absolute;">---Select---</asp:TextBox>
            <asp:Panel ID="Pallowance" runat="server" CssClass="multxtpanel" Height="200px">
                <asp:CheckBox ID="chkallowance" runat="server" AutoPostBack="True" CssClass="fontstyle"
                    OnCheckedChanged="chlallowance_CheckedChanged" Text="Select All" Checked="True" />
                <asp:CheckBoxList ID="chklsallowance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkallowance_SelectedIndexChanged"
                    Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtallowance"
                PopupControlID="Pallowance" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lbldeduction" Text="Deduction" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 186px; left: 211px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtdeduction" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style="top: 185px; left: 306px; position: absolute;">---Select---</asp:TextBox>
            <asp:Panel ID="pdeduction" runat="server" CssClass="multxtpanel" Height="200px">
                <asp:CheckBox ID="chkdeduction" runat="server" AutoPostBack="True" CssClass="fontstyle"
                    OnCheckedChanged="chkdeduction_CheckedChanged" Text="Select All" Checked="True" />
                <asp:CheckBoxList ID="chklsdeduction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklsdeduction_SelectedIndexChanged"
                    Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtdeduction"
                PopupControlID="pdeduction" Position="Bottom">
            </asp:PopupControlExtender>
            <asp:Label ID="lblorder" runat="server" CssClass="fontstyle" Text="Order by" Style="top: 184px;
                left: 415px; position: absolute;"></asp:Label>
            <asp:DropDownList ID="ddlorder" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                Style="top: 184px; left: 490px; position: absolute;">
                <asp:ListItem Text="Dept & Staff Code"></asp:ListItem>
                <asp:ListItem Text="Priority"></asp:ListItem>
                <asp:ListItem Text="Print Priority-1"></asp:ListItem>
                <asp:ListItem Text="Print Priority-2"></asp:ListItem>
                <asp:ListItem Text="Account No"></asp:ListItem>
                <asp:ListItem Text="Staff Wise Priority"></asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="lblsalay_certificate" runat="server" CssClass="fontstyle" Text="Certificate Formate" Style="top:184px;
            left:650px; position:absolute;" /> 
            <asp:DropDownList ID="ddlformatewise" runat="server"  OnSelectedIndexChanged="ddl_formatewiseSelectIndexChange" Font-Names="Book Antiqua" Font-Size="Medium"
            Style="top:184px;left:800px; position:absolute;">
            <asp:ListItem Text="Formate 1" Value="0"></asp:ListItem>
            <asp:ListItem Text="Formate 2" Value="1"></asp:ListItem>

            </asp:DropDownList>

              <asp:LinkButton ID="lnk_btn_print" runat="server" Text="Print Settings" Font-Bold="true"  Style="top: 184px; left: 900px; position: absolute;"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnk_btn_print_click"></asp:LinkButton>
            <asp:Button ID="btngo" runat="server" CssClass="fontstyle" Text="Go" OnClick="btngo_Click"
                Style="top: 184px; left: 1001px; position: absolute;" />


                       <center>
            <div id="printpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 245px;
                        width: 410px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_print" runat="server" Text="Footer Name" Style="color: Black;
                                            width: 165px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_print" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_certificate" runat="server" Text="Certificate Content" Style="color: Black;
                                            width: 165px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_certificate" runat="server" TextMode="MultiLine" Text="" Font-Names="Book Antiqua"
                                            Width="200px" Height="100px" Font-Size="Medium" Font-Bold="true" 
                                            MaxLength="1000"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btn_printSave" CssClass=" textbox1 btn2" OnClick="btnsavePrint_Click"
                                Text="Save" runat="server" />
                            <asp:Button ID="btn_printexit" CssClass=" textbox1 btn2" OnClick="btnexitPrint_Click"
                                Text="Exit" runat="server" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
         <center>
            <div id="img_div1" runat="server" visible="false" style="height: 150em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="alertdiv" runat="server" class="table" style="background-color: White; height: auto;
                        width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblsavealert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass="textbox textbox1 btn2" Width="50px" OnClick="btnerrclose_Click"
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
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="Panel8" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                Width="1080px" Style="position: absolute; left: 0px;">
            </asp:Panel>
            <br />
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#0CA6CA"
                Width="959px">
                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                    ImageAlign="Right" />
            </asp:Panel>
            <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="false"
                    OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                    RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                    <asp:ListItem Text="Total No.of Staff"></asp:ListItem>
                    <asp:ListItem Text="Basic Pay"></asp:ListItem>
                    <asp:ListItem Text="Grade Pay"></asp:ListItem>
                    <asp:ListItem Text="Pay Band"></asp:ListItem>
                    <asp:ListItem Text="Total Allowance"></asp:ListItem>
                    <asp:ListItem Text="Gross Amount"></asp:ListItem>
                    <asp:ListItem Text="Total Deductions"></asp:ListItem>
                    <asp:ListItem Text="Staff Code"></asp:ListItem>
                    <asp:ListItem Text="Department"></asp:ListItem>
                    <asp:ListItem Text="Designation"></asp:ListItem>
                    <asp:ListItem Text="Category"></asp:ListItem>
                    <asp:ListItem Text="Lop"></asp:ListItem>
                    <asp:ListItem Text="Net Amount"></asp:ListItem>
                    <asp:ListItem Text="Remarks"></asp:ListItem>
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <br />
            <asp:Label ID="errmsg" runat="server" ForeColor="Red" CssClass="fontstyle"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="FpMonthOverall" runat="server" Height="250px" Width="400px"
                ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" OnButtonCommand="FpMonthOverall_ButtonCommand" CssClass="cursorptr"
                BorderColor="Black" BorderWidth="0.5" ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <asp:Label ID="lblexcel1" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="False"></asp:Label>
            <asp:TextBox ID="txtexcel1" onkeypress="display()" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" runat="server"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcel1"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()_}{][., ">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnexcel1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Export Excel" OnClick="btnexcel1_Click" />
            <asp:Button ID="btnprint1" runat="server" Text="Print" OnClick="btnprint1_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
            <asp:Button ID="btngenerate" runat="server" CssClass="fontstyle" Text="Generate"
                OnClick="btngenerate_Click" />
              <%--  <asp:Button ID="btnSalaryCertificate" runat="server" CssClass="fontstyle" Text="Salary Certificate"
                OnClick="btn_SalaryCertificate_Click" />
--%>
            <br />
            <asp:Label ID="lblgenerror" runat="server" ForeColor="Red" CssClass="fontstyle"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="FpSalaryReport" runat="server" Height="250px" Width="400px"
                ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" OnButtonCommand="FpSalaryReport_ButtonCommand" CssClass="cursorptr"
                BorderColor="Black" BorderWidth="0.5">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="False"></asp:Label>
            <asp:TextBox ID="txtexcel" onkeypress="display()" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" runat="server"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()_}{][., ">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnexcel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Export Excel" OnClick="btnexcel_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
            <asp:PostBackTrigger ControlID="btngenerate" />
            <asp:PostBackTrigger ControlID="lnk_btn_print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
