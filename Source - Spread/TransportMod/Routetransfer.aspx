<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Routetransfer.aspx.cs" Inherits="Routtransfer" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <body>

    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 70px;
            left: -16px; position: absolute; width: 1025px; height: 21px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Route Transfer"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Back</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" CausesValidation="False">Logout</asp:LinkButton>--%>
        </asp:Panel>
        <br />
        <br />
        <fieldset style="width: 990px; height: 90px; position: absolute; top: 90px; left: 5px;">
            <asp:Label ID="lblsource" Text="Source" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 1px; left: 10px; position: absolute;"></asp:Label>
            <fieldset style="width: 975px; height: 65px; position: absolute; top: 5px; left: 5px;
                border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                background-color: lightblue; border-width: 1px;">
                <asp:Label ID="lblcollege" Text="College" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 20px; left: 10px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Width="400px" Style="top: 20px;
                    left: 80px; position: absolute;">
                </asp:DropDownList>
                <asp:RadioButton ID="rbstudent" runat="server" AutoPostBack="true" OnCheckedChanged="rbstudent_CheckedChange"
                    Text="Student" GroupName="stustaf" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 20px; left: 490px; position: absolute;" />
                <asp:RadioButton ID="rbstaff" runat="server" Text="Staff" GroupName="stustaf" AutoPostBack="true"
                    OnCheckedChanged="rbstaff_CheckedChange" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 20px; left: 600px; position: absolute;" />
                <asp:Label ID="lblbatch" Text="batch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 60px; left: 10px; position: absolute;"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="130px" Style="top: 60px; left: 130px; position: absolute; font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pbatch" runat="server" Width="120px" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbatch_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                            PopupControlID="pbatch" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lbldegree" Text="Degree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 60px; left: 275px; position: absolute;"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="130px" Style="top: 60px; left: 380px; position: absolute; font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pdegree" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" OnCheckedChanged="chkdegree_ChekedChange"
                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdegree_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                            PopupControlID="pdegree" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblbranch" Text="Branch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 60px; left: 520px; position: absolute;"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="130px" Style="top: 60px; left: 620px; position: absolute; font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pbranch" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" OnCheckedChanged="chkbranch_ChekedChange"
                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbranch_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                            PopupControlID="pbranch" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblstrtplace" Text="Boarding" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 60px; left: 760px; position: absolute;"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtstrplace" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="100px" Style="top: 60px; left: 840px; position: absolute; font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pstrplace" runat="server" Width="160px" CssClass="multxtpanel multxtpanleheight">
                            <asp:CheckBox ID="chkstrplace" runat="server" Font-Bold="True" OnCheckedChanged="chkstrplace_ChekedChange"
                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                            <asp:CheckBoxList ID="chklsstrplace" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsstrplace_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstrplace"
                            PopupControlID="pstrplace" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                <asp:Button ID="btngo" Text="Go" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 57px; left: 950px; position: absolute;" OnClick="btngo_Click" />
                     </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </fieldset>
        <fieldset style="width: 990px; height: 45px; position: absolute; top: 220px; left: 5px;">
            <asp:Label ID="Label2" Text="Destination" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 1px; left: 10px; position: absolute;"></asp:Label>
            <fieldset style="width: 975px; height: 20px; position: absolute; top: 20px; left: 1px;
                border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                background-color: lightblue; border-width: 1px;">
                <asp:Label ID="lblboarding" Text="Boarding" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 10px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlboarding" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlboarding_SelectedIndexChanged"
                    Font-Bold="true" Font-Names="Book Antiqua" Width="130px" Font-Size="Medium" Style="top: 10px;
                    left: 90px; position: absolute;">
                </asp:DropDownList>
                <asp:Label ID="lblroute" Text="Route" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 230px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlroute" runat="server" Font-Bold="true" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlroute_SelectedIndexChanged" Font-Names="Book Antiqua"
                    Width="130px" Font-Size="Medium" Style="top: 10px; left: 280px; position: absolute;">
                </asp:DropDownList>
                <asp:Label ID="lblvechicle" Text="Vehicle" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 420px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddlvechile" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Width="100px" Font-Size="Medium" Style="top: 10px; left: 480px; position: absolute;">
                </asp:DropDownList>
                <asp:Label ID="lbltype" Text="Type" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 590px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="ddltype" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Width="100px" Font-Size="Medium" Style="top: 10px; left: 640px; position: absolute;">
                    <asp:ListItem Text="Semester"></asp:ListItem>
                    <asp:ListItem Text="Yearly"></asp:ListItem>
                    <asp:ListItem Text="Monthly"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblfeecat" Text="Fee Category" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 750px; position: absolute;"></asp:Label>
                <asp:DropDownList ID="fee_cate" runat="server" Width="150px" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="top: 10px; left: 850px; position: absolute;">
                </asp:DropDownList>
                <%--    <asp:RadioButton ID="rbsem" runat="server" Font-Bold="true" Text="Sem" Font-Names="Book Antiqua"
                Font-Size="Medium" GroupName="Feecat" Style="top: 20px; left: 720px; position: absolute;"/>
            <asp:RadioButton ID="rbyear" runat="server" Font-Bold="true" Text="Yearly" Font-Names="Book Antiqua"
                Font-Size="Medium" GroupName="Feecat" Style="top: 20px; left: 780px; position: absolute;"/>
            <asp:RadioButton ID="rbmonth" runat="server" Font-Bold="true" Text="Monthly" Font-Names="Book Antiqua"
                Font-Size="Medium" GroupName="Feecat" Style="top: 20px; left: 860px; position: absolute;"/>--%>
            </fieldset>
        </fieldset>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Style="margin-left: 0px; top: 285px; left: -4px; position: absolute; width: 1088px;">
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"
            Font-Names="Book Antiqua" Visible="false"></asp:Label>
        <br />
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Width="900" Height="500" VerticalScrollBarPolicy="Never">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
        <fieldset id="Fieldset2" runat="server" style="position: absolute; left: 25px; height: 19px;
            width: 280px;">
            <asp:Label ID="Label1" runat="server" Text="Select" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="fromno" runat="server" Style="position: absolute; left: 65px; width: 53px;"
                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                FilterType="Numbers" />
            <asp:Label ID="lblto" runat="server" Text="To" Style="position: absolute; left: 128px;"
                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="tono" runat="server" Style="position: absolute; left: 160px; width: 53px;"
                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                FilterType="Numbers" />
            <asp:Button ID="Button2" runat="server" Text="Go" Style="left: 230px; position: absolute;"
                OnClick="selectgo_Click" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
        </fieldset>
        <asp:Button ID="btntransfer" runat="server" Text="Transfer" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Medium" Style="position: absolute; left: 350px; top: 850px" OnClick="btntransfer_Click" />

            
        <br />
        <br />
         

  
        <table>
       
            <tr>
                <td>
           
     
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
   
        
         <center>
                        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 338px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                                        <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                                     </center>
                                                </td>

                                               
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
            </center>   

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
    </body>
</asp:Content>
