<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="biomatric.aspx.cs" Inherits="biomatric" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .style1
        {
            width: 85%;
            height: 29px;
        }
        .style158
        {
            width: 109px;
            height: 31px;
        }
        .styles
        {
        }
        .style245
        {
            font-family: "Book Antiqua";
            font-size: medium;
        }
        .style275
        {
            width: 26px;
            height: 7px;
        }
        #form1
        {
            height: 891px;
            width: 797px;
        }
        .style315
        {
            height: 31px;
            width: 51px;
        }
        .style324
        {
            width: 100%;
        }
        .style325
        {
            width: 31px;
        }
        .style326
        {
            width: 89px;
        }
        .style327
        {
            width: 107px;
        }
        .style328
        {
            width: 122px;
        }
        .style344
        {
            width: 33px;
        }
        .style361
        {
            width: 112px;
        }
        .style362
        {
            width: 29px;
        }
        .style370
        {
            width: 63px;
            height: 31px;
        }
        .style449
        {
            width: 113px;
            height: 31px;
        }
        .style451
        {
            height: 7px;
            width: 36px;
        }
        .style456
        {
            height: 31px;
        }
        .style457
        {
            height: 31px;
            width: 36px;
        }
        .style458
        {
            height: 31px;
            width: 30px;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width: 1000px;
        }
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .style461
        {
            height: 31px;
            width: 14px;
        }
        .style462
        {
            width: 79px;
        }
        .style463
        {
            width: 15px;
        }
        .style464
        {
            width: 354px;
        }
        .style465
        {
            width: 198px;
            height: 26px;
        }
        .style466
        {
            height: 26px;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            var chngPosition1 = $find('ddeblood')._dropPopupPopupBehavior;
            chngPosition1.set_positioningMode(2);
        }
    </script>
    <body>
        <div style="height: 83px">
            <br />
            <asp:Panel ID="Panel5" runat="server" BackImageUrl="~/bioimage/Biomatric_New.jpg"
                Height="136px" Width="1006px" Style="margin-left: -16px;">
                <br />
            </asp:Panel>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style456">
                </td>
                <td class="style457">
                    <asp:Image ID="Image19" runat="server" Height="20px" ImageUrl="~/bioimage/Date.jpg"
                        Width="91px" />
                </td>
                <td class="style315">
                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="From:" Style="font-family: 'Book Antiqua'"
                        Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style461" colspan="3">
                    <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="16px"
                        Width="75px" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txtentryfrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:RequiredFieldValidator ID="regdate1" runat="server" ControlToValidate="Txtentryfrom"
                        ErrorMessage="Please enter the Date" ForeColor="#FF3300" Style="top: 261px; left: 371px;
                        position: absolute; height: 26px; width: 131px"></asp:RequiredFieldValidator>
                </td>
                <td class="style458">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="To:" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td class="style158" colspan="3">
                    <asp:TextBox ID="Txtentryto" runat="server" OnTextChanged="Txtentryto_TextChanged"
                        Height="17px" Width="75px" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:RequiredFieldValidator ID="reqdateto" runat="server" ControlToValidate="Txtentryto"
                        ErrorMessage="Please enter the  to Date" ForeColor="Red" Style="top: 274px; left: 504px;
                        position: absolute; height: 16px; width: 161px"></asp:RequiredFieldValidator>
                </td>
                <td class="style449">
                    <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Style="position: absolute; top: 228px; left: 114px;" ForeColor="Red" Visible="False"></asp:Label>
                </td>
                <td class="style370">
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                </td>
            </tr>
        </table>
        <table class="style1" style="margin-left: 0px;">
            <tr>
                <td>
                    <asp:RadioButton ID="rdodaily" runat="server" OnCheckedChanged="rdodaily_CheckedChanged"
                        AutoPostBack="true" Width="19px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="20px" GroupName="a" />
                </td>
                <td>
                    <asp:Image ID="Image17" runat="server" ImageUrl="~/bioimage/Daily Report.jpg" Height="20px"
                        Width="98px" />
                </td>
                <td>
                    <asp:RadioButton ID="rdoinout" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Size="Medium" OnCheckedChanged="rdoinout_CheckedChanged" Checked="True"
                        Font-Names="Book Antiqua" Width="18px" Height="16px" GroupName="a" />
                </td>
                <td>
                    <asp:Image ID="Image9" runat="server" Height="20px" ImageUrl="~/bioimage/In out.gif"
                        Width="81px" />
                </td>
                <td>
                    <asp:RadioButton ID="rdoinonly" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnCheckedChanged="rdoinonly_CheckedChanged" AutoPostBack="true"
                        Width="16px" GroupName="a" />
                </td>
                <td>
                    <asp:Image ID="Image15" runat="server" ImageUrl="~/bioimage/In Only.jpg" Height="20px" />
                </td>
                <td>
                    <asp:RadioButton ID="rdooutonly" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnCheckedChanged="rdooutonly_CheckedChanged" AutoPostBack="true"
                        Width="10px" GroupName="a" />
                </td>
                <td>
                    <asp:Image ID="Image16" runat="server" ImageUrl="~/bioimage/Out Only.jpg" Style="margin-left: 8px"
                        Height="20px" Width="83px" />
                </td>
                <td>
                    <asp:RadioButton ID="rdounreg" runat="server" GroupName="a" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="16px" Height="16px" AutoPostBack="true" OnCheckedChanged="rdounreg_CheckedChanged" />
                </td>
                <td>
                    <asp:Image ID="Image20" runat="server" ImageUrl="~/bioimage/Un Register Staff.jpg"
                        Height="20px" Width="86px" />
                </td>
                <td>
                    <asp:RadioButton ID="rdoboth1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="a" Text="Both" Height="22px" OnCheckedChanged="rdoboth1_CheckedChanged"
                        AutoPostBack="true" Width="95px" />
                </td>
                <td>
                    <asp:RadioButton ID="rbdailylog" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="a" Text="Daily Logs" Height="22px" OnCheckedChanged="rdodaily_CheckedChanged"
                        AutoPostBack="true" Width="120px" />
                </td>
            </tr>
        </table>
        <table class="style1" style="margin-left: -0px;">
            <tr>
                <td>
                    <asp:CheckBox ID="Chktimein" runat="server" Font-Bold="True" OnCheckedChanged="Chktimein_CheckedChanged"
                        Width="16px" Height="17px" Font-Names="Book Antiqua" BackColor="Transparent"
                        AutoPostBack="True" />
                </td>
                <td class="style451">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/bioimage/In Time.jpg" Height="20px"
                        Width="81px" />
                </td>
                <td>
                    <asp:Label ID="lblfrom" runat="server" Font-Bold="True" Text="From" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td class="style275">
                    <asp:DropDownList ID="cbo_hrtin" runat="server" Width="40px" Font-Bold="False" Font-Names="Book Antiqua"
                        Height="20px" Enabled="False">
                        <asp:ListItem>Hours</asp:ListItem>
                        <asp:ListItem> 1</asp:ListItem>
                        <asp:ListItem> 2</asp:ListItem>
                        <asp:ListItem> 3</asp:ListItem>
                        <asp:ListItem> 4</asp:ListItem>
                        <asp:ListItem> 5</asp:ListItem>
                        <asp:ListItem> 6</asp:ListItem>
                        <asp:ListItem> 7</asp:ListItem>
                        <asp:ListItem> 8</asp:ListItem>
                        <asp:ListItem> 9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_mintimein" runat="server" Width="40px" Font-Bold="False"
                        Font-Names="Book Antiqua" Height="20px" Enabled="False">
                        <asp:ListItem>Min</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
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
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="Requiredtimein" runat="server" ControlToValidate="cbo_hours"
                        ErrorMessage="please choose the time" Font-Bold="True" ForeColor="Red" Style="top: 264px;
                        left: 395px; position: absolute; height: 27px; width: 192px"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_in" runat="server" Width="40px" Font-Bold="False" Font-Names="Book Antiqua"
                        Height="20px" Enabled="False">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblto" runat="server" Font-Bold="True" Text="To:" Font-Names="Book Antiqua"
                        CssClass="style245"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_hrinto" runat="server" Width="40px" Font-Bold="False" Font-Names="Book Antiqua"
                        Height="20px" Enabled="False">
                        <asp:ListItem>Hours:</asp:ListItem>
                        <asp:ListItem> 1</asp:ListItem>
                        <asp:ListItem> 2</asp:ListItem>
                        <asp:ListItem> 3</asp:ListItem>
                        <asp:ListItem> 4</asp:ListItem>
                        <asp:ListItem> 5</asp:ListItem>
                        <asp:ListItem> 6</asp:ListItem>
                        <asp:ListItem> 7</asp:ListItem>
                        <asp:ListItem> 8</asp:ListItem>
                        <asp:ListItem> 9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_mininto" runat="server" Width="40px" Height="20px" Font-Bold="false"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>Min</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
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
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbointo" runat="server" Width="40px" Height="20px" Style="margin-left: 0px"
                        Font-Bold="false" Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="Chktimeout" runat="server" Font-Bold="True" OnCheckedChanged="Chktimeout_CheckedChanged"
                        Height="16px" Width="16px" Font-Names="Book Antiqua" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/bioimage/Out Time.jpg" Height="20px"
                        Width="81px" />
                </td>
                <td>
                    <asp:Label ID="Lblfrm" runat="server" Font-Bold="True" Text="From:" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_hours" runat="server" Width="41px" Font-Bold="false" Height="20px"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>Hours:</asp:ListItem>
                        <asp:ListItem> 1</asp:ListItem>
                        <asp:ListItem> 2</asp:ListItem>
                        <asp:ListItem> 3</asp:ListItem>
                        <asp:ListItem> 4</asp:ListItem>
                        <asp:ListItem> 5</asp:ListItem>
                        <asp:ListItem> 6</asp:ListItem>
                        <asp:ListItem> 7</asp:ListItem>
                        <asp:ListItem> 8</asp:ListItem>
                        <asp:ListItem> 9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_min" runat="server" Width="41px" Font-Bold="false" Height="20px"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>Min</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
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
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_sec" runat="server" Width="40px" Font-Bold="false" Height="20px"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltoutto" runat="server" Font-Bold="True" Text="To:" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_hour2" runat="server" Width="40px" OnSelectedIndexChanged="cbo_hour2_SelectedIndexChanged"
                        Font-Bold="false" Height="20px" Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>Hours:</asp:ListItem>
                        <asp:ListItem> 1</asp:ListItem>
                        <asp:ListItem> 2</asp:ListItem>
                        <asp:ListItem> 3</asp:ListItem>
                        <asp:ListItem> 4</asp:ListItem>
                        <asp:ListItem> 5</asp:ListItem>
                        <asp:ListItem> 6</asp:ListItem>
                        <asp:ListItem> 7</asp:ListItem>
                        <asp:ListItem> 8</asp:ListItem>
                        <asp:ListItem> 9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_min2" runat="server" Width="41px" Font-Bold="false" Height="20px"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>Min</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
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
                        <asp:ListItem>47</asp:ListItem>
                        <asp:ListItem>48</asp:ListItem>
                        <asp:ListItem>49</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>51</asp:ListItem>
                        <asp:ListItem>52</asp:ListItem>
                        <asp:ListItem>53</asp:ListItem>
                        <asp:ListItem>54</asp:ListItem>
                        <asp:ListItem>55</asp:ListItem>
                        <asp:ListItem>56</asp:ListItem>
                        <asp:ListItem>57</asp:ListItem>
                        <asp:ListItem>58</asp:ListItem>
                        <asp:ListItem>59</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cbo_sec2" runat="server" Height="20px" Width="41px" Font-Bold="False"
                        Font-Names="Book Antiqua" Enabled="False">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <table class="style1" style="margin-left: 0px;">
            <tr>
                <td class="style466">
                </td>
                <td class="style466">
                    <asp:Image ID="Image21" runat="server" ImageUrl="~/bioimage/Attendance.jpg" />
                </td>
                <td class="style465">
                    <contenttemplate>
                   <asp:TextBox ID="TextBox1" runat="server" Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="100px" style="top: 369px; left: 134px; height: 20px; width: 121px; font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox> 
                <asp:Panel ID="pnlCustomers" runat="server" CssClass="multxtpanel" 
                    Height="157px" Width="191px">
                        <asp:CheckBox ID="SelectAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"  oncheckedchanged="SelectAll_CheckedChanged" Text="Select All"  AutoPostBack="True" />
                    <asp:CheckBoxList ID="cbo_att" runat="server" 
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="115px" Width="172px" AutoPostBack="True"  onselectedindexchanged="cbo_att_SelectedIndexChanged" >                     
                        <asp:ListItem Value="0" Selected ="false">P</asp:ListItem>
                        <asp:ListItem Value="1" Selected ="false">A</asp:ListItem>
                        <asp:ListItem Value="2" Selected ="false">LA</asp:ListItem>
                        <asp:ListItem Value="3" Selected ="false">PER</asp:ListItem>  
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="TextBox1"
                    PopupControlID="pnlCustomers" Position="Bottom">
                </asp:PopupControlExtender>
                </contenttemplate>
                </td>
                <td class="style466">
                    <asp:RadioButton ID="rdomorning" runat="server" Font-Bold="True" Text="Morning" OnCheckedChanged="rdomorning_CheckedChanged"
                        GroupName="sd" Width="89px" Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" />
                </td>
                <td class="style466">
                    <asp:RadioButton ID="rdoevening" runat="server" Font-Bold="True" Text="Evening" OnCheckedChanged="rdoevening_CheckedChanged"
                        GroupName="sd" Width="86px" Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" />
                </td>
                <td class="style466">
                    <asp:RadioButton ID="rdoboth" runat="server" Font-Bold="True" Text="Both" OnCheckedChanged="rdoboth_CheckedChanged"
                        GroupName="sd" Height="18px" Width="58px" Checked="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" />
                </td>
                <td class="style466">
                    <asp:RadioButton ID="rdoall" runat="server" Font-Bold="True" Text="ALL" OnCheckedChanged="rdoall_CheckedChanged"
                        GroupName="sd" Width="68px" Font-Names="Book Antiqua" Font-Size="Medium" Height="19px" />
                </td>
                <td class="style466">
                    <asp:ImageButton ID="ImageButton1" runat="server" BorderWidth="1px" Height="28px"
                        ImageUrl="~/bioimage/Search Button_1.jpg" Width="103px" OnClick="ImageButton1_Click" />
                </td>
                <td class="style466">
                    <asp:Button ID="btnprint" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnprint_Click" Text="Export Excel" />
                </td>
                <td>
                    <asp:Label ID="lblorder" runat="server" Text="Order By" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="70px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlorder" runat="server" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                        <asp:ListItem Text="Dept & Staff Code"></asp:ListItem>
                        <asp:ListItem Text="Priority"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Image ID="Image4" runat="server" ImageUrl="~/JPG_Biometric/Band.jpg" Height="10px"
            Width="1000px" Style="margin-left: -0px;" />
        <table class="style1" style="margin-left: -0px;">
            <tr>
                <td>
                </td>
                <td>
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/bioimage/Department.jpg" Height="20px"
                        Width="91px" />
                </td>
                <td class="style464">
                    <div id="castediv" runat="server" class="linkbtn">
                        <asp:TextBox ID="tbseattype" runat="server" Height="16px" ReadOnly="true" Width="135px"
                            OnTextChanged="tbseattype_TextChanged" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <br />
                    </div>
                    <asp:Panel ID="pseattype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="300px" ScrollBars="Vertical" Width="350px">
                        <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged"
                            Text="Select All" />
                        <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="235px" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Height="102px"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddeseattype" runat="server" DropDownControlID="pseattype"
                        DynamicServicePath="" Enabled="true" TargetControlID="tbseattype">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/bioimage/Category.jpg" Height="20px"
                        Width="81px" Style="margin-bottom: 0px" />
                </td>
                <td>
                    <asp:TextBox ID="tbblood" runat="server" Height="20px" OnTextChanged="tbblood_TextChanged"
                        ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                    <asp:Panel ID="pblood" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="150px" ScrollBars="Auto" Width="190px" Style="font-family: 'Book Antiqua'">
                        <asp:CheckBox ID="chkcategory" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged"
                            Text="Select All " />
                        <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                            Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddeblood" runat="server" DropDownControlID="pblood" DynamicServicePath=""
                        Enabled="true" TargetControlID="tbblood">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/bioimage/Staff Name.jpg" Height="20px"
                        Width="91px" Style="margin-left: 0px" />
                </td>
                <td>
                    <asp:DropDownList ID="cbostaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Width="55px" Font-Size="Medium" Height="21px" OnSelectedIndexChanged="cbostaffname_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/bioimage/Staff Code.jpg" Height="20px"
                        Width="91px" />
                </td>
                <td>
                    <asp:DropDownList ID="cbostaffcode" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Width="54px" Font-Size="Medium" Height="19px" OnSelectedIndexChanged="cbostaffcode_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldesig" runat="server" Text="Designation" Font-Bold="true" Font-Size="Medium"
                        Width="80px"></asp:Label>
                </td>
                <td>
                    <contenttemplate>
                    <asp:TextBox ID="txt_desig" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                        Width="120px" Height="17px" Font-Bold="True" 
                Font-Names="Book Antiqua" 
                         Font-Size="Medium" 
                OnTextChanged="txt_desig_TextChanged">---Select---</asp:TextBox>
                    <asp:Panel ID="pdesig" runat="server" CssClass="multxtpanel" Height="300px"
                        Style="position: absolute;">
                        <asp:CheckBox ID="chk_desig" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                            OnCheckedChanged="chk_desig_ChekedChanged" Font-Size="Medium" Text="Select All"
                            AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklst_desig" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklst_desig_SelectedIndexChanged" Height="200px" Font-Bold="True"
                            Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="pextenddeisg" runat="server" TargetControlID="txt_desig"
                        PopupControlID="pdesig" Position="Bottom">
                    </asp:PopupControlExtender>
                </contenttemplate>
                </td>
            </tr>
        </table>
        <div>
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#0CA6CA" Height="14px" Style="margin-left: 0px;">
                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="pbodyfilter" runat="server">
                <table>
                    <tr>
                        <asp:TextBox ID="tborder" Visible="false" TextMode="MultiLine" AutoPostBack="true"
                            runat="server">
                        </asp:TextBox>
                        <td>
                            <asp:CheckBoxList ID="cblsearch" runat="server" OnSelectedIndexChanged="cblsearch_SelectedIndexChanged1"
                                Height="20px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                RepeatColumns="7" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">Staff Code</asp:ListItem>
                                <asp:ListItem Value="1">Staff Name</asp:ListItem>
                                <asp:ListItem Value="2">Department</asp:ListItem>
                                <asp:ListItem Value="3">Dept Acronym</asp:ListItem>
                                <asp:ListItem Value="4">Designation</asp:ListItem>
                                <asp:ListItem Value="5">Desig Acronym</asp:ListItem>
                                <asp:ListItem Value="6">Date Of Joining</asp:ListItem>
                                <asp:ListItem Value="7">Category</asp:ListItem>
                                <asp:ListItem Value="8">Att Till Percentage</asp:ListItem>
                                <asp:ListItem Value="9">In Time</asp:ListItem>
                                <asp:ListItem Value="10">Out Time</asp:ListItem>
                                <asp:ListItem Value="11">Total Hrs</asp:ListItem>
                                <asp:ListItem Value="12">Morning</asp:ListItem>
                                <asp:ListItem Value="13">Evening</asp:ListItem>
                                <asp:ListItem Value="14">Manual</asp:ListItem>
                                <asp:ListItem Value="15">Leave</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButtonsremove" Font-Size="X-Small" Visible="false" runat="server"
                                Width="111px" OnClick="LinkButtonsremove_Click" Height="16px" Style="font-family: 'Book Antiqua';
                                font-weight: 700; font-size: small; margin-left: 0px;">Remove  All</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpefilter" runat="server" TargetControlID="pbodyfilter"
                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
        <br />
        <table class="style324">
            <tr>
                <td class="style327">
                    <asp:ImageButton ID="Imglate" runat="server" Height="20px" ImageUrl="~/bioimage/Morning Late.jpg"
                        OnClick="Imglate_Click" Visible="False" />
                </td>
                <td class="style362">
                    <asp:Button ID="late" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="DarkRed" OnClick="late_Click" BackColor="Transparent" BorderWidth="0px"
                        Height="23px" Style="margin-left: 3px" Width="30px" Visible="False" />
                </td>
                <td class="style361">
                    <asp:ImageButton ID="Imgmorper" runat="server" Height="20px" ImageUrl="~/bioimage/Morning Permission.jpg"
                        OnClick="Imgmorper_Click" Width="116px" Visible="False" />
                </td>
                <td class="style344">
                    <asp:Button ID="lblpermission" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Purple" OnClick="lblpermission_Click" BackColor="Transparent"
                        BorderWidth="0px" Height="27px" Width="30px" Visible="False" />
                </td>
                <td class="style326">
                    <asp:ImageButton ID="Imgeveper" runat="server" Height="20px" ImageUrl="~/bioimage/Evening Permission.jpg"
                        OnClick="Imgeveper_Click" Visible="False" Width="121px" />
                </td>
                <td class="style325">
                    <asp:Button ID="lblevngpermission" runat="server" Font-Bold="True" Font-Size="Medium"
                        ForeColor="Chocolate" OnClick="lblevngpermission_Click" Height="24px" Width="30px"
                        BackColor="Transparent" BorderWidth="0px" Visible="False" />
                </td>
                <td class="style328">
                    <asp:ImageButton ID="Imgood" runat="server" Height="20px" ImageUrl="~/bioimage/Staff in OOD.jpg"
                        Width="114px" OnClick="Imgood_Click2" Style="margin-left: 2px" Visible="False" />
                </td>
                <td class="style462">
                    <asp:Button ID="lblood" runat="server" BackColor="Transparent" BorderWidth="0px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="DarkSeaGreen"
                        OnClick="lblood_Click" Height="22px" Style="margin-left: 0px" Width="30px" Visible="False" />
                </td>
                <td class="style463">
                    <asp:ImageButton ID="btngracetime" runat="server" Height="20px" ImageUrl="~/bioimage/Grace Time.jpg"
                        OnClick="btngracetime_Click" Visible="False" />
                </td>
                <td>
                    <asp:Label ID="Lblgracetime" runat="server" CssClass="style245" Font-Bold="True"
                        Font-Size="Medium" ForeColor="#FF8A9E" Text="Label" Visible="False"></asp:Label>
                </td>
                <td class="style463">
                    <asp:ImageButton ID="btnontime" runat="server" Height="20px" ImageUrl="~/bioimage/On Time.jpg"
                        OnClick="btnontime_Click" Visible="False" />
                </td>
                <td>
                    <asp:Label ID="lblontime" runat="server" Text="Label" Font-Bold="True" Font-Size="Medium"
                        CssClass="style245" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="btnmorpresent" runat="server" ImageUrl="~/bioimage/Mor_present.jpg"
                        Visible="False" OnClick="btnmorpresent_Click" />
                </td>
                <td>
                    <asp:Label ID="lblmorpresent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="DarkGreen" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnevepresent" runat="server" ImageUrl="~/bioimage/Eve_present.jpg"
                        Visible="False" OnClick="btnevepresent_Click" />
                </td>
                <td>
                    <asp:Label ID="lblevepresent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="DarkGreen" Visible="False"></asp:Label>
                </td>
                <td class="style326">
                    <asp:ImageButton ID="btnmorabsent" runat="server" ImageUrl="~/bioimage/Mor_Absent.jpg"
                        Visible="False" OnClick="btnmorabsent_Click" />
                </td>
                <td>
                    <asp:Label ID="lblmorabsent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btneveabsent" runat="server" ImageUrl="~/bioimage/Eve_Absent.jpg"
                        Visible="False" OnClick="btneveabsent_Click" />
                </td>
                <td class="style462">
                    <asp:Label ID="lbleveabsent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
                </td>
                <td class="style463">
                </td>
                <td>
                </td>
                <td class="style463">
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" Height="10px" Visible="False" Width="1000px"
            BackImageUrl="~/JPG_Biometric/Band.jpg">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<br />
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Height="16px" Visible="False">
            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        </asp:Panel>
        <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Visible="False"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="     Records Per Page"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                        Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="24px" Width="58px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                        AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxother"
                        FilterType="Numbers" />
                </td>
                <td class="style2">
                    <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                        Width="95px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style273">
                    <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                        OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="17px" Width="34px"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="TextBoxpage"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblne" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="NE-Not Entered"
                        Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <center>
            <FarPoint:FpSpread ID="fpbiomatric" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="295px" Width="1000px">
                <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton" ButtonFaceColor="Control"
                    ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </body>
    </html>
</asp:Content>
