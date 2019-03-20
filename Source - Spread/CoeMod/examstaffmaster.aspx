<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="examstaffmaster.aspx.cs" Inherits="examstaffmaster"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblmessage1').innerHTML = "";

        }
        function make_blank() {
            document.form1.type.value = "";
        }
    </script>
    <style type="text/css">
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 0px;
            border-style: solid;
            cursor: 'default';
            height: 100px;
            font-family: Book Antiqua;
            font-size: small;
            text-align: left;
            list-style-type: none;
            padding-left: 1px;
            width: 430px;
            overflow: auto;
            overflow-x: hidden;
        }
    </style>
    <script type="text/javascript">


        function depart() {

            document.getElementById('<%=btnplus.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnmins.ClientID%>').style.display = 'block';
            document.getElementById('<%=btndesignplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignmins.ClientID%>').style.display = 'none';

            document.getElementById('<%=btncityplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncitymins.ClientID%>').style.display = 'none';

            document.getElementById('<%=btnuniplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnunimins.ClientID%>').style.display = 'none';
        }
        function design() {
            document.getElementById('<%=btnplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnmins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncityplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncitymins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnuniplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnunimins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignplus.ClientID%>').style.display = 'block';
            document.getElementById('<%=btndesignmins.ClientID%>').style.display = 'block';
        }
        function city() {
            document.getElementById('<%=btnplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnmins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnuniplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnunimins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignmins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncityplus.ClientID%>').style.display = 'block';
            document.getElementById('<%=btncitymins.ClientID%>').style.display = 'block';
        }
        function instition() {
            document.getElementById('<%=btnplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnmins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btndesignmins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncityplus.ClientID%>').style.display = 'none';
            document.getElementById('<%=btncitymins.ClientID%>').style.display = 'none';
            document.getElementById('<%=btnuniplus.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnunimins.ClientID%>').style.display = 'block';
        }
    </script>
    <script type="text/javascript">
        function checkEmail(id) {
            var valu = id.value.trim();
            if (valu != '') {
                var emailPattern = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!emailPattern.test(valu)) {
                    id.value = '';
                }
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Exam Staff Master"></asp:Label></center>
    <br />
    <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        runat="server" Width="1000px" Height="60px" BackColor="White" BorderColor="White"
        Style="background: White;">
        <Panes>
            <asp:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    View</Header>
                <Content>
                    <center>
                        <table class="maintablestyle" style="height: 70px; background-color: #0CA6CA;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Stream" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstreamview" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="ddlstreamview_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Type" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txttype" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="106px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_Department" runat="server" CssClass="multxtpanel" Style="visibility: visible;"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="chktype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktype_checkedchanged" />
                                                <asp:CheckBoxList ID="checktype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="checktype_selectedchanged">
                                                    <asp:ListItem>Internal</asp:ListItem>
                                                    <asp:ListItem>External</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txttype"
                                                PopupControlID="panel_Department" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Examiner Type" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Width="125px"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtexaminer" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel2" runat="server" Style="visibility: hidden;" CssClass="multxtpanel"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="chk_examtpe" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_examtpe_checkedchanged" />
                                                <asp:CheckBoxList ID="check_examtype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="check_examtype_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtexaminer"
                                                PopupControlID="panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsession1" runat="server" Text="Session" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsession" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="F.N">F.N</asp:ListItem>
                                        <asp:ListItem Value="A.N">A.N</asp:ListItem>
                                        <asp:ListItem Value="F.N/A.N">Both</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbldept11" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_deptview" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="106px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel3" runat="server" CssClass="multxtpanel" Style="visibility: visible;"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="Checkdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="Checkdept_checkedchanged" />
                                                <asp:CheckBoxList ID="ddldept11" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldept11_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_deptview"
                                                PopupControlID="panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbldesign1" runat="server" Text="Design" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_designview" runat="server" ReadOnly="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="166px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel4" runat="server" CssClass="multxtpanel" Style="visibility: visible;"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="Checkdesign" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="Checkdesign_checkedchanged" />
                                                <asp:CheckBoxList ID="ddldept1" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldept1_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_designview"
                                                PopupControlID="panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblcity1" runat="server" Text="City" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_cityview" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="106px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel5" runat="server" CssClass="multxtpanel" Style="visibility: visible;"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="CheckBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="CheckBox1_checkedchanged" />
                                                <asp:CheckBoxList ID="ddlcity" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlcity_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_cityview"
                                                PopupControlID="panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblexpfrom" runat="server" Text="Exp Year From" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="115px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfromexp" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblexpto" runat="server" Text="Exp Year To" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddltoexp" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblinst" runat="server" Text="Institution" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_viewinstition" runat="server" ReadOnly="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="106px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel6" runat="server" CssClass="multxtpanel" Style="visibility: visible;"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="CheckBox2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="CheckBox2_checkedchanged" />
                                                <asp:CheckBoxList ID="ddlinstition" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlinstition_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_viewinstition"
                                                PopupControlID="panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px" Width="150px">
                                        <asp:CheckBox ID="chkboxsms" runat="server" Text="SMS" OnCheckedChanged="chkboxsms_CheckedChangeds"
                                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                            font-weight: bold;" Font-Names="Book Antiqua" AutoPostBack="true" />
                                        <asp:CheckBox ID="chkboxmail" runat="server" Text="MAIL" OnCheckedChanged="chkboxmail_CheckedChanged"
                                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                            font-weight: bold;" Font-Names="Book Antiqua" AutoPostBack="true" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btngo_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblerrormsg" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="fpcammarkstaff" runat="server" AutoPostBack="false" BorderColor="Black"
                        CssClass="cur" BorderStyle="Solid" BorderWidth="1px" Height="300" Width="1000"
                        Visible="False" OnUpdateCommand="fpcammarkstaff_UpdateCommand" OnButtonCommand="fpcammarkstaff_ButtonCommand"
                        OnCellClick="fpcammarkstaff_CellClick">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()">
                                </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btndelete" runat="server" Text="Delete" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btndelete_click" Height="25px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage1" runat="server" Text="" ForeColor="Red" Visible="False"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="labpurpose" runat="server" Text="Purpose" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlpurpose" runat="server" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Visible="false" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="fpspreadpurpose" runat="server" Visible="false" OnCellClick="fpspreadpurpose_CellClick"
                                    OnPreRender="fpspreadpurpose_SelectedIndexChanged">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                                        ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="false">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        Visible="false" BorderWidth="2px" Height="390px" Width="690px">
                        <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <table>
                                <caption>
                                    <br />
                                    <br />
                                    <br />
                                    <caption>
                                        Message Template</caption>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnplus1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnplus1_Click" Text=" + " />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlpurposemsg" runat="server" AutoPostBack="True" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnminus1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnminus1_Click" Text=" - " />
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                            Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnsave1" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnsave1_Click" Height=" 26px" Width=" 88px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexit_Click" />
                                    </td>
                                </tr>
                                <caption>
                                    <br />
                                    <br />
                                    <br />
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Red" Style="top: 340px; left: 5px; position: absolute;
                                                height: 21px" Width="676px"></asp:Label>
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        Visible="false" BorderWidth="2px" Height="100px" Width="300px">
                        <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                            font-weight: bold; height: 22px; font-family: 'Book Antiqua'; position: absolute;
                                            top: 21px; left: 10px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                            height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                            height: 26px;" OnClick="btnpurposeadd_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                            height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                                    </td>
                                </tr>
                            </table>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnaddtemplate" Visible="false" runat="server" Text="Add Template"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btndeletetemplate" runat="server" Visible="false" Text="Delete Template"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeletetemplate_Click" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblerror12" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtmessage" runat="server" TextMode="MultiLine" Height="200px" Width="500px"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnsms" runat="server" Text="Send" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnsms_Click" />
                            </td>
                        </tr>
                    </table>
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane2" runat="server" BackColor="White">
                <Header>
                    <asp:Label ID="AddPageModify" runat="server" Text="Add"></asp:Label></Header>
                <Content>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Stream" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstreamadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlstreamadd_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:Label ID="lblemptype" runat="server" Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlemptype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlemptype_SelectedIndexChanged">
                                    <asp:ListItem Value="1">External</asp:ListItem>
                                    <asp:ListItem Value="0">Internal</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtissueper" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="" Style="height: 20px;" AutoPostBack="true" OnTextChanged="txtissueper_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" MinimumPrefixLength="1"
                                    CompletionInterval="5" Enabled="True" EnableCaching="true" CompletionSetCount="12"
                                    ServiceMethod="GetCity12" TargetControlID="txtissueper" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnstaff" runat="server" Text="?" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnstaff_Click" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtstaff_co" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="" Style="opacity: 0; height: 0; width: 0;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblempno" runat="server" Text="Staff Code" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmrs" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True">
                                    <asp:ListItem>MR</asp:ListItem>
                                    <asp:ListItem>MRS</asp:ListItem>
                                    <asp:ListItem>MS</asp:ListItem>
                                    <asp:ListItem>DR</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="ddlempno" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" OnTextChanged="ddlempno_TextChanged">
                                </asp:TextBox>
                                <asp:TextBox ID="ddlempno1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" OnTextChanged="ddlempno1_TextChanged">
                                </asp:TextBox>
                                <asp:AutoCompleteExtender ID="TextBox2_AutoCompleteExtender" runat="server" MinimumPrefixLength="1"
                                    CompletionInterval="5" Enabled="True" EnableCaching="true" CompletionSetCount="12"
                                    ServiceMethod="GetCity" TargetControlID="ddlempno" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" MinimumPrefixLength="1"
                                    CompletionInterval="5" Enabled="True" EnableCaching="true" CompletionSetCount="12"
                                    ServiceMethod="GetCity1" TargetControlID="ddlempno1" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnplus" runat="server" Text="+" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Style="display: none;" OnClick="btnplus_Click" />
                            </td>
                            <td>
                                <asp:TextBox ID="ddldept" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" AutoPostBack="true" Width="156px">
                                    <%--OnSelectedIndexChanged="ddldept_SelectedIndexChanged"--%>
                                </asp:TextBox>
                                <asp:DropDownList ID="ddlexdept" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnmins" runat="server" Text="-" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Style="display: none;" OnClick="btnmins_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblgender" runat="server" Text="Gender" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlgender" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True">
                                    <asp:ListItem Value="MALE">M</asp:ListItem>
                                    <asp:ListItem Value="FEMALE">F</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbldesign" runat="server" Text="Designation" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btndesignplus" runat="server" Text="+" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Style="display: none;" OnClick="btndesignplus_Click" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtdesign" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:TextBox>
                                <asp:DropDownList ID="ddlexterdesign" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btndesignmins" runat="server" Text="-" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Style="display: none;" OnClick="btndesignmins_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblinstitu" runat="server" Text="Institution" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnuniplus" runat="server" Text="+" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Style="display: none;" OnClick="btnuniplus_Click" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtinstition" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:TextBox>
                                <asp:DropDownList ID="ddlextuniv" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnunimins" runat="server" Text="-" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Style="display: none;" OnClick="btnunimins_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbluniversity" runat="server" Text="University" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtuniversity" runat="server" Text="" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcrear" runat="server" Text="Carrer Started.Year" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Width="100px"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstatedyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" OnSelectedIndexChanged="ddlstatedyear_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label ID="lbljoinyear" runat="server" Text="Join.Year" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:DropDownList ID="ddlyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblyearofexp" runat="server" Text="Years of Exp." Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Width="70px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbladdress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddress1" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblscheme" runat="server" Text="Scheme" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlscheme" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddress2" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblphone" runat="server" Text="Phone" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtphone" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" MaxLength="10"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtphone"
                                    FilterType="Numbers" ValidChars=",/-() " />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddress3" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblmobile" runat="server" Text="Mobile" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtmobile" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" MaxLength="10"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtmobile"
                                    FilterType="Numbers" ValidChars=",/-() " />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcity" runat="server" Text="City" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btncityplus" runat="server" Text="+" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" OnClick="btncityplus_Click" Style="display: none;" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtcity" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlextcity" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btncitymins" runat="server" Text="-" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="True" OnClick="btncitymins_Click" Style="display: none;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblemail" runat="server" Text="Email" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtemil" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" onblur="checkEmail(this)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpincode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtpincode" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" MaxLength="6"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtpincode"
                                    FilterType="Numbers" ValidChars=",/-() " />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblFNAN" runat="server" Text="FN/AN" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="ddlFnAn" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="F.N/A.N" Selected>Both</asp:ListItem>
                                    <asp:ListItem Value="F.N">F.N</asp:ListItem>
                                    <asp:ListItem Value="A.N">A.N</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <%-- modified --%>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblTa" runat="server" Text="Travel Allowance(T.A) :" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txttravelallowance">
                                </asp:TextBox>
                                <asp:RangeValidator ID="RangeValidatortravelallowance" runat="server" ControlToValidate="txttravelallowance"
                                    ErrorMessage="Invalid" MaximumValue="10000" MinimumValue="0" Type="Integer">
                                </asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="LblDa" runat="server" Text="Daily Allowance (D.A) :" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtdailyallowance">
                                </asp:TextBox>
                                <asp:RangeValidator ID="RangeValidatordailyallowance" runat="server" ControlToValidate="txtdailyallowance"
                                    ErrorMessage="Invalid" MaximumValue="10000" MinimumValue="0" Type="Integer">
                                </asp:RangeValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="lblsetting" runat="server" Text="Setting" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="lblsetting_CheckedChanged">
                                </asp:CheckBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsettingtextbox" runat="server" ReadOnly="true" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="222px">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel9" runat="server" CssClass="multxtpanel" Style="height: 100px;
                                            visibility: visible; position: absolute; top: 300px; z-index: 1000; left: 69px;
                                            width: 222px;">
                                            <asp:CheckBox ID="chksetting" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksetting_checkedchanged" />
                                            <asp:CheckBoxList ID="ddlsetting" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="checksetting_selectedchanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtsettingtextbox"
                                            PopupControlID="panel9" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="lblvalution" runat="server" Text="Valuation" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="lblvalution_CheckedChanged">
                                </asp:CheckBox>
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtvalutationtextbox" runat="server" ReadOnly="true" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Width="222px">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel" Style="height: 100px;
                                            visibility: visible; position: absolute; top: 300px; z-index: 1000; left: 69px;">
                                            <asp:CheckBox ID="chkvalution" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkvalution_checkedchanged" />
                                            <asp:CheckBoxList ID="ddlvalution" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="checkvalution_selectedchanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtvalutationtextbox"
                                            PopupControlID="panel7" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--Added by saranya--%>
                        <tr id="trBank" runat="server" visible="false">
                            <td colspan="3">
                                <asp:Label ID="LblBkName" runat="server" Text="Bank Name :" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="TxtBkName" runat="server" Text="" Style="font-family: Book Antiqua;
                                    font-size: Medium; font-weight: bold; margin-left: 4px; width:200px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trBankIfsc" runat="server" visible="false">
                            <td colspan="3">
                                <asp:Label ID="LblIfsc" runat="server" Text="IFSC Code :" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="TxtIfsc" runat="server" Text="" Style="font-family: Book Antiqua;
                                    font-size: Medium; font-weight: bold; margin-left: 11px; width:200px;" MaxLength="11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trBankAcc" runat="server" visible="false">
                            <td colspan="3">
                                <asp:Label ID="LblAccNo" runat="server" Text="Account No. :" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="TxtAccNo" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Width="200px" MaxLength="18"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblerror1" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Panel ID="panelinvilation" runat="server">
                        <table class="tablfont" style="position=absolute;" align="center">
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="lblinvi" runat="server" Text="Inivigilation" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True"></asp:CheckBox>
                                    <asp:CheckBox ID="lblscred" runat="server" Text="Scrap" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True"></asp:CheckBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table class="tablfont" style="margin-left: 544px; position=absolute;" align="right">
                        <tr>
                            <td>
                                <asp:Button ID="btnnew" runat="server" Text="New" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnnew_click" Width="50px" Height="25px" />
                            </td>
                            <td>
                                <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnsave_click" Width="60px" Height="25px" />
                            </td>
                            <td>
                                <asp:Button ID="Btnedit" runat="server" Text="Exit" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="Btnedit_click" Width="70px" Height="25px" />
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnfoucs" runat="server" Style="opacity: 0;" />
                </Content>
            </asp:AccordionPane>
        </Panes>
    </asp:Accordion>
    <asp:Panel ID="panel8" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
        BorderWidth="2px" Style="background-color: AliceBlue; border-color: Black; border-width: 2px;
        border-style: solid; position: fixed; width: 780px; height: 440px; left: 250px;
        top: 99px;">
        <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
            font-size: Small; font-weight: bold">
            <br />
            <asp:Label ID="Label19" runat="server" Text=" Staff List" Style="width: 150px; position: absolute;
                left: 166px; top: 4px;"></asp:Label>
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="College" Style="width: 150px; position: absolute;
                                    left: -41px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" Style="width: 150px;
                                    position: absolute; left: 70px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDepartment" runat="server" Text="Department" Style="width: 150px;
                                    position: absolute; left: 237px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldepratstaff" runat="server" AutoPostBack="true" Width="150px"
                                    OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged" Style="width: 150px;
                                    position: absolute; left: 360px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Staff Type" Style="width: 150px; position: absolute;
                                    left: -41px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_stftype" runat="server" Width="150px" AutoPostBack="true"
                                    Style="width: 150px; position: absolute; left: 70px; top: 65px;" OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Designation" Style="width: 150px; position: absolute;
                                    left: 237px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_design" runat="server" Width="150px" AutoPostBack="true"
                                    Style="width: 150px; position: absolute; left: 360px; top: 65px;" OnSelectedIndexChanged="ddl_design_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblstaffstream" runat="server" Text="Stream" Style="width: 150px;
                                    position: absolute; left: 460px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaffstream" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaffstream_SelectedIndexChanged"
                                    AutoPostBack="true" Style="width: 150px; position: absolute; left: 560px; top: 65px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" Visible="false" AutoPostBack="true">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" Visible="false" AutoPostBack="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 510px; position: absolute; top: 95px;">
                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                            Width="510" VerticalScrollBarPolicy="AsNeeded" OnButtonCommand="fsstaff_ButtonCommand"
                            BorderWidth="0.5" Visible="False">
                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:CheckBox ID="chkinivigilation" runat="server" Text="Inivigilation" Style="width: 150px;
                position: absolute; left: 20px; top: 398px;" />
            <fieldset style="width: 160px; position: absolute; padding: 4px 1em 19px; left: 328px;
                height: 9px; top: 388px;">
                <asp:Button runat="server" ID="btnstaffadd" OnClick="btnstaffadd_Click" Text="Ok"
                    Width="75px" />
                <asp:Button runat="server" ID="btnexitpop" OnClick="btnexitpop_Click" Text="Exit"
                    Width="75px" />
            </fieldset>
    </asp:Panel>
    <asp:Panel ID="paneldept" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
        Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium"
        Width="450px" Height="100px" Style="position: absolute; left: 598px; top: 352px;">
        <center>
            <table>
                <tr>
                    <td>
                        <caption runat="server" id="capdepart" title="Leave Reason">
                        </caption>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_dept" Width="400px" Height="20px" runat="server" Font-Names="Book Antiqua"
                            MaxLength="50">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_dept"
                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnadd1" Width="50px" runat="server" Text="Add" OnClick="btnadd1_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                        &nbsp;
                        <asp:Button ID="btnexit1" Width="50px" runat="server" Text="Exit" OnClick="btnexit1_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Panel ID="panel1_sedign" runat="server" Visible="false" BorderStyle="Solid"
        BorderWidth="1px" Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua"
        Font-Size="Medium" Width="450px" Height="100px" Style="position: absolute; left: 598px;
        top: 383px;">
        <center>
            <table>
                <tr>
                    <td>
                        <caption runat="server" id="Capdegina" title="Leave Reason">
                        </caption>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_design" Width="400px" Height="20px" runat="server" Font-Names="Book Antiqua"
                            MaxLength="50">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_design"
                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_designadd" Width="50px" runat="server" Text="Add" OnClick="btn_designadd_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                        &nbsp;
                        <asp:Button ID="btn_designexit" Width="50px" runat="server" Text="Exit" OnClick="btn_designexit_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Panel ID="pnlcity" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
        Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium"
        Width="450px" Height="100px" Style="position: absolute; left: 291px; top: 566px;">
        <center>
            <table>
                <tr>
                    <td>
                        <caption runat="server" id="capcity" title="Leave Reason">
                        </caption>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_city" Width="400px" Height="20px" runat="server" Font-Names="Book Antiqua"
                            MaxLength="50">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_city"
                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_cityadd" Width="50px" runat="server" Text="Add" OnClick="btn_cityadd_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                        &nbsp;
                        <asp:Button ID="btn_cityexit" Width="50px" runat="server" Text="Exit" OnClick="btn_cityexit_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Panel ID="pan_instition" runat="server" Visible="false" BorderStyle="Solid"
        BorderWidth="1px" Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book Antiqua"
        Font-Size="Medium" Width="535px" Height="100px" Style="position: absolute; left: 291px;
        top: 412px;">
        <center>
            <table>
                <tr>
                    <td>
                        <center>
                            <asp:Label ID="Capins" runat="server" Text="Leave Reason">
                            </asp:Label>
                            <%-- <caption runat="server" id="Caption1" title="Leave Reason">
                        </caption>--%>
                        </center>
                    </td>
                    <td>
                        <center>
                            <asp:Label ID="Label5" runat="server" Text="KM">
                            </asp:Label>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_instition" Width="400px" Height="20px" runat="server" Font-Names="Book Antiqua"
                            MaxLength="50">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_instition"
                            FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                    </td>
                    <td>
                        <asp:TextBox ID="Text_km" Width="100px" Height="20px" runat="server" Font-Names="Book Antiqua"
                            MaxLength="50">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="Text_km"
                            FilterType="Numbers,Custom" ValidChars="." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_institionadd" Width="50px" runat="server" Text="Add" OnClick="btn_institionadd_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                        &nbsp;
                        <asp:Button ID="btn_institionexit" Width="50px" runat="server" Text="Exit" OnClick="btn_institionexit_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
</asp:Content>
