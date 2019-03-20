<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CommonHomeWork.aspx.cs" Inherits="AttendanceMOD_CommonHomeWork" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }

        function PrintPanel() {
            var panel = document.getElementById("<%=divCounsellingReport.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
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
    <br />
    <center>
        <center>
            <asp:Label ID="Label2" runat="server" Text="Home Work" class="fontstyleheader" Font-Bold="True"
                Font-Names="Book Antiqua" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px;
                position: relative;"></asp:Label>
        </center>
        <div style="width: 1100px; height: 80px; border-radius: 10px; background-color: #0CA6CA;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Size="Medium"
                            Width="269px" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <div id="setwidth" runat="server">
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </div>
                    </td>
                    <td>
                        <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox txtheight2" Font-Bold="True"
                            Font-Names="Book Antiqua" ReadOnly="true">-- Select--</asp:TextBox>
                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="250px" Width="150px">
                            <asp:CheckBox ID="chckdegree" runat="server" Text="Select All" OnCheckedChanged="chckdegree_checkedchange"
                                AutoPostBack="true" />
                            <asp:CheckBoxList ID="ddldegree" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" AutoPostBack="true">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                            PopupControlID="Panel1" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtbranch" runat="server" CssClass="textbox txtheight2" Font-Bold="True"
                            Font-Names="Book Antiqua" ReadOnly="true">-- Select--</asp:TextBox>
                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="250px" Width="200px">
                            <asp:CheckBox ID="chckbranch" runat="server" Text="Select All" OnCheckedChanged="chckbranch_checkedchange"
                                AutoPostBack="true" />
                            <asp:CheckBoxList ID="ddlbranch" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" AutoPostBack="true">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                            PopupControlID="Panel2" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsec" runat="server" CssClass="textbox txtheight2" Width="90px"
                            Font-Bold="True" Font-Names="Book Antiqua" ReadOnly="true">-- Select--</asp:TextBox>
                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="250px" Width="120px">
                            <asp:CheckBox ID="chcksec" runat="server" Text="Select All" OnCheckedChanged="chcksec_checkedchange"
                                AutoPostBack="true" />
                            <asp:CheckBoxList ID="ddlsec" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                OnSelectedIndexChanged="ddlsec_SelectedIndexChanged" AutoPostBack="true">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtsec"
                            PopupControlID="Panel4" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                </tr>
            </table>
            <table style="margin-right: 490px;">
                <tr>
                    <td>
                        <asp:Label ID="lblSuType" runat="server" Text="SubjectType" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubtype" runat="server" CssClass="textbox txtheight2" Font-Bold="True"
                            Font-Names="Book Antiqua" ReadOnly="true">-- Select--</asp:TextBox>
                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="250px" Width="200px">
                            <asp:CheckBox ID="chkSubtype" runat="server" Text="Select All" AutoPostBack="true"
                                OnCheckedChanged="CheckBox1_checkedchange" />
                            <asp:CheckBoxList ID="cblSubtype" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtSubtype"
                            PopupControlID="Panel3" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox txtheight2" Font-Bold="True"
                            Font-Names="Book Antiqua" ReadOnly="true">--Select--</asp:TextBox>
                        <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Width="300px" Height="250px">
                            <asp:CheckBox ID="cbSubjet" runat="server" Text="Select All" AutoPostBack="true"
                                OnCheckedChanged="cbSubjet_checkedchange" />
                            <asp:CheckBoxList ID="cblSubject" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                AutoPostBack="true" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtSubject"
                            PopupControlID="Panel5" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <center>
            <asp:GridView ID="gviewhomewrk" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                <AlternatingRowStyle Height="20px" />
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>' />
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Degree Details">
                        <ItemTemplate>
                            <asp:Label ID="lbldegr" runat="server" Text='<%#Eval("text") %>' />
                            <asp:Label ID="lbldegdetail" Visible="false" runat="server" Text='<%#Eval("degreedetailtag") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject_Code">
                        <ItemTemplate>
                            <asp:Label ID="lblsubcde" runat="server" Text='<%#Eval("subcode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <ItemTemplate>
                            <asp:Label ID="lblsub" runat="server" Text='<%#Eval("subject") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <center>
                                <asp:CheckBox ID="chck" runat="server" AutoPostBack="true" OnCheckedChanged="chck_OnCheckedChanged" />
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Add Homework">
                        <ItemTemplate>
                            <center>
                                <asp:Button ID="btnadd" runat="server" Text="Add" OnClick="btnadd_Onclick" />
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Send">
                        <ItemTemplate>
                            <asp:Button ID="btnsend" runat="server" Text="Send" OnClick="btnview_Onclick" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </center>
        <center>
        </center>
        <div id="divEnterHomework" runat="server" visible="false" style="height: 100em; z-index: 100;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 8%;
            left: 0px;">
            <center>
                <div id="divHomework" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 970px; height: auto; z-index: 1000; border-radius: 5px;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; margin-top: -2%; margin-left: 465px; position: absolute;"
                        OnClick="btnclosespread1_OnClick" />
                    <center>
                        <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                            position: relative; font-weight: bold;">Enter Home Work</span>
                    </center>
                    <br />
                    <br />
                    <table id="Tablenote" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" Text="Subject" runat="server" Font-Bold="true" Visible="true"
                                    Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                    font-weight: bold; width: 90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblsubtext" runat="server" Visible="true" Font-Bold="true" Style="display: inline-block;
                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: auto"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Visible="false" Font-Bold="true" Style="display: inline-block;
                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: auto"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbldate1" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Style="margin-left: 48px" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtdate1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Style="margin-left: -257px" Font-Size="Medium" Width="100px" AutoPostBack="true"
                                    OnTextChanged="txtdate1_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" TargetControlID="txtdate1"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblheading" Text="Heading" runat="server" Font-Bold="true" Visible="true"
                                    Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                    font-weight: bold; width: 90px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtheading" runat="server" border-color="black" Style="display: inline-block;
                                    color: Black; border-width: thin; border-color: Black; font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhomewrk" Text="HomeWork" runat="server" Font-Bold="true" Style="display: inline-block;
                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 90px;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txthomework" TextMode="MultiLine" runat="server" MaxLength="4000"
                                    Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                    border-width: thin; border-color: Black; font-weight: bold; width: 500px; height: 75px;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblfile" Text="Photos" runat="server" Font-Bold="true" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold; margin-left: 0px; width: 90px" text-align="left"></asp:Label>
                                <asp:FileUpload ID="fudfile" runat="server" />
                                <asp:Label ID="lblshowpic" runat="server" Visible="false" />
                                <asp:LinkButton ID="lnkdelpic" runat="server" Visible="false" OnClick="lnlremovepic"
                                    ForeColor="Blue" Font-Underline="true">Remove</asp:LinkButton>
                                <br />
                                <asp:Label ID="lblattachements" Text="Attachements" runat="server" Font-Bold="true"
                                    Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;
                                    text-align: left" />
                                <asp:FileUpload ID="fudattachemntss" runat="server" />
                                <asp:Label ID="lblshowdoc" runat="server" Visible="false" />
                                <asp:LinkButton ID="lnkdeldoc" runat="server" Visible="false" OnClick="lnlremovedoc"
                                    ForeColor="Blue" Font-Underline="true">Remove</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <table style="margin-bottom: 10px; margin-top: 5px; position: relative;">
                        <tr>
                            <td>
                                <asp:Label ID="lblPopDegErr" runat="server" Text=" " CssClass="font" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnsend1" OnClick="btnsend_Click" Text="Send" runat="server" Visible="false"
                                    Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                <asp:Button ID="btnsavewrk" OnClick="btnsavewrk_Click" Text="Save" runat="server"
                                    Visible="false" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                <asp:Button ID="btnupdate" OnClick="btnupdate_Click" Text="Update" runat="server"
                                    Visible="false" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                <asp:Button ID="btndelete" OnClick="btndelete_Click" Text="Delete" runat="server"
                                    Visible="false" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                              
                                <asp:Label ID="lbldel" Visible="false" runat="server" />
                            </td>
                        </tr>
                    </center>
                </div>
            </center>
        </div>
        <div id="div1" runat="server" visible="false" style="height: 100em; z-index: 100;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 8%;
            left: 0px;">
            <center>
                <div id="divhme" runat="server" class="table" visible="false" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-top: 181px; margin-right: auto; width: 1000px; margin-top: 89px; height: auto;
                    z-index: 1000; border-radius: 5px;">
                    <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; margin-top: -2%; margin-left: 482px; position: absolute;"
                        OnClick="btnclosespread_OnClick" />
                    <center>
                        <span style="text-align: center; color: Green; font-size: large; position: relative;
                            font-weight: bold;">Home Work Details</span>
                    </center>
                    <br />
                    <center>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From: "></asp:Label>
                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                    onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:Label ID="Label4" runat="server" Visible="false" Font-Bold="true" Style="display: inline-block;
                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: auto"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btngo1" runat="server" Text="Go" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btngo1_Click" />
                            </td>
                        </tr>
                    </center>
                    <br />
                    <br />
                    <center>
                        <div id="divCounsellingReport" runat="server" visible="false" width="800px">
                            <table id="Tablegview" runat="server">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="gviewhme" runat="server" Style="margin-bottom: 15px; margin-top: -14px;
                                            width: 850px;" Font-Names="Times New Roman" OnSelectedIndexChanged="gviewhme_selectedindexchanged"
                                            OnRowCreated="gviewhme_onRowCreated" ShowHeader="true" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                            <AlternatingRowStyle Height="20px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblsno" runat="server" Text='<%# Eval("SNo") %>' /></center>
                                                        <asp:Label ID="lbluniq" runat="server" Visible="false" Text='<%#Eval("uniqid") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("Date") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltype" runat="server" Text='<%# Eval("Type") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsubject" runat="server" Text='<%# Eval("Subject") %>' />
                                                        <%--<asp:Label ID="lblsubno" runat="server" Visible="false" Text='<%# Eval("Subjectno") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Heading">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhead" runat="server" Text='<%# Eval("Heading") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Home Work">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltopic" runat="server" Text='<%# Eval("Topic") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Photo">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadpic" runat="server" Text='<%#Eval("Photo") %>' ForeColor="Blue"
                                                            OnClick="lnkDownloadpic_click" Font-Underline="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadfile" runat="server" Text='<%#Eval("Attachment") %>'
                                                            ForeColor="Blue" OnClick="lnkDownloadfile_click" Font-Underline="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <center>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrint1" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Visible="false" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                        </tr>
                    </center>
                </div>
            </center>
        </div>
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
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </center>
</asp:Content>
