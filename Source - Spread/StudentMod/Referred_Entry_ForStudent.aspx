<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Referred_Entry_ForStudent.aspx.cs" Inherits="StudentMod_Referred_Entry_ForStudent" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript" src="https://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        function getLocation() {
            getAddressInfoByZip(document.getElementById("<%= TextPincode.ClientID %>").value);
            alert("bdjf");
        }

        function response(obj) {
            console.log(obj);
        }

        function validateCaseSensitiveEmail(email) {
            var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (reg.test(email.value)) {
            }
            else {
                email.value = "";
            }
        }
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txtConsultant.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtConsultant.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=TexAgent.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=TexAgent.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=TextPincode.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=TextPincode.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Textadd.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Textadd.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=TextCity.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=TextCity.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=TextDistrict.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=TextDistrict.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Textstate.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Textstate.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Textphone.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Textphone.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Textemail.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Textemail.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Reference Entry For Others</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 747px; height: 75px;
                font-family: Book Antiqua; font-weight: bold">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -259px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblConsultant" runat="server" Text="Consultant/ Name">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Consultant" runat="server" placeholder="Search Consultant/ Name"
                                                                CssClass="textbox textbox1 txtheight2" Width="160px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="searchConsultant" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_Consultant"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Lal_Agent" runat="server" Text="Agent Name">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextAgent" runat="server" placeholder="Search Agent Name" CssClass="textbox textbox1 txtheight2"
                                                                Width="120px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="searchAgent" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextAgent"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Update_go" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click"
                                                                        BackColor="LightGreen" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_Addnew" runat="server" CssClass="textbox btn2" Text="Addnew"
                                                                OnClick="btn_Addnew_Click" BackColor="LightGreen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            </center>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--progressBar for Go--%>
                                    <center>
                                        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Update_go">
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
                                        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
                                            PopupControlID="UpdateProgress6">
                                        </asp:ModalPopupExtender>
                                    </center>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <br />
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                <ContentTemplate>
                    <div id="griddiv" runat="server" style="width: auto; height: auto;" class="spreadborder"
                        visible="false">
                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                        <asp:GridView ID="SelectGrid" runat="server" AutoGenerateColumns="false" Style="width: auto;
                            height: auto;" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White"
                            OnRowCreated="OnRowCreated" OnSelectedIndexChanged="SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <%#Container.DisplayIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Reference Id:">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_OtherId" runat="server" Text='<%# Eval("ReferId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Consultant/ Name:">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Consultant" runat="server" Text='<%# Eval("Consultant") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Agent Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Agent" runat="server" Text='<%# Eval("Agent") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PinCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_pin" runat="server" Text='<%# Eval("pincode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Address" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_City" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="District">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_District" runat="server" Text='<%# Eval("District") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_State" runat="server" Text='<%# Eval("State") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phone No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Phone" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Remark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                        <asp:Label ID="lbl_idno" runat="server" Visible="false" Text='<%# Eval("considno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="AddpopupRefer" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 5px; margin-left: 215px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <div style="background-color: White; height: 508px; font-family: Book Antiqua; font-weight: bold;
                        width: 458px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_Refer" runat="server" Text="Reference Entry For Others" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_cons" runat="server" Text=" Consultant/ Name:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtConsultant" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_id" runat="server" Text="Id:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel14" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_idval" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                           
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_agent" runat="server" Text="Agent Name:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TexAgent" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lab_Pincode" runat="server" Text="PinCode:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextPincode" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lab_add" runat="server" Text="Address:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Textadd" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_city" runat="server" Text="City:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextCity" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labeldis" runat="server" Text="District:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextDistrict" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labelst" runat="server" Text="State:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Textstate" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labelphone" runat="server" Text="Phone No:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Textphone" runat="server" MaxLength="16" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender89" runat="server" TargetControlID="Textphone"
                                                FilterType="Numbers,Custom" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labelemail" runat="server" Text="Email ID:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Textemail" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true" MaxLength="60" onchange="return validateCaseSensitiveEmail(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender90" runat="server" TargetControlID="Textemail"
                                                FilterType="Numbers, LowercaseLetters, Custom" ValidChars=".@_">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Labelremark" runat="server" Text="Remark:"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Textremk" runat="server" Style="width: 221px;" CssClass="textbox txtheight2"
                                                Enabled="true"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpSave" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                                    BackColor="LightGreen" OnClientClick="return valid1()" />
                                <asp:Button ID="btn_Delete" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Delete" OnClick="btn_Delete_Click" BackColor="Aquamarine" />
                                <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click"
                                    BackColor="Red" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
